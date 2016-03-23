var customerProfile;

var CustomerProfile = function () {
    var self = this;
    self.hideTooltipIntroNickname = function () {
        var tooltipBox = $('#tooltip-box');
        if (tooltipBox.length) {
            tooltipBox.remove();
        }
    };

    self.hideProfileTooltip = function () {
        var profileTooltip = $('#profileTooltip');
        if (profileTooltip.length) {
            profileTooltip.remove();
        }
    };

    self.showTooltipProfilePicture = function () {
        var profileTooltip = $('#profileTooltip');
        if (profileTooltip.length) {
            profileTooltip.css("display", "");
            setTimeout(function () {
                customerProfile.hideProfileTooltip();
            }, 30000);
        }
    };

    self.clickToHide = function () {
        var tooltipBox = $('#tooltip-box');
        var profileTooltip = $('#profileTooltip');
        if (tooltipBox.length) {
            customerProfile.hideTooltipIntroNickname();
            customerProfile.showTooltipProfilePicture();
        }
        else {
            if (profileTooltip.length) {
                customerProfile.hideProfileTooltip();
            }
        }
    };
    self.bindEvents = function () {
        $(".change-picture-link").click(function () {
            customerProfile.hideProfileTooltip();
        });

        var frameLength = window.parent.frames.length;
        for (var i = 0; i < frameLength; i++) {
            $(window.parent.frames[i].window.document).click(function () {
                customerProfile.clickToHide();
            });
        }

        var tooltipBox = $('#tooltip-box');
        if (tooltipBox.length) {
            setTimeout(function () {
                customerProfile.hideTooltipIntroNickname();
                customerProfile.showTooltipProfilePicture();
            }, 10000);
        }
        else {
            customerProfile.showTooltipProfilePicture();
        }
    };
};

customerProfile = new CustomerProfile();