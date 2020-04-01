import {
  Component,
  OnInit,
  Output,
  EventEmitter,
  ViewChild,
  ViewContainerRef,
  TemplateRef,
  Input
} from '@angular/core';
import { User } from 'src/app/domains/user/model/user';

@Component({
  selector: 'app-site-filter',
  templateUrl: './site-filter.component.html',
  styleUrls: ['./site-filter.component.scss']
})
export class SiteFilterComponent implements OnInit {
  public static queryParamUser = 'user';

  @ViewChild('template', { static: true }) template: TemplateRef<HTMLElement>;
  @Input() fxFlexValue: string;
  @Input() users: User[];
  @Input() hidden = false;
  @Output() close = new EventEmitter();
  @Output() searchUsers = new EventEmitter<string>();
  @Output() filter = new EventEmitter<number>();

  userSelected: User;

  constructor(private viewContainerRef: ViewContainerRef) {}

  ngOnInit() {
    // The goal here is that the html content of the component is not contained in its tag "app-user-filter".
    this.viewContainerRef.createEmbeddedView(this.template);
  }

  onClose() {
    this.close.emit();
  }

  onReset() {
    this.userSelected = <User>{};
    this.onFilter();
  }

  onFilter() {
    this.filter.emit(this.userSelected ? this.userSelected.id : 0);
  }

  onSearchUsers(event: any) {
    this.searchUsers.emit(event.query);
  }
}
