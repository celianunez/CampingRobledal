using System.Windows;

namespace CampingRobledal.Monitores
{
    /// <summary>
    /// Diálogo de confirmación reutilizable (boceto 14). No usa MessageBox porque
    /// este no permite poner a los botones el verbo de la acción.
    /// El botón por defecto es Cancelar: la opción segura.
    /// </summary>
    public partial class ConfirmDialog : Window
    {
        public ConfirmDialog(string titulo, string pregunta, string explicacion,
                             string textoAccion)
        {
            InitializeComponent();

            Title = titulo;
            lblPregunta.Text = pregunta;
            lblExplicacion.Text = explicacion;
            btnAccion.Content = textoAccion;
            btnAccion.ToolTip = "Confirma «" + textoAccion + "». Esta acción no se puede deshacer";

            Loaded += ConfirmDialog_Loaded;
        }

        private void ConfirmDialog_Loaded(object sender, RoutedEventArgs e)
        {
            // La opción segura recibe el foco y también es la que responde a Intro.
            btnCancelar.Focus();
        }

        private void btnAccion_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
