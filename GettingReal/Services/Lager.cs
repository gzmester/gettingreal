using System.Diagnostics;
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
        string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Services", "lager.json");
        Debug.WriteLine($"Forsøger at indlæse fra: {jsonPath}");
        try
        {
            if (File.Exists(jsonPath))
            {
                string json = File.ReadAllText(jsonPath);
                Debug.WriteLine($"JSON-indhold: {json}");
                var db = JsonSerializer.Deserialize<Database>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (db == null)
                {
                    _database = new Database();
                    Debug.WriteLine("Database kunne ikke indlæses. Opretter ny database.");
                }
                else
                {
                    _database = db;
                    Debug.WriteLine($"Database indlæst. Antal kunder: {_database.kunder?.Count}, Antal AI-modeller: {_database.aiModels?.Count}");
                }
            }
            else
            {
                _database = new Database { kunder = new List<Kunde>(), aiModels = new List<AiModel>() };
                Debug.WriteLine("lager.json ikke fundet. Opretter ny database.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Fejl under indlæsning af database: {ex.Message}");
            _database = new Database();
        }
    }

    public List<Kunde> HentAlleKunder() => _database.kunder ?? new List<Kunde>();
    public Kunde HentKunde(string id) => _database.kunder?.FirstOrDefault(k => k.Id == id);
    public List<AiModel> HentAlleModeller() => _database.aiModels ?? new List<AiModel>();
    public AiModel HentModel(string modelId) => _database.aiModels?.FirstOrDefault(m => m.id == modelId);
}
