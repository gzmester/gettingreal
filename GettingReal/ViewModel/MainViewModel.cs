using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GettingReal.Model;
using GettingReal.Services;
using System.Linq; // Tilføj for LINQ
using System.Collections.Generic; // Tilføj for List

namespace GettingReal.ViewModel
{
    public enum SortKundeCriteria
    {
        Navn,
        TokensTilgaengelige,
        TotalPris,
        AntalSamtaler
    }

    public enum SortAiModelCriteria
    {
        Navn,
        PrisPerToken
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Kunde> _kunder;
        public ObservableCollection<Kunde> Kunder
        {
            get => _kunder;
            set
            {
                _kunder = value;
                OnPropertyChanged(nameof(Kunder));
            }
        }

        private ObservableCollection<AiModel> _aiModels;
        public ObservableCollection<AiModel> AiModels
        {
            get => _aiModels;
            set
            {
                _aiModels = value;
                OnPropertyChanged(nameof(AiModels));
            }
        }

        private Kunde? _selectedKunde;
        public Kunde? SelectedKunde
        {
            get => _selectedKunde;
            set
            {
                _selectedKunde = value;
                OnPropertyChanged(nameof(SelectedKunde));
            }
        }

        private AiModel? _selectedAiModel;
        public AiModel? SelectedAiModel
        {
            get => _selectedAiModel;
            set
            {
                _selectedAiModel = value;
                OnPropertyChanged(nameof(SelectedAiModel));
            }
        }

		public MainViewModel()
		{
			// Initial load
            var alleKunder = Lager.Instance.HentAlleKunder();
            var alleModeller = Lager.Instance.HentAlleModeller();
			Kunder = new ObservableCollection<Kunde>(alleKunder);
			AiModels = new ObservableCollection<AiModel>(alleModeller);
			Console.WriteLine("MainViewModel initialized.");
        }

        public void SortKunder(SortKundeCriteria criteria, SortDirection direction)
        {
            List<Kunde> sortedList;
            switch (criteria)
            {
                case SortKundeCriteria.TokensTilgaengelige:
                    sortedList = direction == SortDirection.Ascending ?
                        Kunder.OrderBy(k => k.forbrug?.TokensTilgaengelige ?? 0).ToList() :
                        Kunder.OrderByDescending(k => k.forbrug?.TokensTilgaengelige ?? 0).ToList();
                    break;
                case SortKundeCriteria.TotalPris:
                    sortedList = direction == SortDirection.Ascending ?
                        Kunder.OrderBy(k => k.forbrug?.CalculatePrice() ?? 0).ToList() :
                        Kunder.OrderByDescending(k => k.forbrug?.CalculatePrice() ?? 0).ToList();
                    break;
                case SortKundeCriteria.AntalSamtaler:
                    sortedList = direction == SortDirection.Ascending ?
                        Kunder.OrderBy(k => k.forbrug?.SamtalerBrugt ?? 0).ToList() :
                        Kunder.OrderByDescending(k => k.forbrug?.SamtalerBrugt ?? 0).ToList();
                    break;
                case SortKundeCriteria.Navn:
                default:
                    sortedList = direction == SortDirection.Ascending ?
                        Kunder.OrderBy(k => k.Navn).ToList() :
                        Kunder.OrderByDescending(k => k.Navn).ToList();
                    break;
            }
            Kunder.Clear();
            foreach (var kunde in sortedList)
            {
                Kunder.Add(kunde);
            }
        }

        public void SortAiModels(SortAiModelCriteria criteria, SortDirection direction)
        {
            List<AiModel> sortedList;
            switch (criteria)
            {
                case SortAiModelCriteria.PrisPerToken:
                    sortedList = direction == SortDirection.Ascending ?
                        AiModels.OrderBy(m => m.prisPerToken).ToList() :
                        AiModels.OrderByDescending(m => m.prisPerToken).ToList();
                    break;
                case SortAiModelCriteria.Navn:
                default:
                    sortedList = direction == SortDirection.Ascending ?
                        AiModels.OrderBy(m => m.aiModel).ToList() : // Antager at 'aiModel' property indeholder navnet
                        AiModels.OrderByDescending(m => m.aiModel).ToList();
                    break;
            }
            AiModels.Clear();
            foreach (var model in sortedList)
            {
                AiModels.Add(model);
            }
        }

        public Kunde? FindKundeById(string id)
        {
            return Kunder.FirstOrDefault(k => k.Id == id);
        }

        public double? GetForbrugForKunde(string id)
        {
            return Forbrug.HentPrisForKunde(id);
        }

        public List<(Kunde kunde, double totalPris)> GetAlleKundersForbrug()
        {
            return Forbrug.HentAlleKundersForbrug();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
