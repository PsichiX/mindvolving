using FarseerPhysics.Dynamics.Joints;
using System;

namespace Mindvolving.Visualization.Organism
{
    public class Bone : IEquatable<Bone>
    {
        public BodyPart Part1 { get; private set; }
        public BodyPart Part2 { get; private set; }
        public Joint Joint { get; set; }

        public Bone(BodyPart part1, BodyPart part2)
        {
            this.Part1 = part1;
            this.Part2 = part2;
        }

        public bool Equals(Bone other)
        {
            return this.Part1 == other.Part1 && this.Part2 == other.Part2;
        }
    }
}
