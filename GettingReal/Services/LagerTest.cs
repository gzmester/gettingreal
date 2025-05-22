using System;
using GettingReal.Services;
using GettingReal.Model;

namespace GettingReal.Services
{
    public class LagerTest
    {
        public static void PrintLagerData()
        {
            Console.WriteLine("Alle kunder:");
            foreach (var kunde in Lager.Instance.HentAlleKunder())
            {
                Console.WriteLine($"Navn: {kunde.Navn}, Id: {kunde.Id}, Pris: {kunde.Pris}");
            }

            Console.WriteLine("\nAlle AI modeller:");
            foreach (var model in Lager.Instance.HentAlleModeller())
            {
                Console.WriteLine($"Model: {model.aiModel}, Id: {model.id}, Pris pr. token: {model.prisPerToken}");
            }
        }
    }
}
