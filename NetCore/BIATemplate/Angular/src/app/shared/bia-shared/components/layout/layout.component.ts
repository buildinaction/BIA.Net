import { Component, HostBinding, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { APP_SUPPORTED_TRANSLATIONS } from '../../../constants';
import { AuthInfo } from '../../model/auth-info';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaThemeService } from 'src/app/core/bia-core/services/bia-theme.service';
import { NavigationService } from 'src/app/core/bia-core/services/navigation.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { BiaNavigation } from '../../model/bia-navigation';
import { NAVIGATION } from 'src/app/shared/navigation';

@Component({
  selector: 'app-core-layout',
  template: `
    <bia-spinner [overlay]="true" *ngIf="isLoadingUserInfo"></bia-spinner>
    <bia-classic-layout
      [menus]="menus"
      [version]="version"
      [username]="username"
      [headerLogos]="headerLogos"
      [footerLogo]="footerLogo"
      [supportedLangs]="supportedLangs"
      [appTitle]="appTitle"
      companyName="MyCompany"
    >
      <router-outlet></router-outlet>
    </bia-classic-layout>
  `
})
export class LayoutComponent implements OnInit {
  @HostBinding('class.bia-flex') flex = true;
  isLoadingUserInfo = false;

  menus = new Array<BiaNavigation>();
  version = environment.version;
  appTitle = environment.appTitle;
  username = '';
  headerLogos = ['assets/bia/Company.png', 'assets/bia/Division.gif'];
  footerLogo = 'assets/bia/Footer.png';
  supportedLangs = APP_SUPPORTED_TRANSLATIONS;

  constructor(
    private biaTranslationService: BiaTranslationService,
    private navigationService: NavigationService,
    private authService: AuthService,
    private biaThemeService: BiaThemeService
  ) {}

  ngOnInit() {
    this.setAllParamByUserInfo();
  }

  private setAllParamByUserInfo() {
    this.isLoadingUserInfo = true;
    this.authService.authInfo$.subscribe((authInfo: AuthInfo | null) => {
      if (authInfo) {
        this.setUserName(authInfo);
        this.setLanguage(authInfo);
        this.filterNavByRole(authInfo);
        this.setTheme(authInfo);
      }
      this.isLoadingUserInfo = false;
    });
  }

  private setUserName(authInfo: AuthInfo) {
    if (authInfo && authInfo.userInfo) {
      this.username = authInfo.userInfo.firstName ? authInfo.userInfo.firstName : authInfo.userInfo.login;
    } else {
      this.username = '?';
    }
  }

  private setLanguage(authInfo: AuthInfo) {
    const langSelected: string | null = this.biaTranslationService.getLangSelected();
    if (langSelected) {
      this.biaTranslationService.loadAndChangeLanguage(langSelected);
    } else if (authInfo && authInfo.userInfo && authInfo.userInfo.language) {
      this.biaTranslationService.loadAndChangeLanguage(authInfo.userInfo.language);
    }
  }

  private filterNavByRole(authInfo: AuthInfo) {
    if (authInfo) {
      this.menus = this.navigationService.filterNavByRole(authInfo, NAVIGATION);
    }
  }

  private setTheme(authInfo: AuthInfo) {
    if (!this.biaThemeService.getThemeSelected() && authInfo && authInfo.userProfile && authInfo.userProfile.theme) {
      this.biaThemeService.changeTheme(authInfo.userProfile.theme.toLowerCase());
    }
  }
}
