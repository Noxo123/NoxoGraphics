using Microsoft.Win32;
using System.IO;
namespace NoxoGraphics.Services;
public sealed class FiveMDetector {
 public string? Detect() {
  var candidates = new[] {
   Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\FiveM\FiveM.app",
   Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CitizenFX\FiveM\FiveM.app"
  };
  foreach (var p in candidates) if (Directory.Exists(p)) return p;
  try {
   using var key = Registry.CurrentUser.OpenSubKey(@"Software\CitizenFX\FiveM");
   var value = key?.GetValue("InstallPath") as string;
   if (!string.IsNullOrWhiteSpace(value) && Directory.Exists(value)) return value;
  } catch { }
  return null;
 }
 public string? DetectGta() {
  var paths = new[] { @"C:\Program Files\Rockstar Games\Grand Theft Auto V", @"C:\Program Files (x86)\Steam\steamapps\common\Grand Theft Auto V", @"C:\Program Files\Epic Games\GTAV" };
  return paths.FirstOrDefault(Directory.Exists);
 }
}
