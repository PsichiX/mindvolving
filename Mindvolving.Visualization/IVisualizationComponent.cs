namespace Mindvolving.Visualization
{
    public interface IVisualizationComponent
    {
        MindvolvingVisualization Visualization { get; set; }

        void Initialize();
    }
}
