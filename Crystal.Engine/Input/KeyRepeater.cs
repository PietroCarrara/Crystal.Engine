using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Crystal.Engine.Input
{
    public class KeyRepeater
    {
        private readonly List<RepeatingKey> keys = new List<RepeatingKey>();

        public KeyRepeater(IEnumerable<Keys> keys, float repeatDelay, float keysPerSecond)
        {
            foreach (var key in keys)
            {
                this.keys.Add(new RepeatingKey
                {
                    Key = key,
                    RepeatDelay = repeatDelay,
                    RepeatFrequency = keysPerSecond,
                });
            }
        }

        public IEnumerable<Keys> Update(KeyboardState input, float delta)
        {
            foreach (var key in keys)
            {
                if (key.Update(delta, input.IsKeyDown(key.Key)))
                {
                    yield return key.Key;
                }
            }
        }

        private class RepeatingKey
        {
            /// <summary>
            /// The key to repeat
            /// </summary>
            public Keys Key;

            /// <summary>
            /// Delay in seconds between the first press and the repetition start
            /// </summary>
            public float RepeatDelay;

            /// <summary>
            /// How many keys per second should we input
            /// </summary>
            public float RepeatFrequency;

            /// <summary>
            /// Timer that counts how many seconds have passed since the first press
            /// </summary>
            private float timer;

            /// <summary>
            /// Updates this key
            /// </summary>
            /// <param name="delta">Delta time</param>
            /// <param name="isDown">Is this key down?</param>
            /// <returns>True the first press or a repetition was triggered</returns>
            public bool Update(float delta, bool isDown)
            {
                if (!isDown)
                {
                    timer = 0;
                    return false;
                }

                if (timer == 0)
                {
                    timer += delta;
                    return true;
                }

                timer += delta;

                if (timer > RepeatDelay)
                {
                    var timeBetweenRepeats = 1 / RepeatFrequency;

                    if (timer - RepeatDelay > timeBetweenRepeats)
                    {
                        timer -= timeBetweenRepeats;
                        return true;
                    }
                }

                return false;
            }
        }
    }
}