import React from 'react';

import { useReactive, useRequest } from 'ahooks';
import { ContentPropertyReq, PropertyType, CropperSource } from '@n3oltd/umbraco-crowdfunding-client';

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
  
  const [videoLink, setVideoLink] = React.useState<string>();
  const [, setFiles] = React.useState<Array<string>>([]);

  const state = useReactive<{
    image: CropperSource | undefined
  }>({
    image: undefined,
    
  });

  const {pageId} = usePageData();

  const {run: loadPropertyValue} = useRequest(() => _client.getContentPropertyValue(pageId as string, propAlias), {
    manual: true,
    ready: open && !!propAlias,
    onSuccess: data => state.image = data?.cropper?.image
  });

  const {runAsync: updateProperty,} = useRequest((req: ContentPropertyReq) => _client.updateProperty(pageId as string, req), {
    manual: true,
    onSuccess: () => onClose
  })

  React.useEffect(() => {
    if (open) {
      loadPropertyValue()
    }
  }, [loadPropertyValue, state, open, ]);

  const saveContent = async () => {
    try {
      const req: ContentPropertyReq = {
        alias: propAlias,
        type: PropertyType.TextBox,
        cropper: {
          rectangle: {
            bottomLeft: {},
            topRight: {}
          }
        }
      }

      await updateProperty(req)
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
          uploadUrl={`${window.location.origin}${ImageUploadStoragePath}`}
          />
        {/* <div className='image-container'>
          <div className='image-wrapper'>
             <img src="" style={{
              width: '100%',
              objectFit: 'contain',
              borderRadius: '10px'
            }} />
             <svg width="20" height="20" viewBox="0 0 24 24" className="delete-icon">
              <rect x="5" y="6" width="14" height="12" rx="1" fill="#fff" />
              <path
                d="M11 8l6 6-6 6"
                stroke="#000" 
                strokeWidth="2"
                strokeLinecap="round"
                strokeLinejoin="round"
              />
            </svg>
          </div> 
        </div>   */}
        <div className="edit__content">
          <h3>Campaign Video</h3>
          <div className="input__outer">
            <p>Youtube Video's URL</p>
            <div className="input">
              <input
                type="text"
                onChange={e => setVideoLink(e.target.value)}
                placeholder="example: https://www.youtube.com/embed/tnGoQ5HcaoA"
              />
            </div>
            {videoLink && <iframe referrerPolicy="strict-origin-when-cross-origin" src={videoLink} style={{
              width: "100%",
              marginTop: "12px",
              height: '263px',
              borderRadius: "var(--corner-md, 8px)"
            }} frameBorder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"></iframe>}
          </div>
        </div>
      </Modal>
  </>
}