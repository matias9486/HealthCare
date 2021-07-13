﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//código agregado para filtrar las tablas y cambiarle el lenguaje a español
$(document).ready(function () {
    $('#miTabla').DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
        }
    });

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



//ajax para obtener lista de patologias filtradas
const getPatologias = () => {

    let config = {
        type: 'get',    //puedo usar get  y pasar el id en la url.. o post y usar el atributo data para enviar el id        
        url: "/Patologias/GetAll", // Url                
        data: {
            Tipo: "1", // Parámetros            
        },
        dataType: 'json',

        success: (respuesta) => {                     
            $.each(respuesta, (indice, elemento) => {                
                alert(elemento.nombre);                
            });    
        },        
        error: () => {
            alert("Error al ejecutar la api");
        }
    };    
    $.ajax(config);
}

