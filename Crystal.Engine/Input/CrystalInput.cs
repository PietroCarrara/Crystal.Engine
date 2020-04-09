using System.Text;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Numerics;
using Crystal.Framework;
using Crystal.Framework.UI;
using Crystal.Engine.SFML;
using Crystal.Engine.SDL;
using static SDL2.SDL;

namespace Crystal.Engine.Input
{
    public class CrystalInput : Framework.Input
    {
        /// <summary>
        /// Collection of all keyboard keys
        /// </summary>
        private static readonly ReadOnlyCollection<SDL_Scancode> keyboardKeys;

        /// <summary>
        /// Saves, for each key, if it was down
        /// </summary>
        private Dictionary<SDL_Scancode, bool> previousState = new Dictionary<SDL_Scancode, bool>(),
                                               currentState = new Dictionary<SDL_Scancode, bool>();

        private List<SDL_Keycode> pressedKeys = new List<SDL_Keycode>(),
                                   releasedKeys = new List<SDL_Keycode>();

        private List<char> text = new List<char>();

        private Point mousePos;
        public override Vector2 GetMousePositionRaw() => mousePos;

        static CrystalInput()
        {
            // Store the keyboard keys
            var keys = new List<SDL_Scancode>();
            keys.AddRange(((SDL_Scancode[])Enum.GetValues(typeof(SDL_Scancode))).Distinct());
            keyboardKeys = keys.AsReadOnly();
        }

        public CrystalInput()
        {
            // Initialize each key as being up
            foreach (var key in keyboardKeys)
            {
                previousState.Add(key, false);
                currentState.Add(key, false);
            }

            SDLEventManager.KeyDown += onKeyPressed;
            SDLEventManager.KeyUp += onKeyReleased;
            SDLEventManager.TextInput += onTextEntered;
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
            return currentState[button.ToScancode()];
        }

        public override bool WasButtonDown(Buttons button)
        {
            return previousState[button.ToScancode()];
        }

        public void Update()
        {
            SDL_GetMouseState(out this.mousePos.X, out this.mousePos.Y);
        }

        /// <summary>
        /// Saves the current state as being the previous frame
        /// </summary>
        public void AdvanceState()
        {
            foreach (var key in keyboardKeys)
            {
                previousState[key] = currentState[key];
                currentState[key] = false;
            }

            text.Clear();
        }

        private void onKeyPressed(SDL_KeyboardEvent args)
        {
            if (args.repeat == 0)
            {
                this.currentState[args.keysym.scancode] = true;
            }

            this.pressedKeys.Add(args.keysym.sym);
        }

        private void onKeyReleased(SDL_KeyboardEvent args)
        {
            this.currentState[args.keysym.scancode] = false;
            this.releasedKeys.Add(args.keysym.sym);
        }

        private void onTextEntered(SDL_TextInputEvent args)
        {
            var text = "";

            unsafe
            {
                text = Encoding.UTF8.GetString(args.text, 32);
            }

            this.text.AddRange(text);
        }
    }
}