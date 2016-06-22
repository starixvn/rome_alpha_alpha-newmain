/// <reference path="../home/menu-bar.ts" />


declare var EnjoyHint: any;

var introduction = {
    enjoyHint: null,
    readIntroductionCookieName: 'Introduction',

    domElements: {
        beginModal: null,
        startIntroductionBtn: null,
        notNowIntroductionBtn: null,
        markInner: null
    },

    resources: {
        HomeIconIntroduction: (<HTMLInputElement>document.getElementById('HomeIconIntroduction')).value,
        TransferAndViewLogIntroduction: (<HTMLInputElement>document.getElementById('TransferAndViewLogIntroduction')).value,
        QuickMenuIntroduction: (<HTMLInputElement>document.getElementById('QuickMenuIntroduction')).value,
        QuickMenu: (<HTMLInputElement>document.getElementById('QuickMenu')).value,
        SwitchView: (<HTMLInputElement>document.getElementById('SwitchView')).value,
        BackOldViewIntroduction: (<HTMLInputElement>document.getElementById('BackOldViewIntroduction')).value,
        WarningOldVersionDeadline: (<HTMLInputElement>document.getElementById('WarningOldVersionDeadline')).value,
        MainMenuIntroduction: (<HTMLInputElement>document.getElementById('MainMenuIntroduction')).value,
        ReportMenuIntroduction: (<HTMLInputElement>document.getElementById('ReportMenuIntroduction')).value,
        WinLossAnalysisChartIntroduction: (<HTMLInputElement>document.getElementById('WinLossAnalysisChartIntroduction')).value,
        CollapseExpandIcon: (<HTMLInputElement>document.getElementById('CollapseExpandIcon')).value,
        CollapseExpandIconIntroduction: (<HTMLInputElement>document.getElementById('CollapseExpandIconIntroduction')).value,
        MainMenu: (<HTMLInputElement>document.getElementById('MainMenu')).value,
        TransferMenuIntroduction: (<HTMLInputElement>document.getElementById('TransferMenuIntroduction')).value,
        ViewLogMenuIntroduction: (<HTMLInputElement>document.getElementById('ViewLogMenuIntroduction')).value,
        ShowMainMenu: (<HTMLInputElement>document.getElementById('ShowMainMenu')).value
    },

    init: function () {
        if (!alpha.user.isShowIntroduction) {
            return;
        }

        var isReadIntroduction = $.cookie(introduction.readIntroductionCookieName);

        if (isReadIntroduction) {
            return;
        }

        introduction.cacheDOMElements();
        introduction.registerEvents();
    },

    cacheDOMElements: function () {
        introduction.domElements.beginModal = $('#beginModal');
        introduction.domElements.startIntroductionBtn = $('#start-introduction-btn');
        introduction.domElements.markInner = $(".mask-inner");
    },

    buildIntroductionStep: function () {
        var isSiteBarCollapsed = menuBar.isSideBarCollapsed();
        var fullSteps = [
            {
                'next #home-icon': '<p>' + introduction.resources.HomeIconIntroduction + '</p>',
                shape: 'circle',
                radius: 20,
                onBeforeStart: function () {
                    briefProfile.hideAllTooltips();
                }
            },
            {
                "next #main-menu-items": '<p class="title">' + introduction.resources.MainMenu + '</p>' +
                '<p>' + introduction.resources.MainMenuIntroduction + '</p>',
                onBeforeStart: function () {
                    if (isSiteBarCollapsed) {
                        menuBar.domElements.hamburgerIcon.trigger('click');
                    }
                }
            },
            {
                "next #menu-TRANSFER": '<p>' + introduction.resources.TransferMenuIntroduction + '</p>' +
                '<img src="Assets/styles/introduction/images/OldUI-2-1.png" /><br><br>',
                permission: '[C]'
            },
            {
                "next #menu-VIEWLOG": '<p>' + introduction.resources.ViewLogMenuIntroduction + '</p>' +
                '<img src="Assets/styles/introduction/images/OldUI-2-2.png" /><br><br>',
                permission: '[G]'
            },
            {
                "click #hamburger-icon": '<p class="blinker">' + introduction.resources.CollapseExpandIconIntroduction + '</p>',
                shape: 'circle',
                radius: 25,
                showSkip: false
            },
            {
                "next #side-bar": '<p class="title">' + introduction.resources.CollapseExpandIcon + '</p>' +
                '<p>' + introduction.resources.ShowMainMenu + '</p>'
            },           
            {
                "next #btn-header-menu": '<p class="title">' + introduction.resources.QuickMenu + '</p>' +
                '<p>' + introduction.resources.QuickMenuIntroduction + '</p>' +
                '<p><img class="quick-menu-introduction-image" src="Assets/styles/introduction/images/' + alpha.user.level + '.png" /></p>',
                shape: 'circle',
                radius: 20
            },
            {
                "next #old-view-link": '<p class="title">' + introduction.resources.SwitchView + '</p>' +
                '<p>' + introduction.resources.BackOldViewIntroduction + '</p>' +
                '<p>' + introduction.resources.WarningOldVersionDeadline + ' DD/MM/YYYY.</p>',
                'nextButton': { className: "myNext", text: "Got it" },
                'skipButton': { className: "replay-introduction", text: "Replay" }
            }];
        
        //[A]: Create
        //[B]: Update
        //[C]: Transfer
        //[D]: Total Bets
        //[E]: Reports
        //[F]: Forcast
        //[H]: Member Information
        //[G]: View Log
        introduction.introductionSteps = [];
        $.each(fullSteps, function (index, step) {
            if (typeof (step.permission) === undefined
                || step.permission == null
                || alpha.user.subPageAccess.indexOf(step.permission) > -1) {
                introduction.introductionSteps.push(step);

                return;
            }
        });

        introduction.enjoyHint = new EnjoyHint({});
        introduction.enjoyHint.set(introduction.introductionSteps);
    },

    introductionSteps: [],

    showDelayFirstIntroduction: function () {
        var $p = introduction.domElements.markInner.find("P");
        var words = $p.text().split(" ");
        var delay = 100;
        var $tempEl = $("<div>");

        $p.empty().show();

        $.each(words, function (i, word) {
            var $span = $("<span>").text(word + " ");
            $span.css({
                "transition-delay": i * delay + "ms"
            });
            $tempEl.append($span)
        });

        $p.html($tempEl.html());

        setTimeout(function () {
            introduction.domElements.markInner.addClass("in");
            setTimeout(function () {
                $p.addClass("in")
            }, 200);
        }, 200);
    },

    registerEvents: function () {
        introduction.show();
        introduction.domElements.startIntroductionBtn.click(function () {
            introduction.domElements.beginModal.modal('hide');
            introduction.setReadIntroduction();
            introduction.enjoyHint.run();
        });

        $(document)
            .off('click.replay-introduction', '.replay-introduction')
            .on('click.replay-introduction', '.replay-introduction', function () {
                introduction.show();
            });
    },

    show: function () {
        introduction.buildIntroductionStep();
        introduction.showDelayFirstIntroduction();
        introduction.domElements.beginModal.modal('show');
    },

    getCookieExpireOptions: function () {
        var cookieExpire: any = { expires: 365, path: window.location.pathname };

        if (alpha.core.isIe()) {
            cookieExpire = { expires: 365 };
        }

        return cookieExpire;
    },

    setReadIntroduction: function () {
        $.cookie(introduction.readIntroductionCookieName, true, introduction.getCookieExpireOptions());
    }
};

$(function () {
    introduction.init();
});