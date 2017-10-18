/* ticker functions */
//ticker_update_start();

$('#pause').click(function () {
    pauseTicker(true);
});

$('#play').click(function () {
    playTicker(true);
});

$('#refresh').click(function () {
    ticker_update();
});

//setInterval(ticker_update, 10000);

function start_ticker(resume) {
    $('#play').hide();
    $('#pause').show();
    if (resume) {
        animate_ticker();
    }
    else {
        $('#ticker').bind('ticker', function () {
            var width = $('#ticker').width();
            $('#ticker').css({ right: -width });
            animate_ticker();
        }
        ).trigger('ticker');
    }
}

function animate_ticker() {
    var width = $('#ticker').width();
    var offset = $('#ticker').parent().width();
    var duration = 90000;
    var position = offset - parseInt($('#ticker').css('right'));

    $('#ticker').animate({ right: offset }, duration * position / (offset + width), 'linear', function () {
        $('#ticker').trigger('ticker');
    });
}



function ticker_update(start, id, interval, date) {
    if ($('#pause').is(':visible') && !$('#ticker').is(':hover')) {
        $.get('/ticker/update/?fromdate=' + $('#tckfromdate').val() + '&todate=' + $('#tcktodate').val() + '&offset=-5' + '&timezone=' + $('#selectedzone').val(),
            function (data) {
                $('#ticker').html(ticker_transform(data));
                start_ticker();
                Cufon.replace('.bpdotssquares', { fontFamily: 'BPdotsSquares', hover: true });
            }
        )
    }
}

function playTicker(click) {
    if (click) {
        $('#play').hide();
        $('#pause').show();
    }
    start_ticker(true);
}

function pauseTicker(click) {
    if (click) {
        $('#pause').hide();
        $('#play').show();
    }
    var ob = $('#ticker');
    ob.stop(true);

}