using Microsoft.Xna.Framework.Input;
using System;

namespace Mindvolving.Visualization.Engine.Input
{
    public class KeyEventArgs : EventArgs
    {
        public Keys Key { get; private set; }
        public KeyboardState State { get; private set; }

        public KeyEventArgs(Keys key, KeyboardState state)
        {
            Key = key;
            State = state;
        }
    }
}
