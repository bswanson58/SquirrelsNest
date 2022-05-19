import {ClIssue} from '../data/graphQlTypes'
import {showModal} from '../store/uiActions'
import AddIssueDialog from '../views/issues/AddIssueDialog'
import DeleteIssueDialog from '../views/issues/DeleteIssueDialog'
import EditComponentDialog from '../views/issues/EditComponentDialog'
import EditIssueTypeDialog from '../views/issues/EditIssueTypeDialog'

// used by ModalRoot, allows the string key to be stored in the store.
export const modalMap: Record<string, any> = {
  'AddIssueDialog': AddIssueDialog,
  'EditComponentDialog': EditComponentDialog,
  'EditIssueTypeDialog': EditIssueTypeDialog,
  'ConfirmIssueDelete': DeleteIssueDialog,
}

export function showAddIssueModal() {
  return showModal( {
    modalType: 'AddIssueDialog',
    modalProps: {}
  } )
}

export function showEditComponentModal( issue: ClIssue ) {
  return showModal({
    modalType: 'EditComponentDialog',
    modalProps: issue
  })
}

export function showEditIssueTypeModal( issue: ClIssue ) {
  return showModal( {
    modalType: 'EditIssueTypeDialog',
    modalProps: issue
  } )
}

export function showDeleteIssueConfirm( issue: ClIssue ) {
  return showModal( {
    modalType: 'ConfirmIssueDelete',
    modalProps: issue
  } )
}
