import { useRef, useEffect, MutableRefObject } from 'react';
import { PropertyType } from '@n3oltd/umbraco-crowdfunding-client';
import { useEventListener } from 'ahooks';

import { handleModalClick } from '../helpers/js/modals';
import { usePageData } from './usePageData';
import { PropertyAlias } from '../editors/types/propertyAlias';

type UseInsertElementHook = (
  containerId: string,
  options: useInsertElementOptions,
  updatePropertyInfo: (info: PropertyAlias) => void
) => MutableRefObject<HTMLDivElement | null>;

type useInsertElementOptions = {
  dataModId: string,
  svgIconName: string,
  editText: string
}


export const useInsertElement: UseInsertElementHook = (containerId, options: useInsertElementOptions, updatePropertyInfo: (info: PropertyAlias) => void) => {
  const elementRef = useRef<HTMLDivElement | null>(null);
  const {isPageEditable} = usePageData()
  useEventListener('click', e => handleModalClick(e, elementRef.current), {target: elementRef})

  useEffect(() => {
    const container = document.querySelector(containerId);
    let div: HTMLDivElement;
    if (container && isPageEditable) {
      // Check if the container already has the specific element
      const existingElement = container.querySelector('.campaign__edit');
      if (existingElement) {
        elementRef.current = existingElement as HTMLDivElement;
      } else {
        div = document.createElement('div');
        div.className = 'campaign__edit modallBtn';
        div.setAttribute('data-mod', options.dataModId);

        const svg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        const use = document.createElementNS('http://www.w3.org/2000/svg', 'use');
        use.setAttributeNS('http://www.w3.org/1999/xlink', 'xlink:href', options.svgIconName);
        svg.appendChild(use);

        const bold = document.createElement('b');
        bold.textContent = options.editText;

        div.appendChild(svg);
        div.appendChild(bold);
        // Insert the new element as the first child of the container
        if (container.firstChild) {
          container.insertBefore(div, container.firstChild);
        } else {
          container.appendChild(div);
        }

        div.addEventListener('click', (e) => handleModalClick(e, div));

        const {type, propertyAlias} = (container.querySelector('[data-type]') as HTMLDivElement).dataset || {}
        
        if(type && propertyAlias) {
          updatePropertyInfo({
            type: type as PropertyType,
            alias: propertyAlias
          })
        }

        elementRef.current = div;
      }
    }

    return () => {
      if (elementRef.current) {
        elementRef.current.removeEventListener('click', (e) => handleModalClick(e, div));
        updatePropertyInfo({
          alias: '',
          type: undefined,
        })
        elementRef.current.remove();
      }
    };
  }, [containerId, options, isPageEditable, updatePropertyInfo]);

  return elementRef;
};