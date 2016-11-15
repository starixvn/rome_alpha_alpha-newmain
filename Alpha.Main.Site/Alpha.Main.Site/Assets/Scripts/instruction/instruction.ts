/// <reference path="../home/menu-bar.ts" />

declare var EnjoyHint: any;

interface Step {
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

class Instruction {
    protected $: JQueryStatic;
    private enjoyHint;
    private readIntroductionKey: string = 'Instruction';
    private isCookieLess: boolean = (<HTMLInputElement>document.getElementById('IsCookieLess')).value === "1";
    private domElements = {
        beginModal: null,
        startIntroductionBtn: null,
        notNowIntroductionBtn: null,
        markInner: null,
        replayInstruction: null,
        modalInstructionBody: null,
        oldViewLink: null
    };
    private resources = {
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
    };

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        if (!alpha.isScreenLower768) {
            const isReadIntroduction = alpha.localStorage.get(this.readIntroductionKey);

            if (isReadIntroduction) {
                return;
            }

            this.cacheDOMElements();
            this.registerEvents();

            if (!this.isCookieLess && !isReadIntroduction) {
                this.domElements.beginModal.modal('show');
                this.$('.modal-backdrop').css('z-index', 1051);

                this.showDelayFirstIntroduction();
            }
        }
        else if (alpha.isScreenLower768) {
            const mobile = new InstructionMobile(this.$);
            mobile.init();
        }
    }

    cacheDOMElements = () => {
        this.domElements.beginModal = this.$('#beginModal');
        this.domElements.startIntroductionBtn = this.$('#start-introduction-btn');
        this.domElements.markInner = this.$("#mask-inner");
        this.domElements.replayInstruction = this.$('#replay-instruction');
        this.domElements.notNowIntroductionBtn = this.$("#not-now-introduction-btn");
        this.domElements.modalInstructionBody = this.$('#instruction-modal-body');
        this.domElements.oldViewLink = this.$('#old-view-link a');
    }

    showDelayFirstIntroduction = () => {
        let $p = this.domElements.markInner.find("p");
        let words = $p.text().split(" ");
        let delay = 100;
        let $tempEl = this.$("<div>");

        $p.empty().show();

        this.$.each(words, (i, word) => {
            let $span = this.$("<span>").text(word + " ");
            $span.css({
                "transition-delay": i * delay + "ms"
            });
            $tempEl.append($span);
        });

        $p.html($tempEl.html());

        setTimeout(() => {
            this.domElements.markInner.addClass("in");
            setTimeout(() => {
                $p.addClass("in");
            }, 250);
        }, 250);
    }

    registerEvents = () => {
        this.domElements.startIntroductionBtn
            .off('click.introduction')
            .on('click.introduction', () => {
                this.domElements.beginModal.modal('hide');
                this.setReadIntroduction();
                this.showMainSteps();
            });

        this.domElements.notNowIntroductionBtn
            .off('click.introduction')
            .on('click.introduction', () => {
                this.domElements.beginModal.modal('hide');
                this.setReadIntroduction();
                this.showNotNowSteps();
            });

        this.$(document)
            .off('click.replay-introduction', '.replay-introduction')
            .on('click.replay-introduction', '.replay-introduction', this.showMainSteps);

        this.domElements.replayInstruction
            .off('click.replay-introduction')
            .on('click.replay-introduction', this.showMainSteps);
    }

    showNotNowSteps = () => {
        let enjoyHint = new EnjoyHint({});

        enjoyHint.set([{
            "next #replay-instruction": '<p>' + this.resources.TakeTourInstruction + '</p>',
            nextButton: { className: "myNext", text: this.resources.GotIt },
            showSkip: false,
            shape: 'circle',
            radius: 20
        }]);

        enjoyHint.run();
    }

    showMainSteps = () => {
        let isSiteBarCollapsed = menuBar.isSideBarCollapsed();
        let fullSteps: Array<Step> = [
            {
                "next #main-menu-items": '<p class="title">' + this.resources.MainMenu + '</p>' +
                '<p>' + this.resources.MainMenuIntroduction + '</p>',
                onBeforeStart: () => {
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
                "next #menu-TRANSFER": '<p>' + this.resources.TransferMenuIntroduction + '</p>',
                permission: '[C]'
            },
            {
                "next #menu-VIEWLOG": '<p>' + this.resources.ViewLogMenuIntroduction + '</p>',
                permission: '[G]'
            },
            {
                "click #hamburger-icon": '<p class="title">' + this.resources.CollapseExpandIcon + '</p>' +
                '<p class="blinker">' + this.resources.CollapseExpandIconIntroduction + '</p>',
                shape: 'circle',
                radius: 25,
                showSkip: false
            },
            {
                'next #home-icon': '<p>' + this.resources.HomeIconIntroduction + '</p>',
                shape: 'circle',
                radius: 20
            },
            {
                "next #btn-header-menu": '<p class="title">' + this.resources.QuickMenu + '</p>' +
                '<p>' + this.resources.QuickMenuIntroduction + '</p>' +
                '<p><img class="quick-menu-introduction-image" src="Assets/styles/instruction/images/' + alpha.user.level + '.png" /></p>',
                shape: 'circle',
                radius: 20,
                moveUp: 60
            },
            {
                "next #old-view-link": '<p class="title">' + this.resources.SwitchView + '</p>' +
                '<p>' + this.resources.BackOldViewIntroduction + '</p>' +
                '<p>' + this.resources.WarningOldVersionDeadline + ' 10/20/2016.</p>',
                onBeforeStart: () => {
                    this.domElements.oldViewLink.data('old-link', this.domElements.oldViewLink.attr('href'));
                    this.domElements.oldViewLink.attr('href', '#');
                },
                addLeftOffset: 70,
                isOldViewLink: true
            },
            {
                "next #replay-instruction": '<p class="title">' + this.resources.Replay + '?</p>' +
                '<p>' + this.resources.ReplayInstruction + '</p>',
                nextButton: { className: "myNext", text: this.resources.GotIt },
                showSkip: false,
                shape: 'circle',
                radius: 20,
                onBeforeStart: this.setOldViewLink
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
        this.$.each(fullSteps, (index, step) => {
            if (typeof (step.permission) === undefined
                || step.permission == null
                || alpha.user.subPageAccess.indexOf(step.permission) > -1) {
                if (!step.nextButton) {
                    step.nextButton = { text: this.resources.Next }
                }

                if (!step.skipButton) {
                    step.skipButton = { text: this.resources.Skip }
                }

                if (step.isOldViewLink
                    && this.$('#old-view-link').length === 0) {
                    return;
                }

                introductionSteps.push(step);
            }
        });

        let enjoyHint = new EnjoyHint({
            onSkip: this.setOldViewLink
        });

        enjoyHint.set(introductionSteps);
        enjoyHint.run();
    }

    setOldViewLink = () => {
        if (this.domElements.oldViewLink.data('old-link')
            && this.domElements.oldViewLink.data('old-link') !== ""
            && this.domElements.oldViewLink.attr('href') === "#") {
            this.domElements.oldViewLink.attr('href', this.domElements.oldViewLink.data('old-link'));
        }
    }

    setReadIntroduction = () => {
        alpha.localStorage.set(this.readIntroductionKey, true);
    }
}

$(() => {
    const instruction = new Instruction($);
    instruction.init();
});