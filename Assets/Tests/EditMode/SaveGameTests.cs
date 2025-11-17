using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;
using System.Collections.Generic;
using System.IO;

namespace SlimeLab.Tests
{
    [TestFixture]
    public class SaveGameTests
    {
        private string _testSavePath;

        [SetUp]
        public void SetUp()
        {
            _testSavePath = Path.Combine(Path.GetTempPath(), "slimelab_test_save.json");

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
        public void GameState_CanBeCreated()
        {
            var gameState = new GameState();

            Assert.IsNotNull(gameState);
        }

        [Test]
        public void GameState_HasVersion()
        {
            var gameState = new GameState();

            Assert.IsNotNull(gameState.Version);
            Assert.IsNotEmpty(gameState.Version);
        }

        [Test]
        public void GameState_HasTimestamp()
        {
            var gameState = new GameState();

            Assert.Greater(gameState.SaveTimestamp, 0);
        }

        [Test]
        public void GameState_CanSerializeToJson()
        {
            var gameState = new GameState();

            string json = gameState.ToJson();

            Assert.IsNotNull(json);
            Assert.IsNotEmpty(json);
            Assert.IsTrue(json.Contains("Version"));
            Assert.IsTrue(json.Contains("SaveTimestamp"));
        }

        [Test]
        public void GameState_CanDeserializeFromJson()
        {
            var originalState = new GameState();
            string json = originalState.ToJson();

            var loadedState = GameState.FromJson(json);

            Assert.IsNotNull(loadedState);
            Assert.AreEqual(originalState.Version, loadedState.Version);
        }

        [Test]
        public void GameState_PreservesDataAfterSerialization()
        {
            var gameState = new GameState();
            gameState.SetValue("TestKey", "TestValue");
            gameState.SetValue("NumberKey", 42);

            string json = gameState.ToJson();
            var loadedState = GameState.FromJson(json);

            Assert.AreEqual("TestValue", loadedState.GetValue<string>("TestKey"));
            Assert.AreEqual(42, loadedState.GetValue<int>("NumberKey"));
        }

        [Test]
        public void SaveManager_CanBeCreated()
        {
            var saveManager = new SaveManager(_testSavePath);

            Assert.IsNotNull(saveManager);
        }

        [Test]
        public void SaveManager_CanSaveGameState()
        {
            var saveManager = new SaveManager(_testSavePath);
            var gameState = new GameState();

            saveManager.Save(gameState);

            Assert.IsTrue(File.Exists(_testSavePath));
        }

        [Test]
        public void SaveManager_CanLoadGameState()
        {
            var saveManager = new SaveManager(_testSavePath);
            var originalState = new GameState();
            originalState.SetValue("TestData", "Hello World");

            saveManager.Save(originalState);
            var loadedState = saveManager.Load();

            Assert.IsNotNull(loadedState);
            Assert.AreEqual("Hello World", loadedState.GetValue<string>("TestData"));
        }

        [Test]
        public void SaveManager_ReturnsNullWhenNoSaveFileExists()
        {
            var saveManager = new SaveManager(_testSavePath);

            var loadedState = saveManager.Load();

            Assert.IsNull(loadedState);
        }

        [Test]
        public void SaveManager_CanCheckIfSaveExists()
        {
            var saveManager = new SaveManager(_testSavePath);

            Assert.IsFalse(saveManager.SaveExists());

            var gameState = new GameState();
            saveManager.Save(gameState);

            Assert.IsTrue(saveManager.SaveExists());
        }

        [Test]
        public void SaveManager_CanDeleteSave()
        {
            var saveManager = new SaveManager(_testSavePath);
            var gameState = new GameState();

            saveManager.Save(gameState);
            Assert.IsTrue(saveManager.SaveExists());

            saveManager.DeleteSave();
            Assert.IsFalse(saveManager.SaveExists());
        }

        [Test]
        public void AutoSave_CanBeCreated()
        {
            var saveManager = new SaveManager(_testSavePath);
            var autoSave = new AutoSave(saveManager, intervalSeconds: 60);

            Assert.IsNotNull(autoSave);
        }

        [Test]
        public void AutoSave_HasInterval()
        {
            var saveManager = new SaveManager(_testSavePath);
            var autoSave = new AutoSave(saveManager, intervalSeconds: 120);

            Assert.AreEqual(120, autoSave.IntervalSeconds);
        }

        [Test]
        public void AutoSave_IsInitiallyDisabled()
        {
            var saveManager = new SaveManager(_testSavePath);
            var autoSave = new AutoSave(saveManager, intervalSeconds: 60);

            Assert.IsFalse(autoSave.IsEnabled);
        }

        [Test]
        public void AutoSave_CanBeEnabled()
        {
            var saveManager = new SaveManager(_testSavePath);
            var autoSave = new AutoSave(saveManager, intervalSeconds: 60);

            autoSave.Enable();

            Assert.IsTrue(autoSave.IsEnabled);
        }

        [Test]
        public void AutoSave_CanBeDisabled()
        {
            var saveManager = new SaveManager(_testSavePath);
            var autoSave = new AutoSave(saveManager, intervalSeconds: 60);

            autoSave.Enable();
            Assert.IsTrue(autoSave.IsEnabled);

            autoSave.Disable();
            Assert.IsFalse(autoSave.IsEnabled);
        }

        [Test]
        public void AutoSave_DoesNotSaveWhenDisabled()
        {
            var saveManager = new SaveManager(_testSavePath);
            var autoSave = new AutoSave(saveManager, intervalSeconds: 1);
            var gameState = new GameState();

            autoSave.Update(2, gameState); // Update past interval

            Assert.IsFalse(saveManager.SaveExists());
        }

        [Test]
        public void AutoSave_SavesAfterIntervalWhenEnabled()
        {
            var saveManager = new SaveManager(_testSavePath);
            var autoSave = new AutoSave(saveManager, intervalSeconds: 5);
            var gameState = new GameState();
            gameState.SetValue("AutoSaveTest", true);

            autoSave.Enable();

            // Update but not past interval
            autoSave.Update(3, gameState);
            Assert.IsFalse(saveManager.SaveExists());

            // Update past interval
            autoSave.Update(3, gameState); // Total: 6 seconds
            Assert.IsTrue(saveManager.SaveExists());

            // Verify saved data
            var loadedState = saveManager.Load();
            Assert.AreEqual(true, loadedState.GetValue<bool>("AutoSaveTest"));
        }

        [Test]
        public void AutoSave_ResetsTimerAfterSave()
        {
            var saveManager = new SaveManager(_testSavePath);
            var autoSave = new AutoSave(saveManager, intervalSeconds: 5);
            var gameState = new GameState();

            autoSave.Enable();

            // First save
            autoSave.Update(6, gameState);
            Assert.IsTrue(saveManager.SaveExists());

            // Delete save to test next cycle
            saveManager.DeleteSave();

            // Update less than interval - should not save yet
            autoSave.Update(3, gameState);
            Assert.IsFalse(saveManager.SaveExists());

            // Update to complete interval - should save
            autoSave.Update(3, gameState); // Total: 6 seconds from last save
            Assert.IsTrue(saveManager.SaveExists());
        }

        [Test]
        public void AutoSave_CanGetTimeSinceLastSave()
        {
            var saveManager = new SaveManager(_testSavePath);
            var autoSave = new AutoSave(saveManager, intervalSeconds: 10);
            var gameState = new GameState();

            autoSave.Enable();

            Assert.AreEqual(0, autoSave.TimeSinceLastSave);

            autoSave.Update(5, gameState);
            Assert.AreEqual(5, autoSave.TimeSinceLastSave);
        }

        [Test]
        public void GameState_CanStoreComplexData()
        {
            var gameState = new GameState();

            var testList = new List<string> { "Item1", "Item2", "Item3" };
            gameState.SetValue("TestList", testList);

            string json = gameState.ToJson();
            var loadedState = GameState.FromJson(json);

            var loadedList = loadedState.GetValue<List<string>>("TestList");
            Assert.AreEqual(3, loadedList.Count);
            Assert.Contains("Item1", loadedList);
            Assert.Contains("Item2", loadedList);
            Assert.Contains("Item3", loadedList);
        }

        [Test]
        public void SaveManager_CanGetSavePath()
        {
            var saveManager = new SaveManager(_testSavePath);

            Assert.AreEqual(_testSavePath, saveManager.SavePath);
        }

        [Test]
        public void GameState_ReturnsDefaultValueForMissingKey()
        {
            var gameState = new GameState();

            Assert.AreEqual(0, gameState.GetValue<int>("NonExistentKey"));
            Assert.IsNull(gameState.GetValue<string>("NonExistentKey"));
        }

        [Test]
        public void SaveManager_HandlesCorruptedSaveFile()
        {
            var saveManager = new SaveManager(_testSavePath);

            // Write corrupted data
            File.WriteAllText(_testSavePath, "This is not valid JSON");

            var loadedState = saveManager.Load();

            // Should return null for corrupted save
            Assert.IsNull(loadedState);
        }

        [Test]
        public void AutoSave_CanChangeInterval()
        {
            var saveManager = new SaveManager(_testSavePath);
            var autoSave = new AutoSave(saveManager, intervalSeconds: 60);

            Assert.AreEqual(60, autoSave.IntervalSeconds);

            autoSave.SetInterval(120);

            Assert.AreEqual(120, autoSave.IntervalSeconds);
        }

        [Test]
        public void GameState_CanOverwriteExistingKey()
        {
            var gameState = new GameState();

            gameState.SetValue("Key", "FirstValue");
            Assert.AreEqual("FirstValue", gameState.GetValue<string>("Key"));

            gameState.SetValue("Key", "SecondValue");
            Assert.AreEqual("SecondValue", gameState.GetValue<string>("Key"));
        }

        [Test]
        public void SaveManager_CreatesDirectoryIfNotExists()
        {
            string deepPath = Path.Combine(Path.GetTempPath(), "slimelab_test_dir", "saves", "game.json");
            var saveManager = new SaveManager(deepPath);
            var gameState = new GameState();

            saveManager.Save(gameState);

            Assert.IsTrue(File.Exists(deepPath));

            // Cleanup
            Directory.Delete(Path.Combine(Path.GetTempPath(), "slimelab_test_dir"), true);
        }
    }
}
