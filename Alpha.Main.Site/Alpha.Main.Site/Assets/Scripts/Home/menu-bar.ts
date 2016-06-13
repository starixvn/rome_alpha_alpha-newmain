var menuBar = {
    domElements: {
        subMenuItems: null,
        menuOpener: null,
        subMenu: null,
        menuItems: null,
        sideBar: null,
        mainTabs: null,
        menuContainer: null,
        tabContentContainers: null,
        accountInfoTab: null,
        accountInfoContent: null,
        redirectItems: null,
        extraIcon: null,
        header: null,
        userIcon: null,
        mainTab: null,
        mainContent: null
    },

    urls: {
        accountInfo: alpha.urls.oldMainRootPath + '_AccountInfo/AccountInfo.aspx'
    },

    init: function () {
        menuBar.storeElements();
        menuBar.bindEvents();
        menuBar.layoutSmallSize();
    },

    storeElements: function () {
        menuBar.domElements.subMenuItems = $('.sub-menu-item');
        menuBar.domElements.menuOpener = $('.menu-opener');
        menuBar.domElements.subMenu = $('.sub-menu');
        menuBar.domElements.menuItems = $('.menu-item');
        menuBar.domElements.sideBar = $('#side-bar');
        menuBar.domElements.mainTabs = $('.main-tab li');
        menuBar.domElements.menuContainer = $('.mainmenu-container');
        menuBar.domElements.tabContentContainers = $('.tab-content-container');
        menuBar.domElements.accountInfoTab = $('.accountinfo');
        menuBar.domElements.accountInfoContent = $('.accinfo-content');
        menuBar.domElements.redirectItems = $('.redirect-item');
        menuBar.domElements.extraIcon = $('.extra-box');
        menuBar.domElements.header = $('#header');
        menuBar.domElements.userIcon = $('#user-icon');
        menuBar.domElements.mainTab = $('#main-tab');
        menuBar.domElements.mainContent = $('#content-wrapper');
    },

    layoutSmallSize: function () {
        var width = $(window).width(),
            isCollapsed = menuBar.domElements.sideBar.hasClass('collapsed'),
            activeSubMenu = $('.menu-item.active .sub-menu');

        if (width < 768) {
            menuBar.domElements.sideBar.addClass('hidden-mobile');
            menuBar.domElements.mainContent.addClass('expanded-mobile');
        }
        else if (width <= 1024) {
            if (!isCollapsed) {
                if (menuBar.domElements.sideBar.hasClass('docked')) {
                    return;
                }

                activeSubMenu.css('display', 'none');
                menuBar.domElements.sideBar.addClass('collapsed').addClass('collapsed-smallsize');
                menuBar.domElements.mainContent.addClass('expanded');

            }
        }
        else {
            var isAutoCollapse = menuBar.domElements.sideBar.hasClass('collapsed-smallsize');
            if (isAutoCollapse) {
                activeSubMenu.css('display', 'block');
                menuBar.domElements.sideBar.removeClass('collapsed').removeClass('collapsed-smallsize');
                menuBar.domElements.mainContent.removeClass('expanded');
            }
        }
    },

    bindEvents: function () {
        menuBar.domElements.menuOpener.click(function (e) {
            var $this = $(this);
            if (menuBar.domElements.sideBar.hasClass('collapsed') &&
                menuBar.domElements.sideBar.hasClass('focusout')) {
                menuBar.showMenuBar();
                if (!$this.hasClass('active')) {
                    menuBar.collapseMenu($this);
                }
            }
            else {
                menuBar.collapseMenu($this);
            }

            e.stopPropagation();
        });

        menuBar.domElements.subMenuItems.click(function () {
            menuBar.activeSubMenuItem($(this));
        });

        //menuBar.domElements.sideBar.off('mouseenter.menubar')
        //    .on('mouseenter.menubar', function (e) {
        //        setTimeout(menuBar.showMenuBar, 50);
        //    });

        //menuBar.domElements.sideBar.off('mouseleave.menubar')
        //    .on('mouseleave.menubar', menuBar.hideMenuBar);

        menuBar.domElements.mainTabs.click(function () {
            menuBar.openTab($(this));
        });

        menuBar.domElements.accountInfoTab.click(menuBar.showAccountInfo);

        menuBar.domElements.redirectItems.click(function () {
            menuBar.hideMenuBar();
        });

        menuBar.domElements.header.click(menuBar.hideMenuBar);

        menuBar.domElements.extraIcon.click(function (e) {
            setTimeout(menuBar.showMenuBar, 50);
        });
    },

    activeSubMenuItem: function (subMenu) {
        menuBar.domElements.subMenuItems.removeClass('active');
        subMenu.addClass('active');
    },

    openTab: function (tab) {
        if (!tab.hasClass('withripple')) {
            return;
        }

        var currentActiveTab = menuBar.domElements.mainTab.data('active-tab'),
            tabClass = tab.attr('class');

        menuBar.domElements.mainTabs.removeClass('active');
        tab.addClass('active');

        menuBar.domElements.tabContentContainers.css('display', 'none');
        var targetContainer = $("." + tab.data("target-container"));
        targetContainer.css('display', 'block');
    },

    showAccountInfo: function () {
        var accInfoContent = menuBar.domElements.accountInfoContent;
        var src = accInfoContent.attr('src');
        if (!src) {
            accInfoContent.attr('src', menuBar.urls.accountInfo);
        }

        // To work on IE, use plain-old javascript onload event
        accInfoContent[0].onload = function () {
            if ((/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent))) {
                menuBar.domElements.accountInfoContent.addClass('small-device');
            }
            else {
                var contentHeight = accInfoContent.contents().find("body").height();
                var iframeHeightPadding = 40;
                var iframeHeight = contentHeight + iframeHeightPadding;
                accInfoContent.css("height", iframeHeight);
            }
        };
    },

    showMenuBar: function () {
        if (menuBar.domElements.sideBar.hasClass('collapsed')) {
            menuBar.domElements.sideBar.addClass('hovered').removeClass('focusout');

            var activeSubMenu = $('.menu-item.active .sub-menu');
            activeSubMenu.css('display', 'block');
        }

        if (menuBar.domElements.sideBar.hasClass('visible-mobile')) {
            menuBar.domElements.sideBar
                .addClass('hidden-mobile')
                .removeClass('visible-mobile');
        }
    },

    hideMenuBar: function () {
        if (menuBar.domElements.sideBar.hasClass('collapsed')) {
            menuBar.domElements.sideBar.removeClass('hovered').addClass('focusout');

            var activeSubMenu = $('.menu-item.active .sub-menu');
            activeSubMenu.css('display', 'none');
        }

        if (menuBar.domElements.sideBar.hasClass('visible-mobile'))
        {
            menuBar.domElements.sideBar
                .addClass('hidden-mobile')
                .removeClass('visible-mobile');
        }
    },

    collapseMenu: function (clickedItem) {
        var menuContainer = clickedItem.closest('li');
        menuContainer.addClass('active');

        var isVisible = clickedItem.hasClass('active');
        menuBar.domElements.menuItems.removeClass('active');
        menuBar.domElements.menuOpener.removeClass('active');
        menuBar.domElements.subMenu.slideUp('normal');

        var checkElement = clickedItem.next();
        if (isVisible) {
            menuContainer.removeClass('active');
            clickedItem.removeClass('active');

            if (checkElement.is('ul')) {
                checkElement.slideUp('normal');
            }

        } else {
            menuContainer.addClass('active');
            clickedItem.addClass('active');

            if (checkElement.is('ul')) {
                checkElement.slideDown('normal');
            }
        }
    }
};

$(function () {
    menuBar.init();
});
