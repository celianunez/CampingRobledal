using System;
using System.Collections.Generic;

namespace CampingRobledal
{
    public class Actividad
    {
        public string Nombre { get; set; }
        public Destinatarios Destinatarios { get; set; }
        public int EdadMinima { get; set; }
        public string Area { get; set; }
        public List<Monitor> Monitores { get; set; } = new List<Monitor>();
        public List<DayOfWeek> Dias { get; set; } = new List<DayOfWeek>();
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public int CupoMinimo { get; set; }
        public int CupoMaximo { get; set; }
        public int Inscritos { get; set; }
        public decimal PrecioHora { get; set; }
        public decimal? PrecioMes { get; set; }
        public string Descripcion { get; set; }
        public string Materiales { get; set; }
        public string Foto { get; set; }

        public string TextoDestinatarios { get { return Destinatarios == Destinatarios.Ninos ? "Niños" : Destinatarios.ToString(); } }
        public string TextoPrecio { get { return PrecioHora.ToString("0.00") + " €/h"; } }

        public string TextoDias
        {
            get
            {
                string[] n = { "dom", "lun", "mar", "mié", "jue", "vie", "sáb" };
                System.Collections.Generic.List<string> l = new System.Collections.Generic.List<string>();
                foreach (DayOfWeek d in Dias) l.Add(n[(int)d]);
                return string.Join(", ", l);
            }
        }

        public string TextoCupo { get { return Inscritos + " / " + CupoMaximo; } }
        public string Horario { get { return HoraInicio + " – " + HoraFin; } }
        public string TextoMonitores
        {
            get
            {
                List<string> nombres = new List<string>();
                foreach (Monitor m in Monitores) nombres.Add(m.NombreCompleto);
                return string.Join(", ", nombres);
            }
        }
    }
}
