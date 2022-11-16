/// <reference path="../core.ts" />
'use strict';

interface JQuery {
    datepicker(options: any): JQuery;
}

interface DatepickerOptions {
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
    $iconEye: JQuery;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.cachedDOMElements();
        this.renderDatetimePicker();
        this.registerEvents();
    }

    cachedDOMElements = () => {
        this.$form = this.$("#apikey-form");
        this.$datePicker = this.$("#dpkContainer");
        this.$ckbAll = this.$("#ckbAll");
        this.$iconEye = this.$(".icon-eye");
    }

    registerEvents = () => {
        this.$ckbAll.click(this.handleSelectAllScopes);
        this.$("input[name='scope']").click(this.handleSelectScopes);
        this.$iconEye.click(this.showPass);
        this.handleSubmitForm();
    }

    showPass = () => {
        var inputPass = this.$("#passphrase");
        if (inputPass.val().length < 1) {
            return;
        }
        if (inputPass.hasClass("input-pass")) {
            inputPass.removeClass("input-pass")
        }
        else {
            inputPass.addClass("input-pass")
        }
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
        const scopes = this.getScopeValue();
        const self = this;
        this.$.ajax({
            type: 'POST',
            url: 'CreateApiKey',
            data: { apiName, passPhrase, expirationDate, scopes },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                const url = alpha.urls.mainRootPath + `ApiKey/CreateApiKeyResult?apiname=${data.ApiName}&key=${data.Key}&secret=${data.ApiSecret}`;
                self.topWindow.popupManager.open(url, "API Key", 800, 300, null, null, null, 335, 150);
            },
            complete: function (jqXHR, textStatus) {

            }
        })
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
