namespace SlimeLab.Systems
{
    public class LoadManager
    {
        public string LastLoadedVersion { get; private set; }

        private SaveManager _saveManager;
        private string _currentVersion;
        private MigrationManager _migrationManager;

        public LoadManager(SaveManager saveManager)
        {
            _saveManager = saveManager;
            _currentVersion = "1.0.0"; // Default version
            _migrationManager = null;
            LastLoadedVersion = null;
        }

        public void SetCurrentVersion(string version)
        {
            _currentVersion = version;
        }

        public void SetMigrationManager(MigrationManager migrationManager)
        {
            _migrationManager = migrationManager;
        }

        public GameState Load()
        {
            // Use SaveManager to load the file
            GameState gameState = _saveManager.Load();

            if (gameState == null)
            {
                return null;
            }

            LastLoadedVersion = gameState.Version;

            // Check version compatibility
            var versionChecker = new VersionChecker(_currentVersion);
            if (!versionChecker.IsCompatible(gameState.Version))
            {
                // Incompatible version
                return null;
            }

            // Apply migrations if needed
            if (_migrationManager != null && gameState.Version != _currentVersion)
            {
                _migrationManager.Migrate(gameState, _currentVersion);
            }

            return gameState;
        }
    }
}
