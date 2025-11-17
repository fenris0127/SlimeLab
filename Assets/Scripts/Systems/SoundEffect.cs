namespace SlimeLab.Systems
{
    public class SoundEffect
    {
        public string ID { get; private set; }
        public float Volume { get; private set; }
        public float Pitch { get; private set; }
        public bool IsPlaying { get; private set; }

        public SoundEffect(string id, float volume = 1.0f, float pitch = 1.0f)
        {
            ID = id;
            Volume = volume;
            Pitch = pitch;
            IsPlaying = false;
        }

        public void Play()
        {
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
        }
    }
}
