using Mindvolving.Visualization.Engine.Entities;
using System.Collections.Generic;
using Physics = FarseerPhysics;

namespace Mindvolving.Visualization.Engine
{
    public class World
    {
        private List<Entity> entities;

        public Physics.Dynamics.World PhysicalWorld { get; private set; }

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
    }
}
