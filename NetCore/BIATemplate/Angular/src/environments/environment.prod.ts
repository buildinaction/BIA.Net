import {NgxLoggerLevel} from 'ngx-logger';

export const environment = {
  apiUrl: '../WebApi/api',
  urlLogin: '/api/Auth/login',
  urlLog: '/api/logs',
  urlErrorPage: '/static/error.htm',
  urlAppIcon: 'assets/bia/AppIcon.svg',
  useXhrWithCred: false,
  production: true,
  appTitle: 'BIATemplate',
  version: 'v1.0.0',
  logging: {
    conf: {
      serverLoggingUrl: '../WebApi/api/logs',
      level: NgxLoggerLevel.DEBUG,
      serverLogLevel: NgxLoggerLevel.ERROR
    }
  }
};
