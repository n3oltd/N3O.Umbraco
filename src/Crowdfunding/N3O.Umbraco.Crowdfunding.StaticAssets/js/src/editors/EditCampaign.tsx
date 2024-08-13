import React from 'react';
import { useReactive, useRequest, useMutationObserver } from 'ahooks';
import { PagePropertyReq, PropertyType } from '@n3oltd/umbraco-crowdfunding-client';

import { ImageUploader } from './ImageUploader';
import { useInsertElement } from '../hooks/useInsertElement';
import { usePageData } from '../hooks/usePageData';
import { handleClassMutation } from '../helpers/handleClassMutation';
import { PropertyAlias } from './types/propertyAlias';
import { EDIT_TYPE } from '../common/editTypes';
import { _client } from '../common/cfClient';
import { ImageUploadStoragePath } from '../common/constants';

import './EditCampaign.css';

export const EditCampaignGoal: React.FC = () => {
  
  const [videoLink, setVideoLink] = React.useState<string>();
  const [, setFiles] = React.useState<Array<string>>([]);

  const ref = React.useRef<HTMLDivElement>(null);
  const buttonRef = React.useRef<HTMLButtonElement>(null);
  const state = useReactive({
    title: '',
    description: ''
  });

  const {pageId} = usePageData();
  const [isModalOpen, setIsModalOpen] = React.useState<boolean>();
  const [properytInfo, setProperytInfo] = React.useState<PropertyAlias>({alias: ''});

  useInsertElement(`[data-images-edit="edit-campaign-images"]`, EDIT_TYPE.images, setProperytInfo);

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
    <div className="modalsItem modall" id="edit-campaign-images" style={{overflowY: 'scroll'}}>
      <div className="edit">
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
        <div className="edit__foot">
          <button type="button" data-modal-close="true" className="button secondary" ref={buttonRef}>Cancel</button>
          <button type="button" className="button primary" onClick={saveContent}>Save</button>
        </div>
      </div>
    </div>
  </>
}