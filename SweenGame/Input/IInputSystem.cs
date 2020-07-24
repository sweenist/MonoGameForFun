using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SweenGame.Enums;

namespace SweenGame.Input
{
    public delegate void ActionDelegate(object value, GameTime gameTime);
    
    public interface IInputSystem
    {
        List<MappedAction> MappedActions {get; set;}
        void SetAction(ActionType actionType, ActionDelegate function);
        void Enable();
    }
}