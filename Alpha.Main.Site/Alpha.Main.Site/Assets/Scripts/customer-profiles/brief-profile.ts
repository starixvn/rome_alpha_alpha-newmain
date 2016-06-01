/// <reference path="../core.ts" />

'use strict';
declare var $: any;

var briefProfile = {
    maxCountShowTooltip: 4,
    isShowEditProfilePictureTooltip: false,
    isShowNickNameProfileTooltip: false,

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
        briefProfile.isShowEditProfilePictureTooltip = briefProfile.domElements.profileContent.data('is-default-avatar')
            && briefProfile.getCountNickNameToolTip() < briefProfile.maxCountShowTooltip;
    },

    checkAndShowNickNameTooltip: function () {
        var countNickNameToolTip = briefProfile.getCountNickNameToolTip();

        if (briefProfile.domElements.profileContent.data('nick-name').length === 0
            && countNickNameToolTip < briefProfile.maxCountShowTooltip) {
            briefProfile.isShowNickNameProfileTooltip = true;
        }

        briefProfile.setCountNickNameToolTip(++countNickNameToolTip);

        briefProfile.showTooltipIntroNickname();
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
        if (briefProfile.domElements.profileTooltip.length > 0
            && briefProfile.isShowEditProfilePictureTooltip) {
            briefProfile.domElements.profileTooltip.show();

            setTimeout(function () {
                briefProfile.hideProfileTooltip();
            }, 20000);
        }
    },

    showTooltipIntroNickname: function () {
        if (briefProfile.domElements.toolTipBox.length > 0
            && briefProfile.isShowNickNameProfileTooltip) {
            briefProfile.domElements.toolTipBox.show();

            setTimeout(function () {
                briefProfile.hideTooltipIntroNickname();
            }, 20000);
        }
        else {
            briefProfile.showTooltipProfilePicture();
        }
    },

    hideAllTooltips: function () {
        briefProfile.hideTooltipIntroNickname();
        if (briefProfile.isShowNickNameProfileTooltip) {
            briefProfile.showTooltipProfilePicture();
            briefProfile.isShowNickNameProfileTooltip = false;
        }
        else {
            briefProfile.hideProfileTooltip();
        }
    }
};