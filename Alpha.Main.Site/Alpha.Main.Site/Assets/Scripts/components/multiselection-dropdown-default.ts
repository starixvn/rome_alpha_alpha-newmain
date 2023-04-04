/// <reference path="../core.ts" />
"use strict";
interface JQuery {
    multiselect(option: any): void;
}

class MultiSelectionDropdownDefaultOption {
    constructor(
        public dropdownId: string = "multiSelectionDropdownId",
        public inputId: string = "selectedId",) {
    }
}

class MultiSelectionDropdownDefaultComponent {
    $: JQueryStatic;
    $hiddenInput: JQuery;
    $multiSelectionDropdown: JQuery;

    options = new MultiSelectionDropdownDefaultOption();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: MultiSelectionDropdownDefaultOption = new MultiSelectionDropdownDefaultOption()) => {
        this.options = <MultiSelectionDropdownDefaultOption>this.$.extend({}, this.options, options);

        this.$multiSelectionDropdown = this.$("#" + this.options.dropdownId);
        this.$hiddenInput = this.$("#", this.options.inputId);
        const selectAllText = this.$multiSelectionDropdown.data("select-all-text");

        this.$multiSelectionDropdown.multiselect({
            maxHeight: 300,
            numberDisplayed: 2,
            selectAllText: selectAllText,
            includeSelectAllOption: true,

        })

        return this.$multiSelectionDropdown;
    };

    getSelectedItems = () => {
        const selected = [];
        const this$ = this.$;
        this.$multiSelectionDropdown.find("option:selected").each(function () {
            selected.push(this$(this).val());
        });
        return selected;
    };
}