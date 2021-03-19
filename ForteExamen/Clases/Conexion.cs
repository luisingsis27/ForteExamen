using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ForteExamen.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace ForteExamen.Clases
{
    public class Conexion
    {
        public string Token { get; set; }

        private static Conexion instance;

		public static Conexion Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Conexion();
				}
				return instance;
			}
		}

		public async Task<bool> Login(string Usuario, string Password)
		{

				try
				{
					HttpClientHandler handler = new HttpClientHandler();

					using (var client = new HttpClient(handler))
					{
						client.BaseAddress = new Uri("http://apps01.forteinnovation.mx:8590/");
						HttpResponseMessage response = await client.PostAsync("/api/auth/login",
							new StringContent("{ \"usuario\" : \""+ Usuario+ "\", \"password\" : \""+Password+ "\"}",
							Encoding.UTF8, "application/json"));

						string resultContents = await response.Content.ReadAsStringAsync();
						
						Debug.WriteLine(resultContents);


						if (response.IsSuccessStatusCode)
						{
							JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(resultContents);
							Token = jObject.SelectToken("data").SelectToken("token")?.ToString();

							return true;
						}
					return false;
					}


				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
					return false;
				}
		}

		public async Task<ObservableCollection<Clientes>> ObtenerClientes()
        {
			return await Task.Run(async () =>
			{
				try
				{

					ObservableCollection<Clientes> listClientes = new ObservableCollection<Clientes>();
					HttpClientHandler handler = new HttpClientHandler();

					using (var client = new HttpClient(handler))
					{
						var authHeader = new AuthenticationHeaderValue("bearer", Token);

						client.BaseAddress = new Uri("http://apps01.forteinnovation.mx:8590/");
						client.DefaultRequestHeaders.Authorization = authHeader;
						HttpResponseMessage response = await client.GetAsync("/api/clientes");


						if (response.IsSuccessStatusCode)
						{
							string resultContents = await response.Content.ReadAsStringAsync();
							Debug.WriteLine(resultContents);
							JObject jObject = JObject.Parse(resultContents);
							var data = jObject.SelectToken("data")?.ToString();

							listClientes = JsonConvert.DeserializeObject<ObservableCollection<Clientes>>(data);

							return listClientes;
						}
                        else
                        {
							return null;
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					return null;
				}
			});
        }

		public async Task<bool> RegistrarCliente(Clientes cliente)
		{
			return await Task.Run(async () =>
			{
				try
				{

					HttpClientHandler handler = new HttpClientHandler();

					using (var client = new HttpClient(handler))
					{
						var authHeader = new AuthenticationHeaderValue("bearer", Token);

						client.BaseAddress = new Uri("http://apps01.forteinnovation.mx:8590/");
						client.DefaultRequestHeaders.Authorization = authHeader;


						var jsonCliente = JsonConvert.SerializeObject(cliente);
						HttpResponseMessage response = await client.PostAsync("/api/cliente",new StringContent(jsonCliente,
							Encoding.UTF8, "application/json"));


						if (response.IsSuccessStatusCode)
						{
							return true;
						}
						else
						{
							return false;
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					return false;
				}
			});
		}

		public async Task<bool> ActualizarCliente(Clientes cliente)
		{
			return await Task.Run(async () =>
			{
				try
				{

					HttpClientHandler handler = new HttpClientHandler();

					using (var client = new HttpClient(handler))
					{
						var authHeader = new AuthenticationHeaderValue("bearer", Token);

						client.BaseAddress = new Uri("http://apps01.forteinnovation.mx:8590/");
						client.DefaultRequestHeaders.Authorization = authHeader;


						var jsonCliente = JsonConvert.SerializeObject(cliente);
						HttpResponseMessage response = await client.PutAsync("/api/cliente/"+cliente.clienteId+"", new StringContent(jsonCliente,
							Encoding.UTF8, "application/json"));


						if (response.IsSuccessStatusCode)
						{
							return true;
						}
						else
						{
							return false;
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					return false;
				}
			});
		}

		public async Task<bool> EliminarCliente(Clientes cliente)
		{
			return await Task.Run(async () =>
			{
				try
				{

					HttpClientHandler handler = new HttpClientHandler();

					using (var client = new HttpClient(handler))
					{
						var authHeader = new AuthenticationHeaderValue("bearer", Token);

						client.BaseAddress = new Uri("http://apps01.forteinnovation.mx:8590/");
						client.DefaultRequestHeaders.Authorization = authHeader;


						var jsonCliente = JsonConvert.SerializeObject(cliente);
						HttpResponseMessage response = await client.DeleteAsync("/api/cliente/" + cliente.clienteId + "");


						if (response.IsSuccessStatusCode)
						{
							return true;
						}
						else
						{
							return false;
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					return false;
				}
			});
		}
	}
}
