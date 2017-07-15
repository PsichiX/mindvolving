using Mindvolving.Visualization.Engine.Entities;
using System.Collections.Generic;
using Physics = FarseerPhysics;
using Microsoft.Xna.Framework;
using System;

namespace Mindvolving.Visualization.Engine
{
    public class World : IUpdateable, IVisualizationComponent
    {
        private List<Entity> entities;

        public IReadOnlyList<Entity> Entities { get { return entities; } }
        public Physics.Dynamics.World PhysicalWorld { get; private set; }
        public MindvolvingVisualization Visualization { get; set; }

        public World()
        {
            PhysicalWorld = new Physics.Dynamics.World(Physics.Common.Vector2.Zero);

            entities = new List<Entity>();
        }

        public T CreateEntity<T>() where T : Entity, new()
        {
            T entity = new T();
            entity.World = this;
            entities.Add(entity);
            entity.Initialize();
            return entity;
        }

        public void BringEntityIntoWorld(Entity entity)
        {
            entity.World = this;
            entities.Add(entity);
            entity.Initialize();
        }

        public void Update(GameTime gt)
        {
            PhysicalWorld.Step(1 / 60f);
 
            for(int i = 0; i < entities.Count; i++)
            {
                if (!entities[i].IsDestroyed)
                    entities[i].Update(gt);
                else
                {
                    entities[i].Destroy();
                    entities.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Initialize()
        {

        }
    }
}
