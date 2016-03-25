var indexPageManager;

$(document).ready(function () {
    indexPageManager.init();
});

var IndexPageManager = function () {
    var self = this;
    self.data = {
        iframes: {
            main: null
        }
    };

    var currentTab = null;
    self.init = function () {
        self.initElements();
        self.bindEvents();
        self.setSharedVariables();
    };
    self.initElements = function () {
        self.selectTopMenu();
    };
    self.bindEvents = function () {
        customerProfile.bindEvents();
        jQuery(".iframe-content").load(function () {
            self.setURL();
        });
        setTimeout(self.reCheckin, 30000);
    };

    self.signOut = function () {
        setCookie(_resources.cookieCurrentURLKey, '', -1);
        var index = window;
        while (index != index.parent)
        {
            index = index.parent;
        }

        index.location = "/" + top.baseOldMainApp + '_Authorization/Handlers/SignOut.ashx';
        if (index.location.href.indexOf('https://imb.') == 0 ||
            index.location.href.indexOf('https://int.') == 0) {
            index.close();
        }
    };
    self.changeMenu = function (url) {
        top.main.location = url;
        self.selectTopMenu(url);
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
    self.setSharedVariables = function () {
        top.ph = window.passwordHash;
        top.isNewSite = true;
        top.errCodes = [];
        top.baseOldMainApp = baseOldMainApp;
    };
    self.reCheckin = function () {
        // Post with return result in JSON format
        setCookie("ssl", location.href.indexOf('https://') == 0, 30);
        var url = "/" + top.baseOldMainApp + '_Authorization/Handlers/ReCheckin.ashx';
        $.ajax({
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
                        indexPageManager.signOut();
                    } else {
                        alert('Could not contact the server. Please check your network connection.');
                    }
                }
            },
            error: function (jqXhr, event, ajaxSettings) {
            }
        });
    };
    self.setURL = function () {
        var url = top.main.location.href;
        var currentURL = url.replace("http:", '')
            .replace("https:", '')
            .replace(_resources.baseUrl, '');
        if (currentURL == null) {
            return;
        }

        setCookie(_resources.cookieCurrentURLKey, currentURL, 1);
    };
};

indexPageManager = new IndexPageManager();