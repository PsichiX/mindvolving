using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Renderers.Enviroment;
using System;

namespace Mindvolving.Visualization.Engine.Enviroment
{
    public class Bubble : Decal
    {
        private float timeAccumulator;
        private float timeAccumulatorFactor;

        public float Size { get; set; }

        public Bubble()
        {
            Size = 1;
        }

        public override void Initialize()
        {
            base.Initialize();

            Renderer = World.Visualization.CreateRenderer(() => new BubbleRenderer(this));

            timeAccumulatorFactor = (float)(MindvolvingVisualization.Random.NextDouble() * 0.9 + 0.1);
            timeAccumulator = (float)(MindvolvingVisualization.Random.NextDouble() * Math.PI);
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);

            timeAccumulator += (float)gt.ElapsedGameTime.TotalSeconds * timeAccumulatorFactor;

            Position += new Vector2(0, (float)MindvolvingVisualization.Random.NextDouble()  * (-0.2f) * (MindvolvingVisualization.Random.Next(0, 3) == 0 ? -1 : 1)) * (timeAccumulator % 1 + 0.01f);
            Position += new Vector2((float)MindvolvingVisualization.Random.NextDouble() * (-0.1f), 0) * (float)Math.Sin(timeAccumulator);

        }
    }
}
