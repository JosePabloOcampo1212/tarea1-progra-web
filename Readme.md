# Mesa de préstamos de equipos

*Universidad Estatal a Distancia (UNED)*
Escuela de Ciencias Exactas y Naturales · Ingeniería Informática · Cátedra Desarrollo de Software

*Curso:* 03101 Programación avanzada en web
*Tarea:* No. 1 · III cuatrimestre 2026
*Estudiante:* Jose Pablo Ocampo Corrales



---

## Descripción

Prototipo web para la mesa de TI de un laboratorio institucional. Permite anotar los préstamos de equipos durante un turno, consultarlos y registrar su devolución. No tiene autenticación ni base de datos: los datos viven en una lista en memoria (List<Prestamo> en Session) y se pierden al cerrar la sesión.

## Funcionalidades

| Función | Qué hace |
|---|---|
| *Registro* | Formulario de seis campos con validación en el cliente (jQuery) y en el servidor. No deja prestar un equipo que ya está prestado. |
| *Consulta* | Tabla de los préstamos de la sesión, que se actualiza por Ajax sin recargar la página. |
| *Procesamiento* | Registra la devolución (Prestado → Devuelto) y muestra el conteo de activos, devueltos y total. |



## Estructura


Models/      Clases de datos (Prestamo, Equipo y vistas para Ajax)
Services/    PrestamoRepositorio: lista en Session y reglas de negocio
Scripts/     jQuery, comun.js y un script por función
Content/     Estilos
*.aspx       Páginas Web Forms (Registro, Consulta, Procesamiento) y Site.Master



## Limitaciones

Sin persistencia real: la lista se pierde al cerrar el navegador, al vencer la sesión (60 minutos) o al reciclarse el proceso de IIS. El catálogo de equipos es fijo.