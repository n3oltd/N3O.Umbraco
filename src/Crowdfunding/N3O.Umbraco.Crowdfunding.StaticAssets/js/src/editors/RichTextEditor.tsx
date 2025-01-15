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
import { getCrowdfundingCookie } from "../common/cookie";

export const RichTextEditor: React.FC<EditorProps> = ({
  open,
  propAlias,
  onClose
}) => {
  
  const editor = React.useRef<CKEditor<ClassicEditor> | null>(null);

  const {pageId} = usePageData();
  const [editorContent, setEditorContent] =  React.useState<string>('');

  const {runAsync: loadPropertyValue, data: dataResponse, loading} = useRequest((pageId: string) => _client.getContentPropertyValue(pageId, propAlias, getCrowdfundingCookie()), {
    manual: true,
    ready: open && !!propAlias,
    onSuccess: data => {
      editor.current?.editor?.setData(data?.raw?.value as string)
      setEditorContent(data?.raw?.value as string)
    }
  });

    const { runAsync: updateProperty, loading: updating } = useRequest((req: ContentPropertyReq, pageId) => _client.updateProperty(pageId, getCrowdfundingCookie(), req), {
    manual: true,
    onSuccess: () => {
      onClose();
      window.location.reload()
    }
  })
  
  React.useEffect(() => {
    if (open && pageId) {
     loadingToast(loadPropertyValue(pageId as string))
    }
  }, [loadPropertyValue, pageId, open]);

  
  const saveContent = async () => {

    try {
      const req: ContentPropertyReq = {
        alias: propAlias,
        type: PropertyType.Raw,
        raw: {
          value: editor.current?.editor?.getData() || ''
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
      oKButtonProps={{
        disabled: loading || updating
      }}
    >
      {loading ? <p className="n3o-p">{window.themeConfig.text.crowdfunding.apiLoading}</p> : <>
        <h3 className="n3o-h3">{dataResponse?.raw?.configuration?.description}</h3>
          <div className="n3o-edit__info">
            <div className="n3o-detail">{window.themeConfig.text.crowdfunding.richTextEditorNote.replace("%val", dataResponse?.raw?.configuration?.maximumLength?.toString() || "100")}</div>
          </div>
          <div className="n3o-richText" style={{paddingTop: '24px'}}>
              <CkEditor 
                editor={editor}
                initialContent={editorContent}
                characterLimit={dataResponse?.raw?.configuration?.maximumLength}
              />
          </div>
        </>}
    </Modal>
  </>
}
