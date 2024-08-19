import React from 'react';

import { useReactive, useRequest } from 'ahooks';
import { ContentPropertyReq, PropertyType, ContentPropertyValueRes } from '@n3oltd/umbraco-crowdfunding-client';

import { ImageUploader } from './common/ImageUploader';
import { Modal } from './common/Modal';

import { usePageData } from '../hooks/usePageData';

import { _client } from '../common/cfClient';
import { EditorProps } from './types/EditorProps';
import { ImageUploadStoragePath } from '../common/constants';

import './CropperNested.css';

export const CropperNested: React.FC<EditorProps> = ({
  open,
  propAlias,
  onClose
}) => {
  
  const [, setFiles] = React.useState<Array<string>>([]);

  const state = useReactive<{
    propertyRes: ContentPropertyValueRes | undefined
  }>({
    propertyRes: undefined,
    
  });

  const {pageId} = usePageData();

  const {run: loadPropertyValue} = useRequest((pageId: string) => _client.getContentPropertyValue(pageId, propAlias), {
    manual: true,
    ready: open && !!propAlias,
    onSuccess: data => state.propertyRes = data
  });

  const {runAsync: updateProperty,} = useRequest((req: ContentPropertyReq, pageId: string) => _client.updateProperty(pageId, req), {
    manual: true,
    onSuccess: () => onClose
  })

  React.useEffect(() => {
    if (open) {
      loadPropertyValue(pageId as string)
    }
  }, [loadPropertyValue,pageId, open]);

  const saveContent = async () => {
    try {
      const req: ContentPropertyReq = {
        alias: propAlias,
        type: PropertyType.Nested,
        nested: {
          items: []
        }
      }

      await updateProperty(req, pageId as string)
    } catch(e) {
      console.error(e)
    }
  }

  const handleUplodedFile = React.useCallback((file: string) => {
    setFiles(prev => [...prev, file])
  }, [setFiles]);

  return <>
    <Modal
      id="cropper-nested-edit"
      isOpen={open}
      onOk={saveContent}
      onClose={onClose}
    >
        <h3>Upload Campaign Images</h3>
        <ImageUploader 
          aspectRatio={4/3} 
          onFileUpload={handleUplodedFile}
          onCrop={console.debug}
          maxFiles={3}
          elementId='campaign-cover'
          uploadUrl={`https://localhost:6001${ImageUploadStoragePath}`}
          />
        
      </Modal>
  </>
}