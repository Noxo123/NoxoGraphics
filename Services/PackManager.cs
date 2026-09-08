using System.IO.Compression;
using System.IO;

namespace NoxoGraphics.Services;

public sealed class PackManager
{
    public async Task InstallAsync(
        string archive,
        string target,
        string backupRoot,
        IProgress<string>? progress = null,
        CancellationToken ct = default)
    {
        if (!File.Exists(archive))
            throw new FileNotFoundException("Pack introuvable", archive);

        Directory.CreateDirectory(target);
        Directory.CreateDirectory(backupRoot);

        progress?.Report("Création de la sauvegarde...");
        foreach (var file in Directory.EnumerateFiles(target, "*", SearchOption.AllDirectories))
        {
            ct.ThrowIfCancellationRequested();
            var relative = Path.GetRelativePath(target, file);
            var backup = Path.Combine(backupRoot, relative);
            Directory.CreateDirectory(Path.GetDirectoryName(backup)!);
            File.Copy(file, backup, true);
        }

        progress?.Report("Vérification du pack...");
        await Task.Run(() =>
        {
            using var archiveFile = ZipFile.OpenRead(archive);
            var targetFullPath = Path.GetFullPath(target)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                + Path.DirectorySeparatorChar;

            foreach (var entry in archiveFile.Entries)
            {
                ct.ThrowIfCancellationRequested();

                if (string.IsNullOrWhiteSpace(entry.FullName))
                    continue;

                var destination = Path.GetFullPath(Path.Combine(target, entry.FullName));
                if (!destination.StartsWith(targetFullPath, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidDataException("Le pack contient un chemin ZIP non sécurisé.");
            }

            progress?.Report("Extraction du pack...");
            foreach (var entry in archiveFile.Entries)
            {
                ct.ThrowIfCancellationRequested();
                if (string.IsNullOrEmpty(entry.Name))
                    continue;

                var destination = Path.GetFullPath(Path.Combine(target, entry.FullName));
                Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
                entry.ExtractToFile(destination, true);
            }
        }, ct);

        progress?.Report("Installation terminée.");
    }

    public Task UninstallAsync(
        string backupRoot,
        string target,
        IProgress<string>? progress = null,
        CancellationToken ct = default)
    {
        if (!Directory.Exists(backupRoot))
            throw new DirectoryNotFoundException("Aucune sauvegarde disponible.");

        progress?.Report("Restauration de la sauvegarde...");
        Directory.CreateDirectory(target);

        foreach (var file in Directory.EnumerateFiles(target, "*", SearchOption.AllDirectories).ToList())
        {
            ct.ThrowIfCancellationRequested();
            var relative = Path.GetRelativePath(target, file);
            var backup = Path.Combine(backupRoot, relative);
            if (!File.Exists(backup))
                File.Delete(file);
        }

        foreach (var file in Directory.EnumerateFiles(backupRoot, "*", SearchOption.AllDirectories))
        {
            ct.ThrowIfCancellationRequested();
            var relative = Path.GetRelativePath(backupRoot, file);
            var destination = Path.Combine(target, relative);
            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            File.Copy(file, destination, true);
        }

        progress?.Report("Restauration terminée.");
        return Task.CompletedTask;
    }
}
