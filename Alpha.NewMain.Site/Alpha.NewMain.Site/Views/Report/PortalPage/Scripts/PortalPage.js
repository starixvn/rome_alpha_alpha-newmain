var dashboardManager;
var portalPageManager;

var PortalPageManager = function () {
    var self = this;
    var moreIsLoading = false;
    var firstLoadAnnouncements = true;
    self.init = function () {
        self.bindEvents();
    };
    self.bindEvents = function () {
        $(".announcement-title").click(function () {
            dashboardManager.changeMenu(dashboardManager.getMainSitePath() + 'Info/NewMessageForm.aspx?pageQuery=normal', this);
        });

        self.bindAnnouncementEvents();
    };

    self.bindAnnouncementEvents = function () {
        var toogleIcons = $(".announcement-content .toogleIcon");
        toogleIcons.unbind("click");
        toogleIcons.click(function () {
            var $this = $(this);
            var announcementId = $this.attr("data-announcementid");
            var isShowFullContent = $this.hasClass("showfullcontent-icon");
            var row = $(".announcement-" + announcementId);
            if (isShowFullContent) {
                $this.removeClass("showfullcontent-icon");
                $this.addClass("showbriefcontent-icon");
                row.find(".announcement-briefcontent").addClass("display-none");
                row.find(".announcement-fullcontent").removeClass("display-none");
            } else {
                $this.removeClass("showbriefcontent-icon");
                $this.addClass("showfullcontent-icon");
                row.find(".announcement-fullcontent").addClass("display-none");
                row.find(".announcement-briefcontent").removeClass("display-none");
            }
        });

        $(".announcement-content .viewmore-link").click(function () {
            var $this = $(this);
            moreIsLoading = true;
            $this.html("<img src='../Content/Report/PortalPage/Styles/Images/loading_2.gif' />");
            setTimeout(self.getMoreAnnouncements, 1000);
        });
    };

    self.getMoreAnnouncements = function () {
        var lastPostTime = getCookie("lastPostTime");
        var url = rootUrl + 'Report/GetMoreAnnouncements';
        if (firstLoadAnnouncements == true) {
            setCookie('lastPostTime', '');
            setCookie('moreIsVisible', '');
        }

        firstLoadAnnouncements = false;
        jQuery.ajax({
            type: "POST",
            url: url,
            data: {
                lastPostTime: lastPostTime
            },
            responseType: "json",
            error: function (jqXhr, event, ajaxSettings) {
            },
            success: function (data) {
                var html = '';
                for (var i = 0, announcementGroupLength = data.AnnouncementGroup.length; i < announcementGroupLength; i++) {
                    var announcementGroup = data.AnnouncementGroup[i];
                    html += '<div class="date-item">';

                    html += (announcementGroup.ShowTitle ? '<div class="date-announcement">' + announcementGroup.GroupId + '</div>' : '');
                    html += '<div class="date-list-row">\
                                <table class="date-row-item">\
                                    <tbody>';
                    for (var j = 0, announcementLength = announcementGroup.AnnouncementItems.length; j < announcementLength; j++) {
                        var announcement = announcementGroup.AnnouncementItems[j];
                        html += '<tr class="' + (isEvenRowAnnouncement ? "even" : "") + ' announcement-' + announcement.Id + '">';
                        html += '<td class="col-paddingleft"><div>&nbsp;</div></td>\
                                 <td class="col-posttime"><div>'+ announcement.PostTime + '</div></td>\
                                 <td class="col-message">\
                                    <div class="toogleIcon '+ (announcement.BriefContent == announcement.FullContent ? "" : "showfullcontent-icon") + '" data-announcementid="' + announcement.Id + '"></div>\
                                    <div class="col-message announcement-fullcontent display-none">'+ announcement.FullContent + '</div>\
                                    <div class="col-message announcement-briefcontent">'+ announcement.BriefContent + '</div>\
                                 </td>\
                            </tr>';

                        isEvenRowAnnouncement = !isEvenRowAnnouncement;
                    }

                    html += '</tbody></table></div></div>';
                }

                var viewMoreLink = $(".announcement-content .viewmore-link").remove();
                if (data.IsShowMoreLink) {
                    html += '<div class="viewmore-link">' + resources.more + '...</div>';
                }

                $(".announcement-content").append(html);
                self.bindAnnouncementEvents();
                moreIsLoading = false;
            }
        });
    };
};

portalPageManager = new PortalPageManager();

$(document).ready(function () {
    dashboardManager = parent.dashboardManager;
    portalPageManager.init();
});