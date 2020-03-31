import { Component, Input, ViewChild, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { Table } from 'primeng/table';
import { LazyLoadEvent } from 'primeng/api';
import { ROUTER_LINK_ID, BiaListConfig, TypeTS, PrimeTableColumn } from './bia-table-config';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';

@Component({
  selector: 'bia-table',
  templateUrl: './bia-table.component.html',
  styleUrls: ['./bia-table.component.scss']
})
export class BiaTableComponent implements OnChanges {
  @Input() pageSize: number;
  @Input() totalRecord: number;
  @Input() elements: any[];
  @Input() columnToDisplays: string[];
  @Input() sortFieldValue = '';
  @Input() configuration: BiaListConfig;
  @Input() showColSearch = false;
  @Input() globalSearchValue = '';
  @Input() canEdit = true;
  @Input() canDelete = true;
  @Input() loading = false;
  @Input() tableId: string = '';
  @Input() viewPreference: string = '';

  @Output() edit = new EventEmitter<any>();
  @Output() remove = new EventEmitter<any>();
  @Output() filter = new EventEmitter<number>();
  @Output() loadLazy = new EventEmitter<LazyLoadEvent>();

  @ViewChild('dt', { static: false }) table: Table;

  displayedColumns: PrimeTableColumn[];

  private lastLazyLoadEvent: LazyLoadEvent;

  constructor(public authService: AuthService) {}

  ngOnChanges(changes: SimpleChanges) {
    this.onConfigurationChange(changes);
    this.onColumnChange(changes);
    this.onPageSizeChange(changes);
    this.onSearchGlobalChanged(changes);
    this.onViewPreferenceChange(changes);
  }

  private onConfigurationChange(changes: SimpleChanges) {
    if (this.configuration && changes.configuration) {
      if (this.columnToDisplays) {
        this.displayedColumns = this.configuration.columns.filter(
          (col) => this.columnToDisplays.indexOf(col.header) > -1
        );
      } else {
        this.displayedColumns = this.configuration.columns.slice();
      }
      if (changes.configuration.isFirstChange() && this.sortFieldValue.length < 1) {
        this.sortFieldValue = this.displayedColumns[0].field;
      }
    }
  }

  private onColumnChange(changes: SimpleChanges) {
    if (this.columnToDisplays && changes.columnToDisplays) {
      this.displayedColumns = this.configuration.columns.filter(
        (col) => this.columnToDisplays.indexOf(col.header) > -1
      );
    }
  }

  private onPageSizeChange(changes: SimpleChanges) {
    if (changes.pageSize && this.lastLazyLoadEvent) {
      this.lastLazyLoadEvent.first = 0;
      this.lastLazyLoadEvent.rows = Number(this.pageSize);
      this.onLoadLazy(this.lastLazyLoadEvent);
    }
  }

  private onSearchGlobalChanged(changes: SimpleChanges) {
    if (changes.globalSearchValue) {
      this.searchGlobalChanged(this.globalSearchValue);
    }
  }

  private onViewPreferenceChange(changes: SimpleChanges) {
    if (this.table) {
      if(changes.viewPreference){
        if(changes.viewPreference.currentValue !== '')
        {
          localStorage.setItem(this.tableId, changes.viewPreference.currentValue);
          this.table.restoreState();
        }
        else{
          localStorage.removeItem(this.tableId);
          this.table.reset();
        }
        const lazyLoadEvent: LazyLoadEvent = this.table.createLazyLoadMetadata();
        this.showColSearch = false;
        if(this.table.hasFilter()) {
          for (const key in this.table.filters) {
            if (!key.startsWith('global|')) {
              this.showColSearch = true;
              break;
            }
          }
        }
        
        this.table.onLazyLoad.emit(lazyLoadEvent);
      }
    }
  }

  editElement(plane: any) {
    this.edit.emit(plane);
  }

  deleteElement(plane: any) {
    this.remove.emit(plane);
  }

  searchGlobalChanged(value: string) {
    if (this.table) {
      if (this.table.lazy === true) {
        this.configuration.columns.forEach((col) => {
          if (col.isSearchable === true && col.type !== TypeTS.Boolean) {
            this.table.filter(value, 'global|' + col.field, col.filterMode);
          }
        });
      } else {
        this.table.filterGlobal(value, 'contains');
      }
    }
  }

  onFilter() {
    if (this.table) {
      this.filter.emit(this.table.totalRecords);
    }
  }

  onLoadLazy(event: LazyLoadEvent) {
    this.lastLazyLoadEvent = event;
    this.loadLazy.emit(event);
  }

  getRouterLink(items: any[], id: number): any[] {
    const cloneItems = [...items];
    const index = cloneItems.indexOf(ROUTER_LINK_ID);

    if (index !== -1) {
      cloneItems[index] = id;
    }
    return cloneItems;
  }

  public hasPermission(permission: string): boolean {
    return this.authService.hasPermission(permission);
  }

  showActionCol() {
    const haveCustomActionRight =
      this.configuration.customButtons.length < 1 ||
      this.configuration.customButtons
        .map((customButton) => this.authService.hasPermission(customButton.permission))
        .some((x) => x === true);

    return this.canEdit === true || this.canDelete === true || haveCustomActionRight === true;
  }

  getLastLazyLoadEvent(): LazyLoadEvent {
    return { ...this.lastLazyLoadEvent };
  }
}
