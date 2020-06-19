using System.Collections.Generic;
using TestGame.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TestGame
{
    enum PlayerState
    {
        Unequipped,
        Sword,
        Shield,
        Full
    }

    public class MovementEventArgs : EventArgs
    {
        public MovementEventArgs(Direction direction)
        {
            Direction = direction;
        }

        public Direction Direction { get; }
    }

    public class Player : DrawableGameComponent
    {
        const int DELAY = 5;
        const int MOVE_STEPS = 48;
        const int PACE = 4;

        private PlayerState _playerState;
        private Texture2D _playerAtlas;
        private Vector2 _playerLocation;

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

        public EventHandler<MovementEventArgs> HandlePlayerMovement;

        public int Rows { get; set; }
        public int Columns { get; set; }
        public int Width => _spriteWidth;
        public int Height => _spriteHeight;

        public Direction CurrentDirection { get; set; }

        public bool IsMoving { get; set; }

        private Vector2 MoveVector { get; set; } = Vector2.Zero;

        public Rectangle Destination => new Rectangle((int)_playerLocation.X,
                                                      (int)_playerLocation.Y,
                                                      _spriteWidth,
                                                      _spriteHeight);

        public override void Initialize()
        {
            Rows = 4;
            Columns = 8;
            CurrentDirection = Direction.South;

            _currentFrame = 0;
            _totalFrames = Columns;
            _playerLocation = new Vector2(480, 288);

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
            if (!IsMoving && Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                StartMovement(Direction.East);
            }
            else if (!IsMoving && Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                StartMovement(Direction.West);
            }
            if (!IsMoving && Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                StartMovement(Direction.North);
            }
            else if (!IsMoving && Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                StartMovement(Direction.South);
            }
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

        private void StartMovement(Direction direction)
        {
            CurrentDirection = direction;
            HandlePlayerMovement.Invoke(this, new MovementEventArgs(direction));

            if (!IsMoving)
                return;

            _remainingSteps = MOVE_STEPS;

            switch (direction)
            {
                case Direction.East:
                    MoveVector = new Vector2(PACE, 0);
                    break;

                case Direction.South:
                    MoveVector = new Vector2(0, PACE);
                    break;

                case Direction.West:
                    MoveVector = new Vector2(-PACE, 0);
                    break;

                case Direction.North:
                    MoveVector = new Vector2(0, -PACE);
                    break;
            }
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            if (!IsMoving)
                return;

            _remainingSteps -= PACE;
            _playerLocation += MoveVector;

            if (_remainingSteps.Equals(0))
                IsMoving = false;
        }

        public override void Draw(GameTime gameTime)
        {
            var sourceRectangle = _playerSprites[_playerState][((int)CurrentDirection * 2) + _currentFrame];
            var destinationRectangle = new Rectangle((int)_playerLocation.X, (int)_playerLocation.Y, _spriteWidth, _spriteHeight);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_playerAtlas,
                             _playerLocation,
                             sourceRectangle,
                             Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}