using System;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Engine.Entities;

namespace Mindvolving.Visualization.Renderers.Entities
{
    public class OrganismEntityRenderer : IRenderable
    {
        private BodyRenderer bodyRenderer;

        public MindvolvingVisualization Visualization { get; set; }
        public OrganismEntity Entity { get; private set; }

        public OrganismEntityRenderer(OrganismEntity entity)
        {
            Entity = entity;
            bodyRenderer = new BodyRenderer();
        }

        public void Draw(GameTime gt)
        {
            bodyRenderer.Draw(gt);
        }

        public void Initialize()
        {
            bodyRenderer.Body = Entity.OrganicBody;
        }
    }
}
