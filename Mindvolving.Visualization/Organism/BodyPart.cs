using System.Collections.Generic;

namespace Mindvolving.Visualization.Organism
{
    public class BodyPart
    {
        private Body body;

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
