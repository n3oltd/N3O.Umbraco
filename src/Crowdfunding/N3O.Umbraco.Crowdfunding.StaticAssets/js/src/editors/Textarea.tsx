import React from "react";

import {ContentPropertyReq, PropertyType } from "@n3oltd/umbraco-crowdfunding-client";
import { useRequest, useReactive } from "ahooks";

import { usePageData } from "../hooks/usePageData";

import { Modal } from "./common/Modal";
import { _client } from "../common/cfClient";
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

  const {run: loadPropertyValue, loading: isPropLoading} = useRequest((pageId: string) => _client.getContentPropertyValue(pageId, propAlias), {
    manual: true,
    ready: !!propAlias && open,
    onSuccess: data => state.description = data?.textarea?.value || ''
  });

  const {runAsync: updateProperty, loading} = useRequest((req: ContentPropertyReq, pageId: string) => _client.updateProperty(pageId, req), {
    manual: true,
    onSuccess: () => {
      onClose()
    }
  })

  React.useEffect(() => {
    if (open && pageId) {
      loadPropertyValue(pageId as string)
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
      await updateProperty(req, pageId as string)
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
      <h3>Campaign Description</h3>
      <div className="edit__content">
        <div className="input__outer dark">
          <p>Short Description (Optional)</p>
          <div className="input">
            <textarea
              onChange={e => state.description = e.target.value}
              rows={3}
              value={state.description}
              placeholder="Type your message here"
              maxLength={160}
              disabled={isPropLoading}
            ></textarea>
          </div>
        </div>
        <p className="subtle">
          Slightly longer text that will appear after the campaign name. You
          can write up to 160 characters.
        </p>
      </div>
    </Modal>
  </>
}