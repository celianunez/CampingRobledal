# Camping El Robledal · Gestión

Prototipo de aplicación de escritorio en **WPF (.NET, XAML + C#)** para gestionar un camping: alojamientos (parcelas y bungalows), disponibilidad, reservas, monitores temporales y actividades de verano.

Práctica de laboratorio de **Interacción Persona-Ordenador I (21757)** · Grado en Ingeniería Informática · ESI Ciudad Real (UCLM) · curso 2026/27.

| | |
|---|---|
| **Integrantes** | Adam Gouram El Bouhali · Celia Núñez Acosta |
| **Grupo de laboratorio** | A (bilingüe) |
| **Profesores de prácticas** | Ana Isabel Molina · Manuel Ortega · Yoel Arroyo |
| **Tecnología** | WPF + C# code-behind, Visual Studio 2026 Community, bocetos en Balsamiq |
| **Solución VS** | `CampingRobledal.sln` (proyecto `CampingRobledal`) |

## Estado

| Hito | Fecha | Estado |
|---|---|---|
| Hito 1 · requisitos y bocetos | entrega 23 oct 2026 · defensa 26–29 oct | ✅ Entregado ([PDF](docs/hito1/Hito1_Gouram_Nunez.pdf)) |
| Hito 2 · primer prototipo WPF | entrega **4 dic 2026** · defensa 7–16 dic | ⏳ En curso |
| Práctica final | **8 ene 2027** (extraordinaria 29 ene 2027) | ⏳ Pendiente |

El plan detallado está en [docs/planificacion.md](docs/planificacion.md).

## Documentación

| Documento | Contenido |
|---|---|
| [CLAUDE.md](CLAUDE.md) | Contexto y reglas para el agente de IA (léelo antes de pedirle nada) |
| [DECISIONS.md](DECISIONS.md) | Registro de decisiones de diseño y de proyecto |
| [CONTRIBUTING.md](CONTRIBUTING.md) | Flujo de ramas, commits, pull requests y convenciones de código |
| [docs/enunciado.md](docs/enunciado.md) | Resumen fiel del enunciado, evaluación y entregas |
| [docs/requisitos.md](docs/requisitos.md) | Requisitos funcionales y no funcionales (Hito 1) con trazabilidad a ramas |
| [docs/pantallas.md](docs/pantallas.md) | Las 18 pantallas, mapa de navegación y nombres de ventanas/controles |
| [docs/guia-estilo.md](docs/guia-estilo.md) | Colores, estados, botones, vocabulario y mensajes de la interfaz |
| [docs/arquitectura.md](docs/arquitectura.md) | Estructura de la solución WPF, modelos y datos de prueba |
| [docs/estudio-apps.md](docs/estudio-apps.md) | Estudio de Campspot, Cloudbeds y Pitchup.com |
| [docs/apuntes-wpf.md](docs/apuntes-wpf.md) | Apuntes de laboratorio S1–S3 (layouts, eventos, login Peliflix) |
| [docs/planificacion.md](docs/planificacion.md) | Sesiones, calendario, reparto por ramas y checklist de entregas |
| [docs/memoria.md](docs/memoria.md) | Esqueleto de la memoria final |

## Ramas

`main` guarda solo lo entregado; el trabajo se integra en `develop` mediante pull request. Cada funcionalidad tiene su rama `feature/*` (detalle en [CONTRIBUTING.md](CONTRIBUTING.md)).

| Rama | Pantallas | Requisitos |
|---|---|---|
| `feature/estructura-base` | solución VS, modelos, datos de prueba, estilos | RNF-01, RNF-02, RNF-05 |
| `feature/login` | 01, 02 | RF-01 – RF-04 |
| `feature/ventana-principal` | cabecera, pestañas, barra de estado | RF-04 – RF-08, RF-41 |
| `feature/parcelas` | 03, 05 | RF-09 – RF-12 |
| `feature/bungalows` | 04, 06 | RF-13 – RF-17 |
| `feature/disponibilidad` | 07 | RF-18 – RF-20 |
| `feature/reservas` | 08, 09 | RF-21 – RF-26 |
| `feature/monitores` | 10, 11 | RF-27 – RF-31 |
| `feature/actividades` | 12, 13 | RF-32 – RF-35 |
| `feature/ayuda-confirmaciones` | 14, 15, 16, 17 | RF-36 – RF-41 |
| `feature/preferencias` | 18 (opcional) | RF-42 |
| `docs/memoria` | memoria PDF, vídeo, `leeme.txt` | — |

## Cómo ejecutarlo

1. Abrir `CampingRobledal.sln` con Visual Studio 2026 Community (carga de trabajo *Desarrollo de escritorio de .NET*).
2. Compilar y ejecutar (F5).
3. Entrar con el usuario de prueba (lo indicamos también en `leeme.txt`):

| Usuario | Contraseña |
|---|---|
| `adam` | `ipo1` |
| `celia` | `ipo1` |

El prototipo no tiene persistencia: los datos de prueba se cargan al arrancar y se pierden al cerrar.
