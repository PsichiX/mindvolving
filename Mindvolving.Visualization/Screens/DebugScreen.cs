using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Mindvolving.Visualization.Renderers;

namespace Mindvolving.Visualization.Screens
{
    public class DebugScreen : Screen
    {
        private DebugViewRenderer debugViewRenderer;
        private MouseState lastMouseState;

        public override void Initialize()
        {
            base.Initialize();

            debugViewRenderer = new DebugViewRenderer(Visualization.World.PhysicalWorld);
            debugViewRenderer.LoadContent(Visualization.GraphicsDevice, Visualization.Content);
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

            if(wheelDelta != 0)
                Visualization.Camera.Scale -= 0.05f * wheelDelta / 120f;

            if (Visualization.InputManager.IsKeyDown("debug-camera-left"))
                Visualization.Camera.Position += new Vector2(2, 0);

            if (Visualization.InputManager.IsKeyDown("debug-camera-right"))
                Visualization.Camera.Position += new Vector2(-2, 0);

            if (Visualization.InputManager.IsKeyDown("debug-camera-down"))
                Visualization.Camera.Position += new Vector2(0, -2);

            if (Visualization.InputManager.IsKeyDown("debug-camera-up"))
                Visualization.Camera.Position += new Vector2(0, 2);

            lastMouseState = Visualization.InputManager.MouseState;
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);

            //debugViewRenderer.BeginCustomDraw(proj, view);
            //debugViewRenderer.DrawSolidCircle(new FPCommon.Vector2(400, 100), 20, new FPCommon.Vector2(0, 0), Color.Red);
            //debugViewRenderer.EndCustomDraw();

            //SpriteBatch.Begin();

            //bodyRenderer.Draw(gameTime);


            //SpriteBatch.End();

            debugViewRenderer.RenderDebugData(Visualization.Camera.Projection, Visualization.Camera.View);
        }
    }
}
