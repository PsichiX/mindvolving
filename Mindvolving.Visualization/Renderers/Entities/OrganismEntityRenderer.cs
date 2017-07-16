using System;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Engine.Entities;

namespace Mindvolving.Visualization.Renderers.Entities
{
    public class OrganismEntityRenderer : Renderer
    {
        private BodyRenderer bodyRenderer;

        public OrganismEntity Entity { get; private set; }

        public OrganismEntityRenderer(OrganismEntity entity)
        {
            Entity = entity;
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);

            bodyRenderer.Body = Entity.OrganicBody;
            bodyRenderer.Draw(gt);
        }

        public override void Initialize()
        {
            base.Initialize();

            bodyRenderer = Visualization.CreateRenderer<BodyRenderer>();
        }
    }
}
