using System.Collections.Generic;

namespace Tarea1PrograWeb.Models
{
    public class EquipoVista
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public bool Prestado { get; set; }
    }

    public class PrestamoVista
    {
        public int Id { get; set; }
        public string NombreSolicitante { get; set; }
        public string Departamento { get; set; }
        public int EquipoId { get; set; }
        public string EquipoCodigo { get; set; }
        public string EquipoNombre { get; set; }
        public string FechaPrestamo { get; set; }
        public string FechaPrevistaDevolucion { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; }
        public string FechaDevolucion { get; set; }
    }

    public class ResumenVista
    {
        public int Activos { get; set; }
        public int Devueltos { get; set; }
        public int Total { get; set; }
    }

    public class ResultadoAjax
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public List<EquipoVista> Equipos { get; set; }
        public List<PrestamoVista> Prestamos { get; set; }
        public ResumenVista Resumen { get; set; }

        public static ResultadoAjax Fallo(string mensaje)
        {
            return new ResultadoAjax
            {
                Exito = false,
                Mensaje = mensaje,
                Equipos = new List<EquipoVista>(),
                Prestamos = new List<PrestamoVista>()
            };
        }
    }
}
