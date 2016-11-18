describe('NoticeMessage_', () => {
    const notice = new NoticeMessage($);
    let onToggleMessage: jasmine.Spy,
        onToggleShowMoreMessages: jasmine.Spy,
        hideMoreBtnIfHaveNoData: jasmine.Spy,
        markMessageAsRead: jasmine.Spy;

    // Setup
    beforeEach(() => {
        setFixtures(
            '<div id="notice">' +
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
            '</div>'
        );

        onToggleMessage = spyOn(notice, 'onToggleMessage');
        onToggleShowMoreMessages = spyOn(notice, 'onToggleShowMoreMessages');
        hideMoreBtnIfHaveNoData = spyOn(notice, 'hideMoreBtnIfHaveNoData');
        markMessageAsRead = spyOn(notice, 'markMessageAsRead');
    });

    describe('registerEvents()_', () => {
        it('Always -> Calls Specific Methods', () => {
            // Act
            notice.registerEvents();

            // Assert
            expect(onToggleMessage).toHaveBeenCalledWith(markMessageAsRead);
            expect(onToggleShowMoreMessages).toHaveBeenCalledWith();
            expect(hideMoreBtnIfHaveNoData).toHaveBeenCalledWith();
        });
    });

    describe('onToggleMessage()_', () => {
        let $message: JQuery,
            shortContent: JQuery,
            fullContent: JQuery,
            icon: JQuery;

        // Setup
        beforeEach(() => {
            onToggleMessage.and.callThrough();
            onToggleMessage.call(this, markMessageAsRead);

            $('.message').each((index, elem) => {
                $message = $(elem);
                shortContent = $message.children('.short-content');
                fullContent = $message.children('.full-content');
                icon = $message.find('.icon');
            });
        });

        it('When ShortContent Is Visible -> Show Full Content', () => {
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

        it('When ShortContent Is Hidden -> Hide Full Content', () => {
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

    describe('markMessageAsRead()_', () => {
        let $message: JQuery,
            $ajax: jasmine.Spy;

        beforeEach(() => {
            $message = $('.message');
            $ajax = spyOn($, 'ajax');
            markMessageAsRead.and.callThrough();
        });

        it('When Message Has Not "Bold" Class -> Not Send Ajax POST', () => {
            // Act
            markMessageAsRead.call(this, $message);

            // Assert
            expect($ajax).not.toHaveBeenCalled();
        });

        it('When Message Has "Bold" Class - Marked Failed Ajax POST -> Not Remove "Bold" Class', () => {
            // Arrange
            $message.addClass('bold');
            $ajax.and.callFake((options) => {
                options.success(false);
            });

            // Act
            markMessageAsRead.call(this, $message);

            // Assert
            expect($message).toHaveClass('bold');
        });

        it('When Message Has "Bold" Class - Marked Success Ajax POST -> Remove "Bold" Class', () => {
            // Arrange
            $message.addClass('bold');
            $ajax.and.callFake((options) => {
                options.success(true);
            });

            // Act
            markMessageAsRead.call(this, $message);

            // Assert
            expect($message).not.toHaveClass('bold');
        });

        it('When Send Ajax POST -> Has Exact Ajax Settings', () => {
            // Arrange
            $message.addClass('bold');
            $message.attr('id', 'id');
            $ajax.and.callFake((options) => {
                options.success(true);
            });
            alpha.urls.mainRootPath = 'path';

            // Act
            markMessageAsRead.call(this, $message);

            // Assert
            expect($ajax).toHaveBeenCalledWith({
                url: 'path/Notice/MarkMessageAsRead',
                data: { messageId: 'id' },
                method: 'POST',
                success: jasmine.any(Function)
            });
        });
    });

    describe('onToggleShowMoreMessages()_', () => {
        const responseHtml = 'responseHtml';
        let $moreBtn: JQuery,
            $ajax: jasmine.Spy;

        beforeEach(() => {
            $moreBtn = $('.more-info');
            $ajax = spyOn($, 'ajax').and.callFake((options) => {
                options.success(responseHtml);
            });

            onToggleShowMoreMessages.and.callThrough();
            onToggleShowMoreMessages.call(this);
        });

        it('On Click More Info Button -> Change Button State', () => {
            // Act
            $moreBtn.trigger('click');

            // Assert
            expect($moreBtn).toHaveAttr('disabled', 'disabled');
            expect($moreBtn).toHaveText('Loading...');
        });

        it('On Click More Info Button and Success Ajax Get -> Set Response Html', () => {
            // Arrange
            const $notice: JQuery = $('#notice');
            spyOn(notice, 'registerEvents');

            // Act
            $moreBtn.trigger('click');

            // Assert
            expect($notice.html()).toEqual(responseHtml);
            expect(notice.registerEvents).toHaveBeenCalled();
        });

        it('Always -> Send Ajax With Correct AjaxSettings', () => {
            // Arrange
            $moreBtn.attr('totalMsg', 10);
            alpha.urls.mainRootPath = 'path';

            // Act
            $moreBtn.trigger('click');

            // Assert
            expect($ajax).toHaveBeenCalledWith({
                method: 'GET',
                url: 'path/Notice/MoreMessages?totalMessage=10',
                success: jasmine.any(Function)
            });
        });
    });

    describe('hideMoreBtnIfHaveNoData()_', () => {
        let $moreBtn: JQuery;

        beforeEach(() => {
            $moreBtn = $('.more-info');
            $moreBtn.show();
            hideMoreBtnIfHaveNoData.and.callThrough();
        });

        it('When Has No Info -> Hide More Button', () => {
            // Arrange
            appendSetFixtures('<div id="no-info"></div>');

            // Act
            hideMoreBtnIfHaveNoData.call(this);

            // Assert
            expect($moreBtn).toBeHidden();
        });

        it('When More Total Message Is Greater Than Current Message Amount(+10) -> Hide More Button', () => {
            // Arrange
            $moreBtn.attr('totalMsg', 20);

            // Act
            hideMoreBtnIfHaveNoData.call(this);

            // Assert
            expect($moreBtn).toBeHidden();
        });
    });
});