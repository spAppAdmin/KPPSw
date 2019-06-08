'use strict';

(function () {

    Office.onReady(function () {
        // Office is ready
        $(document).ready(function () {
            // The document is ready
            $('#set-color').click(setColor);
        });
    });

    function setColor() {
        Excel.run(function (context) {
            var range = context.workbook.getSelectedRange();
            range.format.fill.color = 'green';

            return context.sync();
        }).catch(function (error) {
            console.log("Error: " + error);
            if (error instanceof OfficeExtension.Error) {
                console.log("Debug info: " + JSON.stringify(error.debugInfo));
            }
        });
    }
})();