'use strict';

var passwordResources = {
    rules: "<i class='icon-information-outline icon-rules' data-toggle='modal' data-target='#pwd'></i>",
    pleaseEnterOldPassword: (<HTMLInputElement>document.getElementById('please-enter-oldPassword')).value,
    passwordCannotEmpty: (<HTMLInputElement>document.getElementById('password-cannot-empty')).value,
    confirmPasswordNotCorret: (<HTMLInputElement>document.getElementById('not-correct-confirm-password')).value,
    changePasswordSuccessfully: (<HTMLInputElement>document.getElementById('password-change-successfully')).value,
    newPassMustbeDiffOldPass: (<HTMLInputElement>document.getElementById('newPassword-must-be-diff-old')).value,
    passwordDoesNotMeetMinimunRequirement: (<HTMLInputElement>document.getElementById('password-does-not-min-requirement')).value
};