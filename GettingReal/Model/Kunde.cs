using System;
using System.Text.Json.Serialization;
using GettingReal.Model;

public class Kunde
{
    [JsonPropertyName("navn")]
    public string Navn { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("pris")]
    public double Pris { get; set; }

    [JsonPropertyName("forbrug")]
    public Forbrug forbrug { get; set; }

    public Kunde() { }



    public Kunde(string navn, string id, double pris, Forbrug forbrug)
	{
		Navn = navn;
		Id = id;
		Pris = pris;
		this.forbrug = forbrug;
	}

	public Kunde(string navn, string id, double pris, int tokensTilgængelige)
	{
		Navn = navn;
		Id = id;
		Pris = pris;
		this.forbrug = new Forbrug(tokensTilgængelige,Id);
	}

	public string GetCostumer()
	{
		return $"Name: {Navn} Id: {Id} Price: {Pris}";
	}
    public override string ToString()
    {
        return $"Name: {Navn} Id: {Id} Price: {Pris}";
    }
}
