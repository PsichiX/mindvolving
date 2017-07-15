using System;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Organism;

namespace Mindvolving.Visualization.Renderers
{
    public class MuscleRenderer : IRenderable
    {
        private Body body;

        public MindvolvingVisualization Visualization { get; set; }

        public MuscleRenderer(Body body)
        {
            this.body = body;
        }

        public void Draw(GameTime gt)
        {

        }

        public void Initialize()
        {

        }
    }
}