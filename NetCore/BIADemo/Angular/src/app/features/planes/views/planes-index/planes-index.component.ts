import { Component, HostBinding, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { getAllPlanes, getPlanesTotalCount, getPlaneLoadingGetAll } from '../../store/plane.state';
import { remove, loadAllByPost, load, openDialogEdit, openDialogNew } from '../../store/planes-actions';
import { Observable } from 'rxjs';
import { ConfirmationService, Confirmation, LazyLoadEvent } from 'primeng/api';
import { Plane } from '../../model/plane';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import {
  BiaListConfig,
  PrimeTableColumn,
  TypeTS,
  PrimeNGFiltering
} from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { AppState } from 'src/app/shared/bia-shared/store/state';
import { BiaDialogService } from 'src/app/core/bia-core/services/bia-dialog.service';
import { DEFAULT_PAGE_SIZE } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { PlaneDas } from '../../services/plane-das.service';
import * as FileSaver from 'file-saver';
import { TranslateService } from '@ngx-translate/core';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { Permission } from 'src/app/shared/permission';

@Component({
  selector: 'app-planes-index',
  templateUrl: './planes-index.component.html',
  styleUrls: ['./planes-index.component.scss'],
  providers: [ConfirmationService]
})
export class PlanesIndexComponent implements OnInit {
  @HostBinding('class.bia-flex') flex = true;
  @ViewChild(BiaTableComponent, { static: false }) planeListComponent: BiaTableComponent;
  showColSearch = false;
  globalSearchValue = '';
  defaultPageSize = DEFAULT_PAGE_SIZE;
  pageSize = this.defaultPageSize;
  totalRecords: number;
  planes$: Observable<Plane[]>;
  totalCount$: Observable<number>;
  loading$: Observable<boolean>;
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
    private authService: AuthService,
    private planeDas: PlaneDas,
    private translateService: TranslateService,
    private biaTranslationService: BiaTranslationService
  ) {}

  ngOnInit() {
    this.initTableConfiguration();
    this.setPermissions();
    this.planes$ = this.store.select(getAllPlanes).pipe();
    this.totalCount$ = this.store.select(getPlanesTotalCount).pipe();
    this.loading$ = this.store.select(getPlaneLoadingGetAll).pipe();
  }

  onCreate() {
    this.store.dispatch(openDialogNew());
  }

  onEdit(planeId: number) {
    this.store.dispatch(load({ id: planeId }));
    this.store.dispatch(openDialogEdit());
  }

  onRemove(planeId: number) {
    const confirmation: Confirmation = {
      ...this.biaDialogService.getDeleteConfirmation(),
      accept: () => {
        this.store.dispatch(remove({ id: planeId }));
      }
    };
    this.confirmationService.confirm(confirmation);
  }

  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
    this.store.dispatch(loadAllByPost({ event: lazyLoadEvent }));
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
    const columns: { [key: string]: string } = {};
    this.columns.map((x) => (columns[x.split('.')[1]] = this.translateService.instant(x)));
    const customEvent: any = { columns: columns, ...this.planeListComponent.getLastLazyLoadEvent() };
    this.planeDas.getFile(customEvent).subscribe((data) => {
      FileSaver.saveAs(data, this.translateService.instant('app.planes') + '.csv');
    });
  }

  private setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Plane_Update);
    this.canDelete = this.authService.hasPermission(Permission.Plane_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Plane_Create);
  }

  private initTableConfiguration() {
    this.biaTranslationService.culture$.subscribe((dateFormat) => {
      this.tableConfiguration = {
        customButtons: [],
        columns: [
          new PrimeTableColumn('msn', 'plane.msn'),
          Object.assign(new PrimeTableColumn('isActive', 'plane.isActive'), {
            isSearchable: false,
            isSortable: false,
            type: TypeTS.Boolean
          }),
          Object.assign(new PrimeTableColumn('firstFlightDate', 'plane.firstFlightDate'), {
            type: TypeTS.Date,
            formatDate: dateFormat.dateFormat
          }),
          Object.assign(new PrimeTableColumn('firstFlightTime', 'plane.firstFlightTime'), {
            type: TypeTS.Date,
            formatDate: dateFormat.dateTimeFormat
          }),
          Object.assign(new PrimeTableColumn('lastFlightDate', 'plane.lastFlightDate'), {
            type: TypeTS.Date,
            formatDate: dateFormat.timeFormat
          }),
          Object.assign(new PrimeTableColumn('capacity', 'plane.capacity'), {
            filterMode: PrimeNGFiltering.Equals
          })
        ]
      };

      this.columns = this.tableConfiguration.columns.map((col) => col.header);
      this.displayedColumns = this.columns;
    });
  }
}
