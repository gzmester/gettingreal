using System;

public class Kunde
{
	public string Navn { get; private set; }
	public string Id { get; private set; }

	public double Pris { get; set; }

	public Forbrug forbrug { get; private set; }



	public Kunde(string navn, string id, double pris, Forbrug forbrug)
	{
		Navn = navn;
		Id = id;
		Pris = pris;
		this.forbrug = forbrug;
	}

	public Kunde GetCostumer()
	{
		return this;
	}
    public override string ToString()
    {
        return $"Name: {Navn} Id: {Id} Price: {Pris}";
    }
}
