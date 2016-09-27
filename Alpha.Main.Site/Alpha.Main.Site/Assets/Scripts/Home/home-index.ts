class HomeIndex {
    constructor() {        
        this.loadInfo('#stats');
        this.loadInfo('#notice');
    };

    private loadInfo = (id: string): void => {
        let $component = $(id),
            url = $component.attr('action-url');

        $.get(url, function (responseHtml) {
            $component.html(responseHtml);
        })
    }
}

$(document).ready(function () {
    new HomeIndex();
    console.log("document ready and create HomeIndex instance");
});

$(function () {
    $('[data-toggle="tooltip"]').tooltip();
})