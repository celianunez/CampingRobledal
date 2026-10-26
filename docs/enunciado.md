# Enunciado de la práctica (resumen)

Resumen fiel del enunciado oficial de IPO1 2026/27 (`EnunciadoPractica2627.pdf`, en el Campus Virtual). **Si hay dudas, manda el PDF.**

## Objetivo

Desarrollar un **prototipo** de aplicación de escritorio con GUI en WPF teniendo en cuenta la usabilidad y los factores humanos vistos en teoría (leyes de la Gestalt, metáforas, selección de colores y layouts…). Atención especial a:

- Diseño de ventanas y posicionamiento de controles.
- Diseño de formularios y listados de información.
- Lenguaje visual (iconos) y textual (mensajes de aviso, error y ayuda).
- Entrada correcta de información con feedback adecuado.
- Ajustar el tamaño de las ventanas y **minimizar la navegación**.

## Qué hay que hacer

Prototipo para la **gestión de un complejo turístico rural o camping**. No tiene que ser completamente funcional: lo importante son las pantallas y diálogos, con algunos datos de prueba.

### 1. Alojamientos y reservas (funcionalidad principal)

- **Parcelas**: categorías elegidas por nosotros (pequeña, mediana, deluxe, autocaravana…) con distinto precio por noche y tarifas según **temporada baja, media y alta**. Datos: tamaño, ubicación en el camping (cerca de aseos/fregaderos, piscina, bar-restaurante, entrada…), servicios (agua, luz, desagüe, setos, sombra…).
- **Bungalows / cabañas de madera**: tamaño, capacidad máxima, precio por noche, estancia mínima (opcional), equipamiento (menaje, microondas, ropa de cama, toallas, wifi…), descripción breve y **galería de fotos**.
- De cada parcela y cabaña: consultar **disponibilidad** (ocupada, libre, en reparación, pendiente de limpieza…) y **hacer reservas**.
- **Reserva**: fechas de entrada y salida, nombre de quien reserva, teléfono/s, e-mail (opcional), nº de ocupantes, solicitudes especiales (cuna, menaje, movilidad reducida…), horas estimadas de entrada y salida, etc.

### 2. Empleados temporales (guías y monitores de verano)

Altas, bajas, consulta y modificación: nombre y apellidos, foto reciente, teléfono/s, correo/s, idiomas, formación, restricciones de disponibilidad…

### 3. Actividades de verano

Altas, bajas y modificación: monitor/es, horario, cupo mínimo y máximo, destinatarios (adultos o niños), precio por hora y/o por mes, área del camping, descripción, materiales o equipamiento…

### Requisitos generales

- Gestión = altas, bajas, modificaciones y consultas con tratamiento **dinámico** en **listas y tablas**.
- Un solo rol: la persona encargada de la gestión. Puede haber varias, así que hay **login (usuario y clave)** y, tras entrar, se muestran sus **datos, su foto y la fecha del último acceso**.
- **Sin persistencia** obligatoria: pocos datos de prueba en código o en ficheros de texto/XML como recursos.
- Accesible **en todo momento**: salir, ayuda y adaptación (opcional).
- **Ayuda y documentación**: ayuda sensible al contexto (tooltips…), **confirmación** de acciones que pueden causar error o pérdida de datos, e información de **autoría** (autores, fecha, versión).
- **Adaptación (opcional)**: fuente, tamaño, layout, look & feel…

## Evaluación (3,5 puntos de prácticas)

| Parte | Puntos | Qué se valora |
|---|---|---|
| Prototipo final | **2,5** | Complejidad; calidad y usabilidad del diseño (estética, iconos y metáforas, ayuda, prevención y gestión de errores, feedback, flexibilidad, consistencia, presentación de datos, layouts, navegación); funcionamiento seguro y estable; creatividad |
| Hitos (voluntario) | **0,5** | Hay que **entregar y defender los dos**. Hito 1: requisitos y bocetos. Hito 2: primer prototipo |
| Memoria | **0,5** | Redacción y presentación; justificación del diseño conectada con la teoría; calidad del manual de usuario en vídeo |

## Entregas

Por Campus Virtual, basta con que entregue uno. **Un único `.zip`** por tarea con el primer apellido de cada integrante: `Gouram_Nunez.zip`.

| Entrega | Fecha | Contenido |
|---|---|---|
| Hito 1 | 23 oct 2026 | PDF con requisitos, estudio de 2–4 apps y bocetos de baja fidelidad (+ `.bmml`) |
| Hito 2 | 4 dic 2026 | Primer prototipo software: ventanas, formularios y funcionalidad hasta la fecha |
| Final | 8 ene 2027 (extra. 29 ene 2027) | Proyecto VS comprimido + `leeme.txt` + memoria PDF + vídeo (o `.txt` con enlace) |

### Contenido de la entrega final

- **Prototipo**: proyecto de Visual Studio que funcione en distintos equipos, con `leeme.txt` (integrantes, login y password de prueba).
- **Memoria PDF** (esqueleto en [memoria.md](memoria.md)):
  1. Portada con integrantes y **% de participación**.
  2. Introducción: objeto y delimitación.
  3. Análisis comparativo de 2–4 apps/webs.
  4. Requisitos finales + **diagrama de casos de uso** (se valora).
  5. Bocetos.
  6. Tecnología y recursos (librerías, iconos…).
  7. **Justificación del diseño de la GUI** según la teoría, con capturas.
- **Manual de usuario en vídeo** (por ejemplo, grabado con MS Teams). Si pesa mucho, enlace en un `.txt`.
