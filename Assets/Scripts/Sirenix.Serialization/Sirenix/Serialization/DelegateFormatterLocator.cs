using System;

namespace Sirenix.Serialization
{
	internal class DelegateFormatterLocator : IFormatterLocator
	{
		public bool TryGetFormatter(Type type, FormatterLocationStep step, ISerializationPolicy policy, out IFormatter formatter)
		{
			if (!typeof(Delegate).IsAssignableFrom(type))
			{
				formatter = null;
				return false;
			}
			formatter = (IFormatter)Activator.CreateInstance(typeof(DelegateFormatter<>).MakeGenericType(type));
			return true;
		}
	}
}
