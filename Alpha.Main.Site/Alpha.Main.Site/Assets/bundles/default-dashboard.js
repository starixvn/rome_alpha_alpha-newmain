var alpha = {
    urls: {
        oldMainRootPath: top.document.getElementById('oldMainRootPath') != null
            ? top.document.getElementById('oldMainRootPath').value
            : "not found",
        mainRootPath: top.document.getElementById('mainRootPath') != null
            ? top.document.getElementById('mainRootPath').value
            : "not found",
        accountInfoRootPath: top.document.getElementById('accountInfoRootPath') != null
            ? top.document.getElementById('accountInfoRootPath').value
            : "not found",
        memberInfoRootPath: top.document.getElementById('memberInfoRootPath') != null
            ? top.document.getElementById('memberInfoRootPath').value
            : "not found",
        howToUseRootPath: top.document.getElementById('howToUseRootPath') != null
            ? top.document.getElementById('howToUseRootPath').value
            : "not found",
        exTotalBetsForecastRootPath: top.document.getElementById('exTotalBetsForecastRootPath') != null
            ? top.document.getElementById('exTotalBetsForecastRootPath').value
            : "not found",
        messageRootPath: top.document.getElementById('messageRootPath') != null
            ? top.document.getElementById('messageRootPath').value
            : "not found"
    },
    user: {
        onUserId: document.getElementById('onUserId') != null
            ? document.getElementById('onUserId').value
            : "not found",
        passwordHash: document.getElementById('passwordHash') != null
            ? document.getElementById('passwordHash').value
            : "not found",
        subPageAccess: document.getElementById('subPageAccess') != null
            ? document.getElementById('subPageAccess').value
            : "not found",
        level: document.getElementById('userLevel') != null
            ? document.getElementById('userLevel').value
            : "not found",
        isSubAccount: document.getElementById('isSubAccount') != null
            ? (document.getElementById('isSubAccount').value.toLowerCase() === "true")
            : "not found",
        isShowIntroduction: document.getElementById('isShowIntroduction') != null
            ? (document.getElementById('isShowIntroduction').value.toLowerCase() === "true")
            : "not found",
        isEnableOtp: document.getElementById('isEnabledOtp') != null
            ? (document.getElementById('isEnabledOtp').value.toLowerCase() === "true")
            : false
    },
    core: {
        convertReponseToJson: function (reponse) {
            return eval("(" + reponse + ")");
        },
        signOut: function () {
            var index = window;
            while (index != index.parent) {
                index = index.parent;
            }
            var isInternal = index.$("#isInternal").val();
            var signOutUrl = alpha.urls.oldMainRootPath + '_Authorization/Handlers/SignOut.ashx';
            if (isInternal.toString() !== "0") {
                $.ajax(signOutUrl, {
                    success: function () {
                        index.close();
                    }
                });
            }
            else {
                index.location = signOutUrl;
            }
        },
        isIe: function () {
            return /(MSIE|Trident\/|Edge\/)/i.test(navigator.userAgent);
        },
        IsIe8OrLower: function () {
            var userAgent = window.navigator.userAgent.toLowerCase();
            return (userAgent.indexOf('msie') != -1)
                ? (parseInt(userAgent.split('msie')[1]) < 9)
                : false;
        },
        IsAndroid: function () {
            return /Android/i.test(navigator.userAgent);
        }
    },
    isScreenGreater1024: false,
    isScreenLower768: false,
    isScreenGreater768AndLower1024: false,
    isScreenLower1024: false,
    isMobile: false,
    detectResponsiveInformation: function () {
        if (alpha.core.isIe()) {
            //// Work arround because in IE, bootstrap modal dont delegate focusin event to others.
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        }
        var currentWindowWidth = window.screen.width;
        if (currentWindowWidth < 768) {
            alpha.isScreenLower768 = true;
        }
        else if (currentWindowWidth <= 1024) {
            alpha.isScreenGreater768AndLower1024 = true;
        }
        else {
            alpha.isScreenGreater1024 = true;
        }
        alpha.isMobile = isMobile.any;
    },
    localStorage: {
        get: function (key) {
            return store.enabled ? store.get(key) : $.cookie(key);
        },
        set: function (key, value) {
            store.enabled ? store.set(key, value) : $.cookie(key, value, alpha.localStorage.getCookieExpireOptions());
        },
        check: function (key) {
            return typeof store.get(key) !== 'undefined' || typeof $.cookie(key) !== 'undefined';
        },
        getCookieExpireOptions: function () {
            var cookieExpire = { expires: 365, path: window.location.pathname };
            if (alpha.core.isIe()) {
                cookieExpire = { expires: 365 };
            }
            return cookieExpire;
        },
    },
    showMessage: function ($target, result) {
        $target.empty();
        if (result.isSuccessful) {
            $target.html("<div class='msg msg-success'>" + result.message + "</div>");
            return;
        }
        if (result.isWarning) {
            $target.html("<div class='msg msg-warning'>" + result.message + "</div>");
            return;
        }
        if (result.isFailed) {
            $target.html("<div class='msg msg-error'>" + result.message + "</div>");
            return;
        }
    },
    key: {
        instruction: 'Instruction',
        mobileInstruction: 'MobileInstruction'
    },
    messageType: {
        Normal: 0,
        Personal: 1,
        Special: 2,
        System: 3
    }
};
$(function () {
    $.ajaxSetup({
        beforeSend: function (xhr) {
            var antiXSRFToken = document.getElementsByName("__RequestVerificationToken");
            if (antiXSRFToken.length > 0) {
                xhr.setRequestHeader("__RequestVerificationToken", antiXSRFToken[0].value);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            if (xhr.status === 401) {
                alert('Oops... you are here because of a system error, please notify the administrators.');
                alpha.core.signOut();
            }
        }
    });
});
var Result = /** @class */ (function () {
    function Result(data) {
        this.code = data.Code;
        this.message = data.Message || "";
        this.isFailed = this.code !== 0;
        this.isSuccessful = this.code === 0;
        this.isWarning = data.Code === Result.warningCode;
    }
    Result.warningCode = -1000;
    return Result;
}());
//# sourceMappingURL=core.js.map
var DashboardBase = /** @class */ (function () {
    function DashboardBase($) {
        var _this = this;
        this.loadInfo = function (id, handleOnSuccessLoading) {
            var $component = _this.$(id), url = $component.attr('action-url');
            if (url !== '' && typeof url !== 'undefined') {
                var ajaxSettings = _this.createAjaxSettings(url, 'GET', $component, handleOnSuccessLoading);
                _this.$.ajax(ajaxSettings);
            }
        };
        this.createAjaxSettings = function (url, method, component, handleOnSuccessLoading) {
            var ajaxSetting = {
                method: method,
                url: url,
                cache: false,
                success: function (responseHtml) {
                    component.html(responseHtml);
                    handleOnSuccessLoading();
                }
            };
            return ajaxSetting;
        };
        this.$ = $;
    }
    return DashboardBase;
}());
//# sourceMappingURL=dashboard.js.map
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Statistics = /** @class */ (function (_super) {
    __extends(Statistics, _super);
    function Statistics() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.registerEvents = function () {
            _this.onToggleDropdown('#listTopWinningMembers', _this.loadDropdownInfo);
            _this.onToggleDropdown('#listTopTurnoverMembers', _this.loadDropdownInfo);
            _this.onToggleDropdown('#listNewMembers', _this.loadDropdownInfo);
            _this.registerTooltip();
        };
        _this.onToggleDropdown = function (id, loadInfo) {
            var $this = _this.$(id), dataElement = $this.next(), url = $this.attr('action-url');
            $this.on('click', function () {
                if (dataElement.is(':hidden')) {
                    loadInfo(url, dataElement);
                }
            });
        };
        _this.loadDropdownInfo = function (url, dataElement) {
            if (url !== '' && typeof url !== 'undefined') {
                _this.$.ajax({
                    method: 'GET',
                    url: url,
                    cache: false,
                    success: function (responseHtml) { return dataElement.html(responseHtml); }
                });
            }
        };
        _this.registerTooltip = function () {
            _this.$('[data-toggle="tooltip"]').tooltip({ html: true });
            _this.hideTooltipOnTouchOutInMobile();
        };
        _this.hideTooltipOnTouchOutInMobile = function () {
            var $tooltip = _this.$('[data-toggle="tooltip"]');
            _this.$(window.document).on('touchend', function (event) {
                var target = $(event.target);
                $tooltip.tooltip('hide');
                if (target.attr('data-toggle') === 'tooltip') {
                    target.tooltip('show');
                }
                else if (target.parent().attr('data-toggle') === 'tooltip') {
                    target.parent().tooltip('show');
                }
            });
        };
        return _this;
    }
    return Statistics;
}(DashboardBase));
$(function () {
    var stats = new Statistics($);
    stats.loadInfo('#stats', stats.registerEvents);
});
//# sourceMappingURL=statistics.js.map
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var NoticeMessage = /** @class */ (function (_super) {
    __extends(NoticeMessage, _super);
    function NoticeMessage() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.registerEvents = function () {
            _this.onToggleMessage(_this.markMessageAsRead);
            _this.onToggleShowMoreMessages();
            _this.hideMoreBtnIfHaveNoData();
        };
        _this.onToggleMessage = function (markMessageAsRead) {
            $('.message').each(function (index, element) {
                var $this = _this.$(element), shortContent = $this.children('.short-content'), fullContent = $this.children('.full-content'), icon = $this.find('.icon');
                $this.on('click', function () {
                    if (!shortContent.is(':hidden')) {
                        shortContent.hide();
                        fullContent.show();
                        icon.toggleClass('icon-plus icon-minus');
                        markMessageAsRead($this);
                    }
                    else {
                        shortContent.show();
                        fullContent.hide();
                        icon.toggleClass('icon-minus icon-plus');
                    }
                });
            });
        };
        _this.markMessageAsRead = function (message) {
            var isUnRead = message.hasClass('bold');
            if (isUnRead) {
                var onSuccess = function (isMarkedSuccess) {
                    if (isMarkedSuccess) {
                        message.removeClass('bold');
                    }
                };
                var ajaxSetting = {
                    url: alpha.urls.messageRootPath + 'PortalMessage/MarkMessageAsRead',
                    data: { messageId: message.attr('id') },
                    method: 'POST',
                    success: onSuccess
                };
                _this.$.ajax(ajaxSetting);
            }
        };
        _this.onToggleShowMoreMessages = function () {
            var moreMessagesBtn = _this.$('.more-info');
            moreMessagesBtn.on('click', function () {
                var content = _this.$('#notice');
                var onSuccess = function (htmlString) {
                    content.html(htmlString);
                    _this.registerEvents();
                };
                var ajaxSetting = {
                    url: alpha.urls.messageRootPath + 'PortalMessage/MoreMessages?totalMessage=' + moreMessagesBtn.attr('totalMsg'),
                    method: 'GET',
                    cache: false,
                    success: onSuccess
                };
                moreMessagesBtn.attr('disabled', 'disabled');
                moreMessagesBtn.html('<span class="block-icon-small"></span>');
                _this.$.ajax(ajaxSetting);
            });
        };
        _this.hideMoreBtnIfHaveNoData = function () {
            var noInfo = document.getElementById('no-info'), messageAmount = _this.$('.msg-contents').length, moreTotalMessage = Number($('.more-info').attr('totalMsg'));
            if (noInfo !== null || moreTotalMessage > (messageAmount + 10)) {
                _this.$('.more-info').hide();
            }
        };
        return _this;
    }
    return NoticeMessage;
}(DashboardBase));
$(function () {
    var notice = new NoticeMessage($);
    notice.loadInfo('#notice', notice.registerEvents);
});
//# sourceMappingURL=notice.js.map
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var MemberRequests = /** @class */ (function (_super) {
    __extends(MemberRequests, _super);
    function MemberRequests($, topWindow) {
        var _this = _super.call(this, $) || this;
        _this.init = function () {
            var totalRequest = _this.$('#totalRequest').val();
            _this.$('.new-request').html("(" + totalRequest + ")");
            _this.registerEvents();
        };
        _this.registerEvents = function () {
            var self = _this;
            _this.$('#memberrequest-tab').on('click', function () {
                var position = self.$(this).position();
                self.$(".scroll-tabs").scrollLeft(position.left);
            });
            _this.$('.icon-information-outline').on('click', _this.showGuideline);
        };
        _this.showGuideline = function () {
            var title = _this.$('#PlayForFunGuide').val();
            var url = _this.$('#GuidelineUrl').val();
            _this.topWindow.popupManager.open(url, title, 900, 600, "popup-guidelines");
        };
        _this.topWindow = topWindow;
        return _this;
    }
    return MemberRequests;
}(DashboardBase));
$(function () {
    var memberRequests = new MemberRequests($, window.top);
    memberRequests.loadInfo('#member-requests', memberRequests.init);
});
//# sourceMappingURL=member-requests.js.map