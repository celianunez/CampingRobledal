using System;

namespace CampingRobledal
{
    public class Reserva
    {
        public string Numero { get; set; }
        public Alojamiento Alojamiento { get; set; }
        public DateTime Entrada { get; set; }
        public DateTime Salida { get; set; }
        public string HoraEntrada { get; set; }
        public string HoraSalida { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Telefono2 { get; set; }
        public string Email { get; set; }
        public int Ocupantes { get; set; }
        public bool Cuna { get; set; }
        public bool Movilidad { get; set; }
        public bool MenajeExtra { get; set; }
        public bool Mascota { get; set; }
        public string OtrasSolicitudes { get; set; }
        public EstadoReserva Estado { get; set; }

        public string Cliente { get { return Nombre + " " + Apellidos; } }

        public int Noches
        {
            get { return Salida > Entrada ? (Salida.Date - Entrada.Date).Days : 0; }
        }

        public decimal Importe
        {
            get
            {
                if (Alojamiento == null) return 0;
                Parcela p = Alojamiento as Parcela;
                if (p != null) return Utilidades.Temporadas.ImporteParcela(p, Entrada, Salida);
                Bungalow b = Alojamiento as Bungalow;
                return b != null ? b.PrecioNoche * Noches : 0;
            }
        }

        public string ColorEstado
        {
            get
            {
                switch (Estado)
                {
                    case EstadoReserva.Confirmada: return "#1565C0";
                    case EstadoReserva.CheckIn: return "#2E7D32";
                    case EstadoReserva.Finalizada: return "#757575";
                    default: return "#C62828";
                }
            }
        }

        public string ColorTextoEstado { get { return "#FFFFFF"; } }

        public string TextoEstado
        {
            get
            {
                switch (Estado)
                {
                    case EstadoReserva.Confirmada: return "Confirmada";
                    case EstadoReserva.CheckIn: return "Check-in";
                    case EstadoReserva.Finalizada: return "Finalizada";
                    default: return "Anulada";
                }
            }
        }
    }
}
