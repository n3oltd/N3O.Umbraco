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