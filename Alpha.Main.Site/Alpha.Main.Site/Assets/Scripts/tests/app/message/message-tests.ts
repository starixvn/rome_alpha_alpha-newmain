describe('Message_', () => {
    const message = new Message($);

    let initDateTimePicker: jasmine.Spy;

    beforeEach(() => {
        setFixtures(
            '<div class="nav-tabs">' +
            '   <li role="presentation" class="active">' +
            '       <a href="">Personal Message</a>' +
            '   </li>' +
            '</div>' +
            '<div class="input-group date" id="fromDateContainer" style="position: relative">' +
            '   <input class="form-control" id="fromDate" name="StartDate" type="text" />' +
            '   <span class="input-group-addon">' +
            '       <span class="icon-calendar"></span>' +
            '   </span>' +
            '   <button class="btn-submit"></button>' +
            '</div>'
        );

        initDateTimePicker = spyOn(message, 'initDateTimePicker');
    });

    describe('registerEvent()_', () => {
        it('Always -> Call Specific Methods', () => {
            // Act
            message.registerEvents();

            // Assert
            expect(initDateTimePicker).toHaveBeenCalledTimes(2);
            expect(initDateTimePicker).toHaveBeenCalledWith('#fromDateContainer');
            expect(initDateTimePicker).toHaveBeenCalledWith('#toDateContainer');
        });
    });

    describe('onClick()_', () => {
        let $blockUI: jasmine.Spy;

        beforeEach(() => {
            $blockUI = spyOn($, 'blockUI');
            message.registerEvents();
        });

        it('On Click Tab -> Call Block UI', () => {
            // Arrange
            const $tab = $('.nav-tabs > li');

            // Act
            $tab.trigger('click');

            // Assert
            expect($blockUI).toHaveBeenCalled();
        });

        it('On Click Submit -> Call Block UI', () => {
            // Arrange
            const $submit = $('.btn-submit');

            // Act
            $submit.trigger('click');

            // Assert
            expect($blockUI).toHaveBeenCalled();
        });
    });

    describe('initDateTimePicker()_', () => {
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