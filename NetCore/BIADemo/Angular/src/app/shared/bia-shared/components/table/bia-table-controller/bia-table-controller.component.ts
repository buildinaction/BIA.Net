import { animate, style, transition, trigger } from '@angular/animations';
import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormControl } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { SelectItem, SelectItemGroup } from 'primeng/primeng';
import { View } from 'src/app/domains/view/model/view';

@Component({
  selector: 'bia-table-controller',
  templateUrl: './bia-table-controller.component.html',
  styleUrls: ['./bia-table-controller.component.scss'],
  animations: [
    trigger('options', [
      transition(':enter', [style({ height: 0 }), animate('200ms ease-out', style({ height: '*' }))]),
      transition(':leave', [style({ height: '*' }), animate('200ms ease-out', style({ height: 0 }))])
    ])
  ]
})
export class BiaTableControllerComponent implements OnChanges, OnInit, OnDestroy {
  @Input() pageSizeOptions: number[] = [10, 25, 50, 100];
  @Input() defaultPageSize: number;
  @Input() length: number;
  @Input() columns?: string[];
  @Input() displayedColumns?: string[];
  @Input() hiddenColumns?: string[];
  @Input() views: View[];
  @Input() globalSearch: string;

  @Output() displayedColumnsChange = new EventEmitter<string[]>();
  @Output() filter = new EventEmitter<string>();
  @Output() pageSizeChange = new EventEmitter<number>();
  @Output() toggleSearch = new EventEmitter();
  @Output() viewChange = new EventEmitter<string>();
  @Output() manageView = new EventEmitter();

  pageSize: number;
  pageSizes: SelectItem[];
  displayedPageSizeOptions: number[];
  resultMessageMapping = {
    '=0': 'bia.noResult',
    '=1': 'bia.result',
    other: 'bia.results'
  };
  listedColumns: SelectItem[];
  filterCtrl = new FormControl();
  globalFilter: string = '';
  selectedView: number;
  defaultView: number;
  groupedViews: SelectItemGroup[];
  translateKeys: string[] = ['bia.views.system', 'bia.views.default', 'bia.views.site','bia.views.user'];
  transalations: any;

  private sub = new Subscription();

  constructor(
    public translateService: TranslateService
    ) {}

  ngOnInit() {
    this.updateDisplayedPageSizeOptions();
    this.sub.add(this.filterCtrl.valueChanges.subscribe((filterValue) => {
      if(this.globalFilter !== filterValue.trim().toLowerCase()) {
        this.filter.emit(filterValue.trim().toLowerCase());
      }
    }));

    this.sub.add(this.translateService.stream(this.translateKeys).subscribe(translations => {
      this.transalations = translations;
      this.updateGroupedViews();
    }));
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.columns && changes.columns) {
      let cols: string[];
      if (this.hiddenColumns && changes.hiddenColumns) {
        const hiddenColumns = this.hiddenColumns;
        cols = this.columns.filter((el) => hiddenColumns.indexOf(el) < 0);
      } else {
        cols = this.columns;
      }
      if (changes.columns.isFirstChange()) {
        this.sub.add(
          this.translateService.stream(cols).subscribe((results) => {
            this.listedColumns = new Array<SelectItem>();
            cols.forEach((col) => {
              this.listedColumns.push({ label: results[col.toString()], value: col.toString() });
            });
          })
        );
      }
    }
    if (changes.pageSizeOptions && !changes.pageSizeOptions.isFirstChange()) {
      this.updateDisplayedPageSizeOptions();
    }

    if(changes.defaultPageSize && !changes.defaultPageSize.isFirstChange() && Number(this.pageSize) !== this.defaultPageSize) {
      this.pageSize = this.defaultPageSize;
    }

    if(changes.globalSearch && !changes.globalSearch.isFirstChange()) {
      this.globalFilter = this.globalSearch;
    }

    if(changes.views){
      this.updateGroupedViews();
    }
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onPageSizeChange() {
    this.pageSizeChange.emit(Number(this.pageSize));
  }

  onChangeSelectColumn() {
    this.displayedColumnsChange.emit(this.displayedColumns);
  }

  onToggleSearch() {
    this.toggleSearch.emit();
  }

  onViewChange(event: any) {
    this.selectedView = event.value;
    this.updateFilterValues();
  }

  toggleColumn(column: string, selectedColIndex: number) {
    if (this.displayedColumns !== undefined) {
      const displayColIndex = this.displayedColumns.findIndex((col) => col === column);
      const newDisplayedColumns = this.displayedColumns.slice();
      if (displayColIndex >= 0) {
        newDisplayedColumns.splice(displayColIndex, 1);
      } else {
        let pos = selectedColIndex;
        for (let i = 0; i < this.displayedColumns.length; i++) {
          if (this.columns !== undefined) {
            const colIndex = this.columns.indexOf(this.displayedColumns[i]);
            if (colIndex > selectedColIndex) {
              pos = i;
              break;
            }
          }
        }
        newDisplayedColumns.splice(pos, 0, column);
      }
      this.displayedColumns = newDisplayedColumns;
      this.displayedColumnsChange.emit(this.displayedColumns);
    }
  }

  onManageView() {
    this.manageView.emit();
  }

  private updateDisplayedPageSizeOptions() {
    this.displayedPageSizeOptions = this.pageSizeOptions.sort((a, b) => a - b);

    this.pageSizes = new Array<SelectItem>();
    this.displayedPageSizeOptions.forEach((displayedPageSizeOption) => {
      this.pageSizes.push({ label: displayedPageSizeOption.toString(), value: displayedPageSizeOption.toString() });
    });
  }

  private updateGroupedViews(){
    if(!this.views || ! this.transalations)
    {
      return;
    }

    this.groupedViews = [
      {
        label: this.transalations['bia.views.system'],
        items: [
          {label: this.transalations['bia.views.default'], value: 0}
        ]
      }
    ];

    let defaultView = 0;
    const siteViews = this.views.filter(v => v.viewType == 1);
    const userViews = this.views.filter(v => v.viewType == 2);
    if(siteViews.length > 0) {
      this.groupedViews.push({
        label: this.transalations['bia.views.site'],
        items: siteViews.map(v => { return { label: v.name, value: v.id }})
      });

      const siteDefault = siteViews.filter(v => v.isSiteDefault)[0];
      if(siteDefault != null && siteDefault != undefined)
      {
        defaultView = siteDefault.id;
      }
    }

    if(userViews.length > 0) {
      this.groupedViews.push({
        label: this.transalations['bia.views.user'],
        items: userViews.map(v => { return { label: v.name, value: v.id }})
      });
    }

    const userDefault = this.views.filter(v => v.isUserDefault)[0];
    if(userDefault != null && userDefault != undefined)
    {
      defaultView = userDefault.id;
    }

    this.selectedView = defaultView;
    this.defaultView = defaultView;

    this.updateFilterValues();
  }

  private updateFilterValues() {
    if(this.selectedView != 0)
    {
      const view = this.views.find(f => f.id == this.selectedView);
      if(view)
      {
        this.viewChange.emit(view.preference);
      }
      else {
        this.viewChange.emit('');
      }
    }
    else{
      this.viewChange.emit('');
    }
  }
}