# Planificación

## Sesiones de laboratorio (Planificacion-2627.pdf)

| Sesión | Fechas | Contenido |
|---|---|---|
| 1 | 28 sep – 2 oct | Presentación + Balsamiq |
| 2 | | Ejemplo guiado WPF |
| 3 | | Eventos |
| 4 | | Trabajo autónomo |
| 5 | 26–29 oct | **Defensa Hito 1** |
| 6 | | Formularios |
| 7 | | Listas y tablas |
| 8 | | Trabajo autónomo |
| 9 | | Trabajo autónomo |
| 10 | 7–11 dic | **Defensa Hito 2** |
| 11 | 14–16 dic | Defensa Hito 2 / trabajo autónomo – dudas |
| — | **8 ene 2027** | **Entrega práctica final** (extraordinaria: 29 ene 2027) |

El PDF solo pone fecha a las sesiones 1, 5, 10 y 11; el resto son las semanas intermedias.

## Plan hasta el Hito 2 (4 dic 2026)

El Hito 2 pide el primer prototipo con ventanas, formularios y la funcionalidad que haya. Objetivo: **todas las pantallas creadas y navegables**, con las tablas llenas de datos de prueba y los formularios de alta funcionando en al menos parcelas y reservas.

| Semana | Objetivo | Ramas |
|---|---|---|
| 26 oct – 1 nov | Defensa Hito 1. Crear solución, modelos, datos de prueba y estilos | `feature/estructura-base` |
| 2 – 8 nov | Login y ventana principal con cabecera, pestañas vacías y barra de estado (encaja con S6 *Formularios*) | `feature/login`, `feature/ventana-principal` |
| 9 – 15 nov | Pestañas con tablas y fichas (encaja con S7 *Listas y tablas*) | `feature/parcelas`, `feature/bungalows`, `feature/reservas`, `feature/monitores`, `feature/actividades` |
| 16 – 22 nov | Diálogos de alta/modificación con validación; confirmaciones | las mismas + `feature/ayuda-confirmaciones` |
| 23 – 29 nov | Disponibilidad, Acerca de, ayuda básica, repaso de tooltips | `feature/disponibilidad`, `feature/ayuda-confirmaciones` |
| 30 nov – 4 dic | Integrar en `develop`, probar en otro equipo, merge a `main`, etiqueta `hito2`, **entregar** | `develop` → `main` |

## Plan hasta la entrega final (8 ene 2027)

| Semana | Objetivo |
|---|---|
| 7 – 16 dic | Defensa Hito 2; apuntar lo que digan los profesores como issues |
| 14 – 20 dic | Correcciones del Hito 2; terminar bajas, navegación monitor ↔ actividad, check-in |
| 21 – 27 dic | Ayuda completa, Preferencias (opcional), pulido visual e iconos |
| 28 dic – 3 ene | Memoria (`docs/memoria`), capturas, diagrama de casos de uso, vídeo |
| 4 – 8 ene | Prueba en otro equipo, `leeme.txt`, zip `Gouram_Nunez.zip`, **entregar** |

## Reparto propuesto

A confirmar por el grupo; se apunta en cada issue y en la portada de la memoria (% de participación).

| Adam | Celia |
|---|---|
| `feature/estructura-base` | `feature/login` |
| `feature/ventana-principal` | `feature/parcelas` |
| `feature/bungalows` | `feature/reservas` |
| `feature/disponibilidad` | `feature/monitores` |
| `feature/actividades` | `feature/ayuda-confirmaciones` |
| `feature/preferencias` (opcional) | `docs/memoria` (a medias) |

## Checklist de entregas

### Hito 2 · `Gouram_Nunez.zip` · 4 dic 2026

- [ ] Proyecto VS completo (sin `bin/`, `obj/`, `.vs/`) que compila en otro equipo.
- [ ] Login funcionando con los usuarios de prueba.
- [ ] Ventana principal con cabecera, 4 pestañas y barra de estado.
- [ ] Tablas con datos de prueba en las 4 pestañas.
- [ ] Diálogos de alta/modificación creados (aunque no todos validen aún).
- [ ] Confirmación de eliminar y de salir.
- [ ] Defensa preparada: qué hace cada uno, qué falta.

### Final · `Gouram_Nunez.zip` · 8 ene 2027

- [ ] Proyecto VS comprimido que funciona en distintos equipos.
- [ ] `leeme.txt` con integrantes, usuario y contraseña de prueba (plantilla en `docs/entrega/leeme.txt`).
- [ ] Memoria PDF con las 7 secciones (esqueleto en `docs/memoria.md`).
- [ ] Diagrama de casos de uso.
- [ ] Capturas de la interfaz final en la justificación del diseño.
- [ ] Vídeo manual de usuario (o `.txt` con el enlace).
- [ ] Porcentaje de participación en la portada.
