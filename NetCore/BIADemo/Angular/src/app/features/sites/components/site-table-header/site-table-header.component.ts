import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { BiaTableHeaderComponent } from 'src/app/shared/bia-shared/components/table/bia-table-header/bia-table-header.component';

@Component({
  selector: 'app-site-table-header',
  templateUrl: './site-table-header.component.html',
  styleUrls: [
    '../../../../shared/bia-shared/components/table/bia-table-header/bia-table-header.component.scss',
    './site-table-header.component.scss'
  ],
  providers: [ConfirmationService]
})
export class SiteTableHeaderComponent extends BiaTableHeaderComponent {
  @Input() canManageMembers = false;
  @Output() manageMembers = new EventEmitter();
}
