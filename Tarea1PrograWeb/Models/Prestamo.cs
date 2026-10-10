using System;

namespace Tarea1PrograWeb.Models
{
    [Serializable]
    public class Prestamo
    {
        public int Id { get; set; }
        public string NombreSolicitante { get; set; }
        public string Departamento { get; set; }
        public int EquipoId { get; set; }
        public string EquipoCodigo { get; set; }
        public string EquipoNombre { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaPrevistaDevolucion { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaDevolucion { get; set; }
    }
}
