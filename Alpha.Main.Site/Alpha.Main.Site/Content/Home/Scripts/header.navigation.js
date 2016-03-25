var headerNavigationManager;

$(document).ready(function () {
    headerNavigationManager.init();
});

var HeaderNavigationManager = function () {
    var self = this;

    self.init = function () {
        self.bindEvents();
    };

    self.bindEvents = function () {
        $(".icon-logout").click(function () {
            indexPageManager.signOut();
        });
        $('.icon-collapse').click(function () {
            self.collapseLeftPanel($(this));
        });
        $(".icon-home").click(function () {
            indexPageManager.changeMenu("/" + top.baseOldMainApp + '_Reports/PortalPage/PortalPage.aspx');
        });
        $(".icon-swap-horizontal").click(function () {
            top.location = "/" + top.baseOldMainApp + "Index.aspx";
        });
        $(".old-view-text").click(function () {
            top.location = "/" + top.baseOldMainApp + "Index.aspx";
        });
    };
    self.collapseLeftPanel = function (collapseButton) {
        var sidebar = $('.sidebar');
        var content = $('.content-wrapper');

        var isCollapse = sidebar.hasClass('collapsed');

        var activeSubMenu = $('.menu-item.active .sub-menu');
        if (isCollapse) {
            activeSubMenu.css('display', 'block');
            sidebar.removeClass('collapsed');
            content.removeClass('expanded');
            $("#logoProfileImg").removeClass("collapse-image");
        } else {
            activeSubMenu.css('display', 'none');
            sidebar.addClass('collapsed');
            content.addClass('expanded');
            $("#logoProfileImg").addClass("collapse-image");
        }
    };
    self.closeListSetting = function () {
        self.activeSettingList();
    };
    self.activeSettingList = function () {
        $('.top-right-menu').parent().toggleClass("active");
    };
};

var headerNavigationManager = new HeaderNavigationManager();