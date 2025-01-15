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
import { getCrowdfundingCookie } from '../common/cookie';

export const Gallery: React.FC<EditorProps> = ({
  open,
  propAlias,
  onClose
}) => {
  
  const [filesToUplaod, setFiles] = React.useState<Array<{
    crop?: any,
    token?: string,
    id?: string
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

  const {runAsync: loadPropertyValue, data: dataResponse, loading} = useRequest((pageId: string) => _client.getContentPropertyValue(pageId, propAlias, getCrowdfundingCookie()), {
    manual: true,
    ready: open && !!propAlias,
    onSuccess: data => {
      state.propertyRes = data;
      onLoad(data)

      state.videoUrl = data.nested?.items?.flatMap(item => item.properties)?.find(p => p?.type === PropertyType.TextBox)?.textBox?.value || ""
    }
  });

  const {runAsync: updateProperty,} = useRequest((req: ContentPropertyReq, pageId: string) => _client.updateProperty(pageId, getCrowdfundingCookie(),req), {
    manual: true,
    onSuccess: () => {
      onClose();
      window.location.reload();
    }
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

      const uploadedFiles = state.propertyRes?.nested?.items?.filter(i => i.contentTypeAlias?.includes('HeroImage'))?.flatMap(i => i.properties?.map(p => ({ image: p.cropper?.image, storageToken: p.cropper?.storageToken }))) || [];

    try {

      await Promise.all(uploadedFiles.map(async f => {
        const response = await fetch(`${HostURL}${f?.image?.src}`);
    
        if (!response.ok) {
          return
        }
      
        const blob = await response.blob();

        const file = {
            id: f?.image?.mediaId,
            name: f?.image?.filename as string,
            type: blob.type,
            data: blob,
        }

        const fileId = state.uppy?.addFile(file);
        setFiles(prev => [...prev, { crop: f?.image?.crops ? f?.image?.crops[0]: {}, token: f?.storageToken, id: fileId }])
        state.uppy?.setFileState(fileId as string, {
            aspectRatioApplied: true,
            storageToken: f?.storageToken,
            progress: {
                uploadComplete: true,
                uploadStarted: true,
                percentage: 100,
            },
            isUploaded: true,
            crop: (f?.image?.crops && f?.image?.crops[0]) || {}
        } as any);
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
    toast.error(window.themeConfig.text.crowdfunding.cropperGalleryMinimumRequired.replace("%val", dataResponse?.nested?.configuration?.minimumItems?.toString() || "1"))
    
    return;
   }

   if (dataResponse?.nested?.configuration?.maximumItems && totalItems > dataResponse?.nested?.configuration?.maximumItems) {
    toast.error(window.themeConfig.text.crowdfunding.cropperGalleryMinimumRequired.replace("%val", dataResponse?.nested?.configuration?.maximumItems?.toString() || "1"))
    
    return;
   }

   const transformCrop = crop => {
       if ('bottomLeft' in crop) {
           return crop
       }

       const transformedCrop = {
           bottomLeft: {
               x: crop.x,
               y: crop.y
           },
           topRight: {
               x: crop.x + crop.width,
               y: crop.y + crop.height
           }
       }

       return transformedCrop;
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
                ...transformCrop(file.crop)
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

const uploadImages = async () => {
    await state.uppy?.upload()
}

  const handleClose = () => {
      onClose();
      setFiles([]);
  }

  const handleUplodedFile = React.useCallback((token: string, id, crop) => {
      
    setFiles(prev => [...prev, {crop, token, id}])
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
     onOk={uploadImages}
     onClose={handleClose}
     oKButtonProps={{
        disabled: loading
     }}
     okText={window.themeConfig.text.crowdfunding.upload }
     closeText={window.themeConfig.text.crowdfunding.close}
    >
        {loading ? <p className="n3o-p">{window.themeConfig.text.crowdfunding.apiLoading}</p> : <>
        <h3 className="n3o-h3">{dataResponse?.nested?.configuration?.description}</h3>
        <ImageUploader 
          onFileUpload={handleUplodedFile}
          setUppyInstance={setUppyInstance}
          aspectRatio={4/3}
          maxFiles={Number.isInteger(maxFiles) ? Number(maxFiles) : NaN}
          minFiles={1}
          elementId='campaign-cover'
          uploadUrl={`${HostURL}${ImageUploadStoragePath}`}
          hideUploadButton={true}
          dataConfig={{
            width: (dataResponse?.nested?.configuration as any)?.rectangle?.width,
            height: (dataResponse?.nested?.configuration as any)?.rectangle?.height
          }}
          onFileRemove={file => {
             setFiles(prevFiles => {
              return prevFiles.filter(f => f.id !== file?.id)
             })
          }}
          onUploadsComplete={saveContent}
        />
        </>}

        {(!loading && hasVideoUrlContent) && <div className="n3o-edit__content">
            <h3 className="n3o-h3">{window.themeConfig.text.crowdfunding.campaignVideo}</h3>
            <div className="n3o-input__outer">
              <p className="n3o-p">{window.themeConfig.text.crowdfunding.CampaignVideoUrl}</p>
              <div className="n3o-input">
                <input
                  type="text"
                  value={state.videoUrl}
                  onChange={e => state.videoUrl = e.target.value}
                  placeholder={window.themeConfig.text.crowdfunding.CampaignVideoUrlPlaceholder}
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