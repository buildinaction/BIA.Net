import { Component, HostBinding } from '@angular/core';

@Component({
  selector: 'app-core-page-layout',
  template: `
    <bia-classic-page-layout>
      <router-outlet></router-outlet>
    </bia-classic-page-layout>
  `
})
export class PageLayoutComponent {
  @HostBinding('class.bia-flex') flex = true;
}
