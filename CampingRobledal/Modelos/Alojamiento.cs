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
