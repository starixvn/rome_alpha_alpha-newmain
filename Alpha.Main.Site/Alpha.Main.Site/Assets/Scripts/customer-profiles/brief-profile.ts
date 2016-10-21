/// <reference path="../core.ts" />

'use strict';
//declare var $: any;

var briefProfile = {
    maxCountShowTooltip: 4,
    isShowEditProfilePictureTooltip: false,
    isShowNickNameProfileTooltip: false,
    autoCloseToolTipTime: 20000,

    domElements: {
        toolTipBox: null,
        profileTooltip: null,
        changePictureLink: null,
        profileContent: null,
        profilePicture: null
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
        briefProfile.domElements.profileTooltip
            .off('click')
            .on('click', briefProfile.hideProfileTooltip);

        briefProfile.domElements.toolTipBox
            .off('click')
            .on('click', briefProfile.hideTooltipIntroNickname);

        briefProfile.domElements.profilePicture.click(function () {
            menuBar.collapseSideBar();
        });
    },

    cachedDomElements: function () {
        briefProfile.domElements.toolTipBox = $('#tooltip-box');
        briefProfile.domElements.profileTooltip = $('#profileTooltip');
        briefProfile.domElements.changePictureLink = $('#changePictureLink');
        briefProfile.domElements.profileContent = $('#profile-content');
        briefProfile.domElements.profilePicture = $("#logoProfileImg");
    },

    hideTooltipIntroNickname: function () {
        if (briefProfile.domElements.toolTipBox.length > 0) {
            briefProfile.domElements.toolTipBox.remove();
        }

        briefProfile.showTooltipProfilePicture();
    },

    hideProfileTooltip: function () {
        if (briefProfile.domElements.profileTooltip.length > 0) {
            briefProfile.domElements.profileTooltip.remove();
        }
    },

    hideAllTooltips: function () {
        if (briefProfile.domElements.toolTipBox.length > 0) {
            briefProfile.domElements.toolTipBox.remove();
        }

        briefProfile.hideProfileTooltip();
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
    }
};