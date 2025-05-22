using System;
using System.Collections.Generic;
using System.Linq;
using GettingReal.Services;
using GettingReal.Model;

namespace GettingReal.Model
{
    public class Forbrug
    {
        public int TokensTilgaengelige { get; set; }
        public int SamtalerBrugt { get; set; }
        public string KundeId { get; set; } = string.Empty;
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

            foreach (var kunde in kunder)
            {
                double totalPris = kunde.forbrug?.CalculatePrice() ?? 0;
                resultat.Add((kunde, totalPris));
            }

            return resultat;
        }
    }


}