using System.Collections.Generic;
using TestGame.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestGame
{
    public class Player : DrawableGameComponent, IFocusable
    {
        const int DELAY = 5;
        const int MOVE_STEPS = 48;
        const int PACE = 4;

        private PlayerState _playerState;
        private Texture2D _playerAtlas;
        private Vector2 _position;

        private bool _isMoving;
        private int _currentFrame;
        private int _totalFrames;
        private int _remainingSteps;
        private int _delayCount;
        private int _spriteWidth;
        private int _spriteHeight;

        private readonly Dictionary<PlayerState, List<Rectangle>> _playerSprites;
        private readonly SpriteBatch _spriteBatch;
        private readonly ContentManager _contentManager;

        public Player(Game game, SpriteBatch spriteBatch, ContentManager contentManager) : base(game)
        {
            this._spriteBatch = spriteBatch;
            this._contentManager = contentManager;

            _playerSprites = new Dictionary<PlayerState, List<Rectangle>>{
                {PlayerState.Unequipped, new List<Rectangle>()},
                {PlayerState.Sword, new List<Rectangle>()},
                {PlayerState.Shield, new List<Rectangle>()},
                {PlayerState.Full, new List<Rectangle>()},
            };
        }

        public int Rows { get; set; }
        public int Columns { get; set; }
        public int Width => _spriteWidth;
        public int Height => _spriteHeight;

        public Direction CurrentDirection { get; set; }

        public bool IsMoving
        {
            get => _isMoving;
            set
            {
                if (value)
                    _remainingSteps = MOVE_STEPS;
                _isMoving = value;
            }
        }

        public Vector2 MoveVector { get; set; } = Vector2.Zero;

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
            }
        }

        public Rectangle Destination => new Rectangle((int)_position.X,
                                                      (int)_position.Y,
                                                      _spriteWidth,
                                                      _spriteHeight);

        public override void Initialize()
        {
            Rows = 4;
            Columns = 8;
            CurrentDirection = Direction.South;

            _currentFrame = 0;
            _totalFrames = Columns;
            _position = new Vector2(480, 288);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _playerAtlas = _contentManager.Load<Texture2D>("Sprites/dw_green_hero_48");
            _spriteWidth = (int)(_playerAtlas.Width / Columns);
            _spriteHeight = (int)(_playerAtlas.Height / Rows);

            for (var y = 0; y < Rows; y++)
                for (var x = 0; x < Columns; x++)
                {
                    _playerSprites[(PlayerState)y].Add(new Rectangle(x * _spriteWidth, y * _spriteHeight, _spriteWidth, _spriteHeight));
                }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            UpdateMovement();

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                _playerState = PlayerState.Full;

            _delayCount++;
            if (_delayCount < DELAY)
                return;

            _delayCount = 0;
            _currentFrame++;
            _currentFrame = _currentFrame % 2;

            base.Update(gameTime);
        }

        private void UpdateMovement()
        {
            if (!IsMoving)
                return;

            _remainingSteps -= PACE;
            _position += MoveVector;

            if (_remainingSteps.Equals(0))
                IsMoving = false;
        }

        public override void Draw(GameTime gameTime)
        {
            var sourceRectangle = _playerSprites[_playerState][((int)CurrentDirection * 2) + _currentFrame];
            var destinationRectangle = new Rectangle((int)_position.X, (int)_position.Y, _spriteWidth, _spriteHeight);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_playerAtlas,
                             _position,
                             sourceRectangle,
                             Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private Vector2 SetMoveVector(Vector2 value)
        {
            return new Vector2(value.X * PACE, value.Y * PACE);
        }
    }
}