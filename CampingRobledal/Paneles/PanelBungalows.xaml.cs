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

        public void Nuevo() => AbrirFormulario(null);

        public void Modificar()
        {
            if (BungalowSeleccionada != null && btnModificarBungalow.IsEnabled)
                AbrirFormulario(BungalowSeleccionada);
        }

        private void AbrirFormulario(Bungalow original)
        {
            var dialogo = new DialogoBungalow(original) { Owner = Window.GetWindow(this) };
            if (dialogo.ShowDialog() != true) return;

            var bungalow = dialogo.Bungalow;
            if (original == null)
            {
                DatosPrueba.Bungalows.Add(bungalow);
                if (!vista.Contains(bungalow)) btnLimpiarFiltros_Click(this, null);
                dgBungalows.SelectedItem = bungalow;
                dgBungalows.ScrollIntoView(bungalow);
            }
            ActualizarVista();
            VentanaPrincipal.MostrarEstado($"Bungalow {bungalow.Nombre} guardado");
        }

        internal void ActualizarVista()
        {
            dgBungalows.Items.Refresh();
            dgBungalows_SelectionChanged(dgBungalows, null);
        }

        public void Eliminar()
        {
            var bungalow = BungalowSeleccionada;
            if (bungalow == null) return;
            var afectadas = DatosPrueba.Reservas.Where(r => r.Alojamiento == bungalow).ToList();
            int activas = afectadas.Count(r => r.Estado == EstadoReserva.Confirmada || r.Estado == EstadoReserva.CheckIn);
            string mensaje = $"Se eliminará el alojamiento. Quedarán sin alojamiento {afectadas.Count} reservas, de las cuales {activas} están confirmadas o con check-in.";
            if (!VentanaConfirmacion.Confirmar(Window.GetWindow(this), $"¿Eliminar {bungalow.Nombre}?", mensaje, "Eliminar")) return;

            foreach (var reserva in afectadas) reserva.Alojamiento = null;
            DatosPrueba.Bungalows.Remove(bungalow);
            ActualizarVista();
            VentanaPrincipal.MostrarEstado($"Bungalow {bungalow.Nombre} eliminado");
        }

        public void EnfocarBusqueda() { txtBuscarBungalow.Focus(); txtBuscarBungalow.SelectAll(); }
    }
}
