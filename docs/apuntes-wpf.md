# Apuntes de laboratorio WPF (S0–S3)

Resumen de las transparencias de las sesiones de laboratorio (los PDF están en el Campus Virtual). Sirve para escribir el código como se ha visto en clase (`D-011`).

## Herramientas (Campus Virtual)

- **Visual Studio 2026** – versión **Community** (hay PDF de instrucciones de instalación y un "Primer Proyecto de Prueba – Hola Mundo").
- **Balsamiq Mockups**:
  - Enlace a versión de 30 días.
  - Versión de escritorio v2.2.24 (necesita **Adobe AIR**, que también está en el Campus): funcionalidad completa 7 días, pero la **importación/exportación XML sigue funcionando** después, así que se puede usar indefinidamente. La exportación XML se copia al **portapapeles** → pegarla en un .txt para importarla luego.
  - Versión web https://balsamiq.cloud/ durante 14 días (también importa/exporta XML).
  - Los ficheros **.bmml** son también XML (se pueden abrir con un editor de texto).

---

## Apuntes S2 – Introducción a WPF

**WPF**: API de .NET para apps de escritorio con GUI en Windows. Combina:
- **XAML** (lenguaje de marcado) → *apariencia*
- **C#** (código subyacente, *code-behind*) → *comportamiento*
→ Se desacopla la GUI (XAML) de la lógica de presentación (C#).

**Clase Window** (System.Windows, hereda de ContentControl): área **no cliente** (icono, título, menú del sistema, botones min/max/cerrar, borde) + área **cliente** (contenido de la app).

**Controles por categorías**: layout, cuadros de diálogo, botones, información, selección, entrada, menús, presentación de datos, fechas, medios… Docs: https://learn.microsoft.com/es-es/dotnet/desktop/wpf/controls/
Modelo de composición **jerárquico** (casi todo puede contener otros elementos) y especificación **declarativa** de la GUI.

### Layouts (heredan de Panel; hijos en la propiedad `Children`)
Todos salvo Canvas tienen gestor de disposición (se reajustan al redimensionar).

| Panel | Descripción | Posicionamiento | Tamaño |
|---|---|---|---|
| **Canvas** | Posición explícita por coordenadas relativas al lienzo. No redimensiona hijos. Se usa para dibujar. | Fijo (coordenadas) | Fijo |
| **DockPanel** | Zonas Top, Bottom, Left, Right y centro (el último hijo rellena). | Relativo al resto | Fijo |
| **Grid** | Cuadrícula de filas/columnas. El más versátil. Tamaños: absoluto (px), `Auto`, proporcional (`*`). Un elemento puede ocupar varias celdas (`Grid.RowSpan/ColumnSpan`). Índices desde 0. | Por celdas | Variable |
| **StackPanel** | Pila en una línea, `Orientation` Horizontal/Vertical, `FlowDirection`. | Secuencial | Fijo |
| **WrapPanel** | Secuencial; salta de línea al llegar al borde. | Secuencial con salto | Fijo |

### Eventos
Programación orientada a eventos: el usuario interrumpe (clic, tecla…) y se ejecutan los métodos manejadores (*event handlers / listeners / oyentes*) asociados al evento, en secuencia. Un evento en .NET es un objeto con la lista de referencias a sus handlers. Firma típica: `private void btnX_Click(object sender, RoutedEventArgs e)`.

### XAML
eXtensible Application Markup Language: XML declarativo de Microsoft para definir GUIs WPF (objetos = elementos, propiedades = atributos, jerarquía). Archivos `.xaml`, UTF-8. WPF enlaza automáticamente el XAML con su code-behind.

### C# vs Java (lo esencial)
`using` ≈ `import`; `Main` (mayúscula, puede ir sin parámetros) vs `main`; `string` vs `String`; `Console.WriteLine` vs `System.out.println`. Fuertemente tipado, sin variables sin inicializar, recolector de basura. Propiedades con `get; set;` en vez de getters/setters.

### Compilación
Cada `.xaml` genera un `.baml` (XAML tokenizado) y un `.g.cs` (clase parcial que carga y conecta los elementos). Se combinan con los `.cs` → ejecutable.

---

## Ejemplo guiado S2: "IntensificacionesESI"

Ventana de preinscripción para elegir intensificación (Computación, Ingeniería de Computadores, Ingeniería del Software, Tecnologías de la Información). Recurso: `esi_logo_corto.png` (de "Recursos Ejemplo guiado" en Campus; añadirlo al proyecto).

Diseño: **Grid de 3 filas × 3 columnas**; fila 0 → **WrapPanel** (logo + Label título); fila 1 → **StackPanel** con 4 **RadioButton**; fila 2 → **Button** "Enviar". Diseño descendente: primero propiedades de la ventana, luego el Grid, luego los paneles y controles. (Los tamaños son de ejemplo.)

### MainWindow.xaml (versión final con las mejoras del guion)
```xml
<Window x:Class="IntensificacionesESI.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:IntensificacionesESI"
        mc:Ignorable="d"
        Title="Preinscripción-Paso 1" Height="400" Width="320"
        ResizeMode="NoResize"
        WindowStyle="ThreeDBorderWindow"
        WindowStartupLocation="CenterScreen"
        ToolTip="Seleccione la intensificación deseada y después pulse el botón Enviar">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="41*"/>
            <RowDefinition Height="206*"/>
            <RowDefinition Height="124*"/>
        </Grid.RowDefinitions>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="10*"/>
            <ColumnDefinition Width="81*"/>
            <ColumnDefinition Width="10*"/>
        </Grid.ColumnDefinitions>

        <WrapPanel Grid.Column="1" HorizontalAlignment="Left" VerticalAlignment="Top" Height="41" Width="243">
            <Image Width="38" Height="38" Margin="2" Source="esi_logo_corto.png"/>
            <Label Content="ELIGE TU INTENSIFICACIÓN" Width="195" Height="37" Margin="2"
                   Background="#FF747373" Foreground="White" FontSize="14"
                   HorizontalContentAlignment="Center" VerticalContentAlignment="Center"/>
        </WrapPanel>

        <StackPanel x:Name="pnlOpciones" Grid.Row="1" Grid.Column="1"
                    HorizontalAlignment="Left" VerticalAlignment="Top"
                    Height="186" Width="224" Margin="9,10,0,0">
            <RadioButton Content="Computación" Height="40" Width="212" Margin="2,2,2,2"
                         FontFamily="Segoe UI Semibold" Background="White" BorderThickness="2"
                         HorizontalAlignment="Left" VerticalAlignment="Center" VerticalContentAlignment="Center"/>
            <RadioButton Content="Ingeniería de Computadores" Height="40" Width="212" Margin="2,2,2,2"
                         FontFamily="Segoe UI Semibold" Background="White" BorderThickness="2"
                         HorizontalAlignment="Left" VerticalAlignment="Center" VerticalContentAlignment="Center"/>
            <RadioButton Content="Ingeniería del Software" Height="40" Width="212" Margin="2,2,0,2"
                         FontFamily="Segoe UI Semibold" Background="White" BorderThickness="2"
                         HorizontalAlignment="Left" VerticalAlignment="Center" VerticalContentAlignment="Center"/>
            <RadioButton Content="Tecnologías de la Información" Height="40" Width="212" Margin="2,2,0,2"
                         FontFamily="Segoe UI Semibold" Background="White" BorderThickness="2"
                         HorizontalAlignment="Left" VerticalAlignment="Center" VerticalContentAlignment="Center"/>
        </StackPanel>

        <Button x:Name="btnEnviar" Content="Enviar"
                Grid.Row="2" Grid.Column="1"
                HorizontalAlignment="Center" VerticalAlignment="Top"
                Margin="5" Width="120" Height="30"
                FontWeight="Bold" Background="#FF7C4747" Foreground="#FFFDFCFC"
                ToolTip="Pulsar para enviar selección" Cursor="Pen"
                Click="btnEnviar_Click"/>
    </Grid>
</Window>
```

### MainWindow.xaml.cs
```csharp
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace IntensificacionesESI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnEnviar_Click(object sender, RoutedEventArgs e)
        {
            var cadenaMensaje = new StringBuilder("Intensificación elegida: ");

            // Comprobación de los radiobuttons
            foreach (RadioButton rb in pnlOpciones.Children)
            {
                if (rb.IsChecked == true)
                {
                    cadenaMensaje.AppendLine(rb.Content.ToString());
                    break;
                }
            }
            // Mostrar mensaje
            MessageBox.Show(cadenaMensaje.ToString(), "Intensificación seleccionada");
        }
    }
}
```

Notas del guion:
- Las propiedades se pueden poner en XAML o en la ventana Propiedades (Pincel, Diseño, Apariencia, Común).
- Las filas/columnas del Grid se crean pulsando en el borde del Grid en el diseñador.
- Para 4 RadioButton iguales: crear uno, configurarlo y copiar/pegar 3 veces; editar el texto pinchando en él.
- Sin código C# la ventana ya funciona (todo en XAML). Por defecto no sale centrada → `WindowStartupLocation="CenterScreen"`.
- Para usar un control desde C# hay que darle `x:Name`.

---

## Apuntes S1 – Presentación

- Objetivo y tecnología: los de arriba (WPF, XAML + C#).
- Ejemplos que se verán: gestión de eventos, diseño de formularios, listados de información.
- Evaluación: prototipo 2,5 + memoria 0,5 + hitos 0,5 (voluntario) = 3,5.
- Hito 1: requisitos y bocetos → entrega 23 oct, defensa 26–29 oct. Hito 2: primer prototipo → entrega 4 dic, defensa 7–16 dic. Obligatorio entregar y defender ambos para los 0,5 puntos.

---

## Apuntes S3 – Eventos (ejemplo guiado "Login Peliflix")

- Ventana 800×500, `MinHeight=450`, `MinWidth=700`, Grid de 9 filas × 6 columnas con tamaños `*` (crecen al redimensionar) y última fila fija de 45 px para la barra de estado (`lblEstado`).
- Imágenes como recurso: carpeta `imagenes/`, "Incluir en el proyecto".
- Controles: `txtUsuario` (TextBox), `passContrasena` (PasswordBox, `IsEnabled=False` al inicio), `btnLogin`, `imgAvatar`, labels, `lblEstado`.
- Orden de tabulación con `TabIndex` (txtUsuario=1, passContrasena=2, btnLogin=3).
- Eventos vistos: `KeyDown` (Enter en usuario → habilita contraseña y le pasa el foco), `MouseEnter`/`MouseLeave` (rollover del avatar), `MouseDown` en el Grid (coordenadas con `e.GetPosition(this)`), `KeyUp` en la contraseña (validación a cada tecla), `Closing` de la ventana (MessageBox de despedida), `Click` del botón.
- Credenciales de ejemplo: `usuario = "admin"`, `password = "ipo1"`.
- Versión 1: feedback en la barra de estado → **error de usabilidad**: el feedback queda en la periferia, no en el área de interés (fóvea).
- Versión 2: validación escalonada con método genérico `ComprobarEntrada(valorIntroducido, valorValido, Control componenteEntrada, Image imagenFeedBack)` que pone borde/fondo verde o rojo y un icono check/cross (`imagenes/check.png`, `imagenes/cross.png`) junto al campo.
