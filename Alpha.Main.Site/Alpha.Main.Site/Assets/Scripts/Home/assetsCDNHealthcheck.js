//class AssetsCDNHealthcheck {
//}
(function () {
    var assetAppPath = "",
        CDNReCheckinIntervalInSeconds = null;

    function reCheckinCDNAssetAppPath() {
        var xmlhttp = new XMLHttpRequest();
        xmlhttp.onreadystatechange = function () {
            if (xmlhttp.readyState === XMLHttpRequest.DONE) {
                if (xmlhttp.status === 200) {
                    setTimeout(reCheckinCDNAssetAppPath, 1000 * CDNReCheckinIntervalInSeconds);
                } else {
                    resetAssetAppPath();
                }
            }
        };

        var healthCheckUrl = assetAppPath + "/bundles/common/hc.css?v=" + (new Date()).getTime();
        xmlhttp.open("GET", healthCheckUrl, true);
        xmlhttp.send();
    }

    function resetAssetAppPath() {
        var xmlhttp = new XMLHttpRequest();
        xmlhttp.onreadystatechange = function () {
            xmlhttp.readyState === XMLHttpRequest.DONE && reloadPage();
        };

        var url = document.getElementById("mainRootPath").value + "Theme/ResetAssetAppPath";
        xmlhttp.open("POST", url, true);
        xmlhttp.send();
    }

    function reloadPage() {
        top.location.href = removeBookmarksInUrl(top.location.href);
    }

    function removeBookmarksInUrl(url) {
        if (typeof url !== "string") {
            return null;
        }

        var i = url.indexOf("#");
        if (i === -1) {
            return url;
        }

        return url.substr(0, i);
    }

    function initialize() {
        var assetAppPathElement = document.getElementById("assetAppPath");
        var CDNReCheckinIntervalInSecondsElement = document.getElementById("CDNReCheckinIntervalInSeconds");

        if (assetAppPathElement && CDNReCheckinIntervalInSecondsElement) {
            assetAppPath = assetAppPathElement.value;
            CDNReCheckinIntervalInSeconds = CDNReCheckinIntervalInSecondsElement.value;

            reCheckinCDNAssetAppPath();
        }
    }

    initialize();
})();