/// <reference path="../core.ts" />
class BaseChatBot {
    $: JQueryStatic;
    topWindow: any;
    expiredTimeKey = "ChatbotVerificationExpiredTime";

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
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

    updateExpiredTime = () => {
        var currentTime = new Date();
        var expiredTime = currentTime.getTime() + (3 * 60000);//3 minutes
        alpha.localStorage.set(this.expiredTimeKey, expiredTime);
    }
}