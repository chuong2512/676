using System;

namespace Sirenix.Serialization
{
	public class ColorBlockFormatterLocator : IFormatterLocator
	{
		public bool TryGetFormatter(Type type, FormatterLocationStep step, ISerializationPolicy policy, out IFormatter formatter)
		{
			if (step == FormatterLocationStep.BeforeRegisteredFormatters && type.FullName == "UnityEngine.UI.ColorBlock")
			{
				Type type2 = typeof(ColorBlockFormatter<>).MakeGenericType(type);
				formatter = (IFormatter)Activator.CreateInstance(type2);
				return true;
			}
			formatter = null;
			return false;
		}
	}
}
