using System;

namespace Mindvolving.Visualization.Organism
{
    public class Bone : IEquatable<Bone>
    {
        private BodyPart part1;
        private BodyPart part2;

        public Bone(BodyPart part1, BodyPart part2)
        {
            this.part1 = part1;
            this.part2 = part2;
        }

        public bool Equals(Bone other)
        {
            return this.part1 == other.part1 && this.part2 == other.part2;
        }
    }
}
