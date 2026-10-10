using System;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tarea1PrograWeb.Models;
using Tarea1PrograWeb.Services;

namespace Tarea1PrograWeb
{
    public partial class Registro : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EnlazarCatalogo();
            }
        }

        private void EnlazarCatalogo()
        {
            ddlEquipo.Items.Clear();
            ddlEquipo.Items.Add(new ListItem("Seleccione un equipo", ""));
            foreach (Equipo equipo in PrestamoRepositorio.ObtenerCatalogo())
            {
                bool prestado = PrestamoRepositorio.EquipoEstaPrestado(equipo.Id);
                string texto = equipo.Codigo + " — " + equipo.Nombre;
                if (prestado)
                {
                    texto += " (Prestado)";
                }

                var item = new ListItem(texto, equipo.Id.ToString());
                item.Attributes["data-prestado"] = prestado ? "1" : "0";
                if (prestado)
                {
                    item.Attributes["disabled"] = "disabled";
                }

                ddlEquipo.Items.Add(item);
            }
        }

        [WebMethod(EnableSession = true)]
        public static ResultadoAjax ObtenerCatalogo()
        {
            return PrestamoRepositorio.CatalogoConEstado();
        }

        [WebMethod(EnableSession = true)]
        public static ResultadoAjax GuardarPrestamo(
            string nombre,
            string departamento,
            int equipoId,
            string fechaPrestamo,
            string fechaPrevista,
            string motivo)
        {
            return PrestamoRepositorio.Registrar(nombre, departamento, equipoId, fechaPrestamo, fechaPrevista, motivo);
        }
    }
}
