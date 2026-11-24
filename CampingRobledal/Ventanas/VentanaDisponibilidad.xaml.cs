using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CampingRobledal.Datos;

namespace CampingRobledal
{
    /// <summary>Calendario alojamiento × día (pantalla 07, RF-18 a RF-20).</summary>
    public partial class VentanaDisponibilidad : Window
    {
        private const int MaxDias = 42;

        private class Celda
        {
            public Alojamiento Alojamiento;
            public DateTime Dia;
        }

        private readonly bool soloElegir;
        private readonly Alojamiento alojamientoInicial;
        private bool cargando = true;

        // Selección actual: un alojamiento y un tramo de días libres consecutivos
        private Alojamiento selAlojamiento;
        private DateTime? selInicio;
        private DateTime? selFin;

        /// <summary>Resultado en modo «solo elegir» (cuando se abre desde el formulario de reserva).</summary>
        public Alojamiento AlojamientoElegido { get; private set; }
        public DateTime Entrada { get; private set; }
        public DateTime Salida { get; private set; }

        public VentanaDisponibilidad() : this(null, null, false) { }

        /// <param name="alojamiento">Alojamiento a destacar (puede ser null).</param>
        /// <param name="desde">Primer día a mostrar (null = primera reserva o hoy).</param>
        /// <param name="soloElegir">true: el botón devuelve alojamiento y fechas en vez de crear la reserva.</param>
        public VentanaDisponibilidad(Alojamiento alojamiento, DateTime? desde, bool soloElegir)
        {
            InitializeComponent();
            this.soloElegir = soloElegir;
            alojamientoInicial = alojamiento;

            if (soloElegir) btnReservar.Content = "Elegir estas fechas";
            if (alojamiento is Parcela) cmbTipoAlojamiento.SelectedIndex = 1;
            else if (alojamiento is Bungalow) cmbTipoAlojamiento.SelectedIndex = 2;

            DateTime inicio;
            if (desde.HasValue) inicio = desde.Value.Date;
            else if (DatosPrueba.Reservas.Count > 0) inicio = DatosPrueba.Reservas.Min(r => r.Entrada).Date;
            else inicio = DateTime.Today;
            dpDesde.SelectedDate = inicio;
            dpHasta.SelectedDate = inicio.AddDays(13);

            cargando = false;
            Dibujar();
        }

        // ---------- Estado de cada celda ----------

        private static char EstadoDia(Alojamiento a, DateTime dia)
        {
            if (a.Estado == EstadoAlojamiento.EnReparacion) return 'X';
            foreach (Reserva r in DatosPrueba.Reservas)
            {
                if (r.Alojamiento != a) continue;
                if (r.Estado == EstadoReserva.Anulada || r.Estado == EstadoReserva.Finalizada) continue;
                if (r.Entrada.Date <= dia && dia < r.Salida.Date)
                    return r.Estado == EstadoReserva.CheckIn ? 'O' : 'R';
            }
            if (a.Estado == EstadoAlojamiento.PendienteLimpieza && dia == DateTime.Today) return 'P';
            return 'L';
        }

        private static string NombreEstado(char letra)
        {
            switch (letra)
            {
                case 'L': return "Libre";
                case 'O': return "Ocupado";
                case 'R': return "Reservado";
                case 'X': return "En reparación";
                default: return "Pendiente de limpieza";
            }
        }

        private static Brush Fondo(char letra)
        {
            string color;
            switch (letra)
            {
                case 'L': color = "#2E7D32"; break;
                case 'O': color = "#C62828"; break;
                case 'R': color = "#1565C0"; break;
                case 'X': color = "#6D4C41"; break;
                default: color = "#F9A825"; break;
            }
            return (Brush)new BrushConverter().ConvertFromString(color);
        }

        // ---------- Filtros y periodo ----------

        private bool PasaFiltros(Alojamiento a)
        {
            if (cmbTipoAlojamiento.SelectedIndex == 1 && !(a is Parcela)) return false;
            if (cmbTipoAlojamiento.SelectedIndex == 2 && !(a is Bungalow)) return false;
            ComboBoxItem cat = cmbCategoria.SelectedItem as ComboBoxItem;
            if (cmbCategoria.SelectedIndex > 0 && cat != null)
            {
                string texto = cat.Content.ToString();
                Parcela p = a as Parcela;
                Bungalow b = a as Bungalow;
                if (p != null && p.TextoCategoria != texto) return false;
                if (b != null && b.TextoTipo != texto) return false;
            }
            return true;
        }

        private void LimpiarSeleccion()
        {
            selAlojamiento = null;
            selInicio = null;
            selFin = null;
        }

        private void Periodo_Cambiado(object sender, SelectionChangedEventArgs e)
        {
            if (cargando) return;
            LimpiarSeleccion();
            Dibujar();
        }

        private void Filtro_Cambiado(object sender, SelectionChangedEventArgs e)
        {
            if (cargando) return;
            LimpiarSeleccion();
            Dibujar();
        }

        private void DesplazarPeriodo(int signo)
        {
            if (!dpDesde.SelectedDate.HasValue || !dpHasta.SelectedDate.HasValue) return;
            int dias = (dpHasta.SelectedDate.Value.Date - dpDesde.SelectedDate.Value.Date).Days + 1;
            if (dias < 1 || dias > MaxDias) return;
            cargando = true;
            dpDesde.SelectedDate = dpDesde.SelectedDate.Value.AddDays(signo * dias);
            dpHasta.SelectedDate = dpHasta.SelectedDate.Value.AddDays(signo * dias);
            cargando = false;
            LimpiarSeleccion();
            Dibujar();
        }

        private void btnPeriodoAnterior_Click(object sender, RoutedEventArgs e) { DesplazarPeriodo(-1); }
        private void btnPeriodoSiguiente_Click(object sender, RoutedEventArgs e) { DesplazarPeriodo(1); }

        // ---------- Dibujo del calendario ----------

        private void Dibujar()
        {
            pnlCalendario.Children.Clear();
            pnlCalendario.ColumnDefinitions.Clear();
            pnlCalendario.RowDefinitions.Clear();

            // Validar periodo antes de construir nada
            if (!dpDesde.SelectedDate.HasValue || !dpHasta.SelectedDate.HasValue)
            {
                lblErrorPeriodo.Text = "✖ Elige las dos fechas del periodo, p. ej. del 01/07/2027 al 14/07/2027.";
                lblErrorPeriodo.Visibility = Visibility.Visible;
                ActualizarSeleccion();
                return;
            }
            DateTime desde = dpDesde.SelectedDate.Value.Date;
            DateTime hasta = dpHasta.SelectedDate.Value.Date;
            if (hasta < desde)
            {
                lblErrorPeriodo.Text = "✖ La fecha final debe ser igual o posterior a la inicial, p. ej. 14/07/2027.";
                lblErrorPeriodo.Visibility = Visibility.Visible;
                ActualizarSeleccion();
                return;
            }
            if ((hasta - desde).Days + 1 > MaxDias)
            {
                lblErrorPeriodo.Text = "✖ Elige como máximo 6 semanas, p. ej. 14 días.";
                lblErrorPeriodo.Visibility = Visibility.Visible;
                ActualizarSeleccion();
                return;
            }
            lblErrorPeriodo.Text = "";
            lblErrorPeriodo.Visibility = Visibility.Collapsed;

            List<DateTime> dias = new List<DateTime>();
            for (DateTime d = desde; d <= hasta; d = d.AddDays(1)) dias.Add(d);
            List<Alojamiento> filas = DatosPrueba.TodosLosAlojamientos().Where(PasaFiltros).ToList();

            // Columna 0 = nombre; resto = un día cada una
            pnlCalendario.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto, MinWidth = 120 });
            foreach (DateTime d in dias)
                pnlCalendario.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star), MinWidth = 34 });

            pnlCalendario.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            string iniciales = "DLMXJVS";
            for (int i = 0; i < dias.Count; i++)
            {
                DateTime d = dias[i];
                bool finde = d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday;
                TextBlock cab = new TextBlock
                {
                    Text = iniciales[(int)d.DayOfWeek] + "\n" + d.Day,
                    TextAlignment = TextAlignment.Center,
                    FontWeight = finde ? FontWeights.Bold : FontWeights.Normal,
                    Margin = new Thickness(0, 0, 0, 4),
                    ToolTip = d.ToString("dddd dd/MM/yyyy")
                };
                Grid.SetRow(cab, 0);
                Grid.SetColumn(cab, i + 1);
                pnlCalendario.Children.Add(cab);
            }

            if (filas.Count == 0)
            {
                TextBlock vacio = new TextBlock { Text = "No hay alojamientos con esos filtros.", Margin = new Thickness(8) };
                pnlCalendario.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                Grid.SetRow(vacio, 1);
                Grid.SetColumnSpan(vacio, dias.Count + 1);
                pnlCalendario.Children.Add(vacio);
            }

            for (int f = 0; f < filas.Count; f++)
            {
                Alojamiento a = filas[f];
                pnlCalendario.RowDefinitions.Add(new RowDefinition { Height = new GridLength(28) });

                TextBlock nombre = new TextBlock
                {
                    Text = a.Nombre,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 10, 0),
                    FontWeight = a == alojamientoInicial ? FontWeights.Bold : FontWeights.Normal,
                    ToolTip = a.Nombre + " · capacidad " + a.Capacidad
                };
                Grid.SetRow(nombre, f + 1);
                Grid.SetColumn(nombre, 0);
                pnlCalendario.Children.Add(nombre);

                for (int i = 0; i < dias.Count; i++)
                {
                    DateTime d = dias[i];
                    char letra = EstadoDia(a, d);
                    bool seleccionada = EstaSeleccionada(a, d);
                    Border celda = new Border
                    {
                        Background = Fondo(letra),
                        Margin = new Thickness(1),
                        BorderBrush = seleccionada ? Brushes.Black : Brushes.Transparent,
                        BorderThickness = new Thickness(seleccionada ? 3 : 0),
                        Cursor = letra == 'L' ? Cursors.Hand : Cursors.Arrow,
                        Tag = new Celda { Alojamiento = a, Dia = d },
                        ToolTip = a.Nombre + " · " + d.ToString("dd/MM/yyyy") + " · " + NombreEstado(letra)
                    };
                    celda.Child = new TextBlock
                    {
                        Text = letra.ToString(),
                        Foreground = letra == 'P' ? Brushes.Black : Brushes.White,
                        FontWeight = FontWeights.SemiBold,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    celda.MouseLeftButtonDown += Celda_Click;
                    Grid.SetRow(celda, f + 1);
                    Grid.SetColumn(celda, i + 1);
                    pnlCalendario.Children.Add(celda);
                }
            }
            ActualizarSeleccion();
        }

        // ---------- Selección de días ----------

        private bool EstaSeleccionada(Alojamiento a, DateTime dia)
        {
            if (selAlojamiento != a || !selInicio.HasValue) return false;
            DateTime fin = selFin ?? selInicio.Value;
            return dia >= selInicio.Value && dia <= fin;
        }

        private void Celda_Click(object sender, MouseButtonEventArgs e)
        {
            Border borde = sender as Border;
            Celda celda = borde == null ? null : borde.Tag as Celda;
            if (celda == null) return;

            if (EstadoDia(celda.Alojamiento, celda.Dia) != 'L')
            {
                lblSeleccion.Text = "Ese día no está libre (" + NombreEstado(EstadoDia(celda.Alojamiento, celda.Dia))
                    + "). Elige un día marcado con L.";
                return;
            }

            bool empezar = selAlojamiento != celda.Alojamiento || !selInicio.HasValue || selFin.HasValue
                           || celda.Dia < selInicio.Value;
            if (!empezar)
            {
                // Segundo clic: todos los días intermedios deben estar libres
                bool todosLibres = true;
                for (DateTime d = selInicio.Value; d <= celda.Dia; d = d.AddDays(1))
                    if (EstadoDia(celda.Alojamiento, d) != 'L') { todosLibres = false; break; }
                if (todosLibres) selFin = celda.Dia;
                else empezar = true;
            }
            if (empezar)
            {
                selAlojamiento = celda.Alojamiento;
                selInicio = celda.Dia;
                selFin = null;
            }
            Dibujar();
        }

        private void ActualizarSeleccion()
        {
            bool hay = selAlojamiento != null && selInicio.HasValue;
            btnReservar.IsEnabled = hay;
            if (!hay)
            {
                lblSeleccion.Text = "Haz clic en un día libre para empezar y en otro para terminar la estancia.";
                return;
            }
            DateTime entrada = selInicio.Value;
            DateTime salida = (selFin ?? selInicio.Value).AddDays(1);
            int noches = (salida - entrada).Days;
            lblSeleccion.Text = selAlojamiento.Nombre + " · entrada " + entrada.ToString("dd/MM/yyyy")
                + " · salida " + salida.ToString("dd/MM/yyyy") + " · " + noches + (noches == 1 ? " noche" : " noches");
        }

        // ---------- Botones ----------

        private void btnReservar_Click(object sender, RoutedEventArgs e)
        {
            if (selAlojamiento == null || !selInicio.HasValue) return;
            DateTime entrada = selInicio.Value;
            DateTime salida = (selFin ?? selInicio.Value).AddDays(1);

            if (soloElegir)
            {
                AlojamientoElegido = selAlojamiento;
                Entrada = entrada;
                Salida = salida;
                DialogResult = true;
                return;
            }

            DialogoReserva dlg = new DialogoReserva(selAlojamiento, entrada, salida) { Owner = this };
            if (dlg.ShowDialog() == true)
            {
                DatosPrueba.Reservas.Add(dlg.Reserva);
                VentanaPrincipal.MostrarEstado("Reserva " + dlg.Reserva.Numero + " guardada");
                if (VentanaPrincipal.Actual != null) VentanaPrincipal.Actual.ActualizarResumen();
                LimpiarSeleccion();
                Dibujar();
            }
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
