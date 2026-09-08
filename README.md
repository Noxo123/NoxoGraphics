# NoxoGraphics

Gestionnaire Windows moderne pour installer, sauvegarder, restaurer et gérer des packs graphiques FiveM.

## Stack
- C# / .NET 8
- WPF
- Installation autonome win-x64 via GitHub Actions

## Fonctions
- Détection automatique de FiveM et GTA V
- Interface sombre moderne et responsive
- Catalogue de packs piloté par JSON
- Architecture d'installation avec sauvegarde avant modification
- Rollback/restauration
- Journal d'opérations
- Base pour détection matérielle et recommandations qualité/FPS

## Ajouter un pack
Ajoutez son entrée dans `packs/catalog.json`, puis publiez son archive via votre infrastructure de distribution. Le manifeste peut ensuite être étendu avec les chemins de destination, hashes SHA-256, prérequis et version minimale.

## Sécurité
Les packs distribués par NoxoGraphics doivent être vérifiés (hash/signature) avant installation. L'application ne doit jamais exécuter un fichier arbitraire provenant d'une archive.
