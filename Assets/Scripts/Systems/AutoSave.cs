namespace SlimeLab.Systems
{
    public class AutoSave
    {
        public int IntervalSeconds { get; private set; }
        public bool IsEnabled { get; private set; }
        public int TimeSinceLastSave { get; private set; }

        private SaveManager _saveManager;

        public AutoSave(SaveManager saveManager, int intervalSeconds)
        {
            _saveManager = saveManager;
            IntervalSeconds = intervalSeconds;
            IsEnabled = false;
            TimeSinceLastSave = 0;
        }

        public void Enable()
        {
            IsEnabled = true;
        }

        public void Disable()
        {
            IsEnabled = false;
        }

        public void Update(int deltaSeconds, GameState gameState)
        {
            if (!IsEnabled)
            {
                return;
            }

            TimeSinceLastSave += deltaSeconds;

            if (TimeSinceLastSave >= IntervalSeconds)
            {
                _saveManager.Save(gameState);
                TimeSinceLastSave = 0;
            }
        }

        public void SetInterval(int intervalSeconds)
        {
            IntervalSeconds = intervalSeconds;
        }
    }
}
