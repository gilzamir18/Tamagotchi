using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using RestSharp;
using Tamagotchi.Model;

namespace Tamagotchi.Service
{
    public interface DAO
    {
        public List<TipoDeMascote> GetTiposDeMascote(int maxResult = -1);
        public Mascote? GetMascote(string name);
    }

    public class RestAPIDAO : DAO
    {
        private RestClientOptions options;

        private static DAO? _instance = null;

        private RestAPIDAO()
        {
            options = new RestClientOptions("https://pokeapi.co/api/v2/")
            {
                MaxTimeout = -1,
            };
        }

        public static DAO GetInstance()
        {
            if (_instance == null)
            {
                _instance = new RestAPIDAO();
            }
            return _instance;
        }


        private bool GetTiposDeMascoteHelper(string url, out ListaDeTiposDeMascote result)
        {
            var client = new RestClient(options);
            var request = new RestRequest(url, Method.Get);
            RestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result = JsonSerializer.Deserialize<ListaDeTiposDeMascote>(response.Content!)!;
                return true;
            }
            else
            {
                result = new ListaDeTiposDeMascote();
                result.Resultados = new List<TipoDeMascote>();
                return false;
            }
        }

        public List<TipoDeMascote> GetTiposDeMascote(int maxResult = -1)
        {
            var client = new RestClient(options);
            var request = new RestRequest("/pokemon", Method.Get);
            RestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<TipoDeMascote> data = new List<TipoDeMascote>();

                var results = JsonSerializer.Deserialize<ListaDeTiposDeMascote>(response.Content!);

                foreach (var r in results!.Resultados!)
                {
                    data.Add(r);
                    if (maxResult > 0 && data.Count >= maxResult)
                    {
                        break;
                    }
                }

                if (maxResult <= 0 && data.Count < maxResult)
                {
                    ListaDeTiposDeMascote result;

                    while (results.Proximo != null && GetTiposDeMascoteHelper(results.Proximo!, out result))
                    {
                        data.AddRange(result.Resultados!);
                        if (maxResult > 0 && data.Count >= maxResult)
                        {
                            break;
                        }
                    }
                }

                return data;
            }
            else
            {
                throw new Exception(response.ErrorMessage);
            }
        }

        public Mascote? GetMascote(string nome)
        {
            var client = new RestClient(options);
            var request = new RestRequest($"/pokemon/{nome}", Method.Get);
            RestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var res = JsonSerializer.Deserialize<Mascote>(response.Content!)!;
                return res!;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                throw new Exception(response.ErrorMessage);
            }
        }
    }
}
