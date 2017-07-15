using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mindvolving.Visualization
{
    public class TextureManager
    {
        private MindvolvingVisualization visualization;

        public TextureManager(MindvolvingVisualization visualization)
        {
            this.visualization = visualization;
        }

        public void LoadContent()
        {
            Circle = visualization.Content.Load<Texture2D>("circle");

            BlankTexture = new Texture2D(visualization.GraphicsDevice, 1, 1);
            BlankTexture.SetData(new Color[] { Color.White });
        }

        #region Textures
        public Texture2D Circle { get; private set; }
        public Texture2D BlankTexture { get; private set; }
        #endregion     
    }
}
