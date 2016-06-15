declare var EnjoyHint: any;

//initialize instance
var enjoyhint_instance = new EnjoyHint({});

//simple config.
//Only one step - highlighting(with description) "New" button
//hide EnjoyHint after a click on the button.
var enjoyhint_script_steps = [
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
        bottom: 50
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
];

//set script config
enjoyhint_instance.set(enjoyhint_script_steps);

//run Enjoyhint script
//enjoyhint_instance.run();
