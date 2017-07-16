using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using System;

namespace Mindvolving.Visualization.Engine
{
    public class CollisionEventArgs : EventArgs
    {
        public object Collidee { get; private set; }
        public object Collider { get; private set; }
        public Fixture CollideeFixture { get; private set; }
        public Fixture ColliderFixture { get; private set; }
        public Contact Contact { get; private set; }
        public bool Result { get; set; }

        public CollisionEventArgs(object collidee, object collider, Fixture collideeFixture, Fixture colliderFixture, Contact contact)
        {
            Collidee = collidee;
            Collider = collider;
            CollideeFixture = collideeFixture;
            ColliderFixture = colliderFixture;
            Contact = contact;
            Result = false;
        }
    }
}
