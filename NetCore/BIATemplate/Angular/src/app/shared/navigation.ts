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
];
