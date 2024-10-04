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
    "cancel": "Cancel",
    "save": "Save",
    "remove": "Remove",
    "searchHerePlaceholder": "Type to search...",
    "selectDate": "Select Date",
    "editGoalTitle": "Edit Goal",
    "editGoalDescription": "You can modify and/or add projects to your page here.",
    "addProject": "Add Another Project",
    "totalGoalAmountError": "Goals total amount should be atleast",
    "minimunAmountNote": "Note: Minimum amount is",
    "amountMultipleOf": "must have amount multiple of",
    "raiseGoalAmount": "I want to raise",
    "goalAmountRequired": "Amount is required",
    "goalSelectDimension1": "Select Dimension 1",
    "goalSelectDimension2": "Select Dimension 2",
    "goalSelectDimension3": "Select Dimension 3",
    "goalSelectDimension4": "Select Dimension 4",
    "goalSelectDimension": "Select Dimension",
    "goalProjectRequired": "Please select the project",
    "goalCustomFieldRequired": "%name is Required",
    "apiLoadingError": "Unable to load. Please try again",
    "apiLoadingSuccess": "Loaded Successfully",
    "apiLoading": "Loading...",
    "apiUpdatingError": "Unable to update. Please try again",
    "apiUpdatingSuccess": "Updated Successfully",
    "apiUpdating": "Updating...",
    "cropperImageCropRequired": "Please edit each image to apply a required crop.",
    "cropperImageRequired": "'Please first upload image(s)'.",
    "cropperImageLoadError": "Unable to load the image. Please try again",
    "cropperGalleryMinimunRequired": "Atleast add %val item(s)",
    "cropperGalleryMaximumRequired": "Only Max %val items are allowed",
    "richTextEditorNote": "Write up to %val characters",
    "textAreaEditorNote": "Slightly longer text that will appear after the campaign name. You can write up to %val characters.",
    "textAreaEditorPlaceholder": "Type your message here",
    "textAreaEditorTtile": "Short Description (Optional)",
    "textEditorPlaceholder": "E.g. Building a school",
    "suggestSlugError": "Failed to fetch suggested Slug",
    "createFundraiserError": "Failed to create fundraiser",
    "campaignGoalOptionsError": "Failed to fetch campaign goal options",
    "genericError": "Sorry, an error has occurred. Please try again or contact support!",
    "tryAgainError": "Something went wrong. Please try again"
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