/// <chutzpah_reference path="../../dashboard/dashboard.js" />
/// <chutzpah_reference path="../libs/jquery-1.11.3.js" />

describe("Test Dashboard class", () => {
    const dashBoard = new Dashboard($);
    const fakeElement = $("<div></div>");

    let jquerySelectorInit, jqueryGetElementAttribute, jquerySetElementHtml, jqueryAjax;

    describe("Test loadInfo method", () => {
        // Setup
        beforeEach(() => {
            jquerySelectorInit = spyOn($.fn, 'init').and.returnValue(fakeElement);
            jqueryGetElementAttribute = spyOn(fakeElement, 'attr').and.returnValue('url');
            jquerySetElementHtml = spyOn(fakeElement, 'html');
            jqueryAjax = spyOn($, 'ajax').and.callFake(() => { });
        });

        it('Call jquerySelectorInit', () => {
            // Act
            dashBoard.loadInfo('#fakeId', null);

            // Assert
            expect(jquerySelectorInit).toHaveBeenCalledWith('#fakeId', undefined);
        });

        it('Call jqueryGetElementAttribute', () => {
            // Act
            dashBoard.loadInfo('#fakeId', null);

            // Assert
            expect(jqueryGetElementAttribute).toHaveBeenCalledWith('action-url');
        });

        it('Call jqueryGet method if url is not empty or undefined', () => {
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
            it('Not call jqueryGet method if url is empty or undefined', () => {
                // Arrange
                jqueryGetElementAttribute.and.returnValue(url);

                // Act
                dashBoard.loadInfo('#fakeId', null);

                // Assert
                expect(jqueryAjax).not.toHaveBeenCalled();
            });
        });

        it('Call createAjaxSettings and jqueryGet methods if url is not empty or undefined', () => {
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

        it('createAjaxSettings Call exact methods on success', () => {
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