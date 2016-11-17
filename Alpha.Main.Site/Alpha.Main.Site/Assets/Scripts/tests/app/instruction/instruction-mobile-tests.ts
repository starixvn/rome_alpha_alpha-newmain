describe('InstructionMobile', () => {
    const instructionMobile = new InstructionMobile($);
    let init: jasmine.Spy,
        cacheDom: jasmine.Spy,
        registerEvent: jasmine.Spy,
        showInstruction: jasmine.Spy,
        readInstruction: jasmine.Spy,
        setReadInstruction: jasmine.Spy;

    let mobileBlockUI: JQuery,
        mobileInstructionModal: JQuery;

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
        setReadInstruction = spyOn(instructionMobile, 'setReadInstruction');

        mobileBlockUI = $('.blockUI-mobile');
        mobileInstructionModal = $('#instruction-mobile');
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

    describe('showInstruction()', () => {
        it('Always -> Show BlockUI and InstructionModal', () => {
            // Arrange
            cacheDom.and.callThrough();
            cacheDom.call(this);
            mobileBlockUI.hide();
            mobileInstructionModal.hide();

            // Act
            showInstruction.and.callThrough();
            showInstruction.call(this);

            // Assert
            expect(mobileBlockUI).toBeVisible();
            expect(mobileInstructionModal).toBeVisible();
        });
    });

    describe('hideInstruction()', () => {
        beforeEach(() => {
            cacheDom.and.callThrough();
            cacheDom.call(this);
        })

        it('When Instruction Is Visible', () => {
            // Arrange
            mobileBlockUI.show();
            mobileInstructionModal.show();

            // Act
            instructionMobile.hideInstruction();

            // Assert
            expect(mobileBlockUI).toBeHidden();
            expect(mobileInstructionModal).toBeHidden();
            expect(setReadInstruction).toHaveBeenCalled();
        });

        it('When Instruction Is Hidden', () => {
            // Arrange
            mobileBlockUI.hide();
            mobileInstructionModal.hide();

            // Act
            instructionMobile.hideInstruction();

            // Assert
            expect(mobileBlockUI).toBeHidden();
            expect(mobileInstructionModal).toBeHidden();
        });
    });

    describe('readInstruction()', () => {
        it('Always -> Get From Local Storage', () => {
            // Arrange
            let getKey = spyOn(alpha.localStorage, 'get').and.returnValue(true);

            // Act
            readInstruction.and.callThrough();
            const actual = readInstruction.call(this);

            // Arrange
            expect(getKey).toHaveBeenCalledWith('MobileInstruction');
            expect(actual).toBe(true);
        });
    });

    describe('setReadInstruction()', () => {
        it('Always -> Get From Local Storage', () => {
            // Arrange
            let setKey = spyOn(alpha.localStorage, 'set');

            // Act
            setReadInstruction.and.callThrough();
            setReadInstruction.call(this);

            // Arrange
            expect(setKey).toHaveBeenCalledWith('MobileInstruction', true);
        });
    });
});