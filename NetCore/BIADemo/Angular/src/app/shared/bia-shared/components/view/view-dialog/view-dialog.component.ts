import { Component, OnInit, Output, EventEmitter, OnDestroy, Input } from '@angular/core';
import { Subscription } from 'rxjs';
import { View } from 'src/app/domains/view/model/view';

@Component({
  selector: 'view-dialog',
  templateUrl: './view-dialog.component.html',
  styleUrls: ['./view-dialog.component.scss']
})
export class ViewDialogComponent implements OnInit, OnDestroy {
  @Input() display = false;
  @Input() userViews: View[];
  private sub = new Subscription();
  @Output() displayChange = new EventEmitter<boolean>();
  @Output() deleteView = new EventEmitter<number>();
  @Output() setDefaultUserView = new EventEmitter<{}>();

  constructor() {}

  ngOnInit() {
    
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onClose() {
    this.displayChange.emit(false);
  }

  onDeleteView(viewId: number){
    this.deleteView.emit(viewId);
  }

  onSetDefaultUserView(viewId: number, isDefault: boolean) {
    this.setDefaultUserView.emit({viewId, isDefault});
  }
}
