class ProfilePictureTour extends BaseTourController {
    protected maxPage = 3;
}

$(() => {
    const tour = new ProfilePictureTour($);
    tour.init();
})