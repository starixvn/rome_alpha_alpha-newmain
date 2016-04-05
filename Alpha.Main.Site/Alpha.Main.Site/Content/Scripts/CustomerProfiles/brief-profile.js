'use strict';

var briefProfile = {
    domElements: {
        toolTipBox: null,
        profileTooltip: null,
        changePictureLink: null,
    },

    init: function () {
        briefProfile.cachedDomElements();
        briefProfile.registerEvents();
    },

    registerEvents: function () {

        $(document).click(function () {
            briefProfile.clickToHide();
        });

        var frameLength = window.parent.frames.length;
        for (var i = 0; i < frameLength; i++) {
            $(window.parent.frames[i].window.document).click(function () {
                briefProfile.clickToHide();
            });
        }

        briefProfile.domElements.changePictureLink
            .off('click.brief-profile')
            .on('click.brief-profile', self.hideProfileTooltip);

        if (briefProfile.domElements.length > 0) {
            setTimeout(function () {
                briefProfile.hideTooltipIntroNickname();
                briefProfile.showTooltipProfilePicture();
            }, 10000);
        }
        else {
            briefProfile.showTooltipProfilePicture();
        }
    },

    cachedDomElements: function () {
        briefProfile.domElements.toolTipBox = $('#tooltip-box');
        briefProfile.domElements.profileTooltip = $('#profileTooltip');
        briefProfile.domElements.changePictureLink = $('#changePictureLink');
    },

    hideTooltipIntroNickname: function () {
        if (briefProfile.domElements.toolTipBox.length > 0) {
            briefProfile.domElements.toolTipBox.remove();
        }
    },

    hideProfileTooltip: function () {
        if (briefProfile.domElements.profileTooltip.length > 0) {
            briefProfile.domElements.profileTooltip.remove();
        }
    },

    showTooltipProfilePicture: function () {
        if (briefProfile.domElements.profileTooltip.length > 0) {
            briefProfile.domElements.profileTooltip.show();

            setTimeout(function () {
                briefProfile.hideProfileTooltip();
            }, 30000);
        }
    },

    clickToHide: function () {
        if (briefProfile.domElements.toolTipBox.length > 0) {
            briefProfile.hideTooltipIntroNickname();
            briefProfile.showTooltipProfilePicture();
        }
        else {
            if (briefProfile.domElements.profileTooltip.length > 0) {
                briefProfile.hideProfileTooltip();
            }
        }
    }
};