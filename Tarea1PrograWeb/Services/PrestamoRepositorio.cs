using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using Tarea1PrograWeb.Models;

namespace Tarea1PrograWeb.Services
{
    public static class PrestamoRepositorio
    {
        public const string EstadoPrestado = "Prestado";
        public const string EstadoDevuelto = "Devuelto";
        private const string ClaveSesion = "Tarea1PrograWeb.Prestamos";
        private const string FormatoFecha = "yyyy-MM-dd";

        private static readonly Regex NombreRegex = new Regex(
            @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ.'\- ]+$",
            RegexOptions.Compiled);

        private static readonly Regex DepartamentoRegex = new Regex(
            @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ0-9.'\- ]+$",
            RegexOptions.Compiled);

        public static IList<Equipo> ObtenerCatalogo()
        {
            return new List<Equipo>
            {
                new Equipo { Id = 1, Codigo = "LAB-LT-01", Nombre = "Laptop Dell Latitude 5420" },
                new Equipo { Id = 2, Codigo = "LAB-LT-02", Nombre = "Laptop HP ProBook 450" },
                new Equipo { Id = 3, Codigo = "LAB-PR-01", Nombre = "Proyector Epson EB-X51" },
                new Equipo { Id = 4, Codigo = "LAB-CM-01", Nombre = "Cámara web Logitech C920" },
                new Equipo { Id = 5, Codigo = "LAB-TB-01", Nombre = "Tablet Samsung Galaxy Tab A8" },
                new Equipo { Id = 6, Codigo = "LAB-MN-01", Nombre = "Monitor portátil HDMI 15 pulgadas" }
            };
        }

        public static List<Prestamo> ObtenerPrestamos()
        {
            var contexto = HttpContext.Current;
            if (contexto != null && contexto.Session != null)
            {
                var lista = contexto.Session[ClaveSesion] as List<Prestamo>;
                if (lista == null)
                {
                    lista = new List<Prestamo>();
                    contexto.Session[ClaveSesion] = lista;
                }

                return lista;
            }

            return new List<Prestamo>();
        }

        public static bool EquipoEstaPrestado(int equipoId)
        {
            foreach (var prestamo in ObtenerPrestamos())
            {
                if (prestamo.EquipoId == equipoId && prestamo.Estado == EstadoPrestado)
                {
                    return true;
                }
            }

            return false;
        }

        public static ResultadoAjax CatalogoConEstado()
        {
            var equipos = new List<EquipoVista>();
            foreach (var equipo in ObtenerCatalogo())
            {
                equipos.Add(new EquipoVista
                {
                    Id = equipo.Id,
                    Codigo = equipo.Codigo,
                    Nombre = equipo.Nombre,
                    Prestado = EquipoEstaPrestado(equipo.Id)
                });
            }

            return new ResultadoAjax
            {
                Exito = true,
                Mensaje = string.Empty,
                Equipos = equipos,
                Prestamos = new List<PrestamoVista>()
            };
        }

        public static ResultadoAjax Registrar(
            string nombre,
            string departamento,
            int equipoId,
            string fechaPrestamo,
            string fechaPrevista,
            string motivo)
        {
            string nombreLimpio = Limpiar(nombre);
            string departamentoLimpio = Limpiar(departamento);
            string motivoLimpio = motivo == null ? string.Empty : motivo.Trim();
            DateTime diaPrestamo;
            DateTime diaPrevisto;

            if (nombreLimpio.Length < 3 || nombreLimpio.Length > 80 || !NombreRegex.IsMatch(nombreLimpio))
            {
                return ResultadoAjax.Fallo("Indique el nombre del solicitante (mínimo 3 caracteres, solo letras).");
            }

            if (departamentoLimpio.Length < 2 || departamentoLimpio.Length > 60 || !DepartamentoRegex.IsMatch(departamentoLimpio))
            {
                return ResultadoAjax.Fallo("Indique el departamento (entre 2 y 60 caracteres).");
            }

            Equipo equipo = BuscarEquipo(equipoId);
            if (equipo == null)
            {
                return ResultadoAjax.Fallo("Seleccione un equipo del catálogo.");
            }

            if (EquipoEstaPrestado(equipo.Id))
            {
                return ResultadoAjax.Fallo("Ese equipo ya está Prestado. Registre la devolución antes de prestarlo de nuevo.");
            }

            if (!TryParseFecha(fechaPrestamo, out diaPrestamo))
            {
                return ResultadoAjax.Fallo("Indique una fecha de préstamo válida (aaaa-mm-dd).");
            }

            if (!TryParseFecha(fechaPrevista, out diaPrevisto))
            {
                return ResultadoAjax.Fallo("Indique una fecha prevista de devolución válida (aaaa-mm-dd).");
            }

            if (diaPrevisto.Date < diaPrestamo.Date)
            {
                return ResultadoAjax.Fallo("La fecha prevista de devolución debe ser igual o posterior a la fecha de préstamo.");
            }

            if (motivoLimpio.Length < 5 || motivoLimpio.Length > 300)
            {
                return ResultadoAjax.Fallo("Indique el motivo (entre 5 y 300 caracteres).");
            }

            var lista = ObtenerPrestamos();
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
            {
                return ResultadoAjax.Fallo("No hay sesión activa para guardar el préstamo.");
            }

            int nuevoId = 1;
            foreach (var existente in lista)
            {
                if (existente.Id >= nuevoId)
                {
                    nuevoId = existente.Id + 1;
                }
            }

            var prestamo = new Prestamo
            {
                Id = nuevoId,
                NombreSolicitante = nombreLimpio,
                Departamento = departamentoLimpio,
                EquipoId = equipo.Id,
                EquipoCodigo = equipo.Codigo,
                EquipoNombre = equipo.Nombre,
                FechaPrestamo = diaPrestamo.Date,
                FechaPrevistaDevolucion = diaPrevisto.Date,
                Motivo = motivoLimpio,
                Estado = EstadoPrestado,
                FechaDevolucion = null
            };

            lista.Add(prestamo);
            HttpContext.Current.Session[ClaveSesion] = lista;

            return new ResultadoAjax
            {
                Exito = true,
                Mensaje = "Préstamo registrado. El equipo queda en estado Prestado.",
                Equipos = CatalogoConEstado().Equipos,
                Prestamos = new List<PrestamoVista> { Mapear(prestamo) },
                Resumen = ConstruirResumen(lista)
            };
        }

        public static ResultadoAjax Listar()
        {
            var lista = ObtenerPrestamos();
            return new ResultadoAjax
            {
                Exito = true,
                Mensaje = string.Empty,
                Equipos = new List<EquipoVista>(),
                Prestamos = MapearTodos(lista, false),
                Resumen = ConstruirResumen(lista)
            };
        }

        public static ResultadoAjax Panel()
        {
            var lista = ObtenerPrestamos();
            return new ResultadoAjax
            {
                Exito = true,
                Mensaje = string.Empty,
                Equipos = new List<EquipoVista>(),
                Prestamos = MapearTodos(lista, true),
                Resumen = ConstruirResumen(lista)
            };
        }

        public static ResultadoAjax RegistrarDevolucion(int id, string fechaDevolucion)
        {
            DateTime diaDevolucion;
            if (!TryParseFecha(fechaDevolucion, out diaDevolucion))
            {
                return ResultadoAjax.Fallo("Indique la fecha de devolución (aaaa-mm-dd).");
            }

            if (HttpContext.Current == null || HttpContext.Current.Session == null)
            {
                return ResultadoAjax.Fallo("No hay sesión activa para registrar la devolución.");
            }

            var lista = ObtenerPrestamos();
            Prestamo prestamo = null;
            foreach (var item in lista)
            {
                if (item.Id == id)
                {
                    prestamo = item;
                    break;
                }
            }

            if (prestamo == null)
            {
                return ResultadoAjax.Fallo("No se encontró el préstamo en esta sesión.");
            }

            if (prestamo.Estado == EstadoDevuelto)
            {
                return ResultadoAjax.Fallo("Ese préstamo ya está Devuelto.");
            }

            if (diaDevolucion.Date < prestamo.FechaPrestamo.Date)
            {
                return ResultadoAjax.Fallo("La fecha de devolución debe ser igual o posterior a la fecha de préstamo.");
            }

            prestamo.Estado = EstadoDevuelto;
            prestamo.FechaDevolucion = diaDevolucion.Date;
            HttpContext.Current.Session[ClaveSesion] = lista;

            return new ResultadoAjax
            {
                Exito = true,
                Mensaje = "Devolución registrada. El equipo queda en estado Devuelto.",
                Equipos = new List<EquipoVista>(),
                Prestamos = MapearTodos(lista, true),
                Resumen = ConstruirResumen(lista)
            };
        }

        private static Equipo BuscarEquipo(int equipoId)
        {
            foreach (var equipo in ObtenerCatalogo())
            {
                if (equipo.Id == equipoId)
                {
                    return equipo;
                }
            }

            return null;
        }

        private static List<PrestamoVista> MapearTodos(IList<Prestamo> lista, bool soloActivos)
        {
            var vistas = new List<PrestamoVista>();
            foreach (var prestamo in lista)
            {
                if (soloActivos && prestamo.Estado != EstadoPrestado)
                {
                    continue;
                }

                vistas.Add(Mapear(prestamo));
            }

            return vistas;
        }

        private static PrestamoVista Mapear(Prestamo prestamo)
        {
            return new PrestamoVista
            {
                Id = prestamo.Id,
                NombreSolicitante = prestamo.NombreSolicitante,
                Departamento = prestamo.Departamento,
                EquipoId = prestamo.EquipoId,
                EquipoCodigo = prestamo.EquipoCodigo,
                EquipoNombre = prestamo.EquipoNombre,
                FechaPrestamo = prestamo.FechaPrestamo.ToString(FormatoFecha, CultureInfo.InvariantCulture),
                FechaPrevistaDevolucion = prestamo.FechaPrevistaDevolucion.ToString(FormatoFecha, CultureInfo.InvariantCulture),
                Motivo = prestamo.Motivo,
                Estado = prestamo.Estado,
                FechaDevolucion = prestamo.FechaDevolucion.HasValue
                    ? prestamo.FechaDevolucion.Value.ToString(FormatoFecha, CultureInfo.InvariantCulture)
                    : string.Empty
            };
        }

        private static ResumenVista ConstruirResumen(IList<Prestamo> lista)
        {
            int activos = 0;
            int devueltos = 0;
            foreach (var prestamo in lista)
            {
                if (prestamo.Estado == EstadoDevuelto)
                {
                    devueltos++;
                }
                else if (prestamo.Estado == EstadoPrestado)
                {
                    activos++;
                }
            }

            return new ResumenVista
            {
                Activos = activos,
                Devueltos = devueltos,
                Total = lista.Count
            };
        }

        private static bool TryParseFecha(string valor, out DateTime fecha)
        {
            return DateTime.TryParseExact(
                Limpiar(valor),
                FormatoFecha,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out fecha);
        }

        private static string Limpiar(string valor)
        {
            return valor == null ? string.Empty : valor.Trim();
        }
    }
}
