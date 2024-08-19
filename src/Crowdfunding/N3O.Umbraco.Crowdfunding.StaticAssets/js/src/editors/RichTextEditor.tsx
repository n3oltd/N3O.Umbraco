import React from "react";

import { useRequest } from "ahooks";
import { ClassicEditor } from "ckeditor5";
import { CKEditor } from "@ckeditor/ckeditor5-react";
import { ContentPropertyReq, PropertyType } from "@n3oltd/umbraco-crowdfunding-client";

import { Modal } from "./common/Modal";
import { CkEditor } from "./common/CKEditor";
import { loadingToast, updatingToast } from "../helpers/toaster";

import { usePageData } from "../hooks/usePageData";

import { _client } from "../common/cfClient";
import { EditorProps } from "./types/EditorProps";

import './RichTextEditor.css'

export const RichTextEditor: React.FC<EditorProps> = ({
  open,
  propAlias,
  onClose
}) => {
  
  const editor = React.useRef<CKEditor<ClassicEditor> | null>(null);

  const {pageId} = usePageData();
  const [editorContent, setEditorContent] =  React.useState<string>('');

  const {runAsync: loadPropertyValue} = useRequest((pageId: string) => _client.getContentPropertyValue(pageId, propAlias), {
    manual: true,
    ready: open && !!propAlias,
    onSuccess: data => editor.current?.editor?.setData(data?.raw?.value as string)
  });

  const {runAsync: updateProperty,} = useRequest((req: ContentPropertyReq, pageId) => _client.updateProperty(pageId, req), {
    manual: true,
    onSuccess: () => {
      onClose()
    }
  })

  React.useEffect(() => {
    if (open && pageId) {
     loadingToast(loadPropertyValue(pageId as string))
    }
  }, [loadPropertyValue, pageId, open]);

  const handleContentChange = React.useCallback(content => {
    setEditorContent(content);
  }, [setEditorContent]);

  const saveContent = async () => {

    try {
      const req: ContentPropertyReq = {
        alias: propAlias,
        type: PropertyType.Raw,
        raw: {
          value: editorContent
        } 
      }

      updatingToast(updateProperty(req, pageId as string))
    } catch(e) {
      console.error(e)
    }
  }

  return <>
    <Modal
      id="raw-edit"
      isOpen={open}
      onOk={saveContent}
      onClose={onClose}
    >
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
    </Modal>
  </>
}
