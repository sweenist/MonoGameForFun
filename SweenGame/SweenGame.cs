﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SweenGame.Enums;
using SweenGame.Input;
using SweenGame.Screens;
using SweenGame.Services;
using SweenGame.Sounds;
using static SweenGame.Extensions.Constants;
using static SweenGame.Helpers.FunctionHelpers;

namespace SweenGame
{
    public class SweenGame : Game
    {
        private GraphicsDeviceManager _graphicsManager;
        private ScreenManager _screenManager;

        public SweenGame()
        {
            _graphicsManager = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            TargetElapsedTime = TimeSpan.FromMilliseconds(25);
            //GetDisplayModes();
        }

        private static void GetDisplayModes()
        {
            foreach (var mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                Console.WriteLine($"Aspect: {mode.AspectRatio}\n\tSize: {mode.Width}x{mode.Height} Safe Area:{mode.TitleSafeArea}");
            }
        }

        protected override void Initialize()
        {
            _graphicsManager.PreferredBackBufferWidth = ScreenWidth;
            _graphicsManager.PreferredBackBufferHeight = ScreenHeight;
            _graphicsManager.ApplyChanges();

            Window.Title = "Sween's Awesome Game";

            var input = new InputSystem(this);
            input.Enabled = true;
            input.AddAction("Move Left", Control.Left, ActionType.MoveLeft, Noop);
            input.AddAction("Move Up", Control.Up, ActionType.MoveUp, Noop);
            input.AddAction("Move Right", Control.Right, ActionType.MoveRight, Noop);
            input.AddAction("Move Down", Control.Down, ActionType.MoveDown, Noop);
            input.AddAction("Debug", Control.A, ActionType.Debug, Noop);
            ServiceLocator.Instance.AddService<IInputSystem>(input);

            ServiceLocator.Instance.AddService<ISoundManager>(typeof(SoundManager), this);

            _screenManager = new ScreenManager(this, _graphicsManager);
            ServiceLocator.Instance.AddService<IScreenManager>(_screenManager);
            _screenManager.Initialize();
            _screenManager.AddScreen(new MainMenuScreen());

            Components.Add(input);
            Components.Add(ServiceLocator.Instance.GetService<ISoundManager>() as SoundManager);
            Components.Add(_screenManager);

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}
