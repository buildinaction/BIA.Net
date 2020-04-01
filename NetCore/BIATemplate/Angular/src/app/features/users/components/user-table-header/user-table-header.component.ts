import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'user-table-header',
  templateUrl: './user-table-header.component.html',
  styleUrls: ['./user-table-header.component.scss']
})
export class UserTableHeaderComponent implements OnInit {
  @Input() haveFilter = false;
  @Input() showFilter = false;
  @Input() showBtnFilter = false;
  @Input() canSync = false;
  @Input() canAdd = false;
  @Input() canExportCSV = false;
  @Input() headerTitle: string;
  @Input() btnBackRouterLink: any[];
  @Output() create = new EventEmitter();
  @Output() openFilter = new EventEmitter();
  @Output() exportCSV = new EventEmitter();
  @Output() synchronize = new EventEmitter();

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




