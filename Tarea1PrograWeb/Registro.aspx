<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="Tarea1PrograWeb.Registro" ValidateRequest="false" %>
<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">Registro</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Registro de préstamo</h1>
    <p class="nota">Todos los campos son obligatorios. La fecha prevista debe ser igual o posterior a la fecha de préstamo. Un equipo en estado Prestado no se puede prestar otra vez.</p>
    <div id="divMensaje" class="mensaje" role="status" aria-live="polite"></div>
    <div class="panel">
        <div class="rejilla">
            <div>
                <label for="txtNombre">Nombre del solicitante</label>
                <asp:TextBox ID="txtNombre" runat="server" MaxLength="80" />
                <span id="errNombre" class="error-campo" role="alert"></span>
            </div>
            <div>
                <label for="txtDepartamento">Departamento</label>
                <asp:TextBox ID="txtDepartamento" runat="server" MaxLength="60" />
                <span id="errDepartamento" class="error-campo" role="alert"></span>
            </div>
            <div class="campo-ancho">
                <label for="ddlEquipo">Equipo</label>
                <asp:DropDownList ID="ddlEquipo" runat="server" />
                <span id="errEquipo" class="error-campo" role="alert"></span>
            </div>
            <div>
                <label for="txtFechaPrestamo">Fecha de préstamo</label>
                <asp:TextBox ID="txtFechaPrestamo" runat="server" TextMode="Date" />
                <span id="errFechaPrestamo" class="error-campo" role="alert"></span>
            </div>
            <div>
                <label for="txtFechaPrevista">Fecha prevista de devolución</label>
                <asp:TextBox ID="txtFechaPrevista" runat="server" TextMode="Date" />
                <span id="errFechaPrevista" class="error-campo" role="alert"></span>
            </div>
            <div class="campo-ancho">
                <label for="txtMotivo">Motivo</label>
                <asp:TextBox ID="txtMotivo" runat="server" TextMode="MultiLine" MaxLength="300" Rows="4" />
                <span id="errMotivo" class="error-campo" role="alert"></span>
            </div>
        </div>
        <p>
            <button type="button" id="btnRegistrar">Registrar préstamo</button>
        </p>
    </div>
</asp:Content>
<asp:Content ID="ContentScripts" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="<%= ResolveUrl("~/Scripts/registro.js") %>"></script>
</asp:Content>
