var body = document.body;

// scroll start
let header = document.getElementById("header");
function scrollFunc() {
  if (window.scrollY >= 60) {
    header.classList.add("sticky");
  } else {
    header.classList.remove("sticky");
  }
}
if (header) {
  window.onscroll = function () {
    scrollFunc();
  };
}
// scroll end
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

const sortFunc = () => {
  const sorts = document.querySelectorAll(".sort");
  sorts.forEach((sort) => {
    const selected = sort.querySelector(".sort__selected");
    const selectedText = selected.querySelector("b");
    const sortOptions = sort.querySelector(".sort__options");
    const listItems = sortOptions.querySelectorAll("li");
    const input = sort.querySelector("input[type='hidden']");
    selected.onclick = () => {
      sort.classList.toggle("active");
      eventHandler();
    };
    listItems.forEach((listItem) => {
      listItem.onclick = () => {
        selectedText.innerHTML = listItem.innerHTML;
        sort.classList.remove("active");
        input.value = listItem.getAttribute("data-value");
        eventHandler();
      };
    });
    const eventHandler = () => {
      window.addEventListener("click", (e) => {
        if (!sort.contains(e.target)) {
          sort.classList.remove("active");
        }
      });
    };
  });
};
sortFunc();

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
          if (inputWrapper.classList.contains("iti")) {
            inputWrapper.classList.remove("error");
            inputWrapper.parentElement.classList.add("error");
          }
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

// copy url start
const copyButton = document.getElementById("copyBtn");
if (copyButton) {
  const textElement = document.getElementById("myUrl");

  const copyText = (e) => {
    window.getSelection().selectAllChildren(textElement);
    document.execCommand("copy");
  };
  copyButton.addEventListener("click", (e) => copyText(e));
}
// copy end

// url to ifram start
$(document).ready(function () {
  $("#sendUrl").click(function () {
    function getId(url) {
      var regExp =
          /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=)([^#\&\?]*).*/;
      var match = url.match(regExp);

      if (match && match[2].length == 11) {
        return match[2];
      } else {
        return "error";
      }
    }
    var videoId = $("#url").val();
    var myId = getId(videoId);

    $("#videoSrc").html(myId);

    $("#myVideo").html(
        '<iframe src="//www.youtube.com/embed/' +
        myId +
        '" frameborder="0" allowfullscreen></iframe>'
    );
  });
});

// url to ifram end

// adminC contribution start
const adminC = document.getElementById("adminC");
if (adminC) {
  const adminCText = adminC.querySelector(".adminC__text");
  const adminCRow = adminC.querySelector(".adminC__row");
  const adminCRowCustom = adminC.querySelector(".adminC__foot");
  const customFeeBtns = adminC.querySelectorAll(".customBtn");
  const customBack = adminC.querySelector(".customBack");
  const customInput = adminC.querySelector(".cta__input");
  const adminCSlider = adminC.querySelector(".adminC__slider");
  customFeeBtns.forEach((customFeeBtn) => {
    customFeeBtn.onclick = () => {
      customInput.classList.add("active");
      adminCRowCustom.classList.add("active");
      adminCText.classList.remove("active");
      adminCRow.classList.remove("active");
      adminCSlider.classList.remove("active");
    };
  });
  customBack.onclick = () => {
    customInput.classList.remove("active");
    adminCRowCustom.classList.remove("active");
    adminCText.classList.add("active");
    adminCRow.classList.remove("active");
    adminCSlider.classList.add("active");
  };
}
// adminC contribution end

const cta_amounts = document.querySelectorAll(".ctaAmount");
cta_amounts.forEach((cta_amount) => {
  const cta_select = cta_amount.querySelector("select");
  const cta_label = cta_amount.querySelector("label");
  cta_select.onchange = (e) => {
    if (e.target.value === "GBP") {
      cta_label.innerHTML = "£";
    } else if (e.target.value === "USD") {
      cta_label.innerHTML = "$";
    }
  };
});
