using System.IO;

public class Database
{
    public List<Kunde> kunder { get; set; }
    public List<AiModel> aiModels { get; set; }
}

public class Kunde
{
    public string navn { get; set; }
    public string id { get; set; }
    public double pris { get; set; }
    public Forbrug forbrug { get; set; }
}

public class Forbrug
{
    public int tokensTilgaengelige { get; set; }
    public int samtalerBrugt { get; set; }
    public string kundeId { get; set; }
    public List<Samtale> samtaler { get; set; }
}

public class Samtale
{
    public int id { get; set; }
    public DateTime dato { get; set; }
    public int tokensBrugt { get; set; }
    public double cost { get; set; }
    public string IdaiModel { get; set; }
}

public class AiModel
{
    public string aiModel { get; set; }
    public string id { get; set; }
    public double prisPerToken { get; set; }
}
