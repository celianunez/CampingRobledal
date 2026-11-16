using System;
using System.Collections.Generic;

namespace CampingRobledal
{
    public class Monitor
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Foto { get; set; }
        public string Especialidad { get; set; }
        public string Telefono { get; set; }
        public string Telefono2 { get; set; }
        public string Correo { get; set; }
        public string Correo2 { get; set; }
        public List<string> Idiomas { get; set; } = new List<string>();
        public string Formacion { get; set; }
        public DateTime InicioContrato { get; set; }
        public DateTime FinContrato { get; set; }
        public string Restricciones { get; set; }
        public bool Activo { get; set; } = true;

        public string NombreCompleto { get { return Nombre + " " + Apellidos; } }
        public string TextoIdiomas { get { return string.Join(", ", Idiomas); } }
        public string Disponibilidad { get { return Activo ? "Disponible" : "De baja"; } }
    }
}
