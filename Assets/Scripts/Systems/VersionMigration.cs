using System;
using System.Collections.Generic;

namespace SlimeLab.Systems
{
    public class VersionMigration
    {
        public string FromVersion { get; private set; }
        public string ToVersion { get; private set; }

        private List<Action<GameState>> _transforms;

        public VersionMigration(string fromVersion, string toVersion)
        {
            FromVersion = fromVersion;
            ToVersion = toVersion;
            _transforms = new List<Action<GameState>>();
        }

        public void AddTransform(Action<GameState> transform)
        {
            _transforms.Add(transform);
        }

        public void Apply(GameState gameState)
        {
            foreach (var transform in _transforms)
            {
                transform(gameState);
            }

            // Update version after applying all transforms
            gameState.Version = ToVersion;
        }
    }
}
