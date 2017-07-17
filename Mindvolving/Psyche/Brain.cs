using System;

namespace Mindvolving.Psyche
{
	public class Brain : IDisposable
	{
		private string m_uid;
		private Session m_session;
		private IntPtr m_pointer;

		public string UID { get { return m_uid; } }

		public bool IsValid { get { return !string.IsNullOrEmpty(m_uid); } }

		public Session Session { get { return m_session; } }

		public IntPtr Pointer { get { return m_pointer; } }

		public float DemandForEnergyUnits { get { return NAPI.BrainGetDemandForEnergyUnits(m_pointer); } }

		public float Energy { get { return NAPI.BrainGetEnergy(m_pointer); } }

		public uint SensorsCount { get { return NAPI.BrainGetSensorsCount(m_pointer); } }

		public uint EffectorsCount { get { return NAPI.BrainGetEffectorsCount(m_pointer); } }

		public Brain(Session session)
		{
			if (session == null)
				return;
			m_session = session;
			m_uid = NAPI.BrainCreate(session.UID);
			if (IsValid) {
				m_pointer = NAPI.BrainGet(m_session.UID, m_uid);
				m_session.RegisterBrain(this);
			}
		}

		~Brain()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (!IsValid)
				return;
			m_session.UnregisterBrain(this);
			NAPI.BrainDestroy(m_session.UID, m_uid);
			m_uid = null;
			m_session = null;
			m_pointer = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		public bool PerformOnLater(BrainCallback action)
		{
			return IsValid && NAPI.PerformOnBrainLater(m_session.UID, m_uid, action);
		}

		public bool ResetDemandForEnergyUnits()
		{
			return IsValid && NAPI.BrainResetDemandForEnergyUnits(m_pointer);
		}

		public bool Feed(float energy)
		{
			return IsValid && NAPI.BrainFeed(m_pointer, energy);
		}

		public bool SensorPushImpulse(uint index, float value)
		{
			return IsValid && NAPI.BrainSensorPushImpulse(m_pointer, index, value);
		}

		public float EffectorTakeImpulse(uint index)
		{
			return IsValid ? NAPI.BrainEffectorTakeImpulse(m_pointer, index) : 0;
		}

		public byte[] Serialize()
		{
			return IsValid ? NAPI.BrainSerialize(m_pointer) : null;
		}

		public bool Deserialize(byte[] data)
		{
			return IsValid && NAPI.BrainDeserialize(m_pointer, data);
		}
	}
}
