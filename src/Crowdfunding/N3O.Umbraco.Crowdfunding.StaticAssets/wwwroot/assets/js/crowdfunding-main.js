// // menu start
// var menu = document.getElementById("menu");
// var menuBtn = document.getElementById("menuBtn");
// menuBtn.onclick = function () {
//   menu.classList.toggle("active");
//   menuBtn.classList.toggle("active");
//   body.classList.toggle("active");
// };
// window.onclick = function (event) {
//   if (event.target == menu) {
//     menu.classList.remove("active");
//     menuBtn.classList.remove("active");
//     body.classList.remove("active");
//   }
// };
// // menu end
var body = document.body;

// scroll start
let header = document.getElementById("header");
function scrollFunc() {
  if (window.scrollY >= 60) {
    header?.classList.add("sticky");
  } else {
    header?.classList.remove("sticky");
  }
}
window.onscroll = function () {
  scrollFunc();
};
// scroll end
// // faq start
// const tabBtn = document.querySelectorAll(".tabBtn");
// const tabEvent = document.querySelectorAll(".tabEvent");
// tabBtn.forEach((e) => {
//   onTabClick(tabBtn, tabEvent, e);
// });
// function onTabClick(tabBtns, tabItems, item) {
//   item.addEventListener("click", function (e) {
//     let currentBtn = item;
//     let tabId = currentBtn.getAttribute("data-tab");
//     let currentTab = document.querySelector(tabId);
//     if (currentBtn.classList.contains("active")) {
//       console.log("now active");
//       const faq = currentBtn.parentElement.querySelector(".tabEvent");
//       if (faq) {
//         faq.classList.remove("active");
//         currentBtn.classList.remove("active");
//       }
//     } else if (!currentBtn.classList.contains("active")) {
//       tabBtns.forEach(function (item) {
//         item.classList.remove("active");
//       });

//       tabItems.forEach(function (item) {
//         item.classList.remove("active");
//       });
//       currentBtn.classList.add("active");
//       currentTab.classList.add("active");
//     }
//   });
// }
// // faq end
// sliders
$(function () {
  $(".campaignSlider").slick({
    infinite: true,
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: false,
    dots: true,
    speed: 600,
    fade: true,
  });
});

const selectFunc = () => {
  const selects = document.querySelectorAll(".select");

  selects.forEach((select) => {
    const selected = select.querySelector(".select__selected");
    const selectOptions = select.querySelector(".select__options");
    const listItems = selectOptions.querySelectorAll("li");
    const input = select.querySelector("input[type='hidden']");
    selected.onclick = () => {
      select.classList.toggle("active");
      eventHandler();
    };
    listItems.forEach((listItem) => {
      listItem.onclick = () => {
        selected.innerHTML = listItem.innerHTML;
        select.classList.remove("active");
        input.value = listItem.getAttribute("data-value");
        eventHandler();
      };
    });
    const eventHandler = () => {
      window.addEventListener("click", (e) => {
        if (!select.contains(e.target)) {
          select.classList.remove("active");
        }
      });
    };
  });
};
selectFunc();

const checkoutForm = document.getElementById("checkoutForm");
if (checkoutForm) {
  const checkoutSubmit = document.getElementById("checkoutSubmit");
  const checkoutMessage = document.getElementById("checkoutMessage");
  const inputs = checkoutForm.querySelectorAll("input");
  const giftaid = document.getElementById("giftaid");

  if (giftaid) {
    const giftButtons = giftaid.querySelectorAll("input");
    giftButtons.forEach((giftButton) => {
      giftButton.onclick = () => {
        const giftButtonText = giftButton.parentElement.querySelector("span");
        if (giftButton.checked) {
          giftaid.classList.remove("error");
        }
        if (giftButtonText.innerText == "Yes") {
          giftaid.querySelector("#giftaidText").classList.remove("hidden");
        } else {
          giftaid.querySelector("#giftaidText").classList.add("hidden");
        }
        const errors = document.querySelectorAll(".error");
        if (errors.length === 0) {
          if (checkoutMessage) {
            checkoutMessage.classList.remove("active");
          }
        }
      };
    });
  }
  if (checkoutSubmit) {
    checkoutSubmit.onclick = () => {
      if (giftaid) {
        const giftButtons = giftaid.querySelectorAll("input");
        const giftArray = [];
        giftButtons.forEach((giftButton) => {
          if (giftButton.checked) {
            giftArray.push(true);
          } else {
            giftArray.push(false);
          }
        });
        if (giftArray.includes(true)) {
        } else {
          giftaid.classList.add("error");
        }
      }
      inputs.forEach((input) => {
        const inputWrapper = input.parentElement;
        if (input.value == "") {
          inputWrapper.classList.add("error");
          checkoutMessage.classList.add("active");
        }
      });
    };
    inputs.forEach((input) => {
      const inputWrapper = input.parentElement;
      input.onchange = (e) => {
        if (e.target.value == "") {
          inputWrapper.classList.add("error");
        } else {
          inputWrapper.classList.remove("error");
        }
        const errors = document.querySelectorAll(".error");
        if (errors.length === 0) {
          checkoutMessage.classList.remove("active");
        }
      };
    });
  }
}
// showMore start
const showMoreWrapper = document.getElementById("showMoreWrapper");
if (showMoreWrapper) {
  const showMoreBtns = document.querySelectorAll(".showMoreBtn");
  showMoreBtns.forEach((showMoreBtn) => {
    showMoreBtn.onclick = () => {
      const parentElement = showMoreBtn.parentNode;
      parentElement.classList.toggle("active");
      showMoreBtn.classList.toggle("active");
      if (showMoreBtn.classList.contains("active")) {
        showMoreBtn.innerHTML = "Hide Options";
      } else {
        showMoreBtn.innerHTML = "More Options";
      }
    };
  });
}
// showMore end
