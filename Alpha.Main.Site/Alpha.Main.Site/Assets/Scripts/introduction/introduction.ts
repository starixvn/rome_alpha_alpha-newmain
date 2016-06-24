/// <reference path="../home/menu-bar.ts" />


declare var EnjoyHint: any;

var introduction = {
    enjoyHint: null,
    readIntroductionCookieName: 'Introduction',

    domElements: {
        beginModal: null,
        startIntroductionBtn: null,
        notNowIntroductionBtn: null,
        markInner: null,
        replayInstruction: null
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
        Skip: (<HTMLInputElement>document.getElementById('Skip')).value,
        Next: (<HTMLInputElement>document.getElementById('Next')).value,
        Replay: (<HTMLInputElement>document.getElementById('Replay')).value,
        GotIt: (<HTMLInputElement>document.getElementById('GotIt')).value,
        ShowMainMenu: (<HTMLInputElement>document.getElementById('ShowMainMenu')).value
    },

    init: function () {
        if (!alpha.user.isShowIntroduction) {
            return;
        }

        var isReadIntroduction = $.cookie(introduction.readIntroductionCookieName);

        if (isReadIntroduction) {
            //return;
        }

        introduction.cacheDOMElements();
        introduction.registerEvents();
    },

    cacheDOMElements: function () {
        introduction.domElements.beginModal = $('#beginModal');
        introduction.domElements.startIntroductionBtn = $('#start-introduction-btn');
        introduction.domElements.markInner = $(".mask-inner");
        introduction.domElements.replayInstruction = $('#replay-instruction');
    },

    buildIntroductionStep: function () {
        var isSiteBarCollapsed = menuBar.isSideBarCollapsed();
        var fullSteps = [
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
                "next #menu-TRANSFER": '<p>' + introduction.resources.TransferMenuIntroduction + '</p>',
                permission: '[C]'
            },
            {
                "next #menu-VIEWLOG": '<p>' + introduction.resources.ViewLogMenuIntroduction + '</p>',
                permission: '[G]'
            },            
            {
                "click #hamburger-icon": '<p class="title">' + introduction.resources.CollapseExpandIcon + '</p>' +
                '<p class="blinker">' + introduction.resources.CollapseExpandIconIntroduction + '</p>',
                shape: 'circle',
                radius: 25,
                showSkip: false
            },            
            {
                'next #home-icon': '<p>' + introduction.resources.HomeIconIntroduction + '</p>',
                shape: 'circle',
                radius: 20,
                onBeforeStart: function () {
                    briefProfile.hideAllTooltips();
                }
            }, 
            {
                "next #btn-header-menu": '<p class="title">' + introduction.resources.QuickMenu + '</p>' +
                '<p>' + introduction.resources.QuickMenuIntroduction + '</p>' +
                '<p><img class="quick-menu-introduction-image" src="Assets/styles/introduction/images/' + alpha.user.level + '.png" /></p>',
                shape: 'circle',
                radius: 20,
                moveUp: 30
            },
            {
                "next #old-view-link": '<p class="title">' + introduction.resources.SwitchView + '</p>' +
                '<p>' + introduction.resources.BackOldViewIntroduction + '</p>' +
                '<p>' + introduction.resources.WarningOldVersionDeadline + ' DD/MM/YYYY.</p>'
                
            },
            {
                "next .icon-time": '<p class="title">Replay?</p>' +
                '<p>You can replay this tour by clicking on this button</p>',
                shape: 'circle',
                radius: 20,                
                nextButton: { className: "myNext", text: introduction.resources.GotIt },
                skipButton: { className: "replay-introduction", text: introduction.resources.Replay }
            }
        ];
        
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

                if (!step.nextButton) {
                    step.nextButton = { text: introduction.resources.Next }
                }

                if (!step.skipButton) {
                    step.skipButton = { text: introduction.resources.Skip }
                }

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
        introduction.show(false);
        introduction.domElements.startIntroductionBtn.click(function () {
            introduction.domElements.beginModal.modal('hide');
            introduction.setReadIntroduction();
            introduction.enjoyHint.run();
        });

        $(document)
            .off('click.replay-introduction', '.replay-introduction')
            .on('click.replay-introduction', '.replay-introduction', function () {
                introduction.show(true);
            });

        introduction.domElements.replayInstruction
            .off('click.replay-introduction')
            .on('click.replay-introduction', function () {
                introduction.show(true);
            });
    },

    show: function (isShowInstructionImmediately) {
        introduction.buildIntroductionStep();
        introduction.showDelayFirstIntroduction();
        if (isShowInstructionImmediately) {
            introduction.enjoyHint.run();
        }
        else {
            introduction.domElements.beginModal.modal('show');
            $('.modal-backdrop').css('z-index', 1051);
        }
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