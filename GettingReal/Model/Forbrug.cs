using System;
using System.Collections.Generic;
using System.Linq;
using GettingReal.Services;
using GettingReal.Model;
using System.Text.Json.Serialization;
using System.Diagnostics;

namespace GettingReal.Model
{
    public class Forbrug
    {

        [JsonPropertyName("tokensTilgaengelige")]
        public int TokensTilgaengelige { get; set; }

        [JsonPropertyName("samtalerBrugt")]
        public int SamtalerBrugt { get; set; }

        [JsonPropertyName("kundeId")]
        public string KundeId { get; set; } = string.Empty;

        [JsonPropertyName("samtaler")]
        public List<AISamtale> Samtaler { get; set; }

        public Forbrug()
        {
            Samtaler = new List<AISamtale>();
        }

        public Forbrug(int tokensTilgaengelige, string kundeId)
        {
            TokensTilgaengelige = tokensTilgaengelige;
            KundeId = kundeId;
            Samtaler = new List<AISamtale>();
        }

        public double CalculatePrice()
        {
            double total = 0;
            foreach (var samtale in Samtaler)
            {
                total += samtale.Cost * samtale.TokensBrugt;
            }
            return total;
        }

        public static double HentPrisForKunde(string kundeId)
        {
            var kunde = Lager.Instance.HentKunde(kundeId);
            return kunde?.forbrug?.CalculatePrice() ?? 0;
        }

        public static List<(Kunde kunde, double totalPris)> HentAlleKundersForbrug()
        {
            var kunder = Lager.Instance.HentAlleKunder();
            var resultat = new List<(Kunde, double)>();

            Debug.WriteLine($"Antal kunder hentet: {kunder.Count}");

            foreach (var kunde in kunder)
            {
                Debug.WriteLine($"Kunde: {kunde.Navn} ({kunde.Id})");

                if (kunde.forbrug?.Samtaler != null)
                {
                    Debug.WriteLine($"  Samtaler: {kunde.forbrug.Samtaler.Count}");
                    foreach (var samtale in kunde.forbrug.Samtaler)
                    {
                        Debug.WriteLine($"    Samtale ID: {samtale.Id}, Tokens: {samtale.TokensBrugt}, Cost: {samtale.Cost}, Model: {samtale.IdAiModel}");
                    }
                }
                else
                {
                    Debug.WriteLine("  Samtaler er null eller tom.");
                }

                double totalPris = kunde.forbrug?.CalculatePrice() ?? 0;
                Debug.WriteLine($"  TotalPris beregnet: {totalPris}");

                resultat.Add((kunde, totalPris));
            }

            return resultat;
        }
    }


}