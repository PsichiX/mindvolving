using System;
using System.Runtime.InteropServices;

namespace Mindvolving.Psyche
{
	public static class NAPI
	{
		public enum BindingsMode
		{
			Pinvoke,
			DynamicLinking
		}

		private const string LibName = "psi-engine";
		public const BindingsMode Bindings = BindingsMode.Pinvoke;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void BrainCallback(
			IntPtr session,
			IntPtr brain
		);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void BrainSerializeCallback(
			IntPtr brain,
			IntPtr data,
			[MarshalAs(UnmanagedType.I4)]
			int size
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.LPStr)]
		public extern static string SessionCreate();

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I1)]
		public extern static bool SessionDestroy(
			[MarshalAs(UnmanagedType.LPStr)]
			string uid
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public extern static IntPtr SessionGet(
			[MarshalAs(UnmanagedType.LPStr)]
			string uid
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public extern static float SessionGetTimeScale(
			[MarshalAs(UnmanagedType.LPStr)]
			string uid
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public extern static void SessionSetTimeScale(
			[MarshalAs(UnmanagedType.LPStr)]
			string uid,
			float v
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I4)]
		public extern static int SessionGetProcessingReportFlags(
			[MarshalAs(UnmanagedType.LPStr)]
			string uid
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public extern static void SessionSetProcessingReportFlags(
			[MarshalAs(UnmanagedType.LPStr)]
			string uid,
			[MarshalAs(UnmanagedType.I4)]
			int v
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SessionGetInterval", CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I4)]
		public extern static int SessionGetIntervalMicroseconds(
			[MarshalAs(UnmanagedType.LPStr)]
			string uid
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SessionSetInterval", CharSet = CharSet.Ansi)]
		public extern static void SessionSetIntervalMicroseconds(
			[MarshalAs(UnmanagedType.LPStr)]
			string uid,
			[MarshalAs(UnmanagedType.I4)]
			int v
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public extern static BrainCallback SessionGetBrainPreProcessCallback(
			[MarshalAs(UnmanagedType.LPStr)]
			string uid
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public extern static void SessionSetBrainPreProcessCallback(
			[MarshalAs(UnmanagedType.LPStr)]
			string uid,
			[MarshalAs(UnmanagedType.FunctionPtr)]
			BrainCallback v
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public extern static BrainCallback SessionGetBrainPostProcessCallback(
			[MarshalAs(UnmanagedType.LPStr)]
			string uid
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public extern static void SessionSetBrainPostProcessCallback(
			[MarshalAs(UnmanagedType.LPStr)]
			string uid,
			[MarshalAs(UnmanagedType.FunctionPtr)]
			BrainCallback v
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I8)]
		public extern static bool SessionStart(
			[MarshalAs(UnmanagedType.LPStr)]
			string uid
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I8)]
		public extern static bool SessionStop(
			[MarshalAs(UnmanagedType.LPStr)]
			string uid
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.LPStr)]
		public extern static string BrainCreate(
			[MarshalAs(UnmanagedType.LPStr)]
			string suid
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I8)]
		public extern static bool BrainDestroy(
			[MarshalAs(UnmanagedType.LPStr)]
			string suid,
			[MarshalAs(UnmanagedType.LPStr)]
			string buid
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I8)]
		public extern static bool PerformOnBrainLater(
			[MarshalAs(UnmanagedType.LPStr)]
			string suid,
			[MarshalAs(UnmanagedType.LPStr)]
			string buid,
			[MarshalAs(UnmanagedType.FunctionPtr)]
			BrainCallback action
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.LPStr)]
		public extern static string BrainGetUUID(
			IntPtr brain
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public extern static float BrainGetDemandForEnergyUnits(
			IntPtr brain
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I8)]
		public extern static bool BrainResetDemandForEnergyUnits(
			IntPtr brain
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public extern static float BrainGetEnergy(
			IntPtr brain
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I8)]
		public extern static bool BrainFeed(
			IntPtr brain,
			float energy
		);

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I8)]
		private extern static bool BrainSerialize(
			IntPtr brain,
			[MarshalAs(UnmanagedType.FunctionPtr)]
			BrainSerializeCallback action
		);

		public static byte[] BrainSerialize(IntPtr brain)
		{
			byte[] result = null;
			BrainSerialize(brain, (b, data, size) => {
				if (b != IntPtr.Zero && data != IntPtr.Zero && size > 0) {
					result = new byte[size];
					Marshal.Copy(data, result, 0, size);
				}
			});
			return result;
		}

		[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I8)]
		private extern static bool BrainDeserialize(
			IntPtr brain,
			IntPtr data,
			[MarshalAs(UnmanagedType.U4)]
			uint size
		);

		public static bool BrainDeserialize(IntPtr brain, byte[] data)
		{
			var bytes = Marshal.AllocHGlobal(data.Length);
			Marshal.Copy(data, 0, bytes, data.Length);
			var status = BrainDeserialize(brain, bytes, (uint)data.Length);
			Marshal.FreeHGlobal(bytes);
			return status;
		}
	}
}
