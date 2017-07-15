namespace Mindvolving.Visualization.Engine
{
    public interface IWorldElement
    {
        World World { get; set; }

        /// <summary>
        /// Initilizes the entity, invoked after creation of entity and bringing it into world
        /// </summary>
        void Initialize();
    }
}