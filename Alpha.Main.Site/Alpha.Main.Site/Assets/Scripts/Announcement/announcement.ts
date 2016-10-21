'use strict';

var announcement = {
    showedMessageIds: [],
    securityCodeId: '20130426,20130526',
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
        prevButton: null,
        nextButton: null
    },

    constants: {
        maximumTimeToShowAnnouncementPopUp: 5,
        normalExpiredDays: 30,
        securityCodeAnnouncementExpiredDay: 1,
        timeToShowAnnouncement: 120000,
        announcementsTransitionTime: 30000
    },

    urls: {
        hotAnnouncement: alpha.urls.mainRootPath + 'Announcement/HotAnnouncement'
    },

    init: function () {
        if (!alpha.isScreenLower768) {
            announcement.loadAnnouncement();
        }
        else if (alpha.isScreenLower768) {
            announcement.loadAnnouncementPeriodically(5000);
        }
    },

    cacheElements: function () {
        announcement.domElements.announcementId = $('#announcement-id');
        announcement.domElements.popUpId = $("#popup-id");
        announcement.domElements.popUpPanel = $("#popup_panel");
        announcement.domElements.popUpContainer = $('#popup-container');
        announcement.domElements.popUpContainerText = $('#popup-container-text');
        announcement.domElements.hideAdvertisement = $("#popup-container #chkHide");
        announcement.domElements.announcementCookieId = $("#announcement-cookies-id");
        announcement.domElements.prevButton = $("#prevButton");
        announcement.domElements.nextButton = $("#nextButton");
    },

    loadAnnouncement: function () {
        $.ajax({
            type: "POST",
            url: announcement.urls.hotAnnouncement,
            data: {
                messageIds: announcement.showedMessageIds.join(',')
            },
            success: function (data) {
                announcement.showAnnouncement(data);

                if (!alpha.isScreenLower768) {
                    announcement
                        .timeoutAnnouncement = setTimeout(announcement.removeAnnouncement,
                            announcement.constants.timeToShowAnnouncement);
                }

                announcement.checkNextAnnouncement((result) => {
                    if (result.length < 10) {
                        announcement.showRedCircleInIconAnnouncementMobile(false);
                    }
                });
            }
        });
    },

    removeAnnouncement: function () {
        var announcementId = announcement.domElements.announcementId;
        if (announcementId != null) {
            announcementId.remove();
            clearTimeout(announcement.timeoutAnnouncement);

            if (!alpha.isScreenLower768) {
                setTimeout(function () {
                    announcement.loadAnnouncementNext();
                },
                    announcement.constants.announcementsTransitionTime);
            }
        }
    },

    loadAnnouncementNext: function () {
        $.ajax({
            type: "POST",
            url: announcement.urls.hotAnnouncement,
            data: {
                messageIds: announcement.showedMessageIds.join(','),
                announmentNext: 1
            },
            responseType: "json",
            success: function (data) {
                announcement.showAnnouncement(data);
                announcement
                    .timeoutAnnouncement = setTimeout(announcement.removeAnnouncement,
                        announcement.constants.timeToShowAnnouncement);
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
        div.setAttribute('id', 'announcement-id');
        document.body.appendChild(div);

        announcement.cacheElements();
        announcement.announcementCookieName = announcement.getAnnouncementCookieName();

        var popUp = announcement.domElements.popUpId;
        var id = popUp.length ? popUp.val() : 0;

        if (!announcement.checkShowedMessage(id)) {
            announcement.showedMessageIds.push(id);
        }

        if (!alpha.isScreenLower768) {
            var count = announcement.getAnnouncementShowCount();
            if (count > announcement.constants.maximumTimeToShowAnnouncementPopUp) {
                setTimeout(announcement.removeAnnouncement, 1000);
                return;
            }

            announcement.increaseAnnouncementShowTimes();
        }

        var popUpPanel = announcement.domElements.popUpPanel;
        if (id < 0 && popUpPanel) {
            popUpPanel.css('display', 'none');
        }

        announcement.bindEvents();
    },

    bindEvents: function () {
        announcement.domElements.popUpContainerText.find('.icon-close')
            .click(function () {
                announcement.removeAnnouncement();
            });

        announcement.domElements.popUpContainer.find('.icon-close')
            .click(function () {
                //// Re-test again
                parent['announcement'].removeAnnouncement();
            });

        announcement.domElements.hideAdvertisement.click(function () {
            announcement.doNotShowAnnouncement($(this).is(':checked'));
        });

        announcement.domElements.prevButton.click(function () {
            announcement.loadAnnouncementPreviousForMobile();
        });

        announcement.domElements.nextButton.click(function () {
            announcement.loadAnnouncementNextForMobile();
        });

        if (!alpha.isScreenLower768) {
            setTimeout(function () {
                announcement.registerAnnouncementModal();
            },
                2000);
        } else {
            announcement.registerAnnouncementModal();
        }
    },

    registerAnnouncementModal: function () {
        var popUpContainer = announcement.domElements.popUpContainerText.length
            ? announcement.domElements.popUpContainerText
            : announcement.domElements.popUpContainer;
        popUpContainer
            .modal({ show: true, backdrop: false })
            .tinyDraggable({ exclude: 'button.icon.icon-close.close, .modal-body, p' });
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

    getAnnouncementShowCount: function () {
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

    getAnnouncementCookieName: function () {
        try {
            var popUpId = announcement.domElements.popUpId;
            if (popUpId.length == 0) {
                return null;
            }

            if (parseInt(popUpId.val()) < 0) {
                return null;
            }

            return "-" + popUpId.val() + announcement.domElements.announcementCookieId.val();
        } catch (e) {
            return null;
        }
    },

    increaseAnnouncementShowTimes: function () {
        var count = announcement.getAnnouncementShowCount();
        var popUpId = announcement.domElements.popUpId;
        var cookieDays = announcement.securityCodeId.indexOf(popUpId.val()) == -1
            ? announcement.constants.normalExpiredDays
            : announcement.constants.securityCodeAnnouncementExpiredDay;
        $.cookie(announcement.announcementCookieName, ++count, { expires: cookieDays, path: '/' });
    },

    doNotShowAnnouncement: function (hide) {
        $.cookie(
            announcement.announcementCookieName,
            hide ? announcement.constants.maximumTimeToShowAnnouncementPopUp : 1,
            { expires: announcement.constants.normalExpiredDays, path: '/' });
    },

    loadAnnouncementPreviousForMobile: function () {
        if (announcement.showedMessageIds.length >= 2) {
            announcement.showedMessageIds = announcement.showedMessageIds.slice(0, -2);
            announcement.loadAnnouncementNextForMobile();
        }
    },

    loadAnnouncementNextForMobile: function () {
        $.ajax({
            type: "POST",
            url: announcement.urls.hotAnnouncement,
            data: {
                messageIds: announcement.showedMessageIds.join(','),
                announmentNext: 1
            },
            responseType: "json",
            success: (result) => {
                announcement.appendNewAnnouncementToPopup(result);

                announcement.checkNextAnnouncement((result) => {
                    if (result.length < 10) {
                        announcement.showRedCircleInIconAnnouncementMobile(false);
                    }
                });
            }
        });
    },

    loadAnnouncementPeriodically: (interval: number) => {
        const loadAnnouncement = () => {
            $.ajax({
                type: "POST",
                url: announcement.urls.hotAnnouncement,
                data: {
                    messageIds: announcement.showedMessageIds.join(','),
                    announmentNext: 1
                },
                responseType: "json",
                success: (result: string) => {
                    if (result.length > 10) {
                        announcement.showRedCircleInIconAnnouncementMobile();
                    } else {
                        announcement.showRedCircleInIconAnnouncementMobile(false);
                    }
                }
            });
        };

        setInterval(loadAnnouncement, interval);
    },

    checkNextAnnouncement: (callBack) => {
        $.ajax({
            type: "POST",
            url: announcement.urls.hotAnnouncement,
            data: {
                messageIds: announcement.showedMessageIds.join(','),
                announmentNext: 1
            },
            responseType: "json",
            success: callBack
        });
    },

    appendNewAnnouncementToPopup: function (result) {
        var newAnnouncement = $("<div>" + result + "</div>");
        var newId = newAnnouncement.find("#popup-id").val();

        var text = result;
        if (text.length < 10) {
            return;
        }

        $('#announcementModalBody').empty().append(newAnnouncement.find("#announcementModalBody").html());
        $("#popup-id").val(newId);

        var popUp = announcement.domElements.popUpId;
        var id = popUp.length ? popUp.val() : 0;
        var popUpPanel = announcement.domElements.popUpPanel;

        if (!announcement.checkShowedMessage(id)) {
            announcement.showedMessageIds.push(id);
        }

        if (id < 0 && popUpPanel) {
            popUpPanel.css('display', 'none');
        }
    },

    showRedCircleInIconAnnouncementMobile: (isShow: boolean = true) => {
        if (isShow) {
            $('.icon-bell').find('.circle-red').show();
        } else {
            $('.icon-bell').find('.circle-red').hide();
        }
    }
};

$(document).ready(() => {
    announcement.init();
});