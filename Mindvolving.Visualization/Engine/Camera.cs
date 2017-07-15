using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mindvolving.Visualization.Engine
{
    public class Camera
    {
        private Vector2 position;

        public GraphicsDevice GraphicsDevice { get; private set;}
        public Matrix Projection { get; private set; }
        public Matrix View { get; private set; }
        public Vector2 Position { get { return position; } set { position = value; Update(); } }

        public Camera(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            Projection = Matrix.CreateOrthographicOffCenter(0f, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0f, 0f, 1f);
            View = Matrix.Identity;
        }


        private void Update()
        {
            View = Matrix.CreateTranslation(new Vector3(position, 0));
        }

    }
}
