using System.Collections.Generic;
using System;
using System.Numerics;
using Crystal.Framework.UI;
using Microsoft.Xna.Framework.Input;
using Button = Crystal.Framework.Buttons;
using GameWindow = Microsoft.Xna.Framework.GameWindow;
using TextInputEventArgs = Microsoft.Xna.Framework.TextInputEventArgs;

namespace Crystal.Engine.Input
{
    public class CrystalInput : Framework.Input
    {
        private KeyboardState prevKbState, currKbState;
        private MouseState prevMouseState, currMouseState;

        // Buffers to handle text input events
        private List<TextInputData> textPrimaryBuffer = new List<TextInputData>(),
                                    textSecondaryBuffer = new List<TextInputData>();

        // Buffers to handle keys input events
        private List<KeyInputData> controlPrimaryBuffer = new List<KeyInputData>(),
                                   controlSecondaryBuffer = new List<KeyInputData>();

        private KeyRepeater repeater;

        public override Vector2 MousePosition => currMouseState.Position.ToVector2().ToCrystal();

        public override bool IsButtonDown(Button button) => buttonDown(button, currKbState, currMouseState);

        public override bool WasButtonDown(Button button) => buttonDown(button, prevKbState, prevMouseState);

        public CrystalInput(GameWindow window)
        {
            window.TextInput += this.onText;

            this.repeater = new KeyRepeater(new List<Keys>
            {
                Keys.Left,
                Keys.Right,
                Keys.Up,
                Keys.Down,
            }, 0.5f, 20);
        }

        public override IEnumerable<TextInputData> GetText()
        {
            return textPrimaryBuffer;
        }

        public override IEnumerable<KeyInputData> GetKeysPressed()
        {
            return controlPrimaryBuffer;
        }

        public override IEnumerable<KeyInputData> GetKeysReleased()
        {
            // TODO: Implement
            return new KeyInputData[0];
        }

        public override void Update(float delta)
        {
            // Swap and clear text buffers
            var tmpTextBuf = textPrimaryBuffer;
            textPrimaryBuffer = textSecondaryBuffer;
            textSecondaryBuffer = tmpTextBuf;
            textSecondaryBuffer.Clear();

            // Swap and clear control buffers
            var tmpControlBuf = controlPrimaryBuffer;
            controlPrimaryBuffer = controlSecondaryBuffer;
            controlSecondaryBuffer = tmpControlBuf;
            controlSecondaryBuffer.Clear();

            prevMouseState = currMouseState;
            prevKbState = currKbState;

            currMouseState = Mouse.GetState();
            currKbState = Keyboard.GetState();

            foreach (var key in this.repeater.Update(currKbState, delta))
            {
                this.controlSecondaryBuffer.Add(new KeyInputData(key.ToCrystal()));
            }
        }

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

        private void onText(object sender, TextInputEventArgs e)
        {
            if (char.IsControl(e.Character))
            {
                this.controlSecondaryBuffer.Add(new KeyInputData(e.Key.ToCrystal()));
            }
            else
            {
                this.textSecondaryBuffer.Add(new TextInputData(e.Character, e.Key.ToCrystal()));
            }
        }
    }
}