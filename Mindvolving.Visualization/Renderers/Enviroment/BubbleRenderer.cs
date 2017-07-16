using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mindvolving.Visualization.Engine.Enviroment;

namespace Mindvolving.Visualization.Renderers.Enviroment
{
    public class BubbleRenderer : Renderer
    {
        public Bubble Bubble { get; set; }

        public BubbleRenderer(Bubble bubble)
        {
            Bubble = bubble;
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);

            Visualization.SpriteBatch.Draw(Visualization.Textures.Bubble, Bubble.Position, null, Color.White, 0f, Vector2.Zero, 0.2f * Bubble.Size, SpriteEffects.None, 0f);
        }
    }
}
