<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Procesamiento.aspx.cs" Inherits="Tarea1PrograWeb.Procesamiento" ValidateRequest="false" %>
<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">Procesamiento</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Procesamiento de devoluciones</h1>
    <p class="nota">Elija la fecha real de devolución y pulse Registrar devolución en la fila. El estado pasa de Prestado a Devuelto. El resumen sale de la misma lista en memoria: no es otra función del menú.</p>
    <div id="divMensajeProceso" class="mensaje" role="status" aria-live="polite"></div>
    <section id="divResumen" class="resumen" aria-label="Resumen de la sesión">
        <div>
            <span>Activos (Prestado)</span>
            <strong id="spanActivos">0</strong>
        </div>
        <div>
            <span>Devueltos</span>
            <strong id="spanDevueltos">0</strong>
        </div>
        <div>
            <span>Total de la sesión</span>
            <strong id="spanTotal">0</strong>
        </div>
    </section>
    <div class="barra-acciones">
        <div class="campo-fecha">
            <label for="txtFechaDevolucion">Fecha de devolución</label>
            <asp:TextBox ID="txtFechaDevolucion" runat="server" TextMode="Date" />
        </div>
        <button type="button" id="btnActualizarProceso" class="boton-secundario">Actualizar panel</button>
    </div>
    <p id="pSinActivos" class="vacio" hidden>No hay equipos prestados en esta sesión.</p>
    <div class="tabla-contenedor panel">
        <table id="tblActivos">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Solicitante</th>
                    <th>Equipo</th>
                    <th>Fecha de préstamo</th>
                    <th>Fecha prevista</th>
                    <th>Acción</th>
                </tr>
            </thead>
            <tbody id="tbodyActivos"></tbody>
        </table>
    </div>
</asp:Content>
<asp:Content ID="ContentScripts" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="<%= ResolveUrl("~/Scripts/procesamiento.js") %>"></script>
</asp:Content>
