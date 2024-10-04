import React from 'react';

import Uppy from '@uppy/core';
import toast from 'react-hot-toast';
import { useReactive, useRequest } from 'ahooks';
import { ContentPropertyReq, PropertyType, ContentPropertyValueRes, NestedItemReq, CropShape } from '@n3oltd/umbraco-crowdfunding-client';

import { ImageUploader } from './common/ImageUploader';
import { Modal } from './common/Modal';

import { usePageData } from '../hooks/usePageData';

import { _client } from '../common/cfClient';
import { loadingToast, updatingToast } from '../helpers/toaster';
import { EditorProps } from './types/EditorProps';
import { ImageUploadStoragePath, HostURL } from '../common/constants';

import './Gallery.css';

export const Gallery: React.FC<EditorProps> = ({
  open,
  propAlias,
  onClose
}) => {
  
  const [filesToUplaod, setFiles] = React.useState<Array<{
    crop: any,
    token: string
  }>>([]);

  const state = useReactive<{
    propertyRes: ContentPropertyValueRes | undefined,
    uppy: Uppy | undefined | null,
    videoUrl: string
  }>({
    propertyRes: undefined,
    videoUrl: "",
    uppy: null
  });

  const {pageId} = usePageData();

  const {runAsync: loadPropertyValue, data: dataResponse, loading} = useRequest((pageId: string) => _client.getContentPropertyValue(pageId, propAlias), {
    manual: true,
    ready: open && !!propAlias,
    onSuccess: data => {
      state.propertyRes = data;
      onLoad(data)

      state.videoUrl = data.nested?.items?.flatMap(item => item.properties)?.find(p => p?.type === PropertyType.TextBox)?.textBox?.value || ""
    }
  });

  const {runAsync: updateProperty,} = useRequest((req: ContentPropertyReq, pageId: string) => _client.updateProperty(pageId, req), {
    manual: true,
    onSuccess: () => onClose()
  })

  React.useEffect(() => {
    if (open && pageId) {
      loadingToast(loadPropertyValue(pageId as string))
    }
  }, [loadPropertyValue,pageId, open]);

  const onLoad = async (contentPropertyValueRes: ContentPropertyValueRes) => {
    if (!contentPropertyValueRes) {
      return 
    }

    const uploadedFiles = state.propertyRes?.nested?.items?.filter(i => i.contentTypeAlias?.includes('HeroImage'))?.flatMap(i => i.properties?.map(p => p.cropper?.image)) || [];

    try {

      await Promise.all(uploadedFiles.map(async f => {
        const response = await fetch(`${HostURL}${f?.src}`);
    
        if (!response.ok) {
          return
        }
      
        const blob = await response.blob();

        const file = {
            id: f?.mediaId,
            name: f?.filename as string,
            type: blob.type,
            data: blob,
        }

        const fileId = state.uppy?.addFile(file);
        state.uppy?.setFileState(fileId as string, {
          aspectRatioApplied: true,
          crop: (f?.crops && f?.crops[0]) || {}} as any);
      }))
        
    } catch (error) {
      toast.error(window.themeConfig.text.crowdfunding.cropperImageLoadError);
    }
  }

  const saveContent = async () => {

   const imagesSchema =  state.propertyRes?.nested?.schema?.items?.find(c => c.contentTypeAlias?.includes('HeroImage'));    
   const videoUrlSchema =  state.propertyRes?.nested?.schema?.items?.find(c => c.contentTypeAlias?.includes('Url'));

   let totalItems = 0;
   
    if (state.videoUrl) {
      totalItems += 1;
    }

   if (filesToUplaod.length) {
      totalItems += filesToUplaod.length;
   }

   if (totalItems < (dataResponse?.nested?.configuration?.minimumItems || 1)) {
    toast.error(window.themeConfig.text.crowdfunding.cropperGalleryMinimunRequired.replace("%val", dataResponse?.nested?.configuration?.minimumItems?.toString() || "1"))
    
    return;
   }

   if (dataResponse?.nested?.configuration?.maximumItems && totalItems > dataResponse?.nested?.configuration?.maximumItems) {
    toast.error(window.themeConfig.text.crowdfunding.cropperGalleryMinimunRequired.replace("%val", dataResponse?.nested?.configuration?.maximumItems?.toString() || "1"))
    
    return;
   }
   
   const nestedReq = filesToUplaod.map(file => {
    const req : NestedItemReq = {
      contentTypeAlias: imagesSchema?.contentTypeAlias,
      properties: imagesSchema?.properties?.map(p => {

        const prop : ContentPropertyReq = {
          alias: p.alias,
          type: p.type,
          cropper: {
            storageToken: file.token,
            shape: CropShape.Rectangle,
            rectangle: {
              ...file.crop
            } 
          }
        }
        return prop
      })
    }

    return  req;
   
  })

  if (videoUrlSchema) {
    const videoRequest: NestedItemReq = {
        contentTypeAlias: videoUrlSchema.contentTypeAlias,
        properties: videoUrlSchema.properties?.map(p => {

          return {
            alias: p.alias,
            type: p.type,
            textBox: {
              value: state.videoUrl
            }
          }
        })
    }

    nestedReq.push(videoRequest);
  }

    try {
      const req: ContentPropertyReq = {
        alias: propAlias,
        type: PropertyType.Nested,
        nested: {
          items: nestedReq
        }
      }

      updatingToast(updateProperty(req, pageId as string))
    } catch(e) {
      console.error(e)
    }
  }

  const handleUplodedFile = React.useCallback((token: string, crop) => {
    setFiles(prev => [...prev, {crop, token}])
  }, [setFiles]);

  const setUppyInstance = React.useCallback(uppyInstance => {
    state.uppy = uppyInstance;
  }, [state]);

  //TODO: this needs to be improved, but atm keep that way
  const hasVideoUrlContent = state.propertyRes?.nested?.schema?.items?.find(i => i.contentTypeAlias?.includes('Url'));

  const maxFiles = state.videoUrl && dataResponse?.nested?.configuration?.maximumItems 
                  ? dataResponse?.nested?.configuration?.maximumItems - 1 : dataResponse?.nested?.configuration?.maximumItems;

  return <>
    <Modal
      id="cropper-nested-edit"
      isOpen={open}
      onOk={saveContent}
      onClose={onClose}
      oKButtonProps={{
        disabled: loading
      }}
    >
        {loading ? <p>Loading...</p> : <>
        <h3>{dataResponse?.nested?.configuration?.description}</h3>
        <ImageUploader 
          onFileUpload={handleUplodedFile}
          setUppyInstance={setUppyInstance}
          aspectRatio={4/3}
          maxFiles={Number.isInteger(maxFiles) ? Number(maxFiles) : NaN}
          minFiles={1}
          elementId='campaign-cover'
          uploadUrl={`${HostURL}${ImageUploadStoragePath}`}
          openEditor={!state.propertyRes?.nested?.items?.length}
        />
        </>}

        {(!loading && hasVideoUrlContent) && <div className="edit__content">
            <h3>Campaign Video</h3>
            <div className="input__outer">
              <p>Youtube Video's URL</p>
              <div className="input">
                <input
                  type="text"
                  value={state.videoUrl}
                  onChange={e => state.videoUrl = e.target.value}
                  placeholder="example: https://www.youtube.com/embed/tnGoQ5HcaoA"
                />
              </div>
              {state.videoUrl && <iframe referrerPolicy="strict-origin-when-cross-origin" src={state.videoUrl} style={{
                width: "100%",
                marginTop: "12px",
                height: '263px',
                borderRadius: "var(--corner-md, 8px)"
              }} frameBorder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"></iframe>}
            </div>
          </div>}

      </Modal>
  </>
}