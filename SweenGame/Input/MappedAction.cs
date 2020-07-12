using SweenGame.Enums;

namespace SweenGame.Input
{
    public class MappedAction
    {
        private string _name;
        private Control _control;
        private ActionType _actionType;
        private ActionDelegate _function;

        public MappedAction(string name, Control control, ActionType actionType, ActionDelegate function)
        {
            Name = name;
            Control = control;
            ActionType = actionType;
            Function = function;
        }

        public string Name { get => _name; set => _name = value; }
        public Control Control { get => _control; set => _control = value; }
        public ActionType ActionType { get => _actionType; set => _actionType = value; }
        public ActionDelegate Function { get => _function; set => _function = value; }
    }
}