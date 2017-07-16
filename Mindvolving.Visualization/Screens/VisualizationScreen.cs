using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Mindvolving.Visualization.Renderers;

namespace Mindvolving.Visualization.Screens
{
    public class VisualizationScreen : Screen
    {
        private WorldRenderer worldRenderer;
        private MouseState lastMouseState;

        public override void Initialize()
        {
            base.Initialize();

            worldRenderer = new WorldRenderer(Visualization.World);
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);

            Visualization.World.Update(gt);

            MouseState currentMouseState = Visualization.InputManager.MouseState;

            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                Visualization.Camera.Position -= (lastMouseState.Position - currentMouseState.Position).ToVector2() / Visualization.Camera.Scale;
            }

            int wheelDelta = lastMouseState.ScrollWheelValue - currentMouseState.ScrollWheelValue;

            if (wheelDelta != 0)
                Visualization.Camera.Scale -= 0.05f * wheelDelta / 120f;

            lastMouseState = Visualization.InputManager.MouseState;
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);

            Visualization.SpriteBatch.Begin();
            worldRenderer.Draw(gt);
            Visualization.SpriteBatch.End();
        }
    }
}
