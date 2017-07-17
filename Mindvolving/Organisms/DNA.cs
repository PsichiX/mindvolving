using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mindvolving.Organisms
{
	public class DNA
	{
		private static char[] ATCG = new char[] { 'A', 'T', 'C', 'G' };

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
			public Guid UID { get; private set; } = Guid.NewGuid();
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

		public Organ Root { get; set; } = new Organ();
		public HashSet<Muscle> Muscles { get; private set; } = new HashSet<Muscle>(new MuscleComparer());

		public byte[] Serialize()
		{
			using (var memory = new MemoryStream())
			using (var writer = new BinaryWriter(memory)) {
				WriteOrgan(writer, Root);
				writer.Write(Muscles.Count);
				foreach (var muscle in Muscles)
					WriteMuscle(writer, muscle);
				return memory.GetBuffer();
			}
		}

		public static DNA Deserialize(byte[] data)
		{
			using (var memory = new MemoryStream())
			using (var writer = new BinaryWriter(memory)) {
				var dna = new DNA();

				return dna;
			}
		}

		public override string ToString()
		{
			byte[] data = Serialize();
			var sb = new StringBuilder();
			foreach (var item in data)
				WriteStringByte(sb, item);
			return sb.ToString();
		}

		private void WriteOrgan(BinaryWriter writer, Organ organ)
		{
			writer.Write(organ.UID.ToString());
			writer.Write(organ.Radius);
			writer.Write(organ.RadialOrientation);
			writer.Write(organ.Children.Count);
			foreach (var child in organ.Children)
				WriteOrgan(writer, child);
		}

		private void WriteMuscle(BinaryWriter writer, Muscle muscle)
		{
			writer.Write(muscle.From.UID.ToString());
			writer.Write(muscle.To.UID.ToString());
			writer.Write(muscle.FromRadialPosition);
			writer.Write(muscle.ToRadialPosition);
			writer.Write(muscle.ContractionFactor);
		}

		private void WriteStringByte(StringBuilder builder, byte data)
		{
			for (var i = 0; i < 8; ++i)
				WriteStringBit(builder, (data & (1 << i)) != 0);
		}

		private void WriteStringBit(StringBuilder builder, bool bit)
		{
			builder.Append(ATCG[(bit ? 2 : 0) + (new Random().Next() % 2)]);
		}
	}
}
