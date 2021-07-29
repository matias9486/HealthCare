
$(document).ready(function () {
    //Peticion a API
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",        
        url:'/Dashboards/DataPastel',
        error: function () {
            alert("Ocurrio un error al consultar los datos");
        },
        success: function (data) {
            GraficaPastel(data);
        }
    })
});

function GraficaPastel(data) {
    // Build the chart
    Highcharts.chart('pastel', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: 'Informe de tratamientos usados'
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: false
                },
                showInLegend: true
            }
        },
        series: [{
            name: 'Tratamientos',
            colorByPoint: true,
            data: data
        }]
    });

}



   