import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { reducers } from './store/member-role.state';
import { RolesEffects } from './store/member-roles-effects';


@NgModule({
  imports: [
    StoreModule.forFeature('domain-roles', reducers),
    EffectsModule.forFeature([RolesEffects]),
  ]
})
export class MemberRoleModule {}
