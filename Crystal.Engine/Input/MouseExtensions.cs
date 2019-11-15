using System;
using Microsoft.Xna.Framework.Input;

namespace Crystal.Engine.Input
{
    public enum MouseButtons
    {
        Left,
        Right,
        Middle,
        None
    }

    public static class MouseExtensions
    {
        public static bool IsButtonDown(this MouseState self, MouseButtons button)
        {
            ButtonState state;

            switch (button)
            {
                case MouseButtons.Left:
                    state = self.LeftButton;
                    break;
                case MouseButtons.Right:
                    state = self.RightButton;
                    break;
                case MouseButtons.Middle:
                    state = self.MiddleButton;
                    break;
                default:
                    throw new System.Exception($"Unexpected Crystal.Engine.Input.MouseButton value: \"{button}\"!");
            }

            return state == ButtonState.Pressed;
        }

        public static bool IsButtonUp(this MouseState self, MouseButtons button)
        {
            return !self.IsButtonDown(button);
        }

        public static MouseButtons MouseButtonFromString(string bt)
        {
            throw new NotImplementedException();
        }
    }
}