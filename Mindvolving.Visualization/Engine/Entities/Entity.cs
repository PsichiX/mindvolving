using Microsoft.Xna.Framework;
using Physics = FarseerPhysics;

namespace Mindvolving.Visualization.Engine.Entities
{
    public abstract class Entity : IWorldElement
    {
        public World World { get; set; }

        public Physics.Dynamics.Body PhysicalBody { get; private set; }
        public IRenderable Renderer { get; protected set; }
        public Vector2 Position { get { return PhysicalBody.Position.ToMGVector2(); } }

        public virtual void Initialize()
        {

        }

        public virtual void Update(GameTime gt)
        {

        }
    }
}
