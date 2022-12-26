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

'use strict';
class CreateApiKey {
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
        this.renderDropdown();
        this.renderDatetimePicker();
        this.handleSubmitForm();
    }

    cachedDOMElements = () => {
        this.$form = this.$("#apikey-form");
        this.$datePicker = this.$("#dpkContainer");
        this.$datePickerInput = this.$("#expirationdate");
    }

    renderDropdown = () => {
        this.multiSelectionDropdownDefault.render();
    }

    renderDatetimePicker = () => {
        const options: DatepickerOptions = {
            allowInputToggle: true,
            format: "MM/DD/YYYY",
            defaultDate: null
        }
        this.$datePicker.datepicker(options);
    }

    handleSubmitForm = () => {
        const self = this;
        this.$form.submit(function (e) {
            self.handleCreateApiKey();
            e.preventDefault();
        })
    }

    handleCreateApiKey = () => {
        const apiName = this.$("#apiname").val();
        const passPhrase = this.$("#passphrase").val();
        const expirationDate = this.$("#expirationdate").val();
        const scopes = this.multiSelectionDropdownDefault.getSelectedItems().join(",");
        const self = this;
        this.$.ajax({
            type: 'POST',
            url: 'CreateApiKey',
            data: { apiName, passPhrase, expirationDate, scopes },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                self.topWindow.bootboxAlert(self.renderCreateApiKeySuccess(data), undefined);
            },
            complete: function (jqXHR, textStatus) {

            },
            error: function (jqXHR) {

            }
        })
    }

    renderCreateApiKeySuccess = (data) => {
        return [
            `<div>API Key Name: <p>${data.ApiName}</p></div>`,
            `<div>Key: <p>${data.Key}</p></div>`,
            `<div>API Secret: <p>${data.ApiSecret}</p></div>`,
        ].join("");
    }
}

$(function () {
    const createApiKey = new CreateApiKey($, window.top, new MultiSelectionDropdownDefaultComponent($));
    createApiKey.init();
});
