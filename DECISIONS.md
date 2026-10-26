# DECISIONES

Registro de las decisiones del proyecto: diseño de la interfaz, alcance, tecnología, organización y uso de IA. Una entrada por decisión, escrita cuando se toma. Los detalles de código no van aquí, van en los commits.

Cada entrada lleva fecha, hito y origen; después contexto, decisión, alternativas, motivo y consecuencias. Una decisión que cambia no se reescribe: se añade una línea **Estado** que apunta a la que la sustituye.

Este fichero es también contexto para el agente de IA: debe seguir estas decisiones y no cambiarlas por su cuenta.

---

## D-001 · Una sola ventana principal con cuatro pestañas
**Fecha:** 23/10/2026 · **Hito:** 1 · **Origen:** bocetos 03, 04, 08, 10, 12

- **Contexto:** El enunciado pide minimizar la navegación y gestionar alojamientos, reservas, monitores y actividades.
- **Decisión:** Una ventana principal con `TabControl` de 4 pestañas: Alojamientos (con vistas Parcelas y Bungalows), Reservas, Monitores y Actividades.
- **Alternativas:** Menú inicial con una ventana por módulo; menú lateral con páginas.
- **Motivo:** Todo a un clic, sin perder el contexto; es lo que mejor cumple «minimizar la navegación».
- **Consecuencias:** Los formularios son diálogos modales que vuelven a la misma pestaña (RNF-04).

## D-002 · Patrón maestro-detalle en todas las pestañas
**Fecha:** 23/10/2026 · **Hito:** 1 · **Origen:** RF-05, RNF-05

- **Contexto:** Hay cinco entidades con altas, bajas, modificaciones y consultas.
- **Decisión:** Misma estructura en todas: búsqueda y filtros arriba, tabla en el centro, botones *Nuevo / Modificar / Eliminar* debajo y ficha del seleccionado a la derecha.
- **Alternativas:** Abrir la ficha en una ventana aparte al seleccionar.
- **Motivo:** Consistencia (Gestalt: semejanza y proximidad) y menos ventanas.
- **Consecuencias:** Conviene un `UserControl` o estilo común para la barra de filtros y la de botones.

## D-003 · Cabecera fija y barra de estado
**Fecha:** 23/10/2026 · **Hito:** 1 · **Origen:** RF-04, RF-08, RF-41

- **Decisión:** Cabecera con foto, nombre, cargo, usuario y último acceso del gestor, y botones **Ayuda (F1)**, **Preferencias** y **Salir**. Barra de estado inferior con el resultado de la última acción y un resumen de la pestaña.
- **Motivo:** El enunciado exige salir, ayuda y adaptación accesibles siempre; la barra da feedback (RNF-08).
- **Consecuencias:** Además hay menús *Archivo* y *Ayuda*; el login también tiene enlaces Ayuda y Salir.

## D-004 · Altas y modificaciones en diálogos modales
**Fecha:** 23/10/2026 · **Hito:** 1 · **Origen:** RF-06, bocetos 05, 06, 09, 11, 13

- **Decisión:** Cada alta o modificación abre un diálogo modal (`ShowDialog`). Doble clic en una fila abre la modificación. El mismo diálogo sirve para alta y para modificación; el título indica qué se edita.
- **Alternativas:** Edición en línea en la tabla; edición dentro de la ficha.
- **Motivo:** Separa consultar de editar y permite validar antes de guardar.

## D-005 · Estados con color y texto, con el mismo código en toda la app
**Fecha:** 23/10/2026 · **Hito:** 1 · **Origen:** RF-18, RNF-09

- **Decisión:** Estados de alojamiento: Libre, Ocupado, Reservado, En reparación, Pendiente de limpieza. Estados de reserva: Confirmada, Check-in, Finalizada, Anulada. Siempre se muestran con texto (o letra en el calendario) además del color. Colores en `docs/guia-estilo.md`.
- **Motivo:** Accesibilidad: la información nunca depende solo del color.

## D-006 · Confirmaciones con verbo y Cancelar por defecto
**Fecha:** 23/10/2026 · **Hito:** 1 · **Origen:** RF-37, RF-38, bocetos 14, 15

- **Decisión:** Los diálogos de confirmación dicen qué se borra y sus consecuencias, sus botones son verbos («Eliminar», «Anular reserva», «Dar de baja») y el botón por defecto es Cancelar. Al salir con cambios: «Guardar y salir», «Salir sin guardar», «Cancelar».
- **Alternativas:** `MessageBox` estándar con Sí/No.
- **Motivo:** Prevención de errores y mensajes en lenguaje del usuario.
- **Consecuencias:** Hace falta una ventana de confirmación propia (`VentanaConfirmacion`); `MessageBox` no permite cambiar el texto de los botones.

## D-007 · Validación al salir del campo
**Fecha:** 23/10/2026 · **Hito:** 1 · **Origen:** RNF-06, RNF-07, apuntes S3

- **Decisión:** Se valida en `LostFocus` (y en `KeyUp` donde ayude). El error aparece junto al campo, en lenguaje del usuario y con un ejemplo; borde rojo + icono, y *Guardar* deshabilitado hasta corregir. Obligatorios con `*`. Se prefieren controles que restringen la entrada (desplegables, `DatePicker`, casillas).
- **Motivo:** La versión 2 del login de S3: el feedback en la barra de estado queda fuera del foco de atención.

## D-008 · Vocabulario del dominio
**Fecha:** 23/10/2026 · **Hito:** 1 · **Origen:** bocetos 08, 10

- **Decisión:** «Anular reserva» en vez de «Cancelar reserva»; «Dar de baja» para monitores; «Eliminar» para el resto.
- **Motivo:** Evitar confundir anular una reserva con el botón Cancelar de los diálogos; usar el lenguaje de contratos temporales.

## D-009 · Calendario de disponibilidad con reserva directa
**Fecha:** 23/10/2026 · **Hito:** 1 · **Origen:** RF-19, RF-20, boceto 07, estudio de Campspot

- **Decisión:** Tabla alojamiento × día con leyenda, filtros y navegación por periodos. Seleccionar días libres consecutivos y pulsar «Reservar» abre la reserva con alojamiento y fechas ya rellenos.
- **Motivo:** Minimizar pasos al reservar.

## D-010 · Tres tarifas fijas por temporada
**Fecha:** 23/10/2026 · **Hito:** 1 · **Origen:** RF-10, estudio de apps

- **Decisión:** Cada parcela tiene precio por noche en temporada baja, media y alta. Sin precios dinámicos.
- **Motivo:** Lo pide el enunciado y es más fácil de entender que los precios dinámicos de Campspot.

## D-011 · WPF clásico, sin MVVM ni librerías externas
**Fecha:** 26/10/2026 · **Hito:** 2 · **Origen:** enunciado, sesiones S2–S3

- **Decisión:** XAML + C# code-behind, como en las sesiones de laboratorio. Sin MVVM, sin frameworks y sin paquetes NuGet. Prefijos de controles de `CLAUDE.md`.
- **Motivo:** Tenemos que poder defender y explicar todo el código con lo visto en clase.

## D-012 · Sin persistencia: datos de prueba en código
**Fecha:** 26/10/2026 · **Hito:** 2 · **Origen:** enunciado, RNF-02

- **Decisión:** Una clase `DatosPrueba` crea al arrancar los gestores, parcelas, bungalows, reservas, monitores y actividades en `ObservableCollection<T>`. Todas las ventanas trabajan sobre esas colecciones.
- **Alternativas:** Ficheros XML como recurso. Se podrá pasar a XML más adelante si da tiempo.
- **Motivo:** Lo más sencillo para tener datos y listados dinámicos desde el primer día.

## D-013 · Organización del repositorio en ramas por funcionalidad
**Fecha:** 26/10/2026 · **Hito:** 2

- **Decisión:** `main` solo recibe lo entregado en cada hito; `develop` integra el trabajo; una rama `feature/*` por bloque de pantallas (tabla en `README.md`). Todo entra en `develop` por pull request revisado por la otra persona.
- **Motivo:** Trabajar en paralelo sin pisarnos y poder enseñar en la defensa qué hizo cada uno.

## D-014 · Usuarios de prueba
**Fecha:** 26/10/2026 · **Hito:** 2 · **Estado:** propuesta, pendiente de confirmar por el grupo

- **Decisión:** Dos gestores de prueba, `adam` y `celia`, con contraseña `ipo1` (la misma que en el ejemplo de S3). Se indican en `leeme.txt`.
- **Motivo:** El enunciado pide varios encargados y que el `leeme.txt` lleve login y password de prueba.
