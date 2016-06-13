declare var EnjoyHint: any;

//initialize instance
var enjoyhint_instance = new EnjoyHint({});

//simple config.
//Only one step - highlighting(with description) "New" button
//hide EnjoyHint after a click on the button.
var enjoyhint_script_steps = [
    {
        "next .icon-home": '<p>Home tab feature will be replaced by Home icon.</p><br>' +
            '<img src="Styles/Images/OldUI-1.png" /><br><br><br>',
        shape: 'circle',
        radius: 15
    },
    {
        "next .mainmenu-container": '<p>Transfer will be moved into Main Menu </p><br>' +
            '<img src="Styles/Images/OldUI-2.png" /> <br><br><br>', 
		top: 500,
		bottom: 100
    },    
    {
        "click .icon-squares": "Click the button and move on.",
        showSkip: false,
        shape: 'circle',
        radius: 20
    },
    {
        "next .list-setting": '<p>Security tab will be moved in Quick Menu and divided into  </p>' +
            '<p>- Password</p>' + '<p>- Security Code</p><br>' +
            '<img src="Styles/Images/OldUI-3.png" /> <br><br><br>'
    },
    
    {
        "next .content-left": "Lorem ipsum dolor sit amet<br>" +
            "consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua",
        showSkip: false,
        'nextButton': { className: "myNext", text: "Got it!" }
    }
];

//set script config
enjoyhint_instance.set(enjoyhint_script_steps);

//run Enjoyhint script
//enjoyhint_instance.run();
