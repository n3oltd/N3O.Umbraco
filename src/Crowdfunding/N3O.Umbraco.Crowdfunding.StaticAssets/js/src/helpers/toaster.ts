import toast from "react-hot-toast"

export const loadingToast = (func: Promise<any>) => {
  toast.promise(func, {
    error: window.themeConfig.text.crowdfunding.apiLoadingError,
      success: window.themeConfig.text.crowdfunding.apiLoadingSuccess,
      loading: window.themeConfig.text.crowdfunding.apiLoading
  })
}

export const updatingToast = (func: Promise<any> ) => {
  toast.promise(func, {
    error: window.themeConfig.text.crowdfunding.apiUpdatingError,
    success: window.themeConfig.text.crowdfunding.apiUpdatingSuccess,
    loading: window.themeConfig.text.crowdfunding.apiUpdating
  })
}
