import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { Role } from '../model/role';

@Injectable({
  providedIn: 'root'
})
export class RoleDas extends AbstractDas<Role> {
  constructor(injector: Injector) {
    super(injector, 'Roles');
  }
}
