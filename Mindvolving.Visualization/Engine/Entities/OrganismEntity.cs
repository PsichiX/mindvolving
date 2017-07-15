using FarseerPhysics.Factories;

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
        }
    }
}
