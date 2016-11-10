describe('NoticeMessage', () => {
    const notice = new NoticeMessage($);
    let onToggleMessage: jasmine.Spy,
        onToggleShowMoreMessages: jasmine.Spy,
        hideMoreBtnIfHaveNoData: jasmine.Spy,
        markMessageAsRead: jasmine.Spy;

    // Setup
    beforeEach(() => {
        setFixtures(
            '<div id="notice">' +
            '   <div class="message">' +
            '       <div class="short-content">ShortContent</div>' +
            '       <div class="full-content">FullContent</div>' +
            '       <div class="time"><span class="icon icon-plus"></span>17/08/2016</div>' +
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

    describe('registerEvent()', () => {
        it('Always - Calls Specific Methods', () => {
            // Act
            notice.registerEvent();

            // Assert
            expect(onToggleMessage).toHaveBeenCalledWith(markMessageAsRead);
            expect(onToggleShowMoreMessages).toHaveBeenCalledWith();
            expect(hideMoreBtnIfHaveNoData).toHaveBeenCalledWith();
        });
    });

    describe('onToggleMessage()', () => {
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

        it('When ShortContent Is Visible - Show Full Content', () => {
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

        it('When ShortContent Is Hidden - Hide Full Content', () => {
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

    describe('markMessageAsRead()', () => {
        let $message: JQuery,
            $ajax: jasmine.Spy;

        beforeEach(() => {
            $message = $('.message');
            $ajax = spyOn($, 'ajax');
            markMessageAsRead.and.callThrough();
        });

        it('When Message Has Not "Bold" Class - Not Send Ajax POST', () => {
            // Act
            markMessageAsRead.call(this, $message);

            // Assert
            expect($ajax).not.toHaveBeenCalled();
        });

        it('When Message Has "Bold" Class and Marked Failed Ajax POST - Not Remove "Bold" Class', () => {
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

        it('When Message Has "Bold" Class and Marked Success Ajax POST - Remove "Bold" Class', () => {
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

        it('When Send Ajax POST - Has Exact Params', () => {
            // Arrange
            $message.addClass('bold');
            $message.attr('id', 'id');
            $ajax.and.callFake((options) => {
                options.success(true);
            });

            // Act
            markMessageAsRead.call(this, $message);

            // Assert
            expect($ajax).toHaveBeenCalledWith({
                url: alpha.urls.mainRootPath + '/Notice/MarkMessageAsRead',
                data: { messageId: 'id' },
                method: 'POST',
                success: jasmine.any(Function)
            });
        });
    });

    describe('onToggleShowMoreMessages', () => {
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

        it('On Click More Info Button and Success Ajax Get - Set Response Html', () => {
            // Arrange
            const $notice: JQuery = $('#notice');

            // Act
            $moreBtn.trigger('click');

            // Assert
            expect($notice.html()).toEqual(responseHtml);
            expect(spyOn(notice, 'registerEvent')).toHaveBeenCalled();
        })
    });
});