describe('InstructionMobile', () => {
    const instructionMobile = new InstructionMobile($);
    let cacheDom: jasmine.Spy,
        registerEvent: jasmine.Spy,
        showInstruction: jasmine.Spy,
        readInstruction: jasmine.Spy;

    beforeEach(() => {
        setFixtures(
            '<div class="blockUI-mobile"></div>' +
            '<div id="instruction-mobile">' +
            '   <div class="button-close"></div>' +
            '</div>'
        );

        cacheDom = spyOn(instructionMobile, 'cacheDom');
        registerEvent = spyOn(instructionMobile, 'registerEvent');
        showInstruction = spyOn(instructionMobile, 'showInstruction');
        readInstruction = spyOn(instructionMobile, 'readInstruction');
    });

    describe('init()', () => {
        it('When Not Read Instruction -> Call Correct Methods', () => {
            // Arrange
            readInstruction.and.returnValue(false);

            // Act
            instructionMobile.init();

            // Assert
            expect(cacheDom).toHaveBeenCalled();
            expect(registerEvent).toHaveBeenCalled();
            expect(showInstruction).toHaveBeenCalled();
        });

        it('When Read Instruction -> Not Call Methods', () => {
            // Arrange
            readInstruction.and.returnValue(true);

            // Act
            instructionMobile.init();

            // Assert
            expect(cacheDom).not.toHaveBeenCalled();
            expect(registerEvent).not.toHaveBeenCalled();
            expect(showInstruction).not.toHaveBeenCalled();
        });
    });
});