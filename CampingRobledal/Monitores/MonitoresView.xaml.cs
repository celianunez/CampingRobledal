using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CampingRobledal.Datos;

namespace CampingRobledal.Monitores
{
    /// <summary>
    /// Pestaña Monitores: tabla con búsqueda y filtros, ficha del seleccionado y
    /// acciones de alta, modificación y baja. Es un UserControl para poder
    /// incrustarlo tal cual en la ventana principal.
    /// </summary>
    public partial class MonitoresView : UserControl
    {
        private readonly ObservableCollection<Monitor> _monitores;
        private readonly ICollectionView _vista;
        private bool _cargando = true;

        /// <summary>Avisa a la ventana principal del texto que debe mostrar en su barra de estado.</summary>
        public event EventHandler<string> EstadoCambiado;

        /// <summary>La ventana principal puede suscribirse para abrir la pestaña Actividades.</summary>
        public event EventHandler<Monitor> VerActividadesSolicitado;

        public ObservableCollection<Monitor> Monitores
        {
            get { return _monitores; }
        }

        public MonitoresView()
        {
            InitializeComponent();

            _monitores = DatosPrueba.Monitores;
            _vista = CollectionViewSource.GetDefaultView(_monitores);
            _vista.Filter = FiltrarMonitor;

            dgMonitores.ItemsSource = _vista;

            CargarFiltros();
            ActualizarResumen();
            ActualizarRecuento();

            // La ficha del boceto 10 está abierta con Laura: dejamos seleccionada la segunda fila.
            if (dgMonitores.Items.Count > 1)
                dgMonitores.SelectedIndex = 1;
            else if (dgMonitores.Items.Count == 1)
                dgMonitores.SelectedIndex = 0;

            _cargando = false;
            ActualizarFicha();
        }

        // ===================== Filtros =====================

        private void CargarFiltros()
        {
            var idiomas = new List<string> { "Todos" };
            idiomas.AddRange(DatosPrueba.IdiomasDisponibles);

            foreach (var idioma in _monitores.SelectMany(m => m.Idiomas ?? new List<string>()))
            {
                if (!idiomas.Any(i => string.Equals(i, idioma, StringComparison.OrdinalIgnoreCase)))
                    idiomas.Add(idioma);
            }

            cmbFiltroIdioma.ItemsSource = idiomas;
            cmbFiltroIdioma.SelectedIndex = 0;

            var disponibilidades = new List<string> { "Toda" };
            disponibilidades.AddRange(_monitores
                .Select(m => m.Disponibilidad)
                .Where(d => !string.IsNullOrWhiteSpace(d))
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .OrderBy(d => d));

            cmbFiltroDisponibilidad.ItemsSource = disponibilidades;
            cmbFiltroDisponibilidad.SelectedIndex = 0;
        }

        private bool FiltrarMonitor(object elemento)
        {
            var monitor = elemento as Monitor;
            if (monitor == null) return false;

            string busqueda = Normalizar(txtBuscarMonitor.Text);
            if (busqueda.Length > 0 && !Normalizar(monitor.NombreCompleto).Contains(busqueda))
                return false;

            string idioma = cmbFiltroIdioma.SelectedItem as string;
            if (!string.IsNullOrEmpty(idioma) && idioma != "Todos")
            {
                bool hablaIdioma = (monitor.Idiomas ?? new List<string>())
                    .Any(i => string.Equals(i, idioma, StringComparison.OrdinalIgnoreCase));

                if (!hablaIdioma) return false;
            }

            string disponibilidad = cmbFiltroDisponibilidad.SelectedItem as string;
            if (!string.IsNullOrEmpty(disponibilidad) && disponibilidad != "Toda")
            {
                if (!string.Equals(monitor.Disponibilidad, disponibilidad,
                                   StringComparison.CurrentCultureIgnoreCase))
                    return false;
            }

            return true;
        }

        private void AplicarFiltros()
        {
            if (_vista == null) return;

            _vista.Refresh();
            ActualizarRecuento();

            int visibles = _vista.Cast<object>().Count();
            pnlSinResultados.Visibility = visibles == 0 ? Visibility.Visible : Visibility.Collapsed;

            if (dgMonitores.SelectedItem is Monitor seleccionado && !_vista.Contains(seleccionado))
                dgMonitores.SelectedItem = null;
        }

        private static string Normalizar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return string.Empty;

            string descompuesto = texto.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var resultado = new StringBuilder();

            foreach (char c in descompuesto)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    resultado.Append(c);
            }

            return resultado.ToString().Normalize(NormalizationForm.FormC);
        }

        private void txtBuscarMonitor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lblPlaceholderBuscar != null)
            {
                lblPlaceholderBuscar.Visibility = string.IsNullOrEmpty(txtBuscarMonitor.Text)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }

            if (_cargando) return;
            AplicarFiltros();
        }

        private void cmbFiltroIdioma_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_cargando) return;
            AplicarFiltros();
        }

        private void cmbFiltroDisponibilidad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_cargando) return;
            AplicarFiltros();
        }

        // ===================== Ficha y selección =====================

        private void dgMonitores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActualizarFicha();
        }

        private void ActualizarFicha()
        {
            var monitor = dgMonitores.SelectedItem as Monitor;

            pnlFicha.Visibility = monitor == null ? Visibility.Collapsed : Visibility.Visible;
            lblSinSeleccion.Visibility = monitor == null ? Visibility.Visible : Visibility.Collapsed;

            btnModificar.IsEnabled = monitor != null;
            btnDarDeBaja.IsEnabled = monitor != null;
            btnVerActividades.IsEnabled = monitor != null;
        }

        private void ActualizarRecuento()
        {
            if (lblRecuento == null || _vista == null) return;

            int visibles = _vista.Cast<object>().Count();
            bool hayFiltros = !string.IsNullOrWhiteSpace(txtBuscarMonitor.Text)
                              || (cmbFiltroIdioma.SelectedIndex > 0)
                              || (cmbFiltroDisponibilidad.SelectedIndex > 0);

            if (hayFiltros)
                lblRecuento.Text = "Mostrando " + visibles + " de " + _monitores.Count;
            else
                lblRecuento.Text = visibles == 1 ? "1 monitor" : visibles + " monitores";
        }

        private void ActualizarResumen()
        {
            lblResumen.Text = _monitores.Count == 1
                ? "1 monitor contratado esta temporada"
                : _monitores.Count + " monitores contratados esta temporada";
        }

        private void MostrarEstado(string texto)
        {
            lblEstado.Text = texto;
            EstadoCambiado?.Invoke(this, texto);
        }

        // ===================== Acciones =====================

        private void btnNuevoMonitor_Click(object sender, RoutedEventArgs e)
        {
            NuevoMonitor();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            ModificarMonitor();
        }

        private void btnDarDeBaja_Click(object sender, RoutedEventArgs e)
        {
            DarDeBaja();
        }

        private void NuevoMonitor()
        {
            var dialogo = new MonitorDialog
            {
                Owner = Window.GetWindow(this)
            };

            if (dialogo.ShowDialog() != true || dialogo.Resultado == null) return;

            _monitores.Add(dialogo.Resultado);
            ActualizarResumen();
            AplicarFiltros();

            if (_vista.Contains(dialogo.Resultado))
            {
                dgMonitores.SelectedItem = dialogo.Resultado;
                dgMonitores.ScrollIntoView(dialogo.Resultado);
            }

            ActualizarFicha();
            MostrarEstado("Monitor guardado correctamente");
        }

        private void ModificarMonitor()
        {
            var monitor = dgMonitores.SelectedItem as Monitor;
            if (monitor == null) return;

            var dialogo = new MonitorDialog(monitor)
            {
                Owner = Window.GetWindow(this)
            };

            if (dialogo.ShowDialog() != true || dialogo.Resultado == null) return;

            // El diálogo ha trabajado sobre una copia: solo ahora se toca el monitor original.
            monitor.CopiarDesde(dialogo.Resultado);
            AplicarFiltros();

            if (_vista.Contains(monitor))
                dgMonitores.SelectedItem = monitor;

            ActualizarFicha();
            MostrarEstado("Monitor guardado correctamente");
        }

        private void DarDeBaja()
        {
            var monitor = dgMonitores.SelectedItem as Monitor;
            if (monitor == null) return;

            int actividades = monitor.Actividades == null ? 0 : monitor.Actividades.Count;
            string explicacion;

            if (actividades == 0)
            {
                explicacion = "No imparte ninguna actividad en este momento. " +
                              "Esta acción no se puede deshacer.";
            }
            else if (actividades == 1)
            {
                explicacion = "Imparte 1 actividad que se quedará sin monitor. " +
                              "Esta acción no se puede deshacer.";
            }
            else
            {
                explicacion = "Imparte " + actividades + " actividades que se quedarán sin monitor. " +
                              "Esta acción no se puede deshacer.";
            }

            var dialogo = new ConfirmDialog(
                "Confirmar baja",
                "¿Dar de baja a " + monitor.NombreCompleto + "?",
                explicacion,
                "Dar de baja")
            {
                Owner = Window.GetWindow(this)
            };

            if (dialogo.ShowDialog() != true) return;

            _monitores.Remove(monitor);
            ActualizarResumen();
            AplicarFiltros();
            ActualizarFicha();
            MostrarEstado(monitor.NombreCompleto + " dado/a de baja");
        }

        private void btnVerActividades_Click(object sender, RoutedEventArgs e)
        {
            var monitor = dgMonitores.SelectedItem as Monitor;
            if (monitor == null) return;

            var manejador = VerActividadesSolicitado;
            if (manejador != null)
            {
                manejador(this, monitor);
                return;
            }

            string lista = (monitor.Actividades == null || monitor.Actividades.Count == 0)
                ? "Este monitor no tiene actividades asignadas."
                : "• " + string.Join("\n• ", monitor.Actividades);

            MessageBox.Show(
                "Actividades de " + monitor.NombreCompleto + ":\n\n" + lista,
                "Actividades del monitor",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        // ===================== Ratón y teclado =====================

        private void dgMonitores_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource == null) return;

            var fila = ItemsControl.ContainerFromElement(dgMonitores, e.OriginalSource as DependencyObject)
                       as DataGridRow;

            if (fila != null && dgMonitores.SelectedItem is Monitor)
            {
                ModificarMonitor();
                e.Handled = true;
            }
        }

        private void MonitoresView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.N && Keyboard.Modifiers == ModifierKeys.Control)
            {
                NuevoMonitor();
                e.Handled = true;
            }
            else if (e.Key == Key.F && Keyboard.Modifiers == ModifierKeys.Control)
            {
                txtBuscarMonitor.Focus();
                txtBuscarMonitor.SelectAll();
                e.Handled = true;
            }
            else if (e.Key == Key.Delete && dgMonitores.IsKeyboardFocusWithin)
            {
                DarDeBaja();
                e.Handled = true;
            }
            else if (e.Key == Key.Enter && dgMonitores.IsKeyboardFocusWithin)
            {
                ModificarMonitor();
                e.Handled = true;
            }
        }
    }
}
