using System.Collections.Generic;
using System.Linq;

namespace SlimeLab.Systems
{
    public class AudioManager
    {
        public bool IsMusicPlaying { get; private set; }
        public bool IsMusicPaused { get; private set; }
        public string CurrentMusicID { get; private set; }
        public float MusicVolume { get; private set; }
        public float SoundEffectVolume { get; private set; }
        public int ActiveSoundEffectCount => _activeSoundEffects.Count;
        public bool IsTransitioning => _currentTransition != null && !_currentTransition.IsComplete;
        public float TransitionProgress => _currentTransition != null ? _currentTransition.Progress : 0f;
        public bool IsMusicMuted { get; private set; }
        public bool AreSoundEffectsMuted { get; private set; }
        public bool IsFading { get; private set; }

        private List<SoundEffect> _activeSoundEffects;
        private MusicTransition _currentTransition;
        private HashSet<string> _loadedMusic;
        private float _fadeElapsedTime;
        private float _fadeDuration;
        private string _fadingMusicID;

        public AudioManager()
        {
            IsMusicPlaying = false;
            IsMusicPaused = false;
            CurrentMusicID = null;
            MusicVolume = 1.0f;
            SoundEffectVolume = 1.0f;
            IsMusicMuted = false;
            AreSoundEffectsMuted = false;
            IsFading = false;
            _activeSoundEffects = new List<SoundEffect>();
            _currentTransition = null;
            _loadedMusic = new HashSet<string>();
        }

        public void PlayMusic(string musicID)
        {
            if (IsMusicMuted)
            {
                return;
            }

            CurrentMusicID = musicID;
            IsMusicPlaying = true;
            IsMusicPaused = false;
        }

        public void StopMusic()
        {
            CurrentMusicID = null;
            IsMusicPlaying = false;
            IsMusicPaused = false;
        }

        public void PauseMusic()
        {
            IsMusicPaused = true;
        }

        public void ResumeMusic()
        {
            IsMusicPaused = false;
        }

        public void SetMusicVolume(float volume)
        {
            MusicVolume = Clamp(volume, 0f, 1f);
        }

        public void SetSoundEffectVolume(float volume)
        {
            SoundEffectVolume = Clamp(volume, 0f, 1f);
        }

        public void PlaySoundEffect(string sfxID, float volume = 1.0f, float pitch = 1.0f)
        {
            if (AreSoundEffectsMuted)
            {
                return;
            }

            var sfx = new SoundEffect(sfxID, volume, pitch);
            sfx.Play();
            _activeSoundEffects.Add(sfx);
        }

        public void StopAllSoundEffects()
        {
            foreach (var sfx in _activeSoundEffects)
            {
                sfx.Stop();
            }
            _activeSoundEffects.Clear();
        }

        public void SetMusicMuted(bool muted)
        {
            IsMusicMuted = muted;
        }

        public void SetSoundEffectsMuted(bool muted)
        {
            AreSoundEffectsMuted = muted;
        }

        public void TransitionToMusic(string toMusicID, float duration)
        {
            _currentTransition = new MusicTransition(CurrentMusicID, toMusicID, duration);
        }

        public void Update(float deltaTime)
        {
            // Handle music transition
            if (_currentTransition != null && !_currentTransition.IsComplete)
            {
                _currentTransition.Update(deltaTime);

                if (_currentTransition.IsComplete)
                {
                    CurrentMusicID = _currentTransition.ToMusicID;
                    _currentTransition = null;
                }
            }

            // Handle fade
            if (IsFading)
            {
                _fadeElapsedTime += deltaTime;

                if (_fadeElapsedTime >= _fadeDuration)
                {
                    IsFading = false;
                    CurrentMusicID = _fadingMusicID;
                    IsMusicPlaying = true;
                }
            }

            // Clean up stopped sound effects
            _activeSoundEffects.RemoveAll(sfx => !sfx.IsPlaying);
        }

        public void PlayMusicWithFade(string musicID, float fadeDuration)
        {
            IsFading = true;
            _fadeElapsedTime = 0f;
            _fadeDuration = fadeDuration;
            _fadingMusicID = musicID;
        }

        public void LoadMusic(string musicID)
        {
            _loadedMusic.Add(musicID);
        }

        public void UnloadMusic(string musicID)
        {
            _loadedMusic.Remove(musicID);
        }

        public bool IsMusicLoaded(string musicID)
        {
            return _loadedMusic.Contains(musicID);
        }

        public List<string> GetLoadedMusic()
        {
            return new List<string>(_loadedMusic);
        }

        private float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}
