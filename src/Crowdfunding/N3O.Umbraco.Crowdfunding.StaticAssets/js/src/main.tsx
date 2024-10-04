import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import { PropertyType } from '@n3oltd/umbraco-crowdfunding-client'

ReactDOM.createRoot(document.getElementById('cf-container')!).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
)

if (import.meta.env.DEV) {
  window.themeConfig.text.crowdfunding = {
    "addProject": "Add Another Project",
    "amountMultipleOf": "Amount must be a multiple of",
    "apiLoading": "Loading...",
    "apiLoadingError": "Unable to load. Please try again.",
    "apiLoadingSuccess": "Loaded successfully",
    "apiUpdating": "Updating...",
    "apiUpdatingError": "Unable to update. Please try again.",
    "apiUpdatingSuccess": "Updated successfully",
    "campaignGoalOptionsError": "Failed to fetch campaign goal options",
    "cancel": "Cancel",
    "createFundraiserError": "Failed to create fundraiser",
    "cropperGalleryMaximumRequired": "Only %val item(s) are allowed",
    "cropperGalleryMinimumRequired": "At least %val item(s) are required",
    "cropperImageCropRequired": "Please edit each image to apply a crop.",
    "cropperImageLoadError": "Unable to load the image. Please try again.",
    "cropperImageRequired": "'lease first upload image(s).",
    "editGoalDescription": "You can modify and/or add projects to your page here.",
    "editGoalTitle": "Edit Goal",
    "genericError": "Sorry, an error has occurred. Please try again.",
    "goalAmountRequired": "Amount is required",
    "goalCustomFieldRequired": "%name is Required",
    "goalProjectRequired": "Please select the project",
    "goalSelectDimension": "Select Dimension",
    "goalSelectDimension1": "Select Dimension 1",
    "goalSelectDimension2": "Select Dimension 2",
    "goalSelectDimension3": "Select Dimension 3",
    "goalSelectDimension4": "Select Dimension 4",
    "minimumAmountNote": "Note: minimum amount is",
    "raiseGoalAmount": "I hope to raise",
    "remove": "Remove",
    "richTextEditorNote": "Maximum %val characters",
    "save": "Save",
    "searchHerePlaceholder": "Type to search...",
    "SelectDate": "Select date",
    "suggestSlugError": "Failed to fetch suggested URL",
    "textAreaEditorNote": "Maximum %val characters.",
    "textAreaEditorPlaceholder": "Enter text here",
    "textAreaEditorTitle": "Text",
    "textEditorPlaceholder": "Enter text",
    "totalGoalAmountError": "Goals total amount should be at least",
    "tryAgainError": "Something went wrong. Please try again."
  }
}


if (import.meta.env.PROD) {
  console.log('running in prod mode')

  const pageContainer: HTMLDivElement | null = document.querySelector('[data-page-id]');
  

  if (pageContainer) {
   const {pageId, pageMode}  = pageContainer.dataset;

   const isPageEditable = !!pageId && pageMode === 'edit';
  
   if (isPageEditable) {
      const goalEditContainer: HTMLDivElement | null = document.querySelector('[data-goal-edit]');
      if (goalEditContainer) {
        goalEditContainer.addEventListener('click', () =>  {
            (window as any).__openGoalEditor(pageId)
        });
      }

      document.querySelectorAll('[data-type]').forEach(elm => {
        const {type, propertyAlias} = (elm as HTMLElement).dataset;
    
        const shouldAttachEditor = !!type && !!propertyAlias;
    
        if (shouldAttachEditor) {
            elm.addEventListener('click', () => {
              (window as any).__openEditor({
                propType: type,
                propAlias: propertyAlias,
                nested: PropertyType.Nested === type,
                isOpen: true
              })
            });
        }
      })
   }
  }
}