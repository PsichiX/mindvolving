using System;
using System.Collections.Generic;

namespace Mindvolving.Psyche
{
	public class Session : IDisposable
	{
		[Flags]
		public enum ProcessingReport
		{
			NONE = 0,
			SESSION_PREPROCESS = 1 << 0,
			BRAIN_PREPROCESS = 1 << 1,
			SESSION_POSTPROCESS = 1 << 2,
			BRAIN_POSTPROCESS = 1 << 3,
			ALL = -1
		}

		private string m_uid;
		private IntPtr m_pointer;
		private Dictionary<string, Brain> m_brains = new Dictionary<string, Brain>();

		public string UID { get { return m_uid; } }

		public bool IsValid { get { return !string.IsNullOrEmpty(m_uid); } }

		public IntPtr Pointer { get { return m_pointer; } }

		public float TimeScale
		{
			get { return NAPI.SessionGetTimeScale(m_uid); }
			set { NAPI.SessionSetTimeScale(m_uid, value); }
		}

		public ProcessingReport ProcessingReportFlags
		{
			get { return (ProcessingReport)NAPI.SessionGetProcessingReportFlags(m_uid); }
			set { NAPI.SessionSetProcessingReportFlags(m_uid, (int)value); }
		}

		public int IntervalMicroseconds
		{
			get { return NAPI.SessionGetIntervalMicroseconds(m_uid); }
			set { NAPI.SessionSetIntervalMicroseconds(m_uid, value); }
		}

		public BrainCallback BrainPreprocessCallback
		{
			get { return NAPI.SessionGetBrainPreProcessCallback(m_uid); }
			set { NAPI.SessionSetBrainPreProcessCallback(m_uid, value); }
		}

		public BrainCallback BrainPostprocessCallback
		{
			get { return NAPI.SessionGetBrainPostProcessCallback(m_uid); }
			set { NAPI.SessionSetBrainPostProcessCallback(m_uid, value); }
		}

		public Session()
		{
			m_uid = NAPI.SessionCreate();
			if (IsValid)
				m_pointer = NAPI.SessionGet(m_uid);
		}

		~Session()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (!IsValid)
				return;
			NAPI.SessionDestroy(m_uid);
			m_uid = null;
			m_pointer = IntPtr.Zero;
			foreach (var b in m_brains)
				b.Value.Dispose();
			m_brains.Clear();
		}

		public bool Start()
		{
			return IsValid && NAPI.SessionStart(m_uid);
		}

		public bool Stop()
		{
			return IsValid && NAPI.SessionStop(m_uid);
		}

		internal void RegisterBrain(Brain brain)
		{
			if (!m_brains.ContainsKey(brain.UID))
				m_brains[brain.UID] = brain;
		}

		internal void UnregisterBrain(Brain brain)
		{
			if (m_brains.ContainsKey(brain.UID))
				m_brains.Remove(brain.UID);
		}
	}
}
