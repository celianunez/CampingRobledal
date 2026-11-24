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
    public partial class DialogoReserva : Window
    {
        public Reserva Reserva { get; }
        private bool cargando = true;
        private bool modificado;
        private bool guardando;

        public DialogoReserva() : this(null) { }

        public DialogoReserva(Reserva original)
        {
            InitializeComponent();
            Reserva = original ?? new Reserva { Numero = DatosPrueba.NuevoNumeroReserva(), Entrada = DateTime.Today, Salida = DateTime.Today.AddDays(1), Ocupantes = 1 };
            Title = original == null ? "Nueva reserva" : "Modificar reserva " + original.Numero;
            dpEntrada.SelectedDate = Reserva.Entrada;
            dpSalida.SelectedDate = Reserva.Salida;
            txtHoraEntrada.Text = Reserva.HoraEntrada;
            txtHoraSalida.Text = Reserva.HoraSalida;
            txtOcupantes.Text = Reserva.Ocupantes.ToString();
            txtNombre.Text = Reserva.Nombre;
            txtApellidos.Text = Reserva.Apellidos;
            txtTelefono.Text = Reserva.Telefono;
            txtTelefono2.Text = Reserva.Telefono2;
            txtEmail.Text = Reserva.Email;
            chkCuna.IsChecked = Reserva.Cuna;
            chkMovilidad.IsChecked = Reserva.Movilidad;
            chkMenajeExtra.IsChecked = Reserva.MenajeExtra;
            chkMascota.IsChecked = Reserva.Mascota;
            txtOtrasSolicitudes.Text = Reserva.OtrasSolicitudes;
            cmbAlojamiento.ItemsSource = DatosPrueba.TodosLosAlojamientos();
            cmbAlojamiento.SelectedItem = Reserva.Alojamiento;
            Validacion.VigilarCambios(this, () => { if (!cargando) modificado = true; });
            cargando = false;
            Validar(false, null);
        }

        public DialogoReserva(Alojamiento alojamiento, DateTime entrada, DateTime salida) : this()
        {
            cargando = true;
            cmbAlojamiento.SelectedItem = alojamiento;
            dpEntrada.SelectedDate = entrada.Date;
            dpSalida.SelectedDate = salida.Date;
            cargando = false;
            Validar(false, null);
        }

        private void btnVerDisponibilidad_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, "El calendario de disponibilidad llegará en la fase 4.", "Disponibilidad");
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

            var alojamiento = cmbAlojamiento.SelectedItem as Alojamiento;
            var entrada = dpEntrada.SelectedDate;
            var salida = dpSalida.SelectedDate;
            bool fechasValidas = entrada.HasValue && salida > entrada;
            bool solapada = fechasValidas && alojamiento != null && DatosPrueba.Reservas.Any(otra =>
                otra != Reserva && otra.Estado != EstadoReserva.Anulada && otra.Alojamiento == alojamiento
                && otra.Entrada < salida && otra.Salida > entrada);
            lblCapacidad.Text = alojamiento == null ? "" : $"Máx. {alojamiento.Capacidad} en este alojamiento";
            var estancia = new Reserva { Alojamiento = alojamiento, Entrada = entrada.GetValueOrDefault(), Salida = salida.GetValueOrDefault() };
            lblNoches.Text = fechasValidas ? $"Noches: {estancia.Noches}" : "Noches: —";
            lblImporte.Text = "Importe: —";
            if (fechasValidas && alojamiento != null)
            {
                try { lblImporte.Text = $"Importe: {estancia.Importe:0.00} €"; }
                catch (OverflowException) { valido = false; lblImporte.Text = "Importe demasiado alto. Reduce la estancia o revisa la tarifa."; }
            }
            Revisar(txtHoraEntrada, lblErrorHoraEntrada, string.IsNullOrWhiteSpace(txtHoraEntrada.Text) || Validacion.EsHora(txtHoraEntrada.Text), "Indica una hora válida, p. ej. 09:30.");
            Revisar(txtHoraSalida, lblErrorHoraSalida, string.IsNullOrWhiteSpace(txtHoraSalida.Text) || Validacion.EsHora(txtHoraSalida.Text), "Indica una hora válida, p. ej. 09:30.");
            Revisar(txtOcupantes, lblErrorOcupantes, Validacion.TryEntero(txtOcupantes.Text, out int ocupantes, false) && alojamiento != null && ocupantes <= alojamiento.Capacidad, "Indica un número positivo dentro de la capacidad, p. ej. 1.");
            Revisar(txtNombre, lblErrorNombre, !string.IsNullOrWhiteSpace(txtNombre.Text), "Completa este campo, p. ej. Ana.");
            Revisar(txtApellidos, lblErrorApellidos, !string.IsNullOrWhiteSpace(txtApellidos.Text), "Completa este campo, p. ej. López.");
            Revisar(txtTelefono, lblErrorTelefono, Validacion.EsTelefono(txtTelefono.Text), "El teléfono debe tener 9 cifras, p. ej. 612345678.");
            Revisar(txtTelefono2, lblErrorTelefono2, string.IsNullOrWhiteSpace(txtTelefono2.Text) || Validacion.EsTelefono(txtTelefono2.Text), "El teléfono debe tener 9 cifras, p. ej. 612345678.");
            Revisar(txtEmail, lblErrorEmail, string.IsNullOrWhiteSpace(txtEmail.Text) || Validacion.EsEmail(txtEmail.Text), "Escribe un correo válido, p. ej. nombre@correo.es.");
            Revisar(cmbAlojamiento, lblErrorAlojamiento, alojamiento != null, "Selecciona un alojamiento, p. ej. una parcela.");
            Revisar(dpEntrada, lblErrorEntrada, entrada.HasValue, "Indica la llegada, p. ej. 01/07/2027.");
            Revisar(dpSalida, lblErrorSalida, fechasValidas && !solapada, solapada ? "Ya hay una reserva en ese periodo. Elige fechas libres, p. ej. desde su salida." : "La salida debe ser posterior a la entrada, p. ej. el día siguiente.");
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
            Reserva.Entrada = dpEntrada.SelectedDate.Value;
            Reserva.Salida = dpSalida.SelectedDate.Value;
            Reserva.HoraEntrada = txtHoraEntrada.Text.Trim();
            Reserva.HoraSalida = txtHoraSalida.Text.Trim();
            Validacion.TryEntero(txtOcupantes.Text, out int ocupantes, false);
            Reserva.Ocupantes = ocupantes;
            Reserva.Nombre = txtNombre.Text.Trim();
            Reserva.Apellidos = txtApellidos.Text.Trim();
            Reserva.Telefono = txtTelefono.Text.Trim();
            Reserva.Telefono2 = txtTelefono2.Text.Trim();
            Reserva.Email = txtEmail.Text.Trim();
            Reserva.Cuna = chkCuna.IsChecked == true;
            Reserva.Movilidad = chkMovilidad.IsChecked == true;
            Reserva.MenajeExtra = chkMenajeExtra.IsChecked == true;
            Reserva.Mascota = chkMascota.IsChecked == true;
            Reserva.OtrasSolicitudes = txtOtrasSolicitudes.Text.Trim();
            Reserva.Alojamiento = (Alojamiento)cmbAlojamiento.SelectedItem;
            guardando = true;
            DialogResult = true;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape || cmbAlojamiento.IsDropDownOpen) return;
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
