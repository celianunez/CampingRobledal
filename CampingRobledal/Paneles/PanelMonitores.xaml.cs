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
    public partial class PanelMonitores : UserControl
    {
        private readonly ICollectionView vista;
        private readonly bool cargado = false;

        public PanelMonitores()
        {
            InitializeComponent();
            vista = CollectionViewSource.GetDefaultView(DatosPrueba.Monitores);
            vista.Filter = FiltrarMonitor;
            dgMonitores.ItemsSource = vista;
            cargado = true;
            Loaded += (origen, evento) => ActualizarVista();
        }

        private bool FiltrarMonitor(object o)
        {
            Monitor m = o as Monitor;
            if (m == null) return false;
            if (!Texto.Contiene(m.NombreCompleto + " " + m.Especialidad, txtBuscarMonitor.Text)) return false;
            ComboBoxItem idioma = cmbFiltroIdioma.SelectedItem as ComboBoxItem;
            if (cmbFiltroIdioma.SelectedIndex > 0 && idioma != null && !m.Idiomas.Contains(idioma.Content.ToString())) return false;
            ComboBoxItem disp = cmbFiltroDisponibilidad.SelectedItem as ComboBoxItem;
            if (cmbFiltroDisponibilidad.SelectedIndex > 0 && disp != null && m.Disponibilidad != disp.Content.ToString()) return false;
            return true;
        }

        private void Filtro_Cambiado(object sender, RoutedEventArgs e)
        {
            if (!cargado) return;
            vista.Refresh();
        }

        private void btnLimpiarFiltros_Click(object sender, RoutedEventArgs e)
        {
            txtBuscarMonitor.Clear();
            cmbFiltroIdioma.SelectedIndex = 0;
            cmbFiltroDisponibilidad.SelectedIndex = 0;
        }

        private Monitor MonitorSeleccionado { get { return dgMonitores.SelectedItem as Monitor; } }

        private static string Fecha(System.DateTime f) { return f.ToString("dd/MM/yyyy"); }

        private void dgMonitores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Monitor m = MonitorSeleccionado;
            bool hay = m != null;
            btnModificarMonitor.IsEnabled = hay;
            btnDarDeBaja.IsEnabled = hay && m.Activo;
            pnlFichaMonitor.Visibility = hay ? Visibility.Visible : Visibility.Collapsed;
            lblFichaVaciaMonitor.Visibility = hay ? Visibility.Collapsed : Visibility.Visible;
            if (!hay) return;

            lblInicialesMonitor.Text = (m.Nombre.Substring(0, 1) + m.Apellidos.Substring(0, 1)).ToUpper();
            lblFichaNombreMonitor.Text = m.NombreCompleto;
            lblFichaDatosMonitor.Text =
                "Especialidad: " + m.Especialidad + "\n" +
                "Estado: " + m.Disponibilidad + "\n" +
                "Teléfono: " + m.Telefono + (string.IsNullOrWhiteSpace(m.Telefono2) ? "" : " / " + m.Telefono2) + "\n" +
                "Correo: " + (string.IsNullOrWhiteSpace(m.Correo) ? "—" : m.Correo) + "\n" +
                "Idiomas: " + m.TextoIdiomas + "\n" +
                "Formación: " + m.Formacion + "\n" +
                "Contrato: " + Fecha(m.InicioContrato) + " – " + Fecha(m.FinContrato) +
                (string.IsNullOrWhiteSpace(m.Restricciones) ? "" : "\nRestricciones: " + m.Restricciones);

            lstActividadesMonitor.Items.Clear();
            foreach (Actividad a in DatosPrueba.Actividades)
                if (a.Monitores.Contains(m)) lstActividadesMonitor.Items.Add(a.Nombre + " (" + a.TextoDias + ", " + a.Horario + ")");
            if (lstActividadesMonitor.Items.Count == 0) lstActividadesMonitor.Items.Add("Sin actividades asignadas");
            btnVerActividades.IsEnabled = lstActividadesMonitor.Items.Count > 0 && !lstActividadesMonitor.Items[0].ToString().StartsWith("Sin ");
        }

        private void btnVerActividades_Click(object sender, RoutedEventArgs e)
        {
            VentanaPrincipal.Actual.IrAPestana("Actividades");
        }

        private void btnNuevoMonitor_Click(object sender, RoutedEventArgs e) { Nuevo(); }
        private void btnModificarMonitor_Click(object sender, RoutedEventArgs e) { Modificar(); }
        private void btnDarDeBaja_Click(object sender, RoutedEventArgs e) { Eliminar(); }

        private void dgMonitores_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject origen = e.OriginalSource as DependencyObject;
            while (origen != null && !(origen is DataGridRow) && !(origen is DataGridColumnHeader))
                origen = VisualTreeHelper.GetParent(origen);
            if (origen is DataGridRow) Modificar();
        }

        private void dgMonitores_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete) { e.Handled = true; Eliminar(); }
            else if (e.Key == Key.Enter) { e.Handled = true; Modificar(); }
        }

        public void Nuevo() => AbrirFormulario(null);

        public void Modificar()
        {
            if (MonitorSeleccionado != null && btnModificarMonitor.IsEnabled)
                AbrirFormulario(MonitorSeleccionado);
        }

        private void AbrirFormulario(Monitor original)
        {
            var dialogo = new DialogoMonitor(original) { Owner = Window.GetWindow(this) };
            if (dialogo.ShowDialog() != true) return;

            var monitor = dialogo.Monitor;
            if (original == null)
            {
                DatosPrueba.Monitores.Add(monitor);
                if (!vista.Contains(monitor)) btnLimpiarFiltros_Click(this, null);
                dgMonitores.SelectedItem = monitor;
                dgMonitores.ScrollIntoView(monitor);
            }
            ActualizarVista();
            VentanaPrincipal.MostrarEstado($"Monitor {monitor.NombreCompleto} guardado");
        }

        internal void ActualizarVista()
        {
            dgMonitores.Items.Refresh();
            dgMonitores_SelectionChanged(dgMonitores, null);
        }

        public void Eliminar()
        {
            var monitor = MonitorSeleccionado;
            if (monitor == null || !monitor.Activo) return;
            string mensaje = "El monitor dejará de estar disponible para nuevas asignaciones. Se conservarán sus actividades.";
            if (!VentanaConfirmacion.Confirmar(Window.GetWindow(this), $"¿Dar de baja {monitor.NombreCompleto}?", mensaje, "Dar de baja")) return;

            monitor.Activo = false;
            ActualizarVista();
            VentanaPrincipal.MostrarEstado($"Monitor {monitor.NombreCompleto} dado de baja");
        }

        public void EnfocarBusqueda() { txtBuscarMonitor.Focus(); txtBuscarMonitor.SelectAll(); }
    }
}
