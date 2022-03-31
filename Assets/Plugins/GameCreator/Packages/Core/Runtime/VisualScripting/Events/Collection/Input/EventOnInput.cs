using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("On Input")]
    [Category("Input/On Input")]
    [Description("Detects when a button is interacted with")]
    
    [Image(typeof(IconButton), ColorTheme.Type.Yellow)]
    [Keywords("Down", "Up", "Press", "Release")]
    [Keywords("Keyboard", "Mouse", "Button", "Gamepad", "Controller", "Joystick")]

    [Serializable]
    public class EventOnInput : TEventInput
    {
        protected override void OnPerform()
        {
            base.OnPerform();
            this.Execute();
        }
    }
}