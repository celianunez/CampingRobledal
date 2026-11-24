using System.Windows;
using System.Windows.Input;

namespace CampingRobledal
{
    public partial class DialogoAcercaDe : Window
    {
        public DialogoAcercaDe()
        {
            InitializeComponent();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e) { Close(); }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                Close();
            }
        }
    }
}
