/// <reference path="../core.ts" />
'use strict';

interface JQuery {
    datepicker(options: any): JQuery;
}

interface DatepickerOptions {
    allowInputToggle: boolean;
    format: string;
    defaultDate: any;
    useCurrent: false;
}

declare var Clipboard: any;

class EditApiKey {
    $: JQueryStatic;
    topWindow: any;
    clipboard: any;
    $form: JQuery;
    $datePicker: JQuery;
    $datePickerInput: JQuery;
    $ckbAll: JQuery;
    $btnSubmit: JQuery;
    $btnCancel: JQuery;
    $errorMessage: JQuery;

    apiKeyName: any;
    apiKeyExpirationDate: any;
    apiKeyScopes: any;

    constructor($: JQueryStatic, topWindow: any, clipboard: any) {
        this.$ = $;
        this.topWindow = topWindow;
        this.clipboard = clipboard;
    }

    init = () => {
        this.cachedDOMElements();
        this.renderDatetimePicker();
        this.cacheOldApiKeyValue();
        this.handleSelectScopes();
        this.registerEvents();
    }

    resources = {
        pleaseInputAPIKeyName: (<HTMLInputElement>document.getElementById('PleaseInputAPIKeyName')).value,
        apiKeyNameCanNotContainSpecialCharacters: (<HTMLInputElement>document.getElementById('APIKeyNameCanNotContainSpecialCharacters')).value,
        noMoreThan50Characters: (<HTMLInputElement>document.getElementById('NoMoreThan50Characters')).value,
        cantSelectTimeFromPast: (<HTMLInputElement>document.getElementById('CantSelectTimeFromPast')).value,
        scopesIsRequired: (<HTMLInputElement>document.getElementById('ScopesIsRequired')).value,
        updateSuccessful: (<HTMLInputElement>document.getElementById('UpdateSuccessful')).value,
    }

    cachedDOMElements = () => {
        this.$datePicker = this.$("#dpkContainer");
        this.$datePickerInput = this.$("#expirationdate");
        this.$ckbAll = this.$("#ckbAll");
        this.$btnCancel = this.$("#btnCancel");
        this.$btnSubmit = this.$("#btnSubmit");
        this.$errorMessage = this.$("#error-message");
    }

    cacheOldApiKeyValue = () => {
        this.apiKeyName = this.$("#apiname").val();
        this.apiKeyExpirationDate = this.$("#expirationdate").val();
        this.apiKeyScopes = this.getScopeValue();
    }

    registerEvents = () => {
        this.$ckbAll.click(this.handleSelectAllScopes);
        this.$("input[name='scope']").click(this.handleSelectScopes);
        this.$btnCancel.click(this.handleCancelForm);
        this.$btnSubmit.click(this.handleUpdateApiKey);

        const self = this;
        if (window.document.queryCommandSupported
            && window.document.queryCommandSupported("copy")) {
            this.clipboard
                .on("success", function (e) {
                    self.$(e.trigger).tooltip("show");
                })
                .on("error", function (e) {
                    self.$(e.trigger).remove();
                    window.console.error("Action:", e.action);
                    window.console.error("Trigger:", e.trigger);
                });
        }
    }

    handleCancelForm = () => {
        this.topWindow.popupManager.close();
    }

    handleUpdateApiKey = (e: any) => {
        e.preventDefault();
        const apiName = this.$("#apiname").val();
        const apikey = this.$("#key").val();
        const expirationDate = this.$("#expirationdate").val();
        const scopes = this.getScopeValue();
        if (apiName == this.apiKeyName && expirationDate == this.apiKeyExpirationDate && scopes == this.apiKeyScopes) {
            parent.window.location.href = this.removeBookmarksInUrl(parent.window.location.href);
            return;
        }
        const error = this.validateFormInput(apiName, expirationDate, scopes);
        if (error) {
            alpha.showMessage(this.$errorMessage, new Result({ Code: 1, Message: error }));
        }
        else {
            const self = this;
            this.$.ajax({
                type: 'POST',
                url: alpha.urls.mainRootPath + `ApiKey/UpdateApiKey`,
                data: { apikey, apiName, expirationDate, scopes },
                dataType: 'json',
                success: function (data, textStatus, jqXHR) {
                    if (data.IsFailed) {
                        alpha.showMessage(self.$errorMessage, new Result(data));
                    } else {
                        alpha.showMessage(self.$errorMessage, new Result({ Code: 0, Message: self.resources.updateSuccessful }))
                        parent.window.location.href = self.removeBookmarksInUrl(parent.window.location.href);
                    }
                },
                complete: function (jqXHR, textStatus) {

                }
            })
        }
    }

    validateFormInput = (apiName: string, expirationDate: Date, scopes: string) => {
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
        const isPastDate = new Date(expirationDate.toString()) < new Date();
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

    renderDatetimePicker = () => {
        const options: DatepickerOptions = {
            useCurrent: false,
            allowInputToggle: true,
            format: "MM/DD/YYYY",
            defaultDate: null
        }
        this.$datePicker.datepicker(options);
    }

    removeBookmarksInUrl = (url: string) => {
        if (typeof url !== 'string') {
            return null;
        }

        var i = url.indexOf("#");
        if (i === -1) {
            return url;
        }

        return url.substr(0, i);
    }

}

$(function () {
    const editApiKey = new EditApiKey($, window.top, new Clipboard('.icon-content-copy'));
    editApiKey.init();
});
