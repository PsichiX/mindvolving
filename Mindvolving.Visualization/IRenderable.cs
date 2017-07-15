using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindvolving.Visualization
{
    public interface IRenderable
    {
        MindvolvingVisualization Visualization { get; set; }

        void Initialize();
        void Draw(GameTime gt);
    }
}
