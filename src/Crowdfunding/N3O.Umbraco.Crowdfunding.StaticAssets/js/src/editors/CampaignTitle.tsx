import React from "react";

import { useRequest, useReactive, useMutationObserver } from "ahooks";
import { useInsertElement } from "../hooks/useInsertElement";
import { usePageData } from "../hooks/usePageData";

import { PropertyAlias } from "./types/propertyAlias";
import { handleClassMutation } from "../helpers/handleClassMutation";
import { EDIT_TYPE } from "../common/editTypes";
import { _client } from "../common/cfClient";
import { PagePropertyReq, PropertyType } from "@n3oltd/umbraco-crowdfunding-client";

export const CampaignTitle: React.FC = () => {

  const ref = React.useRef<HTMLDivElement>(null);
  const buttonRef = React.useRef<HTMLButtonElement>(null);
  const state = useReactive({
    title: ''
  });

  const {pageId} = usePageData();
  const [isModalOpen, setIsModalOpen] = React.useState<boolean>();
  const [properytInfo, setProperytInfo] = React.useState<PropertyAlias>({alias: ''});

  useInsertElement(`[data-title-edit="edit-campaign-title"]`, EDIT_TYPE.title, setProperytInfo);

  const {run: loadPropertyValue, loading: isPropLoading} = useRequest(() => _client.getPagePropertyValue(pageId as string, properytInfo.alias), {
    manual: true,
    ready: !!pageId && isModalOpen && !!properytInfo.alias,
    onSuccess: data => state.title = data?.textBox?.value || ''
  });

  const {runAsync: updateProperty, loading} = useRequest((req: PagePropertyReq) => _client.updateProperty(pageId as string, req), {
    manual: true,
    onSuccess: () => {
      if (buttonRef.current) {
        buttonRef.current.disabled = false;
        buttonRef.current?.click()
      }
    }
  })

  useMutationObserver(
    handleClassMutation(setIsModalOpen),
    ref,
    { attributes: true },
  );

  React.useEffect(() => {
    if (isModalOpen) {
      loadPropertyValue()
    }

    if (!isModalOpen) {
      state.title = ''
    }

  }, [loadPropertyValue, state, isModalOpen, ]);

  const saveContent = async () => {
    try {
      const req: PagePropertyReq = {
        alias: properytInfo.alias,
        type: PropertyType.TextBox,
        textBox: {
          value: state.title
        } 
      } 
      await updateProperty(req)
    } catch(e) {
      console.error(e)
    }
  }
  
  return <> 
    <div className="modalsItem modall" ref={ref} id="edit-campaign-title" style={{overflowY: 'scroll'}}>
      <div className="edit__wrapper" style={{margin: 'auto'}}>
        <div className="edit">
          <h3>Campaign Name</h3>
          <div className="input big">
            <input type="text" placeholder="E.g. Building a school" value={state.title} onChange={e => state.title = e.target.value} disabled={isPropLoading}/>
          </div>
          <div className="edit__foot">
            <button type="button" data-modal-close="true" className="button secondary" disabled={loading} ref={buttonRef}>Cancel</button>
            <button type="button" className="button primary" disabled={loading} onClick={saveContent}>Save</button>
          </div>
        </div>
      </div>
    </div>
  </>
}