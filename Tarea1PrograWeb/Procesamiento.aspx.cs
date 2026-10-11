using System;
using System.Globalization;
using System.Web.Services;
using System.Web.UI;
using Tarea1PrograWeb.Models;
using Tarea1PrograWeb.Services;

namespace Tarea1PrograWeb
{
    public partial class Procesamiento : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && txtFechaDevolucion != null && string.IsNullOrEmpty(txtFechaDevolucion.Text))
            {
                txtFechaDevolucion.Text = DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
        }

        [WebMethod(EnableSession = true)]
        public static ResultadoAjax ObtenerPanel()
        {
            return PrestamoRepositorio.Panel();
        }

        [WebMethod(EnableSession = true)]
        public static ResultadoAjax RegistrarDevolucion(int id, string fechaDevolucion)
        {
            return PrestamoRepositorio.RegistrarDevolucion(id, fechaDevolucion);
        }
    }
}
