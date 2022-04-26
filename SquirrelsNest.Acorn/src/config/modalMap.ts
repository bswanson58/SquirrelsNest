import {showModal} from '../store/uiActions'
import AddIssueDialog from '../views/issues/AddIssueDialog'

// used by ModalRoot, allows the string key to be stored in the store.
export const modalMap: Record<string, any> = {
  'AddIssueDialog': AddIssueDialog,
}

export function showAddIssueModal() {
  return showModal( {
    modalType: 'AddIssueDialog',
    modalProps: {}
  } )
}
