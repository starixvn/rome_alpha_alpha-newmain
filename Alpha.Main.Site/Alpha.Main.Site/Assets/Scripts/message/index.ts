/// <reference path="../core.ts" />

class Message {
    registerEvent = () => {
        this.onClickTab('.public-tab');
        this.onClickTab('.special-tab');
        this.onClickTab('.private-tab');

        this.initDateTimePicker('#fromDateContainer');
        this.initDateTimePicker('#toDateContainer');
    }

    private onClickTab = (tabName: string) => {
        let thisTab = $(tabName);

        thisTab.click(() => {
            const ajaxSetting = {
                url: alpha.urls.mainRootPath + '/Message/Index',
                data: { messageType: thisTab.attr('id') },
                method: 'GET',
                success: (responseHtml) => {
                    setTimeout(() => {
                        const newDoc = document.open("text/html", "replace");
                        newDoc.write(responseHtml);
                        newDoc.close();
                    }, 300);
                }
            };

            $.ajax(ajaxSetting);
        });
    }

    private initDateTimePicker = (id: string) => {
        $(id).datetimepicker({
            allowInputToggle: true,
            format: 'MM/DD/YYYY'
        });
    }
}

$(document).ready(() => {
    const message = new Message();
    message.registerEvent();
});