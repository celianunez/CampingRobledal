using System.Linq;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using CampingRobledal.Datos;
using CampingRobledal.Utilidades;

namespace CampingRobledal
{
    public partial class PanelParcelas : UserControl
    {
        private readonly ICollectionView vista;
        private readonly bool cargado = false;

        public PanelParcelas()
        {
            InitializeComponent();
            vista = CollectionViewSource.GetDefaultView(DatosPrueba.Parcelas);
            vista.Filter = FiltrarParcela;
            dgParcelas.ItemsSource = vista;
            cargado = true;
        }

        private static string Seleccion(ComboBox cmb)
        {
            ComboBoxItem item = cmb.SelectedItem as ComboBoxItem;
            return (cmb.SelectedIndex <= 0 || item == null) ? null : item.Content.ToString();
        }

        private bool FiltrarParcela(object o)
        {
            Parcela p = o as Parcela;
            if (p == null) return false;
            if (!Texto.Contiene(p.Id + " " + p.Nombre, txtBuscarParcela.Text)) return false;
            string cat = Seleccion(cmbFiltroCategoria);
            if (cat != null && p.TextoCategoria != cat) return false;
            string est = Seleccion(cmbFiltroEstadoParcela);
            if (est != null && p.TextoEstado != est) return false;
            return true;
        }

        private void Filtro_Cambiado(object sender, RoutedEventArgs e)
        {
            if (!cargado) return;
            vista.Refresh();
        }

        private void btnLimpiarFiltros_Click(object sender, RoutedEventArgs e)
        {
            txtBuscarParcela.Clear();
            cmbFiltroCategoria.SelectedIndex = 0;
            cmbFiltroEstadoParcela.SelectedIndex = 0;
        }

        private Parcela ParcelaSeleccionada { get { return dgParcelas.SelectedItem as Parcela; } }

        private void dgParcelas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Parcela p = ParcelaSeleccionada;
            bool hay = p != null;
            btnModificarParcela.IsEnabled = hay;
            btnEliminarParcela.IsEnabled = hay;
            pnlFichaParcela.Visibility = hay ? Visibility.Visible : Visibility.Collapsed;
            lblFichaVaciaParcela.Visibility = hay ? Visibility.Collapsed : Visibility.Visible;
            if (!hay) return;

            lblFichaNombreParcela.Text = p.Nombre + " (" + p.Id + ")";
            lblFichaEstadoParcela.Text = p.TextoEstado;
            lblFichaEstadoParcela.Foreground = (Brush)new BrushConverter().ConvertFromString(p.ColorTextoEstado);
            pnlFichaEstadoParcela.Background = (Brush)new BrushConverter().ConvertFromString(p.ColorEstado);
            lblFichaDatosParcela.Text =
                "Categoría: " + p.TextoCategoria + "\n" +
                "Ubicación: " + p.Ubicacion + "\n" +
                "Tamaño: " + p.TamanoM2 + " m²\n" +
                "Capacidad: " + p.Capacidad + " personas\n" +
                "Servicios: " + p.TextoServicios + "\n\n" +
                "Precio por noche\n" +
                "  Temporada baja: " + p.PrecioBaja.ToString("0.00") + " €\n" +
                "  Temporada media: " + p.PrecioMedia.ToString("0.00") + " €\n" +
                "  Temporada alta: " + p.PrecioAlta.ToString("0.00") + " €" +
                (string.IsNullOrWhiteSpace(p.Observaciones) ? "" : "\n\nObservaciones: " + p.Observaciones);
        }

        private void btnNuevaParcela_Click(object sender, RoutedEventArgs e) { Nueva(); }
        private void btnModificarParcela_Click(object sender, RoutedEventArgs e) { Modificar(); }
        private void btnEliminarParcela_Click(object sender, RoutedEventArgs e) { Eliminar(); }

        private void dgParcelas_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Solo si el doble clic ha sido sobre una fila, no sobre la cabecera
            DependencyObject origen = e.OriginalSource as DependencyObject;
            while (origen != null && !(origen is DataGridRow) && !(origen is DataGridColumnHeader))
                origen = VisualTreeHelper.GetParent(origen);
            if (origen is DataGridRow) Modificar();
        }

        private void dgParcelas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete) { e.Handled = true; Eliminar(); }
            else if (e.Key == Key.Enter) { e.Handled = true; Modificar(); }
        }

        private void btnVerDisponibilidad_Click(object sender, RoutedEventArgs e)
        {
            VentanaPrincipal.MostrarEstado("La disponibilidad se abrirá desde aquí en una fase posterior");
        }

        public void Nueva() => AbrirFormulario(null);

        public void Modificar()
        {
            if (ParcelaSeleccionada != null && btnModificarParcela.IsEnabled)
                AbrirFormulario(ParcelaSeleccionada);
        }

        private void AbrirFormulario(Parcela original)
        {
            var dialogo = new DialogoParcela(original) { Owner = Window.GetWindow(this) };
            if (dialogo.ShowDialog() != true) return;

            var parcela = dialogo.Parcela;
            if (original == null)
            {
                DatosPrueba.Parcelas.Add(parcela);
                if (!vista.Contains(parcela)) btnLimpiarFiltros_Click(this, null);
                dgParcelas.SelectedItem = parcela;
                dgParcelas.ScrollIntoView(parcela);
            }
            ActualizarVista();
            VentanaPrincipal.MostrarEstado($"Parcela {parcela.Nombre} guardada");
        }

        internal void ActualizarVista()
        {
            dgParcelas.Items.Refresh();
            dgParcelas_SelectionChanged(dgParcelas, null);
        }

        public void Eliminar()
        {
            var parcela = ParcelaSeleccionada;
            if (parcela == null) return;
            var afectadas = DatosPrueba.Reservas.Where(r => r.Alojamiento == parcela).ToList();
            int activas = afectadas.Count(r => r.Estado == EstadoReserva.Confirmada || r.Estado == EstadoReserva.CheckIn);
            string mensaje = $"Se eliminará el alojamiento. Quedarán sin alojamiento {afectadas.Count} reservas, de las cuales {activas} están confirmadas o con check-in.";
            if (!VentanaConfirmacion.Confirmar(Window.GetWindow(this), $"¿Eliminar {parcela.Nombre}?", mensaje, "Eliminar")) return;

            foreach (var reserva in afectadas) reserva.Alojamiento = null;
            DatosPrueba.Parcelas.Remove(parcela);
            ActualizarVista();
            VentanaPrincipal.MostrarEstado($"Parcela {parcela.Nombre} eliminada");
        }

        public void EnfocarBusqueda() { txtBuscarParcela.Focus(); txtBuscarParcela.SelectAll(); }
    }
}
