# Pantallas

Las 18 pantallas de los bocetos del Hito 1 (`docs/hito1/Hito1_Gouram_Nunez.pdf`, págs. 10–28), con el nombre de la ventana WPF que les corresponde, los controles principales y la rama donde se implementan. Los nombres de controles siguen los prefijos de `CLAUDE.md`.

## Mapa de navegación

```
VentanaLogin (01-02)
   └─► VentanaPrincipal ──────────────────────────────────────────────┐
         ├─ tbiAlojamientos  (03 Parcelas · 04 Bungalows)             │ Siempre visibles:
         │     Nuevo/Modificar ─► DialogoParcela (05) / DialogoBungalow (06)   Ayuda (F1) ─► VentanaAyuda (16)
         │     Ver disponibilidad ─► VentanaDisponibilidad (07) ─► Reservar ─► DialogoReserva (09)
         │     Reservar ─► DialogoReserva (09)                         │ Ayuda > Acerca de ─► DialogoAcercaDe (17)
         │     Eliminar ─► VentanaConfirmacion (14)                    │ Preferencias ─► DialogoPreferencias (18)
         ├─ tbiReservas (08)   Nueva/Modificar ─► DialogoReserva (09) · Anular ─► (14)   Salir ─► (15) si hay cambios
         ├─ tbiMonitores (10)  Nuevo/Modificar ─► DialogoMonitor (11) · Dar de baja ─► (14)
         └─ tbiActividades (12) Nueva/Modificar ─► DialogoActividad (13) · Eliminar ─► (14)
```

## Resumen

| Nº | Pantalla | Ventana / control WPF | Tipo | Rama |
|---|---|---|---|---|
| 01 | Login | `VentanaLogin` | Ventana | `feature/login` |
| 02 | Login con error | `VentanaLogin` (estado de error) | — | `feature/login` |
| 03 | Principal – Parcelas | `VentanaPrincipal` › `tbiAlojamientos` › `PanelParcelas` | UserControl | `feature/parcelas` |
| 04 | Principal – Bungalows | `VentanaPrincipal` › `tbiAlojamientos` › `PanelBungalows` | UserControl | `feature/bungalows` |
| 05 | Nueva parcela | `DialogoParcela` | Modal | `feature/parcelas` |
| 06 | Nuevo bungalow | `DialogoBungalow` | Modal | `feature/bungalows` |
| 07 | Disponibilidad | `VentanaDisponibilidad` | Modal | `feature/disponibilidad` |
| 08 | Principal – Reservas | `VentanaPrincipal` › `tbiReservas` › `PanelReservas` | UserControl | `feature/reservas` |
| 09 | Nueva reserva | `DialogoReserva` | Modal | `feature/reservas` |
| 10 | Principal – Monitores | `VentanaPrincipal` › `tbiMonitores` › `PanelMonitores` | UserControl | `feature/monitores` |
| 11 | Nuevo monitor | `DialogoMonitor` | Modal | `feature/monitores` |
| 12 | Principal – Actividades | `VentanaPrincipal` › `tbiActividades` › `PanelActividades` | UserControl | `feature/actividades` |
| 13 | Modificar actividad | `DialogoActividad` | Modal | `feature/actividades` |
| 14 | Confirmar eliminación | `VentanaConfirmacion` | Modal | `feature/ayuda-confirmaciones` |
| 15 | Confirmar salida sin guardar | `VentanaConfirmacion` (3 botones) | Modal | `feature/ayuda-confirmaciones` |
| 16 | Ayuda | `VentanaAyuda` | Ventana | `feature/ayuda-confirmaciones` |
| 17 | Acerca de | `DialogoAcercaDe` | Modal | `feature/ayuda-confirmaciones` |
| 18 | Preferencias (opcional) | `DialogoPreferencias` | Modal | `feature/preferencias` |

La cabecera, el menú, el `TabControl` y la barra de estado de `VentanaPrincipal` van en `feature/ventana-principal`.

---

## 01 · Login · `VentanaLogin`

Acceso con usuario y contraseña. Centrada, solo lo imprescindible.

- **Controles:** `imgLogo`, `lblTitulo`, `txtUsuario`, `passContrasena`, `txtContrasenaVisible` + `chkMostrarContrasena`, `btnEntrar` (por defecto, en color), enlaces `btnAyuda` y `btnSalir`.
- **Comportamiento:** Enter en usuario pasa a contraseña (como en S3); `TabIndex` 1-2-3; `btnEntrar` con `IsDefault="True"`.
- **Justificación:** simplicidad y alineación (Gestalt: proximidad); prevención de errores con «mostrar contraseña»; Ayuda y Salir desde el primer momento.
- **RF:** 01, 03, 41.

## 02 · Login – error de autenticación

- **Comportamiento:** al fallar, `lblErrorContrasena` en rojo **con texto e icono** junto al campo, borde rojo en `passContrasena`; se conserva el usuario, se vacía la contraseña y recibe el foco. Si el usuario no existe, el error va junto a `txtUsuario`.
- **Justificación:** error en el foco de atención (versión 2 del login de S3), lenguaje del usuario, no se pierde lo escrito.
- **RF:** 02.

## 03 · Principal – Alojamientos: Parcelas · `PanelParcelas`

- **Cabecera** (común): `imgFotoGestor`, `lblNombreGestor`, `lblCargo`, `lblUsuario`, `lblUltimoAcceso`, `btnAyuda` (F1), `btnPreferencias`, `btnSalir`. Menú `mnuArchivo`, `mnuAyuda`.
- **Selector de vista:** `rbParcelas` / `rbBungalows` (o `TabControl` interior).
- **Filtros:** `txtBuscarParcela` (número o ubicación), `cmbFiltroEstado`, `cmbFiltroCategoria`, `btnLimpiarFiltros`, `btnVerDisponibilidad`.
- **Tabla:** `dgParcelas` — Nº, Categoría, Tamaño, Ubicación, Estado (color + texto), Precio/noche (temporada actual).
- **Acciones:** `btnNuevaParcela`, `btnModificarParcela`, `btnEliminarParcela` (Supr).
- **Ficha:** `GroupBox` con datos, servicios, tarifas, plano (`imgPlano`), `btnVerCalendario`, `btnReservarParcela` (acción principal).
- **Barra de estado:** `lblEstado` (última acción) + resumen («12 libres hoy · temporada media»).
- **RF:** 04, 05, 07, 08, 09, 12, 36, 41.

## 04 · Principal – Alojamientos: Bungalows · `PanelBungalows`

- Igual que 03. **Filtros:** `txtBuscarBungalow`, `cmbFiltroEstado`, `cmbFiltroPlazas`. **Tabla:** `dgBungalows` — Nombre, Tipo, Plazas, Precio/noche, Estancia mín., Estado.
- **Ficha:** galería (`imgFotoGrande`, `lstMiniaturas`, `btnFotoAnterior`, `btnFotoSiguiente`), equipamiento, descripción, `btnReservarBungalow` en la misma posición que en parcelas.
- **RF:** 13, 15, 17.

## 05 · Nueva / Modificar parcela · `DialogoParcela`

- **Bloques** (`GroupBox`): *Identificación* (`txtNumero`*, `cmbCategoria`*, `txtTamano`*, `cmbUbicacion`*, `txtCapacidad`), *Estado y servicios* (`cmbEstado`, `chkAgua`, `chkLuz`, `chkDesague`, `chkSetos`, `chkSombra`, `chkWifi`), *Tarifas por noche* (`txtPrecioBaja`*, `txtPrecioMedia`*, `txtPrecioAlta`*), *Otros* (`txtObservaciones`, `imgPlano` + `btnCargarPlano`).
- **Validación:** en `LostFocus`; `lblError…` junto al campo con ejemplo («Escribe un número, p. ej. 80»); `btnGuardar` deshabilitado hasta que todo esté bien.
- **Botones:** `btnCancelar` (izquierda) · `btnGuardar` (derecha, por defecto). Título: «Nueva parcela» / «Modificar parcela P-12».
- **RF:** 10, 11.

## 06 · Nuevo / Modificar bungalow · `DialogoBungalow`

- Mismos bloques y botones que 05. *Equipamiento* con casillas (`chkMenaje`, `chkMicroondas`, `chkRopaCama`, `chkToallas`, `chkWifi`, `chkAire`, `chkBarbacoa`, `chkCuna`).
- *Galería:* `lstFotos` (miniaturas) + `btnAnadirFoto`, `btnQuitarFoto`, `btnSubirFoto`, `btnBajarFoto`; texto de ayuda en gris «La primera foto es la principal».
- **RF:** 14, 16.

## 07 · Disponibilidad · `VentanaDisponibilidad`

- **Controles:** `dpDesde`, `dpHasta`, `cmbTipoAlojamiento`, `cmbCategoria`, `btnPeriodoAnterior`, `btnPeriodoSiguiente`, rejilla `dgCalendario` (filas = alojamientos, columnas = días; celda con color + letra L/O/R/X/P), leyenda fija, `btnReservar` (activo solo con días libres consecutivos de un alojamiento).
- **Comportamiento:** «Reservar» abre `DialogoReserva` con alojamiento y fechas precargados.
- **RF:** 18, 19, 20.

## 08 · Principal – Reservas · `PanelReservas`

- **Filtros:** `txtBuscarReserva` (cliente o teléfono), `dpFiltroEntrada`, `cmbFiltroEstadoReserva`, `cmbFiltroAlojamiento`.
- **Tabla:** `dgReservas` — Nº, Cliente, Alojamiento, Entrada, Salida, Personas, Estado.
- **Acciones:** `btnNuevaReserva`, `btnModificarReserva`, `btnAnularReserva`.
- **Ficha:** todos los datos, noches e importe calculados, `btnCheckIn`, `btnVerAlojamiento`.
- **RF:** 21, 23, 25, 26.

## 09 · Nueva / Modificar reserva · `DialogoReserva`

- **Bloques:** *Estancia* (`cmbAlojamiento`*, `dpEntrada`*, `dpSalida`*, `txtHoraEntrada`, `txtHoraSalida`, `txtOcupantes`* con ayuda «Máx. 6 en este alojamiento», `lblNoches`, `lblImporte`, `btnVerDisponibilidad`), *Cliente* (`txtNombre`*, `txtApellidos`*, `txtTelefono`*, `txtTelefono2`, `txtEmail`), *Solicitudes especiales* (`chkCuna`, `chkMovilidad`, `chkMenajeExtra`, `chkMascota`, `txtOtrasSolicitudes`).
- **Validación:** salida > entrada; ocupantes ≤ capacidad; teléfono de 9 cifras; e-mail con formato («p. ej. nombre@correo.es»).
- **Botones:** `btnCancelar` · `btnConfirmarReserva`.
- **RF:** 22, 23, 24.

## 10 · Principal – Monitores · `PanelMonitores`

- **Filtros:** `txtBuscarMonitor`, `cmbFiltroIdioma`, `cmbFiltroDisponibilidad`.
- **Tabla:** `dgMonitores` — Nombre, Especialidad, Idiomas, Disponibilidad.
- **Acciones:** `btnNuevoMonitor`, `btnModificarMonitor`, `btnDarDeBaja`.
- **Ficha:** `imgFotoMonitor`, contacto, idiomas, formación, restricciones, contrato, `lstActividadesMonitor` + `btnVerActividades`.
- **RF:** 27, 30, 31.

## 11 · Nuevo / Modificar monitor · `DialogoMonitor`

- **Controles:** `imgFoto` + `btnCargarFoto` (arriba a la izquierda), `txtNombre`*, `txtApellidos`*, `txtEspecialidad`, `txtTelefono`*, `txtTelefono2`, `txtCorreo`, `txtCorreo2`, idiomas en casillas (`chkEspanol`, `chkIngles`, `chkFrances`, `chkAleman`…), `txtFormacion`, `dpInicioContrato`, `dpFinContrato`, `txtRestricciones` (con texto de ejemplo).
- **RF:** 28, 29.

## 12 · Principal – Actividades · `PanelActividades`

- **Filtros:** `txtBuscarActividad`, `cmbFiltroDestinatarios`, `cmbFiltroArea`, `cmbFiltroMonitor`.
- **Tabla:** `dgActividades` — Nombre, Destinatarios, Horario, Monitor(es), Inscritos/Cupo («6 / 8»), Precio.
- **Ficha:** `imgActividad`, datos, `prgCupo` (barra de progreso con mínimo y máximo), descripción, material, `btnVerMonitor`.
- **RF:** 31, 32, 35.

## 13 · Nueva / Modificar actividad · `DialogoActividad`

- **Controles:** `txtNombre`*, destinatarios `rbAdultos` / `rbNinos` / `rbTodos`, `txtEdadMinima`, `cmbArea`*, `lstMonitores` (selección múltiple), días `chkLunes`…`chkDomingo`, `txtHoraInicio`, `txtHoraFin`, `txtCupoMinimo`, `txtCupoMaximo`, `txtInscritos`, `txtPrecioHora`, `txtPrecioMes` («Deja vacío si no aplica»), `txtDescripcion`, `txtMateriales`.
- **Validación:** cupo máximo ≥ mínimo; inscritos ≤ máximo; hora fin > inicio.
- **RF:** 33, 34.

## 14 · Confirmar eliminación · `VentanaConfirmacion`

- Icono de aviso, título con la acción, mensaje con **qué** se borra y **consecuencias** («La parcela P-12 tiene 2 reservas futuras que quedarán sin alojamiento»).
- Botones: `btnCancelar` (por defecto, con foco) y `btnAccion` en rojo con el verbo («Eliminar», «Anular reserva», «Dar de baja»).
- **RF:** 12, 17, 25, 30, 35, 37.

## 15 · Confirmar salida con cambios sin guardar · `VentanaConfirmacion`

- Mismo formato que 14 con tres botones: «Guardar y salir», «Salir sin guardar», «Cancelar». Se lanza desde `Closing` de la ventana principal y de los diálogos.
- **RF:** 38.

## 16 · Ayuda · `VentanaAyuda`

- `lstTemas` (índice), `txtBuscarAyuda`, contenido con pasos numerados y capturas. Se abre con F1 o `btnAyuda`; si viene de una pestaña, selecciona su tema.
- **RF:** 39.

## 17 · Acerca de · `DialogoAcercaDe`

- Logo, «Camping El Robledal · Gestión» en grande, versión, fecha, autores (Adam Gouram El Bouhali, Celia Núñez Acosta), asignatura y curso; `btnCerrar`.
- **RF:** 40.

## 18 · Preferencias (opcional) · `DialogoPreferencias`

- `cmbFuente`, `sldTamanoFuente`, tema `rbClaro` / `rbOscuro` / `rbAltoContraste`, `chkMostrarTooltips`, panel de vista previa, `btnRestablecer`, `btnCancelar`, `btnAplicar`.
- **Implementación prevista:** diccionarios de recursos por tema (`Temas/Claro.xaml`, `Oscuro.xaml`, `AltoContraste.xaml`) que se cambian en `Application.Current.Resources.MergedDictionaries`.
- **RF:** 42.
