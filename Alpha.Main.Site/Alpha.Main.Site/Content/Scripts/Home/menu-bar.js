var menuBarManager = {
    domElements: {
        subMenuItems: null,
        menuOpener: null,
        subMenu: null,
        menuItems: null,
        sideBar: null,
        profilePicture: null,

        mainTabs: null,
        menuContainer: null,
        navigationContainers: null,
        accountInfoTab: null,
        accountInfoContent: null
    },
    urls: {
        accountInfo: alpha.urls.oldMainRootPath + '_AccountInfo/AccountInfo.aspx?fromNewSite=1'
    },

    init: function () {
        menuBarManager.storeElements();
        menuBarManager.bindEvents();
    },
    storeElements: function () {
        menuBarManager.domElements.subMenuItems = $('.sub-menu-item');
        menuBarManager.domElements.menuOpener = $('.menu-opener');
        menuBarManager.domElements.subMenu = $('.sub-menu');
        menuBarManager.domElements.menuItems = $('.menu-item');
        menuBarManager.domElements.sideBar = $('.sidebar');
        menuBarManager.domElements.profilePicture = $("#logoProfileImg");

        menuBarManager.domElements.mainTabs = $('.main-tab li');
        menuBarManager.domElements.menuContainer = $('.mainmenu-container');
        menuBarManager.domElements.navigationContainers = $('.navigation .container');
        menuBarManager.domElements.accountInfoTab = $('.accountinfo');
        menuBarManager.domElements.accountInfoContent = $('.accinfo-content');
    },
    bindEvents: function () {
        menuBarManager.domElements.menuOpener.click(function () {
            var $this = $(this);
            var menuContainer = $this.closest('li');
            menuBarManager.domElements.menuItems.removeClass('active');

            menuContainer.addClass('active');
            var isVisible = $this.hasClass('active');
            menuBarManager.domElements.menuOpener.removeClass('active');
            menuBarManager.domElements.subMenu.slideUp('normal');

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

        menuBarManager.domElements.subMenuItems.click(function () {
            menuBarManager.activeSubMenuItem($(this));
        });

        menuBarManager.domElements.sideBar.hover(
            function () {
                if ($(this).hasClass('collapsed')) {
                    menuBarManager.domElements.profilePicture.removeClass("collapse-image");

                    var activeSubMenu = $('.menu-item.active .sub-menu');
                    activeSubMenu.css('display', 'block');
                }
            },
            function () {
                if ($(this).hasClass('collapsed')) {
                    menuBarManager.domElements.profilePicture.addClass("collapse-image");

                    var activeSubMenu = $('.menu-item.active .sub-menu');
                    activeSubMenu.css('display', 'none');
                }
            }
        );

        menuBarManager.domElements.mainTabs.click(function () {
            var $this = $(this);
            var tabClass = $this.attr('class');

            menuBarManager.domElements.mainTabs.removeClass('active');
            $this.addClass('active');

            var targetContainer = $("." + $this.data("target-container"));
            menuBarManager.domElements.navigationContainers.css('display', 'none');
            targetContainer.css('display', 'block');
        });

        menuBarManager.domElements.accountInfoTab.click(function () {
            var accInfoContent = menuBarManager.domElements.accountInfoContent;
            var src = accInfoContent.attr('src');
            if (!src) {
                accInfoContent.attr('src', menuBarManager.urls.accountInfo);
            }
        });
    },
    activeSubMenuItem: function (subMenu) {
        menuBarManager.domElements.subMenuItems.removeClass('active');
        subMenu.addClass('active');
    }
};

$(function () {
    menuBarManager.init();
});
