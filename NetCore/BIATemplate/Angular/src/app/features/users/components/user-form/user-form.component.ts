import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from 'src/app/domains/user/model/user';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserFormComponent implements OnInit, OnChanges {
  @Output() searchUsers = new EventEmitter<string>();
  @Output() save = new EventEmitter<User[]>();
  @Output() cancel = new EventEmitter();
  @Input() users: User[];

  selectedUsers: User[];
  form: FormGroup;

  constructor(public formBuilder: FormBuilder) {
    this.form = this.formBuilder.group({
      selectedUsers: [this.selectedUsers, Validators.required]
    });
  }

  ngOnInit() {}

  ngOnChanges(changes: SimpleChanges) {
    if (this.users && changes.users) {
      this.users = this.users.sort((a, b) => {
        return a.firstName.localeCompare(b.firstName);
      });
    }
  }

  onCancel() {
    this.form.reset();
    this.cancel.next();
  }

  onSubmit() {
    if (this.form.valid) {
      this.save.emit(this.form.value.selectedUsers);
      this.form.reset();
    }
  }

  onSearchUsers(event: any) {
    this.searchUsers.emit(event.query);
  }
}
