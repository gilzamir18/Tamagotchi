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

namespace Tamagotchi
{

    public record TipoDeMascote 
    {
        [JsonPropertyName("name")]
        public string? Nome { get; set; }
        [JsonPropertyName("url")]
        public string? URL { get; set; }
    }


    public record ListaDeTiposDeMascote
    {
        [JsonPropertyName("count")]
        public int TotalRegistros { get; set; }

        [JsonPropertyName("next")]
        public string? Proximo { get; set; }

        [JsonPropertyName("previous")]
        public string? Anterior { get; set; }

        [JsonPropertyName("results")]
        public List<TipoDeMascote>? Resultados { get; set; }
    }

    public record Formulario
    {
        [JsonPropertyName("name")]
        public string? Nome { get; set; }
        [JsonPropertyName("url")]
        public string? URL { get; set; }
    }

    public record IndiceDeJogo
    {
        [JsonPropertyName("game_index")]
        public int Index { get; set; }
        [JsonPropertyName("version")]
        public Formulario? Formulario { get; set; }
    }

    public record Habilidade
    {
        [JsonPropertyName("ability")]
        public Formulario? HabilidadeInfo { get; set; }
        [JsonPropertyName("is_hidden")]
        public bool Oculto { get; set; }
        [JsonPropertyName("slot")]
        public int Slot { get; set; }
    }

    public record Mascote
    {
        [JsonPropertyName("base_experience")]
        public int BaseExperience { get; set; }
        [JsonPropertyName("height")]
        public int Altura { get; set; }
        [JsonPropertyName("width")]
        public int Largura { get; set; }
        [JsonPropertyName("held_items")]
        public List<string>? ItensGuardados { get; set; }
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("is_default")]
        public bool IsDefault { get; set; }
        [JsonPropertyName("location_area_encounters")]
        public string? LocalizacaoAreasEncontro { get; set; }

        [JsonPropertyName("forms")]
        public List<Formulario>? Formularios { get; set; }

        [JsonPropertyName("abilities")]
        public List<Habilidade>? Habilidades { get; set; }
    }

    public interface DAO
    {
        public List<TipoDeMascote> GetTiposDeMascote(int maxResult=-1);
        public Mascote GetMascote(string name);
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
            
                foreach(var r in results!.Resultados!)
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

        public Mascote GetMascote(string nome)
        {
            var client = new RestClient(options);
            var request = new RestRequest($"/pokemon/{nome}", Method.Get);
            RestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
               var res =  JsonSerializer.Deserialize<Mascote>(response.Content!)!;
                return res!;
            }
            else
            {
                throw new Exception(response.ErrorMessage);
            }
        }
    }
}
