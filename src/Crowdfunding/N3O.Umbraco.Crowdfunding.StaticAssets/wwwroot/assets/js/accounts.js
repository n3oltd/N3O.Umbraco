const accounts = document.getElementById("newAccount");
if (accounts) {
  const newAccountBtn = document.getElementById("newAccountBtn");
  const allElements = accounts.querySelectorAll("input, select");

  let allFilled = true;

  allElements.forEach((item) => {
    item.onkeyup = () => {
      allFilled = true;

      for (const element of allElements) {
        if (element.value === "") {
          allFilled = false;
          break;
        }
      }

      newAccountBtn.disabled = !allFilled;
    };
  });
}
