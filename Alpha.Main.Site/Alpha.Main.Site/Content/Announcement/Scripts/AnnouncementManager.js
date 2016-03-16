var announcementManager;

var AnnouncementManager = function () {
    var self = this;
    var showedMessageIds = new Array();
    var timeoutAnnouncement = 0;
    var timeoutAnnouncementNext = 0;
    self.init = function () {
        self.loadAnnouncement();
    };
    self.loadAnnouncement = function () {
        var url = top.baseMainApp + 'Announcement/HotAnnouncement';
        $.ajax({
            type: "POST",
            url: url,
            data: {
                messageIds: showedMessageIds.join(',')
            },
            responseType: "json",
            error: function (jqXhr, event, ajaxSettings) {
            },
            success: function (data) {
                self.showAnnouncement(data);
                timeoutAnnouncement = setTimeout(self.removeAnnouncement, 120000);
            }
        });
    };
    self.removeAnnouncement = function (timeout) {
        var announcementId = $('#announcement-id');
        if (announcementId != null) {
            announcementId.remove();
            clearTimeout(timeoutAnnouncement);
            setTimeout(function () {
                self.loadAnnouncementNext(timeout);
            }, 30000);
        }
    };
    self.loadAnnouncementNext = function () {
        var url = top.baseMainApp + 'Announcement/HotAnnouncement';
        $.ajax({
            type: "POST",
            url: url,
            data: {
                messageIds: showedMessageIds.join(','),
                announmentNext: 1
            },
            responseType: "json",
            error: function (jqXhr, event, ajaxSettings) {
            },
            success: function (data) {
                self.showAnnouncement(data);
                timeoutAnnouncement = setTimeout(self.removeAnnouncement, 120000);
            }
        });
    };
    self.showAnnouncement = function (result) {
        var text = result;
        if (text.length < 10) {
            return;
        }

        self.removeAnnouncement();
        var div = document.createElement('div');
        div.innerHTML = text;
        div.style.display = 'none';
        div.setAttribute('id', 'announcement-id');
        document.body.appendChild(div);

        var popUp = $('#popup-id')[0];
        var id = popUp ? popUp.value : 0;
        if (!self.checkShowedMessage(id)) {
            showedMessageIds.push(id);
        }

        var count = self.getAnnoucementShowCount();
        if (count >= 6) {
            setTimeout(self.removeAnnouncement, 1000);
            return;
        }

        self.increaseAnnoucementShowTimes();
        var popUpPanel = $("#popup_panel")[0];
        if (id < 0 && popUpPanel) {
            popUpPanel.style.display = 'none';
        }

        var popUpContainer = $('#popup-container')[0];
        var popUpContainerText = $('#popup-container-text')[0];
        if (popUpContainerText == null) {
            div.style.top = popUpContainer.style.top;
            div.style.left = popUpContainer.style.left;
        }
        else {
            div.style.top = popUpContainerText.style.top;
            div.style.left = popUpContainerText.style.left;
        }

        $("#popup-container-text .advertisement-close").click(function () {
            announcementManager.removeAnnouncement();
        });

        $("#popup-container .advertisement-close").click(function () {
            parent.announcementManager.removeAnnouncement();
        });

        $("#popup-container #chkHide").click(function () {
            announcementManager.doNotShowAnnoucement($(this)[0].checked);
        });

        setTimeout(function () {
            $('#announcement-id')[0].style.display = 'block';
        }, 2000);
    };
    self.checkShowedMessage = function (id) {
        var count = showedMessageIds.length;
        for (var i = 0; i < count; i++) {
            if (id == showedMessageIds[i]) {
                return true;
            }
        }

        return false;
    };
    self.getAnnoucementShowCount = function () {
        var cookieName = self.getAnnoucementCookieName();
        if (cookieName == null) return 0;

        var value = getCookie(cookieName);
        var count = parseInt(value, 10);
        if (isNaN(count)) {
            count = 0;
        }

        return count;
    };
    self.getAnnoucementCookieName = function () {
        try {
            var popUpId = $("#popup-id")[0];
            if (!popUpId) {
                return null;
            }

            if (parseInt(popUpId.value) < 0) {
                return null;
            }

            return "-" + popUpId.value + $("#announcement-cookies-id")[0].value;
        }
        catch (e) {
            return null;
        }
    };
    self.increaseAnnoucementShowTimes = function () {
        var count = self.getAnnoucementShowCount();
        var popUpId = $("#popup-id");
        if (popUpId.value == '20130426' ||
            popUpId.value == '20130526') {
            setCookie(self.getAnnoucementCookieName(), ++count, 1);
        }
        else {
            setCookie(self.getAnnoucementCookieName(), ++count, 30);
        }
    };
    self.doNotShowAnnoucement = function (hide) {
        setCookie(self.getAnnoucementCookieName(), hide ? 5 : 1, 30);
    };
};

announcementManager = new AnnouncementManager();

$(document).ready(function () {
    announcementManager.init();
});