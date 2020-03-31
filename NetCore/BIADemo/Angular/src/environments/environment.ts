import {NgxLoggerLevel} from 'ngx-logger';

export const environment = {
  apiUrl: 'http://localhost/BIADemo/api',
  urlLogin: '/api/Auth/login',
  urlLog: '/api/logs',
  urlErrorPage: 'http://localhost/static/error.htm',
  urlAppIcon: 'assets/bia/AppIcon.svg',
  useXhrWithCred: true,
  production: false,
  appTitle: 'BIADemo',
  version: 'v1.0.0-dev',
  logging: {
    conf: {
      serverLoggingUrl: 'http://localhost/BIADemo/api/logs',
      level: NgxLoggerLevel.DEBUG,
      serverLogLevel: NgxLoggerLevel.ERROR
    }
  }
};
