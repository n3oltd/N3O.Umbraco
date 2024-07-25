export function handleModalClick(_, triggerBtn) {
  const currentBtn = triggerBtn;
  const tabId = currentBtn.getAttribute("data-mod");
  const currentTab = document.querySelector(tabId);
  if (currentBtn.classList.contains("active")) {
    currentBtn.classList.remove("active");
    document.body.classList.remove("active");
  } else if (!currentBtn.classList.contains("active")) {
      currentBtn.classList.remove("active");
      document.body.classList.remove("active");
    
    //currentBtn.classList.add('active');
    currentTab.classList.add("active");
    document.body.classList.add("active");
  }

  const closeButton = currentTab.querySelector('button[data-modal-close]');
  if (closeButton) {
    closeButton.onclick = () => {
      currentTab.classList.remove("active");
      document.body.classList.remove("active");
    }
  } 
}
