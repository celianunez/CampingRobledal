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

        // Los diálogos de alta, modificación y confirmación llegan en la fase siguiente
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

        public void Nueva() { VentanaPrincipal.MostrarEstado("Nueva parcela: el formulario llega en la fase siguiente"); }

        public void Modificar()
        {
            if (ParcelaSeleccionada == null) return;
            VentanaPrincipal.MostrarEstado("Modificar " + ParcelaSeleccionada.Nombre + ": el formulario llega en la fase siguiente");
        }

        public void Eliminar()
        {
            if (ParcelaSeleccionada == null) return;
            VentanaPrincipal.MostrarEstado("Eliminar " + ParcelaSeleccionada.Nombre + ": la confirmación llega en la fase siguiente");
        }

        public void EnfocarBusqueda() { txtBuscarParcela.Focus(); txtBuscarParcela.SelectAll(); }
    }
}
