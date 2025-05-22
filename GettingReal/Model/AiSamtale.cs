using GettingReal.Services;
using System;
using System.Text.Json.Serialization;
namespace GettingReal.Model;

public class AISamtale
{

    [JsonPropertyName("IdaiModel")]
    public string IdAiModel { get; set; }

    [JsonPropertyName("dato")]
    public DateTime Dato { get; set; }

    [JsonPropertyName("tokensBrugt")]
    public int TokensBrugt { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("cost")]
    public double Cost { get; set; }

    public AISamtale() { }

    public AISamtale(string idAiModel, DateTime dato, int tokensBrugt, int id)
    {
        IdAiModel = idAiModel;
        Dato = dato;
        TokensBrugt = tokensBrugt;
        Id = id;
        CalculatePrice();
    }

    public AiModel GetAiModel()
    {
        return Lager.Instance.HentModel(IdAiModel);
    }

    public double CalculatePrice()
    {
        var model = GetAiModel();
        if (model != null)
        {
            Cost = TokensBrugt * model.prisPerToken;
        }
        return Cost;
    }

    public override string ToString()
    {
        return $"AI Model: {GetAiModel()?.aiModel ?? "ukendt"}, Tokens: {TokensBrugt}, Pris: {Cost:0.00}";
    }
}