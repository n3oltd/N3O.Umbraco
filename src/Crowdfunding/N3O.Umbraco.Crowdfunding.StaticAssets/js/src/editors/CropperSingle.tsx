import React from "react";

import { Uppy } from "@uppy/core";
import { CropperValueRes, CropShape, ContentPropertyReq, PropertyType, RectangleCropReq } from "@n3oltd/umbraco-crowdfunding-client";
import { useReactive, useRequest } from "ahooks";
import toast from "react-hot-toast";

import { Modal } from "./common/Modal";
import { ImageUploader } from "./common/ImageUploader"

import { usePageData } from "../hooks/usePageData";

import { loadingToast, updatingToast } from "../helpers/toaster";
import { _client } from "../common/cfClient";
import { HostURL, ImageUploadStoragePath } from "../common/constants";
import { EditorProps } from "./types/EditorProps";

export const CropperSingle: React.FC<EditorProps> = ({
  open,
  propAlias,
  onClose
}) => {

  const state = useReactive<{
    uppy: Uppy | undefined | null,
    files: Array<{
      token: string,
      crop: RectangleCropReq
      id?: string
    }>
  }>({
    files: [],
    uppy: null
  });

  const {pageId} = usePageData();

  const {runAsync: loadPropertyValue, data: dataRepsonse, loading} = useRequest((pageId: string) => _client.getContentPropertyValue(pageId as string, propAlias), {
    manual: true,
    ready: open && !!propAlias,
    onSuccess: data => onFileLoadSuccess(data?.cropper)
  });

  const {runAsync: updateProperty, loading: updating} = useRequest((req: ContentPropertyReq, pageId: string) => _client.updateProperty(pageId, req), {
    manual: true,
    onError(e) {
      toast.error(e.message)
    },
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

  const setUppyInstance = React.useCallback(uppyInstance => {
    state.uppy = uppyInstance;
  }, [state]);

  const onFileLoadSuccess = async (cropperResponse: CropperValueRes | undefined) => {
    if (!cropperResponse?.image?.src) {
      return 
    }

    try {
      const response = await fetch(`${HostURL}${cropperResponse.image.src}`);
    
      if (!response.ok) {
        return
      }
    
      const blob = await response.blob();

      const file = {
          id: cropperResponse.image.mediaId,
          name: cropperResponse.image.filename as string,
          type: blob.type,
          data: blob,
          crop: (cropperResponse.image.crops && cropperResponse.image.crops[0]) || {}
      }
      
      state.uppy?.addFile(file)  
    } catch (error) {
      toast.error(window.themeConfig.text.crowdfunding.cropperImageLoadError);
    }
  }  

  const saveContent = async () => {
    if (!state.files.length) {
      toast.error(window.themeConfig.text.crowdfunding.cropperImageRequired);
      return;
    }

    try {
      updatingToast(Promise.all(state.files.map(async (f) => {
        const req: ContentPropertyReq = {
          alias: propAlias,
          type: PropertyType.Cropper,
          cropper: {
            storageToken: f.token,
            shape: CropShape.Rectangle,
            rectangle: f.crop
          } 
        }

        await updateProperty(req, pageId as string)
      })))

    } catch(e) {
      console.error(e)
    }
  }

  return <>
    <Modal
      id="cropper-edit"
      isOpen={open}
      onOk={saveContent}
      onClose={onClose}
      oKButtonProps={{
        disabled: updating || loading
      }}
    >
      {loading ? <p>{window.themeConfig.text.crowdfunding.apiLoading}</p> : <>
      <h3>{dataRepsonse?.cropper?.configuration?.description}</h3>
          
      <ImageUploader 
        aspectRatio={4/1}
        maxFiles={1}
        setUppyInstance={setUppyInstance}
        onFileUpload={(token, id, crop)=> {
          state.files.push({token, id, crop})
        }}
        onFileRemove={file => {
          state.files = state.files.filter(f => f.id !== file?.id)
        }}
        elementId="image-upload"
        uploadUrl={`${HostURL}${ImageUploadStoragePath}`}
        hieght={300}
        dataConfig={{
          width: (dataRepsonse?.cropper?.configuration as any)?.rectangle?.width,
          height: (dataRepsonse?.cropper?.configuration as any)?.rectangle?.height
        }}
      />
      </>}
    </Modal>
  </>
}