(function ($) {
    function pintarResumen(resumen) {
        var datos = resumen || { Activos: 0, Devueltos: 0, Total: 0 };
        $("#spanActivos").text(datos.Activos);
        $("#spanDevueltos").text(datos.Devueltos);
        $("#spanTotal").text(datos.Total);
    }

    function pintarActivos(prestamos) {
        var cuerpo = $("#tbodyActivos");
        cuerpo.empty();
        if (!prestamos || prestamos.length === 0) {
            $("#pSinActivos").prop("hidden", false);
            $("#tblActivos").prop("hidden", true);
            return;
        }
        $("#pSinActivos").prop("hidden", true);
        $("#tblActivos").prop("hidden", false);
        $.each(prestamos, function (_, prestamo) {
            var id = parseInt(prestamo.Id, 10);
            var boton = $("<button>", {
                type: "button",
                id: "btnDevolver-" + id,
                "class": "btnDevolver",
                text: "Registrar devolución"
            });
            boton.attr("data-id", String(id));
            boton.attr("data-fecha-prestamo", prestamo.FechaPrestamo || "");
            var fila = $("<tr>");
            fila.append($("<td>").text(id));
            fila.append($("<td>").text(prestamo.NombreSolicitante || ""));
            fila.append($("<td>").text((prestamo.EquipoCodigo || "") + " — " + (prestamo.EquipoNombre || "")));
            fila.append($("<td>").text(prestamo.FechaPrestamo || ""));
            fila.append($("<td>").text(prestamo.FechaPrevistaDevolucion || ""));
            fila.append($("<td>").append(boton));
            cuerpo.append(fila);
        });
    }

    function aplicarPanel(dato) {
        pintarActivos((dato && dato.Prestamos) || []);
        pintarResumen(dato && dato.Resumen);
    }

    function cargarPanel() {
        var boton = $("#btnActualizarProceso");
        boton.prop("disabled", true);
        Mesa.ajax("Procesamiento.aspx/ObtenerPanel", {}).done(function (respuesta) {
            var dato = Mesa.dato(respuesta) || {};
            if (!dato.Exito) {
                Mesa.mostrar("#divMensajeProceso", dato.Mensaje || "No se pudo leer el panel.", false);
                return;
            }
            aplicarPanel(dato);
        }).fail(function () {
            Mesa.mostrar("#divMensajeProceso", "No se pudo completar la operación. La página no se recargó.", false);
        }).always(function () {
            boton.prop("disabled", false);
        });
    }

    function registrarDevolucion(boton) {
        var id = parseInt(boton.attr("data-id"), 10);
        var fechaPrestamo = boton.attr("data-fecha-prestamo") || "";
        var fechaDevolucion = $("#txtFechaDevolucion").val();

        if (!Mesa.esFechaIso(fechaDevolucion)) {
            Mesa.mostrar("#divMensajeProceso", "Indique la fecha de devolución (aaaa-mm-dd).", false);
            return;
        }
        if (Mesa.esFechaIso(fechaPrestamo) && fechaDevolucion < fechaPrestamo) {
            Mesa.mostrar("#divMensajeProceso", "La fecha de devolución debe ser igual o posterior a la fecha de préstamo.", false);
            return;
        }

        boton.prop("disabled", true);
        Mesa.ajax("Procesamiento.aspx/RegistrarDevolucion", {
            id: id,
            fechaDevolucion: fechaDevolucion
        }).done(function (respuesta) {
            var dato = Mesa.dato(respuesta) || {};
            if (!dato.Exito) {
                Mesa.mostrar("#divMensajeProceso", dato.Mensaje || "No se pudo registrar la devolución.", false);
                boton.prop("disabled", false);
                return;
            }
            aplicarPanel(dato);
            Mesa.mostrar("#divMensajeProceso", dato.Mensaje, true);
        }).fail(function () {
            boton.prop("disabled", false);
            Mesa.mostrar("#divMensajeProceso", "No se pudo completar la operación. La página no se recargó.", false);
        });
    }

    $(function () {
        if (!$("#txtFechaDevolucion").val()) {
            $("#txtFechaDevolucion").val(Mesa.hoyIso());
        }
        cargarPanel();
        $("#btnActualizarProceso").on("click", cargarPanel);
        $("#tbodyActivos").on("click", ".btnDevolver", function () {
            registrarDevolucion($(this));
        });
    });
}(jQuery));
