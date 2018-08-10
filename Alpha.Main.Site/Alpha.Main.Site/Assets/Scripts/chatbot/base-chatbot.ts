class BaseChatBot {
    $: JQueryStatic;
    topWindow: any;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    resources = {
        thisFieldRequired: (<HTMLInputElement>document.getElementById('ThisFieldRequired')).value,
        accountDoesNotExist: (<HTMLInputElement>document.getElementById('AccountDoesNotExist')).value,
        verificationCodeIsInvalidOrExpired: (<HTMLInputElement>document.getElementById('VerificationCodeIsInvalidOrExpired')).value,
        unsubscribeWarning: (<HTMLInputElement>document.getElementById('UnsubscribeWarning')).value,
        overMaxAddAccountTimes: (<HTMLInputElement>document.getElementById('OverMaxAddAccountTimes')).value,
        accountHasAlreadyBeenRegistered: (<HTMLInputElement>document.getElementById('AccountHasAlreadyBeenRegistered')).value,
        updateLanguageFailed: (<HTMLInputElement>document.getElementById('UpdateLanguageFailed')).value,
        accountIsRequired: (<HTMLInputElement>document.getElementById('AccountIsRequired')).value,
        verificationCodeIsRequired: (<HTMLInputElement>document.getElementById('VerificationCodeIsRequired')).value,
        captchaIsRequired: (<HTMLInputElement>document.getElementById('CaptchaIsRequired')).value
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