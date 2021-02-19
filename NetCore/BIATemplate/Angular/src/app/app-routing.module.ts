import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HOME_ROUTES } from './features/home/home.module';
import { LayoutComponent } from './shared/bia-shared/components/layout/layout.component';
import { PageLayoutComponent } from './shared/bia-shared/components/layout/page-layout.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      ...HOME_ROUTES,
      {
        path: '',
        component: PageLayoutComponent,
        children: [
          {
            path: 'sites',
            data: {
              breadcrumb: 'app.sites',
              canNavigate: true
            },
            loadChildren: () => import('./features/sites/site.module').then((m) => m.SiteModule)
          },
          {
            path: 'users',
            data: {
              breadcrumb: 'app.users',
              canNavigate: true
            },
            loadChildren: () => import('./features/users/user.module').then((m) => m.UserModule)
          }
        ]
      }
    ]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
