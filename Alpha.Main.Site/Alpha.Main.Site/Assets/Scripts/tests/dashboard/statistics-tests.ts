describe('Statistics', () => {
    const stats: Statistics = new Statistics($);
    const responseHtml = 'responseHtml';

    let onToggleDropdown: jasmine.Spy,
        loadDropdownInfo: jasmine.Spy,
        $ajax: jasmine.Spy;

    beforeEach(() => {
        setFixtures(
            '<strong data-toggle="tooltip" data-placement="top" title="<div>test</div>">' +
            '<span class="active">1</span>' +
            '</strong>' +
            '<div id="id" action-url="url">' +
            '</div>' +
            '<div id="data"></div>')

        onToggleDropdown = spyOn(stats, 'onToggleDropdown');
        loadDropdownInfo = spyOn(stats, 'loadDropdownInfo');

        $ajax = spyOn($, 'ajax').and.callFake((options) => {
            options.success(responseHtml);
        });
    });

    describe('registerEvent()', () => {
        it('On MouseOver -> Show Tooltip', () => {
            // Arrange
            const $tooltip = $('[data-toggle="tooltip"]');
            stats.registerEvent();

            // Act
            $tooltip.trigger('mouseover');

            // Assert
            expect($tooltip).toHaveAttr('aria-describedby');
            expect($tooltip.next()).toHaveId($tooltip.attr('aria-describedby'));
        });

        it('Always -> Call onToggleDropdown method', () => {
            // Act
            stats.registerEvent();

            // Assert
            expect(onToggleDropdown).toHaveBeenCalledTimes(3);
            expect(onToggleDropdown).toHaveBeenCalledWith('#listTopWinningMembers', loadDropdownInfo);
            expect(onToggleDropdown).toHaveBeenCalledWith('#listTopTurnoverMembers', loadDropdownInfo);
            expect(onToggleDropdown).toHaveBeenCalledWith('#listNewMembers', loadDropdownInfo);
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