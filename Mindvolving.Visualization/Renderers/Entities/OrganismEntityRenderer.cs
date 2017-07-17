using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Engine.Entities;
using Mindvolving.Visualization.Renderers.Organisms;

namespace Mindvolving.Visualization.Renderers.Entities
{
    public class OrganismEntityRenderer : Renderer
    {
        private OrganismRenderer organismRenderer;

        public OrganismEntity Entity { get; private set; }

        public OrganismEntityRenderer(OrganismEntity entity)
        {
            Entity = entity;
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);

            organismRenderer.Organism = Entity.Organism;
            organismRenderer.Draw(gt);
        }

        public override void Initialize()
        {
            base.Initialize();

            organismRenderer = Visualization.CreateRenderer<OrganismRenderer>();
        }
    }
}
