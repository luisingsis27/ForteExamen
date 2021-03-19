using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ForteExamen.Clases;
using ForteExamen.Models;
using Xamarin.Forms;

namespace ForteExamen.ViewModels
{
    public class CRUDClienteVM : BaseViewModel
    {
        public ICommand CancelarCommand { private set; get; }
        public ICommand AceptarCommand { private set; get; }
        Clientes clienteActualizar { set; get; }
        public Action ActionActualizarClientes;

        public CRUDClienteVM(Clientes cliente)
        {
            clienteActualizar = cliente;
            AceptarCommand = new Command(() => Aceptar());
            CancelarCommand = new Command(() => Cancelar());
            CargarEstatus();
            if (cliente != null)
            {
                _titulo = "Editar Cliente";
                CargarDatosCliente();
            }
            else
            {
                _titulo = "Nuevo Cliente";
            }

        }

        void CargarEstatus()
        {
            ObservableCollection<EstatusPersona> estatusPersonas = new ObservableCollection<EstatusPersona>();
            estatusPersonas.Add(new ViewModels.EstatusPersona() { EstatusName = "ACTIVO", IdEstatus = 1 });
            estatusPersonas.Add(new ViewModels.EstatusPersona() { EstatusName = "INACTIVO", IdEstatus = 0 });

            ListEstatusPesona = estatusPersonas;
        }

        void CargarDatosCliente()
        {
            _nombre = clienteActualizar.nombreCompleto;
            _correo = clienteActualizar.correoElectronico;
            _password = clienteActualizar.Password;
            _fechaNacimiento = clienteActualizar.FechaNacimiento;
            _estatusPersonaItem = clienteActualizar.estatusClienteId == 1? ListEstatusPesona.FirstOrDefault(x => x.IdEstatus == 1) : ListEstatusPesona.FirstOrDefault(x => x.IdEstatus == 0);
            _estatusIndex = clienteActualizar.estatusClienteId == 1 ? 0 : 1;
            _domicilio = clienteActualizar.Domicilio;
            _limiteCredito = clienteActualizar.limiteCredito;
            _telefono = clienteActualizar.TelefonoMovil;
            _rfc = clienteActualizar.RFC;

        }

        void Cancelar()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        async void Aceptar()
        {
            try
            {
                if (clienteActualizar != null)
                {
                    
                    clienteActualizar.nombreCompleto = _nombre;
                    clienteActualizar.correoElectronico = _correo;
                    clienteActualizar.Password = _password;
                    clienteActualizar.FechaNacimiento = _fechaNacimiento;
                    clienteActualizar.estatusClienteId = _estatusPersonaItem.IdEstatus;
                    clienteActualizar.Domicilio = _domicilio;
                    clienteActualizar.limiteCredito = _limiteCredito;
                    clienteActualizar.TelefonoMovil = _telefono;
                    clienteActualizar.RFC = _rfc;

                    if (await Conexion.Instance.ActualizarCliente(clienteActualizar))
                    {

                        await Application.Current.MainPage.DisplayAlert(string.Empty, "Cliente actualizado correctamente", "Aceptar");
                        ActionActualizarClientes?.Invoke();
                        Application.Current.MainPage.Navigation.PopModalAsync();

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(string.Empty, "Ocurrio un error al actualizar", "Aceptar");

                    }

                }
                else
                {

                    var listClientes = await Conexion.Instance.ObtenerClientes();

                    var clienCount = listClientes.Count(x => x.correoElectronico == _correo);

                    if (clienCount == 0)
                    {
                        Clientes clienteNuevo = new Clientes();
                        clienteNuevo.nombreCompleto = _nombre;
                        clienteNuevo.correoElectronico = _correo;
                        clienteNuevo.Password = _password;
                        clienteNuevo.FechaNacimiento = _fechaNacimiento;
                        clienteNuevo.estatusClienteId = _estatusPersonaItem.IdEstatus;
                        clienteNuevo.Domicilio = _domicilio;
                        clienteNuevo.limiteCredito = _limiteCredito;
                        clienteNuevo.TelefonoMovil = _telefono;
                        clienteNuevo.RFC = _rfc;

                        if (await Conexion.Instance.RegistrarCliente(clienteNuevo))
                        {

                            await Application.Current.MainPage.DisplayAlert(string.Empty, "Cliente Guardado Correctamente", "Aceptar");
                            ActionActualizarClientes?.Invoke();
                            Application.Current.MainPage.Navigation.PopModalAsync();

                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert(string.Empty, "Ocurrio un error al guardar", "Aceptar");

                        }

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(string.Empty, "Este cliente ya existe", "Aceptar");

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private ObservableCollection<EstatusPersona> _listEstatusPesona;

        public ObservableCollection<EstatusPersona> ListEstatusPesona
        {
            get { return _listEstatusPesona; }
            set { _listEstatusPesona = value; OnPropertyChanged(); }
        }

        private string _titulo;

        public string Titulo
        {
            get { return _titulo; }
            set { _titulo = value; OnPropertyChanged(); }
        }

        private string _nombre;

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; OnPropertyChanged(); }
        }

        private string _correo;

        public string Correo
        {
            get { return _correo; }
            set { _correo = value; OnPropertyChanged(); }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        private DateTime _fechaNacimiento;

        public DateTime FechaNacimiento
        {
            get { return _fechaNacimiento; }
            set { _fechaNacimiento = value; OnPropertyChanged(); }
        }

        private EstatusPersona _estatusPersonaItem;

        public EstatusPersona EstatusPersonaItem
        {
            get { return
                    _estatusPersonaItem; }
            set {
                _estatusPersonaItem = value;
                OnPropertyChanged(); }
        }

        private int _estatusIndex;

        public int EstatusIndex
        {
            get
            {
                return
                  _estatusIndex;
            }
            set
            {
                _estatusIndex = value;
                OnPropertyChanged();
            }
        }

        private string _domicilio;

        public string Domicilio
        {
            get { return _domicilio; }
            set { _domicilio = value; OnPropertyChanged(); }
        }

        private double _limiteCredito;

        public double LimiteCredito
        {
            get { return _limiteCredito; }
            set { _limiteCredito = value; OnPropertyChanged(); }
        }

        private string _rfc;

        public string RFC
        {
            get { return _rfc; }
            set { _rfc = value; OnPropertyChanged(); }
        }

        private string _telefono;

        public string Telefono
        {
            get { return _telefono; }
            set { _telefono = value; OnPropertyChanged(); }
        }

        

    }
    public class EstatusPersona
    {
        public int IdEstatus { get; set; }
        public string EstatusName { get; set; }
    }
}
