class Message {
    private $: JQueryStatic;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    registerEvent = () => {
        this.onClickTab('.public-tab');
        this.onClickTab('.special-tab');
        this.onClickTab('.private-tab');

        this.initDateTimePicker('#fromDateContainer');
        this.initDateTimePicker('#toDateContainer');
    }

    private onClickTab = (tabName: string) => {
        let thisTab = this.$(tabName);

        thisTab.click(() => {
            const ajaxSetting = {
                url: alpha.urls.mainRootPath + '/Message/Index',
                data: { messageType: thisTab.attr('id') },
                method: 'GET',
                success: (responseHtml) => {
                    setTimeout(this.writeDocument(responseHtml), 100);
                },
                error: () => this.$.unblockUI()
            };

            this.$.blockUI();
            this.$.ajax(ajaxSetting);
        });
    }

    private initDateTimePicker = (id: string) => {
        this.$(id).datetimepicker({
            allowInputToggle: true,
            format: 'MM/DD/YYYY'
        });
    }

    private writeDocument = (html: string) => {
        const newDoc = document.open("text/html", "replace");
        newDoc.write(html);
        newDoc.close();
    }
}

$(() => {
    const message = new Message($);
    message.registerEvent();
});