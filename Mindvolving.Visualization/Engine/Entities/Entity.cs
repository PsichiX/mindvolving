using Microsoft.Xna.Framework;
using System;
using Physics = FarseerPhysics;

namespace Mindvolving.Visualization.Engine.Entities
{
    public abstract class Entity : WorldObject
    {
        public Physics.Dynamics.Body PhysicalBody { get; protected set; }

        public override Vector2 Position
        {
            get { return PhysicalBody?.Position.ToMGVector2() ?? base.Position; }
            set
            {
                if (PhysicalBody != null)
                    PhysicalBody.Position = value.ToFPVector2();
                else
                    base.Position = value;
            }
        }


        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gt)
        {

        }

        public override void Dispose()
        {
            base.Dispose();

            if (PhysicalBody != null)
                PhysicalBody.Dispose();
        }
    }
}
