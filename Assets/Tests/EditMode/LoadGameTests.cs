using NUnit.Framework;
using SlimeLab.Systems;
using System.IO;

namespace SlimeLab.Tests
{
    [TestFixture]
    public class LoadGameTests
    {
        private string _testSavePath;

        [SetUp]
        public void SetUp()
        {
            _testSavePath = Path.Combine(Path.GetTempPath(), "slimelab_load_test.json");

            // Clean up any existing test save file
            if (File.Exists(_testSavePath))
            {
                File.Delete(_testSavePath);
            }
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up test save file
            if (File.Exists(_testSavePath))
            {
                File.Delete(_testSavePath);
            }
        }

        [Test]
        public void LoadManager_CanBeCreated()
        {
            var saveManager = new SaveManager(_testSavePath);
            var loadManager = new LoadManager(saveManager);

            Assert.IsNotNull(loadManager);
        }

        [Test]
        public void LoadManager_CanLoadGameState()
        {
            var saveManager = new SaveManager(_testSavePath);
            var loadManager = new LoadManager(saveManager);

            // Save a game state
            var gameState = new GameState();
            gameState.SetValue("TestData", "LoadTest");
            saveManager.Save(gameState);

            // Load it back
            var loadedState = loadManager.Load();

            Assert.IsNotNull(loadedState);
            Assert.AreEqual("LoadTest", loadedState.GetValue<string>("TestData"));
        }

        [Test]
        public void LoadManager_ReturnsNullWhenNoSaveExists()
        {
            var saveManager = new SaveManager(_testSavePath);
            var loadManager = new LoadManager(saveManager);

            var loadedState = loadManager.Load();

            Assert.IsNull(loadedState);
        }

        [Test]
        public void VersionChecker_CanCheckCompatibility()
        {
            var checker = new VersionChecker("1.0.0");

            Assert.IsTrue(checker.IsCompatible("1.0.0"));
        }

        [Test]
        public void VersionChecker_RecognizesIncompatibleVersions()
        {
            var checker = new VersionChecker("2.0.0");

            // Major version mismatch = incompatible
            Assert.IsFalse(checker.IsCompatible("1.0.0"));
        }

        [Test]
        public void VersionChecker_AllowsMinorVersionDifferences()
        {
            var checker = new VersionChecker("1.2.0");

            // Minor version difference should be compatible
            Assert.IsTrue(checker.IsCompatible("1.1.0"));
            Assert.IsTrue(checker.IsCompatible("1.3.0"));
        }

        [Test]
        public void VersionChecker_AllowsPatchVersionDifferences()
        {
            var checker = new VersionChecker("1.0.5");

            // Patch version difference should be compatible
            Assert.IsTrue(checker.IsCompatible("1.0.3"));
            Assert.IsTrue(checker.IsCompatible("1.0.7"));
        }

        [Test]
        public void LoadManager_ChecksVersionCompatibility()
        {
            var saveManager = new SaveManager(_testSavePath);
            var loadManager = new LoadManager(saveManager);

            // Save with version 1.0.0
            var gameState = new GameState();
            gameState.Version = "1.0.0";
            saveManager.Save(gameState);

            // Set current version to 1.0.0
            loadManager.SetCurrentVersion("1.0.0");

            var loadedState = loadManager.Load();
            Assert.IsNotNull(loadedState);
        }

        [Test]
        public void LoadManager_RejectsIncompatibleVersion()
        {
            var saveManager = new SaveManager(_testSavePath);
            var loadManager = new LoadManager(saveManager);

            // Save with version 1.0.0
            var gameState = new GameState();
            gameState.Version = "1.0.0";
            saveManager.Save(gameState);

            // Set current version to 2.0.0 (major version change)
            loadManager.SetCurrentVersion("2.0.0");

            var loadedState = loadManager.Load();
            Assert.IsNull(loadedState); // Should reject incompatible version
        }

        [Test]
        public void VersionMigration_CanBeCreated()
        {
            var migration = new VersionMigration("1.0.0", "1.1.0");

            Assert.IsNotNull(migration);
            Assert.AreEqual("1.0.0", migration.FromVersion);
            Assert.AreEqual("1.1.0", migration.ToVersion);
        }

        [Test]
        public void VersionMigration_CanMigrateGameState()
        {
            var migration = new VersionMigration("1.0.0", "1.1.0");

            migration.AddTransform((state) =>
            {
                // Add new field in version 1.1.0
                state.SetValue("NewField", "DefaultValue");
            });

            var gameState = new GameState();
            gameState.Version = "1.0.0";

            migration.Apply(gameState);

            Assert.AreEqual("1.1.0", gameState.Version);
            Assert.AreEqual("DefaultValue", gameState.GetValue<string>("NewField"));
        }

        [Test]
        public void MigrationManager_CanBeCreated()
        {
            var migrationManager = new MigrationManager();

            Assert.IsNotNull(migrationManager);
        }

        [Test]
        public void MigrationManager_CanAddMigration()
        {
            var migrationManager = new MigrationManager();
            var migration = new VersionMigration("1.0.0", "1.1.0");

            migrationManager.AddMigration(migration);

            Assert.AreEqual(1, migrationManager.MigrationCount);
        }

        [Test]
        public void MigrationManager_CanMigrateToCurrentVersion()
        {
            var migrationManager = new MigrationManager();

            // Add migration from 1.0.0 to 1.1.0
            var migration1 = new VersionMigration("1.0.0", "1.1.0");
            migration1.AddTransform((state) =>
            {
                state.SetValue("Field1", "Added in 1.1.0");
            });

            // Add migration from 1.1.0 to 1.2.0
            var migration2 = new VersionMigration("1.1.0", "1.2.0");
            migration2.AddTransform((state) =>
            {
                state.SetValue("Field2", "Added in 1.2.0");
            });

            migrationManager.AddMigration(migration1);
            migrationManager.AddMigration(migration2);

            // Migrate from 1.0.0 to 1.2.0
            var gameState = new GameState();
            gameState.Version = "1.0.0";

            migrationManager.Migrate(gameState, "1.2.0");

            Assert.AreEqual("1.2.0", gameState.Version);
            Assert.AreEqual("Added in 1.1.0", gameState.GetValue<string>("Field1"));
            Assert.AreEqual("Added in 1.2.0", gameState.GetValue<string>("Field2"));
        }

        [Test]
        public void MigrationManager_SkipsMigrationIfAlreadyAtVersion()
        {
            var migrationManager = new MigrationManager();

            var gameState = new GameState();
            gameState.Version = "1.2.0";
            gameState.SetValue("TestValue", "Original");

            migrationManager.Migrate(gameState, "1.2.0");

            // Should not change anything
            Assert.AreEqual("1.2.0", gameState.Version);
            Assert.AreEqual("Original", gameState.GetValue<string>("TestValue"));
        }

        [Test]
        public void LoadManager_AutomaticallyMigratesOnLoad()
        {
            var saveManager = new SaveManager(_testSavePath);
            var loadManager = new LoadManager(saveManager);
            var migrationManager = new MigrationManager();

            // Set up migration
            var migration = new VersionMigration("1.0.0", "1.1.0");
            migration.AddTransform((state) =>
            {
                state.SetValue("MigratedField", true);
            });
            migrationManager.AddMigration(migration);

            loadManager.SetMigrationManager(migrationManager);
            loadManager.SetCurrentVersion("1.1.0");

            // Save old version
            var gameState = new GameState();
            gameState.Version = "1.0.0";
            gameState.SetValue("OldData", "preserved");
            saveManager.Save(gameState);

            // Load and auto-migrate
            var loadedState = loadManager.Load();

            Assert.IsNotNull(loadedState);
            Assert.AreEqual("1.1.0", loadedState.Version);
            Assert.AreEqual(true, loadedState.GetValue<bool>("MigratedField"));
            Assert.AreEqual("preserved", loadedState.GetValue<string>("OldData"));
        }

        [Test]
        public void VersionMigration_CanApplyMultipleTransforms()
        {
            var migration = new VersionMigration("1.0.0", "1.1.0");

            migration.AddTransform((state) =>
            {
                state.SetValue("Field1", "Value1");
            });

            migration.AddTransform((state) =>
            {
                state.SetValue("Field2", "Value2");
            });

            var gameState = new GameState();
            gameState.Version = "1.0.0";

            migration.Apply(gameState);

            Assert.AreEqual("Value1", gameState.GetValue<string>("Field1"));
            Assert.AreEqual("Value2", gameState.GetValue<string>("Field2"));
        }

        [Test]
        public void MigrationManager_AppliesMultipleMigrationsInOrder()
        {
            var migrationManager = new MigrationManager();

            var migration1 = new VersionMigration("1.0.0", "1.1.0");
            migration1.AddTransform((state) =>
            {
                state.SetValue("Counter", 1);
            });

            var migration2 = new VersionMigration("1.1.0", "1.2.0");
            migration2.AddTransform((state) =>
            {
                int counter = state.GetValue<int>("Counter");
                state.SetValue("Counter", counter + 1);
            });

            var migration3 = new VersionMigration("1.2.0", "1.3.0");
            migration3.AddTransform((state) =>
            {
                int counter = state.GetValue<int>("Counter");
                state.SetValue("Counter", counter + 1);
            });

            migrationManager.AddMigration(migration1);
            migrationManager.AddMigration(migration2);
            migrationManager.AddMigration(migration3);

            var gameState = new GameState();
            gameState.Version = "1.0.0";

            migrationManager.Migrate(gameState, "1.3.0");

            Assert.AreEqual("1.3.0", gameState.Version);
            Assert.AreEqual(3, gameState.GetValue<int>("Counter"));
        }

        [Test]
        public void LoadManager_CanGetLastLoadedVersion()
        {
            var saveManager = new SaveManager(_testSavePath);
            var loadManager = new LoadManager(saveManager);

            // Initially null
            Assert.IsNull(loadManager.LastLoadedVersion);

            // Save and load
            var gameState = new GameState();
            gameState.Version = "1.5.0";
            saveManager.Save(gameState);

            loadManager.SetCurrentVersion("1.5.0");
            loadManager.Load();

            Assert.AreEqual("1.5.0", loadManager.LastLoadedVersion);
        }

        [Test]
        public void VersionChecker_HandlesInvalidVersionStrings()
        {
            var checker = new VersionChecker("1.0.0");

            Assert.IsFalse(checker.IsCompatible("invalid"));
            Assert.IsFalse(checker.IsCompatible("1.0"));
            Assert.IsFalse(checker.IsCompatible(""));
        }

        [Test]
        public void MigrationManager_CanCheckIfMigrationExists()
        {
            var migrationManager = new MigrationManager();
            var migration = new VersionMigration("1.0.0", "1.1.0");

            migrationManager.AddMigration(migration);

            Assert.IsTrue(migrationManager.HasMigrationPath("1.0.0", "1.1.0"));
            Assert.IsFalse(migrationManager.HasMigrationPath("1.0.0", "2.0.0"));
        }

        [Test]
        public void LoadManager_HandlesPartialMigrationPath()
        {
            var saveManager = new SaveManager(_testSavePath);
            var loadManager = new LoadManager(saveManager);
            var migrationManager = new MigrationManager();

            // Only have migration from 1.0.0 to 1.1.0
            var migration = new VersionMigration("1.0.0", "1.1.0");
            migrationManager.AddMigration(migration);

            loadManager.SetMigrationManager(migrationManager);
            loadManager.SetCurrentVersion("1.2.0");

            // Save version 1.0.0
            var gameState = new GameState();
            gameState.Version = "1.0.0";
            saveManager.Save(gameState);

            // Should still load but only migrate as far as possible
            var loadedState = loadManager.Load();

            // Without complete migration path, may return null or partially migrated
            // Implementation should handle this gracefully
            Assert.IsNotNull(loadedState);
        }

        [Test]
        public void VersionMigration_CanRenameFields()
        {
            var migration = new VersionMigration("1.0.0", "1.1.0");

            migration.AddTransform((state) =>
            {
                // Rename OldFieldName to NewFieldName
                var value = state.GetValue<string>("OldFieldName");
                if (value != null)
                {
                    state.SetValue("NewFieldName", value);
                }
            });

            var gameState = new GameState();
            gameState.Version = "1.0.0";
            gameState.SetValue("OldFieldName", "TestValue");

            migration.Apply(gameState);

            Assert.AreEqual("TestValue", gameState.GetValue<string>("NewFieldName"));
        }

        [Test]
        public void MigrationManager_CanGetAllMigrations()
        {
            var migrationManager = new MigrationManager();
            var migration1 = new VersionMigration("1.0.0", "1.1.0");
            var migration2 = new VersionMigration("1.1.0", "1.2.0");

            migrationManager.AddMigration(migration1);
            migrationManager.AddMigration(migration2);

            var migrations = migrationManager.GetAllMigrations();

            Assert.AreEqual(2, migrations.Count);
            Assert.Contains(migration1, migrations);
            Assert.Contains(migration2, migrations);
        }
    }
}
