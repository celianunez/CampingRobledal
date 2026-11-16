using System;
using System.Globalization;
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

        /// <summary>Si mostrar es true marca/limpia el campo junto a su error; si no, solo devuelve si es válido.</summary>
        public static bool Comprobar(bool mostrar, Control campo, TextBlock lblError, bool valido, string mensaje)
        {
            if (mostrar) MarcarCampo(campo, lblError, valido, mensaje);
            return valido;
        }

        /// <summary>Número entero positivo (o cero si permiteCero).</summary>
        public static bool TryEntero(string texto, out int valor, bool permiteCero)
        {
            valor = 0;
            if (string.IsNullOrWhiteSpace(texto)) return false;
            if (!int.TryParse(texto.Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out valor)) return false;
            return permiteCero ? valor >= 0 : valor > 0;
        }

        /// <summary>Importe con coma o punto decimal, p. ej. 18,50 o 18.50.</summary>
        public static bool TryDecimal(string texto, out decimal valor)
        {
            valor = 0;
            if (string.IsNullOrWhiteSpace(texto)) return false;
            string limpio = texto.Trim().Replace(',', '.');
            if (!decimal.TryParse(limpio, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out valor)) return false;
            return valor > 0;
        }

        /// <summary>Hora en formato HH:mm, p. ej. 09:30.</summary>
        public static bool EsHora(string texto)
        {
            DateTime d;
            return texto != null && Regex.IsMatch(texto.Trim(), @"^([01]?\d|2[0-3]):[0-5]\d$")
                   && DateTime.TryParseExact(texto.Trim(), new[] { "H:mm", "HH:mm" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
        }

        /// <summary>Llama a alCambiar cuando el usuario toca cualquier control de entrada bajo raiz.</summary>
        public static void VigilarCambios(DependencyObject raiz, Action alCambiar)
        {
            foreach (object hijo in LogicalTreeHelper.GetChildren(raiz))
            {
                DependencyObject d = hijo as DependencyObject;
                if (d == null) continue;
                if (d is TextBox) ((TextBox)d).TextChanged += (o, e) => alCambiar();
                else if (d is ComboBox) ((ComboBox)d).SelectionChanged += (o, e) => { if (e.OriginalSource == o) alCambiar(); };
                else if (d is ListBox) ((ListBox)d).SelectionChanged += (o, e) => { if (e.OriginalSource == o) alCambiar(); };
                else if (d is System.Windows.Controls.Primitives.ToggleButton)
                    ((System.Windows.Controls.Primitives.ToggleButton)d).Click += (o, e) => alCambiar();
                else if (d is DatePicker) ((DatePicker)d).SelectedDateChanged += (o, e) => alCambiar();
                VigilarCambios(d, alCambiar);
            }
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
