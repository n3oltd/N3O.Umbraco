import React from 'react'
import { Meta, Uppy, UppyFile } from '@uppy/core';
import { Dashboard as ReactDashboard} from '@uppy/react';
import ImageEditor from '@uppy/image-editor';
import XHR from '@uppy/xhr-upload';

import '@uppy/core/dist/style.min.css';
import '@uppy/dashboard/dist/style.min.css';
import '@uppy/image-editor/dist/style.min.css';
import '@uppy/drag-drop/dist/style.min.css';

type ImageUploaderProps = {
  onFileUpload: (fileUploadResponse: string, crop?: any) => void,
  onFileAdded?: (event: UppyFile<Meta, Record<string, never>>) => void,
  onCrop?: (event: CustomEvent<any>) => void;
  setUppyInstance?: (uppy: Uppy) => void;
  maxFiles: number,
  minFiles?: number,
  aspectRatio: number,
  uploadUrl: string,
  elementId: string,
  hieght?: number,
  crop?: any,
  openEditor?: boolean
}

const handleCrop = event => {
  return  {
    bottomLeft: {
      x: Math.floor(event.detail.x),
      y: Math.floor(event.detail.y) 
    },
    topRight: {
      x: Math.floor(event.detail.width),
      y: Math.floor(event.detail.height)
    }
  }

}

export const ImageUploader: React.FC<ImageUploaderProps> = ({
  maxFiles,
  onFileUpload,
  onFileAdded,
  onCrop,
  aspectRatio,
  setUppyInstance,
  uploadUrl,
  minFiles = 1,
  openEditor = true,
  elementId = 'uppy',
  hieght = 550
}) => {

  const filesCropInfo = React.useRef<Array<{file?: any, crop?: any, orignalCrop?: any}>>([]);

  const [uppy] = React.useState(() => {
    const uppy = new Uppy({
      id: elementId,
    
      restrictions: {
        minNumberOfFiles: minFiles,
        maxNumberOfFiles: maxFiles,
        allowedFileTypes: ['image/*'],
        
      },
      onBeforeUpload(files) {
        const filesArray = Object.entries(files);

          const validFiles = filesArray.filter(([, file]) => {
            if ((file as any).aspectRatioApplied) return true;
            uppy.info({ message: window.themeConfig.text.crowdfunding.cropperImageCropRequired }, "error");
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
        responsive: true,
        autoCrop: true,
        movable: false,
        crop: (event: any) => {
          onCrop?.(event)
          const filePosition = filesCropInfo.current.findIndex(f => f.file.name.includes(event.srcElement.alt));
          filesCropInfo.current[filePosition].crop = handleCrop(event)
          filesCropInfo.current[filePosition].orignalCrop = event.detail
        }
          
      }
    })
    .use(XHR, { endpoint: uploadUrl});

    uppy.on('upload-success', (file: any, response) => {
      const body = response.body as unknown;
      const cropInfo = filesCropInfo.current.find(f => file?.id === f.file.id)
      onFileUpload(body as string, cropInfo?.crop || file?.crop)
    });

    uppy.on("file-added", e => {
      onFileAdded?.(e)
    })

    uppy.on('file-editor:start', (file: any) => {
 
      const existingFile = filesCropInfo.current.find(f => f.file.id === file.id);
      const editorPlugin = uppy.getPlugin<any>('ImageEditor');
      
      if (!existingFile) {
        filesCropInfo.current.push({file})
      }

      if (existingFile &&  editorPlugin?.cropper) {
        const cropper = editorPlugin?.cropper;
        cropper.setData(existingFile.orignalCrop);
      }

      if (file.crop &&  editorPlugin?.cropper) {
        editorPlugin?.cropper?.setData(file.crop);
        uppy.setFileState(file.id, {
          
        })
      }
    });

    uppy.on('file-editor:complete', (updatedFile: any) => {
      updatedFile.aspectRatioApplied = true;
      const originalFile = filesCropInfo.current.find(f => f.file.id === updatedFile.id);
      
      if (originalFile) {
        uppy.setFileState(originalFile?.file.id, {
          ...originalFile.file,
          data: originalFile.file.data, // Retain original image data
          preview: originalFile.file.preview, // Retain original preview
        })  
      }
    });

    uppy.on('file-editor:cancel', (file) => {
      filesCropInfo.current = filesCropInfo.current.filter(f => f.file.id !== file.id)
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
      autoOpen={openEditor ? 'imageEditor' :  null}
      uppy={uppy}
      height={hieght} 
      id={elementId}
      proudlyDisplayPoweredByUppy={false}/>
  </>
}