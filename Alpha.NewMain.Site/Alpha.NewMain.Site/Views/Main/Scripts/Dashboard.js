var dashboardManager;

jQuery(document).ready(function () {
    dashboardManager.init();
});

var DashboardManager = function () {
    var self = this;
    var isShowTopRightMenu = false;
    var isShowLanguageList = false;
    var currentTab = null;
    var currentMenuItem = null;
    var msgIndex = 0;

    var popupContents1Height = 0;
    var popupContents1MarginTop = 0;
    var popupContents1 = null;
    var popupContents2 = null;
    var publicMessageId = 0; // public message
    var privateMessageId = 0; // private message
    var scrollMessageTimeout = 0;

    self.data = {
        iframes: {
            main: null
        },
        scroller_left: undefined,
        scroller_width: 0,
        scroller: null,
        topnews: null,
        scrollerPaused: false
    }

    self.init = function () {
        dashboardManager.selectTopMenu();
        dashboardManager.showHotMsg(_resources.msgList);

        self.initElements();
        self.bindEvents();
    };
    self.initElements = function () {
        customerProfile.loadCustomerProfile();
    };

    self.bindEvents = function () {
        jQuery(".top-message").click(function () {
            self.showPopupTop(false);
        });

        var topRightMenu = jQuery('.top-right-menu');
        topRightMenu.click(function () {
            if (isShowLanguageList) {
                self.activeLanguageList();
            }

            self.activeSettingList();
        });

        var languageSelector = jQuery('.language-selector');
        languageSelector.click(function () {
            if (isShowTopRightMenu) {
                self.activeSettingList();
            }

            self.activeLanguageList();
        });

        jQuery(".icon-logout").click(function () {
            self.signOut();
        });
        jQuery('.icon-collapse').click(function () {
            self.collapseLeftPanel(jQuery(this));
        });
        jQuery(".icon-home").click(function () {
            self.changeMenu(self.getMainSitePath() + '_Reports/PortalPage/PortalPage.aspx');
        });
        jQuery(".live-message").click(function () {
            self.changeMenu(self.getMainSitePath() + 'Info/NewMessageForm.aspx?pageQuery=', this);
        });
        jQuery(".special-message").click(function () {
            self.changeMenu(self.getMainSitePath() + 'Info/NewMessageForm.aspx?pageQuery=', this);
        });

        jQuery(".private-message").click(function () {
            self.viewPrivateMessage(self.getMainSitePath() + 'Info/NewMessageForm.aspx?pageQuery=private');
        });
        jQuery(".iframe-content").load(function () {
            self.setURL();
        });
        jQuery(".icon-swap-horizontal").click(function () {
            top.location = rootUrl + self.getMainSitePath() + "Index.aspx";
        });

        setTimeout(self.getMessages, 500);
        setTimeout(self.reCheckin, 30000);
    };
    self.getMainSitePath = function () {
        return '../' + mainSite + '/';
    };
    self.activeLanguageList = function () {
        isShowLanguageList = !isShowLanguageList;
        jQuery('.language-selector').parent().toggleClass("active");
    };
    self.activeSettingList = function () {
        isShowTopRightMenu = !isShowTopRightMenu;
        jQuery('.top-right-menu').parent().toggleClass("active");
    };
    self.changeMenu = function (url) {
        self.clearCurrentLeftMenu();
        top.main.location = rootUrl + url;
        self.selectTopMenu(url);
    };
    self.clearCurrentLeftMenu = function () {
        //if (top.menu && top.menu.ClearCurrentLeftMenu) {
        //    top.menu.ClearCurrentLeftMenu();
        //}

        //if (currentMenuItem != null) {
        //    currentMenuItem.className = 'Bleft_Sub';
        //}
    };
    self.selectTopMenu = function (url) {
        var mainFrame = top.main;
        var mainUrl = '';
        if (url) {
            mainUrl = url;
        } else if (mainFrame) {
            mainUrl = mainFrame.location.href;
        }

        var start = mainUrl.lastIndexOf('/');
        var end = mainUrl.lastIndexOf('?');
        if (end == -1) {
            end = mainUrl.length;
        }

        var name = mainUrl.substring(start + 1, end);

        if (currentTab) {
            currentTab.className = "";
        }

        switch (name) {
            case 'PortalPage.aspx':
                currentTab = $("#balance");
                break;
            case 'MemberList.aspx':
            case 'AgentList.aspx':
            case 'MasterList.aspx':
                currentTab = $("#transfer");
                break;
            case 'ChangePassword.aspx':
                currentTab = $("#changepass");
                break;
            case 'LogCustomerSetting.aspx':
                currentTab = $("#viewlog");
                break;
            default:
                currentTab = null;
                break;
        }

        if (currentTab) {
            currentTab.addClass("active");
        }
        else {
            self.clearActiveTab();
        }
    };
    self.clearActiveTab = function () {
        if (currentTab) {
            currentTab.removeClass("active");
        }
    };
    self.reCheckin = function () {
        // Post with return result in JSON format
        setCookie("ssl", location.href.indexOf('https://') == 0, 30);
        var url = rootUrl + self.getMainSitePath() + '_Authorization/Handlers/ReCheckin.ashx';
        jQuery.ajax({
            type: "POST",
            url: url,
            data: { onid: _resources.onUserId },
            responseType: "json",
            success: function (result) {
                var result = convertReponseToJson(result);
                var errCode = result.errCode;
                if (errCode == 0) {
                    setTimeout(self.reCheckin, 30000);
                } else {
                    var errMsg = result.errMsg;
                    if (errMsg) {
                        alert(errMsg);
                        dashboardManager.SignOut();
                    } else {
                        alert('Could not contact the server. Please check your network connection.');
                    }
                }
            },
            error: function (jqXhr, event, ajaxSettings) {
            }
        });
    };
    self.signOut = function () {
        var index = window;
        while (index != index.parent) // Jump out
        {
            index = index.parent;
        }

        index.location = rootUrl + self.getMainSitePath() + '_Authorization/Handlers/SignOut.ashx';
        if (index.location.href.indexOf('https://imb.') == 0 ||
            index.location.href.indexOf('https://int.') == 0) {
            index.close();
        }
    };
    self.showHotMsg = function (msgs) {
        var topPopup = $("#popup-top");
        if (msgs.length == 0) {
            topPopup.addClass('display-none');
            return;
        }

        popupContents1 = $('#popup-contents')[0];
        popupContents2 = $('#popup-contents-2')[0];
        popupContents1.innerHTML = msgs[msgIndex];
        if (msgs.length > 1) {
            popupContents2.innerHTML = msgs[msgIndex + 1];
        }

        self.showPopupTop(true);
        topPopup.css("height", popupContents1.clientHeight + 'px');
        if (msgs.length > 1) {
            setInterval(self.swapMessage_Start, 18000);
        }
    };
    self.swapMessage_Start = function () {
        if (msgIndex >= _resources.msgList.length - 1) {
            msgIndex = -1;
        }

        popupContents1Height = popupContents1.clientHeight + 20;
        popupContents2.innerHTML = _resources.msgList[++msgIndex];
        setTimeout(self.swapMessage, 50);
    };

    self.swapMessage = function () {
        popupContents1MarginTop -= 5;
        if (Math.abs(popupContents1MarginTop) >= popupContents1Height) {
            popupContents1MarginTop = 0;
            var parentNode = popupContents1.parentNode;
            parentNode.removeChild(popupContents1);
            parentNode.appendChild(popupContents1);
            var node2 = popupContents2;
            popupContents2 = popupContents1;
            popupContents1 = node2;

            popupContents1.style.marginTop = popupContents1MarginTop + "px";
            popupContents2.style.marginTop = popupContents1MarginTop + "px";

            $("#popup-top").css("height", popupContents1.clientHeight + 'px');
            return;
        }

        popupContents1.style.marginTop = popupContents1MarginTop + "px";
        setTimeout(self.swapMessage, 50);
    };
    self.showPopupTop = function (show) {
        var topPopup = $("#popup-top");
        if (show) {
            topPopup.removeClass('display-none');
        } else {
            topPopup.addClass('display-none');
        }
    };
    self.getMessages = function () {
        var url = rootUrl + self.getMainSitePath() + '_UserMessage/Handlers/GetMessages.ashx';
        jQuery.ajax({
            type: "POST",
            url: url,
            data: {},
            responseType: "json",
            error: function (jqXhr, event, ajaxSettings) {
            },
            success: function (results) {
                results = convertReponseToJson(results);
                try {
                    var publicMessage = (results.public_message != null && results.public_message != 'undefined') ? results.public_message : 0;
                    var privateMessage = (results.private_message != null && results.private_message != 'undefined') ? results.private_message : 0;
                    var unreadPriMsgCount = (results.unread_private_messages != null && results.unread_private_messages != 'undefined') ? results.unread_private_messages : 0;

                    jQuery('.private-message .value').html(unreadPriMsgCount);

                    var msgId = 0,
                        msgContent = 1,
                        msgType = 2;

                    // Process Public Message: show new public message and its link to message form
                    var url = self.getMainSitePath() + "Info/NewMessageForm.aspx?pageQuery=";
                    var balloonPopupContent = "";
                    url += publicMessage[msgType];

                    if ((publicMessage[msgId] != null) && (publicMessageId != publicMessage[msgId])) {
                        $('#live-message').html(publicMessage[msgContent]);
                        $('#text-news-out').click(function () {
                            self.changeMenu(url);
                        });

                        // Work with FF, IE6-7
                        if (publicMessageId > 0) {
                            balloonPopupContent = publicMessage[msgContent];
                        }

                        publicMessageId = publicMessage[msgId];
                    }

                    // Process Private Message: show new private message
                    if ((privateMessage[msgId] != null) && (privateMessageId != privateMessage[msgId])) {
                        var liveMessageContent = $('#live-message').html();
                        $('#live-message').html(liveMessageContent + ". <font color=\"green\">Private message:" + privateMessage[msgContent] + "</font>");
                        if (privateMessageId > 0) {
                            balloonPopupContent = privateMessage[msgContent];
                        }

                        privateMessageId = privateMessage[msgId];
                    }

                    // Process Balloon Pop-up: show new public and private messages if existed
                    if (balloonPopupContent != "") {
                        $("#popup-contents").html(balloonPopupContent);
                        self.showPopupTop(0);
                        setTimeout(function () {
                            self.showPopupTop(1);
                        }, (30 * 100));
                    }

                    self.applyScroller();
                } catch (e) { }

                setTimeout(self.getMessages, (60 * 1000));
            }
        });
    };
    self.collapseLeftPanel = function (collapseButton) {
        var headerCla = jQuery('.header');
        var left = jQuery('.content');

        var isCollapse = left.hasClass('collapsed');
        if (isCollapse) {
            jQuery("#logoProfileImg").removeClass("collapse-image");
            left.removeClass('collapsed');
            headerCla.removeClass('collapsed');
        } else {
            jQuery("#logoProfileImg").addClass("collapse-image");
            left.addClass('collapsed');
            headerCla.addClass('collapsed');
            if (jQuery('.accountinfo-container').css('display') == 'block') {
                jQuery('.icon-accinfo').css('display', 'block');
                jQuery('.accountinfo-container .row').each(function () {
                    collapseButton.css('display', 'none');
                });
            }
        }

        jQuery('.accountinfo-container .row').each(function () {
            collapseButton.css('display', 'block');
        });
        jQuery('.icon-accinfo').css('display', 'none');
    };
    self.viewPrivateMessage = function (url) {
        self.changeMenu(url);
        var countPrivateMsg = jQuery('.private-message .value').html();
        if (parseInt(countPrivateMsg) > 0) {
            self.updateCountPrivateMsg();
        }
    };
    self.updateCountPrivateMsg = function () {
        var url = rootUrl + self.getMainSitePath() + '_UserMessage/Handlers/UpdatePrivateMessageCount.ashx';
        jQuery.ajax({
            type: "POST",
            url: url,
            data: {},
            responseType: "json",
            error: function (jqXhr, event, ajaxSettings) {
            },
            success: function (data) {
                data = convertReponseToJson(data);
                if (data.errCode == 0) {
                    jQuery('.private-message .value').html(0);
                }
            }
        });
    };
    self.applyScroller = function () {
        if (typeof self.data.scroller_left != 'undefined') {
            // Apply first time only
            return;
        }

        self.data.scroller_left = -100;
        self.data.scroller_width = 0;

        self.data.scroller = $('#scroller')[0];
        self.data.topnews = $('#live-message')[0];

        self.data.topnews.onmouseover = function () {
            clearTimeout(scrollMessageTimeout);
            scrollMessageTimeout = 0;
            self.data.scrollerPaused = true;
        };
        self.data.topnews.onmouseout = function () {
            if (self.data.scrollerPaused) {
                self.data.scrollerPaused = false;
                scrollMessageTimeout = setTimeout(self.scrollMessage, 20);
            } else {
                self.data.scrollerPaused = false;
            }
        };

        var text_news = $('#text-news-out')[0];
        self.data.maskWidth = parseInt(text_news.offsetWidth);

        self.scrollMessage();
    };
    self.scrollMessage = function () {
        if (self.data.scrollerPaused) {
            return;
        }

        if (self.data.scroller_left + self.data.scroller_width < -5) {
            self.data.scroller_left = self.data.maskWidth + 5;
            self.data.scroller_width = parseInt(self.data.topnews.offsetWidth);
        }

        self.data.scroller.style.left = (self.data.scroller_left--) + 'px';
        scrollMessageTimeout = setTimeout(self.scrollMessage, 20);
    };
    self.setURL = function () {
        var url = top.main.location.href;
        var currentURL = url.replace(_resources.baseUrl, '');
        if (currentURL == null) {
            return;
        }

        setCookie(_resources.cookieCurrentURLKey, currentURL, 1);
    };
};


var dashboardManager = new DashboardManager();