using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using RestSharp;
using Tamaguria.Model;

namespace Tamaguria.Service
{


    public record MascoteURLData
    {
        [JsonPropertyName("name")]
        public string? Nome { get; set; }
        [JsonPropertyName("url")]
        public string? URL { get; set; }
    }

    public record ListaDeMascoteLocation
    {
        [JsonPropertyName("count")]
        public int TotalRegistros { get; set; }

        [JsonPropertyName("next")]
        public string? Proximo { get; set; }

        [JsonPropertyName("previous")]
        public string? Anterior { get; set; }

        [JsonPropertyName("results")]
        public List<MascoteURLData>? Resultados { get; set; }
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

    public record HabilidadeData
    {
        [JsonPropertyName("ability")]
        public Formulario? HabilidadeInfo { get; set; }
        [JsonPropertyName("is_hidden")]
        public bool Oculto { get; set; }
        [JsonPropertyName("slot")]
        public int Slot { get; set; }
    }

    public record MascoteData
    {
        [JsonIgnore]
        public string? Nome { get; set; }

        [JsonPropertyName("base_experience")]
        public int BaseExperience { get; set; }
        [JsonPropertyName("height")]
        public int Altura { get; set; }
        [JsonPropertyName("width")]
        public int Largura { get; set; }

        [JsonPropertyName("weight")]
        public int Peso { get; set; }

        //[JsonPropertyName("held_items")]
        //public List<string>? ItensGuardados { get; set; }
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("is_default")]
        public bool IsDefault { get; set; }
        [JsonPropertyName("location_area_encounters")]
        public string? LocalizacaoAreasEncontro { get; set; }

        [JsonPropertyName("forms")]
        public List<Formulario>? Formularios { get; set; }

        [JsonPropertyName("abilities")]
        public List<HabilidadeData>? Habilidades { get; set; }
    }

    public interface DAO
    {
        public List<MascoteLocation> GetMascoteLocationList(int maxResult = -1);
        public Mascote? GetMascote(string name);
    }

    public class RestAPIDAO : DAO
    {
        private RestClientOptions options;
        private JsonSerializerOptions jsonSerializerOptions;
        
        private static DAO? _instance = null;
        private static IMapper? mascoteMapper = null;
        private static IMapper? locationMapper = null;
        private static IMapper? habilidadeMapper = null;



        private RestAPIDAO()
        {
            options = new RestClientOptions("https://pokeapi.co/api/v2/")
            {
                MaxTimeout = -1,
            };

            jsonSerializerOptions = new JsonSerializerOptions 
            { 
            };
        }

        public static DAO GetInstance()
        {
            if (_instance == null)
            {

                habilidadeMapper = new MapperConfiguration(
                    cfg => cfg.CreateMap<HabilidadeData, Habilidade>()
                    .ForMember(dest => dest.Nome, o => o.MapFrom(src => src.HabilidadeInfo!.Nome))
                    .ForMember(dest => dest.URL, o => o.MapFrom(src => src.HabilidadeInfo!.URL))
                    ).CreateMapper();

                mascoteMapper = new MapperConfiguration(cfg => cfg.CreateMap<MascoteData, Mascote>()
                .ForMember(dest => dest.Habilidades, o => o.MapFrom(src => src.Habilidades!.Select(h =>  habilidadeMapper.Map<Habilidade>(h) )  ))
                ).CreateMapper();


                locationMapper = new MapperConfiguration(cfg => cfg.CreateMap<MascoteURLData, MascoteLocation>()).CreateMapper();
                _instance = new RestAPIDAO();
            }
            return _instance;
        }


        private bool GetMascoteURLHelper(string url, out ListaDeMascoteLocation result)
        {
            var client = new RestClient(options);
            var request = new RestRequest(url, Method.Get);
            RestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result = JsonSerializer.Deserialize<ListaDeMascoteLocation>(response.Content!)!;
                return true;
            }
            else
            {
                result = new ListaDeMascoteLocation();
                result.Resultados = new List<MascoteURLData>();
                return false;
            }
        }

        public List<MascoteLocation> GetMascoteLocationList(int maxResult = -1)
        {
            var client = new RestClient(options);
            var request = new RestRequest("/pokemon", Method.Get);
            RestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<MascoteLocation> data = new List<MascoteLocation>();

                var results = JsonSerializer.Deserialize<ListaDeMascoteLocation>(response.Content!);

                foreach (var r in results!.Resultados!)
                {
                    //data.Add(new MascoteLocation(r.Nome!, r.URL!));
                    data.Add(locationMapper!.Map<MascoteLocation>(r));
                    if (maxResult > 0 && data.Count >= maxResult)
                    {
                        break;
                    }
                }

                if (maxResult <= 0 && data.Count < maxResult)
                {
                    ListaDeMascoteLocation result;

                    while (results.Proximo != null && GetMascoteURLHelper(results.Proximo!, out result))
                    {


                        foreach (MascoteURLData urlData in result.Resultados!)
                        {
                            //data.Add(new MascoteLocation(urlData.Nome!, urlData.URL!));
                            data.Add(locationMapper!.Map<MascoteLocation>(urlData));
                        }
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
                var res = JsonSerializer.Deserialize<MascoteData>(response.Content!, jsonSerializerOptions)!;
                res!.Nome = nome;
                //Mascote mas = new Mascote(res.Nome!, res.Peso, res.Altura, res.Largura);
                Mascote mas = mascoteMapper!.Map<Mascote>(res);
                /*foreach (HabilidadeData habData in res.Habilidades!)
                {
                    Habilidade hab = new Habilidade(habData.HabilidadeInfo!.Nome!);
                    hab.Oculta = habData.Oculto;
                    hab.Slot = habData.Slot;
                    hab.URL = habData.HabilidadeInfo!.URL!;
                    mas.AdicionarHabilidade(hab);
                }*/
                return mas;
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
