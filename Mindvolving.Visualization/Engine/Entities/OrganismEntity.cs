using FarseerPhysics.Factories;
using Mindvolving.Visualization.Renderers.Entities;

namespace Mindvolving.Visualization.Engine.Entities
{
    public class OrganismEntity : Entity
    {
        public Organism.Body OrganicBody { get; set; }

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
