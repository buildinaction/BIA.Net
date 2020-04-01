export interface View {
  id: number;
  tableId: string;
  name: string;
  description: string;
  viewType: number;
  isSiteDefault: boolean;
  isUserDefault: boolean;
  preference: string;
}
