class HomeIndex {
    constructor() {
        this.loadInfo('#stats');
        this.loadInfo('#notice');
    };

    private loadInfo = (id: string): void => {
        let $component = $(id),
            url = $component.attr('action-url');

        if (url !== '' || typeof url !== 'undefined') {
            $.get(url, function (responseHtml) {
                $component.html(responseHtml);
            })
        }
    }
}

$(document).ready(function () {
    new HomeIndex();
});

$(function () {
    $('[data-toggle="tooltip"]').tooltip();
})