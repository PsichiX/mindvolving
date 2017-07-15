using System.Collections.Generic;

namespace Mindvolving.Visualization.Organism
{
    public class Skeleton
    {
        public HashSet<Bone> Bones { get; private set; }

        public Skeleton()
        {
            Bones = new HashSet<Bone>();
        }

        public bool CreateBone(BodyPart part1, BodyPart part2)
        {
            return Bones.Add(new Bone(part1, part2));
        }
    }
}
