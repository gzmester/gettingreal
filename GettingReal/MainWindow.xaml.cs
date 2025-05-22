using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GettingReal.ViewModel;
using GettingReal.Model; // Tilføjet for Forbrug
using GettingReal.Services; // Tilføjet for Lager
using System.Linq; // Tilføjet for LINQ i knap-events

namespace GettingReal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;
        private List<(Kunde kunde, double totalPris)> _aktuelAlleKundersForbrugListe; // Til at gemme den usorterede/sorterede liste

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
            // Sæt default værdier for sortering ComboBoxe
            KundeSortCriteriaComboBox.SelectedIndex = 0;
            AiModelSortCriteriaComboBox.SelectedIndex = 0;
            _aktuelAlleKundersForbrugListe = new List<(Kunde kunde, double totalPris)>();
        }

        private void FindKunde_Click(object sender, RoutedEventArgs e)
        {
            KundeSortOptionsPanel.Visibility = Visibility.Collapsed;
            _aktuelAlleKundersForbrugListe.Clear(); // Ryd data for "alle kunder"

            var id = SearchKundeIdBox.Text.Trim();
            if (string.IsNullOrEmpty(id))
            {
                ResultTextBlock.Text = "Indtast et kunde-id.";
                ResultTextBlock.Foreground = Brushes.OrangeRed;
                return;
            }
            // Brug ViewModel's metode hvis den findes, ellers direkte Lager
            var kunde = _viewModel.FindKundeById(id); // Antager denne findes i ViewModel
            // Eller: var kunde = Lager.Instance.HentKunde(id); 
            if (kunde != null)
            {
                ResultTextBlock.Text = $"Fundet kunde: {kunde.Navn} (Id: {kunde.Id}) Pris: {kunde.Pris}";
                ResultTextBlock.Foreground = Brushes.LightGreen;
                _viewModel.SelectedKunde = kunde; // Opdater valgt kunde i ViewModel
            }
            else
            {
                ResultTextBlock.Text = "Ingen kunde fundet med det id.";
                ResultTextBlock.Foreground = Brushes.OrangeRed;
            }
        }

        private void VisForbrug_Click(object sender, RoutedEventArgs e)
        {
            KundeSortOptionsPanel.Visibility = Visibility.Collapsed;
            _aktuelAlleKundersForbrugListe.Clear();

            var kunde = _viewModel.SelectedKunde;
            if (kunde == null)
            {
                ResultTextBlock.Text = "Vælg eller søg en kunde først.";
                ResultTextBlock.Foreground = Brushes.OrangeRed;
                return;
            }
            // Brug ViewModel's metode hvis den findes, ellers direkte Forbrug klasse
            var forbrugsPris = _viewModel.GetForbrugForKunde(kunde.Id); // Antager denne findes
            // Eller: var forbrugsPris = Forbrug.HentPrisForKunde(kunde.Id);

            if (kunde.forbrug != null)
            {
                ResultTextBlock.Text = $"Forbrug for {kunde.Navn}:\nTokens tilgængelige: {kunde.forbrug.TokensTilgaengelige}\nSamtaler brugt: {kunde.forbrug.SamtalerBrugt}\nTotal pris: {forbrugsPris:C}";
                ResultTextBlock.Foreground = Brushes.White;
            }
            else
            {
                ResultTextBlock.Text = "Ingen forbrugsdata for denne kunde.";
                ResultTextBlock.Foreground = Brushes.OrangeRed;
            }
        }

        private void VisAlleAiModeller_Click(object sender, RoutedEventArgs e)
        {
            // Overvej om kunde sorteringspanelet også skal skjules her
            // KundeSortOptionsPanel.Visibility = Visibility.Collapsed;
            // _aktuelAlleKundersForbrugListe.Clear();

            var modeller = Lager.Instance.HentAlleModeller(); // Hent frisk data
            _viewModel.AiModels.Clear(); // Ryd eksisterende
            foreach(var model in modeller) _viewModel.AiModels.Add(model); // Tilføj ny data

            if (_viewModel.AiModels != null && _viewModel.AiModels.Count > 0)
            {
                // Viser kun AI modeller i deres egen ListBox, ResultTextBlock opdateres ikke her
                // medmindre du specifikt ønsker det.
                // For nu, lad ResultTextBlock være uændret eller vis en generel besked.
                // ResultTextBlock.Text = "AI Modeller er opdateret i listen til højre.";
                // ResultTextBlock.Foreground = Brushes.White;
            }
            else
            {
                ResultTextBlock.Text = "Ingen AI modeller fundet.";
                ResultTextBlock.Foreground = Brushes.OrangeRed;
            }
        }

        private void VisAlleKundersForbrug_Click(object sender, RoutedEventArgs e)
        {
            _aktuelAlleKundersForbrugListe = _viewModel.GetAlleKundersForbrug(); 

            if (_aktuelAlleKundersForbrugListe == null || !_aktuelAlleKundersForbrugListe.Any())
            {
                ResultTextBlock.Text = "Ingen kunders forbrug fundet.";
                ResultTextBlock.Foreground = Brushes.OrangeRed;
                KundeSortOptionsPanel.Visibility = Visibility.Collapsed;
                return;
            }
            
            // Sorteringskriterier er allerede valgt i ComboBoxe, anvend dem
            AnvendKundeForbrugSorteringOgOpdaterDisplay();
            KundeSortOptionsPanel.Visibility = Visibility.Visible;
        }

        private void AnvendKundeForbrugSorteringOgOpdaterDisplay()
        {
            if (_aktuelAlleKundersForbrugListe == null || !_aktuelAlleKundersForbrugListe.Any())
            {
                ResultTextBlock.Text = "Ingen data at vise for alle kunders forbrug.";
                ResultTextBlock.Foreground = Brushes.OrangeRed;
                KundeSortOptionsPanel.Visibility = Visibility.Collapsed; // Skjul hvis ingen data efter f.eks. filtrering
                return;
            }

            var criteria = (SortKundeCriteria)(KundeSortCriteriaComboBox.SelectedItem ?? SortKundeCriteria.Navn);
            var direction = (SortDirection)(KundeSortDirectionComboBox.SelectedItem ?? SortDirection.Ascending);
            
            List<(Kunde kunde, double totalPris)> sortedList;

            switch (criteria)
            {
                case SortKundeCriteria.TokensTilgaengelige:
                    sortedList = direction == SortDirection.Ascending ?
                        _aktuelAlleKundersForbrugListe.OrderBy(t => t.kunde.forbrug?.TokensTilgaengelige ?? 0).ToList() :
                        _aktuelAlleKundersForbrugListe.OrderByDescending(t => t.kunde.forbrug?.TokensTilgaengelige ?? 0).ToList();
                    break;
                case SortKundeCriteria.TotalPris:
                    sortedList = direction == SortDirection.Ascending ?
                        _aktuelAlleKundersForbrugListe.OrderBy(t => t.totalPris).ToList() :
                        _aktuelAlleKundersForbrugListe.OrderByDescending(t => t.totalPris).ToList();
                    break;
                case SortKundeCriteria.AntalSamtaler:
                    sortedList = direction == SortDirection.Ascending ?
                        _aktuelAlleKundersForbrugListe.OrderBy(t => t.kunde.forbrug?.SamtalerBrugt ?? 0).ToList() :
                        _aktuelAlleKundersForbrugListe.OrderByDescending(t => t.kunde.forbrug?.SamtalerBrugt ?? 0).ToList();
                    break;
                case SortKundeCriteria.Navn:
                default:
                    sortedList = direction == SortDirection.Ascending ?
                        _aktuelAlleKundersForbrugListe.OrderBy(t => t.kunde.Navn).ToList() :
                        _aktuelAlleKundersForbrugListe.OrderByDescending(t => t.kunde.Navn).ToList();
                    break;
            }
            
            // Opdater _aktuelAlleKundersForbrugListe med den sorterede liste, hvis du vil beholde sorteringen
            // _aktuelAlleKundersForbrugListe = sortedList; // Valgfrit, afhængig af om du vil re-sortere fra original hver gang

            StringBuilder sb = new StringBuilder("Alle kunders forbrug (sorteret):\n");
            foreach (var t in sortedList)
            {
                sb.AppendLine($"Navn: {t.kunde.Navn} (Id: {t.kunde.Id})");
                sb.AppendLine($"  Total Pris: {t.totalPris:C}");
                sb.AppendLine($"  Tokens Tilgængelige: {t.kunde.forbrug?.TokensTilgaengelige ?? 0}");
                sb.AppendLine($"  Samtaler Brugt: {t.kunde.forbrug?.SamtalerBrugt ?? 0}");
                sb.AppendLine("---");
            }
            ResultTextBlock.Text = sb.ToString();
            ResultTextBlock.Foreground = Brushes.White;
            KundeSortOptionsPanel.Visibility = Visibility.Visible;
        }


        private void KundeSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Kald kun sortering hvis "Alle kunders forbrug" er aktivt (dvs. listen har data)
            if (_aktuelAlleKundersForbrugListe != null && _aktuelAlleKundersForbrugListe.Any())
            {
                AnvendKundeForbrugSorteringOgOpdaterDisplay();
            }
        }

        private void AiModelSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel == null || AiModelSortCriteriaComboBox.SelectedItem == null || AiModelSortDirectionComboBox.SelectedItem == null)
                return;

            var criteria = (SortAiModelCriteria)AiModelSortCriteriaComboBox.SelectedItem;
            var direction = (SortDirection)AiModelSortDirectionComboBox.SelectedItem;
            _viewModel.SortAiModels(criteria, direction); // Dette sorterer ListBox'en for AI Modeller
        }
    }
}