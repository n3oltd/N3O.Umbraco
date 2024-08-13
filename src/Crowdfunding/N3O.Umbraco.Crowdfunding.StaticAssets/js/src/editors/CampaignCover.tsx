import React from "react";

import { CropperValueRes, CropShape, PagePropertyReq, PropertyType, RectangleCropReq } from "@n3oltd/umbraco-crowdfunding-client";
import { useMutationObserver, useReactive, useRequest } from "ahooks";

import { ImageUploader } from "./ImageUploader"
import { useInsertElement } from "../hooks/useInsertElement";
import { usePageData } from "../hooks/usePageData";
import { handleClassMutation } from "../helpers/handleClassMutation";
import { PropertyAlias } from "./types/propertyAlias";
import { EDIT_TYPE } from "../common/editTypes";
import { _client } from "../common/cfClient";
import { Uppy } from "@uppy/core";
import { ImageUploadStoragePath } from "../common/constants";


export const CampaignCover: React.FC = () => {

  const ref = React.useRef<HTMLDivElement>(null);
  const buttonRef = React.useRef<HTMLButtonElement>(null);
  const state = useReactive<{
    uppy: Uppy | undefined | null,
    token: string,
    crop: RectangleCropReq
  }>({
    token: '',
    crop:{},
    uppy: null
  });

  const {pageId} = usePageData();
  const [isModalOpen, setIsModalOpen] = React.useState<boolean>();
  const [properytInfo, setProperytInfo] = React.useState<PropertyAlias>({alias: ''});

  useInsertElement(`[data-cover-edit="edit-campaign-cover"]`, EDIT_TYPE.cover, setProperytInfo);

  const {run: loadPropertyValue} = useRequest(() => _client.getPagePropertyValue(pageId as string, properytInfo.alias), {
    manual: true,
    ready: !!pageId && isModalOpen && !!properytInfo.alias,
    onSuccess: data => onFileLoadSuccess(data?.cropper)
  });

  const {runAsync: updateProperty, loading: updating} = useRequest((req: PagePropertyReq) => _client.updateProperty(pageId as string, req), {
    manual: true,
    onSuccess: () => handleClose()
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
  }, [loadPropertyValue, isModalOpen]);

  const setUppyInstance = React.useCallback(uppyInstance => {
    state.uppy = uppyInstance;
  }, [state]);

  const onFileLoadSuccess = async (cropperResponse: CropperValueRes | undefined) => {
    if (cropperResponse?.image?.src) {
    const response = await fetch(`${window.location.origin}${cropperResponse.image.src}`);
    if (!response.ok) {
      return
    }
    const blob = await response.blob();

      const file = {
          id: cropperResponse.image.mediaId,
          name: cropperResponse.image.filename as string,
          type: blob.type,
          data: blob
      }

      state.uppy?.addFile(file)
    } 
  }

  const handleCrop = event => {
    state.crop.bottomLeft = {
          x: Math.floor(event.detail.x),
          y: Math.floor(event.detail.y) 
        };

    state.crop.topRight = {
      x: Math.floor(event.detail.width),
      y: Math.floor(event.detail.height)
    };
  }

  const saveContent = async () => {
    if (!state.token) {
      return;
    }
    try {
      const req: PagePropertyReq = {
        alias: properytInfo.alias,
        type: PropertyType.Cropper,
        cropper: {
          storageToken: state.token,
          shape: CropShape.Rectangle,
          rectangle: state.crop
        } 
      }

      await updateProperty(req)
    } catch(e) {
      console.error(e)
    }
  }

  const handleClose = () => {
    state.crop = {}
    state.token = '';
    state.uppy?.clear();
    buttonRef.current?.click()
  }

  return <>
    <div className="modalsItem modall" id="edit-campaign-cover" ref={ref}>
      <div className="edit__wrapper" style={{margin: 'auto'}}>
        <div className="edit">
          <h3>Upload Cover Image</h3>
          
            <ImageUploader 
              aspectRatio={4/1}
              maxFiles={1}
              setUppyInstance={setUppyInstance}
              onFileUpload={(token, uppy )=> {
                state.token = token;
                state.uppy = uppy
              }}
              onCrop={handleCrop}
              elementId="campaign-cover"
              uploadUrl={`${window.location.origin}${ImageUploadStoragePath}`}
              hieght={200}
            />
          <div className="edit__foot">
            <button type="button" data-modal-close="true" className="button secondary" onClick={handleClose} ref={buttonRef}>Cancel</button>
            <button type="button" className="button primary" disabled={updating} onClick={saveContent}>Save</button>
          </div>
        </div>
      </div>
    </div>
  </>
}