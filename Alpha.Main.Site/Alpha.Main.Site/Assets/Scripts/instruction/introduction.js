/// <reference path="../home/menu-bar.ts" />
/// <reference path="./mobile-introduction.ts" />
var introduction = {
    enjoyHint: null,
    readIntroductionKey: 'Instruction',
    isCookieLess: document.getElementById('IsCookieLess').value === "1",
    mobileIntroduction: null,
    domElements: {
        beginModal: null,
        startIntroductionBtn: null,
        notNowIntroductionBtn: null,
        markInner: null,
        replayInstruction: null,
        modalInstructionBody: null,
        oldViewLink: null
    },
    resources: {
        HomeIconIntroduction: document.getElementById('HomeIconIntroduction').value,
        TransferAndViewLogIntroduction: document.getElementById('TransferAndViewLogIntroduction').value,
        QuickMenuIntroduction: document.getElementById('QuickMenuIntroduction').value,
        QuickMenu: document.getElementById('QuickMenu').value,
        SwitchView: document.getElementById('SwitchView').value,
        BackOldViewIntroduction: document.getElementById('BackOldViewIntroduction').value,
        WarningOldVersionDeadline: document.getElementById('WarningOldVersionDeadline').value,
        MainMenuIntroduction: document.getElementById('MainMenuIntroduction').value,
        ReportMenuIntroduction: document.getElementById('ReportMenuIntroduction').value,
        WinLossAnalysisChartIntroduction: document.getElementById('WinLossAnalysisChartIntroduction').value,
        CollapseExpandIcon: document.getElementById('CollapseExpandIcon').value,
        CollapseExpandIconIntroduction: document.getElementById('CollapseExpandIconIntroduction').value,
        MainMenu: document.getElementById('MainMenu').value,
        TransferMenuIntroduction: document.getElementById('TransferMenuIntroduction').value,
        ViewLogMenuIntroduction: document.getElementById('ViewLogMenuIntroduction').value,
        Skip: document.getElementById('Skip').value,
        Next: document.getElementById('Next').value,
        Replay: document.getElementById('Replay').value,
        GotIt: document.getElementById('GotIt').value,
        ShowMainMenu: document.getElementById('ShowMainMenu').value,
        ReplayInstruction: document.getElementById('ReplayInstruction').value,
        TakeTourInstruction: document.getElementById('TakeTourInstruction').value
    },
    init: function () {
        if (!alpha.isScreenLower768) {
            var isReadIntroduction = alpha.localStorage.get(introduction.readIntroductionKey);
            introduction.cacheDOMElements();
            introduction.registerEvents();
            if (isReadIntroduction) {
                return;
            }
            if (!introduction.isCookieLess && !isReadIntroduction) {
                introduction.domElements.beginModal.modal('show');
                $('.modal-backdrop').css('z-index', 1051);
                introduction.showDelayFirstIntroduction();
            }
        }
        else if (alpha.isScreenLower768) {
            introduction.mobileIntroduction = new Introduction.MobileIntroduction($);
            if (!introduction.mobileIntroduction.readIntroduction()) {
                introduction.mobileIntroduction.showIntroduction();
            }
        }
    },
    cacheDOMElements: function () {
        introduction.domElements.beginModal = $('#beginModal');
        introduction.domElements.startIntroductionBtn = $('#start-introduction-btn');
        introduction.domElements.markInner = $("#mask-inner");
        introduction.domElements.replayInstruction = $('#replay-instruction');
        introduction.domElements.notNowIntroductionBtn = $("#not-now-introduction-btn");
        introduction.domElements.modalInstructionBody = $('#instruction-modal-body');
        introduction.domElements.oldViewLink = $('#old-view-link a');
    },
    showDelayFirstIntroduction: function () {
        var $p = introduction.domElements.markInner.find("p");
        var words = $p.text().split(" ");
        var delay = 100;
        var $tempEl = $("<div>");
        $p.empty().show();
        $.each(words, function (i, word) {
            var $span = $("<span>").text(word + " ");
            $span.css({
                "transition-delay": i * delay + "ms"
            });
            $tempEl.append($span);
        });
        $p.html($tempEl.html());
        setTimeout(function () {
            introduction.domElements.markInner.addClass("in");
            setTimeout(function () {
                $p.addClass("in");
            }, 250);
        }, 250);
    },
    registerEvents: function () {
        introduction.domElements.startIntroductionBtn
            .off('click.introduction')
            .on('click.introduction', function () {
            introduction.domElements.beginModal.modal('hide');
            introduction.setReadIntroduction();
            introduction.showMainSteps();
        });
        introduction.domElements.notNowIntroductionBtn
            .off('click.introduction')
            .on('click.introduction', function () {
            introduction.domElements.beginModal.modal('hide');
            introduction.setReadIntroduction();
            introduction.showNotNowSteps();
        });
        $(document)
            .off('click.replay-introduction', '.replay-introduction')
            .on('click.replay-introduction', '.replay-introduction', introduction.showMainSteps);
        introduction.domElements.replayInstruction
            .off('click.replay-introduction')
            .on('click.replay-introduction', introduction.showMainSteps);
    },
    showNotNowSteps: function () {
        introduction.enjoyHint = new EnjoyHint({});
        introduction.enjoyHint.set([{
                "next #replay-instruction": '<p>' + introduction.resources.TakeTourInstruction + '</p>',
                nextButton: { className: "myNext", text: introduction.resources.GotIt },
                showSkip: false,
                shape: 'circle',
                radius: 20
            }]);
        introduction.enjoyHint.run();
    },
    showMainSteps: function () {
        var isSiteBarCollapsed = menuBar.isSideBarCollapsed();
        var fullSteps = [
            {
                "next #main-menu-items": '<p class="title">' + introduction.resources.MainMenu + '</p>' +
                    '<p>' + introduction.resources.MainMenuIntroduction + '</p>',
                onBeforeStart: function () {
                    briefProfile.hideAllTooltips();
                    if (isSiteBarCollapsed) {
                        menuBar.domElements.hamburgerIcon.trigger('click');
                    }
                    menuBar.activateMainMenuTab();
                },
                moveUp: alpha.isScreenGreater1024 ? 0 : 120,
                arrowPointer: alpha.isScreenGreater1024 || window.innerHeight > window.innerWidth ? null : ['left', 'right', 'bottom']
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
                radius: 20
            },
            {
                "next #btn-header-menu": '<p class="title">' + introduction.resources.QuickMenu + '</p>' +
                    '<p>' + introduction.resources.QuickMenuIntroduction + '</p>' +
                    '<p><img class="quick-menu-introduction-image" src="Assets/styles/introduction/images/' + alpha.user.level + '.png" /></p>',
                shape: 'circle',
                radius: 20,
                moveUp: 60
            },
            {
                "next #old-view-link": '<p class="title">' + introduction.resources.SwitchView + '</p>' +
                    '<p>' + introduction.resources.BackOldViewIntroduction + '</p>' +
                    '<p>' + introduction.resources.WarningOldVersionDeadline + ' 10/20/2016.</p>',
                onBeforeStart: function () {
                    introduction.domElements.oldViewLink.data('old-link', introduction.domElements.oldViewLink.attr('href'));
                    introduction.domElements.oldViewLink.attr('href', '#');
                },
                addLeftOffset: 70,
                isOldViewLink: true
            },
            {
                "next #replay-instruction": '<p class="title">' + introduction.resources.Replay + '?</p>' +
                    '<p>' + introduction.resources.ReplayInstruction + '</p>',
                nextButton: { className: "myNext", text: introduction.resources.GotIt },
                showSkip: false,
                shape: 'circle',
                radius: 20,
                onBeforeStart: introduction.setOldViewLink
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
        var introductionSteps = [];
        $.each(fullSteps, function (index, step) {
            if (typeof (step.permission) === undefined
                || step.permission == null
                || alpha.user.subPageAccess.indexOf(step.permission) > -1) {
                if (!step.nextButton) {
                    step.nextButton = { text: introduction.resources.Next };
                }
                if (!step.skipButton) {
                    step.skipButton = { text: introduction.resources.Skip };
                }
                if (step.isOldViewLink
                    && $('#old-view-link').length === 0) {
                    return;
                }
                introductionSteps.push(step);
            }
        });
        introduction.enjoyHint = new EnjoyHint({
            onSkip: introduction.setOldViewLink
        });
        introduction.enjoyHint.set(introductionSteps);
        introduction.enjoyHint.run();
    },
    setOldViewLink: function () {
        if (introduction.domElements.oldViewLink.data('old-link')
            && introduction.domElements.oldViewLink.data('old-link') !== ""
            && introduction.domElements.oldViewLink.attr('href') === "#") {
            introduction.domElements.oldViewLink.attr('href', introduction.domElements.oldViewLink.data('old-link'));
        }
    },
    setReadIntroduction: function () {
        alpha.localStorage.set(introduction.readIntroductionKey, true);
    }
};
$(function () {
    introduction.init();
});
