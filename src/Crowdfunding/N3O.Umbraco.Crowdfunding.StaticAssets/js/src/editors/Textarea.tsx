import React from "react";

import {ContentPropertyReq, PropertyType } from "@n3oltd/umbraco-crowdfunding-client";
import { useRequest, useReactive } from "ahooks";

import { usePageData } from "../hooks/usePageData";

import { Modal } from "./common/Modal";
import { _client } from "../common/cfClient";
import { loadingToast, updatingToast } from "../helpers/toaster";
import { EditorProps } from "./types/EditorProps";

export const Textarea: React.FC<EditorProps> = ({
  open,
  propAlias,
  onClose
}) => {

  const state = useReactive({
    description: ''
  });

  const {pageId} = usePageData();

  const {runAsync: loadPropertyValue, data: dataResponse, loading: isPropLoading} = useRequest((pageId: string) => _client.getContentPropertyValue(pageId, propAlias), {
    manual: true,
    ready: !!propAlias && open,
    onSuccess: data => state.description = data?.textarea?.value || ''
  });

  const {runAsync: updateProperty, loading} = useRequest((req: ContentPropertyReq, pageId: string) => _client.updateProperty(pageId, req), {
    manual: true,
    onSuccess: () => {
      onClose();
      window.location.reload()
    }
  })

  React.useEffect(() => {
    if (open && pageId) {
      loadingToast(loadPropertyValue(pageId as string))
    }

  }, [loadPropertyValue, pageId, open]);

  const saveContent = async () => {
    try {
      const req: ContentPropertyReq = {
        alias: propAlias,
        type: PropertyType.Textarea,
        textarea: {
          value: state.description
        } 
      } 
      updatingToast(updateProperty(req, pageId as string))
    } catch(e) {
      console.error(e)
    }
  }
  
  return <> 
    <Modal
      id="textarea-edit"
      isOpen={open}
      onOk={saveContent}
      onClose={onClose}
      oKButtonProps={{
        disabled: isPropLoading || loading
      }}
    >
      {isPropLoading ? <p>{window.themeConfig.text.crowdfunding.apiLoading}</p> : <>
        <h3>{dataResponse?.textarea?.configuration?.description}</h3>
      <div className="edit__content">
        <div className="input__outer dark">
          <p>{window.themeConfig.text.crowdfunding.textAreaEditorTitle}</p>
          <div className="input">
            <textarea
              onChange={e => state.description = e.target.value}
              rows={3}
              value={state.description}
              placeholder={window.themeConfig.text.crowdfunding.textAreaEditorPlaceholder}
              maxLength={dataResponse?.textarea?.configuration?.maximumLength}
              disabled={isPropLoading}
            ></textarea>
          </div>
        </div>
        <p className="subtle">
        {window.themeConfig.text.crowdfunding.textAreaEditorNote.replace("%val", dataResponse?.textarea?.configuration?.maximumLength?.toString() || "100")}        
        </p>
      </div>
      </>}
      
    </Modal>
  </>
}