export const CampaignMeta: React.FC = () => {
  return <>
    <div className="modalsItem modall" id="edit-campaign-title" style={{overflowY: 'scroll'}}>
      <div className="edit__wrapper">
        <div className="edit">
          <h3>Campaign Name</h3>
          <div className="input big">
            <input type="text" placeholder="E.g. Building a school" />
          </div>
          <div className="edit__content">
            <div className="input__outer dark">
              <p>Short Description (Optional)</p>
              <div className="input">
                <textarea
                  name=""
                  id=""
                  rows={3}
                  placeholder="Type your message here"
                ></textarea>
              </div>
            </div>
            <p className="subtle">
              Slightly longer text that will appear after the campaign name. You
              can write up to 160 characters.
            </p>
          </div>
          <div className="edit__foot">
            <button type="button" data-modal-close="true" className="button secondary">Cancel</button>
            <button type="button" className="button primary">Save</button>
          </div>
        </div>
      </div>
    </div>
  </>
}