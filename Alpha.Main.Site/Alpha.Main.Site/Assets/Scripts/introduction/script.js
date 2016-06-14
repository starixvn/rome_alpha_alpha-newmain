//initialize instance
var enjoyhint_instance = new EnjoyHint({});
//simple config.
//Only one step - highlighting(with description) "New" button
//hide EnjoyHint after a click on the button.
var enjoyhint_script_steps = [
    {
        "next .icon-home": '<p>Home tab feature is replaced by Home icon</p>' +
            '<img src="Assets/styles/introduction/images/OldUI-1.png" /><br><br>',
        shape: 'circle',
        radius: 20
    },
    {
        "next .list-menu": '<p>Transfer and View Log are moved into Main Menu </p>' +
            '<img src="Assets/styles/introduction/images/OldUI-2.png" /><br><br>',
        top: 240,
        bottom: 50
    },
    {
        "click #btn-header-menu": '<p class="title">Quick Menu</p>' +
            '<p>Your account settings are grouped together for quick and easy access</p>' +
            '<p>Click the button to move on.</p>',
        showSkip: false,
        shape: 'circle',
        radius: 20
    },
    {
        "next .list-setting": '<p class="title">Quick Menu</p>' +
            '<p>Security tab is divided into Password and Security Code in Quick Menu</p >' +
            '<img src="Assets/styles/introduction/images/OldUI-3.png" /><br><br><br>'
    },
    {
        "click .language-selector-container": '<p class="title">Languages</p>' +
            '<p>Click on the flag icon to switch among the languages simply</p>',
        showSkip: false,
        bottom: 1
    },
    {
        "next #language": '<p>Update the visual look and feel of the language feature with flag icons</p><br>'
    },
    {
        "next .view-type": '<p class="title">Switch View</p>' +
            '<p>Quickly switch between Old and New View of the website.</p>' +
            '<p>Note: The Old View of the website is only maintained until DD/MM/YYYY.</p>'
    },
    {
        "next #navigation": '<p class="title">Main menu</p>' +
            '<p>Reorder the features you use most, all in one place.</p>',
        bottom: 400
    },
    {
        "next .list-menu .menu-item:last-child": '<p>New feature Announcements contains all the public/private messages related to your account</p>'
    },
    {
        "click .list-menu .menu-item:first-child": '<p>Click on the Reports</p>',
        showSkip: false
    },
    {
        "next .list-menu li.active:first-child ul li:last-child": '<p>The Win Loss Analysis Chart is moved into Reports menu</p>'
    },
    {
        "next .hamburger-box": '<p class="title">Collapse/expand Icon</p>' +
            '<p>Collapse/expand Menu Bar to free up space on a page.</p>',
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
//# sourceMappingURL=script.js.map