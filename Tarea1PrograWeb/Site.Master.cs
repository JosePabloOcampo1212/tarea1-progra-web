using System;
using System.IO;
using System.Web.UI;

namespace Tarea1PrograWeb
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected string ClaseNav(string pagina)
        {
            string actual = Path.GetFileName(Request.Url.AbsolutePath);
            if (string.Equals(actual, pagina, StringComparison.OrdinalIgnoreCase))
            {
                return "nav-actual";
            }

            return string.Empty;
        }
    }
}
