using System.IO;

namespace SlimeLab.Systems
{
    public class SaveManager
    {
        public string SavePath { get; private set; }

        public SaveManager(string savePath)
        {
            SavePath = savePath;
        }

        public void Save(GameState gameState)
        {
            // Update timestamp before saving
            gameState.SaveTimestamp = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Ensure directory exists
            string directory = Path.GetDirectoryName(SavePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Write JSON to file
            string json = gameState.ToJson();
            File.WriteAllText(SavePath, json);
        }

        public GameState Load()
        {
            if (!File.Exists(SavePath))
            {
                return null;
            }

            try
            {
                string json = File.ReadAllText(SavePath);
                return GameState.FromJson(json);
            }
            catch
            {
                // Return null if file is corrupted or cannot be read
                return null;
            }
        }

        public bool SaveExists()
        {
            return File.Exists(SavePath);
        }

        public void DeleteSave()
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
            }
        }
    }
}
