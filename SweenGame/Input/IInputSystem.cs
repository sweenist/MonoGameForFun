using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SweenGame.Input
{
    public delegate void ActionDelegate(object value, GameTime gameTime);
    
    public interface IInputSystem
    {
        List<MappedAction> MappedActions {get; set;}
        void SetAction(string name, ActionDelegate function);
        void Enable();
    }
}