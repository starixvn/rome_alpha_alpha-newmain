var dashboardManager;

jQuery(document).ready(function () {
    dashboardManager.init();
});

var DashboardManager = function () {
    var self = this;
    self.init = function () {
        self.initElements();
        self.bindEvents();
    };
    self.initElements = function () {
        customerProfile.loadCustomerProfile();
    };

    self.bindEvents = function () {
        jQuery(".top-message").click(function () {
            ShowPopupTop(false);
        });
    };
};


var dashboardManager = new DashboardManager();