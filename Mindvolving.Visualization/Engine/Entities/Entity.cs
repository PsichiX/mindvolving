using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Physics = FarseerPhysics;

namespace Mindvolving.Visualization.Engine.Entities
{
    public abstract class Entity : WorldObject
    {
        private Physics.Dynamics.Body physicalBody;

        public Physics.Dynamics.Body PhysicalBody { get { return physicalBody; } protected set { SetPhysicalBody(value); } }

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

        public Entity()
        {

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
            var userDataA = fixtureA.UserData as Organisms.Physics.IPhysicsUserData;
            var userDataB = fixtureB.UserData as Organisms.Physics.IPhysicsUserData;

            var me = userDataA.CustomData == this ? fixtureA : (userDataB.CustomData == this ? fixtureB : null);
            var other = me == fixtureA ? fixtureB : (me == fixtureB ? fixtureA : null);

            CollisionEventArgs args;

            if (me != null && other != null)
            {
                args = new CollisionEventArgs(((Organisms.Physics.IPhysicsUserData)me.UserData).CustomData, ((Organisms.Physics.IPhysicsUserData)other.UserData).CustomData, me, other, contact);
                OnCollision(args);

                return args.Result;
            }

            args = new CollisionEventArgs(null, null, me, other, contact);
            OnCollision(args);

            return args.Result;
        }
    }
}
