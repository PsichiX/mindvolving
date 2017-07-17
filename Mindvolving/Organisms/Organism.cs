using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Mindvolving.Psyche;
using System;
using System.Collections.Generic;

namespace Mindvolving.Organisms
{
	public class Organism : IDisposable
	{
		public static float MinimumOrganDistance = 0.01f;

		private List<Muscle> m_muscles = new List<Muscle>();

		public Simulation Simulation { get; private set; }

		public World World { get; private set; }

		public Organ Root { get; private set; }

		public Brain Brain { get; private set; }

		public IReadOnlyList<Muscle> Muscles { get { return m_muscles; } }

		private Organism(Simulation simulation)
		{
			Simulation = simulation;
			World = simulation.World;
		}

		~Organism()
		{
			Dispose();
		}

		public void Dispose()
		{
			foreach (var muscle in Muscles)
				muscle.Dispose();
			if (Root != null)
				Root.Dispose();
			if (Brain != null)
				Brain.Dispose();
			if (Simulation != null)
				Simulation.RemoveOrganism(this);
			Simulation = null;
			World = null;
			Root = null;
			Brain = null;
			m_muscles.Clear();
			GC.SuppressFinalize(this);
		}

		internal static Organism Factory(Simulation simulation, DNA dna, Vector2 position)
		{
			var organism = new Organism(simulation);
			organism.Root = new Organ(dna.Root.UID, organism, null, position, dna.Root.Radius);
			CreateChildrenOrgan(organism.Root, dna.Root, position);
			foreach (var muscle in dna.Muscles)
				CreateMuscle(organism, muscle);
			return organism;
		}

		private static void CreateChildrenOrgan(Organ root, DNA.Organ dna, Vector2 worldPos)
		{
			foreach (var leaf in dna.Children)
			{
				var distance = dna.Radius + leaf.Radius + MinimumOrganDistance;
				var angle = leaf.RadialOrientation * Math.PI / 180.0;
				var x = (float)Math.Cos(angle) * distance;
				var y = (float)Math.Sin(angle) * distance;
				var position = new Vector2(x, y) + worldPos;
				var child = new Organ(leaf.UID, root.Organism, root, position, leaf.Radius);
				root.m_children.Add(child);
				CreateChildrenOrgan(child, leaf, position);
			}
		}

		private static void CreateMuscle(Organism organism, DNA.Muscle muscle)
		{
			var from = FindOrgan(organism.Root, muscle.From.UID);
			var to = FindOrgan(organism.Root, muscle.To.UID);
			if (from != null && to != null)
				organism.m_muscles.Add(new Muscle(organism, from, to, muscle.FromRadialPosition, muscle.ToRadialPosition, muscle.ContractionFactor));
		}

		private static Organ FindOrgan(Organ root, Guid dnauid)
		{
			if (root.DNAUID == dnauid)
				return root;
			foreach (var child in root.Children)
			{
				var found = FindOrgan(child, dnauid);
				if (found != null)
					return found;
			}
			return null;
		}
	}
}
