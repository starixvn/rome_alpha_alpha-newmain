describe('Statistics', () => {
    const stats: Statistics = new Statistics($);
    const responseHtml = 'responseHtml';

    let registerTooltip: jasmine.Spy,
        onToggleDropdown: jasmine.Spy,
        loadDropdownInfo: jasmine.Spy,
        $ajax: jasmine.Spy;

    beforeEach(() => {
        setFixtures(
            '<strong class="tooltip-stats-member" data-toggle="tooltip" data-placement="top" title="<div>test</div>">' +
            '   <span class="active">1</span>' +
            '</strong>' +
            '<div id="id" action-url="url">' +
            '</div>' +
            '<div id="data"></div>')

        registerTooltip = spyOn(stats, 'registerTooltip');
        onToggleDropdown = spyOn(stats, 'onToggleDropdown');
        loadDropdownInfo = spyOn(stats, 'loadDropdownInfo');

        $ajax = spyOn($, 'ajax').and.callFake((options) => {
            options.success(responseHtml);
        });
    });

    describe('registerEvent()', () => {
        it('Always -> Call registerTooltip method', () => {
            // Act
            stats.registerEvents();

            // Assert
            expect(registerTooltip).toHaveBeenCalled();
        });

        it('Always -> Call onToggleDropdown method', () => {
            // Act
            stats.registerEvents();

            // Assert
            expect(onToggleDropdown).toHaveBeenCalledTimes(3);
            expect(onToggleDropdown).toHaveBeenCalledWith('#listTopWinningMembers', loadDropdownInfo);
            expect(onToggleDropdown).toHaveBeenCalledWith('#listTopTurnoverMembers', loadDropdownInfo);
            expect(onToggleDropdown).toHaveBeenCalledWith('#listNewMembers', loadDropdownInfo);
        });
    });

    describe('registerTooltip()', () => {
        beforeEach(() => {
            registerTooltip.and.callThrough();
            registerTooltip.call(this);
        });

        it('Always -> Call hideTooltipOnTouchOutInMobile() method', () => {
            // Arrange
            const hideTooltipOnTouchOutInMobile = spyOn(stats, 'hideTooltipOnTouchOutInMobile');

            // Act
            registerTooltip.call(this);

            // Assert
            expect(hideTooltipOnTouchOutInMobile).toHaveBeenCalledWith('tooltip-stats-member');
        });

        it('On MouseOver -> Show Tooltip', () => {
            // Arrange
            const $tooltip = $('.tooltip-stats-member');

            // Act
            $tooltip.trigger('mouseover');

            // Assert
            expect($tooltip).toHaveAttr('aria-describedby');
            expect($tooltip.next()).toHaveId($tooltip.attr('aria-describedby'));
        });
    });

    describe('hideTooltipOnTouchOutInMobile()', () => {
        beforeEach(() => {
            registerTooltip.and.callThrough();
            registerTooltip.call(this);
        });

        it('On touch out of tooltip -> Hide tooltip', () => {
            // Arrange
            const $tooltip = $('.tooltip-stats-member');
            $tooltip.tooltip('show');

            // Act
            $('body').trigger('touchend');

            // Assert
            expect($tooltip).not.toHaveAttr('aria-describedby');
            expect($tooltip.next()).not.toHaveId($tooltip.attr('aria-describedby'));
        });

        it('On touch on tooltip -> Show tooltip', () => {
            // Arrange
            const $tooltip = $('.tooltip-stats-member');
            $tooltip.tooltip('hide');

            // Act
            $tooltip.trigger('touchend');

            // Assert
            expect($tooltip).toHaveAttr('aria-describedby');
            expect($tooltip.next()).toHaveId($tooltip.attr('aria-describedby'));
        });
    });

    describe('onToggleDropdown()', () => {
        const id: string = '#id';
        const actionUrl: string = 'url';

        beforeEach(() => {
            onToggleDropdown.and.callThrough();
            loadDropdownInfo.and.callThrough();
            onToggleDropdown.call(this, id, loadDropdownInfo);
        });

        it('When Data Element Is Hidden - Action Url Is Valid - Get Success -> Set Response Html', () => {
            // Arrange
            $(id).next().hide();

            // Act
            $(id).trigger('click');

            // Assert
            expect(loadDropdownInfo).toHaveBeenCalledWith(actionUrl, $(id).next());
            expect($(id).next().html()).toEqual(responseHtml);
        });

        it('When Data Element Is Visible -> Not Set Response Html', () => {
            // Act
            $(id).trigger('click');

            // Assert
            expect(loadDropdownInfo).not.toHaveBeenCalled();
            expect($(id).next().html()).toEqual('');
        });

        [
            '',
            undefined
        ].forEach((url) => {
            it('When Data Element Is Hidden - Action Url Is Invalid -> Not Set Response Html', () => {
                // Arrange
                $(id).attr('action-url', url);

                // Act
                $(id).trigger('click');

                // Assert
                expect($(id).next().html()).toEqual('');
            });
        });
    });

    describe('loadDropdownInfo', () => {
        it('When Call loadDropdownInfo() - Call Ajax With Exact Params', () => {
            // Act
            loadDropdownInfo.and.callThrough();
            loadDropdownInfo.call(this, 'url', $('<div></div>'));

            // Assert
            expect($ajax).toHaveBeenCalledWith({ method: 'GET', url: 'url', success: jasmine.any(Function) });
        });
    });
});