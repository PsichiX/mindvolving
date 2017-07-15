using System.Collections.Generic;

namespace Mindvolving.Visualization.Organism
{
    public class Body
    {
        public List<Muscle> Muscles { get; private set; }
        public List<BodyPart> BodyParts { get; private set; }
        public Skeleton Skeleton { get; private set; }

        public Jitter.Dynamics.RigidBody RigidBody { get; private set; }

        public Body()
        {
            BodyParts = new List<BodyPart>();
            Skeleton = new Skeleton();
            Muscles = new List<Muscle>();
        }

        public BodyPart CreateBodyPart()
        {
            BodyPart part = new BodyPart(this);
            BodyParts.Add(part);
            return part;
        }

        public void CreateMuscle(BodyPart part1, BodyPart part2)
        {
            Muscles.Add(new Muscle(part1, part2));
        }
    }
}
