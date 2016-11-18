describe("Dashboard_", () => {
    const dashBoard = new DashboardBase($);
    const fakeElement = $("<div></div>");

    let jquerySelectorInit, jqueryGetElementAttribute, jquerySetElementHtml, jqueryAjax;

    describe("loadInfo()_", () => {
        // Setup
        beforeEach(() => {
            jquerySelectorInit = spyOn($.fn, 'init').and.returnValue(fakeElement);
            jqueryGetElementAttribute = spyOn(fakeElement, 'attr').and.returnValue('url');
            jquerySetElementHtml = spyOn(fakeElement, 'html');
            jqueryAjax = spyOn($, 'ajax').and.callFake(() => { });
        });

        it('Always -> Call jquerySelectorInit', () => {
            // Act
            dashBoard.loadInfo('#fakeId', null);

            // Assert
            expect(jquerySelectorInit).toHaveBeenCalledWith('#fakeId', undefined);
        });

        it('Always -> Call jqueryGetElementAttribute', () => {
            // Act
            dashBoard.loadInfo('#fakeId', null);

            // Assert
            expect(jqueryGetElementAttribute).toHaveBeenCalledWith('action-url');
        });

        it('When URL Is Not Empty or Undefined -> Call jQueryGet Method ', () => {
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
        ].forEach((url) => {
            it('When URL Is Empty or Undefined -> Not Call jQueryGet Method ', () => {
                // Arrange
                jqueryGetElementAttribute.and.returnValue(url);

                // Act
                dashBoard.loadInfo('#fakeId', null);

                // Assert
                expect(jqueryAjax).not.toHaveBeenCalled();
            });
        });

        it('When URL Is Not Empty or Undefined -> Call CreateAjaxSettings and jQueryGet Methods ', () => {
            // Arrange
            const url = 'url';
            const ajaxSettings = {
                method: 'GET',
                url: url,
                success: jasmine.any(Function)
            }

            jqueryGetElementAttribute.and.returnValue(url);
            const createAjaxSettings = spyOn(dashBoard, 'createAjaxSettings').and.callThrough();

            // Act
            dashBoard.loadInfo('#fakeId', null);

            // Assert
            expect(createAjaxSettings).toHaveBeenCalledWith(url, 'GET', fakeElement, null);
            expect(jqueryAjax).toHaveBeenCalledWith(ajaxSettings);
        });

        it('On Success -> createAjaxSettings Calls Exact Methods', () => {
            // Arrange
            const handleOnSuccessLoading = jasmine.createSpy('handleOnSuccessLoading');
            jqueryAjax.and.callFake((options) => {
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