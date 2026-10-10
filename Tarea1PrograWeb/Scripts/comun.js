window.Mesa = window.Mesa || {};

Mesa.ajax = function (url, datos) {
    return $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(datos || {}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false
    });
};

Mesa.dato = function (respuesta) {
    if (respuesta && Object.prototype.hasOwnProperty.call(respuesta, "d")) {
        return respuesta.d;
    }
    return respuesta;
};

Mesa.esFechaIso = function (valor) {
    if (!/^\d{4}-\d{2}-\d{2}$/.test(valor || "")) {
        return false;
    }
    var partes = valor.split("-");
    var anio = parseInt(partes[0], 10);
    var mes = parseInt(partes[1], 10);
    var dia = parseInt(partes[2], 10);
    var fecha = new Date(Date.UTC(anio, mes - 1, dia));
    return fecha.getUTCFullYear() === anio &&
        fecha.getUTCMonth() === mes - 1 &&
        fecha.getUTCDate() === dia;
};

Mesa.escapar = function (texto) {
    return $("<div>").text(texto == null ? "" : String(texto)).html();
};

Mesa.hoyIso = function () {
    var ahora = new Date();
    var mes = String(ahora.getMonth() + 1);
    var dia = String(ahora.getDate());
    if (mes.length < 2) { mes = "0" + mes; }
    if (dia.length < 2) { dia = "0" + dia; }
    return ahora.getFullYear() + "-" + mes + "-" + dia;
};

Mesa.mostrar = function (selector, texto, esExito) {
    var caja = $(selector);
    caja.removeClass("mensaje-ok mensaje-error visible");
    if (!texto) {
        caja.text("");
        return;
    }
    caja.addClass("visible");
    caja.addClass(esExito ? "mensaje-ok" : "mensaje-error");
    caja.text(texto);
};

$(function () {
    $("#formPrincipal").on("submit", function (evento) {
        evento.preventDefault();
    });
});
