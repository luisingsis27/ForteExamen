using System;
using System.Windows.Input;

namespace ForteExamen.Models
{
    public class Clientes
    {
        public int clienteId { get; set; }
        public string nombreCompleto { get; set; }
        public string correoElectronico { get; set; }
        public int edad { get; set; }
        public double limiteCredito { get; set; }
        public int estatusClienteId { get; set; }
        public string estatusCliente { get; set; }

        public string Password { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string TelefonoMovil { get; set; }
        public string Domicilio { get; set; }
        public string RFC { get; set; }


    }
}
