import React from "react"

type ModalProps = {
  isOpen: boolean,
  id: string,
  onClose?: () => void,
  onOk: () => void,
  oKButtonProps?: any,
  closeButtonProps?: any,
  children: React.ReactNode
}

export function Modal({isOpen, onClose, onOk, id, oKButtonProps, closeButtonProps, children}: ModalProps){
  if (!isOpen) {
    return null
  }

  return <>
    <div className="modalsItem modall active" id={id} style={{overflowY: 'scroll'}}>
      <div className="edit__wrapper" style={{margin: 'auto'}}>
        <div className="edit">
          {children}
          <div className="edit__foot">
            <button type="button" className="button secondary" id="modal-close" {...(closeButtonProps || {})} onClick={() => onClose?.()} data-modal-close="true">Cancel</button>
            <button type="button" className="button primary" id="modal-ok" {...(oKButtonProps || {})}  onClick={onOk}>Save</button>
          </div>
        </div>
      </div>
    </div>
  </>
}