var _this = this;
describe('Statistics_', function () {
    var stats = new Statistics($);
    var responseHtml = 'responseHtml';
    var registerTooltip, onToggleDropdown, loadDropdownInfo, $ajax;
    beforeEach(function () {
        setFixtures('<strong class="tooltip-stats-member" data-toggle="tooltip" data-placement="top" title="<div>test</div>">' +
            '   <span class="active">1</span>' +
            '</strong>' +
            '<div id="id" action-url="url">' +
            '</div>' +
            '<div id="data"></div>');
        registerTooltip = spyOn(stats, 'registerTooltip');
        onToggleDropdown = spyOn(stats, 'onToggleDropdown');
        loadDropdownInfo = spyOn(stats, 'loadDropdownInfo');
        $ajax = spyOn($, 'ajax').and.callFake(function (options) {
            options.success(responseHtml);
        });
    });
    describe('registerEvent()_', function () {
        it('Always -> Call registerTooltip method', function () {
            // Act
            stats.registerEvents();
            // Assert
            expect(registerTooltip).toHaveBeenCalled();
        });
        it('Always -> Call onToggleDropdown method', function () {
            // Act
            stats.registerEvents();
            // Assert
            expect(onToggleDropdown).toHaveBeenCalledTimes(3);
            expect(onToggleDropdown).toHaveBeenCalledWith('#listTopWinningMembers', loadDropdownInfo);
            expect(onToggleDropdown).toHaveBeenCalledWith('#listTopTurnoverMembers', loadDropdownInfo);
            expect(onToggleDropdown).toHaveBeenCalledWith('#listNewMembers', loadDropdownInfo);
        });
    });
    describe('registerTooltip()_', function () {
        beforeEach(function () {
            registerTooltip.and.callThrough();
            registerTooltip.call(_this);
        });
        it('Always -> Call hideTooltipOnTouchOutInMobile() method', function () {
            // Arrange
            var hideTooltipOnTouchOutInMobile = spyOn(stats, 'hideTooltipOnTouchOutInMobile');
            // Act
            registerTooltip.call(_this);
            // Assert
            expect(hideTooltipOnTouchOutInMobile).toHaveBeenCalled();
        });
        it('On MouseOver -> Show Tooltip', function () {
            // Arrange
            var $tooltip = $('.tooltip-stats-member');
            // Act
            $tooltip.trigger('mouseover');
            // Assert
            expect($tooltip).toHaveAttr('aria-describedby');
            expect($tooltip.next()).toHaveId($tooltip.attr('aria-describedby'));
        });
    });
    describe('hideTooltipOnTouchOutInMobile()_', function () {
        beforeEach(function () {
            registerTooltip.and.callThrough();
            registerTooltip.call(_this);
        });
        it('On touch out of tooltip -> Hide tooltip', function () {
            // Arrange
            var $tooltip = $('.tooltip-stats-member');
            $tooltip.tooltip('show');
            // Act
            $('body').trigger('touchend');
            // Assert
            expect($tooltip).not.toHaveAttr('aria-describedby');
            expect($tooltip.next()).not.toHaveId($tooltip.attr('aria-describedby'));
        });
        it('On touch on tooltip -> Show tooltip', function () {
            // Arrange
            var $tooltip = $('.tooltip-stats-member');
            $tooltip.tooltip('hide');
            // Act
            $tooltip.trigger('touchend');
            // Assert
            expect($tooltip).toHaveAttr('aria-describedby');
            expect($tooltip.next()).toHaveId($tooltip.attr('aria-describedby'));
        });
    });
    describe('onToggleDropdown()_', function () {
        var id = '#id';
        var actionUrl = 'url';
        beforeEach(function () {
            onToggleDropdown.and.callThrough();
            loadDropdownInfo.and.callThrough();
            onToggleDropdown.call(_this, id, loadDropdownInfo);
        });
        it('When Data Element Is Hidden - Action Url Is Valid - Get Success -> Set Response Html', function () {
            // Arrange
            $(id).next().hide();
            // Act
            $(id).trigger('click');
            // Assert
            expect(loadDropdownInfo).toHaveBeenCalledWith(actionUrl, $(id).next());
            expect($(id).next().html()).toEqual(responseHtml);
        });
        it('When Data Element Is Visible -> Not Set Response Html', function () {
            // Act
            $(id).trigger('click');
            // Assert
            expect(loadDropdownInfo).not.toHaveBeenCalled();
            expect($(id).next().html()).toEqual('');
        });
        [
            '',
            undefined
        ].forEach(function (url) {
            it('When Data Element Is Hidden - Action Url Is Invalid -> Not Set Response Html', function () {
                // Arrange
                $(id).attr('action-url', url);
                // Act
                $(id).trigger('click');
                // Assert
                expect($(id).next().html()).toEqual('');
            });
        });
    });
    describe('loadDropdownInfo()_', function () {
        it('When Call loadDropdownInfo() - Call Ajax With Exact Params', function () {
            // Act
            loadDropdownInfo.and.callThrough();
            loadDropdownInfo.call(_this, 'url', $('<div></div>'));
            // Assert
            expect($ajax).toHaveBeenCalledWith({ method: 'GET', url: 'url', cache: false, success: jasmine.any(Function) });
        });
    });
});
//# sourceMappingURL=statistics-tests.js.map