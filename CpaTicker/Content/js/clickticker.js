var doAjax = 1;
var reportRefreshTime = 10000;
var tickerAnimation = 500; // how many ticker runs before refresh the page

$.rtable = null;
$.rcolindex = 0;
$.rsortdir = 'asc';
$.rtimer = null;
$.rpage = 0;
$.rfirstin = true;


//$.rcolindexheader = undefined;

var fdomain = { value: function ()
{
    var cookieValue = $.cookie("ct_domain");

    if (typeof cookieValue === 'undefined') {
        cookieValue = 0;
    }

    return cookieValue;

}
};

//var cookieValue = $.cookie("iDisplayLength");
$.riDisplayLength = {
    value: function ()
    {
        var cookieValue = $.cookie("iDisplayLength");

        if (typeof cookieValue === 'undefined') {
            cookieValue = 25;
            $.cookie("iDisplayLength", cookieValue, { expires: 10 });
        }

        return cookieValue;
    }
};

$.rurl = {
    update: function () { }
};

jQuery.ajaxSetup({
    // Abort all Ajax requests after 2 seconds
    // timeout: 2000,
    // Defeat browser cache by adding a timestamp to URL
    cache: false
});

$(document).on('click', 'h1.page-title a[href!="#"]', function (e) {
    e.preventDefault();
    var $this = $(e.currentTarget);

    // if parent is not active then get hash, or else page is assumed to be loaded
    if (!$this.parent().hasClass("active") && !$this.attr('target')) {

        // update window with hash
        // you could also do here:  $.device === "mobile" - and save a little more memory

        if ($.root_.hasClass('mobile-view-activated')) {
            $.root_.removeClass('hidden-menu');
            window.setTimeout(function () {
                if (window.location.search) {
                    window.location.href =
                        window.location.href.replace(window.location.search, '')
                            .replace(window.location.hash, '') + '#' + $this.attr('href');
                } else {
                    window.location.hash = $this.attr('href')
                }
            }, 150);
            // it may not need this delay...
        } else {
            if (window.location.search) {
                window.location.href =
                    window.location.href.replace(window.location.search, '')
                        .replace(window.location.hash, '') + '#' + $this.attr('href');
            } else {
                window.location.hash = $this.attr('href');
            }
        }
    }
});

function updateBreadCrumb(url) {
    var parts = url.split('/');
    var title = '';

    $(".breadcrumb").empty();
    //$("#ribbon ol.breadcrumb").empty();
    //$("#ribbon ol.breadcrumb").append($("<li>Home</li>"));

    for (i = 0; i < parts.length; i += 1) {
        // create a property if it doesn't exist
        //$("#ribbon ol.breadcrumb").append($("<li></li>").html(parts[i]));
        $(".breadcrumb").append($("<li></li>").html(parts[i]));

        title = parts[i] + ' ' + title;
    }

    document.title = title;
}

function pajaxSubmit(f, container, hash) {
    $(f).ajaxSubmit({
        type: "POST",
        url: f.action,
        data: $(f).serialize(),
        dataType: 'html',
        cache: false,
        async: false,
        beforeSend: function () {
            // cog placed
            //var ttop = updateSparkPos() + 20;
            //container.html('<h1><i class="fa fa-cog fa-spin" style="position:absolute;margin-top:' + ttop + 'px;"></i> Loading...</h1>');
            container.html('<h1><i class="fa fa-cog fa-spin"></i> Loading...</h1>');
        },
        success: function (data) {

            if ($(data).find('#' + f.id).length) {
                // do nothing
            }
            else {
                // update hash and avoid a third request since i already have 2 request
                // i'm updating the content!
                doAjax = 0;
                //console.log(url);
                window.location.hash = hash;
            }
            container.css({
                opacity: '0.0'
            }).html(data).delay(50).animate({
                opacity: '1.0'
            }, 300);

        },
        error: function (xhr, ajaxOptions, thrownError) {
            container.html('<h4 style="margin-top:10px; display:block; text-align:left"><i class="fa fa-warning txt-color-orangeDark"></i> Error 404! Page not found.</h4>');
        }

    });
}

//function changeHashChangeHandler()
//{
//    $(window).off("hashchange");
//    $(window).on("hashchange", function () {
//        if (doAjax == 1) {
//            checkURL();
//        }
//        else
//            doAjax = 1;
//    });
//}

/*********************** REPORTS ************************/
// doAjax on the links inside rtables (campaign / affiliate references)
$(document).on('click', '.rtable a', function (e) {
    e.preventDefault();
    var $this = $(e.currentTarget);
    window.location.hash = $this.attr('href');
});

$(document).on('change', '.dataTables_length select', function (e) {
    var $this = $(e.currentTarget);
    //console.log($this.val());
    $.cookie("iDisplayLength", $this.val(), { expires: 10 });
});

function runReport(url) {
    clearTimeout($.rtimer);

    

    $.rtimer = setTimeout(function () {
        // save current sort index column and sort direction
        

        //console.log($.rpage);
        //console.log(ipage);
        //console.log($.rtable.oSettings);
        //console.log($.rtable);
        //console.log($.rtable.fnSettings());
        //console.log($.rcolindex);
        //console.log($.rsortdir);

        //$('#content').load(url);
        container = $('#content');
        refreshReport(url, container);

        //loadURL(url, container);

    }, reportRefreshTime);
}

function refreshReport(url, container) {

    $.ajax({
        type: "GET",
        url: url,
        dataType: 'html',
        cache: false,
        beforeSend: function () {

            
               
        },
        success: function (data) {
            
            //container.html(data);

            if (checkReportUrl(url)) {

                var oSettings = $.rtable.fnSettings();

                
                //this is before refresh the content
                $.rcolindex = oSettings.aaSorting[0][0];
                $.rsortdir = oSettings.aaSorting[0][1];

                //$.rcolindexheader = $(".rtable thead tr th").eq($.rcolindex).text(); // gets the text of the sorted column
                

                //console.log('rcolindex: ' + $.rcolindex + ',  header: ' + rheader);

                // destroy all datatable instances
                if ($.navAsAjax && $('.dataTables_wrapper')[0] && (container[0] == $("#content")[0])) {

                    var tables = $.fn.dataTable.fnTables(true);
                    $(tables).each(function () {
                        $(this).dataTable().fnDestroy();
                    });

                    //console.log("datatable nuked!!!");
                
                }


                //console.log($.rcolindexheader);

                //$.riDisplayLength = oSettings._iDisplayLength;
                //var cookieValue = $.cookie("iDisplayLength", oSettings._iDisplayLength);

                // set the cookie
                //$.cookie("iDisplayLength", oSettings._iDisplayLength, { expires: 10 });
                //console.log($.riDisplayLength);

                //var info = $.rtable.page.info(); -- v1.10 
                //var info = $.rtable.fnSettings(); -- not working in version 1.9

                $.rpage = oSettings._iDisplayLength === -1 ? 0 : Math.ceil(oSettings._iDisplayStart / oSettings._iDisplayLength);
                //$.rpage = oSettings._iDisplayLength === 0 ? 0 : Math.ceil(oSettings._iDisplayStart / oSettings._iDisplayLength);

                container.html(data);

                runSparkLine();
                                
            }

        },
        async: true
    });

}

function checkReportUrl(url)
{
    var currenturl = window.location.hash.substring(1);

    var url_array = currenturl.split("?");
    curl_array = url_array[0].split("/");
    //console.log(curl_array);

    url_array = url.split("?");
    rurl_array = url_array[0].split("/");
    //console.log(rurl_array);

    return rurl_array[3] == curl_array[3] && rurl_array[2] == curl_array[2];
}


//function clearReport() {
//    clearTimeout($.rtimer);
//    clearTimeout(rptimer);
//    //if ($.rfirstin) {
//    // // set default opts
//    //    $.rcolindex = 0;
//    //    $.rsortdir = 'asc';
//    //    $.rpage = 0;
//    //    $.rcolindexheader = '';
        
//    //}
   
//    $.rfirstin = true;
//    $.rcolindex = 0;
//    $.rsortdir = 'asc';
//    $.rpage = 0;
//    $.rcolindexheader = undefined;
    
//}








/*********************** TICKERS ************************/
function tickeranime(ticker) {
    var width = ticker.width(),
        offset = ticker.parent().width(),
        animateto = ticker.data("dir") == "True" ? offset : -width,
        duration = ((width + offset) * 7) / ((ticker.data('speed') / 100));

    ticker.animate({ right: animateto }, duration, 'linear', function () {
        tickersetu(ticker);
       // ticker.trigger('ticker');
        //count the animations
        tickerAnimation--;
        //console.log(tickerAnimation);
        if (tickerAnimation < 0) {
            location.reload();
        }
    });
}
//for signal R
//function updateTickerSignalr(ticker) {
//    $.ajax({
//        type: "GET",
//        url: '@Url.Action("buildticker", "helper")?&tickerid=' + ticker.data('tickerid'),
//        success: function (data) {
//            // alert("refresh");
//            //ticker.html(tickertransform(data));
//            ticker.html(tickerformat(data));
//            // set the colorconfig
//            //setTickerColors();
//            tickerColor(ticker);

//           // ticker.trigger('ticker');
//        },
//        //  async: true
//    });
//}

function ticker_update_start() {

    var $tickers = $('.ticker');
    $tickers.bind({
        'ticker': function () {
            $this = $(this);
            var width = $this.width();

            if ($this.data("dir") == "True") {
                $this.css({ right: -width }); //original
            }
            else {
                $this.css({ right: $this.parent().width() }); // test
            }
            tickeranime($(this));
        },
        'mouseover': function () {
            $(this).pause();
            //$this.stop(true);
        },
        'mouseout': function () {
            //$(this).TickerAnimate();
            //tickeranime($(this));
            $(this).resume();
        }

    });

    $tickers.each(function () {

        //$(this).TickerSetup();
        tickersetu($(this));
       // updateTickerSignalr($(this));
        //tickerrefresh($(this));
    });
}

function tickerColor(ticker) {

    ticker.parent().css('backgroundColor', ticker.data("bgcolor"));
    ticker.find('.c').css('color', ticker.data("cpcolor"));
    ticker.find('.im').css('color', ticker.data("imcolor"));
    ticker.find('.cl').css('color', ticker.data("ckcolor"));
    ticker.find('.co').css('color', ticker.data("cvcolor"));
   
}

function updateSparkPos()
{
    // set the top for the title
    var tcount = $('#tkcontent .ticker').length;
    
    var ttopstyle = 0 - (tcount * 105 + 165);
    $(".tspark").css("top", ttopstyle);

    return ttopstyle;
    
}

function tickerformat(data) {
    var result = '',
        aux = '',
        tmp = 0,
        arrow = '';
        
    var ticker_obj = JSON.parse(data);

    $.each(ticker_obj[1], function (i, data) {

        //result += '<td><table><tr><td><div class="c">' + data.CampaignName + '</div></td></tr>';
        result += '<td><table><tr><td><div class="c">' + data.Title + '</div></td></tr>';

        if (ticker_obj[0] % 2 == 1) {
            if (data.OldImpressions != data.Impressions) {
                //arrow = data.OldImpressions > data.Impressions ? ' <i class="fa fa-sort-asc red"></i>' : ' <i class="fa fa-sort-desc green"></i>';
                //arrow = data.OldImpressions > data.Impressions ? ' <i class="fa fa-sort-desc"></i>' : ' <i class="fa fa-sort-asc"></i>';
                //arrow = data.OldImpressions > data.Impressions ? '<td class="arrow"><i class="fa fa-sort-desc"></i></td>' : '<td class="arrow"><i class="fa fa-sort-asc"></i></td>';
                arrow = data.OldImpressions > data.Impressions ? '<td class="arrow desc"></td>' : '<td class="arrow asc"></td>';
                //arrow = data.OldImpressions > data.Impressions ? '<td class="arrow desc"><img src="@Url.Content("~/Images/arrow-down.png")"/></td>' : '<td class="arrow asc"><img src="@Url.Content("~/Images/arrow-up.png")"/></td>';
            }
            //aux += '<td class="im"><div class="val">' + data.Impressions + arrow + '</div><div class="lb">IMPRESSIONS</div></td>';
            aux += arrow + '<td class="im"><div class="val">' + data.Impressions + '</div><div class="lb">IMPRESSIONS</div></td>';
            arrow = '';
        }

        if ((ticker_obj[0] >> 1) % 2 == 1) {
            if (data.OldClicks != data.Clicks) {
                arrow = data.OldClicks > data.Clicks ? '<td class="arrow desc"></td>' : '<td class="arrow asc"></td>';
            }
            aux += arrow + '<td class="cl"><div class="val">' + data.Clicks + '</div><div class="lb">CLICKS</div></td>';
            arrow = '';
        }

        if ((ticker_obj[0] >> 2) % 2 == 1) {
            if (data.OldConversions != data.Conversions) {
                arrow = data.OldConversions > data.Conversions ? '<td class="arrow desc"></td>' : '<td class="arrow asc"></td>';
            }
            aux += arrow + '<td class="co"><div class="val">' + data.Conversions + '</div><div class="lb">CONVERSIONS</div></td>';
            arrow = '';
        }

        if ((ticker_obj[0] >> 3) % 2 == 1) { // Cost
            //if (data.OldConversions != data.Conversions) {
            //    arrow = data.OldConversions > data.Conversions ? '<td class="arrow desc"></td>' : '<td class="arrow asc"></td>';
            //}
            //aux += arrow + '<td class="co"><div class="val">' + data.Conversions + '</div><div class="lb">CONVERSIONS</div></td>';
            aux += '<td><div class="val">$' + data.Cost.toFixed(2) + '</div><div class="lb">Cost</div></td>';
            //arrow = '';
        }

        if ((ticker_obj[0] >> 4) % 2 == 1) { // Revenue
            aux += '<td><div class="val">$' + data.Revenue.toFixed(2) + '</div><div class="lb">Revenue</div></td>';
        }

        /*======================= COST ==============================*/

        if ((ticker_obj[0] >> 5) % 2 == 1) { // CPC
            
            tmp = data.Clicks == 0 ? data.Cost : data.Cost / data.Clicks;
            //console.log(cpc);
            aux += '<td><div class="val">$' + tmp.toFixed(2) + '</div><div class="lb">CPC</div></td>';
            
        }

        if ((ticker_obj[0] >> 6) % 2 == 1) { // CPS

            tmp = data.Conversions == 0 ? data.Cost : data.Cost / data.Conversions;
            aux += '<td><div class="val">$' + tmp.toFixed(2) + '</div><div class="lb">CPS</div></td>';

        }

        if ((ticker_obj[0] >> 7) % 2 == 1) { // CPM
            tmp = data.Impressions == 0 ? data.Cost : data.Cost / (data.Impressions * 1000);
            aux += '<td><div class="val">$' + tmp.toFixed(2) + '</div><div class="lb">CPM</div></td>';
        }
        /*===========================================================*/

        if ((ticker_obj[0] >> 8) % 2 == 1) { // RPC
            tmp = data.Clicks == 0 ? data.Revenue : data.Revenue / data.Clicks;
            aux += '<td><div class="val">$' + tmp.toFixed(2) + '</div><div class="lb">RPC</div></td>';
        }

        if ((ticker_obj[0] >> 9) % 2 == 1) { // RPS
            tmp = data.Conversions == 0 ? data.Revenue : data.Revenue / data.Conversions;
            aux += '<td><div class="val">$' + tmp.toFixed(2) + '</div><div class="lb">RPS</div></td>';
        }

        if ((ticker_obj[0] >> 10) % 2 == 1) { // RPM
            tmp = data.Impressions == 0 ? data.Revenue : data.Revenue / (data.Impressions * 1000);
            aux += '<td><div class="val">$' + tmp.toFixed(2) + '</div><div class="lb">RPM</div></td>';
        }


        if (aux !== '') {
            result += '<tr><td><table><tr>' + aux + '</tr></table></td></tr></table></td>';
            aux = '';
        }
        else {
            result += '</table></td>';
        }

    })

    return '<table><tr>' + result + '</tr></table>';
};

/*********************** END TICKERS ************************/

/*********************** DATATABLES ************************/
function loadDataTables() {
    loadScript("/Content/js/plugin/datatables/jquery.dataTables.min.js", function () {
        //loadScript("/Content/js/plugin/datatables/dataTables.colVis.min.js", function () {
            //loadScript("/Content/js/plugin/datatables/dataTables.tableTools.min.js", function () {
                loadScript("/Content/js/plugin/datatables/dataTables.bootstrap.min.js", function () {
                    loadScript("/Content/js/plugin/datatable-responsive/datatables.responsive.nightly.min.js", runDataTables)
                });
            //});
        //});
    });
}

/*********************** END DATATABLES ************************/

/************************* API REPORT ******************************/

// report vars
var count = 0;
var calls = 0;
var rptimer = null;
var spktimer = null;
var skmodel = null
var oldjson = null;
var compare = false;
var apiurl = null;
var reporturl = null;
var table = null;
var clickchange = false;
var convchange = false;
var getURL = function (u) { }; // will be overriden en each report

var setReportTimer = function () {
    //clearTimeout(spktimer); // override the spark timer
    clearReport();

    //compare = false;
    //console.log(compare);

    if (calls == 0) {
        rptimer = setInterval(function () {

            //console.log(count);
            //console.log(compare);

            // load the spark and then load the table
            $.ajax({
                url: "/api/sparks/get",
                dataType: 'json',
                async: true,
                success: function (data) {

                    table.ajax.reload(function (json) {
                        oldjson = json;

                        // DEBUG                   

                        var doplay = $.cookie("ct_sound");
                        var doplayc = $.cookie("ct_cv_sound");

                        if (clickchange && doplay == 1) {
                            // play click
                            clickchange = false;
                            playClick();
                        }
                        if (convchange && doplayc == 1) {
                            //play conversion
                            convchange = false;
                            playConversion();
                        }

                        compare = true; // never hightlight the reports 
                        skmodel.setData(data); // load the sparkmodel 
                    }, false); // user paging is not reset on reload

                }
            });
                  

        }, reportRefreshTime); // reportRefreshTime
    }
    
};



var updateHash = function (url) {
    doAjax = 0;
    window.location.hash = url;
}

var refreshTable = function (togglecallback) {
    calls += 1;    
    clearReport(); // clear all the timers
    updateHash(getURL(reporturl));
    
    table.ajax.url(getURL(apiurl)).load(function (json) {
        

        if (togglecallback) {
            toggleColumn(togglecallback);
        }

        calls = calls - 1;
       
        setReportTimer();
        
        
    }, true);
}

var clearReport = function () {
    clearTimeout(rptimer);
    clearTimeout(spktimer);
    rptimer = null;
    spktimer = null;
    oldjson = null;
    compare = false;
}
 
var toggleColumn = function (colname) {

    var header = $(table.column(colname + ":name").header());

    header.toggleClass('never');
    header.toggleClass(header.data('class'));
    

    table.responsive.rebuild();
    table.responsive.recalc();
}

/*********************** SPARK ************************/

function runSparkLine() {

    if ($.fn.sparkline) {

        $('#content .sparkline').each(function () {

            if (!$(this).find("canvas").length > 0) {

                var barColor,
                    sparklineHeight,
                    sparklineBarWidth,
                    sparklineBarSpacing,
                    sparklineNegBarColor,
                    sparklineStackedColor
                ;

                var $this = $(this);
                var sparklineType = 'bar';

                barColor = $this.data('sparkline-bar-color') || $this.css('color') || '#0000f0';
                sparklineHeight = $this.data('sparkline-height') || '26px';
                sparklineBarWidth = $this.data('sparkline-barwidth') || 5;
                sparklineBarSpacing = $this.data('sparkline-barspacing') || 2;
                sparklineNegBarColor = $this.data('sparkline-negbar-color') || '#A90329';
                sparklineStackedColor = $this.data('sparkline-barstacked-color') || ["#A90329", "#0099c6", "#98AA56", "#da532c", "#4490B1", "#6E9461", "#990099", "#B4CAD3"];

                $this.sparkline('html', {
                    barColor: barColor,
                    type: sparklineType,
                    height: sparklineHeight,
                    barWidth: sparklineBarWidth,
                    barSpacing: sparklineBarSpacing,
                    stackedBarColor: sparklineStackedColor,
                    negBarColor: sparklineNegBarColor,
                    zeroAxis: 'false'
                });


            }
        });
    }
}

window.onresize = function () {
    runSparkLine();

}

var Spark = function () {
    var self = this;

    self.revenue = ko.observableArray();
    self.clicks = ko.observableArray();
    self.conversions = ko.observableArray();
    //self.oldclicks = null;

    self.arrow = ko.pureComputed(function () {
        self = this;
        var arr = self.clicks();
        return arr[arr.length - 1] < arr[arr.length - 2] ? "fa fa-arrow-circle-down" : "fa fa-arrow-circle-up";
    }, self);

    //self.todayrevenue = ko.pureComputed(function () {
    //    self = this;
    //    var arr = self.revenue();
    //    if (arr.length > 0) {
    //        return '$' + arr[arr.length - 1];
    //    }
    //    return '';        
    //}, self);

    self.loadData();
}

Spark.prototype = {

    setData: function(data) {
        var self = this;

        self.clicks.removeAll();
        self.revenue.removeAll();
        self.conversions.removeAll();

        $(data).each(function (index, element) {
            self.clicks.push(element.Clicks);
            self.revenue.push(element.Revenue.toFixed(2));
            self.conversions.push(element.Conversions);

            //console.log(element.Revenue);
        });
        runSparkLine();
    },

    loadData: function () {
        var self = this;
        
        $.ajax({
            url: "/api/sparks/get",
            dataType: 'json',
            async: true,
            success: function (data) {
                //stuff
                //...

                //var oldclicks = self.clicks.pop();
                //var oldconversions = self.conversions.pop();
                
                self.clicks.removeAll();
                self.revenue.removeAll();
                self.conversions.removeAll();
               
                $(data).each(function (index, element) {
                    self.clicks.push(element.Clicks);
                    self.revenue.push(element.Revenue.toFixed(2));
                    self.conversions.push(element.Conversions);
                    
                    //console.log(element.Revenue);
                });

                //var nclicks = self.clicks()[self.clicks().length - 1];
                //var nconv = self.conversions()[self.conversions().length - 1];

                //var doplay = $.cookie("ct_sound");
               
                //if (doplay == 1) {

                //    // compare old values and play the sound
                //    if (oldclicks < nclicks) {
                //        // play sound
                //        //console.log("play click");

                //        playSound('nclick1.mp3');

                //    }
                //    if (oldconversions < nconv) {
                //        // play sound
                //        //console.log("oldconversions:" + oldconversions + " nconv: " + nconv);
                //        playSound('nconv.mp3');

                //    }

                //}
                
                
                // save old data
                //self.oldclicks = self.clicks()[self.clicks().length - 1];
                //self.oldconversions = self.oldconversions()[self.oldconversions().length - 1];
               

                runSparkLine();

            }
        });

    }
}

function playClick() {
    playSound('nclick1.mp3');
}

function playConversion() {
    playSound('nconv.mp3');
}

function playSound (soundfile) {

    var audioElement = document.createElement('audio');
    audioElement.setAttribute('src', '/Content/sound/' + soundfile);
    audioElement.setAttribute('autoplay', 'autoplay');
    //audioElement.load()
    
    //$.get();

    //audioElement.addEventListener("load", function () {
    //    audioElement.play();
    //}, true);

    //audioElement.pause();
    audioElement.play();

};

var setSparkTimer = function (spakMoldel) {
    spktimer = setInterval(function () {

        spakMoldel.loadData();

    }, reportRefreshTime); // reportRefreshTime
};

