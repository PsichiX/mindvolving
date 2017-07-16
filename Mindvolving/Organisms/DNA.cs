using System;
using System.Collections.Generic;

namespace Mindvolving.Organisms
{
	public class DNA
	{
		private class MuscleComparer : IEqualityComparer<Muscle>
		{
			public bool Equals(Muscle x, Muscle y)
			{
				return x == y;
			}

			public int GetHashCode(Muscle obj)
			{
				var from = obj.From.GetHashCode();
				var to = obj.To.GetHashCode();
				var min = Math.Min(from, to);
				var max = Math.Max(from, to);
				var hash = 17;
				hash = hash * 31 + min;
				hash = hash * 31 + max;
				return hash;
			}
		}

		public class Organ
		{
			public Guid UID { get; private set; } = new Guid();
			public float Radius { get; set; }
			public float RadialOrientation { get; set; }
			public List<Organ> Children { get; private set; } = new List<Organ>();
		}

		public class Muscle
		{
			public Organ From { get; set; }
			public Organ To { get; set; }
			// <-180; 180>
			public float FromRadialPosition { get; set; }
			// <-180; 180>
			public float ToRadialPosition { get; set; }
			// (0; 1)
			public float ContractionFactor { get; set; }
		}

		public Organ Root { get; set; }
		public HashSet<Muscle> Muscles { get; private set; } = new HashSet<Muscle>(new MuscleComparer());
	}
}
