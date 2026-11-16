using System;

namespace CampingRobledal
{
    /// <summary>Gestor del camping que inicia sesión (RF-01, RF-03, RF-04).</summary>
    public class Gestor
    {
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Cargo { get; set; }
        public string Foto { get; set; }
        public DateTime UltimoAcceso { get; set; }

        public string NombreCompleto { get { return Nombre + " " + Apellidos; } }

        public string Iniciales
        {
            get
            {
                string a = string.IsNullOrEmpty(Nombre) ? "" : Nombre.Substring(0, 1);
                string b = string.IsNullOrEmpty(Apellidos) ? "" : Apellidos.Substring(0, 1);
                return (a + b).ToUpper();
            }
        }
    }
}
