import toast from "react-hot-toast"

export const loadingToast = (func: Promise<any>) => {
  toast.promise(func, {
    error: 'Unable to load. Please try again',
    success: 'Loaded Successfully',
    loading: 'Loading...'
  })
}

export const updatingToast = (func: Promise<any> ) => {
  toast.promise(func, {
    error: 'Unable to update. Please try again',
    success: 'Updated Successfully',
    loading: 'Updating...'
  })
}
