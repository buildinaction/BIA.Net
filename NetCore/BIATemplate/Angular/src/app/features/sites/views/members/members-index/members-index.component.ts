import { Component, HostBinding, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { ConfirmationService, Confirmation, LazyLoadEvent } from 'primeng/api';
import { map, switchMap } from 'rxjs/operators';
import * as fromSites from '../../../store/site.state';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Member } from '../../../model/user/member';
import { Site } from '../../../model/site/site';
import { getAllMembers, getMembersTotalCount, getMemberLoadingGetAll } from '../../../store/members/member.state';
import { remove, loadAllByPost, load, openDialogNew, openDialogEdit } from '../../../store/members/members-actions';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { BiaListConfig, PrimeTableColumn } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { AppState } from 'src/app/shared/bia-shared/store/state';
import { BiaDialogService } from 'src/app/core/bia-core/services/bia-dialog.service';
import { Role } from 'src/app/domains/member-role/model/role';
import { getAllRoles } from 'src/app/domains/member-role/store/member-role.state';
import { DEFAULT_PAGE_SIZE } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Permission } from 'src/app/shared/permission';

interface MemberListVM {
  id: number;
  displayName: string;
  roles: string;
  member: Member;
}

@Component({
  selector: 'app-members-index',
  templateUrl: './members-index.component.html',
  styleUrls: ['./members-index.component.scss'],
  providers: [ConfirmationService]
})
export class MembersIndexComponent implements OnInit, OnDestroy {
  @HostBinding('class.bia-flex') flex = true;
  @ViewChild(BiaTableComponent, { static: false }) memberListComponent: BiaTableComponent;
  loading$: Observable<boolean>;
  showColSearch = false;
  globalSearchValue = '';
  defaultPageSize = DEFAULT_PAGE_SIZE;
  pageSize = this.defaultPageSize;
  totalRecords: number;
  roles: Role[];
  members$: Observable<MemberListVM[]>;
  totalCount$: Observable<number>;
  private sub = new Subscription();
  siteRoute = ['/sites'];
  currentSite: Site;
  headerTitle: string;
  canEdit = false;
  canDelete = false;
  canAdd = false;

  tableConfiguration: BiaListConfig;
  columns: string[];
  displayedColumns: string[] = this.columns;

  constructor(
    private route: ActivatedRoute,
    private store: Store<AppState>,
    private confirmationService: ConfirmationService,
    private biaDialogService: BiaDialogService,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.initTableConfiguration();
    this.setPermissions();

    // Roles
    this.sub.add(
      this.store
        .select(getAllRoles)
        .pipe()
        .subscribe((roles: Role[]) => (this.roles = roles))
    );

    // Members
    this.members$ = this.store
      .select(getAllMembers)
      .pipe()
      .pipe(map((members) => members.map((member) => this.toMemberListVM(member))));

    // Members Total Count
    this.totalCount$ = this.store.select(getMembersTotalCount).pipe();

    // Site
    this.sub.add(
      this.route.params
        .pipe(
          map((params: Params) => params['id']),
          switchMap((selectedSiteId) => this.store.select(fromSites.getSiteById(selectedSiteId)).pipe())
        )
        .subscribe((site) => {
          if (site) {
            this.currentSite = site;
          } else {
            this.router.navigate(this.siteRoute);
          }
        })
    );
    this.loading$ = this.store.select(getMemberLoadingGetAll).pipe();
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onCreate() {
    this.store.dispatch(openDialogNew());
  }

  onEdit(memberId: number) {
    this.store.dispatch(load({ id: memberId }));
    this.store.dispatch(openDialogEdit());
  }

  onRemove(memberId: number) {
    const confirmation: Confirmation = {
      ...this.biaDialogService.getDeleteConfirmation(),
      accept: () => {
        this.store.dispatch(remove({ id: memberId }));
      }
    };
    this.confirmationService.confirm(confirmation);
  }

  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
    if (this.currentSite && this.currentSite.id > 0) {
      const customEvent: any = { siteId: this.currentSite.id, ...lazyLoadEvent };
      this.store.dispatch(loadAllByPost({ event: customEvent }));
    }
  }

  searchGlobalChanged(value: string) {
    this.globalSearchValue = value;
  }

  displayedColumnsChanged(values: string[]) {
    this.displayedColumns = values;
  }

  onToggleSearch() {
    this.showColSearch = !this.showColSearch;
  }

  toMemberListVM(objMember: Member): MemberListVM {
    return {
      member: objMember,
      id: objMember.id,
      displayName: `${objMember.userLastName} ${objMember.userFirstName}`,
      roles: objMember.roles
        ? this.roles
            .filter((x: Role) => objMember.roles.some((y) => y.roleId === x.id))
            .map((role: Role) => role.label)
            .join(', ')
        : ''
    };
  }

  private setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Member_Update);
    this.canDelete = this.authService.hasPermission(Permission.Member_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Member_Save);
  }

  private initTableConfiguration() {
    this.tableConfiguration = {
      customButtons: [],
      columns: [
        Object.assign(new PrimeTableColumn('displayName', 'member.user'), {
          isSortable: false,
          isSearchable: false
        }),
        Object.assign(new PrimeTableColumn('roles', 'member.rolesForSite'), {
          isSortable: false,
          isSearchable: false
        })
      ]
    };

    this.columns = this.tableConfiguration.columns.map((col) => col.header);
    this.displayedColumns = this.columns;
  }
}
