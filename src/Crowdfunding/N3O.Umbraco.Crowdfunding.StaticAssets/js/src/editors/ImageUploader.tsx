import React from 'react'
import { Uppy } from '@uppy/core';
import { Dashboard as ReactDashboard} from '@uppy/react';
import ImageEditor from '@uppy/image-editor';
import XHR from '@uppy/xhr-upload';

import '@uppy/core/dist/style.min.css';
import '@uppy/dashboard/dist/style.min.css';
import '@uppy/image-editor/dist/style.min.css';
import '@uppy/drag-drop/dist/style.min.css';

type ImageUploaderProps = {
  onFileUpload: (fileUploadResponse: string, uppy: Uppy) => void,
  onCrop: (event: CustomEvent<any>) => void;
  setUppyInstance?: (uppy: Uppy) => void;
  maxFiles: number,
  aspectRatio: number,
  uploadUrl: string,
  elementId: string,
  hieght?: number
}

export const ImageUploader: React.FC<ImageUploaderProps> = ({
  maxFiles = 10,
  onFileUpload,
  onCrop,
  aspectRatio,
  setUppyInstance,
  uploadUrl = `https://localhost:6001/umbraco/api/Storage/tempUpload`,
  elementId = 'uppy',
  hieght = 550
}) => {

  const [uppy] = React.useState(() => {
    const uppy = new Uppy({
      id: elementId,

      restrictions: {
        minNumberOfFiles: 1,
        maxNumberOfFiles: maxFiles
      },
      onBeforeUpload(files) {
        const filesArray = Object.entries(files);

          const validFiles = filesArray.filter(([, file]) => {
            if ((file as any).aspectRatioApplied) return true;
            uppy.info({ message: "Please edit each image to apply a required crop." }, "error");
            return false;
          });

          return validFiles.length === filesArray.length;
      },
    })    
    .use(ImageEditor, {
      actions: {
        revert: false,
        rotate: false,
        granularRotate: false,
        flip: false,
        zoomIn: false,
        zoomOut: false,
        cropSquare: false,
        cropWidescreen: false,
        cropWidescreenVertical: false,
      },
      cropperOptions: {
        aspectRatio: aspectRatio,
        autoCrop: true,
        crop: onCrop,
      }
    })
    .use(XHR, { endpoint: uploadUrl});

    uppy.on('upload-success', (_, response) => {
      const body = response.body as unknown;
      console.log(_, response)
      onFileUpload(body as string, uppy)
    });

    uppy.on('file-editor:complete', (updatedFile: any) => {
      updatedFile.aspectRatioApplied = true;
      
    });

    return uppy;
  })

  React.useEffect(() => {
    if (setUppyInstance) {
      setUppyInstance(uppy);
    }
  }, [uppy, setUppyInstance])
  
  return <>
    <ReactDashboard 
      uppy={uppy}
      height={hieght} 
      id={elementId}
      proudlyDisplayPoweredByUppy={false}/>
  </>
}