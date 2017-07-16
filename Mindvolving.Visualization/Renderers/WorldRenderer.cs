using System;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Engine;

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
            base.Draw(gt);

            for(int i = 0; i < world.Entities.Count; i++)
            {
                world.Entities[i].Renderer.Draw(gt);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}
