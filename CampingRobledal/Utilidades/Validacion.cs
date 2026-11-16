using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CampingRobledal.Utilidades
{
    /// <summary>Comprobaciones reutilizables (patrón de ComprobarEntrada de S3, D-007).</summary>
    public static class Validacion
    {
        /// <summary>Marca el campo en rojo y muestra el mensaje a su lado, o lo limpia si es válido.</summary>
        public static bool MarcarCampo(Control campo, TextBlock lblError, bool valido, string mensaje)
        {
            campo.BorderBrush = valido
                ? (Brush)Application.Current.Resources["BordeNormal"]
                : (Brush)Application.Current.Resources["ColorPeligro"];
            campo.BorderThickness = new Thickness(valido ? 1 : 2);
            lblError.Text = valido ? "" : "✖ " + mensaje;
            lblError.Visibility = valido ? Visibility.Collapsed : Visibility.Visible;
            return valido;
        }

        public static bool EsTelefono(string texto)
        {
            return texto != null && Regex.IsMatch(texto.Trim(), @"^\d{9}$");
        }

        public static bool EsEmail(string texto)
        {
            return texto != null && Regex.IsMatch(texto.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
