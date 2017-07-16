namespace Mindvolving.Organisms.Physics
{
	public class OrganUserData : IPhysicsUserData
	{
		public Organ Organ { get; internal set; }
		public object CustomData { get; set; }
	}
}
