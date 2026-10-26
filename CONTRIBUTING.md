# Cómo trabajamos

## Ramas

```
main ─────────────●────────────────────●──────────►  solo entregas (Hito 2, final)
                  ▲                    ▲
develop ──●───●───┴──●───●───●─────────┴──────────►  integración
           \     /    \     /
feature/x   ●───●      ●───●                          una por funcionalidad
```

| Rama | Para qué | Quién escribe |
|---|---|---|
| `main` | Lo que se entrega. Se actualiza desde `develop` al cerrar cada hito, con una etiqueta (`hito2`, `final`). | Solo por PR |
| `develop` | Integración: siempre compila y arranca. | Solo por PR |
| `feature/<nombre>` | Una funcionalidad (bloque de pantallas). Sale de `develop` y vuelve a `develop`. | Quien la tenga asignada |
| `docs/<nombre>` | Memoria, vídeo, `leeme.txt`. | Cualquiera |
| `fix/<nombre>` | Corrección de un fallo encontrado en `develop`. | Cualquiera |

Ramas creadas (ver la tabla de requisitos en el `README.md`):

`feature/estructura-base` · `feature/login` · `feature/ventana-principal` · `feature/parcelas` · `feature/bungalows` · `feature/disponibilidad` · `feature/reservas` · `feature/monitores` · `feature/actividades` · `feature/ayuda-confirmaciones` · `feature/preferencias` · `docs/memoria`

**Orden recomendado:** `feature/estructura-base` primero, porque crea la solución, los modelos y los datos de prueba de los que dependen las demás. Después `login` y `ventana-principal`, y a partir de ahí el resto en paralelo.

### Día a día

```bash
git checkout develop && git pull
git checkout feature/reservas
git merge develop                 # traer lo último antes de empezar
# ... trabajar, compilar, probar ...
git add -A && git commit -m "feat(reservas): diálogo Nueva reserva con validación"
git push -u origin feature/reservas
```

Cuando la funcionalidad funciona, se abre un **pull request hacia `develop`** y lo revisa la otra persona. No se hace `push --force` sobre ramas compartidas.

## Commits

En español, en imperativo, con prefijo y ámbito opcional:

| Prefijo | Uso |
|---|---|
| `feat:` | Pantalla o funcionalidad nueva |
| `fix:` | Corrección de un fallo |
| `style:` | Solo aspecto (colores, márgenes, iconos) |
| `refactor:` | Reorganizar código sin cambiar comportamiento |
| `docs:` | Documentación, memoria |
| `chore:` | Configuración del proyecto, `.gitignore` |

Ejemplo: `feat(parcelas): filtros por estado y categoría`.

## Pull requests

Antes de pedir revisión:

- [ ] Compila sin errores ni avisos nuevos.
- [ ] Arranca, se hace login y se llega a la pantalla tocada sin excepciones.
- [ ] La ventana se redimensiona bien (probar al mínimo y maximizada).
- [ ] Todos los controles interactivos tienen `ToolTip`.
- [ ] Nombres con los prefijos de `CLAUDE.md`; textos en español y con el vocabulario de `docs/guia-estilo.md`.
- [ ] Acciones destructivas con confirmación; formularios con validación al salir del campo.
- [ ] Si se ha tomado una decisión nueva, está en `DECISIONS.md`.

## Código

Las reglas completas están en [CLAUDE.md](CLAUDE.md). Lo esencial:

- WPF clásico: XAML + code-behind, sin MVVM ni NuGets.
- Prefijos: `btn`, `txt`, `lbl`, `pnl`, `lst`, `dg`, `cmb`, `chk`, `rb`, `img`, `dp`, `pass`, `tab`, `tbi`, `prg`, `mnu`.
- Una ventana por fichero, en `Ventanas/`; modelos en `Modelos/`; datos de prueba en `Datos/`; imágenes en `imagenes/` (*Build Action = Resource*).
- No subir `bin/`, `obj/` ni `.vs/` (ya están en `.gitignore`).

## Uso de IA

Podemos usar Claude para generar y revisar código y documentación. Todo lo generado se revisa y se entiende antes de hacer merge: en la defensa hay que poder explicarlo. El contexto que usa el agente está en `CLAUDE.md`.
