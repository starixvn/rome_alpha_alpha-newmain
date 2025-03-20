class RebateAndRetentionTour extends BaseTourController {
  protected maxPage = 5;
}

$(() => {
  const rebateAndRetention = new RebateAndRetentionTour($);
  rebateAndRetention.init();
})