namespace SweenGame.Sounds
{
    public interface ISoundManager
    {
        float Volume { get; set; }

        void LoadSong(string songName);
        void Play(string name, float pitch = 0);
        void StopSong(bool forceStop = false);
    }
}