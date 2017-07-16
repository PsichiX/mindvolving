using FarseerPhysics.Common;
using FarseerPhysics.Dynamics.Joints;
using System;

namespace Mindvolving.Organisms
{
	public class Muscle : IDisposable
	{
		public Organism Organism { get; private set; }

		public Organ From { get; private set; }

		public Organ To { get; private set; }

		public DistanceJoint Joint { get; private set; }

		// <0; 1>
		public float ContractionFactor { get; private set; }

		public Muscle(Organism organism, Organ from, Organ to, float fromRadialPosition, float toRadialPosition, float contractionFactor)
		{
			Organism = organism;
			From = from;
			To = to;
			ContractionFactor = contractionFactor;
			var angle = fromRadialPosition * Math.PI / 180.0;
			var x = (float)Math.Cos(angle) * from.Radius;
			var y = (float)Math.Sin(angle) * from.Radius;
			var anchorFrom = new Vector2(x, y);
			angle = toRadialPosition * Math.PI / 180.0;
			x = (float)Math.Cos(angle) * to.Radius;
			y = (float)Math.Sin(angle) * to.Radius;
			var anchorTo = new Vector2(x, y);
			Joint = new DistanceJoint(from.Body, to.Body, anchorFrom, anchorTo) { DampingRatio = 1, Frequency = 0.9f };
			organism.World.AddJoint(Joint);
		}

		~Muscle()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (Joint != null && Organism != null)
				Organism.World.RemoveJoint(Joint);
			Organism = null;
			From = null;
			To = null;
			Joint = null;
			ContractionFactor = 0;
		}
	}
}
