import { Component, HostBinding, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { getAllSites, getSitesTotalCount, getSiteLoadingGetAll } from '../../store/site.state';
import { remove, loadAllByPost, load, openDialogNew, openDialogEdit } from '../../store/sites-actions';
import { Observable } from 'rxjs';
import { ConfirmationService, Confirmation, LazyLoadEvent } from 'primeng/api';
import { map } from 'rxjs/operators';
import { SiteInfo } from '../../model/site/site-info';
import { SiteMember } from '../../model/site/site-member';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import {
  BiaListConfig,
  ROUTER_LINK_ID,
  PrimeTableColumn
} from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { AppState } from 'src/app/shared/bia-shared/store/state';
import { BiaDialogService } from 'src/app/core/bia-core/services/bia-dialog.service';
import { User } from 'src/app/domains/user/model/user';
import { loadAllByFilter } from 'src/app/domains/user/store/users-actions';
import { getAllUsers } from 'src/app/domains/user/store/user.state';
import { DEFAULT_PAGE_SIZE } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Permission } from 'src/app/shared/permission';

interface SiteListVM {
  id: number;
  title: string;
  siteAdmin: string;
}

@Component({
  selector: 'app-sites-index',
  templateUrl: './sites-index.component.html',
  styleUrls: ['./sites-index.component.scss'],
  providers: [ConfirmationService]
})
export class SitesIndexComponent implements OnInit {
  @HostBinding('class.bia-flex') flex = true;
  @ViewChild(BiaTableComponent, { static: false }) siteListComponent: BiaTableComponent;
  showColSearch = false;
  globalSearchValue = '';
  defaultPageSize = DEFAULT_PAGE_SIZE;
  pageSize = this.defaultPageSize;
  totalRecords: number;
  users$: Observable<User[]>;
  sites$: Observable<SiteListVM[]>;
  totalCount$: Observable<number>;
  loading$: Observable<boolean>;
  showFilter = false;
  haveFilter = false;
  private lastLazyLoadEvent: LazyLoadEvent;
  private userId = 0;
  canEdit = false;
  canDelete = false;
  canAdd = false;

  tableConfiguration: BiaListConfig;
  columns: string[];
  displayedColumns: string[] = this.columns;

  constructor(
    private store: Store<AppState>,
    private confirmationService: ConfirmationService,
    private biaDialogService: BiaDialogService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.initTableConfiguration();
    this.setPermissions();
    this.users$ = this.store
      .select(getAllUsers)
      .pipe()
      .pipe(
        map((users: User[]) => {
          users.forEach((user: User) => {
            user.displayName = `${user.firstName} ${user.lastName} (${user.login})`;
          });
          return users;
        })
      );
    this.sites$ = this.store
      .select(getAllSites)
      .pipe()
      .pipe(map((siteInfos) => siteInfos.map((siteInfo) => this.toSiteListVM(siteInfo))));
    this.totalCount$ = this.store.select(getSitesTotalCount).pipe();
    this.loading$ = this.store.select(getSiteLoadingGetAll).pipe();
  }

  onCreate() {
    this.store.dispatch(openDialogNew());
  }

  onEdit(siteId: number) {
    this.store.dispatch(load({ id: siteId }));
    this.store.dispatch(openDialogEdit());
  }

  onRemove(siteId: number) {
    const confirmation: Confirmation = {
      ...this.biaDialogService.getDeleteConfirmation(),
      accept: () => {
        this.store.dispatch(remove({ id: siteId }));
      }
    };
    this.confirmationService.confirm(confirmation);
  }

  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
    this.lastLazyLoadEvent = lazyLoadEvent;
    const customEvent: any = { userId: this.userId, ...lazyLoadEvent };
    this.store.dispatch(loadAllByPost({ event: customEvent }));
  }

  onSearchUsers(value: string) {
    this.store.dispatch(loadAllByFilter({ filter: value }));
  }

  onFilter(userId: number) {
    this.userId = userId;
    this.haveFilter = this.userId > 0;
    this.onLoadLazy(this.lastLazyLoadEvent);
  }

  searchGlobalChanged(value: string) {
    this.globalSearchValue = value;
  }

  displayedColumnsChanged(values: string[]) {
    this.displayedColumns = values;
  }

  onCloseFilter() {
    this.showFilter = false;
  }

  onOpenFilter() {
    this.showFilter = true;
  }

  onToggleSearch() {
    this.showColSearch = !this.showColSearch;
  }

  toSiteListVM(site: SiteInfo): SiteListVM {
    return {
      id: site.id,
      title: site.title,
      siteAdmin: site.siteAdmins
        ? site.siteAdmins
            .map((siteMember: SiteMember) => `${siteMember.userLastName} ${siteMember.userFirstName}`)
            .join(', ')
        : ''
    };
  }

  private setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Site_Update);
    this.canDelete = this.authService.hasPermission(Permission.Site_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Site_Create);
  }

  private initTableConfiguration() {
    this.tableConfiguration = {
      customButtons: [
        {
          classValue: 'ui-icon-assignment-ind  bia-pointer',
          routerLinkValue: ['/sites', ROUTER_LINK_ID, 'members'],
          pTooltipValue: 'member.manage',
          permission: Permission.Member_List_Access
        }
      ],
      columns: [
        new PrimeTableColumn('title', 'site.title'),
        Object.assign(new PrimeTableColumn('siteAdmin', 'site.admins'), {
          isSortable: false,
          isSearchable: false
        })
      ]
    };
    this.columns = this.tableConfiguration.columns.map((col) => col.header);
    this.displayedColumns = this.columns;
  }
}
