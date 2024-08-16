import React from "react";

import { Uppy } from "@uppy/core";
import { CropperValueRes, CropShape, ContentPropertyReq, PropertyType, RectangleCropReq } from "@n3oltd/umbraco-crowdfunding-client";
import { useReactive, useRequest } from "ahooks";

import { Modal } from "./common/Modal";
import { ImageUploader } from "./common/ImageUploader"

import { usePageData } from "../hooks/usePageData";

import { _client } from "../common/cfClient";
import { ImageUploadStoragePath } from "../common/constants";
import { EditorProps } from "./types/EditorProps";

export const CropperSingle: React.FC<EditorProps> = ({
  open,
  propAlias,
  onClose
}) => {

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


  const {run: loadPropertyValue} = useRequest((pageId: string) => _client.getContentPropertyValue(pageId as string, propAlias), {
    manual: true,
    ready: open && !!propAlias,
    onSuccess: data => onFileLoadSuccess(data?.cropper)
  });

  const {runAsync: updateProperty, loading: updating} = useRequest((req: ContentPropertyReq, pageId: string) => _client.updateProperty(pageId, req), {
    manual: true,
    onSuccess: () => onClose()
  })

  

  React.useEffect(() => {
    if (open) {
      loadPropertyValue(pageId as string)
    }
  }, [loadPropertyValue, pageId, open]);

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
      const req: ContentPropertyReq = {
        alias: propAlias,
        type: PropertyType.Cropper,
        cropper: {
          storageToken: state.token,
          shape: CropShape.Rectangle,
          rectangle: state.crop
        } 
      }

      await updateProperty(req, pageId as string)
    } catch(e) {
      console.error(e)
    }
  }

  return <>
    <Modal
      id="cropper-single-edit"
      isOpen={open}
      onOk={saveContent}
      onClose={onClose}
      oKButtonProps={{
        disabled: updating
      }}
    >
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
    </Modal>
  </>
}