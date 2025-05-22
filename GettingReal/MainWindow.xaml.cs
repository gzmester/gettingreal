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
using GettingReal.Model;
using GettingReal.Services;

namespace GettingReal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
        }

        private void FindKunde_Click(object sender, RoutedEventArgs e)
        {
            var id = SearchKundeIdBox.Text.Trim();
            if (string.IsNullOrEmpty(id))
            {
                ResultTextBlock.Text = "Indtast et kunde-id.";
                return;
            }
            var kunde = Lager.Instance.HentKunde(id);
            if (kunde != null)
            {
                ResultTextBlock.Text = $"Fundet kunde: {kunde.Navn} (Id: {kunde.Id}) Pris: {kunde.Pris}";
                _viewModel.SelectedKunde = kunde;
            }
            else
            {
                ResultTextBlock.Text = "Ingen kunde fundet med det id.";
            }
        }

        private void VisForbrug_Click(object sender, RoutedEventArgs e)
        {
            var kunde = _viewModel.SelectedKunde;
            if (kunde == null)
            {
                ResultTextBlock.Text = "Vælg eller søg en kunde først.";
                return;
            }
            var forbrug = kunde.forbrug;
            if (forbrug != null)
            {
                ResultTextBlock.Text = $"Forbrug for {kunde.Navn}: Tokens tilgængelige: {forbrug.TokensTilgaengelige}, Samtaler brugt: {forbrug.SamtalerBrugt}, Total pris: {forbrug.CalculatePrice()}";
            }
            else
            {
                ResultTextBlock.Text = "Ingen forbrugsdata for denne kunde.";
            }
        }

        private void VisAlleAiModeller_Click(object sender, RoutedEventArgs e)
        {
            var modeller = Lager.Instance.HentAlleModeller();
            if (modeller != null && modeller.Count > 0)
            {
                ResultTextBlock.Text = "AI Modeller:\n" + string.Join("\n", modeller.Select(m => $"{m.aiModel} (Id: {m.id}) Pris pr. token: {m.prisPerToken}"));
            }
            else
            {
                ResultTextBlock.Text = "Ingen AI modeller fundet.";
            }
        }

        private void VisAlleKundersForbrug_Click(object sender, RoutedEventArgs e)
        {
            var alle = Forbrug.HentAlleKundersForbrug();
            if (alle.Count == 0)
            {
                ResultTextBlock.Text = "Ingen kunders forbrug fundet.";
                return;
            }
            ResultTextBlock.Text = "Alle kunders forbrug:\n" +
                string.Join("\n", alle.Select(t => $"{t.kunde.Navn} (Id: {t.kunde.Id}): {t.totalPris}"));
        }
    }
}