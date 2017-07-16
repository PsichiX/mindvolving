namespace Mindvolving.Organisms.Physics
{
	public class BoneUserData : IPhysicsUserData
	{
		public Organ From { get; internal set; }
		public Organ To { get; internal set; }
		public object CustomData { get; set; }
	}
}
