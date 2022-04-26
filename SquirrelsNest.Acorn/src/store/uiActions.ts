import {ModalProperties} from '../components/ModalRoot'
import {modalHide, modalShow} from './ui'

export interface ModalAction {
  type: any;
  payload?: ModalProperties;
}

export function showModal(payload: ModalProperties): ModalAction {
  return {
    type: modalShow.type,
    payload: payload
  };
}

export function hideModal(): ModalAction {
  return {
    type: modalHide.type,
  };
}
