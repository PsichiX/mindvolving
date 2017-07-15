namespace Mindvolving.Visualization
{
    public interface IVisualizationHolder
    {
        MindvolvingVisualization Visualization { get; set; }

        void Initialize();
    }
}
