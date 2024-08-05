import { CKEditor } from '@ckeditor/ckeditor5-react';
import { ClassicEditor, Bold, Essentials, Italic, Paragraph, Undo, Underline, Strikethrough, Alignment, List, Link, Image, Heading,
  ImageCaption,
  ImageResize,
  ImageStyle,
  ImageUpload,
  Base64UploadAdapter,
  WordCount } from 'ckeditor5';

import 'ckeditor5/ckeditor5.css';
import React from 'react';

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

  const handleReady = (editor) => {

    const checkCharacterLimit = () => {
      const characterCount = editor.plugins.get('WordCount').characters;
      if (characterLimit && characterCount >= characterLimit) {
        // Disable input or display a warning message
        editor.editing.view.focusTracker.isFocused = false;
      }
    };

    editor.model.document.on('change:data', () => {
      checkCharacterLimit();
    });
  };

  return <>
      <CKEditor
          editor={ ClassicEditor }
          ref={editor}
          onReady={handleReady}
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
                    WordCount
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
            } 
          }
        /> 
  </>
}