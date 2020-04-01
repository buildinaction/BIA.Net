import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'bia-table-header',
  templateUrl: './bia-table-header.component.html',
  styleUrls: ['./bia-table-header.component.scss']
})
export class BiaTableHeaderComponent implements OnInit {
  @Input() haveFilter = false;
  @Input() showFilter = false;
  @Input() showBtnFilter = false;
  @Input() canAdd = true;
  @Input() canExportCSV = false;
  @Input() headerTitle: string;
  @Input() btnBackRouterLink: any[];
  @Output() create = new EventEmitter();
  @Output() openFilter = new EventEmitter();
  @Output() exportCSV = new EventEmitter();

  constructor() {}

  ngOnInit() {}

  onCreate() {
    this.create.next();
  }

  toggleFilter() {
    this.showFilter = !this.showFilter;
    if (this.showFilter === true) {
      this.openFilter.emit();
    }
  }
}
