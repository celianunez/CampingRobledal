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
    public partial class DialogoParcela : Window
    {
        public Parcela Parcela { get; }
        private bool cargando = true;
        private bool modificado;
        private bool guardando;

        public DialogoParcela() : this(null) { }

        public DialogoParcela(Parcela original)
        {
            InitializeComponent();
            Parcela = original ?? new Parcela { Id = DatosPrueba.NuevoIdParcela() };
            Title = original == null ? "Nueva parcela" : "Modificar parcela " + original.Nombre;
            txtNombre.Text = Parcela.Nombre;
            cmbCategoria.SelectedIndex = (int)Parcela.Categoria;
            cmbEstado.SelectedIndex = (int)Parcela.Estado;
            txtTamano.Text = Parcela.TamanoM2.ToString();
            txtCapacidad.Text = Parcela.Capacidad.ToString();
            txtUbicacion.Text = Parcela.Ubicacion;
            chkElectricidad.IsChecked = Parcela.Electricidad;
            chkAgua.IsChecked = Parcela.Agua;
            chkDesague.IsChecked = Parcela.Desague;
            chkSombra.IsChecked = Parcela.Sombra;
            txtPrecioBaja.Text = Parcela.PrecioBaja.ToString();
            txtPrecioMedia.Text = Parcela.PrecioMedia.ToString();
            txtPrecioAlta.Text = Parcela.PrecioAlta.ToString();
            txtObservaciones.Text = Parcela.Observaciones;
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

            Revisar(txtNombre, lblErrorNombre, !string.IsNullOrWhiteSpace(txtNombre.Text) && !DatosPrueba.Parcelas.Any(otro => otro != Parcela && string.Equals(otro.Nombre, txtNombre.Text.Trim(), StringComparison.OrdinalIgnoreCase)), "Escribe un nombre sin repetir, p. ej. P-12.");
            Revisar(cmbCategoria, lblErrorCategoria, cmbCategoria.SelectedIndex >= 0, "Selecciona una opción de la lista, p. ej. la primera.");
            Revisar(txtTamano, lblErrorTamano, Validacion.TryEntero(txtTamano.Text, out _, false), "Escribe un número entero válido, p. ej. 2.");
            Revisar(txtCapacidad, lblErrorCapacidad, Validacion.TryEntero(txtCapacidad.Text, out _, false), "Escribe un número entero válido, p. ej. 2.");
            Revisar(txtPrecioBaja, lblErrorPrecioBaja, Validacion.TryDecimal(txtPrecioBaja.Text, out _), "Escribe un precio positivo, p. ej. 12,50.");
            Revisar(txtPrecioMedia, lblErrorPrecioMedia, Validacion.TryDecimal(txtPrecioMedia.Text, out _), "Escribe un precio positivo, p. ej. 12,50.");
            Revisar(txtPrecioAlta, lblErrorPrecioAlta, Validacion.TryDecimal(txtPrecioAlta.Text, out _), "Escribe un precio positivo, p. ej. 12,50.");
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
            Parcela.Nombre = txtNombre.Text.Trim();
            Parcela.Categoria = (CategoriaParcela)cmbCategoria.SelectedIndex;
            Parcela.Estado = (EstadoAlojamiento)cmbEstado.SelectedIndex;
            Validacion.TryEntero(txtTamano.Text, out int tamano, false);
            Parcela.TamanoM2 = tamano;
            Validacion.TryEntero(txtCapacidad.Text, out int capacidad, false);
            Parcela.Capacidad = capacidad;
            Parcela.Ubicacion = txtUbicacion.Text.Trim();
            Parcela.Electricidad = chkElectricidad.IsChecked == true;
            Parcela.Agua = chkAgua.IsChecked == true;
            Parcela.Desague = chkDesague.IsChecked == true;
            Parcela.Sombra = chkSombra.IsChecked == true;
            Validacion.TryDecimal(txtPrecioBaja.Text, out decimal precioBaja);
            Parcela.PrecioBaja = precioBaja;
            Validacion.TryDecimal(txtPrecioMedia.Text, out decimal precioMedia);
            Parcela.PrecioMedia = precioMedia;
            Validacion.TryDecimal(txtPrecioAlta.Text, out decimal precioAlta);
            Parcela.PrecioAlta = precioAlta;
            Parcela.Observaciones = txtObservaciones.Text.Trim();
            guardando = true;
            DialogResult = true;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape || cmbCategoria.IsDropDownOpen || cmbEstado.IsDropDownOpen) return;
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
