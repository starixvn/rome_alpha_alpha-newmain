class BaseTelegramChatBot {
    $: JQueryStatic;
    topWindow: any;

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
    }
}