using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CampingRobledal.Utilidades;

namespace CampingRobledal
{
    /// <summary>Ayuda con índice de temas y búsqueda (pantalla 16, RF-39).</summary>
    public partial class VentanaAyuda : Window
    {
        private static VentanaAyuda instancia;

        private class Tema
        {
            public string Titulo;
            public string Contenido;
            public override string ToString() { return Titulo; }
        }

        private readonly List<Tema> temas = new List<Tema>
        {
            new Tema { Titulo = "Iniciar sesión", Contenido =
                "1. Escribe tu usuario y pulsa Intro para pasar a la contraseña.\n" +
                "2. Escribe tu contraseña. Marca «Mostrar contraseña» si quieres comprobarla.\n" +
                "3. Pulsa Entrar.\n\n" +
                "Si te equivocas, verás el aviso junto al campo; el usuario se conserva y solo tienes que repetir la contraseña." },
            new Tema { Titulo = "Alojamientos: parcelas y bungalows", Contenido =
                "Para añadir un alojamiento:\n" +
                "1. Abre la pestaña Alojamientos y elige Parcelas o Bungalows.\n" +
                "2. Pulsa Nuevo (Ctrl+N), rellena los campos con * y pulsa Guardar.\n\n" +
                "Para modificar: selecciónalo en la tabla y pulsa Modificar, o haz doble clic en la fila.\n" +
                "Para eliminar: selecciónalo, pulsa Eliminar (Supr) y confirma. Verás cuántas reservas se ven afectadas.\n\n" +
                "Usa la búsqueda y los filtros de arriba para encontrar un alojamiento; «Limpiar filtros» los quita." },
            new Tema { Titulo = "Ver la disponibilidad", Contenido =
                "1. En Parcelas o Bungalows pulsa «Ver disponibilidad».\n" +
                "2. Elige el periodo con Desde y Hasta (máximo 6 semanas) o usa Anterior y Siguiente.\n" +
                "3. Filtra por tipo y categoría si lo necesitas.\n" +
                "4. Haz clic en un día libre (L) y, si quieres, en otro día libre de la misma fila para marcar la estancia.\n" +
                "5. Pulsa Reservar: se abre la reserva con el alojamiento y las fechas ya rellenos.\n\n" +
                "Colores y letras: L libre, O ocupado, R reservado, X en reparación, P pendiente de limpieza." },
            new Tema { Titulo = "Reservas", Contenido =
                "Nueva reserva:\n" +
                "1. Abre la pestaña Reservas y pulsa Nueva (Ctrl+N).\n" +
                "2. Elige el alojamiento, las fechas y el número de ocupantes (no puede superar la capacidad).\n" +
                "3. Escribe los datos del cliente; el teléfono lleva 9 cifras, p. ej. 612345678.\n" +
                "4. Pulsa Guardar. El importe se calcula solo.\n\n" +
                "Si el alojamiento ya está ocupado esos días, el formulario te lo dirá. Puedes abrir el calendario con «Ver disponibilidad».\n\n" +
                "Check-in: selecciona una reserva confirmada y pulsa Check-in en su ficha.\n" +
                "Anular: selecciona la reserva y pulsa «Anular reserva» (Supr). La reserva no se borra, queda como Anulada." },
            new Tema { Titulo = "Monitores", Contenido =
                "1. Abre la pestaña Monitores. Puedes buscar por nombre o especialidad y filtrar por idioma o disponibilidad.\n" +
                "2. Nuevo (Ctrl+N) añade un monitor con sus contactos, idiomas, formación y contrato.\n" +
                "3. En su ficha ves las actividades que imparte; «Ver en Actividades» te lleva a esa pestaña.\n\n" +
                "Dar de baja: selecciona el monitor, pulsa «Dar de baja» y confirma. Sigue apareciendo en la lista como «De baja»." },
            new Tema { Titulo = "Actividades", Contenido =
                "1. Abre la pestaña Actividades. Filtra por destinatarios, área o monitor.\n" +
                "2. Nueva (Ctrl+N) abre el formulario: nombre, días, horario, monitores, cupo y precio.\n" +
                "3. La ficha muestra una barra con los inscritos sobre el cupo máximo.\n\n" +
                "El cupo máximo no puede ser menor que el mínimo y los inscritos no pueden superar el máximo." },
            new Tema { Titulo = "Atajos de teclado", Contenido =
                "F1: abrir la ayuda\n" +
                "Ctrl+N: nuevo en la pestaña actual\n" +
                "Ctrl+F: ir al cuadro de búsqueda\n" +
                "Intro o doble clic en una fila: modificar\n" +
                "Supr: eliminar, anular o dar de baja (siempre pide confirmación)\n" +
                "Esc: cerrar un formulario o ventana\n" +
                "Alt+F4: salir de la aplicación" },
            new Tema { Titulo = "Salir de la aplicación", Contenido =
                "Pulsa Salir en la cabecera o elige Archivo > Salir. Se te pedirá confirmación.\n\n" +
                "Si cierras un formulario con cambios sin guardar, podrás elegir entre «Guardar y salir», «Salir sin guardar» o «Cancelar»." }
        };

        public VentanaAyuda()
        {
            InitializeComponent();
            RellenarLista("");
        }

        private void RellenarLista(string busqueda)
        {
            lstTemas.Items.Clear();
            foreach (Tema t in temas)
                if (Texto.Contiene(t.Titulo + " " + t.Contenido, busqueda)) lstTemas.Items.Add(t);

            if (lstTemas.Items.Count > 0) lstTemas.SelectedIndex = 0;
            else
            {
                lblTituloTema.Text = "Sin resultados";
                lblContenidoTema.Text = "No hay temas con esa palabra. Prueba con otra, p. ej. reserva.";
            }
        }

        private void txtBuscarAyuda_TextChanged(object sender, TextChangedEventArgs e)
        {
            RellenarLista(txtBuscarAyuda.Text);
        }

        private void lstTemas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Tema t = lstTemas.SelectedItem as Tema;
            if (t == null) return;
            lblTituloTema.Text = t.Titulo;
            lblContenidoTema.Text = t.Contenido;
        }

        private void SeleccionarTema(string titulo)
        {
            if (string.IsNullOrEmpty(titulo)) return;
            if (txtBuscarAyuda.Text.Length > 0) txtBuscarAyuda.Clear();
            foreach (Tema t in lstTemas.Items)
                if (t.Titulo.StartsWith(titulo))
                {
                    lstTemas.SelectedItem = t;
                    lstTemas.ScrollIntoView(t);
                    return;
                }
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            instancia = null;
        }

        /// <summary>Abre la ayuda (o la trae al frente) en el tema indicado, por su comienzo.</summary>
        public static void Abrir(string tema)
        {
            if (instancia == null)
            {
                instancia = new VentanaAyuda();
                instancia.Show();
            }
            instancia.SeleccionarTema(tema);
            if (instancia.WindowState == WindowState.Minimized) instancia.WindowState = WindowState.Normal;
            instancia.Activate();
        }

        /// <summary>Cierra la ayuda si está abierta (al salir de la aplicación).</summary>
        public static void CerrarSiAbierta()
        {
            if (instancia != null) instancia.Close();
        }
    }
}
