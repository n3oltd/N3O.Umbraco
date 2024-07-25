import { ImageUploader } from "./ImageUploader"


export const CampaignCover: React.FC = () => {
  return <>
    <div className="modalsItem modall" id="edit-campaign-cover">
      <div className="edit__wrapper">
        <div className="edit">
          <h3>Upload Cover Image</h3>
          
            <ImageUploader 
              aspectRatio={4/1}
              maxFiles={1}
              onFileUpload={console.log}
              uploaderId="campaign-cover"
              uploadUrl="https://n3oltd.n3o.cloud/umbraco/api/Storage/tempUpload"
              hieght={200}
            />
          <div className="edit__foot">
            <button type="button" data-modal-close="true" className="button secondary">Cancel</button>
            <button type="button" className="button primary">Save</button>
          </div>
        </div>
      </div>
    </div>
  </>
}