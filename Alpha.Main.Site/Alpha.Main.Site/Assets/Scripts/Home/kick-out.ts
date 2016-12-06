/// <reference path="../core.ts" />

class Kickout {
    message: string;

    init = () => {
        this.message = document.getElementById('message') != null
            ? (<HTMLInputElement>document.getElementById('message')).value
            : "Oops... you are here because of a system error, please notify the administrators.";

        alert(this.message);
        alpha.core.signOut();
    }
}

$(() => {
    var kickOut = new Kickout();
    kickOut.init();
});