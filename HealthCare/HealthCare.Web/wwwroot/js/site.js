// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification

//código agregado para filtrar las tablas y cambiarle el lenguaje a español
$(document).ready(function () {
    $('#miTabla').DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
        }
    });


    //funcion para cambiar comportamiento segun resolucion
    function myFunction(x) {
        if (x.matches) { // If media query matches        
            $('.miClass').hide();
        } else {
            $('.miClass').show();
        }
    }

    var x = window.matchMedia("(max-width: 770px)")
    myFunction(x) // Call listener function at run time
    x.addListener(myFunction) // Attach listener function on state changes




    //funciones para filtrar combos
    $(".cmborigen").change(function () {
        fillCombo("cmbdestino", $(".cmborigen").val(), $(".cmborigen").data("id"));
        $(".cmbdestino").change();
        //$("#patologiasMostrar").show();
        $("#PatologiaId").prop("disabled", false);
    });

    function fillCombo(updateId, value, action) {
        $.getJSON(action
            + "/" + value,
            function (data) {
                $("." + updateId).empty();

                $.each(data, function (i, item) {
                    $("." + updateId).append("<option  value='"
                        + item.id + "'>" + item.nombre
                        + "</option >");
                });
            });
    }


    //agregado para reemplazar el input file 
    jQuery('input[type=file]').change(function () {
        var filename = jQuery(this).val().split('\\').pop();
        var idname = jQuery(this).attr('id');
        console.log(jQuery(this));
        console.log(filename);
        console.log(idname);
        jQuery('span.' + idname).next().find('span').html(filename);
    });

    //typed
    if ($(".text-slider").length == 1) {

        var typed_strings =
            $(".text-slider-items").text();

        var typed = new Typed(".text-slider", {
            strings: typed_strings.split(", "),
            typeSpeed: 50,
            loop: true,
            backDelay: 900,
            backSpeed: 30,
        });
    }

});

