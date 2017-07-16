using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Mindvolving.Visualization.Engine
{
    public abstract class WorldObject : IUpdateable, IDisposable, IWorldElement
    {
        public World World { get; set; }
        public IRenderable Renderer { get; protected set; }
        public bool IsDestroyed { get; private set; }
        public virtual Vector2 Position { get; set; }

        public virtual void Destroy()
        {
            IsDestroyed = true;
        }

        public virtual void Dispose()
        {
            if (!IsDestroyed)
                Destroy();
        }

        public virtual void Update(GameTime gt)
        {

        }

        public virtual void Initialize()
        {
            
        }
    }
}
