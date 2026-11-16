using System.Collections.Generic;

namespace CampingRobledal
{
    /// <summary>Clase base de parcelas y bungalows.</summary>
    public abstract class Alojamiento
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public int TamanoM2 { get; set; }
        public int Capacidad { get; set; }
        public EstadoAlojamiento Estado { get; set; }
        public List<string> Fotos { get; set; } = new List<string>();

        /// <summary>Colores de la pastilla de estado (docs/guia-estilo.md).</summary>
        public string ColorEstado
        {
            get
            {
                switch (Estado)
                {
                    case EstadoAlojamiento.Libre: return "#2E7D32";
                    case EstadoAlojamiento.Ocupado: return "#C62828";
                    case EstadoAlojamiento.Reservado: return "#1565C0";
                    case EstadoAlojamiento.EnReparacion: return "#6D4C41";
                    default: return "#F9A825";
                }
            }
        }

        public string ColorTextoEstado { get { return Estado == EstadoAlojamiento.PendienteLimpieza ? "#222222" : "#FFFFFF"; } }

        public string TextoEstado
        {
            get
            {
                switch (Estado)
                {
                    case EstadoAlojamiento.Libre: return "Libre";
                    case EstadoAlojamiento.Ocupado: return "Ocupado";
                    case EstadoAlojamiento.Reservado: return "Reservado";
                    case EstadoAlojamiento.EnReparacion: return "En reparación";
                    default: return "Pendiente de limpieza";
                }
            }
        }
    }
}
