# Requisitos

Requisitos entregados en el Hito 1 (`docs/hito1/Hito1_Gouram_Nunez.pdf`, págs. 3–7). La columna **Rama** indica dónde se implementa cada uno y **Estado** se actualiza al hacer merge en `develop`.

Un único tipo de usuario: la persona encargada de gestionar alojamientos, reservas, monitores y actividades. Puede haber varias, cada una con su cuenta.

Leyenda de estado: ⬜ pendiente · 🟨 en curso · ✅ hecho · ➖ opcional

## Requisitos funcionales

### Acceso a la aplicación

| ID | Descripción | Bocetos | Rama | Estado |
|---|---|---|---|---|
| RF-01 | Login con usuario y contraseña, con casilla para mostrar la contraseña. | 01 | `feature/login` | ⬜ |
| RF-02 | Si las credenciales fallan: error junto al campo, se conserva el usuario y el foco vuelve a la contraseña. | 02 | `feature/login` | ⬜ |
| RF-03 | Varias cuentas de gestor con el mismo rol. | 01 | `feature/login` | ⬜ |
| RF-04 | Tras entrar, la cabecera muestra nombre y apellidos, cargo, usuario, foto y fecha y hora del último acceso. | 03, 04, 08, 10, 12 | `feature/ventana-principal` | ⬜ |

### Ventana principal y navegación

| ID | Descripción | Bocetos | Rama | Estado |
|---|---|---|---|---|
| RF-05 | Cuatro pestañas (Alojamientos, Reservas, Monitores, Actividades), cada una maestro-detalle: búsqueda y filtros arriba, tabla, botones de alta/modificación/baja debajo y ficha a la derecha. | 03, 04, 08, 10, 12 | `feature/ventana-principal` | ⬜ |
| RF-06 | Altas y modificaciones en diálogos modales; doble clic en una fila abre su modificación. | 03, 05, 06, 09, 11, 13 | todas las de gestión | ⬜ |
| RF-07 | Las tablas se actualizan dinámicamente tras cada alta, baja o modificación. | 03, 04, 08, 10, 12 | todas las de gestión | ⬜ |
| RF-08 | Barra de estado con el resultado de la última acción y un resumen de la pestaña. | 03, 04, 08, 10, 12 | `feature/ventana-principal` | ⬜ |

### Parcelas

| ID | Descripción | Bocetos | Rama | Estado |
|---|---|---|---|---|
| RF-09 | Tabla de parcelas (número, categoría, tamaño, ubicación, estado, precio de la temporada actual); búsqueda por número o ubicación; filtros por estado y categoría. | 03 | `feature/parcelas` | ⬜ |
| RF-10 | Datos: número, categoría (pequeña, mediana, deluxe, autocaravana), tamaño m², ubicación, capacidad, estado, servicios (agua, luz, desagüe, setos, sombra, wifi), precio baja/media/alta, observaciones y plano de ubicación. | 03, 05 | `feature/parcelas` | ⬜ |
| RF-11 | Alta y modificación; obligatorios número, categoría, tamaño, ubicación y las tres tarifas. | 05 | `feature/parcelas` | ⬜ |
| RF-12 | Baja con confirmación que indica las reservas futuras afectadas. | 03, 14 | `feature/parcelas` | ⬜ |

### Bungalows

| ID | Descripción | Bocetos | Rama | Estado |
|---|---|---|---|---|
| RF-13 | Tabla de bungalows (nombre, tipo, plazas, precio, estancia mínima, estado); búsqueda por nombre; filtros por estado y plazas. | 04 | `feature/bungalows` | ⬜ |
| RF-14 | Datos: nombre, tipo (cabaña de madera, bungalow, familiar, adaptado), tamaño, capacidad, precio por noche, estancia mínima (opcional), estado, equipamiento (menaje, microondas, ropa de cama, toallas, wifi, aire acondicionado, barbacoa, cuna), descripción y galería de fotos. | 04, 06 | `feature/bungalows` | ⬜ |
| RF-15 | Galería en la ficha: foto grande, miniaturas y botones anterior/siguiente. | 04 | `feature/bungalows` | ⬜ |
| RF-16 | Alta y modificación, con añadir, quitar y reordenar fotos (la primera es la principal). | 06 | `feature/bungalows` | ⬜ |
| RF-17 | Baja con confirmación. | 04, 14 | `feature/bungalows` | ⬜ |

### Disponibilidad

| ID | Descripción | Bocetos | Rama | Estado |
|---|---|---|---|---|
| RF-18 | Estados de alojamiento: libre, ocupado, reservado, en reparación, pendiente de limpieza; siempre texto + mismo color. | 03, 04, 07 | `feature/disponibilidad` | ⬜ |
| RF-19 | Calendario de ocupación (alojamiento × día) con intervalo de fechas, filtros, periodo anterior/siguiente y leyenda. Se abre desde «Ver disponibilidad», «Ver calendario» en la ficha y desde el formulario de reserva. | 03, 04, 07, 09 | `feature/disponibilidad` | ⬜ |
| RF-20 | Seleccionar días libres consecutivos y pulsar «Reservar» abre una reserva con alojamiento y fechas rellenos. | 07, 09 | `feature/disponibilidad` | ⬜ |

### Reservas

| ID | Descripción | Bocetos | Rama | Estado |
|---|---|---|---|---|
| RF-21 | Tabla de reservas (número, cliente, alojamiento, entrada, salida, personas, estado: confirmada, check-in, finalizada, anulada); búsqueda por cliente o teléfono; filtros por fecha de entrada, estado y alojamiento. | 08 | `feature/reservas` | ⬜ |
| RF-22 | Datos: alojamiento, fechas y horas estimadas de entrada y salida, nombre y apellidos, teléfono y segundo teléfono opcional, e-mail opcional, ocupantes y solicitudes especiales (cuna, movilidad reducida, menaje extra, mascota, texto libre). | 08, 09 | `feature/reservas` | ⬜ |
| RF-23 | Cálculo automático de noches e importe estimado. | 08, 09 | `feature/reservas` | ⬜ |
| RF-24 | Alta y modificación desde la pestaña Reservas, desde la ficha de un alojamiento o desde el calendario. Ocupantes ≤ capacidad del alojamiento. | 03, 04, 07, 08, 09 | `feature/reservas` | ⬜ |
| RF-25 | Anular reserva con confirmación; se queda en el listado como «Anulada». | 08, 14 | `feature/reservas` | ⬜ |
| RF-26 | Registrar check-in desde la ficha y acceso directo a la ficha del alojamiento (ampliación). | 08 | `feature/reservas` | ⬜ |

### Monitores

| ID | Descripción | Bocetos | Rama | Estado |
|---|---|---|---|---|
| RF-27 | Tabla de monitores (nombre, especialidad, idiomas, disponibilidad); búsqueda por nombre; filtros por idioma y disponibilidad. La ficha muestra foto, contacto, idiomas, formación, restricciones, contrato y actividades. | 10 | `feature/monitores` | ⬜ |
| RF-28 | Datos: nombre y apellidos, foto, especialidad, teléfono(s), correo(s), idiomas, formación, periodo de contrato y restricciones de disponibilidad. | 10, 11 | `feature/monitores` | ⬜ |
| RF-29 | Alta y modificación, con carga de foto. | 11 | `feature/monitores` | ⬜ |
| RF-30 | «Dar de baja» con confirmación. | 10, 14 | `feature/monitores` | ⬜ |
| RF-31 | Navegación monitor → sus actividades y actividad → su monitor. | 10, 12 | `feature/monitores` + `feature/actividades` | ⬜ |

### Actividades

| ID | Descripción | Bocetos | Rama | Estado |
|---|---|---|---|---|
| RF-32 | Tabla de actividades (nombre, destinatarios, horario, monitores, inscritos/cupo, precio); búsqueda por nombre; filtros por destinatarios, área y monitor. Ficha con foto, datos, cupo con barra de progreso, descripción y material. | 12 | `feature/actividades` | ⬜ |
| RF-33 | Datos: nombre, destinatarios (adultos, niños, todos) y edad mínima, área, monitores, horario (días y horas), cupo mínimo y máximo, inscritos, precio por hora y/o mes, descripción y materiales. | 12, 13 | `feature/actividades` | ⬜ |
| RF-34 | Alta y modificación; cupo máximo ≥ mínimo. | 13 | `feature/actividades` | ⬜ |
| RF-35 | Baja con confirmación. | 12, 14 | `feature/actividades` | ⬜ |

### Ayuda, confirmaciones y salida

| ID | Descripción | Bocetos | Rama | Estado |
|---|---|---|---|---|
| RF-36 | Tooltip en botones y controles con su función y atajo. | 03 | todas | ⬜ |
| RF-37 | Confirmación de eliminar / dar de baja / anular indicando qué se borra y sus consecuencias; por defecto Cancelar. | 14 | `feature/ayuda-confirmaciones` | ⬜ |
| RF-38 | Salir con cambios sin guardar: «Guardar y salir», «Salir sin guardar», «Cancelar». | 15 | `feature/ayuda-confirmaciones` | ⬜ |
| RF-39 | Ventana de ayuda (F1 o botón) con índice y buscador; abre el tema de la pestaña actual. | 16 | `feature/ayuda-confirmaciones` | ⬜ |
| RF-40 | «Acerca de»: nombre de la app, autores, fecha y versión. | 17 | `feature/ayuda-confirmaciones` | ⬜ |
| RF-41 | Salir y ayuda siempre accesibles: cabecera, menús Archivo y Ayuda, enlaces en el login. | 01, 03, 04, 08, 10, 12 | `feature/ventana-principal` | ⬜ |

### Adaptación (opcional)

| ID | Descripción | Bocetos | Rama | Estado |
|---|---|---|---|---|
| RF-42 | Preferencias: fuente, tamaño, tema (claro, oscuro, alto contraste), mostrar/ocultar tooltips, vista previa y restablecer. | 03, 18 | `feature/preferencias` | ➖ |

## Requisitos no funcionales

| ID | Aspecto | Descripción |
|---|---|---|
| RNF-01 | Plataforma | Escritorio Windows en WPF; ventanas redimensionables. |
| RNF-02 | Alcance | Sin persistencia obligatoria: datos de prueba en código o XML como recurso. |
| RNF-03 | Usuarios | Un único rol (gestor del camping). |
| RNF-04 | Navegación mínima | Todo desde una ventana con pestañas; formularios modales que vuelven a la misma pestaña. |
| RNF-05 | Consistencia | Misma estructura, posición de botones (principal a la derecha, Cancelar a su izquierda) y vocabulario en todas las pestañas. |
| RNF-06 | Prevención de errores | Controles que restringen la entrada; obligatorios con `*`. |
| RNF-07 | Validación | Al salir del campo; error junto al campo con ejemplo; guardar deshabilitado hasta corregir. |
| RNF-08 | Retroalimentación | Barra de estado, título de la ventana con lo que se edita, selección resaltada, datos calculados al momento. |
| RNF-09 | Accesibilidad | Nunca solo color; atajos (F1 ayuda, Supr eliminar); orden de tabulación lógico. |
| RNF-10 | Idioma | Interfaz, mensajes y ayuda en español. |
| RNF-11 | Diseño | Principios de usabilidad de teoría: Gestalt, metáforas, color, iconos, layouts. |
| RNF-12 | Estabilidad | Funcionamiento seguro y estable en distintos equipos. |

## Casos de uso (para el diagrama de la memoria)

Actor único: **Gestor del camping**.

| CU | Nombre | RF |
|---|---|---|
| CU-01 | Iniciar sesión | RF-01 – RF-03 |
| CU-02 | Gestionar parcelas (consultar, alta, modificar, eliminar) | RF-09 – RF-12 |
| CU-03 | Gestionar bungalows (consultar, alta, modificar, eliminar) | RF-13 – RF-17 |
| CU-04 | Consultar disponibilidad | RF-18 – RF-20 |
| CU-05 | Gestionar reservas (consultar, alta, modificar, anular, check-in) | RF-21 – RF-26 |
| CU-06 | Gestionar monitores (consultar, alta, modificar, dar de baja) | RF-27 – RF-31 |
| CU-07 | Gestionar actividades (consultar, alta, modificar, eliminar) | RF-32 – RF-35 |
| CU-08 | Consultar ayuda / Acerca de | RF-36, RF-39, RF-40 |
| CU-09 | Cambiar preferencias (opcional) | RF-42 |
| CU-10 | Salir de la aplicación | RF-38, RF-41 |

Relaciones: CU-04 «extend» CU-05 (Reservar desde el calendario); CU-02 a CU-07 «include» *Confirmar acción* en las bajas; todos menos CU-01 requieren sesión iniciada.
