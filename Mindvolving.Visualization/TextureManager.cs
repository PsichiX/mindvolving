using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mindvolving.Visualization
{
    public class TextureManager : IVisualizationComponent
    {
        public MindvolvingVisualization Visualization { get; set; }

        public TextureManager()
        {

        }

        public void LoadContent()
        {
            Circle = Visualization.Content.Load<Texture2D>("circle");
            Bubble = Visualization.Content.Load<Texture2D>("bubble");

            BlankTexture = new Texture2D(Visualization.GraphicsDevice, 1, 1);
            BlankTexture.SetData(new Color[] { Color.White });
        }

        public void Initialize()
        {
          
        }

        #region Textures
        public Texture2D Circle { get; private set; }
        public Texture2D BlankTexture { get; private set; }
        public Texture2D Bubble { get; private set; }
        #endregion
    }
}
