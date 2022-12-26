/// <reference path="../core.ts" />
/// <reference path="../components/multiselection-dropdown-default.ts" />

interface JQuery {
    datepicker(options: any): JQuery;
}

interface DatepickerOptions {
    allowInputToggle: boolean;
    format: string;
    defaultDate: any;
}

class EditApiKey {
    $: JQueryStatic;
    topWindow: any;
    multiSelectionDropdownDefault: MultiSelectionDropdownDefaultComponent;
    $form: JQuery;
    $datePicker: JQuery;
    $datePickerInput: JQuery;

    constructor($: JQueryStatic, topWindow: any, multiSelectionDropdownDefault: MultiSelectionDropdownDefaultComponent) {
        this.$ = $;
        this.topWindow = topWindow;
        this.multiSelectionDropdownDefault = multiSelectionDropdownDefault;
    }

    init = () => {
        this.cachedDOMElements();
        this.renderDatetimePicker();
        this.renderDropdown();
    }

    cachedDOMElements = () => {
        this.$datePicker = this.$("#dpkContainer");
        this.$datePickerInput = this.$("#expirationdate");
    }

    renderDatetimePicker = () => {
        const options: DatepickerOptions = {
            allowInputToggle: true,
            format: "MM/DD/YYYY",
            defaultDate: null
        }
        this.$datePicker.datepicker(options);
    }

    renderDropdown = () => {
        this.multiSelectionDropdownDefault.render();
    }
}

$(function () {
    const editApiKey = new EditApiKey($, window.top, new MultiSelectionDropdownDefaultComponent($));
    editApiKey.init();
});
