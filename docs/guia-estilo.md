# Guía de estilo de la interfaz

Reglas visuales y de texto comunes a todas las pantallas. Sirven para la consistencia (RNF-05) y para justificar el diseño en la memoria. Los colores se definen una sola vez como recursos en `App.xaml` (`Estilos/Colores.xaml`) y se usan con `{StaticResource …}`.

## Colores base

| Recurso | Color | Uso |
|---|---|---|
| `ColorPrimario` | `#2E6B3F` (verde bosque) | Cabecera, botón principal, pestaña activa |
| `ColorPrimarioClaro` | `#E8F1EA` | Fondo de la ficha y de los grupos |
| `ColorAcento` | `#B5703A` (madera) | Detalles, iconos, selección |
| `ColorFondo` | `#FAFAF7` | Fondo de ventana |
| `ColorTexto` | `#222222` | Texto normal |
| `ColorTextoSecundario` | `#666666` | Ayudas en línea, texto en gris |
| `ColorPeligro` | `#C62828` | Botones destructivos, errores |
| `ColorExito` | `#2E7D32` | Validación correcta, confirmaciones |

El verde y la madera vienen del nombre del camping (metáfora del robledal). El contraste texto/fondo debe ser al menos 4,5:1.

## Estados (siempre color + texto)

### Alojamientos

| Estado | Letra (calendario) | Color | Icono |
|---|---|---|---|
| Libre | L | `#2E7D32` verde | ✔ |
| Ocupado | O | `#C62828` rojo | ● |
| Reservado | R | `#1565C0` azul | 📅 |
| En reparación | X | `#6D4C41` marrón | 🔧 |
| Pendiente de limpieza | P | `#F9A825` amarillo (texto oscuro) | 🧹 |

### Reservas

| Estado | Color |
|---|---|
| Confirmada | `#1565C0` azul |
| Check-in | `#2E7D32` verde |
| Finalizada | `#757575` gris |
| Anulada | `#C62828` rojo, texto tachado |

En las tablas, la columna Estado es una «pastilla» (`Border` redondeado) con el color de fondo y el texto. En el calendario, la celda lleva el color y la letra.

## Tipografía

- Fuente: **Segoe UI** (la de Windows). Títulos de ventana 18 pt seminegrita, títulos de grupo 13 pt seminegrita, texto 12 pt.
- Con Preferencias (RF-42) el tamaño base se escala; por eso los tamaños se definen como recursos y no a mano en cada control.

## Botones

- **Principal** (Guardar, Entrar, Confirmar reserva, Reservar): fondo `ColorPrimario`, texto blanco, `IsDefault="True"`, a la **derecha**.
- **Cancelar**: botón neutro a la izquierda del principal, `IsCancel="True"` (Esc).
- **Destructivo** (Eliminar, Anular reserva, Dar de baja): texto o fondo `ColorPeligro`; nunca es el botón por defecto.
- Botones de la barra de acciones de cada pestaña en el mismo orden: **Nuevo · Modificar · Eliminar**.
- Botones con icono + texto; los que solo llevan icono, siempre con tooltip.
- Deshabilitados (`IsEnabled=False`) cuando no aplican: Modificar y Eliminar sin fila seleccionada; Guardar con errores.

## Vocabulario

| Usar | No usar | Por qué |
|---|---|---|
| Anular reserva | Cancelar reserva | No confundir con el botón Cancelar |
| Dar de baja (monitor) | Eliminar monitor | Lenguaje de contratos temporales |
| Eliminar (parcela, bungalow, actividad) | Borrar, Suprimir | Uniformidad |
| Nueva / Nuevo | Añadir, Crear | Uniformidad en la barra de acciones |
| Modificar | Editar | Uniformidad |
| Alojamiento | Unidad, habitación | Término del dominio que engloba parcela y bungalow |
| Gestor | Administrador, usuario | Nombre del rol |

## Mensajes

- **Error de validación**: qué pasa + cómo arreglarlo + ejemplo. «El teléfono debe tener 9 cifras, p. ej. 612345678». Junto al campo, en rojo con icono ✖.
- **Confirmación**: título con la acción («¿Eliminar la parcela P-12?»), qué se pierde y consecuencias, botones con verbo.
- **Barra de estado**: pasado y concreto. «Reserva R-0042 guardada», «Parcela P-12 eliminada».
- Sin jerga técnica (nada de «excepción», «null», «índice»). Tratamiento de **tú**, como en los bocetos.

## Tooltips

Todo control interactivo lleva `ToolTip` con qué hace y su atajo: «Eliminar la parcela seleccionada (Supr)». Los campos de formulario explican el formato esperado.

## Atajos de teclado

| Tecla | Acción |
|---|---|
| F1 | Ayuda (tema de la pestaña actual) |
| Supr | Eliminar / dar de baja / anular el elemento seleccionado (con confirmación) |
| Ctrl+N | Nuevo en la pestaña actual |
| Enter (fila) / doble clic | Modificar |
| Ctrl+F | Ir al cuadro de búsqueda |
| Esc | Cancelar el diálogo |
| Alt+F4 | Salir (con confirmación si hay cambios) |

## Layout

- `Grid` con filas y columnas en `*` y `Auto`; `MinWidth`/`MinHeight` en todas las ventanas (principal: mínimo 1100 × 700).
- Márgenes múltiplos de 4 (4, 8, 12, 16).
- Formularios en dos columnas: etiqueta alineada a la derecha + control; obligatorios con `*` en la etiqueta.
- Grupos con `GroupBox` titulado (Gestalt: región común).
