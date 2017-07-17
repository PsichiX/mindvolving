using FarseerPhysics.Factories;
using Mindvolving.Visualization.Renderers.Entities;

namespace Mindvolving.Visualization.Engine.Entities
{
    public class OrganismEntity : Entity
    {
        public Organisms.Organism Organism { get; set; }

        public OrganismEntity()
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            Renderer = World.Visualization.CreateRenderer(() => new OrganismEntityRenderer(this));
        }
    }
}
