import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { MembersEffects } from './store/members/members-effects';
import { SitesEffects } from './store/sites-effects';
import { reducers } from './store/site.state';
import { SiteFormComponent } from './components/site-form/site-form.component';
import { SiteEditDialogComponent } from './views/site-edit-dialog/site-edit-dialog.component';
import { SiteNewDialogComponent } from './views/site-new-dialog/site-new-dialog.component';
import { SitesIndexComponent } from './views/sites-index/sites-index.component';
import { SiteFilterComponent } from './components/site-filter/site-filter.component';
import { reducers as memberReducers } from './store/members/member.state';
import { MemberEditDialogComponent } from './views/members/member-edit-dialog/member-edit-dialog.component';
import { MemberNewDialogComponent } from './views/members/member-new-dialog/member-new-dialog.component';
import { MembersIndexComponent } from './views/members/members-index/members-index.component';
import { MemberEditFormComponent } from './components/members/member-edit-form/member-edit-form.component';
import { MemberNewFormComponent } from './components/members/member-new-form/member-new-form.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { UserModule } from 'src/app/domains/user/user.module';
import { MemberRoleModule } from 'src/app/domains/member-role/member-role.module';
import { PermissionGuard } from 'src/app/shared/bia-shared/guards/permission.guard';
import { Permission } from 'src/app/shared/permission';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Site_List_Access
    },
    component: SitesIndexComponent,
    canActivate: [PermissionGuard]
  },
  {
    path: ':id/members',
    data: {
      breadcrumb: 'app.members',
      canNavigate: false,
      permission: Permission.Member_List_Access
    },
    component: MembersIndexComponent,
    canActivate: [PermissionGuard]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  declarations: [
    SiteFormComponent,
    SiteFilterComponent,
    SiteEditDialogComponent,
    SiteNewDialogComponent,
    SitesIndexComponent,
    MemberEditDialogComponent,
    MemberNewDialogComponent,
    MembersIndexComponent,
    MemberEditFormComponent,
    MemberNewFormComponent
  ],
  entryComponents: [SiteEditDialogComponent, SiteNewDialogComponent],
  imports: [
    SharedModule,
    MemberRoleModule,
    UserModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature('sites', reducers),
    StoreModule.forFeature('members', memberReducers),
    EffectsModule.forFeature([SitesEffects]),
    EffectsModule.forFeature([MembersEffects])
  ]
})
export class SiteModule {}
