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
    protected readInstructionKey: string = 'Instruction';
    private enjoyHint;
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
        HomeIconIntroduction: null,
        TransferAndViewLogIntroduction: null,
        QuickMenuIntroduction: null,
        QuickMenu: null,
        SwitchView: null,
        BackOldViewIntroduction: null,
        WarningOldVersionDeadline: null,
        MainMenuIntroduction: null,
        ReportMenuIntroduction: null,
        WinLossAnalysisChartIntroduction: null,
        CollapseExpandIcon: null,
        CollapseExpandIconIntroduction: null,
        MainMenu: null,
        TransferMenuIntroduction: null,
        ViewLogMenuIntroduction: null,
        Skip: null,
        Next: null,
        Replay: null,
        GotIt: null,
        ShowMainMenu: null,
        ReplayInstruction: null,
        TakeTourInstruction: null
    };

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.readInstructionKey = alpha.key.instruction;
        this.cacheDOM();
        this.cacheResources();
        this.registerEvents();

        if (!this.isCookieLess() && !this.readInstruction()) {
            this.showFirstInstruction();
        }
    }

    showFirstInstruction = () => {
        let $p = this.domElements.markInner.find("p");
        let words = $p.text().split(" ");
        let delay = 100;
        let $tempEl = this.$("<div>");

        this.domElements.beginModal.modal('show');
        this.$('.modal-backdrop').css('z-index', 1051);
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

    readInstruction = (): boolean => {
        return alpha.localStorage.get(this.readInstructionKey);
    }

    setReadInstruction = () => {
        alpha.localStorage.set(this.readInstructionKey, true);
    }

    isCookieLess = (): boolean => {
        return this.$('#IsCookieLess').val() === '1';
    }

    private cacheDOM = () => {
        this.domElements.beginModal = this.$('#beginModal');
        this.domElements.startIntroductionBtn = this.$('#start-introduction-btn');
        this.domElements.markInner = this.$("#mask-inner");
        this.domElements.replayInstruction = this.$('#replay-instruction');
        this.domElements.notNowIntroductionBtn = this.$("#not-now-introduction-btn");
        this.domElements.modalInstructionBody = this.$('#instruction-modal-body');
        this.domElements.oldViewLink = this.$('#old-view-link a');
    }

    private cacheResources = () => {
        this.resources.HomeIconIntroduction = this.$('#HomeIconIntroduction').val();
        this.resources.TransferAndViewLogIntroduction = this.$('#TransferAndViewLogIntroduction').val()
        this.resources.QuickMenuIntroduction = this.$('#QuickMenuIntroduction').val();
        this.resources.QuickMenu = this.$('#QuickMenu').val();
        this.resources.SwitchView = this.$('#SwitchView').val();
        this.resources.BackOldViewIntroduction = this.$('#BackOldViewIntroduction').val();
        this.resources.WarningOldVersionDeadline = this.$('#WarningOldVersionDeadline').val();
        this.resources.MainMenuIntroduction = this.$('#MainMenuIntroduction').val();
        this.resources.ReportMenuIntroduction = this.$('#ReportMenuIntroduction').val();
        this.resources.WinLossAnalysisChartIntroduction = this.$('#WinLossAnalysisChartIntroduction').val();
        this.resources.CollapseExpandIcon = this.$('#CollapseExpandIcon').val();
        this.resources.CollapseExpandIconIntroduction = this.$('#CollapseExpandIconIntroduction').val();
        this.resources.MainMenu = this.$('#MainMenu').val();
        this.resources.TransferMenuIntroduction = this.$('#TransferMenuIntroduction').val();
        this.resources.ViewLogMenuIntroduction = this.$('#ViewLogMenuIntroduction').val();
        this.resources.Skip = this.$('#Skip').val();
        this.resources.Next = this.$('#Next').val();
        this.resources.Replay = this.$('#Replay').val();
        this.resources.GotIt = this.$('#GotIt').val();
        this.resources.ShowMainMenu = this.$('#ShowMainMenu').val();
        this.resources.ReplayInstruction = this.$('#ReplayInstruction').val();
        this.resources.TakeTourInstruction = this.$('#TakeTourInstruction').val();
    }

    private registerEvents = () => {
        this.domElements.startIntroductionBtn.on('click', () => {
            this.domElements.beginModal.modal('hide');
            this.setReadInstruction();
            this.showMainSteps();
        });

        this.domElements.notNowIntroductionBtn.on('click', () => {
            this.domElements.beginModal.modal('hide');
            this.setReadInstruction();
            this.showNotNowSteps();
        });

        this.domElements.replayInstruction.on('click', this.showMainSteps);
    }
}