/// <reference path="../home/menu-bar.ts" />

declare var EnjoyHint: any;

interface IStep {
    permission?: any;
    nextButton?: any;
    skipButton?: any;
    isOldViewLink?: boolean;
    "next #main-menu-items"?: string;
    "next #menu-TRANSFER"?: string;
    "next #menu-VIEWLOG"?: string;
    "click #hamburger-icon"?: string;
    'next #home-icon'?: string;
    "next #btn-header-menu"?: string;
    "next #old-view-link"?: string;
    "next #replay-instruction"?: string;
    shape?: any;
    radius?: number;
    showSkip?: boolean;
    onBeforeStart?: () => void;
    moveUp?: number;
    arrowPointer?: Array<string>;
    addLeftOffset?: number;
}

var instruction = {
    enjoyHint: null,
    readIntroductionKey: 'Instruction',
    isCookieLess: (<HTMLInputElement>document.getElementById('IsCookieLess')).value === "1",
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
        ShowMainMenu: (<HTMLInputElement>document.getElementById('ShowMainMenu')).value,
        ReplayInstruction: (<HTMLInputElement>document.getElementById('ReplayInstruction')).value,
        TakeTourInstruction: (<HTMLInputElement>document.getElementById('TakeTourInstruction')).value
    },

    init: () => {
        if (!alpha.isScreenLower768) {
            const isReadIntroduction = alpha.localStorage.get(instruction.readIntroductionKey);

            instruction.cacheDOMElements();
            instruction.registerEvents();

            if (isReadIntroduction) {
                return;
            }

            if (!instruction.isCookieLess && !isReadIntroduction) {
                instruction.domElements.beginModal.modal('show');
                $('.modal-backdrop').css('z-index', 1051);

                instruction.showDelayFirstIntroduction();
            }
        }
        else if (alpha.isScreenLower768) {
            instruction.mobileIntroduction = new Instruction.Mobile($);

            if (!instruction.mobileIntroduction.readIntroduction()) {
                instruction.mobileIntroduction.showIntroduction();
            }
        }
    },

    cacheDOMElements: function () {
        instruction.domElements.beginModal = $('#beginModal');
        instruction.domElements.startIntroductionBtn = $('#start-introduction-btn');
        instruction.domElements.markInner = $("#mask-inner");
        instruction.domElements.replayInstruction = $('#replay-instruction');
        instruction.domElements.notNowIntroductionBtn = $("#not-now-introduction-btn");
        instruction.domElements.modalInstructionBody = $('#instruction-modal-body');
        instruction.domElements.oldViewLink = $('#old-view-link a');
    },

    showDelayFirstIntroduction: function () {
        var $p = instruction.domElements.markInner.find("p");
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
            instruction.domElements.markInner.addClass("in");
            setTimeout(function () {
                $p.addClass("in");
            }, 250);
        }, 250);
    },

    registerEvents: function () {
        instruction.domElements.startIntroductionBtn
            .off('click.introduction')
            .on('click.introduction', function () {
                instruction.domElements.beginModal.modal('hide');
                instruction.setReadIntroduction();
                instruction.showMainSteps();
            });

        instruction.domElements.notNowIntroductionBtn
            .off('click.introduction')
            .on('click.introduction', function () {
                instruction.domElements.beginModal.modal('hide');
                instruction.setReadIntroduction();
                instruction.showNotNowSteps();
            });

        $(document)
            .off('click.replay-introduction', '.replay-introduction')
            .on('click.replay-introduction', '.replay-introduction', instruction.showMainSteps);

        instruction.domElements.replayInstruction
            .off('click.replay-introduction')
            .on('click.replay-introduction', instruction.showMainSteps);
    },

    showNotNowSteps: function () {
        instruction.enjoyHint = new EnjoyHint({});
        instruction.enjoyHint.set([{
            "next #replay-instruction": '<p>' + instruction.resources.TakeTourInstruction + '</p>',
            nextButton: { className: "myNext", text: instruction.resources.GotIt },
            showSkip: false,
            shape: 'circle',
            radius: 20
        }]);
        instruction.enjoyHint.run();
    },

    showMainSteps: function () {
        var isSiteBarCollapsed = menuBar.isSideBarCollapsed();
        var fullSteps: Array<IStep> = [
            {
                "next #main-menu-items": '<p class="title">' + instruction.resources.MainMenu + '</p>' +
                '<p>' + instruction.resources.MainMenuIntroduction + '</p>',
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
                "next #menu-TRANSFER": '<p>' + instruction.resources.TransferMenuIntroduction + '</p>',
                permission: '[C]'
            },
            {
                "next #menu-VIEWLOG": '<p>' + instruction.resources.ViewLogMenuIntroduction + '</p>',
                permission: '[G]'
            },
            {
                "click #hamburger-icon": '<p class="title">' + instruction.resources.CollapseExpandIcon + '</p>' +
                '<p class="blinker">' + instruction.resources.CollapseExpandIconIntroduction + '</p>',
                shape: 'circle',
                radius: 25,
                showSkip: false
            },
            {
                'next #home-icon': '<p>' + instruction.resources.HomeIconIntroduction + '</p>',
                shape: 'circle',
                radius: 20
            },
            {
                "next #btn-header-menu": '<p class="title">' + instruction.resources.QuickMenu + '</p>' +
                '<p>' + instruction.resources.QuickMenuIntroduction + '</p>' +
                '<p><img class="quick-menu-introduction-image" src="Assets/styles/introduction/images/' + alpha.user.level + '.png" /></p>',
                shape: 'circle',
                radius: 20,
                moveUp: 60
            },
            {
                "next #old-view-link": '<p class="title">' + instruction.resources.SwitchView + '</p>' +
                '<p>' + instruction.resources.BackOldViewIntroduction + '</p>' +
                '<p>' + instruction.resources.WarningOldVersionDeadline + ' 10/20/2016.</p>',
                onBeforeStart: function () {
                    instruction.domElements.oldViewLink.data('old-link', instruction.domElements.oldViewLink.attr('href'));
                    instruction.domElements.oldViewLink.attr('href', '#');
                },
                addLeftOffset: 70,
                isOldViewLink: true
            },
            {
                "next #replay-instruction": '<p class="title">' + instruction.resources.Replay + '?</p>' +
                '<p>' + instruction.resources.ReplayInstruction + '</p>',
                nextButton: { className: "myNext", text: instruction.resources.GotIt },
                showSkip: false,
                shape: 'circle',
                radius: 20,
                onBeforeStart: instruction.setOldViewLink
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
                    step.nextButton = { text: instruction.resources.Next }
                }

                if (!step.skipButton) {
                    step.skipButton = { text: instruction.resources.Skip }
                }

                if (step.isOldViewLink
                    && $('#old-view-link').length === 0) {
                    return;
                }

                introductionSteps.push(step);
            }
        });

        instruction.enjoyHint = new EnjoyHint({
            onSkip: instruction.setOldViewLink
        });
        instruction.enjoyHint.set(introductionSteps);
        instruction.enjoyHint.run();
    },

    setOldViewLink: function () {
        if (instruction.domElements.oldViewLink.data('old-link')
            && instruction.domElements.oldViewLink.data('old-link') !== ""
            && instruction.domElements.oldViewLink.attr('href') === "#") {
            instruction.domElements.oldViewLink.attr('href', instruction.domElements.oldViewLink.data('old-link'));
        }
    },

    setReadIntroduction: function () {
        alpha.localStorage.set(instruction.readIntroductionKey, true);
    }
};

$(() => {
    instruction.init();
});