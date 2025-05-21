using System;

public class Kunde
{
	public string Navn { get; set; }
	public string Id { get; set; }

	public double Pris { get; set; }

	public Forbrug forbrug { get; set; }



	public Kunde(string navn, string id, double pris, Forbrug forbrug)
	{
		Navn = navn;
		Id = id;
		Pris = pris;
		this.forbrug = forbrug;
	}

	public Kunde(string navn, string id, double pris)
	{
		Navn = navn;
		Id = id;
		Pris = pris;
		this.forbrug = new Forbrug(Id);
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
