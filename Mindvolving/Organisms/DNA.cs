using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mindvolving.Organisms
{
	public partial class DNA
	{
		private static char[] ATCG = new char[] { 'A', 'T', 'C', 'G' };
		private static Random ATCGRandom = new Random();

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
			public Guid UID { get; internal set; } = Guid.NewGuid();
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

		public DNA Mutate(int points)
		{
			throw new NotImplementedException();
		}

		public byte[] Serialize()
		{
			using (var memory = new MemoryStream())
			using (var writer = new BinaryWriter(memory))
			{
				WriteOrgan(writer, Root);
				writer.Write(Muscles.Count);
				foreach (var muscle in Muscles)
					WriteMuscle(writer, muscle);
				return memory.GetBuffer();
			}
		}

		public static DNA Deserialize(byte[] data)
		{
			using (var memory = new MemoryStream(data))
			using (var reader = new BinaryReader(memory))
			{
				var dna = new DNA();
				dna.Root = ReadOrgan(reader);
				var c = reader.ReadInt32();
				for (var i = 0; i < c; ++i)
					dna.Muscles.Add(ReadMuscle(reader, dna));
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

		public static DNA FromString(string source)
		{
			if (source.Length % 8 != 0)
				return null;
			var data = new byte[source.Length >> 3];
			for (int i = 0, c = data.Length; i < c; ++i)
				data[i] = ReadStringByte(source.Substring(i << 3, 8));
			return Deserialize(data);
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

		private static Organ ReadOrgan(BinaryReader reader)
		{
			var organ = new Organ();
			organ.UID = new Guid(reader.ReadString());
			organ.Radius = reader.ReadSingle();
			organ.RadialOrientation = reader.ReadSingle();
			var c = reader.ReadInt32();
			for (var i = 0; i < c; ++i)
				organ.Children.Add(ReadOrgan(reader));
			return organ;
		}

		private void WriteMuscle(BinaryWriter writer, Muscle muscle)
		{
			writer.Write(muscle.From.UID.ToString());
			writer.Write(muscle.To.UID.ToString());
			writer.Write(muscle.FromRadialPosition);
			writer.Write(muscle.ToRadialPosition);
			writer.Write(muscle.ContractionFactor);
		}

		private static Muscle ReadMuscle(BinaryReader reader, DNA dna)
		{
			var muscle = new Muscle();
			muscle.From = FindOrgan(dna.Root, new Guid(reader.ReadString()));
			muscle.To = FindOrgan(dna.Root, new Guid(reader.ReadString()));
			muscle.FromRadialPosition = reader.ReadSingle();
			muscle.ToRadialPosition = reader.ReadSingle();
			muscle.ContractionFactor = reader.ReadSingle();
			return muscle;
		}

		private void WriteStringByte(StringBuilder builder, byte data)
		{
			for (var i = 0; i < 8; ++i)
				WriteStringBit(builder, (data & (1 << i)) != 0);
		}

		private static byte ReadStringByte(string data)
		{
			if (data.Length != 8)
				throw new ArgumentException("`data` must have length of 8 characters!");
			byte result = 0;
			for (var i = 0; i < 8; ++i)
				result |= (byte)((ReadStringBit(data[i]) ? 1 : 0) << i);
			return result;
		}

		private void WriteStringBit(StringBuilder builder, bool bit)
		{
			builder.Append(ATCG[(bit ? 2 : 0) + (ATCGRandom.Next() % 2)]);
		}

		private static bool ReadStringBit(char bit)
		{
			switch (bit)
			{
			case 'A':
			case 'T':
				return false;
			case 'C':
			case 'G':
				return true;
			default:
				throw new ArgumentException("`bit` must be one of these characters: A, T, C, G!");
			}
		}

		private static Organ FindOrgan(Organ root, Guid uid)
		{
			if (root.UID == uid)
				return root;
			foreach (var child in root.Children)
			{
				var found = FindOrgan(child, uid);
				if (found != null)
					return found;
			}
			return null;
		}
	}
}
