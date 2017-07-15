using System;

namespace Mindvolving.Visualization.Organism
{
    public class Muscle
    {
        public BodyPart Part1 { get; private set; }
        public BodyPart Part2 { get; private set; }

        public Muscle(BodyPart part1, BodyPart part2)
        {
            this.Part1 = part1;
            this.Part2 = part2;
        }
    }
}
