using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CampingRobledal.Monitores;

namespace CampingRobledal.Datos
{
    /// <summary>
    /// Datos de prueba del prototipo. Se cargan al arrancar y se pierden al cerrar
    /// (el enunciado no pide persistencia).
    /// </summary>
    public static class DatosPrueba
    {
        public static readonly string[] Especialidades =
        {
            "Animación infantil",
            "Deportes acuáticos",
            "Manualidades",
            "Multiaventura",
            "Senderismo",
            "Tiro con arco"
        };

        public static readonly string[] IdiomasDisponibles =
        {
            "Español",
            "Inglés",
            "Francés",
            "Alemán",
            "Italiano"
        };

        public static ObservableCollection<Monitor> Monitores { get; } = CrearMonitores();

        private static ObservableCollection<Monitor> CrearMonitores()
        {
            return new ObservableCollection<Monitor>
            {
                new Monitor
                {
                    Nombre = "Carlos",
                    Apellidos = "Pérez Díaz",
                    Especialidad = "Deportes acuáticos",
                    Disponibilidad = "Julio y agosto",
                    Telefono1 = "611 222 333",
                    Telefono2 = "926 11 22 33",
                    Email = "carlos.perez@correo.es",
                    Idiomas = new List<string> { "Español", "Inglés" },
                    Formacion = "Graduado en Ciencias de la Actividad Física y del Deporte. " +
                                "Socorrista (2025).",
                    Restricciones = "No disponible los lunes por la mañana.",
                    ContratoDesde = new DateTime(2026, 7, 1),
                    ContratoHasta = new DateTime(2026, 8, 31),
                    Actividades = new List<string> { "Kayak", "Paddle surf" }
                },
                new Monitor
                {
                    Nombre = "Laura",
                    Apellidos = "Sánchez Gil",
                    Especialidad = "Animación infantil",
                    Disponibilidad = "Todo el verano",
                    Telefono1 = "600 333 444",
                    Telefono2 = "926 55 66 77",
                    Email = "laura.sanchez@correo.es",
                    Idiomas = new List<string> { "Español", "Francés", "Inglés" },
                    Formacion = "Grado en Educación Infantil. Monitora de ocio y tiempo libre. " +
                                "Primeros auxilios (2025).",
                    Restricciones = "No disponible del 1 al 10 de agosto. " +
                                    "Domingos solo por la mañana.",
                    ContratoDesde = new DateTime(2026, 6, 1),
                    ContratoHasta = new DateTime(2026, 9, 15),
                    Actividades = new List<string> { "Taller de pintura", "Juegos de agua" }
                },
                new Monitor
                {
                    Nombre = "Miguel",
                    Apellidos = "Torres Lara",
                    Especialidad = "Senderismo",
                    Disponibilidad = "Fines de semana",
                    Telefono1 = "622 334 455",
                    Telefono2 = "",
                    Email = "miguel.torres@correo.es",
                    Idiomas = new List<string> { "Español", "Inglés", "Alemán" },
                    Formacion = "Técnico Superior en Guía en el Medio Natural. " +
                                "Primeros auxilios (2024).",
                    Restricciones = "Solo fines de semana. No disponible del 15 al 30 de julio.",
                    ContratoDesde = new DateTime(2026, 6, 15),
                    ContratoHasta = new DateTime(2026, 9, 30),
                    Actividades = new List<string> { "Rutas de senderismo", "Orientación" }
                },
                new Monitor
                {
                    Nombre = "Nuria",
                    Apellidos = "Vega Soto",
                    Especialidad = "Manualidades",
                    Disponibilidad = "Julio",
                    Telefono1 = "633 445 566",
                    Telefono2 = "",
                    Email = "nuria.vega@correo.es",
                    Idiomas = new List<string> { "Español" },
                    Formacion = "Grado en Bellas Artes. Monitora de ocio y tiempo libre.",
                    Restricciones = "No disponible del 20 al 25 de julio.",
                    ContratoDesde = new DateTime(2026, 7, 1),
                    ContratoHasta = new DateTime(2026, 7, 31),
                    Actividades = new List<string> { "Taller de cerámica", "Manualidades infantiles" }
                },
                new Monitor
                {
                    Nombre = "Óscar",
                    Apellidos = "Ramos Peña",
                    Especialidad = "Tiro con arco",
                    Disponibilidad = "Agosto",
                    Telefono1 = "644 556 677",
                    Telefono2 = "926 44 55 66",
                    Email = "oscar.ramos@correo.es",
                    Idiomas = new List<string> { "Español", "Inglés" },
                    Formacion = "Técnico deportivo en tiro con arco. Primeros auxilios (2025).",
                    Restricciones = "Solo por las mañanas.",
                    ContratoDesde = new DateTime(2026, 8, 1),
                    ContratoHasta = new DateTime(2026, 8, 31),
                    Actividades = new List<string> { "Tiro con arco", "Gymkhana" }
                }
            };
        }
    }
}
