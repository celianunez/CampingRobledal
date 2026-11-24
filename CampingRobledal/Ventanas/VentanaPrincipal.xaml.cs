using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CampingRobledal.Datos;

namespace CampingRobledal
{
    public partial class VentanaPrincipal : Window
    {
        private readonly Gestor gestor;
        private bool salidaConfirmada = false;

        /// <summary>Ventana abierta; permite que paneles y diálogos escriban en la barra de estado.</summary>
        public static VentanaPrincipal Actual { get; private set; }

        public VentanaPrincipal(Gestor gestor)
        {
            InitializeComponent();
            this.gestor = gestor;
            Actual = this;

            lblIniciales.Text = gestor.Iniciales;
            lblNombreGestor.Text = gestor.NombreCompleto;
            lblCargoGestor.Text = gestor.Cargo;
            lblUsuarioAcceso.Text = "Usuario: " + gestor.Usuario + "  ·  Último acceso: "
                                    + gestor.UltimoAcceso.ToString("dd/MM/yyyy HH:mm");
            MostrarEstado("Bienvenido, " + gestor.Nombre);
            ActualizarResumen();
        }

        /// <summary>Muestra en la barra de estado el resultado de la última acción (RF-08).</summary>
        public static void MostrarEstado(string mensaje)
        {
            if (Actual == null) return;
            Actual.lblEstado.Text = mensaje;
            Actual.ActualizarResumen();
        }

        public void ActualizarResumen()
        {
            if (lblResumen == null) return;
            TabItem tab = tabPrincipal.SelectedItem as TabItem;
            if (tab == tbiAlojamientos)
            {
                if (tabAlojamientos.SelectedItem == tbiBungalows)
                    lblResumen.Text = DatosPrueba.Bungalows.Count + " bungalows, "
                        + DatosPrueba.Bungalows.Count(b => b.Estado == EstadoAlojamiento.Libre) + " libres";
                else
                    lblResumen.Text = DatosPrueba.Parcelas.Count + " parcelas, "
                        + DatosPrueba.Parcelas.Count(p => p.Estado == EstadoAlojamiento.Libre) + " libres";
            }
            else if (tab == tbiReservas)
                lblResumen.Text = DatosPrueba.Reservas.Count + " reservas, "
                    + DatosPrueba.Reservas.Count(r => r.Estado == EstadoReserva.Confirmada) + " confirmadas";
            else if (tab == tbiMonitores)
                lblResumen.Text = DatosPrueba.Monitores.Count(m => m.Activo) + " monitores activos de "
                    + DatosPrueba.Monitores.Count;
            else if (tab == tbiActividades)
                lblResumen.Text = DatosPrueba.Actividades.Count + " actividades programadas";
        }

        private void tabPrincipal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Los TabControl anidados también lanzan este evento hacia arriba: solo reaccionamos al propio
            if (e.OriginalSource != sender) return;
            ActualizarResumen();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                e.Handled = true;
                btnAyuda_Click(sender, e);
            }
            else if (e.Key == Key.N && Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = true;
                NuevoEnPestanaActual();
            }
            else if (e.Key == Key.F && Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = true;
                BuscarEnPestanaActual();
            }
        }

        /// <summary>Ctrl+N: alta en la pestaña visible.</summary>
        private void NuevoEnPestanaActual()
        {
            TabItem tab = tabPrincipal.SelectedItem as TabItem;
            if (tab == tbiAlojamientos)
            {
                if (tabAlojamientos.SelectedItem == tbiBungalows) pnlBungalows.Nuevo(); else pnlParcelas.Nueva();
            }
            else if (tab == tbiReservas) pnlReservas.Nueva();
            else if (tab == tbiMonitores) pnlMonitores.Nuevo();
            else if (tab == tbiActividades) pnlActividades.Nueva();
        }

        /// <summary>Ctrl+F: foco en el cuadro de búsqueda de la pestaña visible.</summary>
        private void BuscarEnPestanaActual()
        {
            TabItem tab = tabPrincipal.SelectedItem as TabItem;
            if (tab == tbiAlojamientos)
            {
                if (tabAlojamientos.SelectedItem == tbiBungalows) pnlBungalows.EnfocarBusqueda(); else pnlParcelas.EnfocarBusqueda();
            }
            else if (tab == tbiReservas) pnlReservas.EnfocarBusqueda();
            else if (tab == tbiMonitores) pnlMonitores.EnfocarBusqueda();
            else if (tab == tbiActividades) pnlActividades.EnfocarBusqueda();
        }

        /// <summary>Navegación entre pestañas desde fichas y botones «Ver…».</summary>
        public void IrAPestana(string nombre)
        {
            switch (nombre)
            {
                case "Alojamientos": tabPrincipal.SelectedItem = tbiAlojamientos; break;
                case "Reservas": tabPrincipal.SelectedItem = tbiReservas; break;
                case "Monitores": tabPrincipal.SelectedItem = tbiMonitores; break;
                case "Actividades": tabPrincipal.SelectedItem = tbiActividades; break;
            }
        }

        private void btnAyuda_Click(object sender, RoutedEventArgs e)
        {
            // Abre el tema de la pestaña que se está viendo (F1 contextual)
            string tema = "Alojamientos";
            TabItem tab = tabPrincipal.SelectedItem as TabItem;
            if (tab == tbiReservas) tema = "Reservas";
            else if (tab == tbiMonitores) tema = "Monitores";
            else if (tab == tbiActividades) tema = "Actividades";
            VentanaAyuda.Abrir(tema);
        }

        private void btnPreferencias_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Las preferencias estarán disponibles más adelante.",
                "Preferencias", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void mnuAcercaDe_Click(object sender, RoutedEventArgs e)
        {
            new DialogoAcercaDe { Owner = this }.ShowDialog();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (salidaConfirmada) return;
            salidaConfirmada = VentanaConfirmacion.Confirmar(this, "¿Salir de Camping El Robledal?",
                $"Se cerrará la sesión de {gestor.NombreCompleto} y se perderán los datos de esta sesión.", "Salir");
            e.Cancel = !salidaConfirmada;
            if (salidaConfirmada) VentanaAyuda.CerrarSiAbierta();
        }
    }
}
