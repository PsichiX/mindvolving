using FarseerPhysics.Dynamics.Joints;
using System;

namespace Mindvolving.Visualization.Organism
{
    public class Muscle
    {
        public BodyPart Part1 { get; private set; }
        public BodyPart Part2 { get; private set; }
        public DistanceJoint Joint { get; set; }
        public float IdleLength { get; set; }
        public float ContractionLength { get; set; }

        public Muscle(BodyPart part1, BodyPart part2)
        {
            Part1 = part1;
            Part2 = part2;
        }
    }
}
