import {NgxLoggerLevel} from 'ngx-logger';

export const environment = {
  helpUrl: '',
  reportUrl: '',
  apiUrl: 'http://localhost/BIATemplate/WebApi/api',
  hubUrl: 'http://localhost/BIATemplate/WebApi/HubForClients',
  urlAuth: '/api/Auth',
  urlLog: '/api/logs',
  urlEnv: '/api/Environment',
  urlErrorPage: 'http://localhost/static/error.htm',
  urlDMIndex: 'http://localhost/DMIndex',
  urlAppIcon: 'assets/bia/AppIcon.svg',
  useXhrWithCred: true,
  production: false,
  appTitle: 'BIATemplate',
  companyName: 'Safran',
  version: '1.3.0',
  logging: {
    conf: {
      serverLoggingUrl: 'http://localhost/BIATemplate/WebApi/api/logs',
      level: NgxLoggerLevel.DEBUG,
      serverLogLevel: NgxLoggerLevel.ERROR
    }
  }
};
