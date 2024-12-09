using System;
using UnityEngine;

namespace Sirenix.Serialization
{
	public static class ArchitectureInfo
	{
		public static bool Architecture_Supports_Unaligned_Float32_Reads;

		public static bool Architecture_Supports_All_Unaligned_ReadWrites;

		unsafe static ArchitectureInfo()
		{
			try
			{
				byte[] array = new byte[8];
				try
				{
					fixed (byte* ptr = array)
					{
						for (int i = 0; i < 4; i++)
						{
							float num = *(float*)(ptr + i);
						}
						Architecture_Supports_Unaligned_Float32_Reads = true;
					}
				}
				finally
				{
				}
			}
			catch (NullReferenceException)
			{
				Architecture_Supports_Unaligned_Float32_Reads = false;
			}
		}

		internal static void SetRuntimePlatform(RuntimePlatform platform)
		{
			switch (platform)
			{
			case RuntimePlatform.IPhonePlayer:
				Architecture_Supports_All_Unaligned_ReadWrites = false;
				Architecture_Supports_Unaligned_Float32_Reads = false;
				Debug.Log("OdinSerializer detected that it's running on an iPhone; disabling all unaligned read/writes.");
				break;
			case RuntimePlatform.Android:
				Architecture_Supports_All_Unaligned_ReadWrites = false;
				Architecture_Supports_Unaligned_Float32_Reads = false;
				Debug.Log("OdinSerializer detected that it's running in Android; disabling all unaligned read/writes.");
				break;
			default:
				if (Architecture_Supports_Unaligned_Float32_Reads)
				{
					Architecture_Supports_All_Unaligned_ReadWrites = true;
				}
				break;
			}
		}
	}
}
