using System;

namespace Mindvolving.Visualization.Organism
{
    public class Muscle
    {
        private BodyPart part1;
        private BodyPart part2;

        public Muscle(BodyPart part1, BodyPart part2)
        {
            this.part1 = part1;
            this.part2 = part2;
        }
    }
}
