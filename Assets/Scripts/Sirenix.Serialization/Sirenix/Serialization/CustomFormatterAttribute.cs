using System;

namespace Sirenix.Serialization
{
	[AttributeUsage(AttributeTargets.Class)]
	[Obsolete("Use a RegisterFormatterAttribute applied to the containing assembly instead.", true)]
	public class CustomFormatterAttribute : Attribute
	{
		public readonly int Priority;

		public CustomFormatterAttribute()
		{
			Priority = 0;
		}

		public CustomFormatterAttribute(int priority = 0)
		{
			Priority = priority;
		}
	}
}
