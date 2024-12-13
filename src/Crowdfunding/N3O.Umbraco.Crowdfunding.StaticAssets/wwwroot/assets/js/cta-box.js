function n3o_cdf_handleDonationForm() {
  const ctaFunds = document.querySelectorAll(".n3o-ctaFund");
  
  if (ctaFunds.length === 0) {
    return;
  }
  
  const cta = document.querySelector(".n3o-cta");
  const submitBtn = cta.querySelector(".submit3");
  const submitBtnPrice = submitBtn.querySelector("span");
  
  ctaFunds.forEach((ctaFund) => {
    const ctaItems = ctaFund.querySelectorAll(".n3o-ctaFund__item");
    const ctaAmount = ctaFund.querySelector(".n3o-ctaAmount");
    const ctaCheckbox = ctaAmount.querySelector("input[type='checkbox']");
    const ctaInput = ctaAmount.querySelector("input[type='number']");    
    
    ctaInput.oninput = (e) => {
      const ctaCurrencyPickerValue = ctaAmount.querySelector(".n3o-currency-picker").value;
      
      let oldPrice = 0;
      const ctaActiveItems = document.querySelectorAll(".n3o-ctaFund__item.active");
      
      ctaActiveItems.forEach((ctaActiveItem) => {
        const price = ctaActiveItem.querySelector("h3");
        oldPrice = oldPrice + parseFloat(price.innerHTML.slice(1));
      });
      
      const ctaAmounts = document.querySelectorAll(".n3o-ctaAmount.active");
      
      ctaAmounts.forEach((ctaAmount2) => {
        const ctaInput2 = ctaAmount2.querySelector("input[type='number']");
        
        if (ctaAmount2 !== ctaAmount && ctaInput2.value !== "" && ctaInput2.value !== 0) {
          oldPrice = oldPrice + parseFloat(ctaInput2.value);
        }
      });

      let newPrice = ctaInput.value === "" ||
                     ctaInput.value === 0 ||
                     isNaN(parseFloat(e.target.value)) ? 0 : parseFloat(e.target.value);
      let totalPrice = oldPrice + newPrice;
      
      if (totalPrice === 0) {
        submitBtnPrice.innerHTML = "";
        submitBtn.setAttribute("disabled", true);
      } else {
        submitBtnPrice.innerHTML = window.themeConfig.formatter.number.formatMoney(totalPrice, ctaCurrencyPickerValue);
        submitBtn.removeAttribute("disabled");
      }
    };
    
    ctaCheckbox.onchange = (e) => {
      const ctaCurrencyPickerValue = ctaAmount.querySelector(".n3o-currency-picker").value;
      
      if (e.target.checked) {        
        ctaAmount.classList.add("active");
        ctaInput.removeAttribute("disabled");
        
        ctaItems.forEach((ctaItem) => {
          if (ctaItem.classList.contains("active")) {
            const checkbox = ctaItem.querySelector("input");
            
            checkbox.checked = false;
            
            ctaItem.classList.remove("active");
          }
        });
        
        let oldPrice = 0;
        const ctaActiveItems = document.querySelectorAll(".n3o-ctaFund__item.active");
        
        ctaActiveItems.forEach((ctaActiveItem) => {
          const price = ctaActiveItem.querySelector("h3");
          oldPrice = oldPrice + parseFloat(price.innerHTML.slice(1));
        });
        
        const ctaAmounts = document.querySelectorAll(".n3o-ctaAmount.active");
        
        ctaAmounts.forEach((ctaAmount2) => {
          const ctaInput2 = ctaAmount2.querySelector("input[type='number']");
          
          if (ctaInput2.value !== "" && ctaInput2.value !== 0) {
            oldPrice = oldPrice + parseFloat(ctaInput2.value);
          }
        });
        
        let totalPrice = oldPrice;
        
        if (totalPrice === 0) {
          submitBtnPrice.innerHTML = "";
          submitBtn.setAttribute("disabled", true);
        } else {
          submitBtnPrice.innerHTML = window.themeConfig.formatter.number.formatMoney(totalPrice, ctaCurrencyPickerValue);
          submitBtn.removeAttribute("disabled");
        }
      } else {
        ctaAmount.classList.remove("active");
        ctaInput.setAttribute("disabled", true);
        
        let oldPrice = 0;
        const ctaActiveItems = document.querySelectorAll(".n3o-ctaFund__item.active");
        
        ctaActiveItems.forEach((ctaActiveItem) => {
          const price = ctaActiveItem.querySelector("h3");
          oldPrice = oldPrice + parseFloat(price.innerHTML.slice(1));
        });
        
        const ctaAmounts = document.querySelectorAll(".n3o-ctaAmount.active");
        
        ctaAmounts.forEach((ctaAmount2) => {
          const ctaInput2 = ctaAmount2.querySelector("input[type='number']");
          if (ctaInput2.value !== "" && ctaInput2.value !== 0) {
            oldPrice = oldPrice + parseFloat(ctaInput2.value);
          }
        });
        
        let totalPrice = oldPrice;
        if (totalPrice === 0) {
          submitBtnPrice.innerHTML = "";
          submitBtn.setAttribute("disabled", true);
        } else {
          submitBtnPrice.innerHTML = window.themeConfig.formatter.number.formatMoney(totalPrice, ctaCurrencyPickerValue);
          submitBtn.removeAttribute("disabled");
        }
      }
    };
    
    ctaItems.forEach((ctaItem) => {
      const checkbox = ctaItem.querySelector("input");
      
      checkbox.onchange = (e) => {
        const ctaCurrencyPickerValue = ctaAmount.querySelector(".n3o-currency-picker").value;
        
        if (e.target.checked) {
          ctaItem.classList.add("active");
          ctaAmount.classList.remove("active");
          ctaInput.setAttribute("disabled", true);
          ctaCheckbox.checked = false;
          
          ctaItems.forEach((ctaItem2) => {
            if (ctaItem !== ctaItem2 && ctaItem2.classList.contains("active")) {
              const checkbox2 = ctaItem2.querySelector("input");
              checkbox2.checked = false;
              ctaItem2.classList.remove("active");
            }
          });
          
          let oldPrice = 0;
          const ctaActiveItems = document.querySelectorAll(".n3o-ctaFund__item.active");
          
          ctaActiveItems.forEach((ctaActiveItem) => {
            const price = ctaActiveItem.querySelector("h3");
            
            oldPrice = oldPrice + parseFloat(price.innerHTML.slice(1));
          });
          
          const ctaAmounts = document.querySelectorAll(".n3o-ctaAmount.active");
          
          ctaAmounts.forEach((ctaAmount2) => {
            const ctaInput2 = ctaAmount2.querySelector("input[type='number']");
            
            if (ctaInput2.value !== "" && ctaInput2.value !== 0) {
              oldPrice = oldPrice + parseFloat(ctaInput2.value);
            }
          });
          
          let totalPrice = oldPrice;
          if (totalPrice === 0) {
            submitBtnPrice.innerHTML = "";
            submitBtn.setAttribute("disabled", true);
          } else {
            submitBtnPrice.innerHTML = window.themeConfig.formatter.number.formatMoney(totalPrice, ctaCurrencyPickerValue);
            submitBtn.removeAttribute("disabled");
          }
        } else {
          ctaItem.classList.remove("active");
          let oldPrice = 0;
          const ctaActiveItems = document.querySelectorAll(".n3o-ctaFund__item.active");
          
          ctaActiveItems.forEach((ctaActiveItem) => {
            const price = ctaActiveItem.querySelector("h3");
            oldPrice = oldPrice + parseFloat(price.innerHTML.slice(1));
          });
          
          const ctaAmounts = document.querySelectorAll(".n3o-ctaAmount.active");
          
          ctaAmounts.forEach((ctaAmount2) => {
            const ctaInput2 = ctaAmount2.querySelector("input[type='number']");
            
            if (ctaInput2.value !== "" && ctaInput2.value !== 0) {
              oldPrice = oldPrice + parseFloat(ctaInput2.value);
            }
          });
          
          let totalPrice = oldPrice;
          
          if (totalPrice === 0) {
            submitBtnPrice.innerHTML = "";
            submitBtn.setAttribute("disabled", true);
          } else {
            submitBtnPrice.innerHTML = window.themeConfig.formatter.number.formatMoney(totalPrice, ctaCurrencyPickerValue);
            submitBtn.removeAttribute("disabled");
          }
        }
      };
    });
  });

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
}

$(document).ready(() => {
  n3o_cdf_handleDonationForm();
})