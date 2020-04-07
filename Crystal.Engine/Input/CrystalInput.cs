using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Numerics;
using Crystal.Framework;
using Crystal.Framework.UI;
using Crystal.Engine.SFML;
using SFML.Window;

namespace Crystal.Engine.Input
{
    public class CrystalInput : Framework.Input
    {
        /// <summary>
        /// Saves, for each key, if it was down last frame
        /// </summary>
        private Dictionary<Keyboard.Key, bool> previousState = new Dictionary<Keyboard.Key, bool>();

        /// <summary>
        /// Collection of all keyboard keys
        /// </summary>
        private readonly ReadOnlyCollection<Keyboard.Key> keyboardKeys;

        public override Vector2 MousePosition => Mouse.GetPosition().ToCrystal();

        public CrystalInput()
        {
            // Store the keyboard keys
            var keys = new List<Keyboard.Key>();
            keys.AddRange(((Keyboard.Key[])Enum.GetValues(typeof(Keyboard.Key))).Distinct());
            this.keyboardKeys = keys.AsReadOnly();

            // Initialize each key as being up
            foreach (var key in keyboardKeys)
            {
                previousState.Add(key, false);
            }
        }

        public override IEnumerable<KeyInputData> GetKeysPressed()
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerable<KeyInputData> GetKeysReleased()
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerable<TextInputData> GetText()
        {
            throw new System.NotImplementedException();
        }

        public override bool IsButtonDown(Buttons button)
        {
            return Keyboard.IsKeyPressed(button.ToSFML());
        }

        public override bool WasButtonDown(Buttons button)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Saves the current state as being the previous frame
        /// </summary>
        public void AdvanceState()
        {
            foreach (var key in keyboardKeys)
            {
                previousState[key] = Keyboard.IsKeyPressed(key);
            }
        }
    }
}