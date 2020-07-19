using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace SweenGame.Sounds
{
    public class SoundManager : GameComponent, ISoundManager
    {
        private readonly ContentManager _content;

        private IDictionary<string, SoundEffect> _soundEffects = new Dictionary<string, SoundEffect>();
        private Song _currentSong;
        private float _volume;
        private bool _songIsStopping;

        public SoundManager(Game game) : base(game)
        {
            _content = game.Content;

            LoadSounds();
        }

        public string EnqueuedSongName { get; set; }
        public float Volume
        {
            get => _volume;
            set
            {
                if (_volume.Equals(value))
                    return;
                _volume = value;
                MediaPlayer.Volume = _volume;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_songIsStopping)
            {
                Volume -= 0.03f;
                if (Volume <= 0.0f)
                {
                    StopSong();
                }
                return;
            }
            if (MediaPlayer.State == MediaState.Stopped && !(_currentSong is null))
            {
                MediaPlayer.Volume = Volume;
                MediaPlayer.Play(_currentSong);
            }

            Volume = Math.Min(1.0f, Volume + 0.01f);
        }

        public void Play(string name, float pitch = 0.0f)
        {
            _soundEffects[name].Play(1.0f, pitch, 0.0f);
        }

        public void LoadSong(string songName)
        {
            if (_currentSong is null)
            {
                _currentSong = _content.Load<Song>(songName);
                return;
            }
            EnqueuedSongName = songName;
        }

        public void StopSong(bool forceStop)
        {
            _songIsStopping = true;
            if (forceStop)
            {
                StopSong();
            }
        }

        private void StopSong()
        {
            _songIsStopping = false;
            MediaPlayer.Stop();
            Volume = 0.0f;
            LoadNextSong();
        }

        private void LoadNextSong()
        {
            _currentSong.Dispose();
            if (EnqueuedSongName != null)
                _currentSong = _content.Load<Song>(EnqueuedSongName);
        }

        private void LoadSounds()
        {
            AddSound(SoundEffectNames.SelectEffect);
        }

        private void AddSound(string name)
        {
            _soundEffects.Add(name, _content.Load<SoundEffect>(name));
        }
    }
}