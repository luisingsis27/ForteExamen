using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ForteExamen.Clases;
using ForteExamen.Views;
using Xamarin.Forms;

namespace ForteExamen.ViewModels
{
    public class LoginVM : BaseViewModel
    {
        public ICommand LoguearCommand { private set; get; }
        

        public LoginVM()
        {
            LoguearCommand = new Command(IniciarSesion);
        }

        async void IniciarSesion()
        {
            try
            {
                _usuario = "FORTEDEV";
                _password = "Apply2019@pass";
                if (!string.IsNullOrEmpty(_usuario) && !string.IsNullOrEmpty(_password))
                {
                    await Conexion.Instance.Login(_usuario, _password);
                    if (!string.IsNullOrEmpty(Conexion.Instance.Token))
                    {
                        Application.Current.MainPage = new ClientesPage();
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(string.Empty, "Usuario o Contraseña son incorrectos", "Aceptar");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(string.Empty, "Ingresa Usuario y Contraseña", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        #region Propiedades
        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        private string _usuario;

        public string Usuario
        {
            get { return _usuario; }
            set { _usuario = value; OnPropertyChanged(); }
        }
        #endregion


    }
}
