/// <reference path="../core.ts" />

var menuBar = {
    loadAuthTimeout: null,
    domElements: {
        subMenuItems: null,
        menuOpeners: null,
        subMenus: null,
        menuItems: null,
        sideBar: null,
        accountInfoTab: null,
        accountInfoContent: null,
        redirectItems: null,
        extraIcon: null,
        header: null,
        userIcon: null,
        mainTab: null,
        mainContent: null,
        mainMenuTab: null,
        mainMenuHeader: null,
        accountInfo: null,
        accountInfoHeader: null,
        hamburgerIcon: null,
        profileImage: null,
        announcementItems: null,
        closeIconOnMobile: null,
        param: null,
        reportDomain: null
    },

    urls: {
        accountInfo: alpha.urls.accountInfoRootPath + 'SearchAccount'
    },

    init: function () {
        menuBar.storeElements();
        menuBar.bindEvents();
        menuBar.initSideBar();
        menuBar.loadAuthFrame();
    },

    storeElements: function () {
        menuBar.domElements.subMenuItems = $('.sub-menu-item');
        menuBar.domElements.menuOpeners = $('.menu-opener');
        menuBar.domElements.subMenus = $('.sub-menu');
        menuBar.domElements.menuItems = $('.menu-item');
        menuBar.domElements.sideBar = $('#side-bar');
        menuBar.domElements.accountInfoContent = $('.accinfo-content');
        menuBar.domElements.redirectItems = $('.redirect-item');
        menuBar.domElements.extraIcon = $('.extra-box');
        menuBar.domElements.header = $('#header');
        menuBar.domElements.userIcon = $('#user-icon');
        menuBar.domElements.mainTab = $('#main-tab');
        menuBar.domElements.mainContent = $('#content-wrapper');
        menuBar.domElements.mainMenuTab = $('#main-menu-tab');
        menuBar.domElements.accountInfoTab = $('#account-info-tab');
        menuBar.domElements.accountInfoHeader = $('#account-info-header');
        menuBar.domElements.mainMenuHeader = $('#main-menu-header');
        menuBar.domElements.hamburgerIcon = $('#hamburger-icon');
        menuBar.domElements.profileImage = $('#logoProfileImg');
        menuBar.domElements.announcementItems = $("#menu-ANNOUNCEMENTS li a");
        menuBar.domElements.closeIconOnMobile = $('#closeIconOnMobile');
        menuBar.domElements.param = $('#param').val();
        menuBar.domElements.reportDomain = $('#ReportDomain').val();
    },

    initSideBar: function () {
        var sidebar = menuBar.domElements.sideBar,
            mainContent = menuBar.domElements.mainContent;

        var activeSubMenu = $('.menu-item.active .sub-menu');

        if (alpha.isScreenLower768) {
            //mainContent.addClass('expanded-mobile');
            sidebar.addClass('hidden-mobile');
            return;
        }

        if (alpha.isScreenGreater768AndLower1024) {
            menuBar.collapseLeftPanel();
            return;
        }

        if (alpha.isScreenGreater1024) {
            activeSubMenu.show();
            sidebar.removeClass('collapsed').removeClass('collapsed-smallsize');
            mainContent.removeClass('expanded');
        }
    },

    isSideBarCollapsed: function () {
        return menuBar.domElements.sideBar.hasClass('collapsed');
    },

    bindEvents: function () {
        menuBar.domElements.menuOpeners.click(function (e) {
            var $this = $(this),
                sideBar = menuBar.domElements.sideBar,
                isCollapsed = sideBar.hasClass('focusout')
                    && sideBar.hasClass('collapsed-smallsize')
                    && !sideBar.hasClass('docked'),
                isActive = $this.hasClass('active');

            menuBar.expandSideBar();

            if (isCollapsed && isActive) {
                menuBar.activateMainMenuTab();
            }
            else {
                menuBar.toggleMainMenu($this);
            }
        });

        menuBar.domElements.subMenuItems.click(function () {
            menuBar.activateSubMenuItem($(this));
        });

        menuBar.domElements.mainMenuHeader.click(menuBar.activateMainMenuTab);
        menuBar.domElements.accountInfoHeader.click(menuBar.activateAccountInfoTab);
        menuBar.domElements.userIcon.click(menuBar.activateAccountInfoTab);
        menuBar.domElements.redirectItems.click(menuBar.collapseSideBar);
        //menuBar.domElements.header.click(menuBar.collapseSideBar);
        menuBar.domElements.extraIcon.click(menuBar.expandSideBar);
        menuBar.domElements.hamburgerIcon.click(function (e) {
            menuBar.collapseLeftPanel();
            if (alpha.isScreenLower768) {
                e.stopPropagation();
            }
        });
        menuBar.registerMouseHoverForDesktopBrowsers();

        menuBar.domElements.announcementItems.click(function () {
            menuBar.clickAnnouncementItem($(this));
        });

        menuBar.domElements.closeIconOnMobile.click(menuBar.collapseSideBar);
    },

    clickAnnouncementItem: function (menu) {
        var url = menu.attr("href");
        var parameters = url.split('?');
        if (parameters.length == 2) {
            var messageType = parameters[1].split('=')[1];
            headerNav.buildIconNewMessageByType(messageType);
        } else {
            headerNav.buildIconNewMessageByType(headerNav.messageType.System);
        }
    },

    registerMouseHoverForDesktopBrowsers: function () {
        if (!alpha.isMobile) {
            menuBar.domElements.sideBar
                .off('mouseenter.menubar')
                .on('mouseenter.menubar', function (e) {
                    setTimeout(menuBar.expandSideBar, 50);
                });

            menuBar.domElements.sideBar
                .off('mouseleave.menubar')
                .on('mouseleave.menubar', menuBar.collapseSideBar);
        }
    },

    collapseLeftPanel: function () {
        var sidebar = menuBar.domElements.sideBar,
            content = menuBar.domElements.mainContent,
            header = menuBar.domElements.header,
            mainContent = menuBar.domElements.mainContent,
            closeIconOnMobile = menuBar.domElements.closeIconOnMobile,
            hamburgerIcon = menuBar.domElements.hamburgerIcon;

        if (alpha.isScreenLower768) {
            if (sidebar.hasClass('hidden-mobile')) {
                content.addClass('expanded-mobile');
                sidebar.removeClass('hidden-mobile').addClass('visible-mobile');
                header.addClass('expanded-header-mobile');
                hamburgerIcon.hide();
                closeIconOnMobile.addClass('active');
            }
            else {
                content.removeClass('expanded-mobile');
                sidebar.addClass('hidden-mobile collapse-mobile').removeClass('visible-mobile');
                header.removeClass('expanded-header-mobile');
            }
        }
        else {
            var collapsedClass = 'collapsed',
                expandedClass = 'expanded',
                collapseImageClass = 'collapse-image',
                dockedClass = 'docked',
                isCollapsed = sidebar.hasClass(collapsedClass),
                isHidenMobile = sidebar.hasClass('hidden-mobile'),
                activeSubMenu = $('.menu-item.active .sub-menu'),
                profileImage = menuBar.domElements.profileImage;

            if (isCollapsed) {
                activeSubMenu.show();
                sidebar.addClass(dockedClass).removeClass(collapsedClass);
                content.removeClass(expandedClass);
                profileImage.removeClass(collapseImageClass);

                //// When expanding, check current active tab, and active it
                if (menuBar.domElements.accountInfoHeader.hasClass('active')) {
                    menuBar.domElements.mainMenuTab.hide();
                    menuBar.domElements.accountInfoTab.show()
                }
                else {
                    menuBar.domElements.mainMenuTab.show();
                    menuBar.domElements.accountInfoTab.hide();
                }
            } else {
                activeSubMenu.hide();
                sidebar.removeClass(dockedClass).addClass(collapsedClass);
                content.addClass(expandedClass);
                profileImage.addClass(collapseImageClass);
                menuBar.domElements.mainMenuTab.show();
                menuBar.domElements.accountInfoTab.hide();
            }
        }

        menuBar.positionPTTooltip();
    },

    activateAccountInfoTab: function (event?: any, userName: string = '') {
        menuBar.domElements.accountInfoHeader.addClass('active');
        menuBar.domElements.mainMenuHeader.removeClass('active');
        menuBar.domElements.mainMenuTab.hide();
        menuBar.domElements.accountInfoTab.show();

        var accInfoContent = menuBar.domElements.accountInfoContent,
            accountInfoUrl = accInfoContent.attr('src');

        if (!accountInfoUrl) {
			accInfoContent.attr('src', menuBar.urls.accountInfo + '?mobileView=' + alpha.isScreenLower768);
        }

        if (userName !== '' || typeof userName === 'undefined') {
			accInfoContent.attr('src', menuBar.urls.accountInfo + '?mobileView=' + alpha.isScreenLower768 + '&SearchAccountName=' + userName);
        }

        // To work on IE, use plain-old javascript onload event
        accInfoContent[0].onload = function () {
            var navigationHeight = $('#navigation').height();
            var mainMenuHeight = $('.main-tab').height();
            var accountInfoIframeHeight = navigationHeight - mainMenuHeight;
            menuBar.domElements.accountInfoTab.css("height", accountInfoIframeHeight);

            if (alpha.isMobile) {
                $('#accountinfo-content').css("height", accountInfoIframeHeight - 20);
                menuBar.domElements.accountInfoContent.addClass('small-device');
            } else {
                $('#accountinfo-content').css("height", accountInfoIframeHeight - 5);
            }
        };
    },

	activateSubMenuItem: function (subMenu) {
        menuBar.domElements.subMenuItems.removeClass('active');
		subMenu.addClass('active');
        try {
            if (!alpha.core.isIe()) {
                $('#main').contents().find("head").append($("#AssetCommonCssLink").val());
                $('#main').contents().find('body').empty().block();
            }
        } catch(e) { }

        if (subMenu.data("domain")) {
            var sessionId = $('#sessionId').val();
            var cookieless = '/(S(' + sessionId + '))';
            var appPath = subMenu.data("apppath") + cookieless;
            $('#main').attr('src', subMenu.data("domain") + appPath + subMenu.data("url")).attr("target", "main");
        }
        else {
            $('#main').attr('src', subMenu.data("url")).attr("target", "main");
        }
    },

    expandSideBar: function () {
        var sideBar = menuBar.domElements.sideBar;
        if (sideBar.hasClass('collapsed')) {
            sideBar.addClass('hovered').removeClass('focusout');

            //// Show all active sub menus
            var activeSubMenus = $('.menu-item.active .sub-menu');
            activeSubMenus.css('display', 'block');

            //// When expanding, if active tab is account info, then hide menu bar
            if (menuBar.domElements.accountInfoHeader.hasClass('active')) {
                menuBar.domElements.mainMenuTab.hide();
                menuBar.domElements.accountInfoTab.show();
            }
        }
    },

    collapseSideBar: function () {
        var sideBar = menuBar.domElements.sideBar,
            closeIconOnMobile = menuBar.domElements.closeIconOnMobile,
            hamburgerIcon = menuBar.domElements.hamburgerIcon;

        if (alpha.isScreenLower768 && sideBar.hasClass('visible-mobile')) {
            menuBar.domElements.mainContent.removeClass('expanded-mobile');
            menuBar.domElements.header.removeClass('expanded-header-mobile');
            sideBar.addClass('hidden-mobile').removeClass('visible-mobile');
            closeIconOnMobile.removeClass('active');
            hamburgerIcon.show();
        }
        else {
            if (sideBar.hasClass('docked')) {
                return;
            }

            if (sideBar.hasClass('collapsed')) {
                sideBar.removeClass('hovered').addClass('focusout');

                //// Hide all expanded sub menus
                var activeSubMenus = $('.menu-item.active .sub-menu');
                activeSubMenus.css('display', 'none');

                //// Show all menu bar icon
                menuBar.domElements.mainMenuTab.show();
                menuBar.domElements.accountInfoTab.hide();
            }
        }
    },

    activateMainMenuTab: function () {
        //// Set header tab state, and hide account infor tab
        menuBar.domElements.accountInfoHeader.removeClass('active');
        menuBar.domElements.accountInfoTab.hide();
        menuBar.domElements.mainMenuHeader.addClass('active');
        menuBar.domElements.mainMenuTab.show();
    },

    toggleMainMenu: function (menuOpener) {
        menuBar.activateMainMenuTab();

        var mainMenu = menuOpener.parent(),
            isVisible = menuOpener.hasClass('active');

        //// Hide all active menus
        menuBar.domElements.menuItems.removeClass('active');
        menuBar.domElements.menuOpeners.removeClass('active');
        menuBar.domElements.subMenus.slideUp('fast');

        var subMenuContainer = menuOpener.next();
        if (isVisible) {
            //// Unset active for selected menu, when collapse
            mainMenu.removeClass('active');
            menuOpener.removeClass('active');

            //// Incase of has sub menu, then collapse sub menus
            if (subMenuContainer.is('ul')) {
                subMenuContainer.slideUp('fast');
            }
        } else {
            //// Expand selected menu
            mainMenu.addClass('active');
            menuOpener.addClass('active');

            //// expand all sub menu
            if (subMenuContainer.is('ul')) {
                subMenuContainer.slideDown('fast');
            }
        }
    },

    positionPTTooltip: function () {
        // tabPT tool tip
        setTimeout(function () {
            var tabPT = $('#toolTipSettingMinPT');
            if (tabPT && tabPT.length) {
                var tabPTLeft = $('.sidebar').width() + 20;
                tabPT.css('left', tabPTLeft);
            }
        }, 400);
    },

    loadAuthFrame: function () {
        var sessionId = $('#sessionId').val();
        var domain = menuBar.domElements.reportDomain;
        var authUrl = domain + '/site-reports/(S(' + sessionId + '))/Auth';
        var url = authUrl + '?param=' + menuBar.domElements.param;

        menuBar.domElements.header.find('iframe#reportFrame')[0].src = url;
    }
};

$(function () {
    menuBar.init();
});