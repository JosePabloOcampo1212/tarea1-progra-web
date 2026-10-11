(function ($) {
    function claseEstado(estado) {
        if (estado === "Devuelto") {
            return "estado estado-devuelto";
        }
        return "estado estado-prestado";
    }

    function pintar(prestamos) {
        var cuerpo = $("#tbodyPrestamos");
        cuerpo.empty();
        if (!prestamos || prestamos.length === 0) {
            $("#pVacio").prop("hidden", false);
            $("#tblPrestamos").prop("hidden", true);
            return;
        }
        $("#pVacio").prop("hidden", true);
        $("#tblPrestamos").prop("hidden", false);
        $.each(prestamos, function (_, prestamo) {
            var equipo = Mesa.escapar(prestamo.EquipoCodigo) + " — " + Mesa.escapar(prestamo.EquipoNombre);
            var fila = [
                "<tr>",
                "<td>", prestamo.Id, "</td>",
                "<td>", Mesa.escapar(prestamo.NombreSolicitante), "</td>",
                "<td>", Mesa.escapar(prestamo.Departamento), "</td>",
                "<td>", equipo, "</td>",
                "<td>", Mesa.escapar(prestamo.FechaPrestamo), "</td>",
                "<td>", Mesa.escapar(prestamo.FechaPrevistaDevolucion), "</td>",
                "<td>", Mesa.escapar(prestamo.Motivo), "</td>",
                "<td><span class=\"", claseEstado(prestamo.Estado), "\">", Mesa.escapar(prestamo.Estado), "</span></td>",
                "<td>", Mesa.escapar(prestamo.FechaDevolucion || "—"), "</td>",
                "</tr>"
            ];
            cuerpo.append(fila.join(""));
        });
    }

    function cargar() {
        var boton = $("#btnActualizar");
        boton.prop("disabled", true);
        Mesa.ajax("Consulta.aspx/ListarPrestamos", {}).done(function (respuesta) {
            var dato = Mesa.dato(respuesta) || {};
            if (!dato.Exito) {
                Mesa.mostrar("#divMensajeConsulta", dato.Mensaje || "No se pudo consultar la sesión.", false);
                return;
            }
            Mesa.mostrar("#divMensajeConsulta", "", true);
            pintar(dato.Prestamos || []);
        }).fail(function () {
            Mesa.mostrar("#divMensajeConsulta", "No se pudo completar la operación. La página no se recargó.", false);
        }).always(function () {
            boton.prop("disabled", false);
        });
    }

    $(function () {
        cargar();
        $("#btnActualizar").on("click", cargar);
    });
}(jQuery));
