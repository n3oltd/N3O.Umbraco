import React from 'react';

import { useReactive } from 'ahooks';

import { usePageData } from './hooks/usePageData';

import { Components, getComponentForPropType } from './editors';
import { PropConfig } from './common/types/propConfig';

import './App.css';

declare global {
  interface Window { __openEditor: unknown; }
}

function App() {

  const state = useReactive<PropConfig>({
    selector: "",    
    propType: undefined,
    propAlias: "",
    nested: false,
    isOpen: false
  })

  const [, forceUpdate] = React.useState(false);

  window.__openEditor = (config: PropConfig) => {
    state.isOpen = config.isOpen,
    state.propType = config.propType;
    state.propAlias = config.propAlias;
    forceUpdate(prev => !prev);

  };
  
  const {isPageEditable} = usePageData();

  if(!isPageEditable){
    return
  }

  const onClose = () => {
    state.isOpen = false;
  }

  const Component = getComponentForPropType(state.propType, Components, state.nested);

  if (!Component) {
    return null
  }

  return (
    <>
      <div className='modals'>
        <Component open={state.isOpen} onClose={onClose} {...state} />
      </div>
    </>
  )
}

export default App
