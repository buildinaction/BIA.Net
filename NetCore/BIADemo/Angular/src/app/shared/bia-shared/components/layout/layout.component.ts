import { Component, HostBinding, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { APP_SUPPORTED_TRANSLATIONS, THEME_LIGHT, THEME_DARK } from '../../../constants';
import { AuthInfo } from '../../model/auth-info';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaThemeService } from 'src/app/core/bia-core/services/bia-theme.service';
import { NavigationService } from 'src/app/core/bia-core/services/navigation.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { BiaNavigation } from '../../model/bia-navigation';
import { NAVIGATION } from 'src/app/shared/navigation';
import { Store } from '@ngrx/store';
import { AppState } from '../../../../store/state';
import { Observable } from 'rxjs';
import { Site } from 'src/app/domains/site/model/site';
import { getAllSites } from 'src/app/domains/site/store/site.state';
import { setDefaultSite } from 'src/app/domains/site/store/sites-actions';
import { getLocaleId } from 'src/app/app.module';
import { filter, map } from 'rxjs/operators';
import { EnvironmentType } from 'src/app/domains/environment-configuration/model/environment-configuration';
import { getEnvironmentConfiguration } from 'src/app/domains/environment-configuration/store/environment-configuration.state';

@Component({
  selector: 'app-bia-layout',
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
      [helpUrl]="helpUrl"
      [reportUrl]="reportUrl"
      [sites]="sites$ | async"
      [siteId]="currentSiteId"
      [environmentType]="environmentType$ | async"
      [companyName]="companyName"
      (siteChange)="onSiteChange($event)"
      (setDefaultSite)="onSetDefaultSite($event)"
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
  companyName = environment.companyName;
  helpUrl = environment.helpUrl;
  reportUrl = environment.reportUrl;
  username = '';
  headerLogos: string[];
  footerLogo = 'assets/bia/Footer.png';
  supportedLangs = APP_SUPPORTED_TRANSLATIONS;
  sites$: Observable<Site[]>;
  environmentType$: Observable<EnvironmentType | null>;
  currentSiteId: number;

  constructor(
    private biaTranslationService: BiaTranslationService,
    private navigationService: NavigationService,
    private authService: AuthService,
    private biaThemeService: BiaThemeService,
    private store: Store<AppState>
  ) {}

  ngOnInit() {
    this.initEnvironmentType();
    this.initSites();
    this.setAllParamByUserInfo();
    this.initHeaderLogos();
  }

  private initEnvironmentType() {
    this.environmentType$ = this.store.select(getEnvironmentConfiguration).pipe(
      filter((envConf) => !!envConf),
      map((envConf) => (envConf ? envConf.type : null))
    );
  }

  private initSites() {
    this.sites$ = this.store.select(getAllSites).pipe();
  }

  onSiteChange(siteId: number) {
    // this.store.dispatch(setDefaultSite({ id: siteId }));
    this.authService.setCurrentSiteId(siteId);
    location.assign('/');
  }

  onSetDefaultSite(siteId: number) {
    this.store.dispatch(setDefaultSite({ id: siteId }));
  }

  private initHeaderLogos() {
    this.biaThemeService.isCurrentThemeDark$.subscribe((isThemeDark) => {
      this.headerLogos = [
        'assets/bia/Company.png',
        `assets/bia/themes/${isThemeDark !== true ? THEME_LIGHT : THEME_DARK}/img/Division.gif`
      ];
    });
  }

  private setAllParamByUserInfo() {
    this.isLoadingUserInfo = true;
    this.authService.authInfo$.subscribe((authInfo: AuthInfo | null) => {
      if (authInfo) {
        this.setCurrentSiteId(authInfo);
        this.setUserName(authInfo);
        this.setLanguage(authInfo);
        this.filterNavByRole(authInfo);
        this.setTheme(authInfo);
      }
      this.isLoadingUserInfo = false;
    });
  }

  private setCurrentSiteId(authInfo: AuthInfo) {
    if (authInfo && authInfo.additionalInfos && authInfo.additionalInfos.userData) {
      this.currentSiteId = authInfo.additionalInfos.userData.currentSiteId;
    } else {
      this.currentSiteId = 0;
    }
  }

  private setUserName(authInfo: AuthInfo) {
    if (authInfo && authInfo.additionalInfos && authInfo.additionalInfos.userInfo) {
      this.username = authInfo.additionalInfos.userInfo.firstName
        ? authInfo.additionalInfos.userInfo.firstName
        : authInfo.additionalInfos.userInfo.login;
    } else {
      this.username = '?';
    }
  }

  private setLanguage(authInfo: AuthInfo) {
    const langSelected: string | null = this.biaTranslationService.getLangSelected();
    if (langSelected) {
      this.biaTranslationService.loadAndChangeLanguage(langSelected);
    } else if (authInfo && authInfo.additionalInfos && authInfo.additionalInfos.userInfo) {
      const language: string =
        authInfo.additionalInfos.userInfo.language && authInfo.additionalInfos.userInfo.language.length > 0
          ? authInfo.additionalInfos.userInfo.language
          : getLocaleId();
      this.biaTranslationService.loadAndChangeLanguage(language);
    }
  }

  private filterNavByRole(authInfo: AuthInfo) {
    if (authInfo) {
      this.menus = this.navigationService.filterNavByRole(authInfo, NAVIGATION);
    }
  }

  private setTheme(authInfo: AuthInfo) {
    if (
      !this.biaThemeService.getThemeSelected() &&
      authInfo &&
      authInfo.additionalInfos &&
      authInfo.additionalInfos.userProfile &&
      authInfo.additionalInfos.userProfile.theme
    ) {
      this.biaThemeService.changeTheme(authInfo.additionalInfos.userProfile.theme.toLowerCase());
    }
  }
}
