export type PageData = {
    pageId: number | undefined,
    pageMode: string | undefined
}

export type PageDataEditable = {
  isPageEditable: boolean
} & PageData