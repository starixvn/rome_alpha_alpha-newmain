declare var EnjoyHint: any;

var introduction = {
    enjoyHint: null,

    domElements: {
        beginModal: null,
        letGoButtons: null,
        markInner: null
    },

    init: function () {
        if (!alpha.user.isShowIntroduction) {
            return;
        }

        introduction.cacheDOMElements();
        introduction.registerEvents();
    },

    cacheDOMElements: function () {
        introduction.domElements.beginModal = $('#beginModal');
        introduction.domElements.letGoButtons = $('.js-let-go-btn');
        introduction.domElements.markInner = $(".mask-inner");
        ///"[A][B][C][D][E][G][H]"
        //[0]: All
        //[A]: Create
        //[B]: Update
        //[C]: Transfer
        //[D]: Total Bets
        //[E]: Reports
        //[F]: Forcast
        //[H]: Member Information
        //[G]: View Log
    },

    introductionSteps: [
        {
            "next .icon-home": '<p>Home icon – access to home page</p>' +
            '<img src="Assets/styles/introduction/images/OldUI-1.png" /><br><br>',
            shape: 'circle',
            radius: 20
        },
        {
            "next .list-menu": '<p>Transfer and View Log are now located in the Main Menu</p>' +
            '<img src="Assets/styles/introduction/images/OldUI-2.png" /><br><br>',
            top: 240,
            bottom: 50,
            permission: '[]'
        },
        {
            "next #btn-header-menu": '<p class="title">Quick Menu</p>' +
            '<p>Quick access to Account Settings</p>' +
            '<p><img src="Assets/styles/introduction/images/Quickmenu.png" /></p>',
            shape: 'circle',
            radius: 20
        },
        {
            "next .view-type": '<p class="title">Switch View</p>' +
            '<p>Not familiar with this new interface? Click here to get back.</p>' +
            '<p>Note: The old version is only available until DD/MM/YYYY.</p>'
        },
        {
            "next #navigation": '<p class="title">Main menu</p>' +
            '<p>Logical order, all in one place</p>',
            bottom: 400
        },
        {
            "click .list-menu .menu-item:first-child":
            '<p class="blinker">What’s new in the Reports? Click this</p>',
            showSkip: false
        },
        {
            "next .list-menu li.active:first-child ul li:last-child":
            '<p>The Win Loss Analysis Chart is here</p>'
        },
        {
            "next #hamburger-icon": '<p class="title">Collapse/expand Icon</p>' +
            '<p>Free up space</p>',
            shape: 'circle',
            radius: 25,
            right: 5,
            top: 5
        }
    ],

    registerEvents: function () {
        //initialize instance
        introduction.enjoyHint = new EnjoyHint({});
        introduction.enjoyHint.set(introduction.introductionSteps);

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
            }, 300);
        }, 300);
        introduction.domElements.beginModal.modal('show');
        introduction.domElements.letGoButtons.click(function () {
            introduction.domElements.beginModal.modal('hide');
            introduction.enjoyHint.run();
        });
    }
};


$(function () {
    introduction.init();
});