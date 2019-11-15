using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Crystal.Engine.Input
{
    public class CrystalInputAction
    {            
        public List<Keys> Keys = new List<Keys>();
        public List<MouseButtons> MouseButtons = new List<MouseButtons>();
        public List<Buttons> GamepadButtons = new List<Buttons>();

        /// <summary>
        /// Takes a sequence of strings and converts them into a single action.
        /// </summary>
        /// <param name="keys">
        /// The keys to be used in the action. Should be in the form:
        /// x, where x is a monogame Keys value
        /// keys.x, where x is a monogame Keys value
        /// mouse.x, where x is a crystal MouseButtons value
        /// TODO: gamepad.x
        /// 
        /// The prefix is case-insensitive, but the key is not
        /// </param>
        public static CrystalInputAction ParseAction(string[] keys)
        {
            var action = new CrystalInputAction();

            foreach (var key in keys)
            {
                addUnparsedKeyToAction(action, key);
            }

            return action;
        }

        private static void addUnparsedKeyToAction(CrystalInputAction action, string key)
        {
            var parts = key.Split('.', 2);
            
            // No "keys.", "mouse." or "gamepad." informed
            // Assume default "keys."
            if (parts.Length <= 1) {
                parts = new string[] { "keys", parts[0] };
            }

            switch (parts[0].ToLower())
            {
                case "keys":
                    action.Keys.Add((Keys)Enum.Parse(typeof(Keys), parts[1]));
                break;
                case "mouse":
                    action.MouseButtons.Add((MouseButtons)Enum.Parse(typeof(MouseButtons), parts[1]));
                break;
                case "gamepad":
                    // TODO
                break;
                default:
                    throw new Exception($"Unexpected key type \"{parts[0]}\" while parsing actions!");
            }
        }
    }
}