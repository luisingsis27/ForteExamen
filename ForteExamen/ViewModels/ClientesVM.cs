using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ForteExamen.Clases;
using ForteExamen.Models;
using ForteExamen.Views;
using Xamarin.Forms;

namespace ForteExamen.ViewModels
{
    public class ClientesVM : BaseViewModel
    {
        public ICommand AgregarClienteCommand { private set; get; }
        public ICommand EliminarClienteCommand { set; get; }
        
    

        ClienteCRUDPage clienteCRUDPage;

        public ClientesVM()
        {
            AgregarClienteCommand = new Command(async()=> {  NuevoCliente(); } );
            EliminarClienteCommand = new Command(ElimnarPrueba);
            _obclientes = new ObservableCollection<Clientes>();
            CargarClientes();

          
        }

        void CargarClientes()
        {
            try
            {
                Task.Run(async() => { 
                    OBClientes = await Conexion.Instance.ObtenerClientes();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        async Task ActualizarCliente()
        {
            try
            {
                clienteCRUDPage = new ClienteCRUDPage();
                CRUDClienteVM cRUDClienteVM = new CRUDClienteVM(SelectedCliente);
                cRUDClienteVM.ActionActualizarClientes += (() => {
                    CargarClientes();
                });
                clienteCRUDPage.BindingContext = cRUDClienteVM;
                await Application.Current.MainPage.Navigation.PushModalAsync(clienteCRUDPage).ContinueWith((obj)=> {
                    SelectedCliente = null;
                    _selectedCliente = null;
                });
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        async Task NuevoCliente()
        {
            try
            {
                clienteCRUDPage = new ClienteCRUDPage();
                CRUDClienteVM cRUDClienteVM = new CRUDClienteVM(null);
                cRUDClienteVM.ActionActualizarClientes += (() => {
                    CargarClientes();
                });
                clienteCRUDPage.BindingContext = cRUDClienteVM;
                await Application.Current.MainPage.Navigation.PushModalAsync(clienteCRUDPage).ContinueWith((obj) => {
                    SelectedCliente = null;
                    _selectedCliente = null;
                }); 

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        async void ElimnarPrueba(object obj)
        {
            
            try
            {
                var elimnado = await Task<bool>.Run(async () =>
                {
                    var content = obj as Clientes;
                    if (content != null)
                    {
                        return await Conexion.Instance.EliminarCliente(content);

                    }
                    else
                    {
                        return false;
                    }
                });

                if(elimnado)
                {
                    await Application.Current.MainPage.DisplayAlert(string.Empty, "Cliente eliminado correctamente", "Aceptar");
                    CargarClientes();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    

        private Clientes _selectedCliente;

        public Clientes SelectedCliente
        {
            get {
                return _selectedCliente; }
            set {
                
                _selectedCliente = value;
                OnPropertyChanged();
                if (_selectedCliente != null)
                {
                    ActualizarCliente();
                }

            }
        }

        private ObservableCollection<Clientes> _obclientes;

        public ObservableCollection<Clientes> OBClientes
        {
            get {
                return _obclientes;
            }
            set {
                _obclientes = value;
                OnPropertyChanged();
            }
        }


    }
}
