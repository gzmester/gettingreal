namespace GettingReal.Model;
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
        Cost = 0.0;
    }

    public double CalculatePrice(double prisPerToken)
    {
        Cost = TokensBrugt * prisPerToken;
        return Cost;
    }

    public override string ToString()
    {
        return $"AI Model ID: {IdAiModel}, Date: {Dato}, Tokens Used: {TokensBrugt}, ID: {Id}, Cost: {Cost}";
    }
}
