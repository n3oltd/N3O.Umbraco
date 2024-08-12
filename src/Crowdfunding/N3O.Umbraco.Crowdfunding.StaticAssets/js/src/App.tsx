import { CampaignCover } from './editors/CampaignCover'
import { EditCampaignGoal } from './editors/EditCampaign';
import { RichTextEditor } from './editors/RichTextEditor'
import { CampaignTitle } from './editors/CampaignTitle';
import { CampaignDescription } from './editors/CampaignDescription';

import { usePageData } from './hooks/usePageData';

import './App.css';

function App() {
  const {isPageEditable} = usePageData();
  
  if(!isPageEditable){
    return
  }

  return (
    <>
      <div className='modals'>
        <EditCampaignGoal  />
        <RichTextEditor />
        <CampaignTitle />
        <CampaignDescription />
        <CampaignCover />
      </div>
    </>
  )
}

export default App
