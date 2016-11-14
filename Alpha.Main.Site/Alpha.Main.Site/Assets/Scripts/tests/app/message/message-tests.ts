describe('Message', () => {
    const message = new Message($);

    let onClickTab: jasmine.Spy,
        initDateTimePicker: jasmine.Spy,
        writeDocument: jasmine.Spy;

    beforeEach(() => {
        setFixtures(
            '<div class="tab"></div>' +
            '   <div class="input-group date" id="fromDateContainer" style="position: relative">' +
            '       <input class="form-control" id="fromDate" name="StartDate" type="text" />' +
            '       <span class="input-group-addon">' +
            '           <span class="icon-calendar"></span>' +
            '       </span>' +
            '</div>'
        );

        onClickTab = spyOn(message, 'onClickTab');
        initDateTimePicker = spyOn(message, 'initDateTimePicker');
        writeDocument = spyOn(message, 'writeDocument');
    });

    describe('registerEvent()', () => {
        it('Always -> Call Specific Methods', () => {
            // Act
            message.registerEvent();

            // Assert
            expect(onClickTab).toHaveBeenCalledTimes(3);
            expect(onClickTab).toHaveBeenCalledWith('.public-tab');
            expect(onClickTab).toHaveBeenCalledWith('.special-tab');
            expect(onClickTab).toHaveBeenCalledWith('.private-tab');

            expect(initDateTimePicker).toHaveBeenCalledTimes(2);
            expect(initDateTimePicker).toHaveBeenCalledWith('#fromDateContainer');
            expect(initDateTimePicker).toHaveBeenCalledWith('#toDateContainer');
        });
    });

    describe('onClickTab()', () => {
        let $blockUI: jasmine.Spy,
            $unBlockUI: jasmine.Spy,
            $ajax: jasmine.Spy,
            $setTimeout: jasmine.Spy,
            $tab: JQuery;

        beforeEach(() => {
            $tab = $('.tab');
            $blockUI = spyOn($, 'blockUI');
            $unBlockUI = spyOn($, 'unblockUI');
            $ajax = spyOn($, 'ajax');
            $setTimeout = spyOn(window, 'setTimeout');

            onClickTab.and.callThrough();
            onClickTab.call(this, '.tab');
        });

        it('On Click Tab -> Call Block UI and Send Ajax With Correct AjaxSettings', () => {
            // Arrange
            alpha.urls.mainRootPath = 'path';
            $tab.attr('id', 'type');

            // Act
            $tab.trigger('click');

            // Assert
            expect($blockUI).toHaveBeenCalled();
            expect($ajax).toHaveBeenCalledWith({
                url: 'path/Message/Index',
                data: { messageType: 'type' },
                method: 'GET',
                success: jasmine.any(Function),
                error: jasmine.any(Function)
            });
        });

        it('On Click Tab and Send Ajax Success -> Call Set Timeout', () => {
            // Arrange
            $ajax.and.callFake((options) => {
                options.success('responseHtml');
            });

            writeDocument.and.callFake((responseHtml) => { });

            // Act
            $tab.trigger('click');

            // Assert
            expect($setTimeout).toHaveBeenCalledWith(writeDocument(), 100);
            expect(writeDocument).toHaveBeenCalledWith('responseHtml');
        });

        it('On Click Tab and Send Ajax Error -> Call UnBlockUI', () => {
            // Arrange
            $ajax.and.callFake((options) => {
                options.error();
            });

            // Act
            $tab.trigger('click');

            // Assert
            expect($.unblockUI).toHaveBeenCalled();
        });
    });

    describe('initDateTimePicker()', () => {
        beforeEach(() => {
            initDateTimePicker.and.callThrough();
        });

        it('Always -> Init DateTimePicker', () => {
            // Arrange
            const $datePicker = spyOn($.fn, 'datetimepicker');

            // Act
            initDateTimePicker.call(this, '#fromDateContainer');

            // Assert
            expect($('#fromDateContainer').datetimepicker).toHaveBeenCalledWith({
                allowInputToggle: true,
                format: 'MM/DD/YYYY'
            });
        });

        it('On Click Input Group Addon -> Show DateTimePicker Widget', () => {
            // Arrange
            const button = $('.input-group-addon');

            // Act
            initDateTimePicker.call(this, '#fromDateContainer');
            button.trigger('click');

            // Assert
            const widget = button.prev();
            expect(widget).toHaveClass('bootstrap-datetimepicker-widget');
            expect(widget).toBeVisible();
        });
    });
});