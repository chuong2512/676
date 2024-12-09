using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.Serialization.Utilities;
using UnityEngine;

namespace Sirenix.Serialization
{
	public static class SerializationUtility
	{
		public static IDataWriter CreateWriter(Stream stream, SerializationContext context, DataFormat format)
		{
			switch (format)
			{
			case DataFormat.Binary:
				return new BinaryDataWriter(stream, context);
			case DataFormat.JSON:
				return new JsonDataWriter(stream, context);
			case DataFormat.Nodes:
				Debug.LogError(string.Concat("Cannot automatically create a writer for the format '", DataFormat.Nodes, "', because it does not use a stream."));
				return null;
			default:
				throw new NotImplementedException(format.ToString());
			}
		}

		public static IDataReader CreateReader(Stream stream, DeserializationContext context, DataFormat format)
		{
			switch (format)
			{
			case DataFormat.Binary:
				return new BinaryDataReader(stream, context);
			case DataFormat.JSON:
				return new JsonDataReader(stream, context);
			case DataFormat.Nodes:
				Debug.LogError(string.Concat("Cannot automatically create a reader for the format '", DataFormat.Nodes, "', because it does not use a stream."));
				return null;
			default:
				throw new NotImplementedException(format.ToString());
			}
		}

		private static IDataWriter GetCachedWriter(out IDisposable cache, DataFormat format, Stream stream, SerializationContext context)
		{
			IDataWriter result;
			switch (format)
			{
			case DataFormat.Binary:
			{
				Cache<BinaryDataWriter> cache3 = Cache<BinaryDataWriter>.Claim();
				BinaryDataWriter value2 = cache3.Value;
				value2.Stream = stream;
				value2.Context = context;
				value2.PrepareNewSerializationSession();
				result = value2;
				cache = cache3;
				break;
			}
			case DataFormat.JSON:
			{
				Cache<JsonDataWriter> cache2 = Cache<JsonDataWriter>.Claim();
				JsonDataWriter value = cache2.Value;
				value.Stream = stream;
				value.Context = context;
				value.PrepareNewSerializationSession();
				result = value;
				cache = cache2;
				break;
			}
			case DataFormat.Nodes:
				throw new InvalidOperationException(string.Concat("Cannot automatically create a writer for the format '", DataFormat.Nodes, "', because it does not use a stream."));
			default:
				throw new NotImplementedException(format.ToString());
			}
			return result;
		}

		private static IDataReader GetCachedReader(out IDisposable cache, DataFormat format, Stream stream, DeserializationContext context)
		{
			IDataReader result;
			switch (format)
			{
			case DataFormat.Binary:
			{
				Cache<BinaryDataReader> cache3 = Cache<BinaryDataReader>.Claim();
				BinaryDataReader value2 = cache3.Value;
				value2.Stream = stream;
				value2.Context = context;
				value2.PrepareNewSerializationSession();
				result = value2;
				cache = cache3;
				break;
			}
			case DataFormat.JSON:
			{
				Cache<JsonDataReader> cache2 = Cache<JsonDataReader>.Claim();
				JsonDataReader value = cache2.Value;
				value.Stream = stream;
				value.Context = context;
				value.PrepareNewSerializationSession();
				result = value;
				cache = cache2;
				break;
			}
			case DataFormat.Nodes:
				throw new InvalidOperationException(string.Concat("Cannot automatically create a reader for the format '", DataFormat.Nodes, "', because it does not use a stream."));
			default:
				throw new NotImplementedException(format.ToString());
			}
			return result;
		}

		public static void SerializeValueWeak(object value, IDataWriter writer)
		{
			Serializer.GetForValue(value).WriteValueWeak(value, writer);
			writer.FlushToStream();
		}

		public static void SerializeValueWeak(object value, IDataWriter writer, out List<UnityEngine.Object> unityObjects)
		{
			Cache<UnityReferenceResolver> cache = Cache<UnityReferenceResolver>.Claim();
			writer.Context.IndexReferenceResolver = cache.Value;
			Serializer.GetForValue(value).WriteValueWeak(value, writer);
			writer.FlushToStream();
			unityObjects = cache.Value.GetReferencedUnityObjects();
		}

		public static void SerializeValue<T>(T value, IDataWriter writer)
		{
			Serializer.Get<T>().WriteValue(value, writer);
			writer.FlushToStream();
		}

		public static void SerializeValue<T>(T value, IDataWriter writer, out List<UnityEngine.Object> unityObjects)
		{
			Cache<UnityReferenceResolver> cache = Cache<UnityReferenceResolver>.Claim();
			writer.Context.IndexReferenceResolver = cache.Value;
			Serializer.Get<T>().WriteValue(value, writer);
			writer.FlushToStream();
			unityObjects = cache.Value.GetReferencedUnityObjects();
		}

		public static void SerializeValueWeak(object value, Stream stream, DataFormat format, SerializationContext context = null)
		{
			IDisposable cache;
			IDataWriter cachedWriter = GetCachedWriter(out cache, format, stream, context);
			try
			{
				if (context != null)
				{
					SerializeValueWeak(value, cachedWriter);
					return;
				}
				Cache<SerializationContext> cache2 = Cache<SerializationContext>.Claim();
				cachedWriter.Context = cache2;
				SerializeValueWeak(value, cachedWriter);
			}
			finally
			{
				cache.Dispose();
			}
		}

		public static void SerializeValueWeak(object value, Stream stream, DataFormat format, out List<UnityEngine.Object> unityObjects, SerializationContext context = null)
		{
			IDisposable cache;
			IDataWriter cachedWriter = GetCachedWriter(out cache, format, stream, context);
			try
			{
				if (context != null)
				{
					SerializeValueWeak(value, cachedWriter, out unityObjects);
					return;
				}
				Cache<SerializationContext> cache2 = Cache<SerializationContext>.Claim();
				cachedWriter.Context = cache2;
				SerializeValueWeak(value, cachedWriter, out unityObjects);
			}
			finally
			{
				cache.Dispose();
			}
		}

		public static void SerializeValue<T>(T value, Stream stream, DataFormat format, SerializationContext context = null)
		{
			IDisposable cache;
			IDataWriter cachedWriter = GetCachedWriter(out cache, format, stream, context);
			try
			{
				if (context != null)
				{
					SerializeValue(value, cachedWriter);
					return;
				}
				Cache<SerializationContext> cache2 = Cache<SerializationContext>.Claim();
				cachedWriter.Context = cache2;
				SerializeValue(value, cachedWriter);
			}
			finally
			{
				cache.Dispose();
			}
		}

		public static void SerializeValue<T>(T value, Stream stream, DataFormat format, out List<UnityEngine.Object> unityObjects, SerializationContext context = null)
		{
			IDisposable cache;
			IDataWriter cachedWriter = GetCachedWriter(out cache, format, stream, context);
			try
			{
				if (context != null)
				{
					SerializeValue(value, cachedWriter, out unityObjects);
					return;
				}
				Cache<SerializationContext> cache2 = Cache<SerializationContext>.Claim();
				cachedWriter.Context = cache2;
				SerializeValue(value, cachedWriter, out unityObjects);
			}
			finally
			{
				cache.Dispose();
			}
		}

		public static byte[] SerializeValueWeak(object value, DataFormat format, SerializationContext context = null)
		{
			Cache<CachedMemoryStream> cache = CachedMemoryStream.Claim();
			SerializeValueWeak(value, cache.Value.MemoryStream, format, context);
			return cache.Value.MemoryStream.ToArray();
		}

		public static byte[] SerializeValueWeak(object value, DataFormat format, out List<UnityEngine.Object> unityObjects)
		{
			Cache<CachedMemoryStream> cache = CachedMemoryStream.Claim();
			SerializeValueWeak(value, cache.Value.MemoryStream, format, out unityObjects);
			return cache.Value.MemoryStream.ToArray();
		}

		public static byte[] SerializeValue<T>(T value, DataFormat format, SerializationContext context = null)
		{
			Cache<CachedMemoryStream> cache = CachedMemoryStream.Claim();
			SerializeValue(value, cache.Value.MemoryStream, format, context);
			return cache.Value.MemoryStream.ToArray();
		}

		public static byte[] SerializeValue<T>(T value, DataFormat format, out List<UnityEngine.Object> unityObjects, SerializationContext context = null)
		{
			Cache<CachedMemoryStream> cache = CachedMemoryStream.Claim();
			SerializeValue(value, cache.Value.MemoryStream, format, out unityObjects, context);
			return cache.Value.MemoryStream.ToArray();
		}

		public static object DeserializeValueWeak(IDataReader reader)
		{
			return Serializer.Get<object>().ReadValueWeak(reader);
		}

		public static object DeserializeValueWeak(IDataReader reader, List<UnityEngine.Object> referencedUnityObjects)
		{
			Cache<UnityReferenceResolver> cache = Cache<UnityReferenceResolver>.Claim();
			cache.Value.SetReferencedUnityObjects(referencedUnityObjects);
			reader.Context.IndexReferenceResolver = cache.Value;
			return Serializer.Get<object>().ReadValueWeak(reader);
		}

		public static T DeserializeValue<T>(IDataReader reader)
		{
			return Serializer.Get<T>().ReadValue(reader);
		}

		public static T DeserializeValue<T>(IDataReader reader, List<UnityEngine.Object> referencedUnityObjects)
		{
			Cache<UnityReferenceResolver> cache = Cache<UnityReferenceResolver>.Claim();
			cache.Value.SetReferencedUnityObjects(referencedUnityObjects);
			reader.Context.IndexReferenceResolver = cache.Value;
			return Serializer.Get<T>().ReadValue(reader);
		}

		public static object DeserializeValueWeak(Stream stream, DataFormat format, DeserializationContext context = null)
		{
			IDisposable cache;
			IDataReader cachedReader = GetCachedReader(out cache, format, stream, context);
			try
			{
				if (context != null)
				{
					return DeserializeValueWeak(cachedReader);
				}
				Cache<DeserializationContext> cache2 = Cache<DeserializationContext>.Claim();
				cachedReader.Context = cache2;
				return DeserializeValueWeak(cachedReader);
			}
			finally
			{
				cache.Dispose();
			}
		}

		public static object DeserializeValueWeak(Stream stream, DataFormat format, List<UnityEngine.Object> referencedUnityObjects, DeserializationContext context = null)
		{
			IDisposable cache;
			IDataReader cachedReader = GetCachedReader(out cache, format, stream, context);
			try
			{
				if (context != null)
				{
					return DeserializeValueWeak(cachedReader, referencedUnityObjects);
				}
				Cache<DeserializationContext> cache2 = Cache<DeserializationContext>.Claim();
				cachedReader.Context = cache2;
				return DeserializeValueWeak(cachedReader, referencedUnityObjects);
			}
			finally
			{
				cache.Dispose();
			}
		}

		public static T DeserializeValue<T>(Stream stream, DataFormat format, DeserializationContext context = null)
		{
			IDisposable cache;
			IDataReader cachedReader = GetCachedReader(out cache, format, stream, context);
			try
			{
				if (context != null)
				{
					return DeserializeValue<T>(cachedReader);
				}
				Cache<DeserializationContext> cache2 = Cache<DeserializationContext>.Claim();
				cachedReader.Context = cache2;
				return DeserializeValue<T>(cachedReader);
			}
			finally
			{
				cache.Dispose();
			}
		}

		public static T DeserializeValue<T>(Stream stream, DataFormat format, List<UnityEngine.Object> referencedUnityObjects, DeserializationContext context = null)
		{
			IDisposable cache;
			IDataReader cachedReader = GetCachedReader(out cache, format, stream, context);
			try
			{
				if (context != null)
				{
					return DeserializeValue<T>(cachedReader, referencedUnityObjects);
				}
				Cache<DeserializationContext> cache2 = Cache<DeserializationContext>.Claim();
				cachedReader.Context = cache2;
				return DeserializeValue<T>(cachedReader, referencedUnityObjects);
			}
			finally
			{
				cache.Dispose();
			}
		}

		public static object DeserializeValueWeak(byte[] bytes, DataFormat format, DeserializationContext context = null)
		{
			Cache<CachedMemoryStream> cache = CachedMemoryStream.Claim(bytes);
			return DeserializeValueWeak(cache.Value.MemoryStream, format, context);
		}

		public static object DeserializeValueWeak(byte[] bytes, DataFormat format, List<UnityEngine.Object> referencedUnityObjects)
		{
			Cache<CachedMemoryStream> cache = CachedMemoryStream.Claim(bytes);
			return DeserializeValueWeak(cache.Value.MemoryStream, format, referencedUnityObjects);
		}

		public static T DeserializeValue<T>(byte[] bytes, DataFormat format, DeserializationContext context = null)
		{
			Cache<CachedMemoryStream> cache = CachedMemoryStream.Claim(bytes);
			return DeserializeValue<T>(cache.Value.MemoryStream, format, context);
		}

		public static T DeserializeValue<T>(byte[] bytes, DataFormat format, List<UnityEngine.Object> referencedUnityObjects, DeserializationContext context = null)
		{
			Cache<CachedMemoryStream> cache = CachedMemoryStream.Claim(bytes);
			return DeserializeValue<T>(cache.Value.MemoryStream, format, referencedUnityObjects, context);
		}

		public static object CreateCopy(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is string)
			{
				return obj;
			}
			Type type = obj.GetType();
			if (type.IsValueType)
			{
				return obj;
			}
			if (type.InheritsFrom(typeof(UnityEngine.Object)))
			{
				return obj;
			}
			Cache<CachedMemoryStream> cache3 = CachedMemoryStream.Claim();
			Cache<SerializationContext> cache = Cache<SerializationContext>.Claim();
			Cache<DeserializationContext> cache2 = Cache<DeserializationContext>.Claim();
			cache.Value.Config.SerializationPolicy = SerializationPolicies.Everything;
			cache2.Value.Config.SerializationPolicy = SerializationPolicies.Everything;
			SerializeValue(obj, cache3.Value.MemoryStream, DataFormat.Binary, out var unityObjects, cache);
			cache3.Value.MemoryStream.Position = 0L;
			return DeserializeValue<object>(cache3.Value.MemoryStream, DataFormat.Binary, unityObjects, cache2);
		}
	}
}
