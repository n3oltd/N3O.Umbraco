function filterGoals() {
    // Declare variables
    var input, filter, ul, li;
    input = document.getElementById("goals-search");
    filter = input.value.toUpperCase();
    ul = document.getElementById("goal-selection");
    li = ul.getElementsByTagName("li");

    // Loop through all list items, and hide those who don't match the search query
    for (let i = 0; i < li.length; i++) {
        if (li[i].innerHTML.toUpperCase().indexOf(filter) > -1) {
            li[i].style.display = "";
        } else {
            li[i].style.display = "none";
        }
    }
}

import { CrowdfundingClient } from "/assets/index.js";

const client = new CrowdfundingClient();

class SelectedGoal {
    constructor() {
        this.value = null;
        this.type = null;
        this.endDate = null;
        this.fields = [];
        this.fieldsValues = [];
        this.amount = null;
        this.currency = "";
        this.pageTitle = "";
        this.description = "";
        this.slug = "";
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

        if (!this.fields.length) {
            return hasValidValues;
        }

        const hasFieldsValues = this.fields.every(field => {
            let fieldValue = this.fieldsValues.find(val => val.alias === field.alias);

            if (!fieldValue && field.required) {
                return false;
            }

            if (field.required) {
                if (field.textMaxLength > 0) {
                    fieldValue.value.length <= field.textMaxLength;
                }

                return fieldValue.value !== null && fieldValue.value !== "" && fieldValue.value !== undefined
            }

            return true
        })

        return hasFieldsValues && hasValidValues;
    }

    isValidAboutPage() {
        return !!this.pageTitle && !!this.description && !!this.slug;
    }
}

class FieldHandler {
    static createTextInputElement(field) {
        const label = document.createElement('label');
        label.classList.add('input__outer');
        label.classList.add('field');
        label.innerHTML = `<h4>${field.name}</h4>
                                   <div class="input big">
                                     <input type="text" placeholder="" ${field.required ? 'required' : ''} />
                                   </div>`;
        return label;
    }

    static createCheckboxElement(field) {
        const label = document.createElement('label');
        label.classList.add('check__outer');
        label.classList.add('field');
        label.innerHTML = `<h4>${field.name}</h4>
                                   <label class="check">
                                     <div class="check__box">
                                       <input type="checkbox" ${field.required ? 'required' : ''} />
                                       <span></span>
                                     </div>
                                     <p class="small">Label for the checkbox</p>
                                   </label>`;
        return label;
    }

    static createDatePickerElement(field) {
        const label = document.createElement('label');
        label.classList.add('input__outer');
        label.classList.add('field');
        label.innerHTML = `<h4>${field.name}</h4>
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

    static createFieldElement(field) {
        switch (field.type.id) {
            case 'text':
                return this.createTextInputElement(field);
            case 'bool':
                return this.createCheckboxElement(field);
            case 'date':
                return this.createDatePickerElement(field);
            default:
                console.error(`Unsupported field type: ${field.type.id}`);
                return null;
        }
    }
}

class PageManager {
    constructor() {
        this.selectedGoal = new SelectedGoal();
    }

    attachEventListeners() {
        this.handleProjectSelection();
        this.handleGoalAmount();
        this.handleCurrencySelection();
        this.handleHasEndDate();
        this.handlePageTitle();
        this.handlePageDescription();
        this.handlePageSlug();
        this.handleCreatePage();
        this.handleMoveToAboutTab();
        this.handleEndDateToggle();
    }

    handleProjectSelection() {
        document.getElementById("goal-selection").addEventListener('click', event => {
            const { value, type, customFields } = event.target.dataset;
            this.selectedGoal.value = value;
            this.selectedGoal.type = type;
            this.selectedGoal.fields = customFields ? JSON.parse(customFields) : [];

            this.toggleGoalAmountField();
            this.appendOrRemoveCustomFields();
        });
    }

    toggleGoalAmountField() {
        const elements = document.getElementsByClassName('goal-fields');
        for (const element of elements) {
            element.style.display = this.selectedGoal.value ? 'inherit' : 'none';
        }
    }

    appendOrRemoveCustomFields() {
        const fields = document.getElementById('goal-custom-fields').parentElement.querySelectorAll('.field');
        const fieldsContainer = document.getElementById('goal-custom-fields');

        if (!this.selectedGoal.fields.length) {
            fields.forEach(field => field.remove());
            return;
        }

        const fragment = document.createDocumentFragment();

        this.selectedGoal.fields.forEach(field => {
            const fieldElement = FieldHandler.createFieldElement(field);

            if (fieldElement) {
                fieldElement.addEventListener('input', (event) => {
                    const { alias, type } = field;


                    const value = type.id === "bool" ? event.srcElement.checked : event.target.value;

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
            this.selectedGoal.currency = document.getElementById('goal-curreny').value;
            this.toggleAboutPageTab();
        });
    }

    handleCurrencySelection() {
        document.getElementById('goal-curreny').addEventListener('change', event => {
            this.selectedGoal.currency = event.target.value;
            const { symbol } = event.target.selectedOptions[0].dataset;
            document.querySelector("[for='goal-amount']").innerHTML = symbol;
            this.toggleAboutPageTab();
        });
    }

    handleHasEndDate() {
        document.getElementById('endDate').addEventListener('blur', event => {
            this.selectedGoal.endDate = event.target.value;
        });
    }

    handlePageTitle() {
        document.getElementById('pageTitle').addEventListener('blur', event => {
            this.selectedGoal.pageTitle = event.target.value;
            this.toggleCreatePageButton();
        });
    }

    handlePageDescription() {
        document.getElementById('page-description').addEventListener('blur', event => {
            this.selectedGoal.description = event.target.value;
            this.toggleCreatePageButton();
        });
    }

    handlePageSlug() {
        document.getElementById('slug').addEventListener('blur', event => {
            this.selectedGoal.slug = event.target.value;
            this.toggleCreatePageButton();
        });
    }

    handleEndDateToggle() {
        document.getElementById('checkD').addEventListener('input', e => {
            const isChecked = e.srcElement.checked;

            const picker = document.querySelector('.setting__date-input');

            isChecked ? picker.classList.add('active') : picker.classList.remove('active')
        })
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

    handleCreatePage() {
        document.getElementById('create-page-btn').addEventListener("click", async () => {
            if (!(this.selectedGoal.isValidGoal() && this.selectedGoal.isValidAboutPage())) {
                return;
            }

            const customFieldsReq = this.selectedGoal.fieldsValues.map(fieldValue => ({
                alias: fieldValue.alias,
                [fieldValue.type.id]: fieldValue.value
            }));

            const createPageReq = {
                title: this.selectedGoal.pageTitle,
                slug: this.selectedGoal.slug,
                campaignId: 'b43b962f-6f7a-4eee-8ab5-c5119d1d3b37',
                endDate: this.selectedGoal.endDate,
                allocations: [{
                    amount: +this.selectedGoal.amount,
                    goalId: this.selectedGoal.value,
                    feedbackNewCustomFields: customFieldsReq.length ? { entries: customFieldsReq } : undefined
                }]
            };

            try {
                const isTitleAvailable = await client.checkTitle(createPageReq);

                if (!isTitleAvailable) {
                    document.getElementById('pageTitle').parentElement.classList.add("error")

                }

                if (isTitleAvailable) {
                    const response = await client.createFundraiser(createPageReq);
                    window.location.href = response
                }

            } catch (error) {
                this.displayErrorMessages(error);
            }
        });
    }

    isPlainObject(value) {
        return typeof value === "object" && value !== null && value.constructor === Object;
    }

    displayErrorMessages(error) {
        const container = document.getElementById('errorMessage2');
        if (error.errors) {
            const fragment = document.createDocumentFragment();
            container.querySelector('.detail').innerHTML = '';

            if (error?.errors?.length) {
                Object.values(error.errors).forEach(e => {
                    if (Array.isArray(e)) {
                        e.forEach(message => {
                            const elm = document.createElement('li');
                            elm.textContent = isPlainObject(message) ? message.error : message;
                            fragment.appendChild(elm);
                        });
                    } else {
                        const elm = document.createElement('span');
                        elm.textContent = e.message || e.error || e;
                        fragment.appendChild(elm);
                    }
                });
            }

            if (typeof error === 'string') {
                container.querySelector('.detail').textContent = error;
            }


            container.querySelector('.detail').appendChild(fragment);
            container.style.display = 'inherit';
        }
    }
}

const pageManager = new PageManager();
pageManager.attachEventListeners();


//TODO: Worke Left to do for capturing campaignid & account referrence. need to circle back once those done