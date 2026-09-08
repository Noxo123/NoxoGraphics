using System.IO.Compression;
namespace NoxoGraphics.Services;
public sealed class PackManager {
 public async Task InstallAsync(string archive, string target, string backupRoot, IProgress<string>? progress=null, CancellationToken ct=default) {
  if (!File.Exists(archive)) throw new FileNotFoundException("Pack introuvable", archive);
  Directory.CreateDirectory(target); Directory.CreateDirectory(backupRoot);
  progress?.Report("Création de la sauvegarde...");
  foreach (var file in Directory.EnumerateFiles(target, "*", SearchOption.AllDirectories)) {
   ct.ThrowIfCancellationRequested();
   var relative = Path.GetRelativePath(target, file); var backup = Path.Combine(backupRoot, relative);
   Directory.CreateDirectory(Path.GetDirectoryName(backup)!); File.Copy(file, backup, true);
  }
  progress?.Report("Extraction du pack...");
  await Task.Run(() => ZipFile.ExtractToDirectory(archive, target, true), ct);
  progress?.Report("Installation terminée.");
 }
 public Task UninstallAsync(string backupRoot, string target, IProgress<string>? progress=null, CancellationToken ct=default) {
  if (!Directory.Exists(backupRoot)) throw new DirectoryNotFoundException("Aucune sauvegarde disponible.");
  progress?.Report("Restauration de la sauvegarde...");
  foreach (var file in Directory.EnumerateFiles(backupRoot, "*", SearchOption.AllDirectories)) {
   ct.ThrowIfCancellationRequested(); var relative=Path.GetRelativePath(backupRoot,file); var dest=Path.Combine(target,relative);
   Directory.CreateDirectory(Path.GetDirectoryName(dest)!); File.Copy(file,dest,true);
  }
  progress?.Report("Restauration terminée."); return Task.CompletedTask;
 }
}
