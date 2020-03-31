import { DtoState } from './dto-state.enum';

export interface BaseDto {
  id: number;
  dtoState: DtoState;
}
