// njhones code report opts

function reportopts() {
    $('#ropts').on('click', function (e) {
        e.preventDefault();
        var editbox = $('.jarviswidget-editbox');
        var speed = 200;
        //alert($.rtable.fnSettings().aaSorting);
        //alert('here');
        if (editbox.is(':visible')) {
            // start $.rtimer
            //runReport(rurl);
            $.rurl.update();
            //updateRunUrl();
            $(this)
              .children()
              .removeClass('fa fa-save')
              .addClass('fa fa-gear');

            editbox.slideUp(speed, function () {
                // Animation complete.
            });
        }
        else {
            // stop $.rtimer
            clearTimeout($.rtimer);

            $(this)
               .children()
               .removeClass('fa fa-gear')
               .addClass('fa fa-save');

            editbox.slideDown(speed, function () {
                // Animation complete.
            });
        }
    });
}