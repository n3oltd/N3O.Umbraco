import { CampaignMeta } from './editors/CampaignMeta'
import { CampaignCover } from './editors/CampaignCover'
import { EditCampaignGoal } from './editors/EditCampaign';
import { RichTextEditor } from './editors/RichTextEditor'
import { usePageData } from './hooks/usePageData';

import './App.css';
import { useInsertElement } from './hooks/useInsertElement';

const EDIT_TYPE = {
  cover: {
    svgIconName: '#imagePlus',
    editText: 'Edit Cover Image',
    dataModId: ''
  },
  images: {
    svgIconName: '#imagePlus',
    editText: 'Edit Media',
    dataModId: '#edit-campaign-images'
  },
  title: {
    svgIconName: '#editIcon',
    editText: 'Edit',
    dataModId: ''
  },
  about: {
    svgIconName: '#editIcon',
    editText: 'Edit',
    dataModId: ''
  }
}

function App() {
  const {isPageEditable} = usePageData();
  
  useInsertElement(`[data-cover-edit="edit-campaign-cover"]`, EDIT_TYPE.cover);
  useInsertElement(`[data-images-edit="edit-campaign-images"]`, EDIT_TYPE.images);
  useInsertElement(`[data-title-edit="edit-campaign-title"]`, EDIT_TYPE.title);
  useInsertElement(`[data-about-edit="edit-campaign-about"]`, EDIT_TYPE.about);
  
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
