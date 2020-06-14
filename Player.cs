using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame
{
    enum PlayerState
    {
        Unequipped,
        Sword,
        Shield,
        Full
    }

    public class Player
    {
        const int DELAY = 5;

        private readonly Vector2 scale = new Vector2(3, 3);

        private PlayerState _playerState;
        private int _currentFrame;
        private int _totalFrames;
        private int _delayCount = 0;

        public Player(Texture2D texture, int rows, int columns)
        {
            PlayerTexture = texture;
            Rows = rows;
            Columns = columns;

            _currentFrame = 0;
            _totalFrames = Columns;
        }

        public Texture2D PlayerTexture { get; set; }

        public int Rows { get; set; }
        public int Columns { get; set; }

        public void Update()
        {
            _delayCount++;
            if (_delayCount < DELAY)
                return;

            _delayCount = 0;
            _currentFrame++;
            if (_currentFrame.Equals(_totalFrames))
                _currentFrame = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            var width = PlayerTexture.Width / Columns;
            var height = PlayerTexture.Height / Rows;
            var row = (int)(_playerState);
            var column = _currentFrame % Columns;

            var sourceRectangle = new Rectangle(width * column, height * row, width, height);
            var destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Begin();
            spriteBatch.Draw(PlayerTexture,
                             location,
                             sourceRectangle,
                             Color.White,
                             rotation: 0,
                             origin: Vector2.Zero,
                             scale,
                             SpriteEffects.None,
                             layerDepth: 0);
            spriteBatch.End();
        }
    }
}