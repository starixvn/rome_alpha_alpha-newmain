class NewFeatureTour extends BaseTourController {
    init() {
        super.init();
        this.$("#lnk-new-feature").attr("href", this.$("#NewFeatureUrl").val());
    }
}

$(() => new NewFeatureTour($).init());