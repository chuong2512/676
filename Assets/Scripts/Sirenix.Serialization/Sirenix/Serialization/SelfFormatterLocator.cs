using System;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	internal class SelfFormatterLocator : IFormatterLocator
	{
		public bool TryGetFormatter(Type type, FormatterLocationStep step, ISerializationPolicy policy, out IFormatter formatter)
		{
			formatter = null;
			if (!typeof(ISelfFormatter).IsAssignableFrom(type))
			{
				return false;
			}
			if ((step == FormatterLocationStep.BeforeRegisteredFormatters && type.IsDefined<AlwaysFormatsSelfAttribute>()) || step == FormatterLocationStep.AfterRegisteredFormatters)
			{
				formatter = (IFormatter)Activator.CreateInstance(typeof(SelfFormatterFormatter<>).MakeGenericType(type));
				return true;
			}
			return false;
		}
	}
}
