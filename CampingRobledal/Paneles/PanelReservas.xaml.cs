using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using CampingRobledal.Datos;
using CampingRobledal.Utilidades;

namespace CampingRobledal
{
    public partial class PanelReservas : UserControl
    {
        private readonly ICollectionView vista;
        private readonly bool cargado = false;

        public PanelReservas()
        {
            InitializeComponent();

            // Desplegable de alojamientos: "Todos" + todos los nombres
            cmbFiltroAlojamiento.Items.Add("Todos");
            foreach (Parcela p in DatosPrueba.Parcelas) cmbFiltroAlojamiento.Items.Add(p.Nombre);
            foreach (Bungalow b in DatosPrueba.Bungalows) cmbFiltroAlojamiento.Items.Add(b.Nombre);
            cmbFiltroAlojamiento.SelectedIndex = 0;

            vista = CollectionViewSource.GetDefaultView(DatosPrueba.Reservas);
            vista.Filter = FiltrarReserva;
            dgReservas.ItemsSource = vista;
            cargado = true;
        }

        private bool FiltrarReserva(object o)
        {
            Reserva r = o as Reserva;
            if (r == null) return false;
            if (!Texto.Contiene(r.Cliente + " " + r.Telefono, txtBuscarReserva.Text)) return false;
            if (dpFiltroEntrada.SelectedDate.HasValue && r.Entrada.Date < dpFiltroEntrada.SelectedDate.Value.Date) return false;
            ComboBoxItem est = cmbFiltroEstadoReserva.SelectedItem as ComboBoxItem;
            if (cmbFiltroEstadoReserva.SelectedIndex > 0 && est != null && r.TextoEstado != est.Content.ToString()) return false;
            string aloj = cmbFiltroAlojamiento.SelectedItem as string;
            if (cmbFiltroAlojamiento.SelectedIndex > 0 && aloj != null && (r.Alojamiento == null || r.Alojamiento.Nombre != aloj)) return false;
            return true;
        }

        private void Filtro_Cambiado(object sender, RoutedEventArgs e)
        {
            if (!cargado) return;
            vista.Refresh();
        }

        private void btnLimpiarFiltros_Click(object sender, RoutedEventArgs e)
        {
            txtBuscarReserva.Clear();
            dpFiltroEntrada.SelectedDate = null;
            cmbFiltroEstadoReserva.SelectedIndex = 0;
            cmbFiltroAlojamiento.SelectedIndex = 0;
        }

        private Reserva ReservaSeleccionada { get { return dgReservas.SelectedItem as Reserva; } }

        private static string SiNo(bool valor) { return valor ? "sí" : "no"; }

        private void dgReservas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Reserva r = ReservaSeleccionada;
            bool hay = r != null;
            btnModificarReserva.IsEnabled = hay && r.Estado != EstadoReserva.Anulada && r.Estado != EstadoReserva.Finalizada;
            btnAnularReserva.IsEnabled = hay && (r.Estado == EstadoReserva.Confirmada);
            pnlFichaReserva.Visibility = hay ? Visibility.Visible : Visibility.Collapsed;
            lblFichaVaciaReserva.Visibility = hay ? Visibility.Collapsed : Visibility.Visible;
            if (!hay) return;

            btnCheckIn.IsEnabled = r.Estado == EstadoReserva.Confirmada;
            lblFichaNumeroReserva.Text = "Reserva " + r.Numero;
            lblFichaEstadoReserva.Text = r.TextoEstado;
            lblFichaEstadoReserva.Foreground = Brushes.White;
            lblFichaEstadoReserva.TextDecorations = r.Estado == EstadoReserva.Anulada ? TextDecorations.Strikethrough : null;
            pnlFichaEstadoReserva.Background = (Brush)new BrushConverter().ConvertFromString(r.ColorEstado);
            lblFichaDatosReserva.Text =
                "Cliente: " + r.Cliente + "\n" +
                "Teléfono: " + r.Telefono + (string.IsNullOrWhiteSpace(r.Telefono2) ? "" : " / " + r.Telefono2) + "\n" +
                "E-mail: " + (string.IsNullOrWhiteSpace(r.Email) ? "—" : r.Email) + "\n\n" +
                "Alojamiento: " + (r.Alojamiento == null ? "—" : r.Alojamiento.Nombre) + "\n" +
                "Entrada: " + r.Entrada.ToString("dd/MM/yyyy") + " a las " + r.HoraEntrada + "\n" +
                "Salida: " + r.Salida.ToString("dd/MM/yyyy") + " a las " + r.HoraSalida + "\n" +
                "Noches: " + r.Noches + "\n" +
                "Ocupantes: " + r.Ocupantes + "\n" +
                "Importe: " + r.Importe.ToString("0.00") + " €\n\n" +
                "Cuna: " + SiNo(r.Cuna) + " · Movilidad reducida: " + SiNo(r.Movilidad) + "\n" +
                "Menaje extra: " + SiNo(r.MenajeExtra) + " · Mascota: " + SiNo(r.Mascota) +
                (string.IsNullOrWhiteSpace(r.OtrasSolicitudes) ? "" : "\nOtras solicitudes: " + r.OtrasSolicitudes);
        }

        private void btnCheckIn_Click(object sender, RoutedEventArgs e)
        {
            Reserva r = ReservaSeleccionada;
            if (r == null || r.Estado != EstadoReserva.Confirmada) return;
            r.Estado = EstadoReserva.CheckIn;
            dgReservas.Items.Refresh();
            dgReservas_SelectionChanged(dgReservas, null);
            VentanaPrincipal.MostrarEstado("Check-in de la reserva " + r.Numero + " realizado");
        }

        private void btnVerAlojamiento_Click(object sender, RoutedEventArgs e)
        {
            VentanaPrincipal.Actual.IrAPestana("Alojamientos");
        }

        // Los diálogos de alta, modificación y confirmación llegan en la fase siguiente
        private void btnNuevaReserva_Click(object sender, RoutedEventArgs e) { Nueva(); }
        private void btnModificarReserva_Click(object sender, RoutedEventArgs e) { Modificar(); }
        private void btnAnularReserva_Click(object sender, RoutedEventArgs e) { Eliminar(); }

        private void dgReservas_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject origen = e.OriginalSource as DependencyObject;
            while (origen != null && !(origen is DataGridRow) && !(origen is DataGridColumnHeader))
                origen = VisualTreeHelper.GetParent(origen);
            if (origen is DataGridRow) Modificar();
        }

        private void dgReservas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete) { e.Handled = true; Eliminar(); }
            else if (e.Key == Key.Enter) { e.Handled = true; Modificar(); }
        }

        public void Nueva() { VentanaPrincipal.MostrarEstado("Nueva reserva: el formulario llega en la fase siguiente"); }

        public void Modificar()
        {
            if (!btnModificarReserva.IsEnabled) return;
            VentanaPrincipal.MostrarEstado("Modificar reserva " + ReservaSeleccionada.Numero + ": el formulario llega en la fase siguiente");
        }

        public void Eliminar()
        {
            if (!btnAnularReserva.IsEnabled) return;
            VentanaPrincipal.MostrarEstado("Anular reserva " + ReservaSeleccionada.Numero + ": la confirmación llega en la fase siguiente");
        }

        public void EnfocarBusqueda() { txtBuscarReserva.Focus(); txtBuscarReserva.SelectAll(); }
    }
}
