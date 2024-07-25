import { CKEditor } from '@ckeditor/ckeditor5-react';
import { ClassicEditor, Bold, Essentials, Italic, Paragraph, Undo, Underline, Strikethrough, Alignment, List, Link, Image,
  ImageCaption,
  ImageResize,
  ImageStyle,
  ImageUpload, Base64UploadAdapter } from 'ckeditor5';

import 'ckeditor5/ckeditor5.css';
import React from 'react';

type CKEditorProps = {
  onChange: (content: string) => void,
  initialContent: string
}

export const CkEditor: React.FC<CKEditorProps> = ({
  
  onChange,
  initialContent
}) => {

  return <>
      <CKEditor
            editor={ ClassicEditor }
            onChange={(_, e) => onChange(e.getData())}
            config={ {
                toolbar: {
                    items: [ 
                      'undo', 'redo', '|', 
                      'bold', 'italic', 'underline', 'strikethrough' , '|', 
                      'alignment', '|',
                      'bulletedList', 'numberedList', 'link', 'uploadImage'
                     ],
                },
                plugins: [
                     Essentials, Bold, Italic, Paragraph, Undo, Underline, Strikethrough, Alignment, List, ImageUpload ,Link, Image, 
                     ImageCaption,
                     ImageResize,
                     ImageStyle,
                     Base64UploadAdapter
                ],
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
            } }
        /> 
  </>
}