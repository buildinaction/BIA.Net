export enum PrimeNGFiltering {
  StartsWith = 'startsWith',
  Contains = 'contains',
  EndsWith = 'endsWith',
  Equals = 'equals',
  NotEquals = 'notEquals',
  In = 'in',
  Lt = 'lt',
  Lte = 'lte',
  Gt = 'gt',
  Gte = 'gte'
}

export enum TypeTS {
  Date = 'Date',
  Number = 'Number',
  Boolean = 'Boolean',
  String = 'String'
}

export interface CustomButton {
  classValue: string;
  numEvent: number;
  pTooltipValue: string;
  permission: string;
}

export class PrimeTableColumn {
  field: string;
  header: string;
  type: TypeTS;
  filterMode: PrimeNGFiltering;
  formatDate: string;
  isSearchable: boolean;
  isSortable: boolean;

  constructor(field: string, header: string) {
    this.field = field;
    this.header = header;
    this.type = TypeTS.String;
    this.filterMode = PrimeNGFiltering.Contains;
    this.formatDate = '';
    this.isSearchable = true;
    this.isSortable = true;
  }
}

export interface BiaListConfig {
  columns: PrimeTableColumn[];
}
