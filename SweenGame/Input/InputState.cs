using Microsoft.Xna.Framework.Input;

namespace SweenGame.Input
{
    public class InputState
    {
        public KeyboardState CurrentKeyBoardState;
        public KeyboardState LastKeyBoardState;

        public bool MenuUp => IsNewKeyPress(Keys.Up);

        public bool MenuDown => IsNewKeyPress(Keys.Down);

        public bool MenuSelect => IsNewKeyPress(Keys.Space) || IsNewKeyPress(Keys.Enter);
        public bool MenuCancel => IsNewKeyPress(Keys.Escape);
        public bool PauseGame => IsNewKeyPress(Keys.Escape);

        public void Update()
        {
            LastKeyBoardState = CurrentKeyBoardState;
            CurrentKeyBoardState = Keyboard.GetState();
        }

        public bool IsNewKeyPress(Keys key)
        {
            return CurrentKeyBoardState.IsKeyDown(key) && LastKeyBoardState.IsKeyUp(key);
        }

        public bool IsKeyDown(Keys key) => CurrentKeyBoardState.IsKeyDown(key);
        public bool IsNewKeyUp(Keys key)
        {
            return CurrentKeyBoardState.IsKeyUp(key) && LastKeyBoardState.IsKeyDown(key);
        }
    }
}