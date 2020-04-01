import { Component, ChangeDetectionStrategy, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { BiaClassicLayoutService } from '../classic-layout/bia-classic-layout.service';
import { Platform } from '@angular/cdk/platform';
import { MenuItem } from 'primeng/api';
import { TranslateService } from '@ngx-translate/core';
import { BiaNavigation } from '../../../model/bia-navigation';
import { Subscription, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'bia-classic-header',
  templateUrl: './classic-header.component.html',
  styleUrls: ['./classic-header.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})
export class ClassicHeaderComponent implements OnDestroy {
  @Input()
  set username(name: string | undefined) {
    if (name) {
      this.usernameParam = { name };
    }
    this.buildTopBarMenu();
  }
  @Input() appTitle: string;
  @Input() version: string;
  @Input()
  set menus(navigations: BiaNavigation[]) {
    if (navigations && navigations.length > 0) {
      this.navigations = navigations;
      this.buildNavigation();
    }
  }

  @Input() logos: string[];
  @Input() supportedLangs: string[];
  @Input() allowThemeChange?: boolean;
  @Input() helpUrl?: string;

  @Output() language = new EventEmitter<string>();
  @Output() theme = new EventEmitter<string>();

  usernameParam: { name: string };
  navigations: BiaNavigation[];
  fullscreenMode = false;
  isIE = this.platform.TRIDENT;
  urlAppIcon = environment.urlAppIcon;

  private sub = new Subscription();

  topBarMenuItems: MenuItem[];
  navMenuItems: MenuItem[];
  appIcon$: Observable<string>;

  constructor(
    public layoutService: BiaClassicLayoutService,
    private platform: Platform,
    private translateService: TranslateService
  ) {}

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  toggleFullscreenMode() {
    this.fullscreenMode = !this.fullscreenMode;
    if (this.fullscreenMode === true) {
      this.layoutService.hideFooter();
    } else {
      this.layoutService.showFooter();
    }
  }

  refresh() {
    localStorage.clear();
    location.reload();
  }

  openHelp() {
    window.open(this.helpUrl, 'blank');
  }

  private onChangeTheme(theme: string) {
    this.theme.emit(theme);
  }

  private onChangeLanguage(lang: string) {
    this.language.emit(lang);
  }

  buildNavigation() {
    const translationKeys = new Array<string>();
    this.navigations.forEach((menu) => {
      if (menu.children) {
        menu.children.forEach((child) => {
          translationKeys.push(child.labelKey);
        });
      }
      translationKeys.push(menu.labelKey);
    });

    this.sub.add(
      this.translateService.stream(translationKeys).subscribe((translations) => {
        this.navMenuItems = [];
        this.navigations.forEach((menu) => {
          const childrenMenuItem: MenuItem[] = [];
          if (menu.children) {
            menu.children.forEach((child) => {
              childrenMenuItem.push({
                label: translations[child.labelKey],
                routerLink: child.path
              });
            });
          }
          this.navMenuItems.push({
            label: translations[menu.labelKey],
            routerLink: menu.path,
            items: childrenMenuItem.length > 0 ? childrenMenuItem : undefined
          });
        });
      })
    );
  }

  buildTopBarMenu() {
    const translationKeys = [
      'bia.lang.fr',
      'bia.lang.de',
      'bia.lang.es',
      'bia.lang.gb',
      'bia.lang.mx',
      'bia.lang.us',
      'bia.greetings',
      'bia.languages',
      'bia.theme',
      'bia.themeLight',
      'bia.themeDark'
    ];
    this.sub.add(
      this.translateService.stream(translationKeys).subscribe((translations) => {
        const menuItemLang: MenuItem[] = [];

        if (this.supportedLangs) {
          this.supportedLangs.forEach((lang) => {
            menuItemLang.push({
              label: translations['bia.lang.' + lang.split('-')[1].toLowerCase()],
              command: () => {
                this.onChangeLanguage(lang);
              }
            });
          });
        }

        let displayName = '';
        if (this.usernameParam && this.usernameParam.name) {
          displayName = this.usernameParam.name;
        }

        this.topBarMenuItems = [
          {
            label: translations['bia.greetings'] + ' ' + displayName,
            items: [
              [
                {
                  label: translations['bia.languages'],
                  items: menuItemLang
                },
                {
                  label: translations['bia.theme'],
                  items: [
                    {
                      label: translations['bia.themeLight'],
                      command: () => {
                        this.onChangeTheme('light');
                      }
                    },
                    {
                      label: translations['bia.themeDark'],
                      command: () => {
                        this.onChangeTheme('dark');
                      }
                    }
                  ]
                }
              ]
            ]
          }
        ];
      })
    );
  }
}
