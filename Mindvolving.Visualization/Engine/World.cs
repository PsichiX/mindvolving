using Mindvolving.Visualization.Engine.Entities;
using System.Collections.Generic;
using Physics = FarseerPhysics;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Engine.Enviroment;
using Mindvolving;

namespace Mindvolving.Visualization.Engine
{
    public class World : IUpdateable, IVisualizationComponent
    {
        private List<Decal> decals;
        private List<Entity> entities;
        private bool initialized;

        public IReadOnlyList<Entity> Entities { get { return entities; } }
        public Physics.Dynamics.World PhysicalWorld { get { return Simulation.World; } }
        public MindvolvingVisualization Visualization { get; set; }
        public IReadOnlyList<Decal> Decals { get { return decals; } }
        public Simulation Simulation { get; private set; }

        public World()
        {
            Simulation = new Simulation();

            entities = new List<Entity>();
            decals = new List<Decal>();
        }

        public T CreateEntity<T>() where T : Entity, new()
        {
            T entity = new T();
            entity.World = this;
            entities.Add(entity);

            if(initialized)
                entity.Initialize();

            return entity;
        }

        public OrganismEntityBuilder CreateOrganism(Organisms.DNA dna)
        {
            OrganismEntityBuilder builder = new OrganismEntityBuilder();

            Organisms.Organism organism = Simulation.CreateOrganism(dna);
            builder.BeginBuilding(this, organism);

            return builder;
        }

        public T CreateDecal<T>() where T : Decal, new()
        {
            T decal = new T();
            decal.World = this;
            decals.Add(decal);

            if (initialized)
                decal.Initialize();

            return decal;
        }

        public void BringEntityIntoWorld(Entity entity)
        {
            entity.World = this;
            entities.Add(entity);

            if (initialized)
                entity.Initialize();
        }

        public void Update(GameTime gt)
        {
            System.Diagnostics.Debug.Assert(initialized, "World must be initialized before updating");

            Simulation.Process(1 / 60f);
 
            for(int i = 0; i < entities.Count; i++)
            {
                if (!entities[i].IsDestroyed)
                    entities[i].Update(gt);
                else
                {
                    entities[i].Destroy();
                    entities[i].Dispose();
                    entities.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < decals.Count; i++)
            {
                if (!decals[i].IsDestroyed)
                    decals[i].Update(gt);
                else
                {
                    decals[i].Destroy();
                    decals[i].Dispose();
                    decals.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Initialize()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Initialize();
            }

            for (int i = 0; i < decals.Count; i++)
            {
                decals[i].Initialize();
            }

            initialized = true;
        }
    }
}
