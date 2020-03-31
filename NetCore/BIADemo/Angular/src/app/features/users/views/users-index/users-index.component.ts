import { Component, HostBinding, OnInit, ViewChild } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { getAllUsers, getUsersTotalCount } from '../../store/user.state';
import { remove, loadAllByPost, synchronize } from '../../store/users-actions';
import { Observable } from 'rxjs';
import { ConfirmationService, Confirmation, LazyLoadEvent } from 'primeng/api';
import { User } from 'src/app/domains/user/model/user';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { BiaListConfig, PrimeTableColumn } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { AppState } from 'src/app/shared/bia-shared/store/state';
import { BiaDialogService } from 'src/app/core/bia-core/services/bia-dialog.service';
import { DEFAULT_PAGE_SIZE } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Permission } from 'src/app/shared/permission';
import { View } from 'src/app/domains/view/model/view';
import { getAllViews } from 'src/app/domains/view/store/view.state';
import { map } from 'rxjs/operators';
import { loadAll as loadAllViews, removeUserView, setDefaultUserView } from 'src/app/domains/view/store/views-actions';

@Component({
  selector: 'app-users-index',
  templateUrl: './users-index.component.html',
  styleUrls: ['./users-index.component.scss'],
  providers: [ConfirmationService]
})
export class UsersIndexComponent implements OnInit {
  @HostBinding('class.bia-flex') flex = true;
  @ViewChild(BiaTableComponent, { static: false }) userListComponent: BiaTableComponent;
  showColSearch = false;
  globalSearchValue = '';
  pageSize = DEFAULT_PAGE_SIZE;
  totalRecords: number;
  users$: Observable<User[]>;
  totalCount$: Observable<number>;
  displayEditUserDialog = false;
  displayNewUserDialog = false;
  canSync = false;
  canDelete = false;
  canAdd = false;
  views$: Observable<View[]>;
  viewPreference: string = '';
  sortField: string = '';

  tableConfiguration: BiaListConfig = {
    customButtons: [],
    columns: [
      new PrimeTableColumn('lastName', 'user.lastName'),
      new PrimeTableColumn('firstName', 'user.firstName'),
      new PrimeTableColumn('login', 'user.login')
    ]
  };

  columns = this.tableConfiguration.columns.map((col) => col.header);
  displayedColumns: string[] = this.columns;
  lastLazyLoadEvent: LazyLoadEvent;
  displayViewDialog = false;
  private tableId: string = 'usersGrid';

  constructor(
    private store: Store<AppState>,
    private confirmationService: ConfirmationService,
    private biaDialogService: BiaDialogService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.setPermissions();
    this.users$ = this.store.select(getAllUsers).pipe();
    this.totalCount$ = this.store.select(getUsersTotalCount).pipe();
    this.views$ = this.store.pipe(select(getAllViews)).pipe(map((views) => views.filter((view) => view.tableId == this.tableId)));
    this.store.dispatch(loadAllViews());
  }

  onCreate() {
    this.displayNewUserDialog = true;
  }

  onRemove(userId: number) {
    const confirmation: Confirmation = {
      ...this.biaDialogService.getDeleteConfirmation(),
      accept: () => {
        this.store.dispatch(remove({ id: userId }));
      }
    };
    this.confirmationService.confirm(confirmation);
  }

  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
    if(JSON.stringify(this.lastLazyLoadEvent) === JSON.stringify(lazyLoadEvent)){
      this.store.dispatch(loadAllByPost({ event: lazyLoadEvent }));
      return;
    }

    this.lastLazyLoadEvent = lazyLoadEvent;
    if(lazyLoadEvent.rows && this.pageSize != lazyLoadEvent.rows){
      this.pageSize = lazyLoadEvent.rows;
    }

    if(lazyLoadEvent.sortField && this.sortField != lazyLoadEvent.sortField){
      this.sortField = lazyLoadEvent.sortField;
    }

    let globalSearch;
    for (const key in lazyLoadEvent.filters) {
      if (key.startsWith('global|')) {
        globalSearch = lazyLoadEvent.filters[key].value;
        break;
      }
    }
    if(globalSearch){
      this.globalSearchValue = globalSearch;
    }
    else {
      this.globalSearchValue = '';
      this.store.dispatch(loadAllByPost({ event: lazyLoadEvent }));
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

  onExportCSV() {
  }

  onSynchronize() {
    this.store.dispatch(synchronize());
  }

  onViewChange(viewPreference: string) {
    this.viewPreference = viewPreference;
  }

  onManageView() {
    this.store.dispatch(loadAllViews());
    this.displayViewDialog = true;
  }

  onDisplayViewChange(displayView: false) {
    this.store.dispatch(loadAllViews());
    this.displayViewDialog = displayView;
  }

  onDeleteView(viewId: number) {
    this.store.dispatch(removeUserView({id: viewId}));
  }

  onSetDefaultUserView(event: {viewId: number, isDefault: boolean}) {
    this.store.dispatch(setDefaultUserView({id: event.viewId, isDefault: event.isDefault, tableId: this.tableId}));
  }

  private setPermissions() {
    this.canSync = this.authService.hasPermission(Permission.User_Sync);
    this.canDelete = this.authService.hasPermission(Permission.User_Delete);
    this.canAdd = this.authService.hasPermission(Permission.User_Add);
  }
}