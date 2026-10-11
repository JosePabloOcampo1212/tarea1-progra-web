using System;
using System.Web.Services;
using System.Web.UI;
using Tarea1PrograWeb.Models;
using Tarea1PrograWeb.Services;

namespace Tarea1PrograWeb
{
    public partial class Consulta : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod(EnableSession = true)]
        public static ResultadoAjax ListarPrestamos()
        {
            return PrestamoRepositorio.Listar();
        }
    }
}
