using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CampingRobledal.Datos;
using CampingRobledal.Utilidades;

namespace CampingRobledal
{
    public partial class DialogoMonitor : Window
    {
        public Monitor Monitor { get; }
        private bool cargando = true;
        private bool modificado;
        private bool guardando;
        private string foto;

        public DialogoMonitor() : this(null) { }

        public DialogoMonitor(Monitor original)
        {
            InitializeComponent();
            Monitor = original ?? new Monitor { InicioContrato = DateTime.Today, FinContrato = DateTime.Today.AddMonths(1) };
            Title = original == null ? "Nuevo monitor" : "Modificar monitor " + original.NombreCompleto;
            txtNombre.Text = Monitor.Nombre;
            txtApellidos.Text = Monitor.Apellidos;
            txtEspecialidad.Text = Monitor.Especialidad;
            txtTelefono.Text = Monitor.Telefono;
            txtTelefono2.Text = Monitor.Telefono2;
            txtCorreo.Text = Monitor.Correo;
            txtCorreo2.Text = Monitor.Correo2;
            txtFormacion.Text = Monitor.Formacion;
            dpInicioContrato.SelectedDate = Monitor.InicioContrato;
            dpFinContrato.SelectedDate = Monitor.FinContrato;
            txtRestricciones.Text = Monitor.Restricciones;
            foto = Monitor.Foto;
            if (!string.IsNullOrWhiteSpace(foto)) MostrarFoto(foto);
            chkEspanol.IsChecked = Monitor.Idiomas.Contains("Español");
            chkIngles.IsChecked = Monitor.Idiomas.Contains("Inglés");
            chkFrances.IsChecked = Monitor.Idiomas.Contains("Francés");
            chkAleman.IsChecked = Monitor.Idiomas.Contains("Alemán");
            Validacion.VigilarCambios(this, () => { if (!cargando) modificado = true; });
            cargando = false;
            Validar(false, null);
        }

        private bool MostrarFoto(string ruta)
        {
            try
            {
                var imagen = new System.Windows.Media.Imaging.BitmapImage();
                imagen.BeginInit();
                imagen.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                imagen.DecodePixelWidth = 320;
                imagen.UriSource = new Uri(ruta, UriKind.RelativeOrAbsolute);
                imagen.EndInit();
                imgFoto.Source = imagen;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void btnCargarFoto_Click(object sender, RoutedEventArgs e)
        {
            var selector = new Microsoft.Win32.OpenFileDialog { Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp" };
            if (selector.ShowDialog(this) != true) return;
            if (MostrarFoto(selector.FileName))
            {
                foto = selector.FileName;
                modificado = true;
            }
            else MessageBox.Show(this, "No se pudo leer la imagen. Elige otra foto JPG o PNG.", "Cargar foto");
        }

        private bool Validar(bool todo, Control campo)
        {
            if (cargando) return false;
            bool valido = true;
            void Revisar(Control control, TextBlock error, bool correcto, string mensaje)
            {
                bool mostrar = todo || campo == control || error.Visibility == Visibility.Visible;
                valido &= Validacion.Comprobar(mostrar, control, error, correcto, mensaje);
            }

            Revisar(txtNombre, lblErrorNombre, !string.IsNullOrWhiteSpace(txtNombre.Text) && !DatosPrueba.Monitores.Any(otro => otro != Monitor && string.Equals(otro.NombreCompleto, txtNombre.Text.Trim() + " " + txtApellidos.Text.Trim(), StringComparison.OrdinalIgnoreCase)), "Escribe un nombre sin repetir, p. ej. Ana López.");
            Revisar(txtApellidos, lblErrorApellidos, !string.IsNullOrWhiteSpace(txtApellidos.Text), "Completa este campo, p. ej. López.");
            Revisar(txtTelefono, lblErrorTelefono, Validacion.EsTelefono(txtTelefono.Text), "El teléfono debe tener 9 cifras, p. ej. 612345678.");
            Revisar(txtTelefono2, lblErrorTelefono2, string.IsNullOrWhiteSpace(txtTelefono2.Text) || Validacion.EsTelefono(txtTelefono2.Text), "El teléfono debe tener 9 cifras, p. ej. 612345678.");
            Revisar(txtCorreo, lblErrorCorreo, string.IsNullOrWhiteSpace(txtCorreo.Text) || Validacion.EsEmail(txtCorreo.Text), "Escribe un correo válido, p. ej. nombre@correo.es.");
            Revisar(txtCorreo2, lblErrorCorreo2, string.IsNullOrWhiteSpace(txtCorreo2.Text) || Validacion.EsEmail(txtCorreo2.Text), "Escribe un correo válido, p. ej. nombre@correo.es.");
            Revisar(dpInicioContrato, lblErrorInicio, dpInicioContrato.SelectedDate.HasValue, "Indica el inicio del contrato, p. ej. 01/06/2027.");
            Revisar(dpFinContrato, lblErrorFin, dpFinContrato.SelectedDate > dpInicioContrato.SelectedDate, "Elige un fin posterior al inicio, p. ej. el día siguiente.");
            btnGuardar.IsEnabled = valido;
            return valido;
        }

        private void Campo_LostFocus(object sender, RoutedEventArgs e) => Validar(false, sender as Control);
        private void Campo_TextChanged(object sender, TextChangedEventArgs e) => Validar(false, null);
        private void Campo_Cambiado(object sender, SelectionChangedEventArgs e) => Validar(false, null);
        private void btnGuardar_Click(object sender, RoutedEventArgs e) => Guardar();
        private void btnCancelar_Click(object sender, RoutedEventArgs e) => Close();

        private void Guardar()
        {
            if (!Validar(true, null)) return;
            Monitor.Nombre = txtNombre.Text.Trim();
            Monitor.Apellidos = txtApellidos.Text.Trim();
            Monitor.Especialidad = txtEspecialidad.Text.Trim();
            Monitor.Telefono = txtTelefono.Text.Trim();
            Monitor.Telefono2 = txtTelefono2.Text.Trim();
            Monitor.Correo = txtCorreo.Text.Trim();
            Monitor.Correo2 = txtCorreo2.Text.Trim();
            Monitor.Formacion = txtFormacion.Text.Trim();
            Monitor.InicioContrato = dpInicioContrato.SelectedDate.Value;
            Monitor.FinContrato = dpFinContrato.SelectedDate.Value;
            Monitor.Restricciones = txtRestricciones.Text.Trim();
            Monitor.Foto = foto;
            Monitor.Idiomas.Clear();
            if (chkEspanol.IsChecked == true) Monitor.Idiomas.Add("Español");
            if (chkIngles.IsChecked == true) Monitor.Idiomas.Add("Inglés");
            if (chkFrances.IsChecked == true) Monitor.Idiomas.Add("Francés");
            if (chkAleman.IsChecked == true) Monitor.Idiomas.Add("Alemán");
            guardando = true;
            DialogResult = true;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape) return;
            e.Handled = true;
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (guardando || !modificado) return;
            var respuesta = VentanaConfirmacion.PreguntarSalida(this, "Tienes cambios pendientes en este formulario.");
            e.Cancel = respuesta != ResultadoConfirmacion.Descartar;
            if (respuesta == ResultadoConfirmacion.Accion && Validar(true, null))
                Dispatcher.BeginInvoke(new Action(Guardar));
        }
    }
}
