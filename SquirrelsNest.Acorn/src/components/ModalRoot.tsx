import {connect, ConnectedProps} from 'react-redux'
import {modalMap} from '../config/modalMap'
import {AppDispatch, RootState} from '../store/configureStore'
import {hideModal} from '../store/uiActions'

export interface ModalProperties {
  modalType: string,
  modalProps: any,
}

export interface ModalPayload extends ModalProperties {
  modalState: boolean,
}

const mapStateToProps = ( state: RootState ) => ({
  payload: state.ui.modals.payload,
})

const mapDispatchToProps = ( dispatch: AppDispatch ) => ({
  hideModal: () => dispatch( hideModal() )
})

const connector = connect( mapStateToProps, mapDispatchToProps )

type ModalProps = {} & ConnectedProps<typeof connector>;

export function ModalRoot( props: ModalProps ) {
  if(( !props.payload.modalType ) ||
     ( !props.payload.modalState )) {
    return null
  }

  const SpecificModal = modalMap[props.payload.modalType]

  return <SpecificModal {...props} />
}

export default connector( ModalRoot )
