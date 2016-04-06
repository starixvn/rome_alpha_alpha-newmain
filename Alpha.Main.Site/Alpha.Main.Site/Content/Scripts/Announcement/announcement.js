'use strict';

var announcementManager = {
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

    init: function () {
        announcementManager.loadAnnouncement();
    },
    cacheElements: function () {
        announcementManager.domElements.announcementId = $('#announcement-id');
        announcementManager.domElements.popUpId = $("#popup-id")[0];
        announcementManager.domElements.popUpPanel = $("#popup_panel")[0];
        announcementManager.domElements.popUpContainer = $('#popup-container')[0];
        announcementManager.domElements.popUpContainerText = $('#popup-container-text')[0];
        announcementManager.domElements.hideAdvertisement = $("#popup-container #chkHide");
        announcementManager.domElements.announcementCookieId = $("#announcement-cookies-id")[0];
    },
    loadAnnouncement: function () {
        var url = alpha.urls.mainRootPath + 'Announcement/HotAnnouncement';
        $.ajax({
            type: "POST",
            url: url,
            data: {
                messageIds: announcementManager.showedMessageIds.join(',')
            },
            responseType: "json",
            success: function (data) {
                announcementManager.showAnnouncement(data);
                announcementManager.timeoutAnnouncement = setTimeout(announcementManager.removeAnnouncement, 120000);
            }
        });
    },

    removeAnnouncement: function (timeout) {
        var announcementId = announcementManager.domElements.announcementId;
        if (announcementId != null) {
            announcementId.remove();
            clearTimeout(announcementManager.timeoutAnnouncement);
            setTimeout(function () {
                announcementManager.loadAnnouncementNext(timeout);
            }, 30000);
        }
    },

    loadAnnouncementNext: function () {
        var url = alpha.urls.mainRootPath + 'Announcement/HotAnnouncement';
        $.ajax({
            type: "POST",
            url: url,
            data: {
                messageIds: announcementManager.showedMessageIds.join(','),
                announmentNext: 1
            },
            responseType: "json",
            success: function (data) {
                announcementManager.showAnnouncement(data);
                announcementManager.timeoutAnnouncement = setTimeout(announcementManager.removeAnnouncement, 120000);
            }
        });
    },

    showAnnouncement: function (result) {
        var text = result;
        if (text.length < 10) {
            return;
        }

        announcementManager.removeAnnouncement();

        var div = document.createElement('div');
        div.innerHTML = text;
        div.style.display = 'none';
        div.setAttribute('id', 'announcement-id');
        document.body.appendChild(div);
        announcementManager.cacheElements();
        announcementManager.announcementCookieName = announcementManager.getAnnoucementCookieName();

        var popUp = announcementManager.domElements.popUpId;
        var id = popUp ? popUp.value : 0;
        if (!announcementManager.checkShowedMessage(id)) {
            announcementManager.showedMessageIds.push(id);
        }

        var count = announcementManager.getAnnoucementShowCount();
        if (count >= 6) {
            setTimeout(announcementManager.removeAnnouncement, 1000);
            return;
        }

        announcementManager.increaseAnnoucementShowTimes();
        var popUpPanel = announcementManager.domElements.popUpPanel;
        if (id < 0 && popUpPanel) {
            popUpPanel.style.display = 'none';
        }

        var popUpContainer =
            announcementManager.domElements.popUpContainerText ||
            announcementManager.domElements.popUpContainer;
        div.style.top = popUpContainer.style.top;
        div.style.left = popUpContainer.style.left;

        announcementManager.bindEvents();
    },

    bindEvents: function () {
        $('#popup-container-text .advertisement-close').click(function () {
            announcementManager.removeAnnouncement();
        });

        $('#popup-container .advertisement-close').click(function () {
            parent.announcementManager.removeAnnouncement();
        });

        announcementManager.domElements.hideAdvertisement.click(function () {
            announcementManager.doNotShowAnnoucement($(this)[0].checked);
        });

        setTimeout(function () {
            announcementManager.domElements.announcementId[0].style.display = 'block';
        }, 2000);
    },
    checkShowedMessage: function (id) {
        var count = announcementManager.showedMessageIds.length;
        for (var i = 0; i < count; i++) {
            if (id == announcementManager.showedMessageIds[i]) {
                return true;
            }
        }

        return false;
    },

    getAnnoucementShowCount: function () {
        var cookieName = announcementManager.announcementCookieName;
        if (cookieName == null) {
            return 0;
        }

        var value = getCookie(cookieName);
        var count = parseInt(value, 10);
        if (isNaN(count)) {
            count = 0;
        }

        return count;
    },

    getAnnoucementCookieName: function () {
        try {
            var popUpId = announcementManager.domElements.popUpId;
            if (!popUpId) {
                return null;
            }

            if (parseInt(popUpId.value) < 0) {
                return null;
            }

            return "-" + popUpId.value + announcementManager.domElements.announcementCookieId.value;
        }
        catch (e) {
            return null;
        }
    },
    increaseAnnoucementShowTimes: function () {
        var count = announcementManager.getAnnoucementShowCount();
        var popUpId = announcementManager.domElements.popUpId;
        var cookieDays = announcementManager.securityCodeId.indexOf(popUpId.value) == -1 ? 30 : 1;
        setCookie(announcementManager.announcementCookieName, ++count, cookieDays);
    },
    doNotShowAnnoucement: function (hide) {
        setCookie(announcementManager.announcementCookieName, hide ? 5 : 1, 30);
    }
};

$(function () {
    announcementManager.init();
});