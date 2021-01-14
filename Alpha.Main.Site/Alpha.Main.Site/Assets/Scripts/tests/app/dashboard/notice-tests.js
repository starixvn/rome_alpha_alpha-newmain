var _this = this;
describe('NoticeMessage_', function () {
    var notice = new NoticeMessage($);
    var onToggleMessage, onToggleShowMoreMessages, hideMoreBtnIfHaveNoData, markMessageAsRead;
    // Setup
    beforeEach(function () {
        setFixtures('<div id="notice">' +
            '   <div class="msg-contents">' +
            '       <div class="message">' +
            '           <div class="time"><span class="icon icon-plus"></span>17/08/2016</div>' +
            '           <div class="short-content">ShortContent</div>' +
            '           <div class="full-content">FullContent</div>' +
            '       </div>' +
            '   </div>' +
            '   <div class="more-info" totalMsg="10">' +
            '       <span class="icon-arrow-down icon-more"></span>' +
            '   </div>' +
            '</div>');
        onToggleMessage = spyOn(notice, 'onToggleMessage');
        onToggleShowMoreMessages = spyOn(notice, 'onToggleShowMoreMessages');
        hideMoreBtnIfHaveNoData = spyOn(notice, 'hideMoreBtnIfHaveNoData');
        markMessageAsRead = spyOn(notice, 'markMessageAsRead');
    });
    describe('registerEvents()_', function () {
        it('Always -> Calls Specific Methods', function () {
            // Act
            notice.registerEvents();
            // Assert
            expect(onToggleMessage).toHaveBeenCalledWith(markMessageAsRead);
            expect(onToggleShowMoreMessages).toHaveBeenCalledWith();
            expect(hideMoreBtnIfHaveNoData).toHaveBeenCalledWith();
        });
    });
    describe('onToggleMessage()_', function () {
        var $message, shortContent, fullContent, icon;
        // Setup
        beforeEach(function () {
            onToggleMessage.and.callThrough();
            onToggleMessage.call(_this, markMessageAsRead);
            $('.message').each(function (index, elem) {
                $message = $(elem);
                shortContent = $message.children('.short-content');
                fullContent = $message.children('.full-content');
                icon = $message.find('.icon');
            });
        });
        it('When ShortContent Is Visible -> Show Full Content', function () {
            // Arrange
            shortContent.show();
            fullContent.hide();
            // Act
            $message.trigger('click');
            // Assert
            expect(shortContent).toBeHidden();
            expect(fullContent).toBeVisible();
            expect(icon).not.toHaveClass('icon-plus');
            expect(icon).toHaveClass('icon-minus');
        });
        it('When ShortContent Is Hidden -> Hide Full Content', function () {
            // Arrange
            shortContent.hide();
            fullContent.show();
            icon.toggleClass('icon-plus icon-minus');
            // Act
            $message.trigger('click');
            // Assert
            expect(shortContent).toBeVisible();
            expect(fullContent).toBeHidden();
            expect(icon).not.toHaveClass('icon-minus');
            expect(icon).toHaveClass('icon-plus');
        });
    });
    describe('markMessageAsRead()_', function () {
        var $message, $ajax;
        beforeEach(function () {
            $message = $('.message');
            $ajax = spyOn($, 'ajax');
            markMessageAsRead.and.callThrough();
        });
        it('When Message Has Not "Bold" Class -> Not Send Ajax POST', function () {
            // Act
            markMessageAsRead.call(_this, $message);
            // Assert
            expect($ajax).not.toHaveBeenCalled();
        });
        it('When Message Has "Bold" Class - Marked Failed Ajax POST -> Not Remove "Bold" Class', function () {
            // Arrange
            $message.addClass('bold');
            $ajax.and.callFake(function (options) {
                options.success(false);
            });
            // Act
            markMessageAsRead.call(_this, $message);
            // Assert
            expect($message).toHaveClass('bold');
        });
        it('When Message Has "Bold" Class - Marked Success Ajax POST -> Remove "Bold" Class', function () {
            // Arrange
            $message.addClass('bold');
            $ajax.and.callFake(function (options) {
                options.success(true);
            });
            // Act
            markMessageAsRead.call(_this, $message);
            // Assert
            expect($message).not.toHaveClass('bold');
        });
        it('When Send Ajax POST -> Has Exact Ajax Settings', function () {
            // Arrange
            $message.addClass('bold');
            $message.attr('id', 'id');
            $ajax.and.callFake(function (options) {
                options.success(true);
            });
            alpha.urls.messageRootPath = 'path/';
            // Act
            markMessageAsRead.call(_this, $message);
            // Assert
            expect($ajax).toHaveBeenCalledWith({
                url: 'path/PortalMessage/MarkMessageAsRead',
                data: { messageId: 'id' },
                method: 'POST',
                success: jasmine.any(Function)
            });
        });
    });
    describe('onToggleShowMoreMessages()_', function () {
        var responseHtml = 'responseHtml';
        var $moreBtn, $ajax;
        beforeEach(function () {
            $moreBtn = $('.more-info');
            $ajax = spyOn($, 'ajax').and.callFake(function (options) {
                options.success(responseHtml);
            });
            onToggleShowMoreMessages.and.callThrough();
            onToggleShowMoreMessages.call(_this);
        });
        it('On Click More Info Button -> Change Button State', function () {
            // Act
            $moreBtn.trigger('click');
            // Assert
            expect($moreBtn).toHaveAttr('disabled', 'disabled');
            expect($moreBtn).toContainHtml('<span class="block-icon-small"></span>');
        });
        it('On Click More Info Button and Success Ajax Get -> Set Response Html', function () {
            // Arrange
            var $notice = $('#notice');
            spyOn(notice, 'registerEvents');
            // Act
            $moreBtn.trigger('click');
            // Assert
            expect($notice.html()).toEqual(responseHtml);
            expect(notice.registerEvents).toHaveBeenCalled();
        });
        it('Always -> Send Ajax With Correct AjaxSettings', function () {
            // Arrange
            $moreBtn.attr('totalMsg', 10);
            alpha.urls.messageRootPath = 'path/';
            // Act
            $moreBtn.trigger('click');
            // Assert
            expect($ajax).toHaveBeenCalledWith({
                method: 'GET',
                url: 'path/PortalMessage/MoreMessages?totalMessage=10',
                cache: false,
                success: jasmine.any(Function)
            });
        });
    });
    describe('hideMoreBtnIfHaveNoData()_', function () {
        var $moreBtn;
        beforeEach(function () {
            $moreBtn = $('.more-info');
            $moreBtn.show();
            hideMoreBtnIfHaveNoData.and.callThrough();
        });
        it('When Has No Info -> Hide More Button', function () {
            // Arrange
            appendSetFixtures('<div id="no-info"></div>');
            // Act
            hideMoreBtnIfHaveNoData.call(_this);
            // Assert
            expect($moreBtn).toBeHidden();
        });
        it('When More Total Message Is Greater Than Current Message Amount(+10) -> Hide More Button', function () {
            // Arrange
            $moreBtn.attr('totalMsg', 20);
            // Act
            hideMoreBtnIfHaveNoData.call(_this);
            // Assert
            expect($moreBtn).toBeHidden();
        });
    });
});
//# sourceMappingURL=notice-tests.js.map