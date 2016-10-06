'use strict'; 

var securityCodeResources = {
    rules: "<i class='icon-information-outline icon-rules' data-toggle='modal' data-target='sc'></i>",
    oldSecurityCodeEmpty: (<HTMLInputElement>document.getElementById('alert-old-securitycode-empty')).value,
    securityCodeEmpty: (<HTMLInputElement>document.getElementById('alert-securitycode-empty')).value,
    securityCodeWrongRule: (<HTMLInputElement>document.getElementById('alert-securitycode-wrong-rule')).value,
    errorSecurityCodeConfirm: (<HTMLInputElement>document.getElementById('alert-error-securitycode-confirm')).value,
    sameSecurityCode: (<HTMLInputElement>document.getElementById('alert-same-securitycode')).value,
    securityCodeChangeSucessfully: (<HTMLInputElement>document.getElementById('alert-securitycode-change-ok')).value
};