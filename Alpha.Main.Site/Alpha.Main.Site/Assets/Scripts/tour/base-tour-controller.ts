class BaseTourController {
    protected $: JQueryStatic;
    protected currentPage = 1;
    protected maxPage = 5;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    public init() {
        this.$(".page-btn-next").click(() => {
            this.currentPage++;
            this.showHideNextPrevButton();
            this.$(".page-index").html(this.currentPage.toString());
            this.$('.content-page').addClass('hidden');
            this.$('.content-page-' + this.currentPage).removeClass('hidden');
        });

        this.$(".page-btn-prev").click(() => {
            this.currentPage--;

            this.showHideNextPrevButton();
            this.$(".page-index").html(this.currentPage.toString());
            this.$('.content-page').addClass('hidden');
            this.$('.content-page-' + this.currentPage).removeClass('hidden');
        });
    };

    showHideNextPrevButton = () => {
        if (this.currentPage === this.maxPage) {
            this.$(".page-btn-next").addClass('hidden');
        } else {
            this.$(".page-btn-next").removeClass('hidden');
        }

        if (this.currentPage === 1) {
            this.$(".page-btn-prev").addClass('hidden');
        } else {
            this.$(".page-btn-prev").removeClass('hidden');
        }
    };
}