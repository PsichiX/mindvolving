using Microsoft.Xna.Framework;
using Physics = FarseerPhysics;

namespace Mindvolving.Visualization.Engine.Entities
{
    public abstract class Entity : IWorldElement
    {
        public World World { get; set; }

        public Physics.Dynamics.Body PhysicalBody { get; private set; }
        
        public virtual void Initialize()
        {

        }

        public virtual void Update(GameTime gt)
        {

        }
    }
}
