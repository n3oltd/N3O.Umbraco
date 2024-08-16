import React from "react";

import { ContentPropertyReq, PropertyType } from "@n3oltd/umbraco-crowdfunding-client";
import { useRequest, useReactive } from "ahooks";

import { usePageData } from "../hooks/usePageData";

import { _client } from "../common/cfClient";
import { Modal } from "./common/Modal";
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


  const {run: loadPropertyValue, loading: isPropLoading} = useRequest((pageId: string) => _client.getContentPropertyValue(pageId, propAlias), {
    manual: true,
    ready: !!propAlias && open,
    onSuccess: data => state.title = data?.textBox?.value || ''
  });

  const {runAsync: updateProperty, loading} = useRequest((req: ContentPropertyReq, pageId: string) => _client.updateProperty(pageId, req), {
    manual: true,
    onSuccess: () => {
      onClose()
    }
  })

  React.useEffect(() => {
    if (open) {
      loadPropertyValue(pageId as string)
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
      await updateProperty(req, pageId as string)
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
        <h3>Campaign Name</h3>
        <div className="input big">
          <input type="text" placeholder="E.g. Building a school" value={state.title} onChange={e => state.title = e.target.value} disabled={isPropLoading}/>
        </div>
    </Modal>
  </>
}