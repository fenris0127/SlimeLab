using NUnit.Framework;
using SlimeLab.Systems;

namespace SlimeLab.Tests
{
    [TestFixture]
    public class AudioSystemTests
    {
        [Test]
        public void AudioManager_CanBeCreated()
        {
            var audioManager = new AudioManager();

            Assert.IsNotNull(audioManager);
        }

        [Test]
        public void AudioManager_CanPlayMusic()
        {
            var audioManager = new AudioManager();

            audioManager.PlayMusic("bgm_menu");

            Assert.IsTrue(audioManager.IsMusicPlaying);
            Assert.AreEqual("bgm_menu", audioManager.CurrentMusicID);
        }

        [Test]
        public void AudioManager_CanStopMusic()
        {
            var audioManager = new AudioManager();

            audioManager.PlayMusic("bgm_menu");
            Assert.IsTrue(audioManager.IsMusicPlaying);

            audioManager.StopMusic();

            Assert.IsFalse(audioManager.IsMusicPlaying);
            Assert.IsNull(audioManager.CurrentMusicID);
        }

        [Test]
        public void AudioManager_CanPauseMusic()
        {
            var audioManager = new AudioManager();

            audioManager.PlayMusic("bgm_menu");
            audioManager.PauseMusic();

            Assert.IsTrue(audioManager.IsMusicPaused);
        }

        [Test]
        public void AudioManager_CanResumeMusic()
        {
            var audioManager = new AudioManager();

            audioManager.PlayMusic("bgm_menu");
            audioManager.PauseMusic();
            Assert.IsTrue(audioManager.IsMusicPaused);

            audioManager.ResumeMusic();

            Assert.IsFalse(audioManager.IsMusicPaused);
        }

        [Test]
        public void AudioManager_CanSetMusicVolume()
        {
            var audioManager = new AudioManager();

            audioManager.SetMusicVolume(0.5f);

            Assert.AreEqual(0.5f, audioManager.MusicVolume);
        }

        [Test]
        public void AudioManager_ClampsMusicVolume()
        {
            var audioManager = new AudioManager();

            audioManager.SetMusicVolume(1.5f);
            Assert.AreEqual(1.0f, audioManager.MusicVolume);

            audioManager.SetMusicVolume(-0.5f);
            Assert.AreEqual(0.0f, audioManager.MusicVolume);
        }

        [Test]
        public void AudioManager_CanPlaySoundEffect()
        {
            var audioManager = new AudioManager();

            audioManager.PlaySoundEffect("sfx_click");

            // Sound effects play independently
            Assert.AreEqual(1, audioManager.ActiveSoundEffectCount);
        }

        [Test]
        public void AudioManager_CanSetSoundEffectVolume()
        {
            var audioManager = new AudioManager();

            audioManager.SetSoundEffectVolume(0.7f);

            Assert.AreEqual(0.7f, audioManager.SoundEffectVolume);
        }

        [Test]
        public void MusicTransition_CanBeCreated()
        {
            var transition = new MusicTransition("bgm_menu", "bgm_battle", 2.0f);

            Assert.IsNotNull(transition);
            Assert.AreEqual("bgm_menu", transition.FromMusicID);
            Assert.AreEqual("bgm_battle", transition.ToMusicID);
            Assert.AreEqual(2.0f, transition.Duration);
        }

        [Test]
        public void MusicTransition_HasProgress()
        {
            var transition = new MusicTransition("bgm_menu", "bgm_battle", 2.0f);

            Assert.AreEqual(0f, transition.Progress);
        }

        [Test]
        public void MusicTransition_CanUpdate()
        {
            var transition = new MusicTransition("bgm_menu", "bgm_battle", 2.0f);

            transition.Update(1.0f); // 1 second

            Assert.AreEqual(0.5f, transition.Progress); // 50% of 2 seconds
        }

        [Test]
        public void MusicTransition_CompletesWhenProgressReachesOne()
        {
            var transition = new MusicTransition("bgm_menu", "bgm_battle", 2.0f);

            transition.Update(2.0f);

            Assert.IsTrue(transition.IsComplete);
            Assert.AreEqual(1.0f, transition.Progress);
        }

        [Test]
        public void AudioManager_CanTransitionMusic()
        {
            var audioManager = new AudioManager();

            audioManager.PlayMusic("bgm_menu");
            audioManager.TransitionToMusic("bgm_battle", 2.0f);

            Assert.IsTrue(audioManager.IsTransitioning);
            Assert.AreEqual("bgm_menu", audioManager.CurrentMusicID);
        }

        [Test]
        public void AudioManager_CompletesTransitionAfterDuration()
        {
            var audioManager = new AudioManager();

            audioManager.PlayMusic("bgm_menu");
            audioManager.TransitionToMusic("bgm_battle", 2.0f);

            // Update through transition
            audioManager.Update(2.0f);

            Assert.IsFalse(audioManager.IsTransitioning);
            Assert.AreEqual("bgm_battle", audioManager.CurrentMusicID);
        }

        [Test]
        public void SoundEffect_CanBeCreated()
        {
            var sfx = new SoundEffect("sfx_click");

            Assert.IsNotNull(sfx);
            Assert.AreEqual("sfx_click", sfx.ID);
        }

        [Test]
        public void SoundEffect_CanHaveVolume()
        {
            var sfx = new SoundEffect("sfx_click", volume: 0.8f);

            Assert.AreEqual(0.8f, sfx.Volume);
        }

        [Test]
        public void SoundEffect_CanBePlayed()
        {
            var sfx = new SoundEffect("sfx_click");

            sfx.Play();

            Assert.IsTrue(sfx.IsPlaying);
        }

        [Test]
        public void SoundEffect_CanBeStopped()
        {
            var sfx = new SoundEffect("sfx_click");

            sfx.Play();
            Assert.IsTrue(sfx.IsPlaying);

            sfx.Stop();

            Assert.IsFalse(sfx.IsPlaying);
        }

        [Test]
        public void AudioManager_CanStopAllSoundEffects()
        {
            var audioManager = new AudioManager();

            audioManager.PlaySoundEffect("sfx_click");
            audioManager.PlaySoundEffect("sfx_feed");

            Assert.AreEqual(2, audioManager.ActiveSoundEffectCount);

            audioManager.StopAllSoundEffects();

            Assert.AreEqual(0, audioManager.ActiveSoundEffectCount);
        }

        [Test]
        public void AudioManager_CanMuteMusic()
        {
            var audioManager = new AudioManager();

            audioManager.SetMusicMuted(true);

            Assert.IsTrue(audioManager.IsMusicMuted);
        }

        [Test]
        public void AudioManager_CanMuteSoundEffects()
        {
            var audioManager = new AudioManager();

            audioManager.SetSoundEffectsMuted(true);

            Assert.IsTrue(audioManager.AreSoundEffectsMuted);
        }

        [Test]
        public void AudioManager_MutedMusicDoesNotPlay()
        {
            var audioManager = new AudioManager();

            audioManager.SetMusicMuted(true);
            audioManager.PlayMusic("bgm_menu");

            // Music should not be considered playing when muted
            Assert.IsFalse(audioManager.IsMusicPlaying);
        }

        [Test]
        public void MusicTransition_CanHaveCustomCurve()
        {
            var transition = new MusicTransition("bgm_menu", "bgm_battle", 2.0f, TransitionCurve.EaseInOut);

            Assert.AreEqual(TransitionCurve.EaseInOut, transition.Curve);
        }

        [Test]
        public void AudioManager_CanGetTransitionProgress()
        {
            var audioManager = new AudioManager();

            audioManager.PlayMusic("bgm_menu");
            audioManager.TransitionToMusic("bgm_battle", 2.0f);

            audioManager.Update(1.0f); // 50% progress

            Assert.AreEqual(0.5f, audioManager.TransitionProgress);
        }

        [Test]
        public void SoundEffect_HasDefaultVolume()
        {
            var sfx = new SoundEffect("sfx_click");

            Assert.AreEqual(1.0f, sfx.Volume);
        }

        [Test]
        public void AudioManager_CanPlayMusicWithFade()
        {
            var audioManager = new AudioManager();

            audioManager.PlayMusicWithFade("bgm_menu", 1.0f);

            Assert.IsTrue(audioManager.IsFading);
        }

        [Test]
        public void AudioManager_CompletesFadeAfterDuration()
        {
            var audioManager = new AudioManager();

            audioManager.PlayMusicWithFade("bgm_menu", 1.0f);
            audioManager.Update(1.0f);

            Assert.IsFalse(audioManager.IsFading);
            Assert.IsTrue(audioManager.IsMusicPlaying);
        }

        [Test]
        public void AudioManager_CanCheckIfMusicIsLoaded()
        {
            var audioManager = new AudioManager();

            audioManager.LoadMusic("bgm_menu");

            Assert.IsTrue(audioManager.IsMusicLoaded("bgm_menu"));
        }

        [Test]
        public void AudioManager_CanUnloadMusic()
        {
            var audioManager = new AudioManager();

            audioManager.LoadMusic("bgm_menu");
            Assert.IsTrue(audioManager.IsMusicLoaded("bgm_menu"));

            audioManager.UnloadMusic("bgm_menu");

            Assert.IsFalse(audioManager.IsMusicLoaded("bgm_menu"));
        }

        [Test]
        public void SoundEffect_CanHavePitch()
        {
            var sfx = new SoundEffect("sfx_click", volume: 1.0f, pitch: 1.2f);

            Assert.AreEqual(1.2f, sfx.Pitch);
        }

        [Test]
        public void AudioManager_CanPlaySoundEffectWithPitch()
        {
            var audioManager = new AudioManager();

            audioManager.PlaySoundEffect("sfx_click", volume: 1.0f, pitch: 0.8f);

            Assert.AreEqual(1, audioManager.ActiveSoundEffectCount);
        }

        [Test]
        public void MusicTransition_ProgressDoesNotExceedOne()
        {
            var transition = new MusicTransition("bgm_menu", "bgm_battle", 2.0f);

            transition.Update(5.0f); // Update beyond duration

            Assert.AreEqual(1.0f, transition.Progress);
        }

        [Test]
        public void AudioManager_CanGetLoadedMusicList()
        {
            var audioManager = new AudioManager();

            audioManager.LoadMusic("bgm_menu");
            audioManager.LoadMusic("bgm_battle");

            var loadedMusic = audioManager.GetLoadedMusic();

            Assert.AreEqual(2, loadedMusic.Count);
            Assert.Contains("bgm_menu", loadedMusic);
            Assert.Contains("bgm_battle", loadedMusic);
        }
    }
}
