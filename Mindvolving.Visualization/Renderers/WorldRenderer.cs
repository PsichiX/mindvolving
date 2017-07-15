using System;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Engine;

namespace Mindvolving.Visualization.Renderers
{
    public class WorldRenderer : IRenderable
    {
        private World world;

        public MindvolvingVisualization Visualization { get; set; }

        public WorldRenderer(World world)
        {
            this.world = world;
        }

        public void Draw(GameTime gt)
        {
            for(int i = 0; i < world.Entities.Count; i++)
            {
                world.Entities[i].Renderer.Draw(gt);
            }
        }

        public void Initialize()
        {

        }
    }
}
