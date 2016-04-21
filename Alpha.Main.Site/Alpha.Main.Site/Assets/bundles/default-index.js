var alpha = alpha || {};

alpha.urls = {
    oldMainRootPath: document.getElementById('oldMainRootPath').value,
    mainRootPath: document.getElementById('mainRootPath').value,
    howToUseRootPath: document.getElementById('howToUseRootPath').value
};

alpha.user =
    {
        onUserId: document.getElementById('onUserId').value,
        passwordHash: document.getElementById('passwordHash').value,
        subPageAccess: document.getElementById('subPageAccess').value
    };

alpha.core = {
    convertReponseToJson: function (reponse) {
        return eval("(" + reponse + ")");
    },

    signOut: function () {
        var index = window;
        while (index != index.parent) {
            index = index.parent;
        }

        index.location = alpha.urls.oldMainRootPath + '_Authorization/Handlers/SignOut.ashx';

        if (index.location.href.indexOf('https://imb.') == 0 ||
            index.location.href.indexOf('https://int.') == 0) {
            index.close();
        }
    }
};

//// Set value for old main => every share variables should be listed here
top.ph = alpha.user.passwordHash;
top.isNewSite = true;
top.errCodes = [];
top.oldMainRootPath = alpha.urls.oldMainRootPath;
top.mainRootPath = alpha.urls.mainRootPath;

top.AGE = top.AGE || {};
top.AGE.PERMISSIONS = alpha.user.subPageAccess;

//// Functions for Site-old only, do not use these
function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1, c.length);
        }

        if (c.indexOf(nameEQ) == 0) {
            return c.substring(nameEQ.length, c.length);
        }
    }

    return null;
}

function setCookie(name, value, days, domain) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else {
        var expires = "";
    }

    document.cookie = name + "=" + value + expires + "; path=/;" + ((typeof domain != 'undefined') ? ("domain=" + domain) : String.Empty);
}

window.age = window.age || {};
window.age.OpenPopUp = function (url, title, width, height, position) {
    title = title || '';
    var popH = height || 360;
    var popW = width || 480;
    var left = position ? position.left : 200;
    var top = position ? position.top : 200;

    var popUp = $("#page-popup");
    popUp.css({
        width: popW,
        height: popH,
        left: left,
        top: top
    });

    popUp.removeClass("hidden");
    popUp.find(".popup-title").html(title);
    popUp.find(".ex-modal").css("height", popH + "px");
    popUp.find(".modal-content .modal-body").css('padding-top', '8px');
    popUp.find(".modal-content .modal-body").css("height", (popH - 40) + "px");

    var iframe = popUp.find("#popup-iframe");

    var padding = 15 * 2;
    iframe.css({
        width: popW - padding,
        height: popH - 82,
        left: left,
        top: top
    });
    iframe.attr("src", url);
    popUp.modal({ show: true, backdrop: false });
    popUp.find(".icon-close").unbind("click").click(function () {
        popUp.addClass("hidden");
    });

    iframe.load(function () {
        var iframe_doc = iframe.contents();

        if (title == '' && iframe_doc.title != 'Home') {
            popUp.find(".popup-title").html(title);
        }
    });

    popUp.tinyDraggable();
}

window.age.ClosePopUp = function () {
    var popUp = $("#page-popup");
    popUp.addClass("hidden");
};

window.AGEWnd = window.AGEWnd || {};
window.AGEWnd.open = window.AGEWnd.open || function (url, title, left, top, width, height) {
    title = title || '';
    var popH = height || 360;
    var popW = (width + 100) || 480;
    var left = left || 200;
    var top = top || 200;

    var popUp = $("#page-popup").clone();
    popUp.css({
        width: popW,
        height: popH,
        left: left,
        top: top
    });

    popUp.appendTo("body");
    popUp.removeClass("hidden");
    popUp.find(".popup-title").html(title);
    popUp.find(".ex-modal").css("height", popH + "px");
    popUp.find(".modal-content .modal-body").css("height", (popH - 40) + "px");

    var iframe = popUp.find("#popup-iframe");

    var padding = 15 * 2;
    iframe.css({
        width: popW - padding,
        height: popH - 40 - 20,
        left: left,
        top: top
    });

    iframe.attr("src", url);
    popUp.modal({ show: true, backdrop: false });
    popUp.find(".icon-close").unbind("click").click(function () {
        popUp.remove();
    });

    iframe.load(function () {
        var iframe_doc = iframe.contents();

        if (title == '' && iframe_doc.title != 'Home') {
            popUp.find(".popup-title").html(title);
        }
    });

    popUp.tinyDraggable();
};

/// <reference path="../core.js" />
/// <reference path="index-page.js" />

'use strict';

var headerNav = {
    domElements: {
        clock: null,
        languageCode: null,
        language: null,
        languageSelectorContainer: null,

        iconLogOut: null,
        iconCollapse: null,
        iconHome: null,
        swapView: null,
        changeLanguageLink: null,

        sideBar: null,
        sideBarContent: null,
        profileImage: null
    },

    resources: {
        labelAM: '',
        labelPM: '',
        months: []
    },

    languageCode: null,
    clockTime: null,
    hideHeaderDropdownMenuTimeout: null,

    urls: {
        home: alpha.urls.oldMainRootPath + '_Reports/PortalPage/PortalPage.aspx',
        changeLanguage: alpha.urls.mainRootPath + 'Language/Change'
    },

    init: function () {
        headerNav.initClock();
        headerNav.runClock();

        headerNav.cachedDomElements();
        headerNav.initLanguage();
        headerNav.registerEvents();

        $('[data-toggle="tooltip"]').tooltip();
    },

    initClock: function () {
        // to keep the clock difference least to the actual time,
        // we must run this first of all 
        headerNav.domElements.clock = $('#clock');
        var clock = headerNav.domElements.clock;
        headerNav.resources.labelAM = clock.data("label-am");
        headerNav.resources.labelPM = clock.data("label-pm");
        headerNav.resources.labelMonths = JSON.parse(clock.data("label-months").replace(/'/g, '"'));

        var clock = headerNav.domElements.clock;
        headerNav.clockTime = new Date(
            clock.attr("data-year"),
            clock.attr("data-month") - 1,
            clock.attr("data-day"),
            clock.attr("data-hour"),
            clock.attr("data-minute"),
            clock.attr("data-second"),
            0);
    },
    runClock: function () {
        headerNav.clockTime.setSeconds(headerNav.clockTime.getSeconds() + 1);

        var strTime = headerNav.formatClockTime(headerNav.clockTime);
        headerNav.domElements.clock.html(strTime);
        setTimeout(headerNav.runClock, 1000);
    },

    formatClockTime: function (clockTime) {
        var year = clockTime.getFullYear();
        var month = clockTime.getMonth();
        var day = clockTime.getDate();

        var hour = clockTime.getHours();
        var minute = clockTime.getMinutes();
        var second = clockTime.getSeconds();

        // Expected: hour from 0->11
        var strHours = hour >= 12 ? hour - 12 : hour;
        var strMinute = minute >= 10 ? minute : '0' + minute;
        var strSecond = second >= 10 ? second : '0' + second;
        var stra = hour >= 12 ? headerNav.resources.labelPM : headerNav.resources.labelAM;

        var str = strHours + ":" + strMinute + ":" + strSecond + " " + stra + " " +
            headerNav.resources.labelMonths[month] + " " + day + ", " + year + " GMT-4";
        return str;
    },
    cachedDomElements: function () {
        headerNav.domElements.languageCode = $("#language-code");
        headerNav.domElements.language = $("#language");
        headerNav.domElements.languageSelectorContainer = $(".language-selector-container");

        headerNav.domElements.iconLogOut = $(".icon-logout");
        headerNav.domElements.iconCollapse = $('.icon-collapse');
        headerNav.domElements.iconHome = $(".icon-home");
        headerNav.domElements.changeLanguageLink = $(".changelanguage-link");

        headerNav.domElements.sideBar = $('.sidebar');
        headerNav.domElements.sideBarContent = $('.content-wrapper');
        headerNav.domElements.profileImage = $("#logoProfileImg");
    },

    initLanguage: function () {
        headerNav.languageCode = headerNav.domElements.languageCode.val();

        var currentLanguage = headerNav.domElements.language.data("currentlanguage");
        var language = $(".list-language ." + currentLanguage + " > a").html();

        headerNav.domElements.languageSelectorContainer.html(language);
    },

    registerEvents: function () {
        headerNav.domElements.iconLogOut.click(alpha.core.signOut);

        headerNav.domElements.iconCollapse.click(function () {
            headerNav.collapseLeftPanel($(this));
        });

        headerNav.domElements.iconHome.click(function () {
            index.changeMenu(headerNav.urls.home);
        });

        headerNav.domElements.changeLanguageLink.click(function () {
            headerNav.changeLanguage($(this).data("value"));
        });
    },

    collapseLeftPanel: function (collapseButton) {
        var sidebar = headerNav.domElements.sideBar;
        var content = headerNav.domElements.sideBarContent;

        var isCollapse = sidebar.hasClass('collapsed');
        var activeSubMenu = $('.menu-item.active .sub-menu');

        if (isCollapse) {
            activeSubMenu.css('display', 'block');
            sidebar.removeClass('collapsed');
            content.removeClass('expanded');
            headerNav.domElements.profileImage.removeClass("collapse-image");
        } else {
            activeSubMenu.css('display', 'none');
            sidebar.addClass('collapsed');
            content.addClass('expanded');
            headerNav.domElements.profileImage.addClass("collapse-image");
        }
    },
    changeLanguage: function (lang) {
        jQuery.ajax({
            type: "POST",
            url: headerNav.urls.changeLanguage,
            data: { languageCode: lang },
            responseType: "json",
            success: function (result) {
                if (result !== undefined) {
                    // Remove www so that subdomain can override LANGUAGE value
                    var domain = window.location.hostname.replace(/([a-zA-Z0-9]+.)/, '').replace('www.', '');
                    $.cookie('LANGUAGE', lang, { expires: 30, path: domain });
                    headerNav.reloadPage();
                }
            }
        });
    },
    reloadPage: function () {
        top.location = headerNav.removeBookmarksInUrl(top.location.href);
    },
    removeBookmarksInUrl: function (url) {
        if (typeof url != 'string') {
            return null;
        }

        var i = url.indexOf('#');
        if (i == -1) {
            return url;
        }

        return url.substr(0, i);
    }
};

$(function () {
    headerNav.init();
});

/// <reference path="index.js" />

'use strict';
/*global index*/

var headerMessage = {
    urls: {
        oldMainAppNormalMessageForm: alpha.urls.oldMainRootPath + 'Info/NewMessageForm.aspx?pageQuery=',
        oldMainAppPrivateMessageForm: alpha.urls.oldMainRootPath + 'Info/NewMessageForm.aspx?pageQuery=private',
        oldMainAppSpecialMessageForm: alpha.urls.oldMainRootPath + 'Info/NewMessageForm.aspx?pageQuery=special',
        updatePrivateMessageCount: alpha.urls.oldMainRootPath + '_UserMessage/Handlers/UpdatePrivateMessageCount.ashx',
        getMessages: alpha.urls.oldMainRootPath + '_UserMessage/Handlers/GetMessages.ashx'
    },

    messageIndex: 0,
    messages: [],

    messageIdIndex: 0,
    messageContentIndex: 1,
    messageTypeIndex: 2,
    publicMessageId: 0,
    privateMessageId: 0,
    scrollMessageTimeout: null,

    domElements: {
        btnPrivateMessage: null,
        privateMessageCounter: null,
        liveMessage: null,
        btnSpecialMessage: null,
        marqueeMessage: null,
        hotMessages: null,
        userMessageModal: null,
        currentHotMessage: null,
        scroller: null
    },

    init: function () {
        headerMessage.cacheElements();
        headerMessage.registerEvents();

        headerMessage.showHotMessages();
        headerMessage.getMessages();
    },

    cacheElements: function () {
        headerMessage.domElements.btnPrivateMessage = $('#btnPrivateMessage');
        headerMessage.domElements.privateMessageCounter = $('#privateMessageCounter');
        headerMessage.domElements.liveMessage = $('#liveMessage');
        headerMessage.domElements.btnSpecialMessage = $('#btnSpecialMessage');
        headerMessage.domElements.marqueeMessage = $('#marqueeMessage');
        headerMessage.domElements.hotMessages = $('#userMessageModalBody');
        headerMessage.domElements.userMessageModal = $('#userMessageModal');
        headerMessage.domElements.currentHotMessage = headerMessage.domElements.hotMessages.find('p.current-hot-message');
        headerMessage.domElements.scroller = $('#scroller');
    },

    registerEvents: function () {
        headerMessage.domElements.btnPrivateMessage
            .off('click.headermsg')
            .on('click.headermsg', headerMessage.viewPrivateMessage);

        headerMessage.domElements.btnSpecialMessage
            .off('click.headermsg')
            .on('click.headermsg', headerMessage.viewSpecialMessage);

        headerMessage.domElements.liveMessage
            .off('click.headermsg')
            .on('click.headermsg', headerMessage.viewLiveMessage);

        headerMessage.domElements.marqueeMessage
            .off('click.headermsg')
            .on('click.headermsg', headerMessage.viewLiveMessage);
    },

    viewPrivateMessage: function () {

        index.changeMenu(headerMessage.urls.oldMainAppPrivateMessageForm);

        var countPrivateMsg = headerMessage.domElements.privateMessageCounter.html().replace("+", "");

        if (parseInt(countPrivateMsg) > 0) {
            //// Reset private message after click on view private message link
            $.ajax({
                url: headerMessage.urls.updatePrivateMessageCount,
                type: 'POST',
                responseType: 'json',
                success: function (rawResult) {
                    var result = alpha.core.convertReponseToJson(rawResult);
                    if (result.errCode === 0) {
                        headerMessage.domElements.privateMessageCounter.html(0);
                    }
                }
            });
        }
    },

    viewSpecialMessage: function () {
        index.changeMenu(headerMessage.urls.oldMainAppSpecialMessageForm);
    },

    viewLiveMessage: function () {
        var liveMessageUrl = headerMessage.domElements.liveMessage.data('live-message-url');
        if (liveMessageUrl) {
            index.changeMenu(liveMessageUrl);
        }
        else {
            index.changeMenu(headerMessage.urls.oldMainAppNormalMessageForm);
        }
    },

    showHotMessages: function () {
        if (headerMessage.domElements.currentHotMessage.length > 0) {
            headerMessage.domElements.userMessageModal.modal({ show: true, backdrop: false });
            headerMessage.domElements.userMessageModal.tinyDraggable();
        }

        setInterval(headerMessage.swapMessage, 18 * 1000);
    },

    showPublicMessage: function (publicMessage) {
        // Process Public Message: show new public message and its link to message form
        var url = headerMessage.urls.oldMainAppNormalMessageForm + publicMessage[headerMessage.messageTypeIndex];

        if (publicMessage[headerMessage.messageIdIndex]
            && publicMessage[headerMessage.messageIdIndex] !== headerMessage.publicMessageId) {

            headerMessage.domElements.liveMessage
                .html(publicMessage[headerMessage.messageContentIndex])
                .data(url);

            headerMessage.appendMessageToPopup(publicMessage[headerMessage.messageContentIndex]);

            headerMessage.publicMessageId = publicMessage[headerMessage.messageIdIndex];
        }
    },

    showPrivateMessage: function (privateMessage, unreadPriMsgCount) {
        // Process Private Message: show new private message
        if (privateMessage[headerMessage.messageIdIndex]
            && headerMessage.privateMessageId !== privateMessage[headerMessage.messageIdIndex]) {

            //// Append Private Message to Marquee with green color format.
            var privateMessageContent = headerMessage.domElements.liveMessage.find('#private-message-content');

            if (privateMessageContent.length > 0) {
                privateMessageContent.html(privateMessage[headerMessage.messageContentIndex]);
            }
            else {
                var liveMessageContent = headerMessage.domElements.liveMessage.html();
                headerMessage.domElements.liveMessage.html(
                    liveMessageContent
                    + ". <font id='private-message-content' color=\"green\">Private message: "
                    + privateMessage[headerMessage.messageContentIndex]
                    + "</font>");
            }

            headerMessage.appendMessageToPopup(privateMessage[headerMessage.messageContentIndex]);

            headerMessage.privateMessageId = privateMessage[headerMessage.messageIdIndex];
        }

        unreadPriMsgCount = unreadPriMsgCount > 99 ? "99+" : unreadPriMsgCount;

        headerMessage.domElements.privateMessageCounter.html(unreadPriMsgCount);
    },

    appendMessageToPopup: function (messsage) {
        headerMessage.domElements.hotMessages.append(
                '<p class="not-shown-message">'
                + messsage
                + '</p>');
    },

    getMessages: function () {
        $.ajax({
            type: "POST",
            url: headerMessage.urls.getMessages,
            responseType: "json",
            success: function (rawResults) {
                var results = alpha.core.convertReponseToJson(rawResults);

                try {
                    var publicMessage = results.public_message ? results.public_message : 0,
                        privateMessage = results.private_message ? results.private_message : 0,
                        unreadPriMsgCount = results.unread_private_messages ? results.unread_private_messages : 0;

                    headerMessage.showPublicMessage(publicMessage);

                    headerMessage.showPrivateMessage(privateMessage, unreadPriMsgCount);

                    headerMessage.applyScroller();
                } catch (e) { }

                setTimeout(headerMessage.getMessages, 60 * 1000);
            }
        });

    },

    swapMessage: function () {
        var nextMessage = [];

        if (headerMessage.domElements.currentHotMessage.length === 0) {
            nextMessage = headerMessage.domElements.hotMessages.find('p.not-shown-message:first');
        }
        else {
            nextMessage = headerMessage.domElements.currentHotMessage.next();
        }

        if (nextMessage.length > 0) {
            if (headerMessage.domElements.currentHotMessage.length > 0) {
                //// Hide old Message
                headerMessage.domElements.currentHotMessage.animate({
                    opacity: 0.0
                },
                1500,
                function () {
                    //// Remove Old Message and Show New Message
                    headerMessage.domElements.currentHotMessage.remove();
                    headerMessage.showNextMessage(nextMessage);
                });
            }
            else {
                headerMessage.showNextMessage(nextMessage);
            }

            headerMessage.domElements.userMessageModal.modal({ show: true, backdrop: false });
            headerMessage.domElements.userMessageModal.tinyDraggable();
        }
    },

    showNextMessage: function (nextMessage) {
        nextMessage.show().removeClass('not-shown-message').animate({
            opacity: 1.0
        },
        1500,
        function () {
            headerMessage.domElements.currentHotMessage = nextMessage;
        });
    },

    applyScroller: function () {
        if (headerMessage.scrollerLeft) {
            return;
        }

        headerMessage.scrollerLeft = 100;
        headerMessage.scrollerWidth = headerMessage.domElements.scroller.width();

        headerMessage.scroller = headerMessage.domElements.scroller[0];
        headerMessage.topnews = headerMessage.domElements.liveMessage[0];

        headerMessage.topnews.onmouseover = function () {
            clearTimeout(headerMessage.scrollMessageTimeout);
            headerMessage.scrollMessageTimeout = 0;
            headerMessage.scrollerPaused = true;
        };
        headerMessage.topnews.onmouseout = function () {
            if (headerMessage.scrollerPaused) {
                headerMessage.scrollerPaused = false;
                headerMessage.scrollMessageTimeout = setTimeout(headerMessage.scrollMessage, 20);
            } else {
                headerMessage.scrollerPaused = false;
            }
        };

        var textNews = headerMessage.domElements.marqueeMessage[0];
        headerMessage.maskWidth = parseInt(textNews.offsetWidth);

        headerMessage.scrollMessage();
    },

    scrollMessage: function () {
        if (headerMessage.scrollerPaused) {
            return;
        }

        if (headerMessage.scrollerLeft + headerMessage.scrollerWidth < -5) {
            headerMessage.scrollerLeft = headerMessage.maskWidth + 5;
            headerMessage.scrollerWidth = parseInt(headerMessage.topnews.offsetWidth);
        }

        headerMessage.scroller.style.left = (headerMessage.scrollerLeft--) + 'px';
        headerMessage.scrollMessageTimeout = setTimeout(headerMessage.scrollMessage, 20);
    }
};

$(function () {
    headerMessage.init();
});

/// <reference path="../core.js" />

'use strict';
/*global alpha, briefProfile*/

var index = {
    currentTab: null,
    currentPageCookieName: 'DFFKDHFJKLDHFJDH',

    domElements: {
        mainFrame: null,
        partialContents: null,
        content: null,
        accountInfoContent: null
    },

    urls: {
        reCheckinUrl: alpha.urls.oldMainRootPath + '_Authorization/Handlers/ReCheckin.ashx'
    },

    init: function () {
        window.document.title = alpha.urls.mainRootPath.substr(1, alpha.urls.mainRootPath.length - 2);
        $.material.init();
        index.cachedDomElements();
        index.setCurrentUrl();
        index.loadPartialViews();
        index.registerEvents();

        setTimeout(index.cacheCurrentUrl, 1000);
    },

    registerEvents: function () {
        setTimeout(index.reCheckin, 30000);
    },

    cachedDomElements: function () {
        index.domElements.partialContents = $('.partial-content');
        index.domElements.content = $('#content');
        index.domElements.accountInfoContent = $('#accountinfo-content');
    },

    changeMenu: function (url) {
        index.domElements.mainFrame.attr('src', url);
    },

    reCheckin: function () {
        // Post with return result in JSON format
        $.cookie('ssl', location.href.indexOf('https://') === 0, { expires: 30, path: '/' });

        $.ajax({
            type: "POST",
            url: index.urls.reCheckinUrl,
            data: { onid: alpha.user.onUserId },
            responseType: "json",
            success: function (result) {
                result = alpha.core.convertReponseToJson(result);
                var errCode = result.errCode;
                if (errCode === 0) {
                    setTimeout(index.reCheckin, 30000);
                } else {
                    var errMsg = result.errMsg;
                    if (errMsg) {
                        alert(errMsg);
                        alpha.core.signOut();
                    } else {
                        alert('Could not contact the server. Please check your network connection.');
                    }
                }
            }
        });
    },

    loadPartialViews: function () {
        index.domElements.partialContents.each(function (index, partialContent) {
            var $partialContent = $(partialContent),
                url = $partialContent.data('url');
            if (url && url.length > 0) {
                $partialContent.load(url + '?' + Math.random(), function () {
                    if ($partialContent.hasClass('customer-profile')) {
                        briefProfile.init();
                    }
                });
            }
        });
    },

    cacheCurrentUrl: function () {
        var currentUrl = document.getElementById("main").contentWindow.location.href;
       
        $.cookie(index.currentPageCookieName, currentUrl, { expires: 1, path: '/' });

        setTimeout(index.cacheCurrentUrl, 1000);
    },

    setCurrentUrl: function () {
        var currentUrl = $.cookie(index.currentPageCookieName);

        if (!currentUrl || currentUrl === 'about:blank') {
            currentUrl = index.domElements.content.data('default-url');
        }

        if (index.domElements.mainFrame
            && index.domElements.mainFrame.length > 0) {
            index.domElements.mainFrame.attr('src', currentUrl);
        }
        else {
            index.domElements.content.append('<iframe frameborder="0" id="main" src="' + currentUrl + '" width="100%" height="100%" scrolling="auto" name="main" marginwidth="0" marginheight="0" allowtransparency="true" class="iframe-content"></iframe>');
            index.domElements.mainFrame = $('#main');
            index.domElements.mainFrame.load(function () {
                var iframeWindow = index.domElements.mainFrame[0].contentWindow || index.domElements.mainFrame[0].contentDocument;
                index.closeAllOpenDropdowns(iframeWindow);
            });

            index.domElements.accountInfoContent.load(function () {
                var iframeWindow = index.domElements.accountInfoContent[0].contentWindow || index.domElements.accountInfoContent[0].contentDocument;
                index.closeAllOpenDropdowns(iframeWindow);
            });
        }
    },

    closeAllOpenDropdowns: function (iframeWindow) {
        $(iframeWindow.document).click(function () {
            $('.dropdown-toggle').each(function (index, dropdown) {
                var $dropdown = $(dropdown);
                if ($dropdown.parent().hasClass('open')) {
                    $dropdown.dropdown('toggle');
                }
            });
        });
    }
};

$(function () {
    index.init();
});

var menuBar = {
    domElements: {
        subMenuItems: null,
        menuOpener: null,
        subMenu: null,
        menuItems: null,
        sideBar: null,
        profilePicture: null,

        mainTabs: null,
        menuContainer: null,
        tabContentContainers: null,
        accountInfoTab: null,
        accountInfoContent: null
    },
    urls: {
        accountInfo: alpha.urls.oldMainRootPath + '_AccountInfo/AccountInfo.aspx?fromNewSite=1'
    },

    init: function () {
        menuBar.storeElements();
        menuBar.bindEvents();


    },
    storeElements: function () {
        menuBar.domElements.subMenuItems = $('.sub-menu-item');
        menuBar.domElements.menuOpener = $('.menu-opener');
        menuBar.domElements.subMenu = $('.sub-menu');
        menuBar.domElements.menuItems = $('.menu-item');
        menuBar.domElements.sideBar = $('.sidebar');
        menuBar.domElements.profilePicture = $("#logoProfileImg");

        menuBar.domElements.mainTabs = $('.main-tab li');
        menuBar.domElements.menuContainer = $('.mainmenu-container');
        menuBar.domElements.tabContentContainers = $('.tab-content-container');
        menuBar.domElements.accountInfoTab = $('.accountinfo');
        menuBar.domElements.accountInfoContent = $('.accinfo-content');
    },
    bindEvents: function () {
        menuBar.domElements.menuOpener.click(function () {
            var $this = $(this);
            var menuContainer = $this.closest('li');
            menuBar.domElements.menuItems.removeClass('active');

            menuContainer.addClass('active');
            var isVisible = $this.hasClass('active');
            menuBar.domElements.menuOpener.removeClass('active');
            menuBar.domElements.subMenu.slideUp('normal');

            var checkElement = $this.next();
            if (isVisible) {
                menuContainer.removeClass('active');
                $this.removeClass('active');

                if (checkElement.is('ul')) {
                    checkElement.slideUp('normal');
                }
            } else {
                menuContainer.addClass('active');
                $this.addClass('active');

                if (checkElement.is('ul')) {
                    checkElement.slideDown('normal');
                }
            }
        });

        menuBar.domElements.subMenuItems.click(function () {
            menuBar.activeSubMenuItem($(this));
        });

        menuBar.domElements.sideBar.hover(
            function () {
                if ($(this).hasClass('collapsed')) {
                    menuBar.domElements.profilePicture.removeClass("collapse-image");

                    var activeSubMenu = $('.menu-item.active .sub-menu');
                    activeSubMenu.css('display', 'block');
                }
            },
            function () {
                if ($(this).hasClass('collapsed')) {
                    menuBar.domElements.profilePicture.addClass("collapse-image");

                    var activeSubMenu = $('.menu-item.active .sub-menu');
                    activeSubMenu.css('display', 'none');
                }
            }
        );

        menuBar.domElements.mainTabs.click(function () {
            var $this = $(this);
            var tabClass = $this.attr('class');

            menuBar.domElements.mainTabs.removeClass('active');
            $this.addClass('active');

            var targetContainer = $("." + $this.data("target-container"));
            menuBar.domElements.tabContentContainers.css('display', 'none');
            targetContainer.css('display', 'block');
        });

        menuBar.domElements.accountInfoTab.click(function () {
            var accInfoContent = menuBar.domElements.accountInfoContent;
            var src = accInfoContent.attr('src');
            if (!src) {
                accInfoContent.attr('src', menuBar.urls.accountInfo);
            }

            accInfoContent.unbind("load").load(function () {
                var contentHeight = accInfoContent.contents().find("html").height();

                var iframeHeightPadding = 20;

                var iframeHeight = contentHeight + iframeHeightPadding;
                var topHeight = 88;
                var maxHeight = $(window).height() - 50 - topHeight;
                if (iframeHeight >= maxHeight) {
                    iframeHeight = maxHeight;
                }

                accInfoContent.css("height", iframeHeight);
            });

        });

        menuBar.domElements.accountInfoTab.click(function () {
            var accInfoContent = menuBar.domElements.accountInfoContent;
            var src = accInfoContent.attr('src');
            if (!src) {
                accInfoContent.attr('src', menuBar.urls.accountInfo);
            }

            accInfoContent.unbind("load").load(function () {
                var contentHeight = accInfoContent.contents().find("html").height();
                var iframeHeightPadding = 20;
                var iframeHeight = contentHeight + iframeHeightPadding;
                accInfoContent.css("height", iframeHeight);
            });

        });
       
    },
    activeSubMenuItem: function (subMenu) {
        menuBar.domElements.subMenuItems.removeClass('active');
        subMenu.addClass('active');
    }
};

$(function () {
    menuBar.init();
});

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
            popUpContainer.tinyDraggable();
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
'use strict';

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
        && briefProfile.getCountNickNameToolTip() < briefProfile.maxCountNickNameTooltip
        && briefProfile.domElements.profileContent.data('nick-name').length === 0;

        if (isShowEditProfilePictureTooltip) {
            briefProfile.showTooltipProfilePicture();
        }
    },

    checkAndShowNickNameTooltip: function () {
        var countNickNameToolTip = briefProfile.getCountNickNameToolTip();

        if (countNickNameToolTip >= briefProfile.maxCountNickNameTooltip) {
            briefProfile.showTooltipIntroNickname();
        }
        else {
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
            }, 30000);
        }
    },

    showTooltipIntroNickname: function () {
        if (briefProfile.domElements.toolTipBox.length > 0) {
            briefProfile.domElements.toolTipBox.show();

            setTimeout(function () {
                briefProfile.hideTooltipIntroNickname();
            }, 30000);
        }
    },

    hideAllTooltips: function () {
        briefProfile.hideTooltipIntroNickname();
        briefProfile.hideProfileTooltip();
    }
};