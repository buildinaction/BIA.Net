// Modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PortalModule } from '@angular/cdk/portal';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { HttpClientModule } from '@angular/common/http';

// PrimeNG Modules
import { AutoCompleteModule } from 'primeng/autocomplete';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { CheckboxModule } from 'primeng/checkbox';
import { ChipsModule } from 'primeng/chips';
import { CodeHighlighterModule } from 'primeng/codehighlighter';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ContextMenuModule } from 'primeng/contextmenu';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { EditorModule } from 'primeng/editor';
import { FullCalendarModule } from 'primeng/fullcalendar';
import { InputMaskModule } from 'primeng/inputmask';
import { InputSwitchModule } from 'primeng/inputswitch';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { ListboxModule } from 'primeng/listbox';
import { MegaMenuModule } from 'primeng/megamenu';
import { MenuModule } from 'primeng/menu';
import { MenubarModule } from 'primeng/menubar';
import { MessagesModule } from 'primeng/messages';
import { MessageModule } from 'primeng/message';
import { MultiSelectModule } from 'primeng/multiselect';
import { PaginatorModule } from 'primeng/paginator';
import { PanelModule } from 'primeng/panel';
import { PanelMenuModule } from 'primeng/panelmenu';
import { ProgressBarModule } from 'primeng/progressbar';
import { RadioButtonModule } from 'primeng/radiobutton';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { SelectButtonModule } from 'primeng/selectbutton';
import { SlideMenuModule } from 'primeng/slidemenu';
import { SliderModule } from 'primeng/slider';
import { SpinnerModule } from 'primeng/spinner';
import { SplitButtonModule } from 'primeng/splitbutton';
import { TabMenuModule } from 'primeng/tabmenu';
import { TableModule } from 'primeng/table';
import { TabViewModule } from 'primeng/tabview';
import { TieredMenuModule } from 'primeng/tieredmenu';
import { ToastModule } from 'primeng/toast';
import { ToggleButtonModule } from 'primeng/togglebutton';
import { ToolbarModule } from 'primeng/toolbar';
import { TooltipModule } from 'primeng/tooltip';

// PrimeNG Services
import { MessageService } from 'primeng/api';

// Component
import { BiaTableHeaderComponent } from './components/table/bia-table-header/bia-table-header.component';
import { ClassicFooterComponent } from './components/layout/classic-footer/classic-footer.component';
import { ClassicHeaderComponent } from './components/layout/classic-header/classic-header.component';
import { ClassicLayoutComponent } from './components/layout/classic-layout/classic-layout.component';
import { ClassicPageLayoutComponent } from './components/layout/classic-page-layout/classic-page-layout.component';
import { IeWarningComponent } from './components/layout/classic-header/ie-warning/ie-warning.component';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { BiaTableControllerComponent } from './components/table/bia-table-controller/bia-table-controller.component';
import { BiaTableComponent } from './components/table/bia-table/bia-table.component';
import { LayoutComponent } from './components/layout/layout.component';
import { PageLayoutComponent } from './components/layout/page-layout.component';
import { PrimengCalendarLocaleDirective } from './directives/primeng-calendar-locale.directive';
import { BiaSafeHtmlPipe } from './pipes/bia-safe-html.pipe';
import { ViewDialogComponent } from './components/view/view-dialog/view-dialog.component';

const PRIMENG_MODULES = [
  AutoCompleteModule,
  BreadcrumbModule,
  ButtonModule,
  CalendarModule,
  CheckboxModule,
  ChipsModule,
  CodeHighlighterModule,
  ConfirmDialogModule,
  ContextMenuModule,
  DialogModule,
  DropdownModule,
  EditorModule,
  FullCalendarModule,
  InputMaskModule,
  InputSwitchModule,
  InputTextModule,
  InputTextareaModule,
  ListboxModule,
  MegaMenuModule,
  MenuModule,
  MenubarModule,
  MessageModule,
  MessagesModule,
  MultiSelectModule,
  PaginatorModule,
  PanelModule,
  PanelMenuModule,
  ProgressBarModule,
  RadioButtonModule,
  ScrollPanelModule,
  SelectButtonModule,
  SlideMenuModule,
  SliderModule,
  SpinnerModule,
  SplitButtonModule,
  TableModule,
  TabMenuModule,
  TabViewModule,
  TieredMenuModule,
  ToastModule,
  ToggleButtonModule,
  ToolbarModule,
  TooltipModule
];

const MODULES = [
  CommonModule,
  PortalModule,
  TranslateModule,
  FormsModule,
  ReactiveFormsModule,
  FlexLayoutModule,
  HttpClientModule
];

const COMPONENTS = [
  ClassicFooterComponent,
  ClassicHeaderComponent,
  ClassicLayoutComponent,
  ClassicPageLayoutComponent,
  SpinnerComponent,
  IeWarningComponent,
  BiaTableComponent,
  BiaTableHeaderComponent,
  BiaTableControllerComponent,
  LayoutComponent,
  PageLayoutComponent,
  PrimengCalendarLocaleDirective,
  ViewDialogComponent
];

const SERVICES = [MessageService];

const PIPES = [BiaSafeHtmlPipe];

@NgModule({
  imports: [...PRIMENG_MODULES, ...MODULES],
  declarations: [...COMPONENTS, ...PIPES],
  exports: [...PRIMENG_MODULES, ...MODULES, ...COMPONENTS],
  providers: [...SERVICES]
})

// https://medium.com/@benmohamehdi/angular-best-practices-coremodule-vs-sharedmodule-25f6721aa2ef
export class BiaSharedModule {}
