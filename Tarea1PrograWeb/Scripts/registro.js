(function ($) {
    var nombreRegex = /^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ.'\- ]+$/;
    var departamentoRegex = /^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ0-9.'\- ]+$/;

    function limpiarErrores() {
        $("#errNombre, #errDepartamento, #errEquipo, #errFechaPrestamo, #errFechaPrevista, #errMotivo").text("");
    }

    function pintarCatalogo(equipos) {
        var select = $("#ddlEquipo");
        select.empty();
        select.append($("<option>", { value: "", text: "Seleccione un equipo" }));
        $.each(equipos || [], function (_, equipo) {
            var texto = equipo.Codigo + " — " + equipo.Nombre;
            if (equipo.Prestado) {
                texto += " (Prestado)";
            }
            var opcion = $("<option>", { value: String(equipo.Id), text: texto });
            opcion.attr("data-prestado", equipo.Prestado ? "1" : "0");
            if (equipo.Prestado) {
                opcion.prop("disabled", true);
            }
            select.append(opcion);
        });
    }

    function cargarCatalogo() {
        Mesa.ajax("Registro.aspx/ObtenerCatalogo", {}).done(function (respuesta) {
            var dato = Mesa.dato(respuesta);
            if (dato && dato.Equipos) {
                pintarCatalogo(dato.Equipos);
            }
        }).fail(function () {
            Mesa.mostrar("#divMensaje", "No se pudo cargar el catálogo de equipos.", false);
        });
    }

    function validar() {
        limpiarErrores();
        var valido = true;
        var nombre = $.trim($("#txtNombre").val());
        var departamento = $.trim($("#txtDepartamento").val());
        var motivo = $.trim($("#txtMotivo").val());
        var fechaPrestamo = $("#txtFechaPrestamo").val();
        var fechaPrevista = $("#txtFechaPrevista").val();
        var opcion = $("#ddlEquipo option:selected");
        var equipoId = parseInt($("#ddlEquipo").val(), 10);

        if (nombre.length < 3 || nombre.length > 80 || !nombreRegex.test(nombre)) {
            $("#errNombre").text("Indique el nombre del solicitante (mínimo 3 caracteres, solo letras).");
            valido = false;
        }
        if (departamento.length < 2 || departamento.length > 60 || !departamentoRegex.test(departamento)) {
            $("#errDepartamento").text("Indique el departamento (entre 2 y 60 caracteres).");
            valido = false;
        }
        if (!equipoId) {
            $("#errEquipo").text("Seleccione un equipo del catálogo.");
            valido = false;
        } else if (opcion.attr("data-prestado") === "1") {
            $("#errEquipo").text("Ese equipo ya está Prestado. Registre la devolución antes de prestarlo de nuevo.");
            valido = false;
        }
        if (!Mesa.esFechaIso(fechaPrestamo)) {
            $("#errFechaPrestamo").text("Indique una fecha de préstamo válida (aaaa-mm-dd).");
            valido = false;
        }
        if (!Mesa.esFechaIso(fechaPrevista)) {
            $("#errFechaPrevista").text("Indique una fecha prevista de devolución válida (aaaa-mm-dd).");
            valido = false;
        } else if (Mesa.esFechaIso(fechaPrestamo) && fechaPrevista < fechaPrestamo) {
            $("#errFechaPrevista").text("La fecha prevista de devolución debe ser igual o posterior a la fecha de préstamo.");
            valido = false;
        }
        if (motivo.length < 5 || motivo.length > 300) {
            $("#errMotivo").text("Indique el motivo (entre 5 y 300 caracteres).");
            valido = false;
        }
        return valido;
    }

    function registrar() {
        Mesa.mostrar("#divMensaje", "", false);
        if (!validar()) {
            Mesa.mostrar("#divMensaje", "Revise los campos marcados. No se envió el préstamo.", false);
            return;
        }

        var boton = $("#btnRegistrar");
        boton.prop("disabled", true);
        Mesa.ajax("Registro.aspx/GuardarPrestamo", {
            nombre: $.trim($("#txtNombre").val()),
            departamento: $.trim($("#txtDepartamento").val()),
            equipoId: parseInt($("#ddlEquipo").val(), 10),
            fechaPrestamo: $("#txtFechaPrestamo").val(),
            fechaPrevista: $("#txtFechaPrevista").val(),
            motivo: $.trim($("#txtMotivo").val())
        }).done(function (respuesta) {
            var dato = Mesa.dato(respuesta) || {};
            if (!dato.Exito) {
                Mesa.mostrar("#divMensaje", dato.Mensaje || "No se pudo registrar el préstamo.", false);
                if (dato.Equipos) {
                    pintarCatalogo(dato.Equipos);
                }
                return;
            }
            $("#txtNombre, #txtDepartamento, #txtMotivo").val("");
            $("#txtFechaPrestamo, #txtFechaPrevista").val("");
            pintarCatalogo(dato.Equipos || []);
            limpiarErrores();
            Mesa.mostrar("#divMensaje", dato.Mensaje, true);
        }).fail(function () {
            Mesa.mostrar("#divMensaje", "No se pudo completar la operación. La página no se recargó.", false);
        }).always(function () {
            boton.prop("disabled", false);
        });
    }

    $(function () {
        cargarCatalogo();
        $("#btnRegistrar").on("click", registrar);
        $("#formPrincipal").on("submit", function (evento) {
            evento.preventDefault();
            registrar();
        });
    });
}(jQuery));
