using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CampingRobledal.Monitores
{
    /// <summary>
    /// Monitor temporal de verano. Implementa INotifyPropertyChanged para que la
    /// ficha y la tabla se actualicen en cuanto cambia un dato.
    /// </summary>
    public class Monitor : INotifyPropertyChanged
    {
        private string _nombre = string.Empty;
        private string _apellidos = string.Empty;
        private string _especialidad = string.Empty;
        private string _disponibilidad = string.Empty;
        private string _foto;
        private string _telefono1 = string.Empty;
        private string _telefono2 = string.Empty;
        private string _email = string.Empty;
        private List<string> _idiomas = new List<string>();
        private string _formacion = string.Empty;
        private string _restricciones = string.Empty;
        private DateTime _contratoDesde;
        private DateTime _contratoHasta;
        private List<string> _actividades = new List<string>();

        public string Nombre
        {
            get { return _nombre; }
            set
            {
                if (_nombre == value) return;
                _nombre = value ?? string.Empty;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NombreCompleto));
            }
        }

        public string Apellidos
        {
            get { return _apellidos; }
            set
            {
                if (_apellidos == value) return;
                _apellidos = value ?? string.Empty;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NombreCompleto));
            }
        }

        public string Especialidad
        {
            get { return _especialidad; }
            set
            {
                if (_especialidad == value) return;
                _especialidad = value ?? string.Empty;
                OnPropertyChanged();
            }
        }

        /// <summary>Texto libre que se ve en la tabla: «Julio y agosto», «Todo el verano»…</summary>
        public string Disponibilidad
        {
            get { return _disponibilidad; }
            set
            {
                if (_disponibilidad == value) return;
                _disponibilidad = value ?? string.Empty;
                OnPropertyChanged();
            }
        }

        /// <summary>Ruta del fichero de imagen; null si no tiene foto.</summary>
        public string Foto
        {
            get { return _foto; }
            set
            {
                if (_foto == value) return;
                _foto = string.IsNullOrWhiteSpace(value) ? null : value;
                OnPropertyChanged();
            }
        }

        public string Telefono1
        {
            get { return _telefono1; }
            set
            {
                if (_telefono1 == value) return;
                _telefono1 = value ?? string.Empty;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TelefonosTexto));
            }
        }

        public string Telefono2
        {
            get { return _telefono2; }
            set
            {
                if (_telefono2 == value) return;
                _telefono2 = value ?? string.Empty;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TelefonosTexto));
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if (_email == value) return;
                _email = value ?? string.Empty;
                OnPropertyChanged();
            }
        }

        public List<string> Idiomas
        {
            get { return _idiomas; }
            set
            {
                _idiomas = value ?? new List<string>();
                OnPropertyChanged();
                OnPropertyChanged(nameof(IdiomasCorto));
                OnPropertyChanged(nameof(IdiomasTexto));
            }
        }

        public string Formacion
        {
            get { return _formacion; }
            set
            {
                if (_formacion == value) return;
                _formacion = value ?? string.Empty;
                OnPropertyChanged();
            }
        }

        public string Restricciones
        {
            get { return _restricciones; }
            set
            {
                if (_restricciones == value) return;
                _restricciones = value ?? string.Empty;
                OnPropertyChanged();
            }
        }

        public DateTime ContratoDesde
        {
            get { return _contratoDesde; }
            set
            {
                if (_contratoDesde == value) return;
                _contratoDesde = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ContratoTexto));
            }
        }

        public DateTime ContratoHasta
        {
            get { return _contratoHasta; }
            set
            {
                if (_contratoHasta == value) return;
                _contratoHasta = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ContratoTexto));
            }
        }

        public List<string> Actividades
        {
            get { return _actividades; }
            set
            {
                _actividades = value ?? new List<string>();
                OnPropertyChanged();
                OnPropertyChanged(nameof(ActividadesTexto));
            }
        }

        // ---------- Propiedades calculadas, solo lectura ----------

        public string NombreCompleto
        {
            get { return (Nombre + " " + Apellidos).Trim(); }
        }

        /// <summary>«ES · FR · EN», el formato compacto que se usa en la tabla.</summary>
        public string IdiomasCorto
        {
            get
            {
                if (Idiomas == null || Idiomas.Count == 0) return "—";
                return string.Join(" · ", Idiomas.Select(CodigoIdioma));
            }
        }

        /// <summary>«Español · Francés · Inglés», para la ficha.</summary>
        public string IdiomasTexto
        {
            get
            {
                if (Idiomas == null || Idiomas.Count == 0) return "Sin idiomas indicados";
                return string.Join(" · ", Idiomas);
            }
        }

        /// <summary>«01/06 – 15/09/2026».</summary>
        public string ContratoTexto
        {
            get
            {
                if (ContratoDesde == DateTime.MinValue && ContratoHasta == DateTime.MinValue)
                    return "Sin contrato";

                if (ContratoDesde == DateTime.MinValue)
                    return "Hasta " + ContratoHasta.ToString("dd/MM/yyyy");

                if (ContratoHasta == DateTime.MinValue)
                    return "Desde " + ContratoDesde.ToString("dd/MM/yyyy");

                if (ContratoDesde.Year == ContratoHasta.Year)
                    return ContratoDesde.ToString("dd/MM") + " – " + ContratoHasta.ToString("dd/MM/yyyy");

                return ContratoDesde.ToString("dd/MM/yyyy") + " – " + ContratoHasta.ToString("dd/MM/yyyy");
            }
        }

        public string TelefonosTexto
        {
            get
            {
                var telefonos = new List<string>();
                if (!string.IsNullOrWhiteSpace(Telefono1)) telefonos.Add(Telefono1);
                if (!string.IsNullOrWhiteSpace(Telefono2)) telefonos.Add(Telefono2);
                return telefonos.Count == 0 ? "Sin teléfono" : string.Join(" · ", telefonos);
            }
        }

        public string ActividadesTexto
        {
            get
            {
                if (Actividades == null || Actividades.Count == 0)
                    return "Sin actividades asignadas";
                return string.Join(", ", Actividades);
            }
        }

        // ---------- Utilidades ----------

        public Monitor Clonar()
        {
            return new Monitor
            {
                Nombre = Nombre,
                Apellidos = Apellidos,
                Especialidad = Especialidad,
                Disponibilidad = Disponibilidad,
                Foto = Foto,
                Telefono1 = Telefono1,
                Telefono2 = Telefono2,
                Email = Email,
                Idiomas = new List<string>(Idiomas ?? new List<string>()),
                Formacion = Formacion,
                Restricciones = Restricciones,
                ContratoDesde = ContratoDesde,
                ContratoHasta = ContratoHasta,
                Actividades = new List<string>(Actividades ?? new List<string>())
            };
        }

        /// <summary>Copia los valores de otro monitor sobre este (se usa al guardar una modificación).</summary>
        public void CopiarDesde(Monitor otro)
        {
            if (otro == null) return;

            Nombre = otro.Nombre;
            Apellidos = otro.Apellidos;
            Especialidad = otro.Especialidad;
            Disponibilidad = otro.Disponibilidad;
            Foto = otro.Foto;
            Telefono1 = otro.Telefono1;
            Telefono2 = otro.Telefono2;
            Email = otro.Email;
            Idiomas = new List<string>(otro.Idiomas ?? new List<string>());
            Formacion = otro.Formacion;
            Restricciones = otro.Restricciones;
            ContratoDesde = otro.ContratoDesde;
            ContratoHasta = otro.ContratoHasta;
            Actividades = new List<string>(otro.Actividades ?? new List<string>());
        }

        public static string CodigoIdioma(string idioma)
        {
            if (string.IsNullOrWhiteSpace(idioma)) return "—";

            switch (QuitarAcentos(idioma).Trim().ToLowerInvariant())
            {
                case "espanol": return "ES";
                case "ingles": return "EN";
                case "frances": return "FR";
                case "aleman": return "DE";
                case "italiano": return "IT";
                case "portugues": return "PT";
                case "catalan": return "CA";
                case "euskera": return "EU";
                case "gallego": return "GL";
                default:
                    string limpio = new string(idioma.Where(char.IsLetter).ToArray());
                    return limpio.Length >= 2 ? limpio.Substring(0, 2).ToUpperInvariant() : "—";
            }
        }

        private static string QuitarAcentos(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return string.Empty;

            string descompuesto = texto.Normalize(NormalizationForm.FormD);
            var resultado = new StringBuilder();

            foreach (char c in descompuesto)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c)
                    != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    resultado.Append(c);
                }
            }

            return resultado.ToString().Normalize(NormalizationForm.FormC);
        }

        public override string ToString()
        {
            return NombreCompleto;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string nombrePropiedad = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombrePropiedad));
        }
    }
}
