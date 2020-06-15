using System;
using System.Collections.Generic;
using TestGame.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestGame
{
    enum PlayerState
    {
        Unequipped,
        Sword,
        Shield,
        Full
    }

    public class Player : DrawableGameComponent
    {
        const int DELAY = 5;

        private readonly Vector2 scale = new Vector2(3, 3);

        private PlayerState _playerState;
        private Texture2D _playerAtlas;
        private Vector2 _playerLocation;

        private int _currentFrame;
        private int _totalFrames;
        private int _delayCount = 0;
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

            Console.WriteLine($"Sprite Batch: {_spriteBatch.Name}");
        }

        public int Rows { get; set; }
        public int Columns { get; set; }

        public Direction CurrentDirection { get; set; }

        public override void Initialize()
        {
            Console.WriteLine("Initializing Player");
            Rows = 4;
            Columns = 8;
            CurrentDirection = Direction.South;

            _currentFrame = 0;
            _totalFrames = Columns;
            _playerLocation = new Vector2(256, 312);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Console.WriteLine($"{nameof(Player)} got content loaded. How?");
            _playerAtlas = _contentManager.Load<Texture2D>("Sprites/dw_green_hero");
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
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                CurrentDirection = Direction.East;
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                CurrentDirection = Direction.West;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                CurrentDirection = Direction.North;
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                CurrentDirection = Direction.South;

            _delayCount++;
            if (_delayCount < DELAY)
                return;

            _delayCount = 0;
            _currentFrame++;
            _currentFrame = _currentFrame % 2;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var sourceRectangle = _playerSprites[_playerState][((int)CurrentDirection * 2) + _currentFrame];
            var destinationRectangle = new Rectangle((int)_playerLocation.X, (int)_playerLocation.Y, _spriteWidth, _spriteHeight);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_playerAtlas,
                             _playerLocation,
                             sourceRectangle,
                             Color.White,
                             rotation: 0,
                             origin: Vector2.Zero,
                             scale,
                             SpriteEffects.None,
                             layerDepth: 0);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}