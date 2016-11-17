/// <reference path="../core.ts" />
/// <reference path="menu-bar.ts" />

'use strict';
/*global alpha, briefProfile */

var indexManager = {
    currentTab: null,
    currentUrlCookieName: 'DFFKDHFJKLDHFJDH',

    domElements: {
        mainFrame: null,
        partialContents: null,
        content: null,
        accountInfoContent: null,
        currentURLCookieName: null,
        pagePopup: null
    },

    urls: {
        reCheckinUrl: alpha.urls.oldMainRootPath + '_Authorization/Handlers/ReCheckin.ashx'
    },

    init: function () {
        alpha.detectResponsiveInformation();
        if (!alpha.core.IsIe8OrLower()) {
            $.material.init();
        }

        indexManager.cachedDomElements();
        indexManager.loadPartialViews();
        indexManager.registerEvents();
        indexManager.setCurrentUrl();
        indexManager.setTouchActionStyleToAllHtml();
    },

    registerEvents: function () {
        indexManager.domElements.pagePopup.tinyDraggable();
        setTimeout(indexManager.reCheckin, 30000);
    },

    cachedDomElements: function () {
        indexManager.domElements.partialContents = $('.partial-content');
        indexManager.domElements.content = $('#content');
        indexManager.domElements.accountInfoContent = $('#accountinfo-content');
        indexManager.domElements.currentURLCookieName = $('#currentURLCookieName');
        indexManager.domElements.pagePopup = $('#page-popup');
        indexManager.currentUrlCookieName = indexManager.domElements.currentURLCookieName.val();
    },

    changeMenu: function (url) {
        indexManager.domElements.mainFrame.attr('src', url);
    },

    reCheckin: function () {
        // Post with return result in JSON format
        $.cookie('ssl', location.href.indexOf('https://') === 0, { expires: 30, path: '/' });

        $.ajax({
            type: "POST",
            url: indexManager.urls.reCheckinUrl,
            data: { onid: alpha.user.onUserId },
            success: (result) => {
                try {
                    result = alpha.core.convertReponseToJson(result);
                    var errCode = result.errCode;
                    var errMsg = result.errMsg;
                    if (errCode !== 0) {
                        if (errMsg) {
                            alert(errMsg);
                            alpha.core.signOut();
                        } else {
                            alert('Oops... you are here because of a system error, please notify the administrators.');
                        }
                    }
                }
                catch (e) {
                    if (typeof (result) === "string"
                        && result.indexOf("Oops... you are here because of a system error") > 0) {
                        alpha.core.signOut();
                        alert('Oops... you are here because of a system error, please notify the administrators.');
                    }
                }
            },
            error: (xhr) => {
                // If internal server error
                if (xhr.status === 500) {
                    alpha.core.signOut();
                    alert('Oops... you are here because of a system error, please notify the administrators.');
                }
            },
            complete: () => {
                setTimeout(indexManager.reCheckin, 30000);
            }
        });
    },

    loadPartialViews: function () {
        indexManager.domElements.partialContents.each(function (index, partialContent) {
            var $partialContent = $(partialContent),
                url = $partialContent.data('url');
            if (url && url.length > 0) {
                $partialContent.load(url + '?' + Math.random(), function () {
                    if ($partialContent.hasClass('customer-profile')) {
                        briefProfile.init();
                    }
                });
            }
        });
    },

    cacheCurrentUrl: function () {
        var currentUrl = (<HTMLIFrameElement>document.getElementById("main")).contentWindow.location.href;

        $.cookie(indexManager.currentUrlCookieName, currentUrl, { expires: 1, path: '/' });
    },

    setCurrentUrl: function () {
        var currentUrl = indexManager.domElements.content.data('default-url');

        if (indexManager.domElements.mainFrame
            && indexManager.domElements.mainFrame.length > 0) {
            indexManager.domElements.mainFrame.attr('src', currentUrl);
        }
        else {
            indexManager.domElements.content.append('<iframe frameborder="0" id="main" src="' + currentUrl + '" width="100%" height="100%" scrolling="auto" name="main" marginwidth="0" marginheight="0" allowtransparency="true" class="iframe-content"></iframe>');
            indexManager.domElements.mainFrame = $('#main');
            indexManager.domElements.mainFrame
                .off('load.newmain')
                .on('load.newmain', function () {
                    var iframeWindow = indexManager.domElements.mainFrame[0].contentWindow || indexManager.domElements.mainFrame[0].contentDocument;
                    indexManager.closeAllOpenDropdowns(iframeWindow);

                    indexManager.cacheCurrentUrl();

                    $(iframeWindow.document)
                        .off('click.closeMenu touchstart.closemenu')
                        .on('click.closeMenu touchstart.closemenu',
                        function () {
                            menuBar.collapseSideBar();
                        });

                    if (alpha.isMobile) {
                        $(iframeWindow.document).find(':focus').blur();
                    }
                });

            indexManager.domElements.accountInfoContent.load(function () {
                var iframeWindow = indexManager.domElements.accountInfoContent[0].contentWindow ||
                    indexManager.domElements.accountInfoContent[0].contentDocument;
                indexManager.closeAllOpenDropdowns(iframeWindow);
            });
        }
    },

    closeAllOpenDropdowns: function (iframeWindow) {
        const closeDropdownSubMenu = () => {
            $('.dropdown-submenu').find('.dropdown-menu').hide();
        }

        $(document).click(closeDropdownSubMenu);

        $('#btn-header-menu').click(closeDropdownSubMenu);

        $(iframeWindow.document).click(() => {
            $('.dropdown-toggle').each((index, dropdown) => {
                var $dropdown = $(dropdown);

                if ($dropdown.parent().hasClass('open')) {
                    $dropdown.dropdown('toggle');
                }
            });

            closeDropdownSubMenu();
        });
    },

    setTouchActionStyleToAllHtml: () => {
        $('html').css('touch-action', 'manipulation');
        $('iframe').load(() => {
            $('iframe').contents().find('html').css('touch-action', 'manipulation');
        });
    },
};

$(function () {
    indexManager.init();
});