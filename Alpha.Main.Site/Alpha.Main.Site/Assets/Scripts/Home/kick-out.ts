/// <reference path="../core.ts" />

class Kickout {
    message: string;

    init = () => {
        this.message = document.getElementById('message') != null
            ? (<HTMLInputElement>document.getElementById('message')).value
            : "Sorry, there was an unexpected issue on the server. Please seek help from our customer support. [5003]";

        alpha.core.signOut();
        alert(this.message);
    }
}

$(() => {
    var kickOut = new Kickout();
    kickOut.init();
});