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
        profilePicture: null,
        adminAndSubAcc: null
    },

    countNickNameToolTipCookieName: null,

    init: function () {
        briefProfile.cachedDomElements();

        briefProfile.checkAndSetShowEditProfilePictureTooltip();
        briefProfile.checkAndShowNickNameTooltip();
        briefProfile.setProfilePictureStorage();
        briefProfile.setActorNameStorage();
        briefProfile.registerEvents();

        $(".open-link").unbind().bind('click', event => { alpha.viewDataURL(event); });
    },

    checkAndSetShowEditProfilePictureTooltip: function () {
        briefProfile.isShowEditProfilePictureTooltip = briefProfile.domElements.profileContent.data('is-show-profile-picture-tooltip')
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

    setProfilePictureStorage: function () {
        localStorage.setItem('actorProfilePicture', JSON.stringify({
            ImageId: briefProfile.domElements.profilePicture.attr('profile-image-id'),
            Image: briefProfile.domElements.profilePicture.attr('src'),
            IsDefaultAvatar: briefProfile.domElements.profilePicture.attr('default-avatar')
        }));
    },

    setActorNameStorage: function () {
        const actorNameStorage = localStorage.getItem('actorName');
        const actorName = $('#Username').val();
        const IsOldActor = actorName === actorNameStorage;
        localStorage.setItem('IsOldActor', JSON.stringify(IsOldActor));
        if (!IsOldActor) {
            localStorage.setItem('actorName', actorName);
        }
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

        if (briefProfile.domElements.adminAndSubAcc.length > 0) {
            briefProfile.domElements.adminAndSubAcc.tooltip();
        }

        $(".nickname-box").tooltip();
    },

    cachedDomElements: function () {
        briefProfile.domElements.toolTipBox = $('#tooltip-box');
        briefProfile.domElements.profileTooltip = $('#profileTooltip');
        briefProfile.domElements.changePictureLink = $('#changePictureLink');
        briefProfile.domElements.profileContent = $('#profile-content');
        briefProfile.domElements.profilePicture = $("#logoProfileImg");
        briefProfile.domElements.adminAndSubAcc = $("#adminAndSubAcc");
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
        if (indexManager.isBlockPage) {
            return;
        }

        if (briefProfile.domElements.profileTooltip.length > 0
            && briefProfile.isShowEditProfilePictureTooltip) {
            briefProfile.domElements.profileTooltip.show();

            setTimeout(function () {
                briefProfile.hideProfileTooltip();
            }, 20000);
        }
    },

    showTooltipIntroNickname: function () {
        if (indexManager.isBlockPage) {
            return;
        }

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