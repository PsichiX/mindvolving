using Physics = FarseerPhysics;

namespace Mindvolving.Visualization.Engine.Entities
{
    public class Entity : IWorldElement
    {
        public World World { get; set; }

        public Physics.Dynamics.Body PhysicalBody { get; private set; }
        
        public virtual void Initialize()
        {

        }
    }
}
