using System;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace Mindvolving.Visualization.Renderers
{
    public class WorldRenderer : Renderer
    {
        private World world;

        public WorldRenderer(World world)
        {
            this.world = world;
        }

        public override void Draw(GameTime gt)
        {
            Visualization.SpriteBatch.Begin();
            base.Draw(gt);

            for(int i = 0; i < world.Entities.Count; i++)
            {
                world.Entities[i].Renderer.Draw(gt);
            }
            Visualization.SpriteBatch.End();

            Visualization.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            for (int i = 0; i < world.Decals.Count; i++)
            {
                world.Decals[i].Renderer.Draw(gt);
            }
            Visualization.SpriteBatch.End();
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}
