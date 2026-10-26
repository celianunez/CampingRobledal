# Arquitectura del prototipo

Estructura propuesta para la solución de Visual Studio. Es WPF clásico (XAML + code-behind, `D-011`), sin persistencia (`D-012`). La crea la rama `feature/estructura-base`.

## Solución

```
CampingRobledal.sln
└── CampingRobledal/                     proyecto WPF (.NET 8 o superior, net8.0-windows)
    ├── App.xaml / App.xaml.cs           StartupUri = Ventanas/VentanaLogin.xaml; carga Estilos/*.xaml
    ├── Estilos/
    │   ├── Colores.xaml                 pinceles de docs/guia-estilo.md
    │   ├── Botones.xaml                 BotonPrincipal, BotonPeligro, BotonNeutro
    │   └── Controles.xaml               DataGrid, GroupBox, TextBox con error, pastilla de estado
    ├── Temas/                           (opcional, RF-42) Claro.xaml, Oscuro.xaml, AltoContraste.xaml
    ├── Modelos/
    │   ├── Gestor.cs
    │   ├── Alojamiento.cs               clase base abstracta: Id, Nombre, Capacidad, Estado
    │   ├── Parcela.cs                   : Alojamiento
    │   ├── Bungalow.cs                  : Alojamiento
    │   ├── Reserva.cs
    │   ├── Monitor.cs
    │   ├── Actividad.cs
    │   └── Enumeraciones.cs             EstadoAlojamiento, EstadoReserva, CategoriaParcela, Temporada…
    ├── Datos/
    │   └── DatosPrueba.cs               colecciones estáticas con los datos de prueba
    ├── Utilidades/
    │   ├── Validacion.cs                métodos de comprobación reutilizables (como ComprobarEntrada de S3)
    │   └── Temporadas.cs                temporada de una fecha y precio de una parcela en esa fecha
    ├── Ventanas/
    │   ├── VentanaLogin.xaml(.cs)                01-02
    │   ├── VentanaPrincipal.xaml(.cs)            cabecera, menú, TabControl, barra de estado
    │   ├── VentanaDisponibilidad.xaml(.cs)       07
    │   ├── VentanaConfirmacion.xaml(.cs)         14-15
    │   ├── VentanaAyuda.xaml(.cs)                16
    │   └── Dialogos/
    │       ├── DialogoParcela.xaml(.cs)          05
    │       ├── DialogoBungalow.xaml(.cs)         06
    │       ├── DialogoReserva.xaml(.cs)          09
    │       ├── DialogoMonitor.xaml(.cs)          11
    │       ├── DialogoActividad.xaml(.cs)        13
    │       ├── DialogoAcercaDe.xaml(.cs)         17
    │       └── DialogoPreferencias.xaml(.cs)     18
    ├── Paneles/                         UserControl de cada pestaña (uno por rama → menos conflictos)
    │   ├── PanelParcelas.xaml(.cs)               03
    │   ├── PanelBungalows.xaml(.cs)              04
    │   ├── PanelReservas.xaml(.cs)               08
    │   ├── PanelMonitores.xaml(.cs)              10
    │   └── PanelActividades.xaml(.cs)            12
    └── imagenes/                        Build Action = Resource
        ├── iconos/                      (añadir, modificar, eliminar, ayuda, salir, check, cross…)
        ├── gestores/  parcelas/  bungalows/  monitores/  actividades/
        └── logo.png
```

**Por qué cada pestaña es un `UserControl`:** así cada rama `feature/*` trabaja en su propio fichero y `VentanaPrincipal.xaml` solo contiene `<paneles:PanelParcelas/>`. Los `UserControl` se ven en clase en *GUIs dinámicas y reutilización*.

## Modelos

Clases planas con propiedades `{ get; set; }`, sin lógica de interfaz. Propiedades calculadas de solo lectura para lo que se muestra en tablas (por ejemplo, `Reserva.Noches`, `Reserva.Importe`, `Actividad.TextoCupo` → «6 / 8»).

| Clase | Propiedades principales |
|---|---|
| `Gestor` | Usuario, Contrasena, Nombre, Apellidos, Cargo, Foto, UltimoAcceso |
| `Alojamiento` (abstracta) | Id, Nombre, TamanoM2, Capacidad, Estado, Fotos |
| `Parcela` | Categoria, Ubicacion, Servicios (bools), PrecioBaja/Media/Alta, Observaciones, Plano |
| `Bungalow` | Tipo, PrecioNoche, EstanciaMinima (int?), Equipamiento (bools), Descripcion |
| `Reserva` | Numero, Alojamiento, Entrada, Salida, HoraEntrada, HoraSalida, Nombre, Apellidos, Telefono, Telefono2, Email, Ocupantes, Cuna, Movilidad, MenajeExtra, Mascota, OtrasSolicitudes, Estado |
| `Monitor` | Nombre, Apellidos, Foto, Especialidad, Telefonos, Correos, Idiomas, Formacion, InicioContrato, FinContrato, Restricciones, Activo |
| `Actividad` | Nombre, Destinatarios, EdadMinima, Area, Monitores, Dias, HoraInicio, HoraFin, CupoMinimo, CupoMaximo, Inscritos, PrecioHora, PrecioMes, Descripcion, Materiales, Foto |

## Datos de prueba

`DatosPrueba` expone `ObservableCollection<T>` estáticas (`Gestores`, `Parcelas`, `Bungalows`, `Reservas`, `Monitores`, `Actividades`) que se rellenan una vez al arrancar. Las tablas se enlazan con `ItemsSource`, así que añadir o quitar de la colección actualiza la tabla sola (RF-07). Para que la tabla refleje una **modificación** de un objeto ya listado, se llama a `dgX.Items.Refresh()` tras cerrar el diálogo.

Mínimo recomendado para que las pantallas se vean llenas en la defensa: 2 gestores, 10 parcelas, 5 bungalows, 8 reservas (con todos los estados), 4 monitores y 5 actividades.

## Paso de datos entre ventanas

Como en clase, por constructor y propiedades públicas:

```csharp
var dlg = new DialogoParcela(parcelaSeleccionada) { Owner = Window.GetWindow(this) };
if (dlg.ShowDialog() == true)
{
    dgParcelas.Items.Refresh();
    VentanaPrincipal.MostrarEstado($"Parcela {parcelaSeleccionada.Nombre} modificada");
}
```

- Diálogo con constructor sin parámetros = **alta**; con un objeto = **modificación** (copia los valores al abrir y solo los escribe al guardar, para que Cancelar no deje cambios a medias).
- `DialogResult = true` al guardar.
- El gestor que ha entrado se pasa de `VentanaLogin` a `VentanaPrincipal` por constructor.

## Validación

Patrón de la versión 2 del login de S3, generalizado en `Utilidades/Validacion.cs`:

```csharp
public static bool MarcarCampo(Control campo, TextBlock lblError, bool valido, string mensaje)
```

Pone borde rojo y el mensaje junto al campo si no es válido, o lo limpia si lo es. Cada diálogo tiene un `ComprobarFormulario()` que habilita o deshabilita `btnGuardar`.
