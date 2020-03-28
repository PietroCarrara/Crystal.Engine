using System;
using System.Numerics;
using Microsoft.Xna.Framework.Input;
using Button = Crystal.Framework.Buttons;

namespace Crystal.Engine.Input
{
    public class CrystalInput : Framework.Input
    {
        private static KeyboardState prevKbState, currKbState;
        private static MouseState prevMouseState, currMouseState;

        public override Vector2 MousePosition => currMouseState.Position.ToVector2().ToCrystal();
        
        public override bool IsButtonDown(Button button) => buttonDown(button, currKbState, currMouseState);

        public override bool WasButtonDown(Button button) => buttonDown(button, prevKbState, prevMouseState);

        private bool buttonDown(Button button, KeyboardState keyboard, MouseState mouse)
        {
            var bt = button.ToMonoGame();

            // Key found
            if (bt != Keys.None)
            {
                return keyboard.IsKeyDown(bt);
            }
            // Key not found, probably a mouse button
            else
            {
                switch(button)
                {
                    case Button.MouseLeft:
                        return mouse.LeftButton == ButtonState.Pressed;
                    case Button.MouseRight:
                        return mouse.RightButton == ButtonState.Pressed;
                    case Button.MouseMiddle:
                        return mouse.MiddleButton == ButtonState.Pressed;
                    case Button.MouseButton4:
                        return mouse.XButton1 == ButtonState.Pressed;
                    case Button.MouseButton5:
                        return mouse.XButton2 == ButtonState.Pressed;
                    case Button.MouseButton6:
                        throw new NotImplementedException();
                    case Button.MouseButton7:
                        throw new NotImplementedException();
                    case Button.MouseButton8:
                        throw new NotImplementedException();
                    default:
                        throw new Exception($"Could not locate button ${button}!");
                }
            }
        }

        public override void Update()
        {
            prevMouseState = currMouseState;
            prevKbState = currKbState;

            currMouseState = Mouse.GetState();
            currKbState = Keyboard.GetState();
        }
    }
}