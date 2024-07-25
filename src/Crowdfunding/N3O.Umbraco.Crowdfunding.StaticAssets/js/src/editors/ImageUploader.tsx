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
  onFileUpload: (fileUploadResponse: string) => void,
  maxFiles: number,
  aspectRatio: number,
  uploadUrl: string,
  uploaderId: string,
  hieght?: number
}

export const ImageUploader: React.FC<ImageUploaderProps> = ({
  maxFiles = 10,
  onFileUpload,
  aspectRatio,
  uploadUrl = 'https://n3oltd.n3o.cloud/umbraco/api/Storage/tempUpload',
  uploaderId = 'uppy',
  hieght = 550
}) => {

  const [uppy] = React.useState(() => {
    const uppy = new Uppy({
      id: uploaderId,
      restrictions: {
        minNumberOfFiles: 1,
        maxNumberOfFiles: maxFiles
      }
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
        aspectRatio: aspectRatio 
      }
    })
    .use(XHR, { endpoint: uploadUrl});

    uppy.on('upload-success', (_, response) => {
      const body = response.body as unknown;
      onFileUpload(body as string)
    });

    return uppy;
  })
  
  return <>
    <ReactDashboard 
      uppy={uppy}
      height={hieght} 
      id={uploaderId}
      proudlyDisplayPoweredByUppy={false}/>
  </>
}