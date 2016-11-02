var Introduction;
(function (Introduction) {
    var MobileIntroduction = (function () {
        function MobileIntroduction($) {
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
                return alpha.localStorage.get(MobileIntroduction.mobileInstructionKey);
            };
            this.registerEvent = function () {
                _this.mobileCloseIntroBtn.click(function () {
                    _this.hideIntroduction();
                });
                _this.$('.header-nav, .header-nav .icon-collapse').click(function () {
                    _this.hideIntroduction();
                });
            };
            this.setReadIntroduction = function () {
                alpha.localStorage.set(MobileIntroduction.mobileInstructionKey, true);
            };
            this.$ = $;
            this.mobileBlockUI = $('.blockUI-mobile');
            this.mobileIntroModal = $('#instruction-mobile');
            this.mobileCloseIntroBtn = $('#instruction-mobile').find('.button-close');
            this.registerEvent();
        }
        MobileIntroduction.mobileInstructionKey = 'MobileInstruction';
        return MobileIntroduction;
    }());
    Introduction.MobileIntroduction = MobileIntroduction;
})(Introduction || (Introduction = {}));
