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

        private List<Keyboard.Key> pressedKeys = new List<Keyboard.Key>(),
                                   releasedKeys = new List<Keyboard.Key>();

        private List<char> text = new List<char>();

        /// <summary>
        /// Collection of all keyboard keys
        /// </summary>
        private static readonly ReadOnlyCollection<Keyboard.Key> keyboardKeys;

        public override Vector2 GetMousePositionRaw() => Mouse.GetPosition().ToCrystal();

        static CrystalInput()
        {
            // Store the keyboard keys
            var keys = new List<Keyboard.Key>();
            keys.AddRange(((Keyboard.Key[])Enum.GetValues(typeof(Keyboard.Key))).Distinct());
            keyboardKeys = keys.AsReadOnly();
        }

        public CrystalInput(Window window)
        {
            // Initialize each key as being up
            foreach (var key in keyboardKeys)
            {
                previousState.Add(key, false);
            }

            window.KeyPressed += onKeyPressed;
            window.KeyReleased += onKeyReleased;
            window.TextEntered += onTextEntered;
        }

        public override IEnumerable<KeyInputData> GetKeysPressed()
        {
            return pressedKeys.Select(k => new KeyInputData(k.ToCrystal()));
        }

        public override IEnumerable<KeyInputData> GetKeysReleased()
        {
            return releasedKeys.Select(k => new KeyInputData(k.ToCrystal()));
        }

        public override IEnumerable<TextInputData> GetText()
        {
            return text.Select(c => new TextInputData(c));
        }

        public override bool IsButtonDown(Buttons button)
        {
            return Keyboard.IsKeyPressed(button.ToSFML());
        }

        public override bool WasButtonDown(Buttons button)
        {
            return previousState[button.ToSFML()];
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

            pressedKeys.Clear();
            releasedKeys.Clear();
            text.Clear();
        }

        private void onKeyPressed(object sender, KeyEventArgs args)
        {
            this.pressedKeys.Add(args.Code);
            Console.WriteLine(args);
        }

        private void onKeyReleased(object sender, KeyEventArgs args)
        {
            this.releasedKeys.Add(args.Code);
        }

        private void onTextEntered(object sender, TextEventArgs args)
        {
            this.text.AddRange(args.Unicode.Where(c => !char.IsControl(c)));
        }
    }
}