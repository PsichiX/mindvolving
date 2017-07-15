using Microsoft.Xna.Framework;
using System;
using Physics = FarseerPhysics;

namespace Mindvolving.Visualization.Engine.Entities
{
    public abstract class Entity : IWorldElement, IDisposable
    {
        private Vector2 position;

        public World World { get; set; }

        public Physics.Dynamics.Body PhysicalBody { get; protected set; }
        public IRenderable Renderer { get; protected set; }
        public bool IsDestroyed { get; private set; }

        public Vector2 Position
        {
            get { return PhysicalBody?.Position.ToMGVector2() ?? position; }
            set
            {
                if (PhysicalBody != null)
                    PhysicalBody.Position = value.ToFPVector2();
                else
                    position = value;
            }
        }


        public virtual void Initialize()
        {

        }

        public virtual void Update(GameTime gt)
        {

        }

        public void Destroy()
        {
            IsDestroyed = true;
            Dispose();
        }

        public void Dispose()
        {
            if (!IsDestroyed)
                IsDestroyed = true;
            else
                return;

            if (PhysicalBody != null)
                PhysicalBody.Dispose();
        }
    }
}
