/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */



$(function () {
    $(".sortable").sortable({
        revert: true
    });
    $(".draggable").draggable({
        connectToSortable: ".sortable",
        helper: "clone",
        revert: "invalid"
    });
    $("ul, li").disableSelection();
});

$(document).ready(function () {

    $(".accordion-toggle,.expand-t").click(function () {
        //                    $(".loadcls").show(100, function () {
        $(this).next(".loadcls").fadeIn(400, function () {
            //alert("time to hide");
            //                        $(".loadcls").fadeOut('400');
            $(this).fadeOut('400');
        });
    });
    $('.expand-t').click(function () {

        var ele = $(this).attr("data-value");
        $(".panel").hide();
        $("#" + ele).show();
        $("#" + ele).find('.panel-collapse').collapse('show');

        $('#top').hide();
        $('#left').hide();
        $('#events').hide();
        $('.footer').hide();
        $('#content').addClass('margin-content-n');

        $(this).addClass('display-n').removeClass("display");
        $(".collapse-t").css("display", "block").removeClass("display-n");

    });

    $('.collapse-t').click(function () {

        $('#top').show();
        $('#left').show();
        $('#events').show();
        $('#content').removeClass('margin-content-n');
        $(".panel").show();

        $('.expand-t').addClass('display').removeClass("display-n");
        $(".collapse-t").css({display: 'none'});
    });

//    $('.checkbox #campaignfilter').on(click())
//    });

    $('.checkbox #campaignfilter').click(function () {

        if ($(this).prop('checked')) {
            // do what you need here 
            $('#cpfilter').css({display: 'block'});
//            alert("Checked");
        }
        else {
            // do what you need here   
            $('#cpfilter').css({display: 'none'});
//            alert("Unchecked");
        }
    });


    $('.checkbox #affiliatefilter').click(function () {
        if ($(this).prop('checked')) {
            $('#affilter').css({display: 'block'});
        }

        else {
            $('#affilter').css({display: 'none'});
        }

    });

    $('.checkbox #countriesfilter').click(function () {
        if ($(this).prop('checked')) {
            $('#ctfilter').css({display: 'block'});
        }
        else {
            $('#ctfilter').css({display: 'none'});
        }
    });


});

$('#some_class_1').show();
$('#some_class_2').hide();
$('#some_class_3').hide();

$('#some_class_a').show();
$('#some_class_b').hide();
$('#some_class_c').hide();

var show_hour = function () {
//                                                                alert('ok');
    $('#some_class_1').show();
    $('#some_class_2').hide();
    $('#some_class_3').hide();
};

var show_day = function () {
    $('#some_class_1').show();
    $('#some_class_2').show();
    $('#some_class_3').hide();
};
var show_week = function () {
    $('#some_class_1').show();
    $('#some_class_2').show();
    $('#some_class_3').hide();
};
var show_month = function () {
    $('#some_class_1').hide();
    $('#some_class_2').hide();
    $('#some_class_3').show();
};
var show_year = function () {
    $('#some_class_1').hide();
    $('#some_class_2').hide();
    $('#some_class_3').hide();
};

var show_hour_b = function () {
    $('#some_class_a').show();
    $('#some_class_b').hide();
    $('#some_class_c').hide();
};

var show_day_b = function () {
    $('#some_class_a').show();
    $('#some_class_b').show();
    $('#some_class_c').hide();
};
var show_week_b = function () {
    $('#some_class_a').show();
    $('#some_class_b').show();
    $('#some_class_c').hide();
};
var show_month_b = function () {
    $('#some_class_a').hide();
    $('#some_class_b').hide();
    $('#some_class_c').show();
};
var show_year_b = function () {
    $('#some_class_a').hide();
    $('#some_class_b').hide();
    $('#some_class_c').hide();
};



$('#datetimepicker').datetimepicker({
    dayOfWeekStart: 1,
    lang: 'en',
    disabledDates: ['1986/01/08', '1986/01/09', '1986/01/10'],
    startDate: '1986/01/05'
});
$('#datetimepicker').datetimepicker({value: '2015/04/15 05:03', step: 10});

$('.some_class').datetimepicker();

$('#default_datetimepicker').datetimepicker({
    formatTime: 'H:i', formatDate: 'd.m.Y',
    defaultDate: '8.12.1986', // it's my birthday
    defaultTime: '10:00',
    timepickerScrollbar: false
});

$('#datetimepicker10').datetimepicker({
    step: 5,
    inline: true
});
$('#datetimepicker_mask').datetimepicker({
    mask: '9999/19/39 29:59'
});

$('#datetimepicker1').datetimepicker({
    datepicker: false,
    format: 'H:i',
    step: 5
});
$('#datetimepicker2').datetimepicker({
    yearOffset: 222,
    lang: 'ch',
    timepicker: false,
    format: 'd/m/Y',
    formatDate: 'Y/m/d',
    minDate: '-1970/01/02', // yesterday is minimum date
    maxDate: '+1970/01/02' // and tommorow is maximum date calendar
});
$('#datetimepicker3').datetimepicker({
    inline: true
});
$('#datetimepicker4').datetimepicker();
$('#open').click(function () {
    $('#datetimepicker4').datetimepicker('show');
});
$('#close').click(function () {
    $('#datetimepicker4').datetimepicker('hide');
});
$('#reset').click(function () {
    $('#datetimepicker4').datetimepicker('reset');
});
$('#datetimepicker5').datetimepicker({
    datepicker: false,
    allowTimes: ['12:00', '13:00', '15:00', '17:00', '17:05', '17:20', '19:00', '20:00'],
    step: 5
});
$('#datetimepicker6').datetimepicker();
$('#destroy').click(function () {
    if ($('#datetimepicker6').data('xdsoft_datetimepicker')) {
        $('#datetimepicker6').datetimepicker('destroy');
        this.value = 'create';
    } else {
        $('#datetimepicker6').datetimepicker();
        this.value = 'destroy';
    }
});
var logic = function (currentDateTime) {
    if (currentDateTime.getDay() == 6) {
        this.setOptions({
            minTime: '11:00'
        });
    } else
        this.setOptions({minTime: '8:00'
        });
};
$('#datetimepicker7').datetimepicker({
    onChangeDateTime: logic,
    onShow: logic
});
$('#datetimepicker8').datetimepicker({
    onGenerate: function (ct) {
        $(this).find('.xdsoft_date')
                .toggleClass('xdsoft_disabled');
    },
    minDate: '-1970/01/2',
    maxDate: '+1970/01/2',
    timepicker: false
});
$('#datetimepicker9').datetimepicker({
    onGenerate: function (ct) {
        $(this).find('.xdsoft_date.xdsoft_weekend')
                .addClass('xdsoft_disabled');
    },
    weekends: ['01.01.2014', '02.01.2014', '03.01.2014', '04.01.2014', '05.01.2014', '06.01.2014'],
    timepicker: false
});
var dateToDisable = new Date();
dateToDisable.setDate(dateToDisable.getDate() + 2);
$('#datetimepicker11').datetimepicker({
    beforeShowDay: function (date) {
        if (date.getMonth() == dateToDisable.getMonth() && date.getDate() == dateToDisable.getDate()) {
            return [false, ""]
        }

        return [true, ""];
    }
});
$('#datetimepicker12').datetimepicker({
    beforeShowDay: function (date) {
        if (date.getMonth() == dateToDisable.getMonth() && date.getDate() == dateToDisable.getDate()) {
            return [true, "custom-date-style"];
        }

        return [true, ""];
    }
});
$('#datetimepicker_dark').datetimepicker({theme: 'dark'})


      