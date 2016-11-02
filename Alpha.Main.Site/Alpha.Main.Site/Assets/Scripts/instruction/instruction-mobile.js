var Instruction;
(function (Instruction) {
    var Mobile = (function () {
        function Mobile($) {
            var _this = this;
            this.showIntroduction = function () {
                _this.mobileBlockUI.show();
                _this.mobileIntroModal.show();
            };
            this.hideIntroduction = function () {
                _this.setReadIntroduction();
                _this.mobileBlockUI.hide();
                _this.mobileIntroModal.hide();
            };
            this.readIntroduction = function () {
                return alpha.localStorage.get(Mobile.mobileInstructionKey);
            };
            this.registerEvent = function () {
                _this.mobileCloseIntroBtn.click(function () {
                    _this.hideIntroduction();
                    headerMessage.init();
                });
                _this.$('.header-nav, .header-nav .icon-collapse').click(function () {
                    _this.hideIntroduction();
                });
            };
            this.setReadIntroduction = function () {
                alpha.localStorage.set(Mobile.mobileInstructionKey, true);
            };
            this.$ = $;
            this.mobileBlockUI = $('.blockUI-mobile');
            this.mobileIntroModal = $('#instruction-mobile');
            this.mobileCloseIntroBtn = $('#instruction-mobile').find('.button-close');
            this.registerEvent();
        }
        Mobile.mobileInstructionKey = 'MobileInstruction';
        return Mobile;
    }());
    Instruction.Mobile = Mobile;
})(Instruction || (Instruction = {}));
