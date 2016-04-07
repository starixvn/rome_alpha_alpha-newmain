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
        navigationContainers: null,
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
        menuBar.domElements.navigationContainers = $('.navigation .container');
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
            menuBar.domElements.navigationContainers.css('display', 'none');
            targetContainer.css('display', 'block');
        });

        menuBar.domElements.accountInfoTab.click(function () {
            var accInfoContent = menuBar.domElements.accountInfoContent;
            var src = accInfoContent.attr('src');
            if (!src) {
                accInfoContent.attr('src', menuBar.urls.accountInfo);
            }
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
