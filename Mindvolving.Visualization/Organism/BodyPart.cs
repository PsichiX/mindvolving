using Microsoft.Xna.Framework;

namespace Mindvolving.Visualization.Organism
{
    public class BodyPart
    {
        private Body body;

        public Vector2 Position { get { return RigidBody.Position.ToVector2(); } }
        public Jitter.Dynamics.RigidBody RigidBody { get; private set; }

        public BodyPart(Body body)
        {
            this.body = body;
        }

        public void AttachMuscle(BodyPart bodyPart)
        {
            body.CreateMuscle(this, bodyPart);
        }

        public bool AttachBone(BodyPart bodyPart)
        {
            return body.Skeleton.CreateBone(this, bodyPart);
        }
    }
}
