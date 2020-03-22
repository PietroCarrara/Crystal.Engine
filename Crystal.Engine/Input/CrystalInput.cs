using System.Numerics;
using Microsoft.Xna.Framework.Input;
using Crystal.Framework;
using Crystal.Framework.Input;

namespace Crystal.Engine.Input
{
    public class CrystalInput : IInput
    {
        private readonly ActionPool actions;
        private readonly Scene scene;
        private readonly CrystalGame game;

        // Keys should be synced for every input object
        private static KeyboardState prevKbState, currKbState;
        private static MouseState prevMouseState, currMouseState;

        public CrystalInput(CrystalGame game, Scene scene)
        {
            this.scene = scene;
            this.game = game;

            this.actions = game.Config.Actions;
        }

        public void Update()
        {
            this.UpdateKeyboard(Keyboard.GetState());
            this.UpdateMouse(Mouse.GetState());
            // TODO: Gamepad
        }

        public void UpdateKeyboard(KeyboardState state)
        {
            prevKbState = currKbState;
            currKbState = state;
        }

        public void UpdateMouse(MouseState state)
        {
            prevMouseState = currMouseState;
            currMouseState = state;
        }

        public float GetActionStrength(string action)
        {
            var act = this.actions.Get(action);

            var strength = 0f;
            var totalButtons = 0;

            foreach (var key in act.Keys)
            {
                if (currKbState.IsKeyDown(key))
                {
                    strength += 1;
                    totalButtons++;
                }
            }

            foreach (var bt in act.MouseButtons)
            {
                if (bt == MouseButtons.Middle)
                {
                    strength += currMouseState.ScrollWheelValue - prevMouseState.ScrollWheelValue;
                    totalButtons++;
                }
                else if (currMouseState.IsButtonDown(bt))
                {
                    strength += 1;
                    totalButtons++;
                }
            }

            // TODO: Gamepad

            return strength / totalButtons;
        }

        public Vector2 GetMousePosition()
        {
            // TODO: Scale the coordinates to match the design space
            return new Vector2(currMouseState.Position.X, currMouseState.Y);
        }

        public bool IsActionDown(string action)
        {
            return this.IsActionDown(action, currKbState, currMouseState);
        }

        public bool IsActionDown(string action, KeyboardState kState, MouseState mState)
        {
            var act = this.actions.Get(action);

            foreach (var key in act.Keys)
            {
                if (kState.IsKeyUp(key))
                {
                    return false;
                }
            }

            foreach (var mb in act.MouseButtons)
            {
                if (mState.IsButtonUp(mb))
                {
                    return false;
                }
            }

            // TODO: Gamepad buttons

            return true;
        }

        public bool IsActionUp(string action)
        {
            return this.IsActionUp(action, currKbState, currMouseState);
        }

        public bool IsActionUp(string action, KeyboardState kState, MouseState mState)
        {
            return !this.IsActionDown(action, kState, mState);
        }

        public bool IsActionPressed(string action)
        {
            return this.IsActionUp(action, prevKbState, prevMouseState) &&
                   this.IsActionDown(action, currKbState, currMouseState);
        }

        public bool IsActionReleased(string action)
        {
            return this.IsActionDown(action, prevKbState, prevMouseState) &&
                   this.IsActionUp(action, currKbState, currMouseState);
        }
    }
}