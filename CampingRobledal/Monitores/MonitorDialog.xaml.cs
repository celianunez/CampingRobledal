using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CampingRobledal.Datos;
using Microsoft.Win32;

namespace CampingRobledal.Monitores
{
    /// <summary>
    /// Diálogo modal de alta y modificación de monitores (boceto 11).
    /// Trabaja siempre sobre una copia: si se pulsa Cancelar, el monitor original
    /// no cambia. La validación se muestra al salir del campo y bloquea Guardar.
    /// </summary>
    public partial class MonitorDialog : Window
    {
        private static readonly Regex FormatoEmail =
            new Regex(@"^[^@\s]+@[^@\s]+\.[A-Za-z]{2,}$", RegexOptions.Compiled);

        private readonly Monitor _copia;
        private Monitor _monitorOriginal;

        private string _fotoRuta;
        private bool _cargando = true;

        private bool _nombreTocado;
        private bool _apellidosTocado;
        private bool _telefonoTocado;
        private bool _emailTocado;
        private bool _fechasTocado;

        /// <summary>Monitor resultante. Solo tiene valor si el diálogo se cerró con Guardar.</summary>
        public Monitor Resultado { get; private set; }

        public MonitorDialog()
        {
            InitializeComponent();
            _copia = new Monitor();
            Configurar(null);
        }

        public MonitorDialog(Monitor monitor)
        {
            InitializeComponent();
            _copia = monitor == null ? new Monitor() : monitor.Clonar();
            Configurar(monitor);
        }

        private void Configurar(Monitor original)
        {
            _monitorOriginal = original;

            Title = original == null
                ? "Nuevo monitor"
                : "Modificar monitor · " + original.NombreCompleto;

            CargarEspecialidades(_copia.Especialidad);
            CargarControles(_copia);

            _cargando = false;
            ComprobarFormulario();

            Loaded += MonitorDialog_Loaded;
        }

        private void MonitorDialog_Loaded(object sender, RoutedEventArgs e)
        {
            txtNombre.Focus();
            txtNombre.SelectAll();
        }

        private void CargarEspecialidades(string especialidadActual)
        {
            var especialidades = new List<string>(DatosPrueba.Especialidades);

            if (!string.IsNullOrWhiteSpace(especialidadActual)
                && !especialidades.Any(e => string.Equals(e, especialidadActual,
                                                           StringComparison.CurrentCultureIgnoreCase)))
            {
                especialidades.Add(especialidadActual);
            }

            cmbEspecialidad.ItemsSource = especialidades;

            if (string.IsNullOrWhiteSpace(especialidadActual))
            {
                cmbEspecialidad.SelectedIndex = -1;
            }
            else
            {
                cmbEspecialidad.SelectedItem = especialidades
                    .FirstOrDefault(e => string.Equals(e, especialidadActual,
                                                       StringComparison.CurrentCultureIgnoreCase));
            }
        }

        private void CargarControles(Monitor monitor)
        {
            _cargando = true;

            txtNombre.Text = monitor.Nombre;
            txtApellidos.Text = monitor.Apellidos;
            txtTelefono1.Text = monitor.Telefono1;
            txtTelefono2.Text = monitor.Telefono2;
            txtEmail.Text = monitor.Email;
            txtFormacion.Text = monitor.Formacion;
            txtRestricciones.Text = monitor.Restricciones;

            var idiomas = monitor.Idiomas ?? new List<string>();
            chkEspanol.IsChecked = ContieneIdioma(idiomas, "Español");
            chkIngles.IsChecked = ContieneIdioma(idiomas, "Inglés");
            chkFrances.IsChecked = ContieneIdioma(idiomas, "Francés");
            chkAleman.IsChecked = ContieneIdioma(idiomas, "Alemán");
            chkItaliano.IsChecked = ContieneIdioma(idiomas, "Italiano");

            var otros = idiomas
                .Where(i => !DatosPrueba.IdiomasDisponibles.Any(d =>
                    string.Equals(d, i, StringComparison.CurrentCultureIgnoreCase)))
                .ToList();

            txtOtroIdioma.Text = string.Join(", ", otros);

            dpContratoDesde.SelectedDate = monitor.ContratoDesde == DateTime.MinValue
                ? (DateTime?)null
                : monitor.ContratoDesde;

            dpContratoHasta.SelectedDate = monitor.ContratoHasta == DateTime.MinValue
                ? (DateTime?)null
                : monitor.ContratoHasta;

            _fotoRuta = monitor.Foto;
            MostrarFoto();
            ActualizarEjemploRestricciones();

            _cargando = false;
        }

        private static bool ContieneIdioma(List<string> idiomas, string idioma)
        {
            return idiomas.Any(i => string.Equals(i, idioma,
                                                  StringComparison.CurrentCultureIgnoreCase));
        }

        // ===================== Foto =====================

        private void btnCargarFoto_Click(object sender, RoutedEventArgs e)
        {
            var dialogo = new OpenFileDialog
            {
                Title = "Elegir foto del monitor",
                Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Todos los archivos|*.*",
                CheckFileExists = true
            };

            if (dialogo.ShowDialog(this) != true) return;

            _fotoRuta = dialogo.FileName;
            MostrarFoto();
        }

        private void MostrarFoto()
        {
            if (string.IsNullOrWhiteSpace(_fotoRuta))
            {
                imgFoto.Source = null;
                imgFoto.Visibility = Visibility.Collapsed;
                return;
            }

            try
            {
                var imagen = new BitmapImage();
                imagen.BeginInit();
                imagen.CacheOption = BitmapCacheOption.OnLoad;
                imagen.UriSource = new Uri(_fotoRuta, UriKind.Absolute);
                imagen.EndInit();

                imgFoto.Source = imagen;
                imgFoto.Visibility = Visibility.Visible;
            }
            catch
            {
                imgFoto.Source = null;
                imgFoto.Visibility = Visibility.Collapsed;
            }
        }

        // ===================== Validación =====================

        private void CampoTexto_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_cargando) return;
            ComprobarFormulario();
        }

        private void CampoTexto_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_cargando) return;

            if (sender == txtNombre) _nombreTocado = true;
            else if (sender == txtApellidos) _apellidosTocado = true;
            else if (sender == txtTelefono1 || sender == txtTelefono2) _telefonoTocado = true;
            else if (sender == txtEmail) _emailTocado = true;

            ComprobarFormulario();
        }

        private void CampoFecha_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (_cargando) return;
            ComprobarFormulario();
        }

        private void CampoFecha_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_cargando) return;
            _fechasTocado = true;
            ComprobarFormulario();
        }

        private void txtRestricciones_TextChanged(object sender, TextChangedEventArgs e)
        {
            ActualizarEjemploRestricciones();
        }

        private void ActualizarEjemploRestricciones()
        {
            if (lblEjemploRestricciones == null || txtRestricciones == null) return;

            lblEjemploRestricciones.Visibility = string.IsNullOrEmpty(txtRestricciones.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private bool ComprobarFormulario(bool mostrarTodos = false)
        {
            if (btnGuardar == null) return false;

            bool nombreCorrecto = ValidarNombre(mostrarTodos || _nombreTocado);
            bool apellidosCorrectos = ValidarApellidos(mostrarTodos || _apellidosTocado);
            bool telefonoCorrecto = ValidarTelefono(mostrarTodos || _telefonoTocado);
            bool emailCorrecto = ValidarEmail(mostrarTodos || _emailTocado);
            bool fechasCorrectas = ValidarFechas(mostrarTodos || _fechasTocado);

            bool todoCorrecto = nombreCorrecto && apellidosCorrectos && telefonoCorrecto
                                && emailCorrecto && fechasCorrectas;

            btnGuardar.IsEnabled = todoCorrecto;
            return todoCorrecto;
        }

        private bool ValidarNombre(bool mostrar)
        {
            bool correcto = !string.IsNullOrWhiteSpace(txtNombre.Text);

            MarcarCampo(txtNombre, lblErrorNombre, correcto,
                "Escribe el nombre. Ejemplo: Laura", mostrar);

            return correcto;
        }

        private bool ValidarApellidos(bool mostrar)
        {
            bool correcto = !string.IsNullOrWhiteSpace(txtApellidos.Text);

            MarcarCampo(txtApellidos, lblErrorApellidos, correcto,
                "Escribe los apellidos. Ejemplo: Sánchez Gil", mostrar);

            return correcto;
        }

        private bool ValidarTelefono(bool mostrar)
        {
            string principal = txtTelefono1.Text;
            string segundo = txtTelefono2.Text;

            bool principalCorrecto = TelefonoValido(principal);
            bool segundoCorrecto = string.IsNullOrWhiteSpace(segundo) || TelefonoValido(segundo);

            string mensaje;
            if (string.IsNullOrWhiteSpace(principal))
                mensaje = "Escribe el teléfono. Ejemplo: 612345678";
            else if (!principalCorrecto)
                mensaje = "El teléfono debe tener 9 cifras. Ejemplo: 612345678";
            else if (!segundoCorrecto)
                mensaje = "El segundo teléfono debe tener 9 cifras. Ejemplo: 926112233";
            else
                mensaje = string.Empty;

            MarcarCampo(txtTelefono1, null, principalCorrecto, string.Empty, mostrar);
            MarcarCampo(txtTelefono2, null, segundoCorrecto, string.Empty, mostrar);
            MarcarCampo(null, lblErrorTelefono, principalCorrecto && segundoCorrecto,
                mensaje, mostrar);

            return principalCorrecto && segundoCorrecto;
        }

        private bool ValidarEmail(bool mostrar)
        {
            string email = txtEmail.Text.Trim();
            bool correcto = email.Length == 0 || FormatoEmail.IsMatch(email);

            MarcarCampo(txtEmail, lblErrorEmail, correcto,
                "Formato no válido. Ejemplo: nombre@dominio.es", mostrar);

            return correcto;
        }

        private bool ValidarFechas(bool mostrar)
        {
            bool desdeCorrecto = dpContratoDesde.SelectedDate.HasValue;
            bool hastaCorrecto = dpContratoHasta.SelectedDate.HasValue;

            string mensajeDesde = "Selecciona la fecha de inicio del contrato.";
            string mensajeHasta = "Selecciona la fecha de fin del contrato.";

            if (desdeCorrecto && hastaCorrecto
                && dpContratoHasta.SelectedDate.Value.Date < dpContratoDesde.SelectedDate.Value.Date)
            {
                hastaCorrecto = false;
                mensajeHasta = "La fecha de fin no puede ser anterior a la de inicio.";
            }

            MarcarCampo(dpContratoDesde, lblErrorContratoDesde, desdeCorrecto,
                mensajeDesde, mostrar);
            MarcarCampo(dpContratoHasta, lblErrorContratoHasta, hastaCorrecto,
                mensajeHasta, mostrar);

            return desdeCorrecto && hastaCorrecto;
        }

        private static void MarcarCampo(Control campo, TextBlock etiquetaError,
                                        bool correcto, string mensaje, bool mostrar)
        {
            if (campo != null)
            {
                if (correcto)
                {
                    campo.ClearValue(Control.BorderBrushProperty);
                    campo.ClearValue(Control.BorderThicknessProperty);
                }
                else if (mostrar)
                {
                    Brush rojo = ObtenerRojo();
                    campo.BorderBrush = rojo;
                    campo.BorderThickness = new Thickness(2);
                }
            }

            if (etiquetaError == null) return;

            if (!correcto && mostrar)
            {
                etiquetaError.Text = mensaje;
                etiquetaError.Visibility = Visibility.Visible;
            }
            else if (correcto)
            {
                etiquetaError.Text = string.Empty;
                etiquetaError.Visibility = Visibility.Collapsed;
            }
        }

        private static Brush ObtenerRojo()
        {
            if (Application.Current != null
                && Application.Current.Resources.Contains("ColorPeligro"))
            {
                return (Brush)Application.Current.Resources["ColorPeligro"];
            }

            return Brushes.Red;
        }

        private static bool TelefonoValido(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return false;

            foreach (char c in texto)
            {
                if (!char.IsDigit(c) && !char.IsWhiteSpace(c)
                    && c != '-' && c != '+' && c != '(' && c != ')' && c != '.')
                    return false;
            }

            return SoloDigitos(texto).Length == 9;
        }

        private static string SoloDigitos(string texto)
        {
            return new string((texto ?? string.Empty).Where(char.IsDigit).ToArray());
        }

        private static string FormatearTelefono(string texto)
        {
            string digitos = SoloDigitos(texto);
            if (digitos.Length != 9) return (texto ?? string.Empty).Trim();

            return digitos.Substring(0, 3) + " " + digitos.Substring(3, 3) + " "
                   + digitos.Substring(6, 3);
        }

        // ===================== Guardar =====================

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!ComprobarFormulario(true)) return;

            DateTime desde = dpContratoDesde.SelectedDate.Value.Date;
            DateTime hasta = dpContratoHasta.SelectedDate.Value.Date;

            string disponibilidad = _copia.Disponibilidad;
            if (_monitorOriginal == null
                || _copia.ContratoDesde != desde
                || _copia.ContratoHasta != hasta)
            {
                disponibilidad = CalcularDisponibilidad(desde, hasta);
            }

            var resultado = new Monitor
            {
                Nombre = txtNombre.Text.Trim(),
                Apellidos = txtApellidos.Text.Trim(),
                Especialidad = ObtenerEspecialidad(),
                Disponibilidad = disponibilidad,
                Foto = _fotoRuta,
                Telefono1 = FormatearTelefono(txtTelefono1.Text),
                Telefono2 = string.IsNullOrWhiteSpace(txtTelefono2.Text)
                    ? string.Empty
                    : FormatearTelefono(txtTelefono2.Text),
                Email = txtEmail.Text.Trim(),
                Idiomas = LeerIdiomas(),
                Formacion = txtFormacion.Text.Trim(),
                Restricciones = txtRestricciones.Text.Trim(),
                ContratoDesde = desde,
                ContratoHasta = hasta,
                Actividades = new List<string>(_copia.Actividades ?? new List<string>())
            };

            Resultado = resultado;
            DialogResult = true;
        }

        private string ObtenerEspecialidad()
        {
            if (cmbEspecialidad.SelectedItem is string seleccionada)
                return seleccionada;

            return cmbEspecialidad.Text.Trim();
        }

        private List<string> LeerIdiomas()
        {
            var idiomas = new List<string>();

            if (chkEspanol.IsChecked == true) idiomas.Add("Español");
            if (chkIngles.IsChecked == true) idiomas.Add("Inglés");
            if (chkFrances.IsChecked == true) idiomas.Add("Francés");
            if (chkAleman.IsChecked == true) idiomas.Add("Alemán");
            if (chkItaliano.IsChecked == true) idiomas.Add("Italiano");

            foreach (string otro in txtOtroIdioma.Text.Split(new[] { ',', ';', '/' },
                                                              StringSplitOptions.RemoveEmptyEntries))
            {
                string limpio = otro.Trim();
                if (limpio.Length == 0) continue;

                if (!idiomas.Any(i => string.Equals(i, limpio,
                                                    StringComparison.CurrentCultureIgnoreCase)))
                {
                    idiomas.Add(limpio);
                }
            }

            return idiomas;
        }

        private static string CalcularDisponibilidad(DateTime desde, DateTime hasta)
        {
            if (desde.Month == 6 && desde.Day <= 1 && hasta.Month == 9 && hasta.Day >= 15)
                return "Todo el verano";

            if (desde.Month == 7 && desde.Day == 1 && hasta.Month == 8 && hasta.Day == 31)
                return "Julio y agosto";

            if (desde.Month == 7 && hasta.Month == 7)
                return "Julio";

            if (desde.Month == 8 && hasta.Month == 8)
                return "Agosto";

            if (desde.Month == 6 && hasta.Month == 6)
                return "Junio";

            if (desde.Month == 9 && hasta.Month == 9)
                return "Septiembre";

            return "Del " + desde.ToString("dd/MM") + " al " + hasta.ToString("dd/MM");
        }
    }
}
