using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mindvolving.Visualization.Engine
{
    public class Camera
    {
        private Vector2 position;
        private float scale;

        public GraphicsDevice GraphicsDevice { get; private set;}
        public Matrix Projection { get; private set; }
        public Matrix View { get; private set; }
        public Vector2 Position { get { return position; } set { position = value; Update(); } }
        public float Scale { get { return scale; } set { scale = value; Update(); } }

        public Camera(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            position = new Vector2(-GraphicsDevice.Viewport.Width / 2f, -GraphicsDevice.Viewport.Height / 2f);
            scale = 1f;

            Projection = Matrix.CreateOrthographicOffCenter(0f, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0f, 0f, 1f);
            View = Matrix.Identity;

            Update();
        }

        public Vector2 ToCamera(Vector2 vec)
        {
            return Vector2.Transform(vec, Matrix.Invert(View));
        }

        public Vector2 ToWorld(Vector2 vec)
        {
            return Vector2.Transform(vec, View);
        }

        private void Update()
        {
            View = Matrix.CreateTranslation(new Vector3(position, 0)) *
                   Matrix.CreateScale(scale) *
                   Matrix.CreateTranslation(new Vector3(GraphicsDevice.Viewport.Width / 2f, GraphicsDevice.Viewport.Height / 2f, 0f));
        }

    }
}
