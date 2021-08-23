
$(document).ready(function () {
    

    function GraficaPastel(data) {
        // Build the chart
        Highcharts.chart('pastel', {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie',
                backgroundColor: {
                    linearGradient: { x1: 0, y1: 0, x2: 1, y2: 1 },
                    stops: [
                        [0, 'rgb( 30, 132, 73,0.5 )'],
                        [1, 'rgb( 187, 233, 94,0.5)']
                    ]
                },

            },

            title: {

                text: '',
                style: {
                    color: 'white',
                    fontWeight: 'bold',
                    fontSize: '1.5em'
                }
            },
            tooltip: {
                //headerFormat: '<small>{point.key}</small>',

                pointFormat: '{series.name} <b>{point.name}</b>: <b>{point.percentage:.1f}%</b>',
                backgroundColor: '#FCFFC5',
                borderColor: 'black',
                style: {
                    color: 'black',
                    fontWeight: 'bold',
                    fontSize: '1.2em'
                }
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
                        enabled: true,
                        style: {

                            color: 'white ',
                            fontWeight: 'bold',
                            fontSize: '1.2em'
                        },
                        //format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        format: '<b>{point.percentage:.1f} %</b>',

                    },
                    showInLegend: true
                }
            },
            series: [{
                name: data.nombre,
                colorByPoint: true,
                data: data.lista
            }],
            legend: {
                itemStyle: {
                    color: 'white',
                    fontWeight: 'bold',
                    fontSize: '1.2em'
                }
            },

        });

    }



    //------------- filtrar por fecha------------------
    $("#filtrar").click(function () {
        var inicial = $(fechaInicial).val();
        var final = $(fechaFinal).val();

        if (inicial.length > 0 && final.length > 0)
        {
            $.ajax({
                data: {
                    inicial, final,
                },
                type: "GET",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: '/Dashboards/graficaFiltrada',
                error: function () {
                    alert("Ocurrio un error al consultar los datos");
                },
                success: function (data) {
                    $("#msjFiltro").empty();
                    if (data.lista.length > 0)
                    {
                        GraficaPastel(data);                        
                    }
                    else
                    {
                        $("#msjFiltro").empty();
                        $("#pastel").empty();
                        $("#msjFiltro").append('<div class="alert alert-warning alert-dismissible fade show" role="alert">No existen registros para las fechas ingresadas<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div > ');            
                    }
                }
            });    

        }
        else
        {
            $("#msjFiltro").empty();
            $("#pastel").empty();
            $("#msjFiltro").append('<div class="alert alert-warning alert-dismissible fade show" role="alert">Seleccione las fechas para filtrar los datos<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div > ');            
        }
        
    });
});





   