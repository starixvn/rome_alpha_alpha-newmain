var statisticsManager;

var Statistics = function () {
    var self = this;
    var timerId = null;
    self.init = function () {
        self.bindEvents();
    };
    self.bindEvents = function () {
        $(".total-activeclosedsuspened").click(function () {
            self.closeOthersDetail("total-activeclosedsuspened-detail");
            self.showDetail($(".total-activeclosedsuspened-detail"), false);
        });

        $(".top10-winningmembers").click(function () {
            self.closeOthersDetail("top10-winningmembers-detail");
            self.showDetail($(".top10-winningmembers-detail"), false, self.loadTop10WinningOfMonth);
        });

        $(".highestturnover").click(function () {
            self.closeOthersDetail("highestturnover-detail");
            self.showDetail($(".highestturnover-detail"), false, self.loadHighestTurnoverOfMonth);
        });

        $(".newmember").click(function () {
            self.closeOthersDetail("newmember-detail");
            self.showDetail($(".newmember-detail"), false, self.loadNewMembers);
        });

        $(".div-detail").hover(
            function () {
                clearTimeout(timerId);
                timerId = null;
            },
            function () {
                var $this = $(this);
                timerId = setTimeout(function () {
                    self.showDetail($this, true);
                }, 1000000);
            });
    };
    self.iconViewDetailClick = function (icon) {
        var $this = $(icon);
        var isExpand = $this.hasClass("expand");
        if (isExpand) {
            $this.find(".viewdetail-icon").attr("src", "../Content/Images/collapse.png");
            $this.removeClass("detail-open");
            $this.removeClass("expand");
        } else {
            $this.find(".viewdetail-icon").attr("src", "../Content/Images/expand.png");
            $this.addClass("detail-open");
            $this.addClass("expand");
        }
    };
    self.showDetail = function (detailDiv, forceHide, loadDataFunction) {
        var $this = detailDiv;
        var forceHide = forceHide || !$this.hasClass("display-none");
        var target = $this.attr("data-target");
        var icon = $("." + target);
        if (forceHide) {
            $this.addClass("display-none");
            icon.find(".viewdetail-icon").attr("src", "../Content/Images/collapse.png");
            icon.removeClass("detail-open");
            icon.removeClass("expand");
        } else {
            if (loadDataFunction && typeof (loadDataFunction) == "function") {
                icon.find(".viewdetail-icon").attr("src", "../Content/Images/loading.gif");

                loadDataFunction(function () {
                    $this.removeClass("display-none");
                    icon.find(".viewdetail-icon").attr("src", "../Content/Images/expand.png");
                    icon.addClass("detail-open");
                    icon.addClass("expand");
                });
            } else {
                $this.removeClass("display-none");
                icon.find(".viewdetail-icon").attr("src", "../Content/Images/expand.png");
                icon.addClass("detail-open");
                icon.addClass("expand");
            }
        }
    };
    self.closeOthersDetail = function (currentDiv) {
        var divIds = ["total-activeclosedsuspened-detail", "top10-winningmembers-detail",
            "highestturnover-detail", "newmember-detail"];
        for (var i = 0, divLength = divIds.length; i < divLength; i++) {
            var divId = divIds[i];
            if (currentDiv != divId) {
                self.showDetail($("." + divId), true);
            }
        }
    };
    self.loadTop10WinningOfMonth = function (completeFunction) {
        var url = rootUrl + 'Statistics/Top10WinningOfMonth';
        $.ajax({
            type: "POST",
            url: url,
            data: {},
            responseType: "json",
            error: function (jqXhr, event, ajaxSettings) {
            },
            success: function (data) {
                var customerLength = data.CustomerInfo ? data.CustomerInfo.length : 0;
                var html = '<table class="table-detail">\
                                <tr class="rowHeader">\
                                    <th class="rowHeader">'+ resources.username + '</th>\
                                    <th class="rowHeader">' + resources.winningAmount + '</th>\
                                </tr>';
                if (customerLength) {
                    for (var i = 0; i < customerLength; i++) {
                        var customerInfo = data.CustomerInfo[i];
                        var isEvenRow = (i + 1) % 2;
                        html += '<tr class="' + (isEvenRow ? "even-row" : "odd-row") + '">\
                                    <td class="align-left">'+ customerInfo.Username + '</td>\
                                    <td class="align-right">' + customerInfo.WinLoss + '</td>\
                            </tr>';
                    }
                } else {
                    html += '<tr class="odd-row"><td colspan="2" class="align-center bold">' + resources.noInfo + '</td></tr>';
                }

                html += '</table>';
                $(".top10-winningmembers-detail").html(html);
                if (completeFunction) {
                    completeFunction();
                }
            }
        });
    };
    self.loadHighestTurnoverOfMonth = function (completeFunction) {
        var url = rootUrl + 'Statistics/HighestTurnoverOfMonth';
        $.ajax({
            type: "POST",
            url: url,
            data: {},
            responseType: "json",
            error: function (jqXhr, event, ajaxSettings) {
            },
            success: function (data) {
                var customerLength = data.CustomerInfo ? data.CustomerInfo.length : 0;
                var html = '<table class="table-detail">\
                                <tr class="rowHeader">\
                                    <th class="rowHeader">'+ resources.username + '</th>\
                                    <th class="rowHeader">' + resources.turnover + '</th>\
                                </tr>';
                if (customerLength) {
                    for (var i = 0; i < customerLength; i++) {
                        var customerInfo = data.CustomerInfo[i];
                        var isEvenRow = (i + 1) % 2;
                        html += '<tr class="' + (isEvenRow ? "even-row" : "odd-row") + '">\
                                    <td class="align-left">'+ customerInfo.Username + '</td>\
                                    <td class="align-right">' + customerInfo.Turnover + '</td>\
                            </tr>';
                    }
                } else {
                    html += '<tr class="odd-row"><td colspan="2" class="align-center bold">' + resources.noInfo + '</td></tr>';
                }

                html += '</table>';
                $(".highestturnover-detail").html(html);
                if (completeFunction) {
                    completeFunction();
                }
            }
        });
    };
    self.loadNewMembers = function (completeFunction) {
        var url = rootUrl + 'Statistics/NewMembersOfMonth';
        $.ajax({
            type: "POST",
            url: url,
            data: {},
            responseType: "json",
            error: function (jqXhr, event, ajaxSettings) {
            },
            success: function (data) {
                var customerLength = data.CustomerInfo ? data.CustomerInfo.length : 0;
                var html = '<table class="table-detail">\
                                <tr class="rowHeader">\
                                    <th class="rowHeader">'+ resources.username + '</th>\
                                    <th class="rowHeader">' + resources.createdDate + '</th>\
                                    <th class="rowHeader">' + resources.givenCredit + '</th>\
                                </tr>';
                if (customerLength) {
                    for (var i = 0, customerLength = data.CustomerInfo.length; i < customerLength; i++) {
                        var customerInfo = data.CustomerInfo[i];
                        var isEvenRow = (i + 1) % 2;
                        html += '<tr class="' + (isEvenRow ? "even-row" : "odd-row") + '">\
                                    <td class="align-left">'+ customerInfo.Username + '</td>\
                                    <td class="align-right">' + customerInfo.GivenCredit + '</td>\
                                    <td class="align-right">' + customerInfo.CreatedDate + '</td>\
                            </tr>';
                    }
                } else {
                    html += '<tr class="odd-row"><td colspan="2" class="align-center bold">' + resources.noInfo + '</td></tr>';
                }

                html += '</table>';
                $(".newmember-detail").html(html);
                if (completeFunction) {
                    completeFunction();
                }
            }
        });
    };
};

statisticsManager = new Statistics();
$(document).ready(function () {
    statisticsManager.init();
});