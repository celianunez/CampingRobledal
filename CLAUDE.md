# CLAUDE.md · Camping El Robledal · Gestión (IPO1)

Contexto para el agente de IA. Léelo entero antes de tocar nada. Si algo de aquí choca con el enunciado oficial, **manda el enunciado** (resumen en `docs/enunciado.md`).

## Qué es esto

Prototipo WPF de gestión de un camping para la práctica de **IPO1 (21757)**, ESI Ciudad Real, curso 2026/27.
Grupo A (bilingüe): **Adam Gouram El Bouhali** y **Celia Núñez Acosta**. Zips de entrega: `Gouram_Nunez.zip`.

## Dónde estamos (26 oct 2026)

- Hito 1 (requisitos + 18 bocetos): entregado el 23 oct; defensa en la sesión 5 (26–29 oct).
- Ahora toca el **Hito 2**: primer prototipo WPF con ventanas, formularios y la funcionalidad que haya. Entrega **viernes 4 dic 2026**, defensa 7–16 dic.
- **Práctica final**: 8 ene 2027 (extraordinaria 29 ene 2027).
- Plan y checklist: `docs/planificacion.md`.

## Mapa de documentos

| Necesitas… | Mira |
|---|---|
| Qué pide el enunciado y cómo se evalúa | `docs/enunciado.md` |
| Requisito RF/RNF concreto | `docs/requisitos.md` |
| Cómo es una pantalla, cómo se llama su ventana y sus controles | `docs/pantallas.md` |
| Colores, estados, textos de botones, mensajes | `docs/guia-estilo.md` |
| Dónde va cada clase, datos de prueba | `docs/arquitectura.md` |
| Una decisión ya tomada | `DECISIONS.md` |
| Ramas, commits, PRs | `CONTRIBUTING.md` |
| Cómo se hizo algo en clase (layouts, eventos, login) | `docs/apuntes-wpf.md` |
| Los bocetos originales | `docs/hito1/Hito1_Gouram_Nunez.pdf` |

## Reglas de código (obligatorias)

- **WPF clásico como en clase**: XAML + C# code-behind. Layouts `Grid`, `StackPanel`, `DockPanel`, `WrapPanel`, `Canvas`; eventos en code-behind; `MessageBox` o ventanas propias para confirmar; `ToolTip` en todo control interactivo; ventanas que se pasan datos por constructor/propiedades.
- **Nada de MVVM, frameworks ni NuGets.** Se permite `ObservableCollection<T>` y `DataGrid`/`ListView` con `Binding` sencillo (se ve en la sesión 7, *Listas y tablas*).
- **Prefijos de controles**: `btn`, `txt`, `lbl`, `pnl`, `lst`, `dg`, `cmb`, `chk`, `rb`, `img`, `dp` (DatePicker), `pass` (PasswordBox), `tab`/`tbi` (TabControl/TabItem), `sld`, `prg`, `mnu`. Manejadores: `btnGuardar_Click`.
- Nombres de clases, variables, comentarios y textos de la interfaz **en español**.
- **Sin persistencia**: datos de prueba en `Datos/DatosPrueba.cs` (o XML como recurso). Imágenes en `imagenes/` con *Build Action = Resource*.
- Ventanas **redimensionables**: `Grid` con `*`, `MinWidth`/`MinHeight`; nada de posiciones absolutas fuera de `Canvas`.
- Que **no pete nunca**: validar antes de convertir (`int.TryParse`, `DateTime` nulos, selección vacía en tablas).
- Entregar siempre `.xaml` + `.xaml.cs` completos con `namespace CampingRobledal` (o subespacio) y `x:Class` correctos.

## Reglas de diseño (de las decisiones del Hito 1, ver `DECISIONS.md`)

- Una ventana principal con **4 pestañas**: Alojamientos (Parcelas | Bungalows), Reservas, Monitores, Actividades.
- Cada pestaña es **maestro-detalle**: búsqueda y filtros arriba, tabla, botones *Nuevo / Modificar / Eliminar* debajo, ficha a la derecha.
- Cabecera fija: foto, nombre, cargo, usuario, último acceso, **Ayuda (F1)**, **Preferencias**, **Salir**. Barra de estado abajo.
- Altas y modificaciones en **diálogos modales** (`ShowDialog`); doble clic en fila = modificar.
- Estados con **color + texto** siempre y con los colores de `docs/guia-estilo.md`.
- Confirmaciones con verbo («Eliminar», «Anular reserva», «Dar de baja»); por defecto **Cancelar**.
- Validación al salir del campo, error junto al campo con ejemplo, Guardar deshabilitado hasta corregir, obligatorios con `*`.
- Vocabulario: «Anular reserva» (nunca «Cancelar reserva»), «Dar de baja» para monitores.

## Forma de trabajar

- Una rama `feature/*` por funcionalidad (tabla en `README.md`). Nunca commits directos a `main` ni a `develop`: todo por pull request revisado por la otra persona.
- Commits en español, en imperativo y con prefijo: `feat:`, `fix:`, `docs:`, `style:`, `refactor:`.
- No cambiar una decisión de `DECISIONS.md` por iniciativa propia: proponerla y esperar a que el grupo la acepte.
- Si falta un dato, mirar el enunciado y los bocetos antes de inventar; si se inventa, decirlo.
