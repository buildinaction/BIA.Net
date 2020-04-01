import { UserInfo } from './user-info';
import { UserProfile } from './user-profile';

export interface AuthInfo {
  userInfo: UserInfo;
  userProfile: UserProfile;
  token: string;
  permissions: string[];
}
