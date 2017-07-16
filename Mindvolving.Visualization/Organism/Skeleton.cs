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

        public Bone CreateBone(BodyPart part1, BodyPart part2)
        {
            Bone bone = new Bone(part1, part2);

            if (Bones.Add(bone))
                return bone;

            return null;
        }
    }
}
