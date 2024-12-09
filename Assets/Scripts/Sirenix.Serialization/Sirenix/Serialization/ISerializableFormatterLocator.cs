using System;
using System.Runtime.Serialization;

namespace Sirenix.Serialization
{
	internal class ISerializableFormatterLocator : IFormatterLocator
	{
		public bool TryGetFormatter(Type type, FormatterLocationStep step, ISerializationPolicy policy, out IFormatter formatter)
		{
			if (step != FormatterLocationStep.AfterRegisteredFormatters || !typeof(ISerializable).IsAssignableFrom(type))
			{
				formatter = null;
				return false;
			}
			formatter = (IFormatter)Activator.CreateInstance(typeof(SerializableFormatter<>).MakeGenericType(type));
			return true;
		}
	}
}
