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

    removeBookmarksInUrl = (url: string) => {
        if (typeof url !== 'string') {
            return null;
        }

        var i = url.indexOf("#");
        if (i === -1) {
            return url;
        }

        return url.substr(0, i);
    }

    reloadPage = () => {
        parent.window.location.href = this.removeBookmarksInUrl(parent.window.location.href);
    }
}