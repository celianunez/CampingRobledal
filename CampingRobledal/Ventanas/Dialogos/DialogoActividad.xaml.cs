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
    public partial class DialogoActividad : Window
    {
        public Actividad Actividad { get; }
        private bool cargando = true;
        private bool modificado;
        private bool guardando;

        public DialogoActividad() : this(null) { }

        public DialogoActividad(Actividad original)
        {
            InitializeComponent();
            Actividad = original ?? new Actividad { Destinatarios = Destinatarios.Todos };
            Title = original == null ? "Nueva actividad" : "Modificar actividad " + original.Nombre;
            txtNombre.Text = Actividad.Nombre;
            txtEdadMinima.Text = Actividad.EdadMinima.ToString();
            txtHoraInicio.Text = Actividad.HoraInicio;
            txtHoraFin.Text = Actividad.HoraFin;
            txtCupoMinimo.Text = Actividad.CupoMinimo.ToString();
            txtCupoMaximo.Text = Actividad.CupoMaximo.ToString();
            txtInscritos.Text = Actividad.Inscritos.ToString();
            txtPrecioHora.Text = Actividad.PrecioHora.ToString();
            txtPrecioMes.Text = Actividad.PrecioMes.ToString();
            txtDescripcion.Text = Actividad.Descripcion;
            txtMateriales.Text = Actividad.Materiales;
            cmbArea.SelectedValuePath = "Content";
            cmbArea.SelectedValue = Actividad.Area;
            lstMonitores.ItemsSource = DatosPrueba.Monitores.Where(m => m.Activo).ToList();
            foreach (var monitor in Actividad.Monitores.Where(m => m.Activo)) lstMonitores.SelectedItems.Add(monitor);
            rbAdultos.IsChecked = Actividad.Destinatarios == Destinatarios.Adultos;
            rbNinos.IsChecked = Actividad.Destinatarios == Destinatarios.Ninos;
            rbTodos.IsChecked = Actividad.Destinatarios == Destinatarios.Todos;
            chkDomingo.IsChecked = Actividad.Dias.Contains(DayOfWeek.Sunday);
            chkLunes.IsChecked = Actividad.Dias.Contains(DayOfWeek.Monday);
            chkMartes.IsChecked = Actividad.Dias.Contains(DayOfWeek.Tuesday);
            chkMiercoles.IsChecked = Actividad.Dias.Contains(DayOfWeek.Wednesday);
            chkJueves.IsChecked = Actividad.Dias.Contains(DayOfWeek.Thursday);
            chkViernes.IsChecked = Actividad.Dias.Contains(DayOfWeek.Friday);
            chkSabado.IsChecked = Actividad.Dias.Contains(DayOfWeek.Saturday);
            Validacion.VigilarCambios(this, () => { if (!cargando) modificado = true; });
            cargando = false;
            Validar(false, null);
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

            Revisar(txtNombre, lblErrorNombre, !string.IsNullOrWhiteSpace(txtNombre.Text) && !DatosPrueba.Actividades.Any(otro => otro != Actividad && string.Equals(otro.Nombre, txtNombre.Text.Trim(), StringComparison.OrdinalIgnoreCase)), "Escribe un nombre sin repetir, p. ej. Senderismo.");
            Revisar(txtEdadMinima, lblErrorEdadMinima, string.IsNullOrWhiteSpace(txtEdadMinima.Text) || Validacion.TryEntero(txtEdadMinima.Text, out _, true), "Escribe un número entero válido, p. ej. 2.");
            Revisar(txtHoraInicio, lblErrorHoraInicio, Validacion.EsHora(txtHoraInicio.Text), "Indica una hora válida, p. ej. 09:30.");
            Revisar(txtHoraFin, lblErrorHoraFin, Validacion.EsHora(txtHoraFin.Text) && (!Validacion.EsHora(txtHoraInicio.Text) || TimeSpan.Parse(txtHoraFin.Text.Trim()) > TimeSpan.Parse(txtHoraInicio.Text.Trim())), "Indica una hora posterior al inicio, p. ej. 12:30.");
            Revisar(txtCupoMinimo, lblErrorCupoMinimo, Validacion.TryEntero(txtCupoMinimo.Text, out _, false), "Escribe un número entero válido, p. ej. 2.");
            Revisar(txtCupoMaximo, lblErrorCupoMaximo, Validacion.TryEntero(txtCupoMaximo.Text, out _, false) && (!Validacion.TryEntero(txtCupoMinimo.Text, out int minimo, false) || txtCupoMaximo.Text.Trim().Length > 0 && int.Parse(txtCupoMaximo.Text) >= minimo), "El cupo máximo debe cubrir el mínimo, p. ej. 12.");
            Revisar(txtInscritos, lblErrorInscritos, Validacion.TryEntero(txtInscritos.Text, out _, true) && (!Validacion.TryEntero(txtCupoMaximo.Text, out int maximo, false) || int.Parse(txtInscritos.Text) <= maximo), "Indica entre cero y el cupo máximo, p. ej. 0.");
            Revisar(txtPrecioHora, lblErrorPrecioHora, Validacion.TryDecimal(txtPrecioHora.Text, out _), "Escribe un precio positivo, p. ej. 12,50.");
            Revisar(txtPrecioMes, lblErrorPrecioMes, string.IsNullOrWhiteSpace(txtPrecioMes.Text) || Validacion.TryDecimal(txtPrecioMes.Text, out _), "Escribe un precio positivo, p. ej. 12,50.");
            Revisar(cmbArea, lblErrorArea, cmbArea.SelectedIndex >= 0, "Selecciona un área, p. ej. Piscina.");
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
            Actividad.Nombre = txtNombre.Text.Trim();
            Validacion.TryEntero(txtEdadMinima.Text, out int edadMinima, true);
            Actividad.EdadMinima = edadMinima;
            Actividad.HoraInicio = txtHoraInicio.Text.Trim();
            Actividad.HoraFin = txtHoraFin.Text.Trim();
            Validacion.TryEntero(txtCupoMinimo.Text, out int cupoMinimo, false);
            Actividad.CupoMinimo = cupoMinimo;
            Validacion.TryEntero(txtCupoMaximo.Text, out int cupoMaximo, false);
            Actividad.CupoMaximo = cupoMaximo;
            Validacion.TryEntero(txtInscritos.Text, out int inscritos, true);
            Actividad.Inscritos = inscritos;
            Validacion.TryDecimal(txtPrecioHora.Text, out decimal precioHora);
            Actividad.PrecioHora = precioHora;
            Validacion.TryDecimal(txtPrecioMes.Text, out decimal precioMes);
            Actividad.PrecioMes = txtPrecioMes.Text.Trim().Length == 0 ? null : precioMes;
            Actividad.Descripcion = txtDescripcion.Text.Trim();
            Actividad.Materiales = txtMateriales.Text.Trim();
            Actividad.Area = (string)cmbArea.SelectedValue;
            Actividad.Monitores = Actividad.Monitores.Where(m => !m.Activo).Concat(lstMonitores.SelectedItems.Cast<Monitor>()).ToList();
            Actividad.Destinatarios = rbAdultos.IsChecked == true ? Destinatarios.Adultos : rbNinos.IsChecked == true ? Destinatarios.Ninos : Destinatarios.Todos;
            Actividad.Dias.Clear();
            if (chkDomingo.IsChecked == true) Actividad.Dias.Add(DayOfWeek.Sunday);
            if (chkLunes.IsChecked == true) Actividad.Dias.Add(DayOfWeek.Monday);
            if (chkMartes.IsChecked == true) Actividad.Dias.Add(DayOfWeek.Tuesday);
            if (chkMiercoles.IsChecked == true) Actividad.Dias.Add(DayOfWeek.Wednesday);
            if (chkJueves.IsChecked == true) Actividad.Dias.Add(DayOfWeek.Thursday);
            if (chkViernes.IsChecked == true) Actividad.Dias.Add(DayOfWeek.Friday);
            if (chkSabado.IsChecked == true) Actividad.Dias.Add(DayOfWeek.Saturday);
            guardando = true;
            DialogResult = true;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape || cmbArea.IsDropDownOpen) return;
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
