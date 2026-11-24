using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CampingRobledal.Datos;
using CampingRobledal.Utilidades;

namespace CampingRobledal
{
    public partial class VentanaLogin : Window
    {
        private bool entrando = false;

        public VentanaLogin()
        {
            InitializeComponent();
            txtUsuario.Focus();
            // Si se sale del login sin entrar, la ayuda abierta no debe mantener viva la aplicación
            Closed += (o, e) => { if (!entrando) VentanaAyuda.CerrarSiAbierta(); };
        }

        /// <summary>Contraseña escrita, esté visible o no.</summary>
        private string ContrasenaEscrita
        {
            get { return chkMostrarContrasena.IsChecked == true ? txtContrasenaVisible.Text : passContrasena.Password; }
        }

        private void LimpiarErrores()
        {
            Validacion.MarcarCampo(txtUsuario, lblErrorUsuario, true, "");
            Validacion.MarcarCampo(passContrasena, lblErrorContrasena, true, "");
            Validacion.MarcarCampo(txtContrasenaVisible, lblErrorContrasena, true, "");
        }

        private void txtUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                CampoContrasenaActivo().Focus();
            }
        }

        private Control CampoContrasenaActivo()
        {
            if (chkMostrarContrasena.IsChecked == true) return txtContrasenaVisible;
            return passContrasena;
        }

        private void Campo_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Al corregir el usuario se quita el error anterior
            if (lblErrorUsuario != null && lblErrorUsuario.Visibility == Visibility.Visible)
                Validacion.MarcarCampo(txtUsuario, lblErrorUsuario, true, "");
        }

        private void passContrasena_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (chkMostrarContrasena.IsChecked != true) txtContrasenaVisible.Text = passContrasena.Password;
            QuitarErrorContrasena();
        }

        private void txtContrasenaVisible_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (chkMostrarContrasena.IsChecked == true) passContrasena.Password = txtContrasenaVisible.Text;
            QuitarErrorContrasena();
        }

        private void QuitarErrorContrasena()
        {
            if (lblErrorContrasena != null && lblErrorContrasena.Visibility == Visibility.Visible)
            {
                Validacion.MarcarCampo(passContrasena, lblErrorContrasena, true, "");
                Validacion.MarcarCampo(txtContrasenaVisible, lblErrorContrasena, true, "");
            }
        }

        private void chkMostrarContrasena_Cambiado(object sender, RoutedEventArgs e)
        {
            bool visible = chkMostrarContrasena.IsChecked == true;
            txtContrasenaVisible.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            passContrasena.Visibility = visible ? Visibility.Collapsed : Visibility.Visible;
            CampoContrasenaActivo().Focus();
        }

        private void btnEntrar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarErrores();
            string usuario = txtUsuario.Text.Trim();

            if (usuario.Length == 0)
            {
                Validacion.MarcarCampo(txtUsuario, lblErrorUsuario, false, "Escribe tu usuario, p. ej. adam");
                txtUsuario.Focus();
                return;
            }

            Gestor gestor = DatosPrueba.BuscarGestor(usuario);
            if (gestor == null)
            {
                Validacion.MarcarCampo(txtUsuario, lblErrorUsuario, false,
                    "No existe ningún gestor con ese usuario. Revisa que esté bien escrito, p. ej. adam");
                txtUsuario.SelectAll();
                txtUsuario.Focus();
                return;
            }

            if (ContrasenaEscrita != gestor.Contrasena)
            {
                // Se conserva el usuario, se vacía la contraseña y recibe el foco (RF-02)
                passContrasena.Clear();
                txtContrasenaVisible.Clear();
                Control campo = CampoContrasenaActivo();
                Validacion.MarcarCampo(campo, lblErrorContrasena, false,
                    "La contraseña no es correcta. Inténtalo de nuevo");
                campo.Focus();
                return;
            }

            // La cabecera lee el acceso anterior al crearse; después se registra el actual
            VentanaPrincipal principal = new VentanaPrincipal(gestor);
            gestor.UltimoAcceso = DateTime.Now;
            principal.Show();
            entrando = true;
            Close();
        }

        private void btnAyuda_Click(object sender, RoutedEventArgs e)
        {
            VentanaAyuda.Abrir("Iniciar sesión");
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
