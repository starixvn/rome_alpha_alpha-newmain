/// <reference path="../core.ts" />

interface JQuery {
    datepicker(options: any): JQuery;
}

interface DatepickerOptions {
    allowInputToggle: boolean;
    minDate: Date;
    maxDate: Date;
    format: string;
}

class ApiKeyStatistics {
    topWindow: any;
    $: JQueryStatic;
    $datePicker: JQuery;
    $datePickerInput: JQuery;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.cacheElements();
        this.initDateTimePicker();
    }

    cacheElements = () => {
        this.$datePicker = this.$("#dpkContainer");
        this.$datePickerInput = this.$("#input-date");
    };

    initDateTimePicker = () => {
        const options: DatepickerOptions = {
            allowInputToggle: true,
            minDate: new Date(2022, 1, 1),
            maxDate: new Date(2023, 1, 1),
            format: "MM/DD/YYYY"
        }
        this.$datePicker.datepicker(options);
    }
}

$(function () {
    const apiKeyStatistics = new ApiKeyStatistics($, window.top);
    apiKeyStatistics.init();
});
