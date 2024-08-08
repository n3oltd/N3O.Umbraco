import React from "react";
import { PageData, PageDataEditable } from "../common/types/pageData";

const EDIT_MODE = 'edit';

export const usePageData = (): PageDataEditable => {
  const [pageData, setPageData] = React.useState<PageData>();

  React.useEffect(() => {
    const { pageMode, pageId } = document.body.dataset;

    const validatedPageData = {
      pageId: pageId || undefined,
      pageMode: pageMode || 'VIEW', 
    };

    setPageData(validatedPageData);
  }, []); 

  const isPageEditable = !!(pageData?.pageId && pageData?.pageMode === EDIT_MODE);

  return { isPageEditable, pageId: pageData?.pageId, pageMode: pageData?.pageMode };
};
