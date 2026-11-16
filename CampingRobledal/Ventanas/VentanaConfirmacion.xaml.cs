using System.Windows;

namespace CampingRobledal
{
    public enum ResultadoConfirmacion { Cancelar, Accion, Descartar }

    /// <summary>Confirmación propia (pantallas 14 y 15): botones con verbo y Cancelar por defecto (D-006).</summary>
    public partial class VentanaConfirmacion : Window
    {
        public ResultadoConfirmacion Resultado { get; private set; } = ResultadoConfirmacion.Cancelar;

        public VentanaConfirmacion(string titulo, string mensaje, string verbo, bool conDescartar)
        {
            InitializeComponent();
            Title = titulo;
            lblTitulo.Text = titulo;
            lblMensaje.Text = mensaje;
            btnAccion.Content = verbo;
            btnAccion.ToolTip = verbo;
            btnDescartar.Visibility = conDescartar ? Visibility.Visible : Visibility.Collapsed;
            if (conDescartar)
                btnAccion.Style = (Style)FindResource("BotonPrincipal");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnCancelar.Focus();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e) { Resultado = ResultadoConfirmacion.Cancelar; Close(); }
        private void btnAccion_Click(object sender, RoutedEventArgs e) { Resultado = ResultadoConfirmacion.Accion; Close(); }
        private void btnDescartar_Click(object sender, RoutedEventArgs e) { Resultado = ResultadoConfirmacion.Descartar; Close(); }

        /// <summary>Confirmación de una acción destructiva. Devuelve true solo si el usuario pulsa el verbo.</summary>
        public static bool Confirmar(Window propietario, string titulo, string mensaje, string verbo)
        {
            VentanaConfirmacion v = new VentanaConfirmacion(titulo, mensaje, verbo, false) { Owner = propietario };
            v.ShowDialog();
            return v.Resultado == ResultadoConfirmacion.Accion;
        }

        /// <summary>Salida con cambios sin guardar: Guardar y salir / Salir sin guardar / Cancelar.</summary>
        public static ResultadoConfirmacion PreguntarSalida(Window propietario, string mensaje)
        {
            VentanaConfirmacion v = new VentanaConfirmacion("¿Salir sin guardar los cambios?", mensaje, "Guardar y salir", true)
            { Owner = propietario };
            v.ShowDialog();
            return v.Resultado;
        }
    }
}
