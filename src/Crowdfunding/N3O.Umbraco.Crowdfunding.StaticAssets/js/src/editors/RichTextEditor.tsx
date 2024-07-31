import React from "react";
import { useMutationObserver, useRequest } from "ahooks";
import { ClassicEditor } from "ckeditor5";
import { CKEditor } from "@ckeditor/ckeditor5-react";

import { PagePropertyReq, PropertyType } from "@n3oltd/umbraco-crowdfunding-client";

import { CkEditor } from "./CKEditor";
import { usePageData } from "../hooks/usePageData";
import { useInsertElement } from "../hooks/useInsertElement";
import { handleClassMutation } from "../helpers/handleClassMutation";
import { PropertyAlias } from "./types/propertyAlias";
import { EDIT_TYPE } from "../common/editTypes";
import { _client } from "../common/cfClient";

import './RichTextEditor.css'

export const RichTextEditor: React.FC = () => {
  
  const editor = React.useRef<CKEditor<ClassicEditor> | null>(null);
  const ref = React.useRef<HTMLDivElement>(null);
  const buttonRef = React.useRef<HTMLButtonElement>(null);

  const {pageId} = usePageData();
  const [editorContent, setEditorContent] =  React.useState<string>('');
  const [isModalOpen, setIsModalOpen] = React.useState<boolean>();
  const [properytInfo, setProperytInfo] = React.useState<PropertyAlias>({alias: ''});

  useInsertElement(`[data-about-edit="edit-campaign-about"]`, EDIT_TYPE.about, setProperytInfo);

  const {run: loadPropertyValue} = useRequest(() => _client.getPagePropertyValue(pageId as string, properytInfo.alias), {
    manual: true,
    ready: !!pageId && isModalOpen && !!properytInfo.alias,
    onSuccess: data => setEditorContent(data?.textarea?.value as string)
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
      setEditorContent('');
      editor.current?.editor?.setData('')
    }
  }, [loadPropertyValue, setEditorContent, isModalOpen, editor]);

  const handleContentChange = React.useCallback(content => {
    setEditorContent(content);
  }, [setEditorContent]);

  const saveContent = async () => {
    const content = editor.current?.editor?.getData()
    console.log(content) 

    try {
      const req: PagePropertyReq = {
        alias: properytInfo.alias,
        type: PropertyType.Textarea,
        textarea: {
          value: editorContent
        } 
      }

      await updateProperty(req)
    } catch(e) {
      console.error(e)
    }
  }

  return <>
    <div className="modalsItem modall" ref={ref} id="edit-campaign-info" style={{overflowY: 'scroll'}}>
      <div className="edit__wrapper" style={{margin: 'auto'}}>
        <div className="edit">
          <h3>About this campaign</h3>
          <div className="edit__info">
            <div className="detail">Write up to 1600 characters</div>
          </div>
          <div className="richText" style={{paddingTop: '24px'}}>
              <CkEditor 
                editor={editor}
                onChange={handleContentChange}
                initialContent={editorContent}
                characterLimit={1600}
              />
          </div>
          <div className="edit__foot">
            <button type="button" className="button secondary" data-modal-close="true" ref={buttonRef}>Cancel</button>
            <button type="button" className="button primary" onClick={saveContent}>Save</button>
          </div>
        </div>
      </div>
    </div>
  </>
}
