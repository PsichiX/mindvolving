namespace Mindvolving.Organisms.Physics
{
	public class MuscleUserData : IPhysicsUserData
	{
		public Muscle Muscle { get; internal set; }
		public object CustomData { get; set; }
	}
}
