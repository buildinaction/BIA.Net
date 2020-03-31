import { BiaNavigation } from './bia-shared/model/bia-navigation';
import { Permission } from './permission';

export const NAVIGATION: BiaNavigation[] = [
  {
    labelKey: 'app.users',
    permissions: [Permission.User_List_Access],
    path: ['/users']
  },
  {
    labelKey: 'app.sites',
    permissions: [Permission.Site_List_Access],
    path: ['/sites']
  },
  // Begin BIADemo
  {
    labelKey: 'app.examples',
    children: [
      {
        labelKey: 'app.planes',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes']
      },
      {
        labelKey: 'app.planesPageMode',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes-page']
      }
    ]
  }
  // End BIADemo
];
