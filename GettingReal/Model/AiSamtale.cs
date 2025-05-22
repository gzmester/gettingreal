namespace GettingReal.Model;
using GettingReal.Services;
using System;

public class AISamtale
{
    public string IdAiModel { get; set; }
    public DateTime Dato { get; set; }
    public int TokensBrugt { get; set; }
    public string Id { get; set; }
    public double Cost { get; set; }

    public AISamtale(string idAiModel, DateTime dato, int tokensBrugt, string id)
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