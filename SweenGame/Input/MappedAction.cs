using SweenGame.Enums;

namespace SweenGame.Input
{
    public class MappedAction
    {
        private Control _control;
        private ActionType _actionType;
        private ActionDelegate _function;

        public MappedAction(ActionType actionType, Control control, ActionDelegate function)
        {
            Control = control;
            ActionType = actionType;
            Function = function;
        }

        public Control Control { get => _control; set => _control = value; }
        public ActionType ActionType { get => _actionType; set => _actionType = value; }
        public ActionDelegate Function { get => _function; set => _function = value; }
    }
}