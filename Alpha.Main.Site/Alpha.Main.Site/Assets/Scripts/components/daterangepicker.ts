/////// <reference path="../core.ts" />

"use strict";
interface JQuery {
    nexDatePicker(option: any): void;
}

class DateRangePickerOption {
    constructor(
        public selectorId: string = "daterange-picker",
        public reportFormId: string = "report-form",
        public constraintFromTo: number = 6,
        public onChange: Function = null) {
        this.selectorId = selectorId;
        this.reportFormId = reportFormId;
        this.onChange = onChange;
        this.constraintFromTo = constraintFromTo;
    }
}

class DateRangePickerComponent {
    $: JQueryStatic;

    options = new DateRangePickerOption();
    quickChooseType = {
        specificDates: 7
    };
    isCompleteInitDateRangePicker = false;
    $reportForm: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: DateRangePickerOption = new DateRangePickerOption()) => {
        this.options = this.$.extend({}, this.options, options);

        let dateRangePicker = this.$("#" + this.options.selectorId);
        let currentDate = dateRangePicker.data("currentdate");
        let minDate = dateRangePicker.data("mindate");
        let maxDate = dateRangePicker.data("maxdate");
        this.$reportForm = this.$("#" + this.options.reportFormId);

        dateRangePicker.nexDatePicker({
            datePickerFormat: {
                dateFormat: "MM/DD/YYYY"
            },
            selectedItem: dateRangePicker.data("selecteddaterange"),
            minDate: new Date(minDate),
            maxDate: maxDate === "" ? null : maxDate,
            currentDate: new Date(currentDate),
            onSelectDatePicker: () => {
                if (this.options.onChange) {
                    this.options.onChange();
                }
            },
            onQuickChooseChange: (selectValue: number) => {
                if (this.isCompleteInitDateRangePicker && selectValue !== this.quickChooseType.specificDates) {
                    this.$reportForm.submit();
                }
            },
            ConstraintFromTo: options.constraintFromTo
        });

        this.isCompleteInitDateRangePicker = true;

        return dateRangePicker;
    };
}