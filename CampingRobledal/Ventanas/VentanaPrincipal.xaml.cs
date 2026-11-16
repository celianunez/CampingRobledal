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
            if (Actual != null) Actual.lblEstado.Text = mensaje;
        }

        private void ActualizarResumen()
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
        }

        private void btnAyuda_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("La ayuda completa estará disponible más adelante.",
                "Ayuda", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnPreferencias_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Las preferencias estarán disponibles más adelante.",
                "Preferencias", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void mnuAcercaDe_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Camping El Robledal · Gestión\nAdam Gouram El Bouhali y Celia Núñez Acosta\nIPO1 · curso 2026/27",
                "Acerca de", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (salidaConfirmada) return;
            // Confirmación provisional; la ventana propia (14-15) llega con feature/ayuda-confirmaciones
            MessageBoxResult r = MessageBox.Show("¿Quieres salir de la aplicación?", "Salir",
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (r == MessageBoxResult.Yes) salidaConfirmada = true;
            else e.Cancel = true;
        }
    }
}
