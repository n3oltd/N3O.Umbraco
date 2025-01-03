import React from 'react';
import { CKEditor } from '@ckeditor/ckeditor5-react';
import { ClassicEditor, Bold, Essentials, Italic, Paragraph, Undo, Underline, Strikethrough, Alignment, List, Link, Image, Heading,
  ImageCaption,
  ImageResize,
  ImageStyle,
  ImageUpload,
  Base64UploadAdapter,
  WordCount } from 'ckeditor5';
import { CharacterLimit } from './plugins/CKCharacterLimit';

import 'ckeditor5/ckeditor5.css';

type CKEditorProps = {
  editor: React.MutableRefObject<CKEditor<ClassicEditor> | null>,
  onChange: (content: string) => void,
  initialContent: string,
  characterLimit?: number
}

export const CkEditor: React.FC<CKEditorProps> = ({
  editor,
  onChange,
  initialContent,
  characterLimit
}) => {

  
  return <>
      <CKEditor
          editor={ ClassicEditor }
          ref={editor}
          onChange={(_, e) => onChange(e.getData())}
          config={
              {
                wordCount: {
                  displayCharacters: true
                },
              toolbar: {
                  items: [ 
                    'undo', 'redo', '|', 
                    'heading', '|',
                    'bold', 'italic', 'underline', 'strikethrough' , '|', 
                    'alignment', '|',
                    'bulletedList', 'numberedList', 'link', 'uploadImage'
                    ],
              },
              plugins: [
                    Essentials, Bold, Italic, Paragraph, Undo, Underline, Strikethrough, Alignment, List, ImageUpload ,Link, Image, Heading,
                    ImageCaption,
                    ImageResize,
                    ImageStyle,
                    Base64UploadAdapter,
                    WordCount,
                    CharacterLimit
              ],
              characterLimit: {
                    limit: characterLimit
              },
              link: {
                addTargetToExternalLinks: true,
                defaultProtocol: 'https://',
              },
              image: {
                resizeOptions: [
                  {
                    name: 'resizeImage:original',
                    label: 'Default image width',
                    value: null,
                  },
                  {
                    name: 'resizeImage:50',
                    label: '50% page width',
                    value: '50',
                  },
                  {
                    name: 'resizeImage:75',
                    label: '75% page width',
                    value: '75',
                  },
                ],
                toolbar: [
                  'imageTextAlternative',
                  'toggleImageCaption',
                  '|',
                  'imageStyle:inline',
                  'imageStyle:wrapText',
                  'imageStyle:breakText',
                  '|',
                  'resizeImage',
                ],
              },
              initialData: initialContent,
            } as any
          }
        /> 
  </>
}