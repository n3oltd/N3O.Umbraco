import React from 'react';
import { ImageUploader } from './ImageUploader';
import './EditCampaign.css';
 
export const EditCampaignGoal: React.FC = () => {

  const [videoLink, setVideoLink] = React.useState<string>();
  const [, setFiles] = React.useState<Array<string>>([]);

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
          maxFiles={3}
          uploaderId='campaign-cover'
          uploadUrl='https://n3oltd.n3o.cloud/umbraco/api/Storage/tempUpload'
          />
        <div className='image-container'>
          <div className='image-wrapper'>
            <img src="" style={{
              width: '100%',
              objectFit: 'contain',
              borderRadius: '10px'
            }} />
             <svg width="20" height="20" viewBox="0 0 24 24" className="delete-icon">
              <rect x="5" y="6" width="14" height="12" rx="1" fill="#fff" /> {/* White rectangle background */}
              <path
                d="M11 8l6 6-6 6"
                stroke="#000" /* Black color for X */
                strokeWidth="2"
                strokeLinecap="round"
                strokeLinejoin="round"
              /> {/* Black X symbol */}
            </svg>
          </div>
        </div>  
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
          <button type="button" data-modal-close="true" className="button secondary">Cancel</button>
          <button type="button" className="button primary">Save</button>
        </div>
      </div>
    </div>
  </>
}