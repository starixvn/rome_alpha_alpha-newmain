/// <reference path="../core.ts" />

interface JQuery {
    datepicker(options: any): JQuery;
}

class ApiKeyStatistics {
    $: JQueryStatic;
    topWindow: any;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    $dateContainer: JQuery

    init = () => {
        this.$dateContainer = this.$("#dateContainer");
        this.initDateTimePicker();
    }

    initDateTimePicker = () => {
        this.$dateContainer.datepicker({
            minDate: "20-10-2022",
        }).data("DateTimePicker").maxDate("30-10-2022");
    }
}

$(function () {
    const apiKeyStatistics = new ApiKeyStatistics($, window.top);
    apiKeyStatistics.init();
});
