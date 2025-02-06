class ProfilePictureTour extends BaseTourController {
    protected maxPage = 5;

    public init() {
        super.init();
        this.$("#lnk-profile-picture").attr("href", this.$("#ProfilePictureUrl").val());
    };
}

$(() => {
    const tour = new ProfilePictureTour($);
    tour.init();
})