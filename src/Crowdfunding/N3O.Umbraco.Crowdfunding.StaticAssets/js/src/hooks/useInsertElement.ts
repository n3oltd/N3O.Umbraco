import { useRef, useEffect, MutableRefObject } from 'react';
import { useEventListener } from 'ahooks';
import { handleModalClick } from '../helpers/js/modals';
import { usePageData } from './usePageData';

type UseInsertElementHook = (
  containerId: string,
  options: useInsertElementOptions
) => MutableRefObject<HTMLDivElement | null>;

type useInsertElementOptions = {
  dataModId: string,
  svgIconName: string,
  editText: string
}


export const useInsertElement: UseInsertElementHook = (containerId, options: useInsertElementOptions) => {
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


        elementRef.current = div;
      }
    }

    return () => {
      if (elementRef.current) {
        elementRef.current.removeEventListener('click', (e) => handleModalClick(e, div));

        elementRef.current.remove();
      }
    };
  }, [containerId, options, isPageEditable]);

  return elementRef;
};