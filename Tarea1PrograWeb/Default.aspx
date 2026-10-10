<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Tarea1PrograWeb.Inicio" %>
<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">Inicio</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Escritorio de préstamos de equipos</h1>
    <p class="lead">
        La mesa de TI del laboratorio presta laptops, proyectores y otros equipos durante el turno.
        Este escritorio cubre solo tres tareas: anotar el préstamo, consultar lo anotado en esta sesión y registrar la devolución.
    </p>
    <ul class="tarjetas">
        <li>
            <h2>Registro</h2>
            <p>Anota solicitante, departamento, equipo, fechas y motivo. Un equipo que ya está Prestado no se puede volver a prestar.</p>
            <a class="boton" href="<%= ResolveUrl("~/Registro.aspx") %>">Abrir registro</a>
        </li>
        <li>
            <h2>Consulta</h2>
            <p>Muestra la tabla de préstamos capturados en esta sesión y la actualiza sin recargar toda la página.</p>
            <a class="boton" href="<%= ResolveUrl("~/Consulta.aspx") %>">Abrir consulta</a>
        </li>
        <li>
            <h2>Procesamiento</h2>
            <p>Registra la devolución (Prestado pasa a Devuelto) y muestra el conteo de activos y devueltos.</p>
            <a class="boton" href="<%= ResolveUrl("~/Procesamiento.aspx") %>">Abrir procesamiento</a>
        </li>
    </ul>
</asp:Content>
