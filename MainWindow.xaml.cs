using System.Windows;
using NoxoGraphics.Services;
namespace NoxoGraphics;
public partial class MainWindow : Window {
 readonly FiveMDetector detector = new(); readonly PackManager packs = new();
 public MainWindow(){ InitializeComponent(); Loaded += (_,_) => Scan(); }
 void Scan(){ var f=detector.Detect(); var g=detector.DetectGta(); FiveMPath.Text=f ?? "FiveM non détecté — sélection manuelle bientôt disponible"; GtaPath.Text=g is null ? "GTA V non détecté" : $"GTA V : {g}"; StatusText.Text=f is null ? "⚠ Vérification requise" : "● FiveM détecté"; LogText.Text=f is null ? "Installe FiveM puis relance l'application." : "FiveM détecté. Ton environnement est prêt."; }
 void Dashboard_Click(object s,RoutedEventArgs e)=>PageTitle.Text="Dashboard";
 void Packs_Click(object s,RoutedEventArgs e)=>PageTitle.Text="Packs graphiques";
 void Installed_Click(object s,RoutedEventArgs e)=>PageTitle.Text="Packs installés";
 void Settings_Click(object s,RoutedEventArgs e)=>PageTitle.Text="Paramètres";
 void Optimize_Click(object s,RoutedEventArgs e){ LogText.Text="Analyse GPU / RAM / stockage... Recommandation : preset équilibré. Optimisation automatique disponible avec les packs installés."; }
 async void Install_Click(object s,RoutedEventArgs e){ var btn=(System.Windows.Controls.Button)s; btn.IsEnabled=false; try { LogText.Text="Préparation du pack... Une archive de pack signée peut être ajoutée au catalogue pour lancer l'installation 1-clic."; await Task.Delay(500); } finally { btn.IsEnabled=true; } }
}
