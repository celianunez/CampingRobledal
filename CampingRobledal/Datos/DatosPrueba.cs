using System;
using System.Collections.ObjectModel;

namespace CampingRobledal.Datos
{
    /// <summary>Datos de prueba en memoria (D-012). Se crean una vez al arrancar.</summary>
    public static class DatosPrueba
    {
        public static ObservableCollection<Gestor> Gestores { get; private set; }
        public static ObservableCollection<Parcela> Parcelas { get; private set; }
        public static ObservableCollection<Bungalow> Bungalows { get; private set; }
        public static ObservableCollection<Reserva> Reservas { get; private set; }
        public static ObservableCollection<Monitor> Monitores { get; private set; }
        public static ObservableCollection<Actividad> Actividades { get; private set; }

        static DatosPrueba()
        {
            CrearGestores();
            CrearParcelas();
            CrearBungalows();
            CrearMonitores();
            CrearActividades();
            CrearReservas();
        }

        /// <summary>Devuelve el gestor con ese usuario o null si no existe.</summary>
        public static Gestor BuscarGestor(string usuario)
        {
            foreach (Gestor g in Gestores)
                if (string.Equals(g.Usuario, usuario, StringComparison.OrdinalIgnoreCase)) return g;
            return null;
        }

        private static void CrearGestores()
        {
            Gestores = new ObservableCollection<Gestor>
            {
                new Gestor { Usuario = "adam", Contrasena = "ipo1", Nombre = "Adam", Apellidos = "Gouram El Bouhali",
                             Cargo = "Responsable de recepción", UltimoAcceso = DateTime.Now.AddDays(-1).AddHours(-3) },
                new Gestor { Usuario = "celia", Contrasena = "ipo1", Nombre = "Celia", Apellidos = "Núñez Acosta",
                             Cargo = "Gerente", UltimoAcceso = DateTime.Now.AddDays(-2).AddHours(-1) }
            };
        }

        private static void CrearParcelas()
        {
            Parcelas = new ObservableCollection<Parcela>();
            CategoriaParcela[] cats = { CategoriaParcela.Pequena, CategoriaParcela.Mediana, CategoriaParcela.Grande, CategoriaParcela.Premium };
            EstadoAlojamiento[] estados = { EstadoAlojamiento.Libre, EstadoAlojamiento.Ocupado, EstadoAlojamiento.Reservado,
                                            EstadoAlojamiento.Libre, EstadoAlojamiento.EnReparacion, EstadoAlojamiento.PendienteLimpieza };
            for (int i = 1; i <= 10; i++)
            {
                CategoriaParcela cat = cats[i % 4];
                int nivel = (int)cat;
                Parcelas.Add(new Parcela
                {
                    Id = "P-" + i.ToString("00"),
                    Nombre = "Parcela " + i,
                    Categoria = cat,
                    TamanoM2 = 40 + nivel * 20,
                    Capacidad = 3 + nivel,
                    Estado = estados[i % estados.Length],
                    Ubicacion = i <= 5 ? "Zona del robledal" : "Zona del río",
                    Electricidad = true,
                    Agua = nivel >= 1,
                    Desague = nivel >= 2,
                    Sombra = i % 2 == 0,
                    PrecioBaja = 14 + nivel * 4,
                    PrecioMedia = 20 + nivel * 5,
                    PrecioAlta = 28 + nivel * 7,
                    Observaciones = ""
                });
            }
        }

        private static void CrearBungalows()
        {
            Bungalows = new ObservableCollection<Bungalow>();
            TipoBungalow[] tipos = { TipoBungalow.Estandar, TipoBungalow.Familiar, TipoBungalow.Premium, TipoBungalow.Estandar, TipoBungalow.Familiar };
            EstadoAlojamiento[] estados = { EstadoAlojamiento.Libre, EstadoAlojamiento.Ocupado, EstadoAlojamiento.Libre,
                                            EstadoAlojamiento.Reservado, EstadoAlojamiento.PendienteLimpieza };
            for (int i = 1; i <= 5; i++)
            {
                int nivel = (int)tipos[i - 1];
                Bungalows.Add(new Bungalow
                {
                    Id = "B-" + i.ToString("00"),
                    Nombre = "Bungalow " + i,
                    Tipo = tipos[i - 1],
                    TamanoM2 = 25 + nivel * 10,
                    Capacidad = 2 + nivel * 2,
                    Estado = estados[i - 1],
                    PrecioNoche = 55 + nivel * 25,
                    EstanciaMinima = nivel == 2 ? (int?)3 : null,
                    AireAcondicionado = nivel >= 1,
                    Cocina = true,
                    Terraza = nivel >= 1,
                    BanoPropio = true,
                    Descripcion = "Bungalow de madera rodeado de robles."
                });
            }
        }

        private static void CrearMonitores()
        {
            Monitores = new ObservableCollection<Monitor>
            {
                new Monitor { Nombre = "Lucía", Apellidos = "Prieto Vega", Especialidad = "Natación y socorrismo",
                              Telefono = "612345678", Correo = "lucia@correo.es",
                              Idiomas = { "Español", "Inglés" }, Formacion = "Técnico en socorrismo acuático",
                              InicioContrato = new DateTime(2027, 6, 15), FinContrato = new DateTime(2027, 9, 15) },
                new Monitor { Nombre = "Pablo", Apellidos = "Serrano Lillo", Especialidad = "Senderismo y orientación",
                              Telefono = "623456789", Correo = "pablo@correo.es",
                              Idiomas = { "Español", "Francés" }, Formacion = "Guía de montaña",
                              InicioContrato = new DateTime(2027, 6, 15), FinContrato = new DateTime(2027, 9, 15) },
                new Monitor { Nombre = "Sofía", Apellidos = "Martín Ruiz", Especialidad = "Manualidades y juegos",
                              Telefono = "634567890", Correo = "sofia@correo.es",
                              Idiomas = { "Español", "Inglés", "Alemán" }, Formacion = "Monitora de tiempo libre",
                              InicioContrato = new DateTime(2027, 7, 1), FinContrato = new DateTime(2027, 8, 31) },
                new Monitor { Nombre = "Diego", Apellidos = "Cano Moya", Especialidad = "Deportes de equipo",
                              Telefono = "645678901", Correo = "diego@correo.es",
                              Idiomas = { "Español" }, Formacion = "TAFAD", Activo = false,
                              InicioContrato = new DateTime(2027, 7, 1), FinContrato = new DateTime(2027, 8, 31) }
            };
        }

        private static void CrearActividades()
        {
            Actividades = new ObservableCollection<Actividad>
            {
                Nueva("Natación infantil", Destinatarios.Ninos, 5, "Piscina", 0, "10:00", "11:00", 4, 8, 6, 6m, 40m, DayOfWeek.Monday, DayOfWeek.Wednesday),
                Nueva("Ruta de senderismo", Destinatarios.Todos, 8, "Montaña", 1, "09:00", "13:00", 5, 15, 9, 8m, null, DayOfWeek.Saturday),
                Nueva("Taller de manualidades", Destinatarios.Ninos, 4, "Sala de juegos", 2, "17:00", "18:30", 3, 12, 12, 4m, 25m, DayOfWeek.Tuesday, DayOfWeek.Thursday),
                Nueva("Torneo de voleibol", Destinatarios.Adultos, 16, "Pista deportiva", 3, "19:00", "20:30", 6, 12, 4, 3m, null, DayOfWeek.Friday),
                Nueva("Aquagym", Destinatarios.Adultos, 16, "Piscina", 0, "12:00", "13:00", 4, 10, 7, 5m, 30m, DayOfWeek.Monday, DayOfWeek.Friday)
            };
        }

        private static Actividad Nueva(string nombre, Destinatarios dest, int edad, string area, int indiceMonitor,
            string inicio, string fin, int cupoMin, int cupoMax, int inscritos, decimal precioHora, decimal? precioMes,
            params DayOfWeek[] dias)
        {
            Actividad a = new Actividad
            {
                Nombre = nombre, Destinatarios = dest, EdadMinima = edad, Area = area,
                HoraInicio = inicio, HoraFin = fin, CupoMinimo = cupoMin, CupoMaximo = cupoMax,
                Inscritos = inscritos, PrecioHora = precioHora, PrecioMes = precioMes,
                Descripcion = "Actividad de verano del camping.", Materiales = "Ropa cómoda y agua."
            };
            a.Monitores.Add(Monitores[indiceMonitor]);
            a.Dias.AddRange(dias);
            return a;
        }

        private static void CrearReservas()
        {
            Reservas = new ObservableCollection<Reserva>();
            EstadoReserva[] estados = { EstadoReserva.Confirmada, EstadoReserva.CheckIn, EstadoReserva.Finalizada, EstadoReserva.Anulada,
                                        EstadoReserva.Confirmada, EstadoReserva.CheckIn, EstadoReserva.Finalizada, EstadoReserva.Confirmada };
            string[] nombres = { "Marta", "Javier", "Elena", "Raúl", "Carmen", "Andrés", "Laura", "Sergio" };
            string[] apellidos = { "López Gil", "Ramos Sanz", "Torres Vidal", "Ortiz Peña", "Navarro Ríos", "Gómez Lara", "Díaz Mora", "Castro Rey" };
            for (int i = 0; i < 8; i++)
            {
                Alojamiento aloj = i < 5 ? (Alojamiento)Parcelas[i] : Bungalows[i - 5];
                DateTime entrada = new DateTime(2027, 7, 1).AddDays(i * 4);
                Reservas.Add(new Reserva
                {
                    Numero = "R-" + (i + 1).ToString("0000"),
                    Alojamiento = aloj, Entrada = entrada, Salida = entrada.AddDays(3 + i % 4),
                    HoraEntrada = "14:00", HoraSalida = "12:00",
                    Nombre = nombres[i], Apellidos = apellidos[i],
                    Telefono = "6" + (10000000 + i * 1111111).ToString(),
                    Email = nombres[i].ToLower().Replace("ú", "u").Replace("é", "e") + "@correo.es",
                    Ocupantes = 2 + i % 3, Mascota = i % 4 == 0, Cuna = i % 5 == 1,
                    Estado = estados[i]
                });
            }
        }
    }
}
