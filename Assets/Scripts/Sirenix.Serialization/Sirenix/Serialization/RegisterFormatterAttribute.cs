using System;

namespace Sirenix.Serialization
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public class RegisterFormatterAttribute : Attribute
	{
		public Type FormatterType { get; private set; }

		public int Priority { get; private set; }

		public RegisterFormatterAttribute(Type formatterType, int priority = 0)
		{
			FormatterType = formatterType;
			Priority = priority;
		}
	}
}
