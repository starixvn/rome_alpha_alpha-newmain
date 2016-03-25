var sideBarManager;

jQuery(document).ready(function () {
    sideBarManager.init();
});

var SideBarManager = function () {
    var self = this;
    self.data = {
        dom: {
            subMenuItems: null
        }
    };

    self.init = function () {
        self.storeElements();
        self.bindEvents();
    };

    self.storeElements = function () {
        self.data.dom.subMenuItems = $('.sub-menu-item');
    };

    self.bindEvents = function () {
        var menuOpener = $('.menu-opener');
        var subMenu = $('.sub-menu');
        var menuItems = $('.menu-item');
        menuOpener.click(function () {
            var $this = $(this);
            var menuContainer = $this.closest('li');
            menuItems.removeClass('active');

            menuContainer.addClass('active');
            var isVisible = $this.hasClass('active');
            menuOpener.removeClass('active');
            subMenu.slideUp('normal');
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

        self.data.dom.subMenuItems.click(function () {
            self.activeSubMenuItem($(this));
        });

        $('.sidebar').hover(
            function () {
                if ($(this).hasClass('collapsed')) {
                    $("#logoProfileImg").removeClass("collapse-image");

                    var activeSubMenu = $('.menu-item.active .sub-menu');
                    activeSubMenu.css('display', 'block');
                }
            },
            function () {
                if ($(this).hasClass('collapsed')) {
                    $("#logoProfileImg").addClass("collapse-image");

                    var activeSubMenu = $('.menu-item.active .sub-menu');
                    activeSubMenu.css('display', 'none');
                }
            }
        );

        $('.main-tab li').click(function () {
            var $this = $(this);
            var cla = $(this).attr('class');
            if ($this.hasClass('mainmenu')) {
                $('.main-tab li').removeClass('active');
                $(this).addClass('active');
                $('.mainmenu-container').css('display', 'block');
                $('.accountinfo-container').css('display', 'none');
            }
            else if ($this.hasClass('accountinfo')) {
                $('.main-tab li').removeClass('active');
                $(this).addClass('active');
                $('.accountinfo-container').css('display', 'block');
                $('.mainmenu-container').css('display', 'none');
            }
        });

        $('.accountinfo').click(function () {
            var accInfoContent = $('.accinfo-content');
            var src = accInfoContent.attr('src');
            if (!src) {
                accInfoContent.attr('src', '/' + baseOldMainApp + '_AccountInfo/AccountInfo.aspx?fromNewSite=1');
            }
        });

    };
    self.activeSubMenuItem = function (subMenu) {
        self.data.dom.subMenuItems.removeClass('active');
        subMenu.addClass('active');
    };
    self.expandMenu = function (menu) {
        var firstMenu = $('.list-menu > .menu-item:first-child');
        menu.addClass('active');
        menu.find('.parent-menu').addClass('active');
        menu.find('.sub-menu').slideDown('normal');
    };
};

sideBarManager = new SideBarManager();