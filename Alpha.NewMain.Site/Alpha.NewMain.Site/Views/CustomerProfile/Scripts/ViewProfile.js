var customerProfile;

var CustomerProfile = function () {
    var self = this;
    self.hideTooltipIntroNickname = function () {
        var tooltipBox = $('#tooltip-box');
        if (tooltipBox.length) {
            tooltipBox.remove();
        }
    };

    self.hideProfileTooltip = function () {
        var profileTooltip = $('#profileTooltip');
        if (profileTooltip.length) {
            profileTooltip.remove();
        }
    };

    self.showTooltipProfilePicture = function () {
        var profileTooltip = $('#profileTooltip');
        if (profileTooltip.length) {
            profileTooltip.css("display", "");
            setTimeout(function () {
                customerProfile.hideProfileTooltip();
            }, 30000);
        }
    };

    self.clickToHide = function () {
        var tooltipBox = $('#tooltip-box');
        var profileTooltip = $('#profileTooltip');
        if (tooltipBox.length) {
            customerProfile.hideTooltipIntroNickname();
            customerProfile.showTooltipProfilePicture();
        }
        else {
            if (profileTooltip.length) {
                customerProfile.hideProfileTooltip();
            }
        }
    };

    self.loadCustomerProfile = function () {
        var url = rootUrl + "CustomerProfile/ViewProfile";
        var xmlhttp;

        if (window.XMLHttpRequest) {
            // code for IE7+, Firefox, Chrome, Opera, Safari
            xmlhttp = new XMLHttpRequest();
        } else {
            // code for IE6, IE5
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        }

        xmlhttp.onreadystatechange = function () {
            if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
                document.getElementById("logo").innerHTML = xmlhttp.responseText;
                $(".change-picture-link").click(function () {
                    customerProfile.hideProfileTooltip();
                });

                var numerousFrames = window.parent.frames.length;

                for (var i = 0; i < numerousFrames; i++) {
                    window.parent.frames[i].window.document.onclick = function () {
                        customerProfile.clickToHide();
                    }
                }

                var tooltipBox = $('#tooltip-box');
                if (tooltipBox.length) {
                    setTimeout(function () {
                        customerProfile.hideTooltipIntroNickname();
                        customerProfile.showTooltipProfilePicture();
                    }, 10000);
                }
                else {
                    customerProfile.showTooltipProfilePicture();
                }
            }
        }

        xmlhttp.open("POST", url, true);
        xmlhttp.send();
    };
};

customerProfile = new CustomerProfile();