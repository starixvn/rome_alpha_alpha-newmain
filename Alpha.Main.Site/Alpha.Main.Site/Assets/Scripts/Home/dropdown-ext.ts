class DropDownExtension {
    registerEvent = () => {
        this.onClickForDropdownMultiLevel();
    }

    private onClickForDropdownMultiLevel = () => {
        $(".dropdown-menu > li > a.multi-level").on("click", function (e) {
            const current = $(this).next();
            current.toggle();
            e.stopPropagation();
        });
    }
}