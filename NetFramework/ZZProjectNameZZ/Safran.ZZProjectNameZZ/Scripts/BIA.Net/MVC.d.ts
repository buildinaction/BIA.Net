interface ResizeDialogInterface {
    (dialog: JQuery): any;
}
declare var ResizeDialog: ResizeDialogInterface;
declare module BIA.Net.MVC {
    class Localisation {
        static cultureFormatDate: string;
        static cultureShort: string;
        static SetCalendarDatePicker(root: any): void;
        static SetFloatValidator(): void;
    }
}
declare module BIA.Net.MVC {
    class PreventDuplicateRequest {
        static Apply(form: JQuery): JQuery;
    }
}
