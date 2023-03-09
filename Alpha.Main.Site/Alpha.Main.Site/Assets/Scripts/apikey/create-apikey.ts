/// <reference path="../core.ts" />
'use strict';

interface JQuery {
    datepicker(options: any): JQuery;
}

interface DatepickerOptions {
    minDate: any;
    useCurrent: any;
    allowInputToggle: boolean;
    format: string;
    defaultDate: any;
}

class CreateApiKey {
    $: JQueryStatic;
    topWindow: any;
    $form: JQuery;
    $datePicker: JQuery;
    $ckbAll: JQuery;
    $showPass: JQuery;
    $errorMessage: JQuery;
    $showURL: JQuery;
    $urlMask: JQuery;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.cachedDOMElements();
        this.renderDatetimePicker();
        this.registerEvents();
    }

    resources = {
        apiKey: (<HTMLInputElement>document.getElementById('APIKey')).value,
        pleaseInputAPIKeyName: (<HTMLInputElement>document.getElementById('PleaseInputAPIKeyName')).value,
        apiKeyNameCanNotContainSpecialCharacters: (<HTMLInputElement>document.getElementById('APIKeyNameCanNotContainSpecialCharacters')).value,
        noMoreThan50Characters: (<HTMLInputElement>document.getElementById('NoMoreThan50Characters')).value,
        pleaseInputPassPhrase: (<HTMLInputElement>document.getElementById('PleaseInputPassPhrase')).value,
        APIKey_PassphraseValidate: (<HTMLInputElement>document.getElementById('APIKey_PassphraseValidate')).value,
        apiPassPhraseMustBeBetweenCharacters: (<HTMLInputElement>document.getElementById('APIPassPhraseMustBeBetweenCharacters')).value,
        cantSelectTimeFromPast: (<HTMLInputElement>document.getElementById('CantSelectTimeFromPast')).value,
        scopesIsRequired: (<HTMLInputElement>document.getElementById('ScopesIsRequired')).value,
        currentDate: (<HTMLInputElement>document.getElementById('CurrentDate')).value,
    }

    cachedDOMElements = () => {
        this.$form = this.$("#apikey-form");
        this.$datePicker = this.$("#dpkContainer");
        this.$ckbAll = this.$("#ckbAll");
        this.$showPass = this.$(".show-pass");
        this.$errorMessage = this.$("#error-message");
        this.$showURL = this.$(".dummy-text");
        this.$urlMask = this.$(".service-url-mask");
    }

    registerEvents = () => {
        this.$ckbAll.click(this.handleSelectAllScopes);
        this.$("input[name='scope']").click(this.handleSelectScopes);
        this.$showPass.click(this.showPass);
        this.$showURL.click(this.showURL);
        this.handleSubmitForm();
    }

    showPass = () => {
        var inputPass = this.$("#passphrase");
        this.changeEyeIcon(this.$showPass);
        if (inputPass.hasClass("input-pass")) {
            inputPass.removeClass("input-pass")
        }
        else {
            inputPass.addClass("input-pass")
        }
    }

    showURL = () => {
        this.$urlMask.removeClass('close');
        this.$urlMask.addClass('open');
    }

    changeEyeIcon = (eyeElm) => {
        if (eyeElm.hasClass("icon-eye-close")) {
            eyeElm.removeClass("icon-eye-close");
            eyeElm.addClass("icon-eye");
        }
        else {
            eyeElm.removeClass("icon-eye");
            eyeElm.addClass("icon-eye-close");
        }
    }

    renderDatetimePicker = () => {
        const minDate = new Date(this.resources.currentDate);
        const options: DatepickerOptions = {
            minDate: new Date(minDate.getFullYear(), minDate.getMonth(), minDate.getDate()),
            useCurrent: false,
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
        const scopes = this.getScopeValue();
        const error = this.validateFormInput(apiName, passPhrase, expirationDate, scopes);
        if (error) {
            alpha.showMessage(this.$errorMessage, new Result({ Code: 1, Message: error }));
        }
        else {
            const self = this;
            this.$.ajax({
                type: 'POST',
                url: 'CreateApiKey',
                data: { apiName, passPhrase, expirationDate, scopes },
                dataType: 'json',
                success: function (data, textStatus, jqXHR) {
                    if (data.ApiKeyResult) {
                        self.resetForm();
                        const url = alpha.urls.mainRootPath + `ApiKey/CreateApiKeyResult?apiname=` + encodeURIComponent(data.ApiKeyResult.ApiName)
                            + `&key=` + encodeURIComponent(data.ApiKeyResult.Key)
                            + `&secret=` + encodeURIComponent(data.ApiKeyResult.ApiSecret)
                        self.topWindow.popupManager.open(url, self.resources.apiKey, 362, 530, null, null, null, 335, 150, null);
                    } else {
                        alpha.showMessage(self.$errorMessage, new Result(data.Result));
                    }
                },
                complete: function (jqXHR, textStatus) {

                }
            })
        }
    }

    resetForm = () => {
        this.$("#apiname").val("");
        this.$("#passphrase").val("");
        this.$("#expirationdate").val("");
        this.$("#error-message").empty();
    }

    validateFormInput = (apiName: string, passPhrase: string, expirationDate: Date, scopes: string) => {
        const regexName = /^[a-zA-Z0-9\s]+$/m;
        if (apiName == '') {
            return this.resources.pleaseInputAPIKeyName;
        }
        if (!regexName.test(apiName)) {
            return this.resources.apiKeyNameCanNotContainSpecialCharacters;
        }
        if (apiName.length > 50) {
            return this.resources.noMoreThan50Characters;
        }
        if (passPhrase == '') {
            return this.resources.pleaseInputPassPhrase;
        }
        const regexPassPhrase = /^[\w!"#$%&'()*+,-./:;<=>?@[\]^_\`{|}~]+$/m;
        if (!regexPassPhrase.test(passPhrase)) {
            return this.resources.APIKey_PassphraseValidate;
        }
        if (passPhrase.length < 7 || passPhrase.length > 32) {
            return this.resources.apiPassPhraseMustBeBetweenCharacters;
        }
        const currentDate = new Date(this.resources.currentDate);
        const isPastDate = new Date(expirationDate.toString()) < new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate());
        if (isPastDate) {
            return this.resources.cantSelectTimeFromPast;
        }
        if (scopes == "") {
            return this.resources.scopesIsRequired;
        }

        return null;
    }

    getScopeValue = () => {
        const self = this;
        const scopes = [];
        this.$.each(this.$("input[name='scope']:checked"), function (index, checkbox) {
            scopes.push(self.$(checkbox).val());
        });

        return scopes.join(",");
    }

    handleSelectAllScopes = () => {
        const self = this;
        var isChecked = this.$ckbAll.is(":checked");
        this.$.each(this.$("input[name='scope']"), function (index, checkbox) {
            if (isChecked) {
                self.$(checkbox).prop('checked', true).attr('checked', 'checked');
            }
            else {
                self.$(checkbox).prop('checked', false).removeAttr('checked');
            }
        });
    }

    handleSelectScopes = () => {
        if (this.isSelectAllScopes()) {
            this.$ckbAll.prop('checked', true).attr('checked', 'checked');
            return;
        }
        this.$ckbAll.prop('checked', false).removeAttr('checked');
    }

    isSelectAllScopes = () => {
        const self = this;
        let isCheckAll = true;
        this.$.each(this.$("input[name='scope']"), function (index, checkbox) {
            if (!self.$(checkbox).is(":checked")) {
                isCheckAll = false;
            }
        });

        return isCheckAll;
    }
}

$(function () {
    const createApiKey = new CreateApiKey($, window.top);
    createApiKey.init();
});
