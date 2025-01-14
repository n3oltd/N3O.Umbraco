// modals start
function n3o_cdf_handleModalToggle() {
    const body = document.body;

    document.querySelectorAll(".modallClose").forEach((e) => {
        e.addEventListener("click", function (x) {
            var ModalId = x.target.dataset.modal;
            document.querySelector("#" + ModalId).classList.remove("active");
            body.classList.remove("active");
        });
    });
    
    const modallBtn = document.querySelectorAll(".modallBtn");
    const modallItems = document.querySelectorAll(".modall");
    modallBtn.forEach((e) => {
        onTabClick(modallBtn, modallItems, e);
    });
    function onTabClick(modallBtns, modallItems, item) {
        item.addEventListener("click", function (e) {
            let currentBtn = item;
            let tabId = currentBtn.getAttribute("data-mod");
            let currentTab = document.querySelector(tabId);
            if (e.srcElement.classList.contains("active")) {
                e.srcElement.classList.remove("active");
            } else if (!currentBtn.classList.contains("active")) {
                modallBtns.forEach(function (item) {
                    item.classList.remove("active");
                    body.classList.remove("active");
                });
                modallItems.forEach(function (item) {
                    item.classList.remove("active");
                    body.classList.remove("active");
                });
                //currentBtn.classList.add('active');
                currentTab && currentTab.classList.add("active");
                body.classList.add("active");
            }
        });
        window.onclick = function (event) {
            modallItems.forEach((black) => {
                if (black == event.target) {
                    black.classList.remove("active");
                    body.classList.remove("active");
                }
            });
        };
    }
}

document.addEventListener('DOMContentLoaded', function () {
    n3o_cdf_handleModalToggle();
})
// modals end
