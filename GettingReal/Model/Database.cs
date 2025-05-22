using System.Collections.Generic;
using System.Text.Json.Serialization;
using GettingReal.Model;

public class Database
{
    [JsonPropertyName("kunder")]
    public List<Kunde> kunder { get; set; } = new();

    [JsonPropertyName("aiModels")]
    public List<AiModel> aiModels { get; set; } = new();
}






