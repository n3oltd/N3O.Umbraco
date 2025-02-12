import React from "react"

type ModalProps = {
  isOpen: boolean,
  id: string,
  onClose?: () => void,
  onOk: () => void,
  oKButtonProps?: any,
  closeButtonProps?: any,
  children: React.ReactNode,
  okText?: string,
  closeText?: string
}

export function Modal({isOpen, onClose, onOk, id, oKButtonProps, closeButtonProps, children, okText, closeText}: ModalProps){
  if (!isOpen) {
    return null
  }

  return <>
    <div className="n3o-modalsItem modall active" id={id} style={{overflowY: 'scroll'}}>
      <div className="n3o-edit__wrapper" style={{margin: 'auto'}}>
        <div className="n3o-edit">
          {children}
          <div className="n3o-edit__foot">
            <button type="button" className="n3o-button secondary" id="modal-close" {...(closeButtonProps || {})} onClick={() => onClose?.()} data-modal-close="true">
              {closeText || window.themeConfig.text.crowdfunding.cancel}
            </button>
            <button type="button" className="n3o-button primary" id="modal-ok" {...(oKButtonProps || {})}  onClick={onOk}>{okText || window.themeConfig.text.crowdfunding.save}</button>
          </div>
        </div>
      </div>
    </div>
  </>
}