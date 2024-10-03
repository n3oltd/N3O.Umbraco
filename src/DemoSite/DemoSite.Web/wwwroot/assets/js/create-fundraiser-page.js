function n3o_cdf_filterList(inputId, listId) {
    var input, filter, ul, li;
    input = document.getElementById(inputId);
    filter = input.value.toUpperCase();
    ul = document.getElementById(listId);
    li = ul.getElementsByTagName("li");

    for (let i = 0; i < li.length; i++) {
        if (li[i].innerHTML.toUpperCase().indexOf(filter) > -1) {
            li[i].style.display = "";
        } else {
            li[i].style.display = "none";
        }
    }
}

function n3o_cdf_splitDimensionAndNumber(str) {
    if (typeof str !== 'string' || !str.trim()) {
        return [];
    }
    const match = str.match(/^(\w+)([0-9]+)$/);

    if (match) {
        const [_, dimension, number] = match;
        return [dimension, parseInt(number)];
    }

    return [];
}

class n3o_cdf_CreatePageAPI {
    constructor() {
        this.errorMessages = {
            suggestSlug: 'Failed to fetch suggestSlug',
            createFundraiser: 'Failed to create fundraiser',
            getCampaignGoalOptions: 'Failed to fetch campaign goal options'
        };
    }

    async suggestSlug(name) {
        return this.makeRequest('suggestSlug', () => fetch(`/umbraco/api/Crowdfunding/suggestSlug?name=${name}`, { method: "POST" }));
    }

    async createFundraiser(req) {
        return this.makeRequest('createFundraiser', () => fetch(`/umbraco/api/Crowdfunding/fundraisers`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(req)
        }), req);
    }

    async getCampaignGoalOptions(campaignId, goalOptionId) {
        return this.makeRequest('getCampaignGoalOptions', () => fetch(`/umbraco/api/Crowdfunding/campaigns/${campaignId}/goalOptions/${goalOptionId}`), campaignId, goalOptionId);
    }

    async getPrice(req) {
        return this.makeRequest('getPrice ', () => fetch(`/umbraco/api/Giving/pricing`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(req)
        }), req);
    }

    async makeRequest(endpoint, requestFn) {
        const errorMessage = this.errorMessages[endpoint];


        try {
            const response = await requestFn();
            if (!response.ok) {
                throw new n3o_cdf_APIError(errorMessage, response.status, response.statusText, await response.json());
            }
            return response.json();
        } catch (error) {
            throw error;
        }

    }
}

class n3o_cdf_APIError extends Error {
    constructor(message, status, statusText, response) {
        super(`${message}: ${status} ${statusText}`);
        this.status = status;
        this.statusText = statusText;
        this.errors = response.errors
    }
}

const client = new n3o_cdf_CreatePageAPI();

class n3o_cdf_ErrorHanlder {
    static displayErrorMessages(error, messageContainerId) {
        const container = document.getElementById(messageContainerId);
        if (error.errors) {
            const fragment = document.createDocumentFragment();
            container.querySelector('.detail').innerHTML = '';

            if (error?.errors?.length) {
                Object.values(error.errors).forEach(e => {
                    if (Array.isArray(e)) {
                        e.forEach(message => {
                            const elm = document.createElement('li');
                            elm.innerHTML = isPlainObject(message) ? message.error : message;
                            fragment.appendChild(elm);
                        });
                    } else {
                        const elm = document.createElement('li');
                        elm.textContent = e.message || e.error || e;
                        fragment.appendChild(elm);
                    }
                });
            }

            container.querySelector('.detail').appendChild(fragment);
            container.style.display = 'inherit';
        }

        if (typeof error === 'string') {
            container.querySelector('.detail').textContent = error;
            container.style.display = 'inherit';
        }

        if (error.status === 500) {
            container.querySelector('.detail').textContent = 'Sorry, an error has occurred. Please try again or contact support!"';
            container.style.display = 'inherit';
        }
    }
}

class n3o_cdf_SelectedGoal {
    constructor() {
        this.value = null;
        this.type = null;
        this.fields = [];
        this.fieldsValues = [];
        this.amount = null;
        this.currency = "";
        this.pageTitle = "";
        this.slug = "";
        this.pricing = null;
        this.currentMinAmount = 0;
    }

    updateFieldValue(alias, value, type) {
        const existingIndex = this.fieldsValues.findIndex(item => item.alias === alias);

        if (existingIndex !== -1) {
            this.fieldsValues[existingIndex].value = value;
        } else {
            this.fieldsValues.push({ alias, value, type });
        }
    }

    isValidGoal() {
        const hasValidValues = !!this.value && !!this.amount && !!this.currency;

        const hasValidAmount = this.amount >= this.currentMinAmount;

        let isMultipleOf = true;

        if (this.pricing && this.pricing.locked) {
            isMultipleOf = (this.amount % this.pricing.currencyValues[this.currency].amount) === 0;
        }

        const hasValuesWithValidAmount = hasValidValues && hasValidAmount && isMultipleOf;

        if (!this.fields.length) {
            return hasValuesWithValidAmount;
        }

        const hasFieldsValues = this.fields.every(field => {
            let fieldValue = this.fieldsValues.find(val => val.alias === field.alias);

            if (!fieldValue && field.required) {
                return false;
            }

            if (!fieldValue && !field.required) {
                return true;
            }

            const validations = {
                hasValidMaxLength: field.textMaxLength > 0 ? fieldValue.value.length <= field.textMaxLength : true,
                hasRequiredValue: field.required ? fieldValue.value !== null && fieldValue.value !== "" && fieldValue.value !== undefined : true
            }

            return Object.values(validations).every(val => val);
        })

        return hasFieldsValues && hasValuesWithValidAmount;
    }

    isValidAboutPage() {
        return !!this.pageTitle && !!this.slug;
    }
}

class n3o_cdf_FieldHandler {
    static createTextInputElement(field) {
        const label = document.createElement('label');
        label.classList.add('input__outer');
        label.classList.add('field');
        label.innerHTML = `<h4>${field.name} ${field.required ? '*' : ''}</h4>
                                   <div class="input big">
                                     <input type="text" maxlength=${field.textMaxLength > 0 ? field.textMaxLength.toString() : ''} placeholder="" ${field.required ? 'required' : ''} />
                                   </div>`;
        return label;
    }

    static createCheckboxElement(field) {
        const label = document.createElement('label');
        label.classList.add('check__outer');
        label.classList.add('field');
        label.innerHTML = `<h4>${field.name} ${field.required ? '*' : ''}</h4>
                                   <label class="check">
                                     <div class="check__box">
                                       <input type="checkbox" ${field.required ? 'required' : ''} />
                                       <span></span>
                                     </div>
                                     <p class="small"></p>
                                   </label>`;
        return label;
    }

    static createCheckboxToggleElement(field) {
        const container = document.createElement('div');
        container.classList.add('setting__date');
        container.classList.add('field');
        container.innerHTML = `
                            <label class="checkD">
                        <div class="checkD__box">
                          <input type="checkbox" id="checkD" ${field.required ? 'required' : ''}  />
                          <span></span>
                        </div>
                        <p class="sm">${field.name} ${field.required ? '*' : ''}</p>
                      </label>`;
        return container;
    }

    static createDatePickerElement(field) {
        const label = document.createElement('label');
        label.classList.add('input__outer');
        label.classList.add('field');
        label.innerHTML = `<h4>${field.name} ${field.required ? '*' : ''}</h4>
                                   <div class="setting__date-input big">
                                     <input
                                       placeholder="Select Date"
                                       type="text"
                                       onfocus="(this.type='date')"
                                       onblur="(this.type='text')"
                                       ${field.required ? 'required' : ''}  />
                                     <span>
                                       <img src="~/assets/images/icons/calendar.svg" alt="" />
                                     </span>
                                   </div>`;
        return label;
    }

    static clearDimensions() {
        ['dimension1', 'dimension2', 'dimension3', 'dimension4'].forEach(dimension => {
            const [currDimension, index] = n3o_cdf_splitDimensionAndNumber(dimension);
            const dimensionOptionsContainerElm = document.getElementById(`goal-selection-${currDimension}-${index}`);

            if (dimensionOptionsContainerElm) {
                dimensionOptionsContainerElm.innerHTML = "";
            }

        })
    }

    static populateDimensions(dimensions, data) {
        dimensions.forEach(dimension => {
            const dimensionData = data[dimension];

            if (!dimensionData || !dimensionData.default || !dimensionData.allowedOptions.length) {
                return;
            }

            const [currDimension, index] = n3o_cdf_splitDimensionAndNumber(dimension);
            const dimensionElms = document.getElementsByClassName(`${currDimension}-${index}`);

            const dimensionContainerElm = document.getElementById(`${currDimension}-${index}`);
            const dimensionOptionsContainerElm = document.getElementById(`goal-selection-${currDimension}-${index}`);

            dimensionContainerElm.querySelector("input[type='hidden']").value = dimensionData.default.id;
            dimensionContainerElm.querySelector('.select__selected').innerHTML = dimensionData.default.name;

            dimensionOptionsContainerElm.innerHTML = "";

            for (let elm of dimensionElms) {
                elm.style.display = 'none'
            }
            
            if (dimensionData.allowedOptions.length > 1) {
                
                const fragment = document.createDocumentFragment();

                for (const option of dimensionData.allowedOptions) {
                    const li = document.createElement('li');
                    
                    li.textContent = option.name;
                    li.dataset.value = option.id;
                    fragment.appendChild(li);
                }

                dimensionOptionsContainerElm.appendChild(fragment);

                for (let elm of dimensionElms) {
                    elm.style.display = 'block'
                }

                n3o_cdf_selectFunc();
            }

        });
    }

    static createFieldElement(field) {
        switch (field.type) {
            case 'text':
                return this.createTextInputElement(field);
            case 'bool':
                return this.createCheckboxToggleElement(field);
            case 'date':
                return this.createDatePickerElement(field);
            default:
                console.error(`Unsupported field type: ${field.type}`);
                return null;
        }
    }
}

class n3o_cdf_PageManager {
    constructor() {
        this.selectedGoal = new n3o_cdf_SelectedGoal(); 
    }

    attachEventListeners() {
        this.handleProjectSelection();
        this.handleGoalAmount();
        this.handleCurrencySelection();
        this.handlePageTitle();
        this.handlePageSlug();
        this.handleCreatePage();
        this.handleMoveToAboutTab();
        this.handleInitialValues();
    }

    handleInitialValues() {
        const currencyWithAmount = document.getElementById('goal-amount').dataset;
        const selectedCurrency = document.getElementById('goal-currency').value

        const minAmount = currencyWithAmount[selectedCurrency];

        if (minAmount) {
            this.selectedGoal.currentMinAmount = Number(minAmount);
        }

        this.selectedGoal.currency = document.getElementById('goal-currency').value;
    }

    handleProjectSelection() {
        document.getElementById("goal-selection").addEventListener('click', async event => {
            const { id, type } = event.target.dataset;
            this.selectedGoal.value = id;
            this.selectedGoal.type = type;

            this.toggleGoalAmountField();

            this.getGoalInformation(new URLSearchParams(window.location.search).get('campaignId'), id).catch(e => n3o_cdf_ErrorHanlder.displayErrorMessages(e, 'errorMessage'));
        });
    }

    toggleGoalAmountField() {
        const elements = document.getElementsByClassName('goal-fields');
        for (const element of elements) {
            element.style.display = this.selectedGoal.value ? 'inherit' : 'none';
        }
    }

    appendOrRemoveCustomFields(feedbackCustomFields) {
        const fields = document.getElementById('goal-custom-fields').parentElement.querySelectorAll('.field');
        const fieldsContainer = document.getElementById('goal-custom-fields');

        if (!feedbackCustomFields.length) {
            fields.forEach(field => field.remove());
            return;
        }

        const fragment = document.createDocumentFragment();

        feedbackCustomFields.forEach(field => {
            const fieldElement = n3o_cdf_FieldHandler.createFieldElement(field);

            if (fieldElement) {
                fieldElement.addEventListener('input', (event) => {
                    const { alias, type } = field;


                    const value = type === "bool" ? event.srcElement.checked : event.target.value;

                    this.selectedGoal.updateFieldValue(alias, value, type);
                    this.toggleAboutPageTab();
                });

                fragment.appendChild(fieldElement);
            }
        });

        fields.forEach(field => field.remove());
        fieldsContainer.parentElement.insertBefore(fragment, fieldsContainer);

    }

    handleGoalAmount() {
        document.getElementById('goal-amount').addEventListener('blur', event => {
            this.selectedGoal.amount = event.target.value;
            this.selectedGoal.currency = document.getElementById('goal-currency').value;
            this.toggleAboutPageTab();
        });
    }

    handleCurrencySelection() {
        document.getElementById('goal-currency').addEventListener('change', event => {
            this.selectedGoal.currency = event.target.value;
            const { symbol } = event.target.selectedOptions[0].dataset;
            document.querySelector("[for='goal-amount']").innerHTML = symbol;
            const minAmount = document.getElementById('goal-amount').dataset[event.target.value];
            document.getElementById('n3o_cdf-min-amount').innerText = symbol + minAmount;

            this.selectedGoal.currentMinAmount = Number(minAmount);

            document.getElementById('multiple-of').innerHTML = this.selectedGoal.pricing ? this.selectedGoal.pricing.currencyValues[this.selectedGoal.currency].text : '';

            this.toggleAboutPageTab();
        });
    }

    handlePageTitle() {
        document.getElementById('pageTitle').addEventListener('blur', event => {
            this.selectedGoal.pageTitle = event.target.value;
            this.toggleCreatePageButton();
        });

        const handleInput = async event => {
            const value = event.target.value;
            if (this.isSlugDirty) {
                return;
            }

            if (!value) {
                return;
            }

            try {
                const suggestedSlug = await client.suggestSlug(value);

                document.getElementById('slug').value = suggestedSlug;
                document.getElementById('urlEnd').innerText = suggestedSlug;

                this.selectedGoal.slug = suggestedSlug;

                this.toggleCreatePageButton();
            } catch (e) {
                n3o_cdf_ErrorHanlder.displayErrorMessages(e, 'errorMessage2');
            }
        }

        const debouncedHandleInput = this.debounceAsync(handleInput, 200);

        document.getElementById('pageTitle').addEventListener('input', debouncedHandleInput);
    }

    handlePageSlug() {
        const slugElm = document.getElementById('slug');

        slugElm.addEventListener('blur', event => {
            this.selectedGoal.slug = event.target.value;
            this.toggleCreatePageButton();
        });

        slugElm.addEventListener('input', event => {
            this.isSlugDirty = true;
        });
    }

    toggleCreatePageButton() {
        const createPageButton = document.getElementById('create-page-btn');
        const isAboutTabValid = this.selectedGoal.isValidAboutPage();
        isAboutTabValid ? createPageButton.removeAttribute("disabled") : createPageButton.setAttribute("disabled", "");
    }

    toggleAboutPageTab() {
        const isGoalTabValid = this.selectedGoal.isValidGoal();
        const nextButton = document.getElementById('goal-next-tab');
        document.querySelector("[data-tab='#tab-2']").style.pointerEvents = isGoalTabValid ? "auto" : "none";
        isGoalTabValid ? nextButton.removeAttribute("disabled") : nextButton.setAttribute("disabled", "");
    }

    handleMoveToAboutTab() {
        document.getElementById('goal-next-tab').addEventListener('click', () => {
            document.querySelector("[data-tab='#tab-2']").click();
        });
    }

    handleDimensionChange() {
        const dimensionElms = document.querySelectorAll('[id^="dimension-"]');
        dimensionElms.forEach(elm => {
            elm.querySelectorAll("li").forEach(elm => {
                elm.addEventListener('click', () => this.getPrice())
            });
        })
    }

    handleCreatePage() {
        const createPageButtonElm = document.getElementById('create-page-btn');

        createPageButtonElm.addEventListener("click", async () => {
            if (!(this.selectedGoal.isValidGoal() && this.selectedGoal.isValidAboutPage())) {
                return;
            }
            try {

                createPageButtonElm.setAttribute('disabled', '')
                createPageButtonElm.innerText = "Creating..."

                const customFieldsReq = this.selectedGoal.fieldsValues.map(fieldValue => ({
                    alias: fieldValue.alias,
                    [fieldValue.type]: fieldValue.value
                }));


                const goalDimensions = {
                    dimension1: undefined,
                    dimension2: undefined,
                    dimension3: undefined,
                    dimension4: undefined,
                };

                document.querySelectorAll('[id^="dimension-"]').forEach(elm => {
                    if (elm.querySelector("input[type='hidden']").value) {
                        const dimensionIndex = elm.id.split('-')[1]
                        goalDimensions[`dimension${dimensionIndex}`] = elm.querySelector("input[type='hidden']").value
                    }
                })

                const createPageReq = {
                    name: this.selectedGoal.pageTitle,
                    slug: this.selectedGoal.slug,
                    campaignId: new URLSearchParams(window.location.search).get('campaignId'),
                    currency: document.getElementById('goal-currency').value,
                    goals: {
                        items: [{
                            amount: +this.selectedGoal.amount,
                            goalOptionId: this.selectedGoal.value,
                            fundDimensions: goalDimensions,
                            feedback: customFieldsReq.length ? {
                                customFields: {
                                    entries: customFieldsReq
                                }
                            } : undefined
                        }]
                    }
                };


                const response = await client.createFundraiser(createPageReq);
                if (response) {
                    window.location.href = response
                }

            } catch (error) {
                createPageButtonElm.removeAttribute('disabled')
                createPageButtonElm.innerText = "Continue"
                n3o_cdf_ErrorHanlder.displayErrorMessages(error, 'errorMessage2');
            }
        });
    }

    async getPrice() {
        const dimensionElms = document.querySelectorAll('[id^="dimension-"]');
        
        try {
            const goalDimensions = {};
            this.selectedGoal.pricing = null;

            dimensionElms.forEach(elm => {
                if (elm.querySelector("input[type='hidden']").value) {
                    const dimensionIndex = elm.id.split('-')[1]
                    goalDimensions[`dimension${dimensionIndex}`] = elm.querySelector("input[type='hidden']").value
                }
            })

            const response = await client.getPrice({
                donationItem: this.selectedGoal.fund ? this.selectedGoal.fund.id : undefined,
                feedbackScheme: this.selectedGoal.feedback ? this.selectedGoal.feedback.id : undefined,
                sponsorshipComponent: undefined,
                fundDimensions: goalDimensions
            });

            if (response) {
                const price = response.currencyValues[this.selectedGoal.currency]
                document.getElementById('multiple-of').innerText = price ? price.text : '';
            }

            this.selectedGoal.pricing = response;
        } catch (e) {
            n3o_cdf_ErrorHanlder.displayErrorMessages(e, 'errorMessage');
        }
    }

    async getGoalInformation(campaignId, goalId) {
        try {
            n3o_cdf_FieldHandler.clearDimensions();
            const response = await client.getCampaignGoalOptions(campaignId, goalId);
            
            n3o_cdf_FieldHandler.populateDimensions(['dimension1', 'dimension2', 'dimension3', 'dimension4'], response);

            this.selectedGoal.fields = response.feedback?.customFields || [];
            this.selectedGoal.fund = response.fund;
            this.selectedGoal.feedback = response.feedback;

            this.createTags(response.tags);
            this.appendOrRemoveCustomFields(response.feedback?.customFields || []);
            await this.getPrice();
            this.handleDimensionChange();
            
        } catch (e) {
            n3o_cdf_ErrorHanlder.displayErrorMessages(e, 'errorMessage')
        }
        
    }

    createTags(data) {
        const container = document.querySelector('#goal-option-tag');
        if (!container) {
            return;
        }

        const fragment = document.createDocumentFragment();

        container.innerHTML = '';

        data.forEach(item => {
            const link = document.createElement('a');
            link.classList.add('setting__tag');
            link.href = "#";
            const img = document.createElement('img');
            img.src = item.iconUrl;
            img.width = "16px";
            img.height = "16px";

            const text = document.createTextNode(item.name);
            const bold = document.createElement('b');
            bold.appendChild(text);

            link.appendChild(img);
            link.appendChild(bold);

            fragment.appendChild(link);
        });

        container.appendChild(fragment);
    }

    isPlainObject(value) {
        return typeof value === "object" && value !== null && value.constructor === Object;
    }

    debounceAsync = (callback, wait) => {
        let timeoutId = null;
        return (...args) => {
            window.clearTimeout(timeoutId);
            timeoutId = window.setTimeout(async () => {
                await callback(...args);
            }, wait);
        };
    }
}

new n3o_cdf_PageManager().attachEventListeners();
