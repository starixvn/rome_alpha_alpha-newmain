class ProfilePictureTour extends BaseTourController {
    protected maxPage = 3;

    public init() {
        super.init();
        this.$("#lnk-profile-picture").attr("href", this.$("#ProfilePictureUrl").val());
    };
}

$(() => {
    const tour = new ProfilePictureTour($);
    tour.init();
})