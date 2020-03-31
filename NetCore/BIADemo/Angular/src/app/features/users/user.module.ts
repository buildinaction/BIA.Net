import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { UsersEffects  } from './store/users-effects';
import { reducers  } from './store/user.state';
import { UserFormComponent } from './components/user-form/user-form.component';
import { UsersIndexComponent } from './views/users-index/users-index.component';
import { UserNewDialogComponent } from './views/user-new-dialog/user-new-dialog.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { reducers as usersReducers } from 'src/app/domains/user/store/user.state';
import { UsersEffects as DomainUsersEffects } from 'src/app/domains/user/store/users-effects';
import { UserTableHeaderComponent } from './components/user-table-header/user-table-header.component';
import { PermissionGuard } from 'src/app/shared/bia-shared/guards/permission.guard';
import { Permission } from 'src/app/shared/permission';
import { reducers as viewsReducers } from 'src/app/domains/view/store/view.state';
import { ViewsEffects as DomainViewsEffects } from 'src/app/domains/view/store/views-effects';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.User_List_Access
    },
    component: UsersIndexComponent,
    canActivate: [PermissionGuard]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  declarations: [UserFormComponent, UserNewDialogComponent, UsersIndexComponent, UserTableHeaderComponent],
  entryComponents: [UserNewDialogComponent],
  imports: [
    SharedModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature('users', reducers),
    StoreModule.forFeature('domain-users', usersReducers),
    StoreModule.forFeature('domain-views', viewsReducers),
    EffectsModule.forFeature([UsersEffects]),
    EffectsModule.forFeature([DomainUsersEffects]),
    EffectsModule.forFeature([DomainViewsEffects])
  ]
})
export class UserModule {}