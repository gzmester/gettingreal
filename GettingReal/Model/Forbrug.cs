using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GettingReal.Model
{
    public class Forbrug
    {
        public int TokensTilgængelige {  get; set; }
        public int SamtalerBrugt {  get; set; }
        public string KundeId {  get; set; }
        public List<AISamtale> Samtaler {  get; set; }

        public Forbrug(int tokensTilgængelige, string kundeId) 
        { 
            TokensTilgængelige = tokensTilgængelige;
            KundeId = kundeId;
            Samtaler = new List<AISamtale>();
            if (Samtaler != null)
            {
                SamtalerBrugt = Samtaler.Count;
            }
        }

        public void addSamtale(AISamtale samtale)
        {
            Samtaler.Add(samtale);
        }
        public void createSamtale(string idAiModel, DateTime dato, int tokensBrugt, string id)
        {
            Samtaler.Add(new AISamtale(idAiModel,dato, tokensBrugt, id));
        }

        public double CalculatePriceForCustomer()
        {
            double cost = 0;
            foreach (AISamtale item in Samtaler)
            {
                cost += item.Cost;
            }
            return cost;
        }

    }
}
