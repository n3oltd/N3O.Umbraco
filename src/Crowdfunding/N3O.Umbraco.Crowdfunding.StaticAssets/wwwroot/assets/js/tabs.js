// const donationTabs = document.getElementById("donationTabs");
// if (donationTabs) {
//   myTabs = donationTabs.querySelectorAll(".donation__head-tab");
//   myTabs.forEach((myTab) => {
//     myTab.onclick = () => {
//       myTab.classList.add("active");
//       myTabs.forEach((item) => {
//         item.classList.remove("active");
//         myTab.classList.add("active");
//       });
//     };
//   });
// }
// faq start
const tabBtn = document.querySelectorAll(".tabBtn");
const tabEvent = document.querySelectorAll(".tabEvent");
tabBtn.forEach((e) => {
  onTabClick(tabBtn, tabEvent, e);
});
function onTabClick(tabBtns, tabItems, item) {
  item.addEventListener("click", function (e) {
    let currentBtn = item;
    let tabId = currentBtn.getAttribute("data-tab");
    let currentTab = document.querySelector(tabId);
    if (currentBtn.classList.contains("active")) {
      console.log("now active");
      const faq =
        currentBtn.parentElement.parentElement.querySelector(".tabEvent");
      if (faq) {
        faq.classList.remove("active");
        currentBtn.classList.remove("active");
      }
    } else if (!currentBtn.classList.contains("active")) {
      tabBtns.forEach(function (item) {
        item.classList.remove("active");
      });

      tabItems.forEach(function (item) {
        item.classList.remove("active");
      });
      currentBtn.classList.add("active");
      currentTab.classList.add("active");
    }
  });
}
// faq end
