var BriefProfile = function () {
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
                self.hideProfileTooltip();
            }, 30000);
        }
    };

    self.clickToHide = function () {
        var tooltipBox = $('#tooltip-box');
        var profileTooltip = $('#profileTooltip');
        if (tooltipBox.length) {
            self.hideTooltipIntroNickname();
            self.showTooltipProfilePicture();
        }
        else {
            if (profileTooltip.length) {
                self.hideProfileTooltip();
            }
        }
    };
    self.bindEvents = function () {
        $(".change-picture-link").click(function () {
            self.hideProfileTooltip();
        });

        $(document).click(function () {
            self.clickToHide();
        });

        var frameLength = window.parent.frames.length;
        for (var i = 0; i < frameLength; i++) {
            $(window.parent.frames[i].window.document).click(function () {
                self.clickToHide();
            });
        }

        var tooltipBox = $('#tooltip-box');
        if (tooltipBox.length) {
            setTimeout(function () {
                self.hideTooltipIntroNickname();
                self.showTooltipProfilePicture();
            }, 10000);
        }
        else {
            self.showTooltipProfilePicture();
        }
    };
};

var briefProfile = new BriefProfile();