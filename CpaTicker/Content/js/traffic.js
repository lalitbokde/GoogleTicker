function rundaily(url, callback) {

    url = typeof url !== 'undefined' && url != null ? '/chart/gettraffic/?' + url : '/chart/gettraffic';
    var chartopts = gopts;
    chartopts.series[0].name = 'Clicks';
    chartopts.series[1].name = 'Conversions';
    chartopts.series[0].color = '#eb6d5e';
    chartopts.series[1].color = '#7bc1be';
    chartopts.chart.renderTo = 'dailychart';
    chartopts.title.text = 'Traffic Summary';
    chartopts.yAxis.labels.formatter = null;
    chartopts.tooltip.formatter = function () {
        var s = '<span style"font-size:9px;font-weight:bold;">' + this.x + '</span>';
        $.each(this.points, function (i, point) {
            s += '<br /><span style="font-weight:bold;color:' + point.series.color + ';">' + point.series.name + '</span>: <strong>' + point.y + '</strong>';
        });
        return s;
    };
    // not working in ie 10
    //$.getJSON(url, function (data) {
    //    chartopts.xAxis.categories = data.Xaxis;
    //    chartopts.series[0].data = data.Serie1;
    //    chartopts.series[1].data = data.Serie2;
    //    dailychart = new Highcharts.Chart(chartopts);

    //    if (callback) {
    //        callback();
    //    }
    //});

    // not working either in ie 10
    $.get(url, function (data) {

        var obj = JSON.parse(data);

        chartopts.xAxis.categories = obj.Xaxis;
        chartopts.series[0].data = obj.Serie1;
        chartopts.series[1].data = obj.Serie2;
        dailychart = new Highcharts.Chart(chartopts);

        if (callback) {
            callback();
        }
    });
}

function rungross(url, callback) {
    url = typeof url !== 'undefined' && url != null ? '/chart/GetRevenue/?' + url : '/chart/GetRevenue';
    var grossopts = gopts;

    grossopts.series[0].name = 'Cost';
    grossopts.series[1].name = 'Revenue';
    grossopts.series[0].color = '#43b7e6';
    grossopts.series[1].color = '#81bf27';
    grossopts.chart.renderTo = 'grossincome';
    grossopts.title.text = 'Gross Cost / Revenue';
    grossopts.yAxis.labels.formatter = function () {
        return '$' + Highcharts.numberFormat(this.value, 2);
    };
    grossopts.tooltip.formatter = function () {
        var s = '<span style"font-size:9px;font-weight:bold;">' + this.x + '</span>';
        $.each(this.points, function (i, point) {
            s += '<br /><span style="font-weight:bold;color:' + point.series.color + ';">' + point.series.name + '</span>: <strong>$' + point.y.toFixed(2) + '</strong>';
        });
        return s;
    };

    $.getJSON(url, function (data) {
        grossopts.xAxis.categories = data.Xaxis;
        grossopts.series[0].data = data.Serie1;
        grossopts.series[1].data = data.Serie2;
        var chart = new Highcharts.Chart(grossopts);

        if (callback) {
            callback();
        }

    });
}

function runpie(url, callback) {
    url1 = typeof url !== 'undefined' && url != null ? '/chart/rbycampaign/?' + url : '/chart/rbycampaign';
    var copts = pieopts;
    copts.title = 'Revenue by Campaign';
    copts.chart.renderTo = 'piechartrevenue';


    $.getJSON(url1, function (data) {
        copts.series[0].data = data;
        dailychart = new Highcharts.Chart(copts);

        // make the second call to get the cost pie
        url2 = typeof url !== 'undefined' && url != null ? '/chart/cbycampaign/?' + url : '/chart/cbycampaign';
        copts.title = 'Cost by Campaign';
        copts.chart.renderTo = 'piechartcost';
        $.getJSON(url2, function (data) {
            copts.series[0].data = data;
            chart = new Highcharts.Chart(copts);

            if (callback) {
                callback();
            }
        });
    });
}


var dailychart;
var gopts = {
    chart: {
        renderTo: 'dailychart',
        type: 'spline'
    },
    title: {
        text: 'Traffic Summary'
    },
    xAxis: {
        categories: []
    },
    yAxis: {
        plotLines: [{ value: 0, width: 1, color: '#ccc' }],
        labels: {
            formatter: null
        }
    },
    tooltip:
    {
        formatter: null,
        shared: true
    },
    legend:
        {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
    credits: {
        enabled: false
    },
    series: [
        {
            name: 'Clicks',
            data: [],
            color: '#eb6d5e'
        },
        {
            name: 'Conversions',
            data: [],
            color: '#7bc1be'
        }]
};

var pieopts = {
    chart:
        {
            renderTo: 'piechartrevenue',
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
    title:
        {
            text: 'Revenue by Campaign'
        },
    tooltip:
        {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
    plotOptions:
        {
            pie:
                {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels:
                        {
                            enabled: true,
                            color: '#000',
                            connectorColor: '#000',
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                        }
                }
        },
    legend:
        {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
    credits: false,
    series: [
        {
            type: 'pie',
            name: 'Revenue by Campaign',
            data: []//[['c1', 0.00], ['c2', 0.00], ['c3', 0.00], ['c4', 0.00], ['c5', 0.00]]

        }]
};

















