<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Consulta.aspx.cs" Inherits="Tarea1PrograWeb.Consulta" ValidateRequest="false" %>
<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">Consulta</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Consulta de préstamos</h1>
    <p class="nota">La tabla muestra solo los préstamos capturados en esta sesión. Actualizar vuelve a leer la lista en memoria sin recargar toda la página.</p>
    <div id="divMensajeConsulta" class="mensaje" role="status" aria-live="polite"></div>
    <p>
        <button type="button" id="btnActualizar" class="boton-secundario">Actualizar consulta</button>
    </p>
    <p id="pVacio" class="vacio" hidden>No hay préstamos en esta sesión.</p>
    <div class="tabla-contenedor panel">
        <table id="tblPrestamos">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Solicitante</th>
                    <th>Departamento</th>
                    <th>Equipo</th>
                    <th>Fecha de préstamo</th>
                    <th>Fecha prevista</th>
                    <th>Motivo</th>
                    <th>Estado</th>
                    <th>Fecha de devolución</th>
                </tr>
            </thead>
            <tbody id="tbodyPrestamos"></tbody>
        </table>
    </div>
</asp:Content>
<asp:Content ID="ContentScripts" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="<%= ResolveUrl("~/Scripts/consulta.js") %>"></script>
</asp:Content>
