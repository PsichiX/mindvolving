using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Physics = FarseerPhysics;

namespace Mindvolving.Visualization.Engine.Entities
{
    public abstract class Entity : IWorldElement, IDisposable
    {
        private Vector2 position;
        private Physics.Dynamics.Body physicalBody;

        public World World { get; set; }

        public Physics.Dynamics.Body PhysicalBody { get { return physicalBody; } protected set { SetPhysicalBody(value); } }
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

        public Entity()
        {

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

        protected virtual void OnCollision(CollisionEventArgs e)
        {

        }

        protected virtual void OnUndentifiedCollision(CollisionEventArgs e)
        {

        }

        private void SetPhysicalBody(Physics.Dynamics.Body physicalBody)
        {
            if (this.physicalBody != null)
                this.physicalBody.OnCollision -= PhysicalBody_OnCollision;

            this.physicalBody = physicalBody;
            this.physicalBody.OnCollision += PhysicalBody_OnCollision;
        }

        private bool PhysicalBody_OnCollision(Physics.Dynamics.Fixture fixtureA, Physics.Dynamics.Fixture fixtureB, Physics.Dynamics.Contacts.Contact contact)
        {
            var me = fixtureA.UserData == this ? fixtureA : (fixtureB.UserData == this ? fixtureB : null);
            var other = me == fixtureA ? fixtureB : (me == fixtureB ? fixtureA : null);

            CollisionEventArgs args;

            if (me != null && other != null)
            {
                args = new CollisionEventArgs(me.UserData, other.UserData, me, other, contact);
                OnCollision(args);

                return args.Result;
            }

            args = new CollisionEventArgs(null, null, me, other, contact);
            OnCollision(args);

            return args.Result;
        }
    }
}
