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
    public partial class PanelBungalows : UserControl
    {
        private readonly ICollectionView vista;
        private readonly bool cargado = false;

        public PanelBungalows()
        {
            InitializeComponent();
            vista = CollectionViewSource.GetDefaultView(DatosPrueba.Bungalows);
            vista.Filter = FiltrarBungalow;
            dgBungalows.ItemsSource = vista;
            cargado = true;
        }

        private static string Seleccion(ComboBox cmb)
        {
            ComboBoxItem item = cmb.SelectedItem as ComboBoxItem;
            return (cmb.SelectedIndex <= 0 || item == null) ? null : item.Content.ToString();
        }

        private bool FiltrarBungalow(object o)
        {
            Bungalow p = o as Bungalow;
            if (p == null) return false;
            if (!Texto.Contiene(p.Id + " " + p.Nombre, txtBuscarBungalow.Text)) return false;
            string tipo = Seleccion(cmbFiltroTipo);
            if (tipo != null && p.TextoTipo != tipo) return false;
            string est = Seleccion(cmbFiltroEstadoBungalow);
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
            txtBuscarBungalow.Clear();
            cmbFiltroTipo.SelectedIndex = 0;
            cmbFiltroEstadoBungalow.SelectedIndex = 0;
        }

        private Bungalow BungalowSeleccionada { get { return dgBungalows.SelectedItem as Bungalow; } }

        private void dgBungalows_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bungalow p = BungalowSeleccionada;
            bool hay = p != null;
            btnModificarBungalow.IsEnabled = hay;
            btnEliminarBungalow.IsEnabled = hay;
            pnlFichaBungalow.Visibility = hay ? Visibility.Visible : Visibility.Collapsed;
            lblFichaVaciaBungalow.Visibility = hay ? Visibility.Collapsed : Visibility.Visible;
            if (!hay) return;

            lblFichaNombreBungalow.Text = p.Nombre + " (" + p.Id + ")";
            lblFichaEstadoBungalow.Text = p.TextoEstado;
            lblFichaEstadoBungalow.Foreground = (Brush)new BrushConverter().ConvertFromString(p.ColorTextoEstado);
            pnlFichaEstadoBungalow.Background = (Brush)new BrushConverter().ConvertFromString(p.ColorEstado);
            lblFichaDatosBungalow.Text =
                "Tipo: " + p.TextoTipo + "\n" +
                "Tamaño: " + p.TamanoM2 + " m²\n" +
                "Capacidad: " + p.Capacidad + " personas\n" +
                "Precio por noche: " + p.PrecioNoche.ToString("0.00") + " €\n" +
                "Estancia mínima: " + p.TextoEstancia + "\n" +
                "Equipamiento: " + p.TextoEquipamiento +
                (string.IsNullOrWhiteSpace(p.Descripcion) ? "" : "\n\n" + p.Descripcion);
        }

        // Los diálogos de alta, modificación y confirmación llegan en la fase siguiente
        private void btnNuevoBungalow_Click(object sender, RoutedEventArgs e) { Nuevo(); }
        private void btnModificarBungalow_Click(object sender, RoutedEventArgs e) { Modificar(); }
        private void btnEliminarBungalow_Click(object sender, RoutedEventArgs e) { Eliminar(); }

        private void dgBungalows_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Solo si el doble clic ha sido sobre una fila, no sobre la cabecera
            DependencyObject origen = e.OriginalSource as DependencyObject;
            while (origen != null && !(origen is DataGridRow) && !(origen is DataGridColumnHeader))
                origen = VisualTreeHelper.GetParent(origen);
            if (origen is DataGridRow) Modificar();
        }

        private void dgBungalows_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete) { e.Handled = true; Eliminar(); }
            else if (e.Key == Key.Enter) { e.Handled = true; Modificar(); }
        }

        public void Nuevo() { VentanaPrincipal.MostrarEstado("Nuevo bungalow: el formulario llega en la fase siguiente"); }

        public void Modificar()
        {
            if (BungalowSeleccionada == null) return;
            VentanaPrincipal.MostrarEstado("Modificar " + BungalowSeleccionada.Nombre + ": el formulario llega en la fase siguiente");
        }

        public void Eliminar()
        {
            if (BungalowSeleccionada == null) return;
            VentanaPrincipal.MostrarEstado("Eliminar " + BungalowSeleccionada.Nombre + ": la confirmación llega en la fase siguiente");
        }

        public void EnfocarBusqueda() { txtBuscarBungalow.Focus(); txtBuscarBungalow.SelectAll(); }
    }
}
