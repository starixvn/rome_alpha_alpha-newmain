/// <reference path="./notice-message.ts" />

class HomeIndex {
    constructor() {
        this.loadInfo('#stats', () => { $('[data-toggle="tooltip"]').tooltip({ html: true }); });
        this.loadInfo('#notice', this.onCompleteLoadNotice);
    };

    private loadInfo = (id: string, onCompleteLoading: any): void => {
        let $component = $(id),
            url = $component.attr('action-url');

        if (url !== '' || typeof url !== 'undefined') {
            $.get(url, function (responseHtml) {
                $component.html(responseHtml);
                onCompleteLoading();
            })
        }
    }

    private onCompleteLoadNotice = (): void => {
        new NoticeMessage();
    }
}

$(document).ready(function () {
    new HomeIndex();
});
