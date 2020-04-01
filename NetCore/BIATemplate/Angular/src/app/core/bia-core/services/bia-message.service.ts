import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class BiaMessageService {
  constructor(private translateService: TranslateService, private messageService: MessageService) {}

  showAddSuccess() {
    this.showSuccess(this.translateService.instant('biaMsg.addElementSuccess'));
  }

  showUpdateSuccess() {
    this.showSuccess(this.translateService.instant('biaMsg.updateElementSuccess'));
  }

  showDeleteSuccess() {
    this.showSuccess(this.translateService.instant('biaMsg.deleteElementSuccess'));
  }

  showSyncSuccess() {
    this.showSuccess(this.translateService.instant('biaMsg.syncElementSuccess'));
  }

  showError() {
    this.messageService.add({
      key: 'bia',
      severity: 'error',
      summary: this.translateService.instant('bia.error'),
      detail: this.translateService.instant('biaMsg.errorOccurredWhileProccessing')
    });
  }

  showSuccess(detailValue: string) {
    const summaryValue = this.translateService.instant('bia.success');
    this.messageService.add({ key: 'bia', severity: 'success', summary: summaryValue, detail: detailValue });
  }

  showInfo(detailValue: string) {
    const summaryValue = this.translateService.instant('bia.info');
    this.messageService.add({ key: 'bia', severity: 'info', summary: summaryValue, detail: detailValue });
  }
}
