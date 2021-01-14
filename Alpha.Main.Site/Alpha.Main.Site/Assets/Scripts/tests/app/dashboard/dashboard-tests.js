describe("Dashboard_", function () {
    var dashBoard = new DashboardBase($);
    var fakeElement = $("<div></div>");
    var jquerySelectorInit, jqueryGetElementAttribute, jquerySetElementHtml, jqueryAjax;
    describe("loadInfo()_", function () {
        // Setup
        beforeEach(function () {
            jquerySelectorInit = spyOn($.fn, 'init').and.returnValue(fakeElement);
            jqueryGetElementAttribute = spyOn(fakeElement, 'attr').and.returnValue('url');
            jquerySetElementHtml = spyOn(fakeElement, 'html');
            jqueryAjax = spyOn($, 'ajax').and.callFake(function () { });
        });
        it('Always -> Call jquerySelectorInit', function () {
            // Act
            dashBoard.loadInfo('#fakeId', null);
            // Assert
            expect(jquerySelectorInit).toHaveBeenCalledWith('#fakeId', undefined);
        });
        it('Always -> Call jqueryGetElementAttribute', function () {
            // Act
            dashBoard.loadInfo('#fakeId', null);
            // Assert
            expect(jqueryGetElementAttribute).toHaveBeenCalledWith('action-url');
        });
        it('When URL Is Not Empty or Undefined -> Call jQueryGet Method ', function () {
            // Arrange
            jqueryGetElementAttribute.and.returnValue('url');
            // Act
            dashBoard.loadInfo('#fakeId', null);
            // Assert
            expect(jqueryAjax).toHaveBeenCalled();
        });
        [
            '',
            undefined
        ].forEach(function (url) {
            it('When URL Is Empty or Undefined -> Not Call jQueryGet Method ', function () {
                // Arrange
                jqueryGetElementAttribute.and.returnValue(url);
                // Act
                dashBoard.loadInfo('#fakeId', null);
                // Assert
                expect(jqueryAjax).not.toHaveBeenCalled();
            });
        });
        it('When URL Is Not Empty or Undefined -> Call CreateAjaxSettings and jQueryGet Methods ', function () {
            // Arrange
            var url = 'url';
            var ajaxSettings = {
                method: 'GET',
                url: url,
                cache: false,
                success: jasmine.any(Function)
            };
            jqueryGetElementAttribute.and.returnValue(url);
            var createAjaxSettings = spyOn(dashBoard, 'createAjaxSettings').and.callThrough();
            // Act
            dashBoard.loadInfo('#fakeId', null);
            // Assert
            expect(createAjaxSettings).toHaveBeenCalledWith(url, 'GET', fakeElement, null);
            expect(jqueryAjax).toHaveBeenCalledWith(ajaxSettings);
        });
        it('On Success -> createAjaxSettings Calls Exact Methods', function () {
            // Arrange
            var handleOnSuccessLoading = jasmine.createSpy('handleOnSuccessLoading');
            jqueryAjax.and.callFake(function (options) {
                options.success('responseHtml');
            });
            // Act
            dashBoard.loadInfo('#fakeId', handleOnSuccessLoading);
            // Assert
            expect(jquerySetElementHtml).toHaveBeenCalledWith('responseHtml');
            expect(handleOnSuccessLoading).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=dashboard-tests.js.map