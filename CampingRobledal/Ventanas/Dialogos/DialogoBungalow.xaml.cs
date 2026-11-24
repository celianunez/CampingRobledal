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
    public partial class DialogoBungalow : Window
    {
        public Bungalow Bungalow { get; }
        private bool cargando = true;
        private bool modificado;
        private bool guardando;

        public DialogoBungalow() : this(null) { }

        public DialogoBungalow(Bungalow original)
        {
            InitializeComponent();
            Bungalow = original ?? new Bungalow { Id = DatosPrueba.NuevoIdBungalow() };
            Title = original == null ? "Nuevo bungalow" : "Modificar bungalow " + original.Nombre;
            txtNombre.Text = Bungalow.Nombre;
            cmbTipo.SelectedIndex = (int)Bungalow.Tipo;
            cmbEstado.SelectedIndex = (int)Bungalow.Estado;
            txtTamano.Text = Bungalow.TamanoM2.ToString();
            txtCapacidad.Text = Bungalow.Capacidad.ToString();
            txtPrecioNoche.Text = Bungalow.PrecioNoche.ToString();
            txtEstanciaMinima.Text = Bungalow.EstanciaMinima.ToString();
            chkAire.IsChecked = Bungalow.AireAcondicionado;
            chkCocina.IsChecked = Bungalow.Cocina;
            chkTerraza.IsChecked = Bungalow.Terraza;
            chkBano.IsChecked = Bungalow.BanoPropio;
            txtDescripcion.Text = Bungalow.Descripcion;
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

            Revisar(txtNombre, lblErrorNombre, !string.IsNullOrWhiteSpace(txtNombre.Text) && !DatosPrueba.Bungalows.Any(otro => otro != Bungalow && string.Equals(otro.Nombre, txtNombre.Text.Trim(), StringComparison.OrdinalIgnoreCase)), "Escribe un nombre sin repetir, p. ej. Encina.");
            Revisar(cmbTipo, lblErrorTipo, cmbTipo.SelectedIndex >= 0, "Selecciona una opción de la lista, p. ej. la primera.");
            Revisar(txtTamano, lblErrorTamano, Validacion.TryEntero(txtTamano.Text, out _, false), "Escribe un número entero válido, p. ej. 2.");
            Revisar(txtCapacidad, lblErrorCapacidad, Validacion.TryEntero(txtCapacidad.Text, out _, false), "Escribe un número entero válido, p. ej. 2.");
            Revisar(txtPrecioNoche, lblErrorPrecioNoche, Validacion.TryDecimal(txtPrecioNoche.Text, out _), "Escribe un precio positivo, p. ej. 12,50.");
            Revisar(txtEstanciaMinima, lblErrorEstanciaMinima, string.IsNullOrWhiteSpace(txtEstanciaMinima.Text) || Validacion.TryEntero(txtEstanciaMinima.Text, out _, false), "Escribe un número entero válido, p. ej. 2.");
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
            Bungalow.Nombre = txtNombre.Text.Trim();
            Bungalow.Tipo = (TipoBungalow)cmbTipo.SelectedIndex;
            Bungalow.Estado = (EstadoAlojamiento)cmbEstado.SelectedIndex;
            Validacion.TryEntero(txtTamano.Text, out int tamano, false);
            Bungalow.TamanoM2 = tamano;
            Validacion.TryEntero(txtCapacidad.Text, out int capacidad, false);
            Bungalow.Capacidad = capacidad;
            Validacion.TryDecimal(txtPrecioNoche.Text, out decimal precioNoche);
            Bungalow.PrecioNoche = precioNoche;
            Validacion.TryEntero(txtEstanciaMinima.Text, out int estanciaMinima, false);
            Bungalow.EstanciaMinima = txtEstanciaMinima.Text.Trim().Length == 0 ? null : estanciaMinima;
            Bungalow.AireAcondicionado = chkAire.IsChecked == true;
            Bungalow.Cocina = chkCocina.IsChecked == true;
            Bungalow.Terraza = chkTerraza.IsChecked == true;
            Bungalow.BanoPropio = chkBano.IsChecked == true;
            Bungalow.Descripcion = txtDescripcion.Text.Trim();
            guardando = true;
            DialogResult = true;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape || cmbTipo.IsDropDownOpen || cmbEstado.IsDropDownOpen) return;
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
