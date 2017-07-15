using System.Collections.Generic;

namespace Mindvolving.Visualization.Organism
{
    public class Body
    {
        public List<Muscle> Muscles { get; private set; }

        public Skeleton Skeleton { get; private set; }

        public Body()
        {
            Skeleton = new Skeleton();
            Muscles = new List<Muscle>();
        }

        public void CreateMuscle(BodyPart part1, BodyPart part2)
        {
            Muscles.Add(new Muscle(part1, part2));
        }
    }
}
