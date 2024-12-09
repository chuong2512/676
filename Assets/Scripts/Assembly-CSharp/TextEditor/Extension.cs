using System.Globalization;
using UnityEngine;

namespace TextEditor
{
	public static class Extension
	{
		public static Color ToColor(this string origin)
		{
			Color color = Color.white;
			ColorUtility.TryParseHtmlString(origin, out color);
			return color;
		}

		public static string ToColorString(this Color color)
		{
			return ColorUtility.ToHtmlStringRGBA(color);
		}

		public static Color HexColorToColor(this string hexColorString)
		{
			if (string.IsNullOrEmpty(hexColorString))
			{
				return default(Color);
			}
			int num = int.Parse(hexColorString, NumberStyles.AllowHexSpecifier);
			float num2 = 255f;
			int num3 = 0xFF & num;
			int num4 = 0xFF00 & num;
			num4 >>= 8;
			int num5 = 0xFF0000 & num;
			num5 >>= 16;
			return new Color((float)((0xFF000000u & num) >> 24) / num2, (float)num5 / num2, (float)num4 / num2, (float)num3 / num2);
		}

		public static string LocalizedText(this string key)
		{
			return LocalizationManager.Instance.GetLocalizedValue(key);
		}
	}
}
