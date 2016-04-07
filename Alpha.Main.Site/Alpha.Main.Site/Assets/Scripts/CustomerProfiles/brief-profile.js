'use strict';

var briefProfile = {
    maxCountNickNameTooltip: 4,

    domElements: {
        toolTipBox: null,
        profileTooltip: null,
        changePictureLink: null,
        profileContent: null
    },

    countNickNameToolTipCookieName: null,

    init: function () {
        briefProfile.cachedDomElements();

        briefProfile.checkAndSetShowEditProfilePictureTooltip();
        briefProfile.checkAndShowNickNameTooltip();
        
        briefProfile.registerEvents();
    },

    checkAndSetShowEditProfilePictureTooltip: function () {
        var isShowEditProfilePictureTooltip = briefProfile.domElements.profileContent.data('is-default-avatar')
        && briefProfile.getCountNickNameToolTip() < briefProfile.maxCountNickNameTooltip
        && briefProfile.domElements.profileContent.data('nick-name').length === 0;

        if (isShowEditProfilePictureTooltip) {
            briefProfile.showTooltipProfilePicture();
        }
    },

    checkAndShowNickNameTooltip: function () {
        var countNickNameToolTip = briefProfile.getCountNickNameToolTip();

        if (countNickNameToolTip >= briefProfile.maxCountNickNameTooltip) {
            briefProfile.showTooltipIntroNickname();
        }
        else {
            briefProfile.setCountNickNameToolTip(++countNickNameToolTip)
        }
    },

    getCountNickNameToolTip: function () {
        briefProfile.countNickNameToolTipCookieName = briefProfile.domElements.profileContent.data('introduce-nick-name-tooltip-cookie-name');
        var countNickNameTooltip = $.cookie(briefProfile.countNickNameToolTipCookieName);

        return parseInt(countNickNameTooltip || 0);
    },

    setCountNickNameToolTip: function (value) {
        $.cookie(briefProfile.countNickNameToolTipCookieName, value, { expires: 30, path: '/' });
    },

    registerEvents: function () {
        $(document).click(function () {
            briefProfile.hideAllTooltips();
        });

        var frameLength = window.parent.frames.length;
        for (var i = 0; i < frameLength; i++) {
            $(window.parent.frames[i].window.document).click(function () {
                briefProfile.hideAllTooltips();
            });
        }

        briefProfile.domElements.changePictureLink
            .off('click.brief-profile')
            .on('click.brief-profile', briefProfile.hideProfileTooltip);
    },

    cachedDomElements: function () {
        briefProfile.domElements.toolTipBox = $('#tooltip-box');
        briefProfile.domElements.profileTooltip = $('#profileTooltip');
        briefProfile.domElements.changePictureLink = $('#changePictureLink');
        briefProfile.domElements.profileContent = $('#profile-content');
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

    showTooltipIntroNickname: function () {
        if (briefProfile.domElements.toolTipBox.length > 0) {
            briefProfile.domElements.toolTipBox.show();

            setTimeout(function () {
                briefProfile.hideTooltipIntroNickname();
            }, 30000);
        }
    },

    hideAllTooltips: function () {
        briefProfile.hideTooltipIntroNickname();
        briefProfile.hideProfileTooltip();
    }
};