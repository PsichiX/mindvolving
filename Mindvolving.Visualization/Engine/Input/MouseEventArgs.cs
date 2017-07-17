using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Mindvolving.Visualization.Engine.Input
{
    public class MouseEventArgs : EventArgs
    {
        public MouseButtons Button { get; private set; }
        public MouseState State { get; private set; }

        public MouseEventArgs(MouseButtons left, MouseState mouseState)
        {
            Button = left;
            State = mouseState;
        }

        public Vector2 GetPosition(Camera camera)
        {
            return Vector2.Transform(State.Position.ToVector2(), Matrix.Invert(camera.View));
        }
    }
}