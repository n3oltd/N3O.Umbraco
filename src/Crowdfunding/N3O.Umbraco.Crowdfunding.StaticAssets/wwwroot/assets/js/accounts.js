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

function formDataToObject(form) {
    const formData = new FormData(form)

    const output = {};
    for (let [key, value] of formData.entries()) {
        output[key] = value
    }

    return output
}

function convertToFlatStructure(input) {
    const output = {};

    function traverseObject(object, path = "") {
        for (const key in object) {
            const newPath = path ? `${path}.${key}` : key;

            if (typeof object[key] === 'object' && object[key] !== null) {
                traverseObject(object[key], newPath);
            } else {
                output[newPath] = object[key];
            }
        }
    }

    traverseObject(input);

    return output;
}

function convertToObject(dataObject) {
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

const createAccount = () => {
    const accountForm = document.querySelector('#create-account-form');
    const emailField = document.querySelector('input[type="email"]');
    emailField.disabled = true;

    accountForm.addEventListener('submit', async e => {
        e.preventDefault();

        const accountInfo = formDataToObject(accountForm);
        const accountReq = convertToObject(accountInfo);

        await sendCreateAccount(accountReq);
    });

    async function sendCreateAccount(req) {
        const response = await fetch('/umbraco/api/crm/accounts/create', {
            method: "POST",
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify(req)
        })

        if (response.ok) {
            console.log('happy');
            return
        }

        document.querySelector('#newAccountBtn').removeAttribute('disabled')
        if (response.status === 412) {
            console.log('not happy, handle errors', response);

            if (window.themeConfig) {

                window.themeConfig.showErrors(response.errors)
            }
        }

        if (!response.ok) {
            console.log('show generic error');
        }
    }
}

const updateAccount = (account) => {
    const accountModal = document.querySelector('#updateAccount-modal');
    const accountForm = document.querySelector('#update-account-form');
    const emailField = document.querySelector('input[type="email"]');
    emailField.disabled = true;

    const flatAccount = convertToFlatStructure(account);

    for (let [key, value] of Object.entries(flatAccount)) {
        const elm = accountModal.querySelector(`[name='${key}']`);

        if (elm) {
            elm.value = value;
        }
    }

    accountForm.addEventListener('submit', async e => {
        e.preventDefault();

        const accountInfo = formDataToObject(accountForm);
        const accountReq = convertToObject(accountInfo);
        accountReq.id = document.querySelector('#n3o_cdf-selected-account-id').value;
        accountReq.referrence = document.querySelector('#n3o_cdf-selected-account-ref').value;

        await sendUpdateAccount(accountReq);
    });

    async function sendUpdateAccount(req) {
        const response = await fetch('/umbraco/api/crm/accounts/update', {
            method: "PUT",
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify(req)
        })

        if (response.ok) {
            console.log('happy');
            return
        }

        document.querySelector('#newAccountBtn').removeAttribute('disabled')
        if (response.status === 412) {
            console.log('not happy, handle errors', response);

            if (window.themeConfig) {

                window.themeConfig.showErrors(response.errors)
            }
        }

        if (!response.ok) {
            console.log('show generic error');
        }
    }
};