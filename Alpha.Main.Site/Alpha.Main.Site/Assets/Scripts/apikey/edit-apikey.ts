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

    constructor($: JQueryStatic, topWindow: any, clipboard: any) {
        this.$ = $;
        this.topWindow = topWindow;
        this.clipboard = clipboard;
    }

    init = () => {
        this.cachedDOMElements();
        this.renderDatetimePicker();
        this.handleSelectScopes();
        this.registerEvents();
    }

    cachedDOMElements = () => {
        this.$datePicker = this.$("#dpkContainer");
        this.$datePickerInput = this.$("#expirationdate");
        this.$ckbAll = this.$("#ckbAll");
        this.$btnCancel = this.$("#btnCancel");
        this.$btnSubmit = this.$("#btnSubmit");
        this.$errorMessage = this.$("#error-message");
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
                    parent.window.location.href = parent.window.location.href;
                }
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

    renderDatetimePicker = () => {
        const options: DatepickerOptions = {
            allowInputToggle: true,
            format: "MM/DD/YYYY",
            defaultDate: null
        }
        this.$datePicker.datepicker(options);
    }
}

$(function () {
    const editApiKey = new EditApiKey($, window.top, new Clipboard('.icon-content-copy'));
    editApiKey.init();
});
