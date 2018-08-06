class BaseChatBot {
    $: JQueryStatic;
    bootbox: any;

    constructor($: JQueryStatic, bootbox: any) {
        this.$ = $;
        this.bootbox = bootbox;
    }

    resources = {
        thisFieldRequired: (<HTMLInputElement>document.getElementById('ThisFieldRequired')).value,
        accountDoesNotExist: (<HTMLInputElement>document.getElementById('AccountDoesNotExist')).value,
        verificationCodeIsInvalidOrExpired: (<HTMLInputElement>document.getElementById('VerificationCodeIsInvalidOrExpired')).value,
        unsubscribeWarning: (<HTMLInputElement>document.getElementById('UnsubscribeWarning')).value,
        overMaxAddAccountTimes: (<HTMLInputElement>document.getElementById('OverMaxAddAccountTimes')).value,
        accountHasAlreadyBeenRegistered: (<HTMLInputElement>document.getElementById('AccountHasAlreadyBeenRegistered')).value,
        updateLanguageFailed: (<HTMLInputElement>document.getElementById('UpdateLanguageFailed')).value
    }

    onComplete = () => {
        this.$.unblockUI();
    }

    safelyParseJSON = (json) => {
        var parsed;
        try {
            parsed = JSON.parse(json);
        } catch (e) { }

        return parsed;
    }

    convertAttributeToBool = (attrValue: string) => {
        return !!attrValue && attrValue.toString().toLowerCase() === "true";
    };
}