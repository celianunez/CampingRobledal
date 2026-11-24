using System.ComponentModel;
using System.Linq;
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
    public partial class PanelActividades : UserControl
    {
        private readonly ICollectionView vista;
        private readonly bool cargado = false;

        public PanelActividades()
        {
            InitializeComponent();

            cmbFiltroArea.Items.Add("Todas");
            foreach (string area in DatosPrueba.Actividades.Select(a => a.Area).Distinct().OrderBy(a => a))
                cmbFiltroArea.Items.Add(area);
            cmbFiltroArea.SelectedIndex = 0;

            cmbFiltroMonitor.Items.Add("Todos");
            foreach (Monitor m in DatosPrueba.Monitores) cmbFiltroMonitor.Items.Add(m.NombreCompleto);
            cmbFiltroMonitor.SelectedIndex = 0;

            vista = CollectionViewSource.GetDefaultView(DatosPrueba.Actividades);
            vista.Filter = FiltrarActividad;
            dgActividades.ItemsSource = vista;
            cargado = true;
        }

        private bool FiltrarActividad(object o)
        {
            Actividad a = o as Actividad;
            if (a == null) return false;
            if (!Texto.Contiene(a.Nombre, txtBuscarActividad.Text)) return false;
            ComboBoxItem dest = cmbFiltroDestinatarios.SelectedItem as ComboBoxItem;
            if (cmbFiltroDestinatarios.SelectedIndex > 0 && dest != null && a.TextoDestinatarios != dest.Content.ToString()
                && a.Destinatarios != Destinatarios.Todos) return false;
            string area = cmbFiltroArea.SelectedItem as string;
            if (cmbFiltroArea.SelectedIndex > 0 && area != null && a.Area != area) return false;
            string mon = cmbFiltroMonitor.SelectedItem as string;
            if (cmbFiltroMonitor.SelectedIndex > 0 && mon != null && !a.Monitores.Any(m => m.NombreCompleto == mon)) return false;
            return true;
        }

        private void Filtro_Cambiado(object sender, RoutedEventArgs e)
        {
            if (!cargado) return;
            vista.Refresh();
        }

        private void btnLimpiarFiltros_Click(object sender, RoutedEventArgs e)
        {
            txtBuscarActividad.Clear();
            cmbFiltroDestinatarios.SelectedIndex = 0;
            cmbFiltroArea.SelectedIndex = 0;
            cmbFiltroMonitor.SelectedIndex = 0;
        }

        private Actividad ActividadSeleccionada { get { return dgActividades.SelectedItem as Actividad; } }

        private void dgActividades_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Actividad a = ActividadSeleccionada;
            bool hay = a != null;
            btnModificarActividad.IsEnabled = hay;
            btnEliminarActividad.IsEnabled = hay;
            pnlFichaActividad.Visibility = hay ? Visibility.Visible : Visibility.Collapsed;
            lblFichaVaciaActividad.Visibility = hay ? Visibility.Collapsed : Visibility.Visible;
            if (!hay) return;

            lblFichaNombreActividad.Text = a.Nombre;
            lblFichaDatosActividad.Text =
                "Destinatarios: " + a.TextoDestinatarios + " (desde " + a.EdadMinima + " años)\n" +
                "Área: " + a.Area + "\n" +
                "Días: " + a.TextoDias + "\n" +
                "Horario: " + a.Horario + "\n" +
                "Monitor(es): " + a.TextoMonitores + "\n" +
                "Precio: " + a.PrecioHora.ToString("0.00") + " €/hora" +
                (a.PrecioMes.HasValue ? " · " + a.PrecioMes.Value.ToString("0.00") + " €/mes" : "") + "\n\n" +
                a.Descripcion + "\nMaterial: " + a.Materiales;

            prgCupo.Maximum = a.CupoMaximo;
            prgCupo.Value = a.Inscritos;
            // Color + texto: verde con plazas, rojo si completa, ámbar si no llega al mínimo
            string color = a.Inscritos >= a.CupoMaximo ? "#C62828" : (a.Inscritos < a.CupoMinimo ? "#F9A825" : "#2E7D32");
            prgCupo.Foreground = (Brush)new BrushConverter().ConvertFromString(color);
            lblCupo.Text = a.TextoCupo + " inscritos (mín. " + a.CupoMinimo + ", máx. " + a.CupoMaximo + ") · "
                + (a.Inscritos >= a.CupoMaximo ? "Completa" : (a.Inscritos < a.CupoMinimo ? "Faltan inscritos para el mínimo" : "Plazas libres"));
            btnVerMonitor.IsEnabled = a.Monitores.Count > 0;
        }

        private void btnVerMonitor_Click(object sender, RoutedEventArgs e)
        {
            VentanaPrincipal.Actual.IrAPestana("Monitores");
        }

        private void btnNuevaActividad_Click(object sender, RoutedEventArgs e) { Nueva(); }
        private void btnModificarActividad_Click(object sender, RoutedEventArgs e) { Modificar(); }
        private void btnEliminarActividad_Click(object sender, RoutedEventArgs e) { Eliminar(); }

        private void dgActividades_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject origen = e.OriginalSource as DependencyObject;
            while (origen != null && !(origen is DataGridRow) && !(origen is DataGridColumnHeader))
                origen = VisualTreeHelper.GetParent(origen);
            if (origen is DataGridRow) Modificar();
        }

        private void dgActividades_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete) { e.Handled = true; Eliminar(); }
            else if (e.Key == Key.Enter) { e.Handled = true; Modificar(); }
        }

        public void Nueva() => AbrirFormulario(null);

        public void Modificar()
        {
            if (ActividadSeleccionada != null && btnModificarActividad.IsEnabled)
                AbrirFormulario(ActividadSeleccionada);
        }

        private void AbrirFormulario(Actividad original)
        {
            var dialogo = new DialogoActividad(original) { Owner = Window.GetWindow(this) };
            if (dialogo.ShowDialog() != true) return;

            var actividad = dialogo.Actividad;
            if (original == null)
            {
                DatosPrueba.Actividades.Add(actividad);
                if (!vista.Contains(actividad)) btnLimpiarFiltros_Click(this, null);
                dgActividades.SelectedItem = actividad;
                dgActividades.ScrollIntoView(actividad);
            }
            ActualizarVista();
            VentanaPrincipal.MostrarEstado($"Actividad {actividad.Nombre} guardada");
        }

        internal void ActualizarVista()
        {
            dgActividades.Items.Refresh();
            dgActividades_SelectionChanged(dgActividades, null);
        }

        public void Eliminar()
        {
            var actividad = ActividadSeleccionada;
            if (actividad == null) return;
            string mensaje = $"Se eliminará la actividad y se perderán sus {actividad.Inscritos} inscripciones.";
            if (!VentanaConfirmacion.Confirmar(Window.GetWindow(this), $"¿Eliminar {actividad.Nombre}?", mensaje, "Eliminar")) return;

            DatosPrueba.Actividades.Remove(actividad);
            ActualizarVista();
            VentanaPrincipal.MostrarEstado($"Actividad {actividad.Nombre} eliminada");
        }

        public void EnfocarBusqueda() { txtBuscarActividad.Focus(); txtBuscarActividad.SelectAll(); }
    }
}
