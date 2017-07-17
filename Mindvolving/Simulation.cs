using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Mindvolving.Organisms;
using System;
using System.Collections.Generic;

namespace Mindvolving
{
    public class Simulation : IDisposable
    {
		private List<Organism> m_organisms = new List<Organism>();

		public World World { get; private set; }

		public IReadOnlyList<Organism> Organisms { get { return m_organisms; } }

		public Simulation()
		{
			World = new World(Vector2.Zero);
		}

		~Simulation()
		{
			Dispose();
		}

		public void Dispose()
		{
			foreach (var organism in Organisms)
				organism.Dispose();
			World = null;
			m_organisms.Clear();
			GC.SuppressFinalize(this);
		}

		// TODO: move to thread
		public void Process(float deltaTime)
		{
			World.Step(deltaTime);
		}

		public Organism CreateOrganism(DNA dna)
		{
			var organism = Organism.Factory(this, dna);
			m_organisms.Add(organism);
			return organism;
		}

		internal void RemoveOrganism(Organism organism)
		{
			if (m_organisms.Contains(organism))
				m_organisms.Remove(organism);
		}
	}
}
