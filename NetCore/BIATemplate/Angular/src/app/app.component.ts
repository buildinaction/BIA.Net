import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: '<router-outlet></router-outlet>',
  styles: [':host { min-height: 100vh; display: flex; }']
})
export class AppComponent {}
