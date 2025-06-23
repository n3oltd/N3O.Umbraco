function n3o_cdf_formDataToObject(form) {
    const formData = new FormData(form)

    const output = {};
    for (let [key, value] of formData.entries()) {
        output[key] = value
    }

    return output
}

function n3o_cdf_convertToFlatStructure(input) {
    const output = {};

    function n3o_cdf_traverseObject(object, path = "") {
        for (const key in object) {
            const newPath = path ? `${path}.${key}` : key;

            if (typeof object[key] === 'object' && object[key] !== null) {
                n3o_cdf_traverseObject(object[key], newPath);
            } else {
                output[newPath] = object[key];
            }
        }
    }

    n3o_cdf_traverseObject(input);

    return output;
}

function n3o_cdf_convertToObject(dataObject) {
    const parsedData = dataObject;

    const convertedObject = {};

    for (const property in parsedData) {
        const components = property.split('.');

        let currentObject = convertedObject;
        for (let i = 0; i < components.length - 1; i++) {
            if (!currentObject[components[i]]) {
                currentObject[components[i]] = {};
            }
            currentObject = currentObject[components[i]];
        }

        currentObject[components[components.length - 1]] = parsedData[property];
    }

    return convertedObject;
}

function n3o_cdf_getQueryParameter(param) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(param);
}

function n3o_cdf_setQueryParameter(param, value) {
    const url = new URL(window.location.href);
    url.searchParams.set(param, value);
    window.history.replaceState({}, '', url.toString());
}

function n3o_cdf_updateTextIfRequired(refreshCount) {
    const statusMessage = document.getElementById('account-status-message');
    
    if (refreshCount >= 3 && statusMessage) {
        statusMessage.textContent = "@PartialText.Get('Hang tight, almost there.')";
    }
}


function n3o_cdf_poolRequest(fetcher, successAction, ...args) {
    let refreshCount = parseInt(n3o_cdf_getQueryParameter('rc')) || 0;
    
    const intervalId = setInterval(async () => {
        const result = await fetcher(args);
        if (result.status === 200) {
            const data = await result.json();

            if (data && data.isCreated) {
                clearInterval(intervalId);
                successAction();
            }
        }
        
        refreshCount++;
        
        n3o_cdf_setQueryParameter('rc', refreshCount);
        
        n3o_cdf_updateTextIfRequired(refreshCount);
        
    }, 5000)
}

const n3o_cdf_checkAccountCreated = async accountId => {
    const resp = await fetch(`/umbraco/api/engage/accounts/${accountId}/checkCreatedStatus`);

    return resp;
}

const n3o_cdf_createAccount = () => {
    const accountForm = document.querySelector('#create-account-form');
    const emailField = document.querySelector('input[type="email"]');
    emailField.value = document.getElementById("memberEmail").value;
    emailField.readOnly = true;

    const countryField = document.getElementById("address.country");
    const phoneCountry = document.querySelector("#countryCode");
    phoneCountry.value = countryField.value;

    countryField.addEventListener("change", function () {
        phoneCountry.value = countryField.value;
    });

    accountForm.addEventListener('submit', async e => {
        e.preventDefault();

        const accountInfo = n3o_cdf_formDataToObject(accountForm);
        const accountReq = n3o_cdf_convertToObject(accountInfo);

        await n3o_cdf_sendCreateAccount(accountReq);
    });

    async function n3o_cdf_sendCreateAccount(req) {
        const response = await fetch('/umbraco/api/engage/accounts', {
            method: "POST", headers: {'Content-Type': 'application/json'}, body: JSON.stringify(req)
        })

        if (response.ok) {
            n3o_cdf_toggleConfirmAccountModal(true);

            let accountId = await response.text();

            accountId = accountId.replace(/^"|"$/g, '');

            n3o_cdf_poolRequest(n3o_cdf_checkAccountCreated, () => {
                n3o_cdf_toggleConfirmAccountModal(false);

                window.location.reload();
            }, accountId)
        }

        document.querySelector('#newAccountBtn').removeAttribute('disabled')
        if (response.status === 412) {
            if (window.themeConfig) {
                window.themeConfig.showError(response.errors)
            }
        }

        if (!response.ok) {
            if (window.themeConfig) {
                window.themeConfig.showError(response.errors)
            }
        }
    }

    function n3o_cdf_toggleConfirmAccountModal(show) {
        const modal = document.getElementById("confirmAccount-modal");

        if (show) {
            modal.classList.add('active');
        } else {
            modal.classList.remove('active');
        }
    }
}

const n3o_cdf_updateAccount = (account) => {
    const accountModal = document.querySelector('#updateAccount-modal');
    const accountForm = document.querySelector('#update-account-form');
    const emailField = document.querySelector('input[type="email"]');
    emailField.readOnly = true;

    const phoneCountry = document.querySelector("#countryCode");
    phoneCountry.value = document.getElementById("address.country").value;

    const flatAccount = n3o_cdf_convertToFlatStructure(account);

    for (let [key, value] of Object.entries(flatAccount)) {
        const elm = accountModal.querySelector(`[name='${key}']`);

        if (elm) {
            elm.value = value;
        }
    }

    accountForm.addEventListener('submit', async e => {
        e.preventDefault();

        const accountInfo = n3o_cdf_formDataToObject(accountForm);
        const accountReq = n3o_cdf_convertToObject(accountInfo);
        accountReq.id = document.querySelector('#n3o_cdf-selected-account-id').value;
        accountReq.reference = document.querySelector('#n3o_cdf-selected-account-ref').value;

        await n3o_cdf_sendUpdateAccount(accountReq);
    });

    async function n3o_cdf_sendUpdateAccount(req) {
        const response = await fetch('/umbraco/api/engage/accounts', {
            method: "PUT", headers: {'Content-Type': 'application/json'}, body: JSON.stringify(req)
        })

        if (response.ok) {
            window.location.reload();
        }

        document.querySelector('#newAccountBtn').removeAttribute('disabled')
        if (response.status === 412) {
            const res = await response.json();

            if (window.themeConfig) {
                window.themeConfig.showError(response.errors)
            }
            const errorSpan = document.getElementById('errors');

            res.errors.map(e => {
                const elm = document.getElementById(e.property)

                if (elm) {
                    elm.innerText = e.error;
                    elm.classList.remove('hidden');
                }

                if (!elm) {
                    errorSpan.innerHTML = errorSpan.innerHTML + e.error;
                    errorSpan.classList.remove('hidden');
                }
            })

        }

        if (!response.ok) {
            if (window.themeConfig) {
                window.themeConfig.showError(response.errors)
            }
        }

    }
};

const n3o_initialize_account = () => {
    const accounts = document.getElementById("newAccount");

    if (accounts) {
        const newAccountBtn = document.getElementById("newAccountBtn");
        const allElements = accounts.querySelectorAll("input:not([type='hidden']):not([hidden])[data-required='True'], select:not([hidden])[data-required='True']");

        newAccountBtn.disabled = true;

        let allFilled = true;

        allElements.forEach((item) => {
            if (item.value === "") {
                allFilled = false;
            }

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

        newAccountBtn.disabled = !allFilled;
    }
}