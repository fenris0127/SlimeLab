using System.Collections.Generic;
using System.Linq;

namespace SlimeLab.Systems
{
    public class MigrationManager
    {
        public int MigrationCount => _migrations.Count;

        private List<VersionMigration> _migrations;

        public MigrationManager()
        {
            _migrations = new List<VersionMigration>();
        }

        public void AddMigration(VersionMigration migration)
        {
            _migrations.Add(migration);
        }

        public void Migrate(GameState gameState, string targetVersion)
        {
            if (gameState.Version == targetVersion)
            {
                return; // Already at target version
            }

            // Find migration path
            string currentVersion = gameState.Version;

            while (currentVersion != targetVersion)
            {
                var migration = _migrations.FirstOrDefault(m => m.FromVersion == currentVersion);

                if (migration == null)
                {
                    // No migration path found - stop here
                    break;
                }

                migration.Apply(gameState);
                currentVersion = gameState.Version;
            }
        }

        public bool HasMigrationPath(string fromVersion, string toVersion)
        {
            if (fromVersion == toVersion)
            {
                return true;
            }

            string currentVersion = fromVersion;

            while (currentVersion != toVersion)
            {
                var migration = _migrations.FirstOrDefault(m => m.FromVersion == currentVersion);

                if (migration == null)
                {
                    return false;
                }

                currentVersion = migration.ToVersion;

                // Prevent infinite loops
                if (currentVersion == fromVersion)
                {
                    return false;
                }
            }

            return true;
        }

        public List<VersionMigration> GetAllMigrations()
        {
            return new List<VersionMigration>(_migrations);
        }
    }
}
