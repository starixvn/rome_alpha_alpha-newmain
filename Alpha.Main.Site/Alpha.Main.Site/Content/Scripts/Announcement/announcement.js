'use strict';

var announcement = {
    showedMessageIds: [],
    securityCodeId: ['20130426', '20130526'],
    timeoutAnnouncement: 0,
    announcementCookieName: null,

    domElements: {
        announcementId: null,
        popUpId: null,
        popUpPanel: null,
        popUpContainer: null,
        popUpContainerText: null,
        hideAdvertisement: null,
        announcementCookieId: null,
    },
    constants: {
        maximumTimeToShowAnnoucementPopUp: 5,
        normalExpiredDays: 30,
        securityCodeAnnoucementExpiredDay: 1,
        timeToShowAnnouncement: 120000,
        annoucementsTransitionTime: 30000
    },
    urls: {
        hotAnnoucement: alpha.urls.mainRootPath + 'Announcement/HotAnnouncement'
    },

    init: function () {
        announcement.loadAnnouncement();
    },
    cacheElements: function () {
        announcement.domElements.announcementId = $('#announcement-id');
        announcement.domElements.popUpId = $("#popup-id");
        announcement.domElements.popUpPanel = $("#popup_panel");
        announcement.domElements.popUpContainer = $('#popup-container');
        announcement.domElements.popUpContainerText = $('#popup-container-text');
        announcement.domElements.hideAdvertisement = $("#popup-container #chkHide");
        announcement.domElements.announcementCookieId = $("#announcement-cookies-id");
    },
    loadAnnouncement: function () {
        $.ajax({
            type: "POST",
            url: announcement.urls.hotAnnoucement,
            data: {
                messageIds: announcement.showedMessageIds.join(',')
            },
            responseType: "json",
            success: function (data) {
                announcement.showAnnouncement(data);
                announcement.timeoutAnnouncement = setTimeout(announcement.removeAnnouncement, announcement.constants.timeToShowAnnouncement);
            }
        });
    },

    removeAnnouncement: function (timeout) {
        var announcementId = announcement.domElements.announcementId;
        if (announcementId != null) {
            announcementId.remove();
            clearTimeout(announcement.timeoutAnnouncement);
            setTimeout(function () {
                announcement.loadAnnouncementNext(timeout);
            }, announcement.constants.annoucementsTransitionTime);
        }
    },

    loadAnnouncementNext: function () {
        $.ajax({
            type: "POST",
            url: announcement.urls.hotAnnoucement,
            data: {
                messageIds: announcement.showedMessageIds.join(','),
                announmentNext: 1
            },
            responseType: "json",
            success: function (data) {
                announcement.showAnnouncement(data);
                announcement.timeoutAnnouncement = setTimeout(announcement.removeAnnouncement, announcement.constants.timeToShowAnnouncement);
            }
        });
    },

    showAnnouncement: function (result) {
        var text = result;
        if (text.length < 10) {
            return;
        }

        announcement.removeAnnouncement();

        var div = document.createElement('div');
        div.innerHTML = text;
        div.style.display = 'none';
        div.setAttribute('id', 'announcement-id');
        document.body.appendChild(div);

        announcement.cacheElements();
        announcement.announcementCookieName = announcement.getAnnoucementCookieName();

        var popUp = announcement.domElements.popUpId;
        var id = popUp.length ? popUp.val() : 0;
        if (!announcement.checkShowedMessage(id)) {
            announcement.showedMessageIds.push(id);
        }

        var count = announcement.getAnnoucementShowCount();
        if (count > announcement.constants.maximumTimeToShowAnnoucementPopUp) {
            setTimeout(announcement.removeAnnouncement, 1000);
            return;
        }

        announcement.increaseAnnoucementShowTimes();
        var popUpPanel = announcement.domElements.popUpPanel;
        if (id < 0 && popUpPanel) {
            popUpPanel.css('display', 'none');
        }

        var popUpContainer = announcement.domElements.popUpContainerText.length ?
            announcement.domElements.popUpContainerText :
            announcement.domElements.popUpContainer;
        div.style.top = popUpContainer.css('top');
        div.style.left = popUpContainer.css('left');

        announcement.bindEvents();
    },

    bindEvents: function () {
        announcement.domElements.popUpContainerText.find('.icon-close').click(function () {
            announcement.removeAnnouncement();
        });

        announcement.domElements.popUpContainer.find('.icon-close').click(function () {
            parent.announcement.removeAnnouncement();
        });

        announcement.domElements.hideAdvertisement.click(function () {
            announcement.doNotShowAnnoucement($(this).is(':checked'));
        });

        setTimeout(function () {
            announcement.domElements.announcementId.css('display', 'block');
            var popUpContainer = announcement.domElements.popUpContainerText.length ?
                announcement.domElements.popUpContainerText :
                announcement.domElements.popUpContainer;
            popUpContainer.modal({ show: true, backdrop: false });
        }, 2000);
    },
    checkShowedMessage: function (id) {
        var count = announcement.showedMessageIds.length;
        for (var i = 0; i < count; i++) {
            if (id == announcement.showedMessageIds[i]) {
                return true;
            }
        }

        return false;
    },

    getAnnoucementShowCount: function () {
        var cookieName = announcement.announcementCookieName;
        if (cookieName == null) {
            return 0;
        }

        var value = $.cookie(cookieName);
        var count = parseInt(value, 10);
        if (isNaN(count)) {
            count = 0;
        }

        return count;
    },

    getAnnoucementCookieName: function () {
        try {
            var popUpId = announcement.domElements.popUpId;
            if (popUpId.length == 0) {
                return null;
            }

            if (parseInt(popUpId.val()) < 0) {
                return null;
            }

            return "-" + popUpId.val() + announcement.domElements.announcementCookieId.val();
        }
        catch (e) {
            return null;
        }
    },
    increaseAnnoucementShowTimes: function () {
        var count = announcement.getAnnoucementShowCount();
        var popUpId = announcement.domElements.popUpId;
        var cookieDays = announcement.securityCodeId.indexOf(popUpId.val()) == -1 ?
            announcement.constants.normalExpiredDays :
            announcement.constants.securityCodeAnnoucementExpiredDay;
        $.cookie(announcement.announcementCookieName, ++count, { expires: cookieDays, path: '/' });
    },
    doNotShowAnnoucement: function (hide) {
        $.cookie(
            announcement.announcementCookieName,
            hide ? announcement.constants.maximumTimeToShowAnnoucementPopUp : 1,
            { expires: announcement.constants.normalExpiredDays, path: '/' });
    }
};

$(function () {
    announcement.init();
});