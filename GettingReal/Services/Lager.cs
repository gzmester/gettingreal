using System.IO;
using System.Text.Json;

public class Lager
{
    private static Lager _instance;
    private Database _database;

    private Lager()
    {
        LoadDatabase();
    }

    public static Lager Instance => _instance ??= new Lager();

    private void LoadDatabase()
    {
        string jsonPath = "../lager.json";
        if (File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);
            _database = JsonSerializer.Deserialize<Database>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        else
        {
            _database = new Database { kunder = new List<Kunde>(), aiModels = new List<AiModel>() };
        }
    }

    public List<Kunde> HentAlleKunder() => _database.kunder;
    public Kunde HentKunde(string id) => _database.kunder.FirstOrDefault(k => k.id == id);
    public List<AiModel> HentAlleModeller() => _database.aiModels;
    public AiModel HentModel(string modelId) => _database.aiModels.FirstOrDefault(m => m.id == modelId);
}
