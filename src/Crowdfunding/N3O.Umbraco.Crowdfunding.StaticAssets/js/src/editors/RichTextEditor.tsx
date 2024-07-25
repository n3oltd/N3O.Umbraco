import React from "react";

import { CkEditor } from "./CKEditor";
import './RichTextEditor.css'

export const RichTextEditor: React.FC = () => {
  const [editorContent, setEditorContent] =  React.useState<string>('')
  
  const handleContentChange = React.useCallback(content => {
    setEditorContent(content);
  }, [setEditorContent]);

  const saveContent = () => {
    console.log(editorContent) 
  }

  return <>
    <div className="modalsItem modall" id="edit-campaign-info" style={{overflowY: 'scroll'}}>
      <div className="edit__wrapper">
        <div className="edit">
          <h3>About this campaign</h3>
          <div className="edit__info">
            <div className="detail">Write up to 1600 characters</div>
          </div>
          <div className="richText" style={{paddingTop: '24px'}}>
              <CkEditor 
                onChange={handleContentChange}
                initialContent={editorContent}
              />
          </div>
          <div className="edit__foot">
            <button type="button" className="button secondary" data-modal-close="true">Cancel</button>
            <button type="button" className="button primary" onClick={saveContent}>Save</button>
          </div>
        </div>
      </div>
    </div>
  </>
}