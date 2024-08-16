import { PropertyType } from '@n3oltd/umbraco-crowdfunding-client';

import {Textbox} from './Textbox';
import {Textarea} from './Textarea';
import {RichTextEditor} from './RichTextEditor';
import {CropperSingle} from './CropperSingle';
import {CropperNested} from './CropperNested';


export const Components = {
  [PropertyType.TextBox]: Textbox,
  [PropertyType.Textarea]: Textarea,
  [PropertyType.Raw]: RichTextEditor,
  [PropertyType.Cropper]: {
    single: CropperSingle,
    nested: CropperNested
  }
}

export const getComponentForPropType = (propType, components, nested = false) => {
  const Component = components[propType];

  if (typeof Component === 'function') {
    return Component;
  }

  if (nested && propType === PropertyType.Cropper) {
    return components[propType]['nested'];
  }
  
  if (!nested && propType === PropertyType.Cropper) {
    return components[propType]['single'];
  }


  return null
}