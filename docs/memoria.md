# Memoria final · esqueleto

Borrador de la memoria que se entrega en PDF el 8 ene 2027. Se rellena en la rama `docs/memoria`. Las secciones son las que pide el enunciado; entre corchetes, de dónde sale el contenido.

---

## Portada

- Camping El Robledal · Gestión — Memoria de la práctica de IPO I (21757), curso 2026/27
- ESI Ciudad Real, UCLM · Grupo de laboratorio A
- Adam Gouram El Bouhali — __ % de participación
- Celia Núñez Acosta — __ % de participación
- Fecha de entrega

## 1. Introducción

Objeto y delimitación de la práctica: prototipo de escritorio WPF para que el personal de un camping gestione alojamientos, disponibilidad, reservas, monitores y actividades. Qué incluye y qué no (sin persistencia, un único rol, sin pagos). [`docs/enunciado.md`]

## 2. Análisis comparativo

Campspot, Cloudbeds y Pitchup.com: qué son, funciones relacionadas, qué tomamos y qué evitamos, tabla comparativa. [`docs/estudio-apps.md`]

## 3. Análisis de requisitos

- Listado final de RF y RNF (actualizado con los cambios desde el Hito 1). [`docs/requisitos.md`]
- **Diagrama de casos de uso** con el actor Gestor y los CU-01 a CU-10. [`docs/requisitos.md`, apartado Casos de uso]

## 4. Bocetos

Los 18 bocetos de Balsamiq con su descripción, el mapa de navegación y los cambios respecto al Hito 1. [`docs/hito1/`, `docs/pantallas.md`]

## 5. Tecnología y recursos

- WPF sobre .NET, XAML + C#, Visual Studio 2026 Community.
- Balsamiq Mockups para los bocetos.
- Iconos: origen y licencia de cada pack.
- Imágenes de prueba: origen y licencia.
- Herramientas de IA usadas y para qué.

## 6. Justificación del diseño de la GUI

Una subsección por principio, con capturas de la interfaz final:

- **Minimizar la navegación**: ventana única con pestañas, maestro-detalle, diálogos modales. [D-001, D-002, D-004]
- **Gestalt**: proximidad (filtros juntos, acciones juntas), semejanza (mismo color por estado), región común (GroupBox), alineación.
- **Consistencia**: misma estructura y posición de botones en todas las pestañas; vocabulario. [D-002, D-008, `docs/guia-estilo.md`]
- **Color**: paleta verde/madera (metáfora del robledal), estados con color + texto, contraste. [D-005]
- **Iconos y metáforas**: galería de fotos, calendario, barra de progreso del cupo.
- **Prevención y gestión de errores**: controles restrictivos, validación al salir del campo con ejemplo, confirmaciones con verbo. [D-006, D-007]
- **Feedback**: barra de estado, título de la ventana, datos calculados al momento.
- **Ayuda**: tooltips, F1 contextual, Acerca de.
- **Flexibilidad y adaptación**: atajos de teclado, preferencias (si se hace).
- **Layouts**: Grid con `*`, redimensionado, `MinWidth/MinHeight`.

## 7. Manual de usuario

Enlace al vídeo (o nombre del fichero) y guion: login → alojamientos → disponibilidad → nueva reserva → monitores → actividades → ayuda → salir.

---

## Guion del vídeo

1. Arrancar e intentar entrar con una contraseña mal (error junto al campo). Entrar bien: cabecera con foto y último acceso.
2. Parcelas: buscar, filtrar por estado, ver la ficha. Nueva parcela con un error de validación y corregirlo.
3. Bungalows: galería de fotos.
4. Ver disponibilidad, seleccionar días libres y reservar.
5. Reservas: ficha con noches e importe; check-in; anular con confirmación.
6. Monitores: alta con foto; ir a sus actividades.
7. Actividades: barra de cupo; modificar.
8. F1 en una pestaña; Acerca de; Preferencias (si se hace).
9. Salir con cambios sin guardar.
