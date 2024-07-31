import React from "react";

import { useRequest, useReactive, useMutationObserver } from "ahooks";
import { useInsertElement } from "../hooks/useInsertElement";
import { usePageData } from "../hooks/usePageData";

import { PropertyAlias } from "./types/propertyAlias";
import { handleClassMutation } from "../helpers/handleClassMutation";
import { EDIT_TYPE } from "../common/editTypes";
import { _client } from "../common/cfClient";
import { PagePropertyReq, PropertyType } from "@n3oltd/umbraco-crowdfunding-client";

export const CampaignMeta: React.FC = () => {

  const ref = React.useRef<HTMLDivElement>(null);
  const buttonRef = React.useRef<HTMLButtonElement>(null);
  const state = useReactive({
    title: '',
    description: ''
  });

  const {pageId} = usePageData();
  const [isModalOpen, setIsModalOpen] = React.useState<boolean>();
  const [properytInfo, setProperytInfo] = React.useState<PropertyAlias>({alias: ''});

  useInsertElement(`[data-title-edit="edit-campaign-title"]`, EDIT_TYPE.title, setProperytInfo);

  const {run: loadPropertyValue} = useRequest(() => _client.getPagePropertyValue(pageId as string, properytInfo.alias), {
    manual: true,
    ready: !!pageId && isModalOpen && !!properytInfo.alias,
    onSuccess: data => state.title = data?.textBox?.value || ''
  });

  const {runAsync: updateProperty,} = useRequest((req: PagePropertyReq) => _client.updateProperty(pageId as string, req), {
    manual: true,
    onSuccess: () => buttonRef.current?.click()
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
      state.description = ''
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
            <input type="text" placeholder="E.g. Building a school" onChange={e => state.title = e.target.value} />
          </div>
          <div className="edit__content">
            <div className="input__outer dark">
              <p>Short Description (Optional)</p>
              <div className="input">
                <textarea
                  onChange={e => state.description = e.target.value}
                  rows={3}
                  placeholder="Type your message here"
                ></textarea>
              </div>
            </div>
            <p className="subtle">
              Slightly longer text that will appear after the campaign name. You
              can write up to 160 characters.
            </p>
          </div>
          <div className="edit__foot">
            <button type="button" data-modal-close="true" className="button secondary" ref={buttonRef}>Cancel</button>
            <button type="button" className="button primary" onClick={saveContent}>Save</button>
          </div>
        </div>
      </div>
    </div>
  </>
}