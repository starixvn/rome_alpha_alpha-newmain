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
        accountInfoContent: null,
        redirectItems: null
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
        menuBar.domElements.sideBar = $('.sidebar');
        menuBar.domElements.profilePicture = $("#logoProfileImg");

        menuBar.domElements.mainTabs = $('.main-tab li');
        menuBar.domElements.menuContainer = $('.mainmenu-container');
        menuBar.domElements.tabContentContainers = $('.tab-content-container');
        menuBar.domElements.accountInfoTab = $('.accountinfo');
        menuBar.domElements.accountInfoContent = $('.accinfo-content');
        menuBar.domElements.redirectItems = $('.redirect-item');
    },
    layoutSmallSize: function () {
        var width = $(window).width();
        var content = $('.content-wrapper');
        var isCollapse = menuBar.domElements.sideBar.hasClass('collapsed');
        var activeSubMenu = $('.menu-item.active .sub-menu');
        if (width <= 1024) {
            if (!isCollapse) {
                activeSubMenu.css('display', 'none');
                menuBar.domElements.sideBar.addClass('collapsed');
                content.addClass('expanded');
                menuBar.domElements.profilePicture.addClass("collapse-image");
            }
        }
        else {
            activeSubMenu.css('display', 'block');
            menuBar.domElements.sideBar.removeClass('collapsed');
            content.removeClass('expanded');
            menuBar.domElements.profilePicture.removeClass("collapse-image");
        }
    },
    bindEvents: function () {
        menuBar.domElements.menuOpener.click(function () {
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
        });

        menuBar.domElements.subMenuItems.click(function () {
            menuBar.activeSubMenuItem($(this));
        });

        menuBar.domElements.sideBar.hover(
            function () {
                menuBar.showMenuBar();
            },
            function () {
                menuBar.hideMenuBar();
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
        });

        menuBar.domElements.redirectItems.click(function () {
            menuBar.hideMenuBar();
        });

        $(window).resize(function () {
            menuBar.layoutSmallSize();
        });

    },
    activeSubMenuItem: function (subMenu) {
        menuBar.domElements.subMenuItems.removeClass('active');
        subMenu.addClass('active');
    },

    showMenuBar: function () {
        if (menuBar.domElements.sideBar.hasClass('collapsed')) {
            menuBar.domElements.sideBar.addClass('hovered');
            menuBar.domElements.sideBar.removeClass('focusout');

            menuBar.domElements.profilePicture.removeClass("collapse-image");

            var activeSubMenu = $('.menu-item.active .sub-menu');
            activeSubMenu.css('display', 'block');
        }
    },
    hideMenuBar: function () {
        if (menuBar.domElements.sideBar.hasClass('collapsed')) {
            menuBar.domElements.profilePicture.addClass("collapse-image");

            menuBar.domElements.sideBar.removeClass('hovered');
            menuBar.domElements.sideBar.addClass('focusout');

            var activeSubMenu = $('.menu-item.active .sub-menu');
            activeSubMenu.css('display', 'none');
        }
    },
    collapseMenu: function (clickedItem) {
        var menuContainer = clickedItem.closest('li');
        menuBar.domElements.menuItems.removeClass('active');

        menuContainer.addClass('active');
        var isVisible = clickedItem.hasClass('active');
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
