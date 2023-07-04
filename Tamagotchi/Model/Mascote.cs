using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tamagotchi.Model
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
}
