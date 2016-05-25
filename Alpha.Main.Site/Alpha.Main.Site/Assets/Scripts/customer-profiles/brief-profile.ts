/// <reference path="../core.ts" />

'use strict';
declare var $: any;

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
            && briefProfile.getCountNickNameToolTip() < briefProfile.maxCountNickNameTooltip;

        if (isShowEditProfilePictureTooltip) {
            briefProfile.showTooltipProfilePicture();
        }
    },

    checkAndShowNickNameTooltip: function () {
        var countNickNameToolTip = briefProfile.getCountNickNameToolTip();

        if (briefProfile.domElements.profileContent.data('nick-name').length === 0
                && countNickNameToolTip >= briefProfile.maxCountNickNameTooltip) {
            briefProfile.showTooltipIntroNickname();
        }
        else if (countNickNameToolTip < briefProfile.maxCountNickNameTooltip) {
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
            }, 20000);
        }
    },

    showTooltipIntroNickname: function () {
        if (briefProfile.domElements.toolTipBox.length > 0) {
            briefProfile.domElements.toolTipBox.show();

            setTimeout(function () {
                briefProfile.hideTooltipIntroNickname();
            }, 20000);
        }
    },

    hideAllTooltips: function () {
        briefProfile.hideTooltipIntroNickname();
        briefProfile.hideProfileTooltip();
    }
};