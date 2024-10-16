import React from "react";

import { ContentPropertyReq, PropertyType } from "@n3oltd/umbraco-crowdfunding-client";
import { useRequest, useReactive } from "ahooks";

import { usePageData } from "../hooks/usePageData";

import { Modal } from "./common/Modal";
import { _client } from "../common/cfClient";
import { loadingToast, updatingToast } from "../helpers/toaster";
import { EditorProps } from "./types/EditorProps";

export const Textbox: React.FC<EditorProps> = ({
  open,
  propAlias,
  onClose
}) => {

  const state = useReactive({
    title: ''
  });

  const {pageId} = usePageData();


  const {runAsync: loadPropertyValue, data: dataResponse, loading: isPropLoading} = useRequest((pageId: string) => _client.getContentPropertyValue(pageId, propAlias), {
    manual: true,
    ready: !!propAlias && open,
    onSuccess: data => {
      state.title = data?.textBox?.value || ''
    }
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
  }, [loadPropertyValue, open, pageId]);

  const saveContent = async () => {
    try {
      const req: ContentPropertyReq = {
        alias: propAlias,
        type: PropertyType.TextBox,
        textBox: {
          value: state.title
        } 
      }

      updatingToast(updateProperty(req, pageId as string))

    } catch(e) {
      console.error(e)
    }
  }

  return <> 
    <Modal
      id="textbox-edit"
      isOpen={open}
      onOk={saveContent}
      onClose={onClose}
      oKButtonProps={{
        disabled: loading
      }}
    >
      {isPropLoading ? <p>{window.themeConfig.text.crowdfunding.apiLoading}</p> : <>
        <h3>{dataResponse?.textBox?.configuration?.description}</h3>
        <div className="n3o-input big">
          <input type="text" 
            maxLength={dataResponse?.textBox?.configuration?.maximumLength}
            placeholder={window.themeConfig.text.crowdfunding?.textEditorPlaceholder} value={state.title} onChange={e => state.title = e.target.value} disabled={isPropLoading}/>
        </div>
        </>}
    </Modal>
  </>
}