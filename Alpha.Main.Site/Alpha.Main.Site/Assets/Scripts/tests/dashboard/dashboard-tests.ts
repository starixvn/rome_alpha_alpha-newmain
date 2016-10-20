/// <chutzpah_reference path="../../dashboard/dashboard.js" />
/// <chutzpah_reference path="../libs/jquery-1.11.3.js" />

describe("Test Dashboard class", () => {
    const dashBoard = new Dashboard($);
    const fakeElement = $("<div></div>");

    let jquerySelectorInit, jqueryGetElementAttribute, jquerySetElementHtml, jqueryGet;

    // Setup
    beforeEach(() => {
        jquerySelectorInit = spyOn($.fn, 'init').and.returnValue(fakeElement);
        jqueryGetElementAttribute = spyOn(fakeElement, 'attr').and.returnValue('url');
        jquerySetElementHtml = spyOn(fakeElement, 'html');
        jqueryGet = spyOn($, 'get').and.callFake(() => { });
    });

    describe("Test loadInfo method", () => {
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
            expect(jqueryGet).toHaveBeenCalled();
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
                expect(jqueryGet).not.toHaveBeenCalled();
            });
        });

        it('Call createAjaxSettings and jqueryGet methods if url is not empty or undefined', () => {
            // Arrange
            const url = 'url';
            const ajaxSettings = {
                url: url,
                onSuccess: () => { }
            }

            jqueryGetElementAttribute.and.returnValue(url);
            const createAjaxSettings = spyOn(dashBoard, 'createAjaxSettings').and.returnValue(ajaxSettings);

            // Act
            dashBoard.loadInfo('#fakeId', null);

            // Assert
            expect(createAjaxSettings).toHaveBeenCalledWith(url, fakeElement, null);
            expect(jqueryGet).toHaveBeenCalledWith(ajaxSettings);
        });
    });

    describe("Test createAjaxSettings method", () => {
        it('Call exact methods on success', () => {
            // Arrange
            const handleOnSuccessLoading = jasmine.createSpy('handleOnSuccessLoading');
            jqueryGet.and.callFake((options) => {
                options.onSuccess('responseHtml');
            });

            // Act
            dashBoard.loadInfo('#fakeId', handleOnSuccessLoading);

            // Assert
            expect(jquerySetElementHtml).toHaveBeenCalledWith('responseHtml');
            expect(handleOnSuccessLoading).toHaveBeenCalled();
        });
    });
});