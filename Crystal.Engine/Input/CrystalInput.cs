using Microsoft.Xna.Framework.Input;
using Crystal.Framework.Input;
using Crystal.Framework;

namespace Crystal.Engine.Input
{
    public class CrystalInput : IInput
    {
        private ActionPool actions;
        
        private KeyboardState prevKbState, currKbState;
        private MouseState prevMouseState, currMouseState;

        public CrystalInput(ActionPool ap)
        {
            this.actions = ap;
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
            throw new System.NotImplementedException();
        }

        public Vector2 GetMousePosition()
        {
            return new Vector2
            {
                X = currMouseState.X,
                Y = currMouseState.Y,
            };
        }

        public bool IsActionDown(string action)
        {
            var act = this.actions.Get(action);

            foreach (var key in act.Keys)
            {
                if (currKbState.IsKeyUp(key))
                {
                    return false;
                }
            }

            foreach (var mb in act.MouseButtons)
            {
                if (currMouseState.IsButtonUp(mb))
                {
                    return false;
                }
            }

            // TODO: Gamepad buttons

            return true;
        }

        public bool IsActionUp(string action)
        {
            return !this.IsActionDown(action);
        }

        public bool IsActionPressed(string action)
        {
            throw new System.NotImplementedException();
        }

        public bool IsActionReleased(string action)
        {
            throw new System.NotImplementedException();
        }
    }
}