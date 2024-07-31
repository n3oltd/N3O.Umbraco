import React from "react";

import { PagePropertyReq, PropertyType } from "@n3oltd/umbraco-crowdfunding-client";
import { useMutationObserver, useReactive, useRequest } from "ahooks";

import { ImageUploader } from "./ImageUploader"
import { useInsertElement } from "../hooks/useInsertElement";
import { usePageData } from "../hooks/usePageData";
import { handleClassMutation } from "../helpers/handleClassMutation";
import { PropertyAlias } from "./types/propertyAlias";
import { EDIT_TYPE } from "../common/editTypes";
import { _client } from "../common/cfClient";


export const CampaignCover: React.FC = () => {

  const ref = React.useRef<HTMLDivElement>(null);
  const buttonRef = React.useRef<HTMLButtonElement>(null);
  const state = useReactive({
    token: '',
    image: ''
  });

  const {pageId} = usePageData();
  const [isModalOpen, setIsModalOpen] = React.useState<boolean>();
  const [properytInfo, setProperytInfo] = React.useState<PropertyAlias>({alias: ''});

  useInsertElement(`[data-cover-edit="edit-campaign-cover"]`, EDIT_TYPE.cover, setProperytInfo);

  const {run: loadPropertyValue} = useRequest(() => _client.getPagePropertyValue(pageId as string, properytInfo.alias), {
    manual: true,
    ready: !!pageId && isModalOpen && !!properytInfo.alias,
    onSuccess: data => state.image = data?.cropper?.image?.src || ''
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
      state.token = ''
      state.image = ''
    }

  }, [loadPropertyValue, state, isModalOpen, ]);

  const saveContent = async () => {
    try {
      const req: PagePropertyReq = {
        alias: properytInfo.alias,
        type: PropertyType.Cropper,
        cropper: {
          storageToken: state.token
        } 
      }

      await updateProperty(req)
    } catch(e) {
      console.error(e)
    }
  }

  return <>
    <div className="modalsItem modall" id="edit-campaign-cover">
      <div className="edit__wrapper" style={{margin: 'auto'}}>
        <div className="edit">
          <h3>Upload Cover Image</h3>
          
            <ImageUploader 
              aspectRatio={4/1}
              maxFiles={1}
              onFileUpload={token => state.token = token}
              uploaderId="campaign-cover"
              uploadUrl="https://n3oltd.n3o.cloud/umbraco/api/Storage/tempUpload"
              hieght={200}
            />
          <div className="edit__foot">
            <button type="button" data-modal-close="true" className="button secondary" ref={buttonRef}>Cancel</button>
            <button type="button" className="button primary" onClick={saveContent}>Save</button>
          </div>
        </div>
      </div>
    </div>
  </>
}