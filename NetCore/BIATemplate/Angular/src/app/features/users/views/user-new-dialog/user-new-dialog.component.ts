import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Store } from '@ngrx/store';
import { save } from '../../store/users-actions';
import { User } from 'src/app/domains/user/model/user';
import { AppState } from 'src/app/shared/bia-shared/store/state';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { loadAllADByFilter } from 'src/app/domains/user/store/users-actions';
import { getAllUsers } from 'src/app/domains/user/store/user.state';

@Component({
  selector: 'app-user-new-dialog',
  templateUrl: './user-new-dialog.component.html',
  styleUrls: ['./user-new-dialog.component.scss']
})
export class UserNewDialogComponent implements OnInit {
  _display = false;

  @Input()
  set display(val: boolean) {
    this._display = val;
    this.displayChange.emit(this._display);
  }

  @Output() displayChange = new EventEmitter<boolean>();
  users$: Observable<User[]>;

  constructor(private store: Store<AppState>) {}

  ngOnInit() {
    this.users$ = this.store
      .select(getAllUsers)
      .pipe()
      .pipe(
        map((users: User[]) => {
          users.forEach((user: User) => {
            user.displayName = `${user.firstName} ${user.lastName} (${user.login})`;
          });
          return users;
        })
      );
  }

  onSubmitted(userToCreates: User[]) {
    this.store.dispatch(save({ users: userToCreates }));
    this.close();
  }

  onCancelled() {
    this.close();
  }

  public close() {
    this.display = false;
  }

  onSearchUsers(value: string) {
    this.store.dispatch(loadAllADByFilter({ filter: value }));
  }
}
