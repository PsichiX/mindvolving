using System;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Organism;

namespace Mindvolving.Visualization.Renderers
{
    public class BodyPartRenderer : IRenderable
    {
        private Body body;

        public MindvolvingVisualization Visualization { get; set; }
        public BodyPart Current { get; set; }

        public BodyPartRenderer(Body body)
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