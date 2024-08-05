export type PageData = {
    pageId: string | undefined,
    pageMode: string | undefined
}

export type PageDataEditable = {
  isPageEditable: boolean
} & PageData