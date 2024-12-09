using System;

namespace Sirenix.Serialization
{
	internal class ArrayFormatterLocator : IFormatterLocator
	{
		public bool TryGetFormatter(Type type, FormatterLocationStep step, ISerializationPolicy policy, out IFormatter formatter)
		{
			if (!type.IsArray)
			{
				formatter = null;
				return false;
			}
			if (type.GetArrayRank() == 1)
			{
				if (FormatterUtilities.IsPrimitiveArrayType(type.GetElementType()))
				{
					formatter = (IFormatter)Activator.CreateInstance(typeof(PrimitiveArrayFormatter<>).MakeGenericType(type.GetElementType()));
				}
				else
				{
					formatter = (IFormatter)Activator.CreateInstance(typeof(ArrayFormatter<>).MakeGenericType(type.GetElementType()));
				}
			}
			else
			{
				formatter = (IFormatter)Activator.CreateInstance(typeof(MultiDimensionalArrayFormatter<, >).MakeGenericType(type, type.GetElementType()));
			}
			return true;
		}
	}
}
