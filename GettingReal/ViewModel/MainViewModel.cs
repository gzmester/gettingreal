using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GettingReal.Model;
using GettingReal.Services;

namespace GettingReal.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Kunde> Kunder { get; set; }
        public ObservableCollection<AiModel> AiModels { get; set; }

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
			Kunder = new ObservableCollection<Kunde>(Lager.Instance.HentAlleKunder());
			AiModels = new ObservableCollection<AiModel>(Lager.Instance.HentAlleModeller());
			Console.WriteLine("MainViewModel initialized.");
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
