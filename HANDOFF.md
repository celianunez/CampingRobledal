# HANDOFF — Camping El Robledal
Actualizado el 24 de noviembre de 2026 · rama claude/camping-robledal-fases-0-1-uximej
## Objetivo
Completar la fase 3 de altas, modificaciones y confirmaciones.
## Hecho (verificado)
Compilación Release correcta en Windows (.NET SDK 8.0.428). Prueba WPF de los cinco formularios nuevos, edición de reserva sin mutar el original, capacidad, solapamiento, estancias consecutivas y guardado modal superada.
## En curso
Fase 4 escrita (sin compilar en Windows todavía): VentanaDisponibilidad (calendario alojamiento × día, selección de días libres, Reservar), DialogoAcercaDe y VentanaAyuda con búsqueda y tema según pestaña. Conectadas desde Parcelas, Bungalows, DialogoReserva, menú Ayuda y F1.

## Siguiente paso (exacto)
1. Compilar en Windows y corregir errores de la fase 4.
2. Probar: calendario (clic en día libre, segundo clic para ampliar, Reservar), «Ver disponibilidad» desde el formulario de reserva, F1 en cada pestaña, Acerca de.
3. Fase 5: repaso de tooltips, integración en develop, prueba en otro equipo, merge a main, etiqueta hito2.

## Datos del entorno
WPF para Windows, .NET 8.
## Decisiones tomadas (y por qué)
Se conservan las decisiones del hito 1 y los nombres del XAML.
## No hacer / trampas conocidas
Cerrar sin guardar debe conservar los valores originales.
## Preguntas abiertas para Adam
Ninguna.
## Cómo verificar
Compilar CampingRobledal.sln y probar altas, cambios, descartes y bajas desde las cinco tablas.
