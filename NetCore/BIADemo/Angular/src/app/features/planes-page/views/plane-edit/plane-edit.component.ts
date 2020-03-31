import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { update, load } from '../../store/planes-actions';
import { Observable, Subscription } from 'rxjs';
import { getCurrentPlane, getPlaneLoadingGet } from '../../store/plane.state';
import { Plane } from '../../model/plane';
import { AppState } from 'src/app/shared/bia-shared/store/state';
import { Location } from '@angular/common';
import { ActivatedRoute, Params } from '@angular/router';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-plane-edit',
  templateUrl: './plane-edit.component.html',
  styleUrls: ['./plane-edit.component.scss']
})
export class PlaneEditComponent implements OnInit, OnDestroy {
  @Output() displayChange = new EventEmitter<boolean>();
  loading$: Observable<boolean>;
  plane$: Observable<Plane>;
  private sub = new Subscription();
  private planeId: number;

  constructor(private store: Store<AppState>, private location: Location, private route: ActivatedRoute) {}

  ngOnInit() {
    this.loading$ = this.store.select(getPlaneLoadingGet).pipe();
    this.plane$ = this.store.select(getCurrentPlane).pipe(filter((plane) => plane.id > 0));
    this.sub.add(
      this.route.params.subscribe((params: Params) => {
        this.planeId = +params['id'];
        this.store.dispatch(load({ id: this.planeId }));
      })
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(planeToUpdate: Plane) {
    this.store.dispatch(update({ plane: planeToUpdate }));
    this.location.back();
  }

  onCancelled() {
    this.location.back();
  }
}
