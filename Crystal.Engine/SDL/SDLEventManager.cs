using static SDL2.SDL;

namespace Crystal.Engine.SDL
{
    public static class SDLEventManager
    {
        public delegate void KeyboardEventHandler(SDL_KeyboardEvent e);
        public delegate void TextInputEventHandler(SDL_TextInputEvent e);

        public static event KeyboardEventHandler KeyDown, KeyUp;
        public static event TextInputEventHandler TextInput;

        public static void Update()
        {
            while (SDL_PollEvent(out var e) == 1)
            {
                switch(e.type)
                {
                    case SDL_EventType.SDL_KEYDOWN:
                        KeyDown?.Invoke(e.key);
                        break;
                    case SDL_EventType.SDL_KEYUP:
                        KeyUp?.Invoke(e.key);
                        break;
                    case SDL_EventType.SDL_TEXTINPUT:
                        TextInput?.Invoke(e.text);
                        break;
                }
            }
        }
    }
}