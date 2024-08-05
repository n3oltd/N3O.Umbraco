import { CampaignMeta } from './editors/CampaignMeta'
import { CampaignCover } from './editors/CampaignCover'
import { EditCampaignGoal } from './editors/EditCampaign';
import { RichTextEditor } from './editors/RichTextEditor'
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
        <CampaignMeta />
        <CampaignCover />
      </div>
    </>
  )
}

export default App
