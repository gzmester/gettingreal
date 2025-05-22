using System.IO;
using System.Text.Json;
using GettingReal.Model;

namespace GettingReal.Services;

public class Lager
{
    private static Lager? _instance;
    private Database _database = new Database();

    private Lager()
    {
        LoadDatabase();
    }

    public static Lager Instance => _instance ??= new Lager();

    private void LoadDatabase()
    {
        string jsonPath = "lager.json";
        if (File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);
            var db = JsonSerializer.Deserialize<Database>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            // Fallback hvis deserialisering fejler eller lister er null
            if (db == null)
            {
                _database = new Database();
                Console.WriteLine("Database kunne ikke indlæses, opretter ny database.");
            }
            else
            {
                db.kunder ??= new List<Kunde>();
                db.aiModels ??= new List<AiModel>();
                _database = db;
                Console.WriteLine("Database indlæst.");
            }
        }
        else
        {
            _database = new Database { kunder = new List<Kunde>(), aiModels = new List<AiModel>() };
        }
    }

    public List<Kunde> HentAlleKunder() => _database.kunder ?? new List<Kunde>();
    public Kunde HentKunde(string id) => _database.kunder?.FirstOrDefault(k => k.Id == id);
    public List<AiModel> HentAlleModeller() => _database.aiModels ?? new List<AiModel>();
    public AiModel HentModel(string modelId) => _database.aiModels?.FirstOrDefault(m => m.id == modelId);
}
