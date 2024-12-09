using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using Sirenix.Serialization.Utilities;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Sirenix.Serialization
{
	public static class UnitySerializationUtility
	{
		private struct CachedSerializationBackendResult
		{
			public bool HasCalculatedSerializeUnityFieldsTrueResult;

			public bool HasCalculatedSerializeUnityFieldsFalseResult;

			public bool SerializeUnityFieldsTrueResult;

			public bool SerializeUnityFieldsFalseResult;
		}

		public static readonly Type SerializeReferenceAttributeType = typeof(SerializeField).Assembly.GetType("UnityEngine.SerializeReference");

		private static readonly Dictionary<DataFormat, IDataReader> UnityReaders = new Dictionary<DataFormat, IDataReader>();

		private static readonly Dictionary<DataFormat, IDataWriter> UnityWriters = new Dictionary<DataFormat, IDataWriter>();

		private static readonly Dictionary<MemberInfo, Sirenix.Serialization.Utilities.WeakValueGetter> UnityMemberGetters = new Dictionary<MemberInfo, Sirenix.Serialization.Utilities.WeakValueGetter>();

		private static readonly Dictionary<MemberInfo, Sirenix.Serialization.Utilities.WeakValueSetter> UnityMemberSetters = new Dictionary<MemberInfo, Sirenix.Serialization.Utilities.WeakValueSetter>();

		private static readonly Dictionary<MemberInfo, bool> UnityWillSerializeMembersCache = new Dictionary<MemberInfo, bool>();

		private static readonly Dictionary<Type, bool> UnityWillSerializeTypesCache = new Dictionary<Type, bool>();

		private static readonly HashSet<Type> UnityNeverSerializesTypes = new HashSet<Type> { typeof(Coroutine) };

		private static readonly ISerializationPolicy UnityPolicy = SerializationPolicies.Unity;

		private static readonly ISerializationPolicy EverythingPolicy = SerializationPolicies.Everything;

		private static readonly ISerializationPolicy StrictPolicy = SerializationPolicies.Strict;

		private static readonly Dictionary<MemberInfo, CachedSerializationBackendResult> OdinWillSerializeCache_UnityPolicy = new Dictionary<MemberInfo, CachedSerializationBackendResult>(Sirenix.Serialization.Utilities.ReferenceEqualityComparer<MemberInfo>.Default);

		private static readonly Dictionary<MemberInfo, CachedSerializationBackendResult> OdinWillSerializeCache_EverythingPolicy = new Dictionary<MemberInfo, CachedSerializationBackendResult>(Sirenix.Serialization.Utilities.ReferenceEqualityComparer<MemberInfo>.Default);

		private static readonly Dictionary<MemberInfo, CachedSerializationBackendResult> OdinWillSerializeCache_StrictPolicy = new Dictionary<MemberInfo, CachedSerializationBackendResult>(Sirenix.Serialization.Utilities.ReferenceEqualityComparer<MemberInfo>.Default);

		private static readonly Dictionary<ISerializationPolicy, Dictionary<MemberInfo, CachedSerializationBackendResult>> OdinWillSerializeCache_CustomPolicies = new Dictionary<ISerializationPolicy, Dictionary<MemberInfo, CachedSerializationBackendResult>>(Sirenix.Serialization.Utilities.ReferenceEqualityComparer<ISerializationPolicy>.Default);

		public static bool OdinWillSerialize(MemberInfo member, bool serializeUnityFields, ISerializationPolicy policy = null)
		{
			Dictionary<MemberInfo, CachedSerializationBackendResult> value;
			if (policy == null || policy == UnityPolicy)
			{
				value = OdinWillSerializeCache_UnityPolicy;
			}
			else if (policy == EverythingPolicy)
			{
				value = OdinWillSerializeCache_EverythingPolicy;
			}
			else if (policy == StrictPolicy)
			{
				value = OdinWillSerializeCache_StrictPolicy;
			}
			else
			{
				lock (OdinWillSerializeCache_CustomPolicies)
				{
					if (!OdinWillSerializeCache_CustomPolicies.TryGetValue(policy, out value))
					{
						value = new Dictionary<MemberInfo, CachedSerializationBackendResult>(Sirenix.Serialization.Utilities.ReferenceEqualityComparer<MemberInfo>.Default);
						OdinWillSerializeCache_CustomPolicies.Add(policy, value);
					}
				}
			}
			lock (value)
			{
				if (!value.TryGetValue(member, out var value2))
				{
					value2 = default(CachedSerializationBackendResult);
					if (serializeUnityFields)
					{
						value2.SerializeUnityFieldsTrueResult = CalculateOdinWillSerialize(member, serializeUnityFields, policy ?? UnityPolicy);
						value2.HasCalculatedSerializeUnityFieldsTrueResult = true;
					}
					else
					{
						value2.SerializeUnityFieldsFalseResult = CalculateOdinWillSerialize(member, serializeUnityFields, policy ?? UnityPolicy);
						value2.HasCalculatedSerializeUnityFieldsFalseResult = true;
					}
					value.Add(member, value2);
				}
				else if (serializeUnityFields && !value2.HasCalculatedSerializeUnityFieldsTrueResult)
				{
					value2.SerializeUnityFieldsTrueResult = CalculateOdinWillSerialize(member, serializeUnityFields, policy ?? UnityPolicy);
					value2.HasCalculatedSerializeUnityFieldsTrueResult = true;
					value[member] = value2;
				}
				else if (!serializeUnityFields && !value2.HasCalculatedSerializeUnityFieldsFalseResult)
				{
					value2.SerializeUnityFieldsFalseResult = CalculateOdinWillSerialize(member, serializeUnityFields, policy ?? UnityPolicy);
					value2.HasCalculatedSerializeUnityFieldsFalseResult = true;
					value[member] = value2;
				}
				return serializeUnityFields ? value2.SerializeUnityFieldsTrueResult : value2.SerializeUnityFieldsFalseResult;
			}
		}

		private static bool CalculateOdinWillSerialize(MemberInfo member, bool serializeUnityFields, ISerializationPolicy policy)
		{
			if ((object)member.DeclaringType == typeof(UnityEngine.Object))
			{
				return false;
			}
			if (!policy.ShouldSerializeMember(member))
			{
				return false;
			}
			if (member is FieldInfo && member.IsDefined(typeof(OdinSerializeAttribute), inherit: true))
			{
				return true;
			}
			if (serializeUnityFields)
			{
				return true;
			}
			try
			{
				if ((object)SerializeReferenceAttributeType != null && member.IsDefined(SerializeReferenceAttributeType, inherit: true))
				{
					return false;
				}
			}
			catch
			{
			}
			if (GuessIfUnityWillSerialize(member))
			{
				return false;
			}
			return true;
		}

		public static bool GuessIfUnityWillSerialize(MemberInfo member)
		{
			if ((object)member == null)
			{
				throw new ArgumentNullException("member");
			}
			if (!UnityWillSerializeMembersCache.TryGetValue(member, out var value))
			{
				value = GuessIfUnityWillSerializePrivate(member);
				UnityWillSerializeMembersCache[member] = value;
			}
			return value;
		}

		private static bool GuessIfUnityWillSerializePrivate(MemberInfo member)
		{
			FieldInfo fieldInfo = member as FieldInfo;
			if ((object)fieldInfo == null || fieldInfo.IsStatic)
			{
				return false;
			}
			if (Sirenix.Serialization.Utilities.MemberInfoExtensions.IsDefined<NonSerializedAttribute>(fieldInfo))
			{
				return false;
			}
			if ((object)SerializeReferenceAttributeType != null && fieldInfo.IsDefined(SerializeReferenceAttributeType, inherit: true))
			{
				return true;
			}
			if (!typeof(UnityEngine.Object).IsAssignableFrom(fieldInfo.FieldType) && (object)fieldInfo.FieldType == fieldInfo.DeclaringType)
			{
				return false;
			}
			if (!fieldInfo.IsPublic && !Sirenix.Serialization.Utilities.MemberInfoExtensions.IsDefined<SerializeField>(fieldInfo))
			{
				return false;
			}
			if (Sirenix.Serialization.Utilities.MemberInfoExtensions.IsDefined<FixedBufferAttribute>(fieldInfo))
			{
				return Sirenix.Serialization.Utilities.UnityVersion.IsVersionOrGreater(2017, 1);
			}
			return GuessIfUnityWillSerialize(fieldInfo.FieldType);
		}

		public static bool GuessIfUnityWillSerialize(Type type)
		{
			if ((object)type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!UnityWillSerializeTypesCache.TryGetValue(type, out var value))
			{
				value = GuessIfUnityWillSerializePrivate(type);
				UnityWillSerializeTypesCache[type] = value;
			}
			return value;
		}

		private static bool GuessIfUnityWillSerializePrivate(Type type)
		{
			if (UnityNeverSerializesTypes.Contains(type))
			{
				return false;
			}
			if (typeof(UnityEngine.Object).IsAssignableFrom(type) && type.GetGenericArguments().Length == 0)
			{
				return true;
			}
			if (type.IsAbstract || type.IsInterface || (object)type == typeof(object))
			{
				return false;
			}
			if (type.IsEnum)
			{
				Type underlyingType = Enum.GetUnderlyingType(type);
				if (Sirenix.Serialization.Utilities.UnityVersion.IsVersionOrGreater(5, 6))
				{
					if ((object)underlyingType != typeof(long))
					{
						return (object)underlyingType != typeof(ulong);
					}
					return false;
				}
				if ((object)underlyingType != typeof(int))
				{
					return (object)underlyingType == typeof(byte);
				}
				return true;
			}
			if (type.IsPrimitive || (object)type == typeof(string))
			{
				return true;
			}
			if (typeof(Delegate).IsAssignableFrom(type))
			{
				return false;
			}
			if (typeof(UnityEventBase).IsAssignableFrom(type))
			{
				if (type.IsGenericType && !Sirenix.Serialization.Utilities.UnityVersion.IsVersionOrGreater(2020, 1))
				{
					return false;
				}
				if ((object)type != typeof(UnityEvent))
				{
					return Sirenix.Serialization.Utilities.TypeExtensions.IsDefined<SerializableAttribute>(type, inherit: false);
				}
				return true;
			}
			if (type.IsArray)
			{
				Type elementType = type.GetElementType();
				if (type.GetArrayRank() == 1 && !elementType.IsArray && !Sirenix.Serialization.Utilities.TypeExtensions.ImplementsOpenGenericClass(elementType, typeof(List<>)))
				{
					return GuessIfUnityWillSerialize(elementType);
				}
				return false;
			}
			if (type.IsGenericType && !type.IsGenericTypeDefinition && (object)type.GetGenericTypeDefinition() == typeof(List<>))
			{
				Type type2 = Sirenix.Serialization.Utilities.TypeExtensions.GetArgumentsOfInheritedOpenGenericClass(type, typeof(List<>))[0];
				if (type2.IsArray || Sirenix.Serialization.Utilities.TypeExtensions.ImplementsOpenGenericClass(type2, typeof(List<>)))
				{
					return false;
				}
				return GuessIfUnityWillSerialize(type2);
			}
			if (type.Assembly.FullName.StartsWith("UnityEngine", StringComparison.InvariantCulture) || type.Assembly.FullName.StartsWith("UnityEditor", StringComparison.InvariantCulture))
			{
				return true;
			}
			if (type.IsGenericType && !Sirenix.Serialization.Utilities.UnityVersion.IsVersionOrGreater(2020, 1))
			{
				return false;
			}
			if ((object)type.Assembly == typeof(string).Assembly)
			{
				return false;
			}
			if (Sirenix.Serialization.Utilities.TypeExtensions.IsDefined<SerializableAttribute>(type, inherit: false))
			{
				if (Sirenix.Serialization.Utilities.UnityVersion.IsVersionOrGreater(4, 5))
				{
					return true;
				}
				return type.IsClass;
			}
			if (!Sirenix.Serialization.Utilities.UnityVersion.IsVersionOrGreater(2018, 2))
			{
				Type baseType = type.BaseType;
				while ((object)baseType != null && (object)baseType != typeof(object))
				{
					if (baseType.IsGenericType && baseType.GetGenericTypeDefinition().FullName == "UnityEngine.Networking.SyncListStruct`1")
					{
						return true;
					}
					baseType = baseType.BaseType;
				}
			}
			return false;
		}

		public static void SerializeUnityObject(UnityEngine.Object unityObject, ref SerializationData data, bool serializeUnityFields = false, SerializationContext context = null)
		{
			if (unityObject == null)
			{
				throw new ArgumentNullException("unityObject");
			}
			DataFormat dataFormat = (unityObject as IOverridesSerializationFormat)?.GetFormatToSerializeAs(isPlayer: true) ?? (GlobalConfig<GlobalSerializationConfig>.HasInstanceLoaded ? GlobalConfig<GlobalSerializationConfig>.Instance.BuildSerializationFormat : DataFormat.Binary);
			if (dataFormat == DataFormat.Nodes)
			{
				Debug.LogWarning("The serialization format '" + dataFormat.ToString() + "' is disabled outside of the editor. Defaulting to the format '" + DataFormat.Binary.ToString() + "' instead.");
				dataFormat = DataFormat.Binary;
			}
			SerializeUnityObject(unityObject, ref data.SerializedBytes, ref data.ReferencedUnityObjects, dataFormat);
			data.SerializedFormat = dataFormat;
		}

		public static void SerializeUnityObject(UnityEngine.Object unityObject, ref string base64Bytes, ref List<UnityEngine.Object> referencedUnityObjects, DataFormat format, bool serializeUnityFields = false, SerializationContext context = null)
		{
			byte[] bytes = null;
			SerializeUnityObject(unityObject, ref bytes, ref referencedUnityObjects, format, serializeUnityFields, context);
			base64Bytes = Convert.ToBase64String(bytes);
		}

		public static void SerializeUnityObject(UnityEngine.Object unityObject, ref byte[] bytes, ref List<UnityEngine.Object> referencedUnityObjects, DataFormat format, bool serializeUnityFields = false, SerializationContext context = null)
		{
			if (unityObject == null)
			{
				throw new ArgumentNullException("unityObject");
			}
			if (format == DataFormat.Nodes)
			{
				Debug.LogError("The serialization data format '" + format.ToString() + "' is not supported by this method. You must create your own writer.");
				return;
			}
			if (referencedUnityObjects == null)
			{
				referencedUnityObjects = new List<UnityEngine.Object>();
			}
			else
			{
				referencedUnityObjects.Clear();
			}
			Sirenix.Serialization.Utilities.Cache<CachedMemoryStream> cache2 = Sirenix.Serialization.Utilities.Cache<CachedMemoryStream>.Claim();
			Sirenix.Serialization.Utilities.Cache<UnityReferenceResolver> cache = Sirenix.Serialization.Utilities.Cache<UnityReferenceResolver>.Claim();
			cache.Value.SetReferencedUnityObjects(referencedUnityObjects);
			if (context != null)
			{
				context.IndexReferenceResolver = cache.Value;
				SerializeUnityObject(unityObject, GetCachedUnityWriter(format, cache2.Value.MemoryStream, context), serializeUnityFields);
			}
			else
			{
				Sirenix.Serialization.Utilities.Cache<SerializationContext> cache3 = Sirenix.Serialization.Utilities.Cache<SerializationContext>.Claim();
				cache3.Value.Config.SerializationPolicy = SerializationPolicies.Unity;
				if (GlobalConfig<GlobalSerializationConfig>.HasInstanceLoaded)
				{
					cache3.Value.Config.DebugContext.ErrorHandlingPolicy = GlobalConfig<GlobalSerializationConfig>.Instance.ErrorHandlingPolicy;
					cache3.Value.Config.DebugContext.LoggingPolicy = GlobalConfig<GlobalSerializationConfig>.Instance.LoggingPolicy;
					cache3.Value.Config.DebugContext.Logger = GlobalConfig<GlobalSerializationConfig>.Instance.Logger;
				}
				else
				{
					cache3.Value.Config.DebugContext.ErrorHandlingPolicy = ErrorHandlingPolicy.Resilient;
					cache3.Value.Config.DebugContext.LoggingPolicy = LoggingPolicy.LogErrors;
					cache3.Value.Config.DebugContext.Logger = DefaultLoggers.UnityLogger;
				}
				cache3.Value.IndexReferenceResolver = cache.Value;
				SerializeUnityObject(unityObject, GetCachedUnityWriter(format, cache2.Value.MemoryStream, cache3), serializeUnityFields);
			}
			bytes = cache2.Value.MemoryStream.ToArray();
		}

		public static void SerializeUnityObject(UnityEngine.Object unityObject, IDataWriter writer, bool serializeUnityFields = false)
		{
			if (unityObject == null)
			{
				throw new ArgumentNullException("unityObject");
			}
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			try
			{
				writer.PrepareNewSerializationSession();
				MemberInfo[] serializableMembers = FormatterUtilities.GetSerializableMembers(unityObject.GetType(), writer.Context.Config.SerializationPolicy);
				object instance = unityObject;
				foreach (MemberInfo memberInfo in serializableMembers)
				{
					Sirenix.Serialization.Utilities.WeakValueGetter weakValueGetter = null;
					if (!OdinWillSerialize(memberInfo, serializeUnityFields, writer.Context.Config.SerializationPolicy) || (weakValueGetter = GetCachedUnityMemberGetter(memberInfo)) == null)
					{
						continue;
					}
					object obj = weakValueGetter(ref instance);
					if (obj == null || (object)obj.GetType() != typeof(SerializationData))
					{
						Serializer serializer = Serializer.Get(FormatterUtilities.GetContainedType(memberInfo));
						try
						{
							serializer.WriteValueWeak(memberInfo.Name, obj, writer);
						}
						catch (Exception exception)
						{
							writer.Context.Config.DebugContext.LogException(exception);
						}
					}
				}
				writer.FlushToStream();
			}
			catch (SerializationAbortException innerException)
			{
				throw new SerializationAbortException("Serialization of type '" + Sirenix.Serialization.Utilities.TypeExtensions.GetNiceFullName(unityObject.GetType()) + "' aborted.", innerException);
			}
			catch (Exception ex)
			{
				Debug.LogException(new Exception("Exception thrown while serializing type '" + Sirenix.Serialization.Utilities.TypeExtensions.GetNiceFullName(unityObject.GetType()) + "': " + ex.Message, ex));
			}
		}

		public static void DeserializeUnityObject(UnityEngine.Object unityObject, ref SerializationData data, DeserializationContext context = null)
		{
			DeserializeUnityObject(unityObject, ref data, context, isPrefabData: false, null);
		}

		private static void DeserializeUnityObject(UnityEngine.Object unityObject, ref SerializationData data, DeserializationContext context, bool isPrefabData, List<UnityEngine.Object> prefabInstanceUnityObjects)
		{
			if (unityObject == null)
			{
				throw new ArgumentNullException("unityObject");
			}
			if (isPrefabData && prefabInstanceUnityObjects == null)
			{
				prefabInstanceUnityObjects = new List<UnityEngine.Object>();
			}
			if (data.SerializedBytes != null && data.SerializedBytes.Length != 0 && (data.SerializationNodes == null || data.SerializationNodes.Count == 0))
			{
				if (data.SerializedFormat == DataFormat.Nodes)
				{
					DataFormat dataFormat = ((data.SerializedBytes[0] == 123) ? DataFormat.JSON : DataFormat.Binary);
					try
					{
						string text = ProperBitConverter.BytesToHexString(data.SerializedBytes);
						Debug.LogWarning(string.Concat("Serialization data has only bytes stored, but the serialized format is marked as being 'Nodes', which is incompatible with data stored as a byte array. Based on the appearance of the serialized bytes, Odin has guessed that the data format is '", dataFormat, "', and will attempt to deserialize the bytes using that format. The serialized bytes follow, converted to a hex string: ", text));
					}
					catch
					{
					}
					DeserializeUnityObject(unityObject, ref data.SerializedBytes, ref data.ReferencedUnityObjects, dataFormat, context);
				}
				else
				{
					DeserializeUnityObject(unityObject, ref data.SerializedBytes, ref data.ReferencedUnityObjects, data.SerializedFormat, context);
				}
				ApplyPrefabModifications(unityObject, data.PrefabModifications, data.PrefabModificationsReferencedUnityObjects);
				return;
			}
			Sirenix.Serialization.Utilities.Cache<DeserializationContext> cache = null;
			try
			{
				if (context == null)
				{
					cache = Sirenix.Serialization.Utilities.Cache<DeserializationContext>.Claim();
					context = cache;
					context.Config.SerializationPolicy = SerializationPolicies.Unity;
					if (GlobalConfig<GlobalSerializationConfig>.HasInstanceLoaded)
					{
						context.Config.DebugContext.ErrorHandlingPolicy = GlobalConfig<GlobalSerializationConfig>.Instance.ErrorHandlingPolicy;
						context.Config.DebugContext.LoggingPolicy = GlobalConfig<GlobalSerializationConfig>.Instance.LoggingPolicy;
						context.Config.DebugContext.Logger = GlobalConfig<GlobalSerializationConfig>.Instance.Logger;
					}
					else
					{
						context.Config.DebugContext.ErrorHandlingPolicy = ErrorHandlingPolicy.Resilient;
						context.Config.DebugContext.LoggingPolicy = LoggingPolicy.LogErrors;
						context.Config.DebugContext.Logger = DefaultLoggers.UnityLogger;
					}
				}
				IOverridesSerializationPolicy overridesSerializationPolicy = unityObject as IOverridesSerializationPolicy;
				if (overridesSerializationPolicy != null)
				{
					ISerializationPolicy serializationPolicy = overridesSerializationPolicy.SerializationPolicy;
					if (serializationPolicy != null)
					{
						context.Config.SerializationPolicy = serializationPolicy;
					}
				}
				if (isPrefabData || Sirenix.Serialization.Utilities.UnityExtensions.SafeIsUnityNull(data.Prefab))
				{
					goto IL_02c2;
				}
				if (data.Prefab is ISupportsPrefabSerialization)
				{
					if ((object)data.Prefab == unityObject && data.PrefabModifications != null && data.PrefabModifications.Count > 0)
					{
						goto IL_02c2;
					}
					SerializationData data2 = (data.Prefab as ISupportsPrefabSerialization).SerializationData;
					if (!data2.ContainsData)
					{
						DeserializeUnityObject(unityObject, ref data, context, isPrefabData: true, data.ReferencedUnityObjects);
					}
					else
					{
						DeserializeUnityObject(unityObject, ref data2, context, isPrefabData: true, data.ReferencedUnityObjects);
					}
					ApplyPrefabModifications(unityObject, data.PrefabModifications, data.PrefabModificationsReferencedUnityObjects);
					return;
				}
				if ((object)data.Prefab.GetType() != typeof(UnityEngine.Object))
				{
					Debug.LogWarning("The type " + Sirenix.Serialization.Utilities.TypeExtensions.GetNiceName(data.Prefab.GetType()) + " no longer supports special prefab serialization (the interface " + Sirenix.Serialization.Utilities.TypeExtensions.GetNiceName(typeof(ISupportsPrefabSerialization)) + ") upon deserialization of an instance of a prefab; prefab data may be lost. Has a type been lost?");
				}
				goto IL_02c2;
				IL_02c2:
				List<UnityEngine.Object> referencedUnityObjects = (isPrefabData ? prefabInstanceUnityObjects : data.ReferencedUnityObjects);
				if (data.SerializedFormat == DataFormat.Nodes)
				{
					SerializationNodeDataReader serializationNodeDataReader = new SerializationNodeDataReader(context);
					Sirenix.Serialization.Utilities.Cache<UnityReferenceResolver> cache2 = Sirenix.Serialization.Utilities.Cache<UnityReferenceResolver>.Claim();
					cache2.Value.SetReferencedUnityObjects(referencedUnityObjects);
					context.IndexReferenceResolver = cache2.Value;
					serializationNodeDataReader.Nodes = data.SerializationNodes;
					DeserializeUnityObject(unityObject, serializationNodeDataReader);
				}
				else if (data.SerializedBytes != null && data.SerializedBytes.Length != 0)
				{
					DeserializeUnityObject(unityObject, ref data.SerializedBytes, ref referencedUnityObjects, data.SerializedFormat, context);
				}
				else
				{
					DeserializeUnityObject(unityObject, ref data.SerializedBytesString, ref referencedUnityObjects, data.SerializedFormat, context);
				}
				ApplyPrefabModifications(unityObject, data.PrefabModifications, data.PrefabModificationsReferencedUnityObjects);
			}
			finally
			{
				if (cache != null)
				{
					Sirenix.Serialization.Utilities.Cache<DeserializationContext>.Release(cache);
				}
			}
		}

		public static void DeserializeUnityObject(UnityEngine.Object unityObject, ref string base64Bytes, ref List<UnityEngine.Object> referencedUnityObjects, DataFormat format, DeserializationContext context = null)
		{
			if (!string.IsNullOrEmpty(base64Bytes))
			{
				byte[] bytes = null;
				try
				{
					bytes = Convert.FromBase64String(base64Bytes);
				}
				catch (FormatException)
				{
					Debug.LogError("Invalid base64 string when deserializing data: " + base64Bytes);
				}
				if (bytes != null)
				{
					DeserializeUnityObject(unityObject, ref bytes, ref referencedUnityObjects, format, context);
				}
			}
		}

		public static void DeserializeUnityObject(UnityEngine.Object unityObject, ref byte[] bytes, ref List<UnityEngine.Object> referencedUnityObjects, DataFormat format, DeserializationContext context = null)
		{
			if (unityObject == null)
			{
				throw new ArgumentNullException("unityObject");
			}
			if (bytes == null || bytes.Length == 0)
			{
				return;
			}
			if (format == DataFormat.Nodes)
			{
				try
				{
					Debug.LogError("The serialization data format '" + format.ToString() + "' is not supported by this method. You must create your own reader.");
				}
				catch
				{
				}
				return;
			}
			if (referencedUnityObjects == null)
			{
				referencedUnityObjects = new List<UnityEngine.Object>();
			}
			Sirenix.Serialization.Utilities.Cache<CachedMemoryStream> cache = Sirenix.Serialization.Utilities.Cache<CachedMemoryStream>.Claim();
			Sirenix.Serialization.Utilities.Cache<UnityReferenceResolver> cache2 = Sirenix.Serialization.Utilities.Cache<UnityReferenceResolver>.Claim();
			cache.Value.MemoryStream.Write(bytes, 0, bytes.Length);
			cache.Value.MemoryStream.Position = 0L;
			cache2.Value.SetReferencedUnityObjects(referencedUnityObjects);
			if (context != null)
			{
				context.IndexReferenceResolver = cache2.Value;
				DeserializeUnityObject(unityObject, GetCachedUnityReader(format, cache.Value.MemoryStream, context));
				return;
			}
			Sirenix.Serialization.Utilities.Cache<DeserializationContext> cache3 = Sirenix.Serialization.Utilities.Cache<DeserializationContext>.Claim();
			cache3.Value.Config.SerializationPolicy = SerializationPolicies.Unity;
			if (GlobalConfig<GlobalSerializationConfig>.HasInstanceLoaded)
			{
				cache3.Value.Config.DebugContext.ErrorHandlingPolicy = GlobalConfig<GlobalSerializationConfig>.Instance.ErrorHandlingPolicy;
				cache3.Value.Config.DebugContext.LoggingPolicy = GlobalConfig<GlobalSerializationConfig>.Instance.LoggingPolicy;
				cache3.Value.Config.DebugContext.Logger = GlobalConfig<GlobalSerializationConfig>.Instance.Logger;
			}
			else
			{
				cache3.Value.Config.DebugContext.ErrorHandlingPolicy = ErrorHandlingPolicy.Resilient;
				cache3.Value.Config.DebugContext.LoggingPolicy = LoggingPolicy.LogErrors;
				cache3.Value.Config.DebugContext.Logger = DefaultLoggers.UnityLogger;
			}
			cache3.Value.IndexReferenceResolver = cache2.Value;
			DeserializeUnityObject(unityObject, GetCachedUnityReader(format, cache.Value.MemoryStream, cache3));
		}

		public static void DeserializeUnityObject(UnityEngine.Object unityObject, IDataReader reader)
		{
			if (unityObject == null)
			{
				throw new ArgumentNullException("unityObject");
			}
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			IOverridesSerializationPolicy overridesSerializationPolicy = unityObject as IOverridesSerializationPolicy;
			if (overridesSerializationPolicy != null)
			{
				ISerializationPolicy serializationPolicy = overridesSerializationPolicy.SerializationPolicy;
				if (serializationPolicy != null)
				{
					reader.Context.Config.SerializationPolicy = serializationPolicy;
				}
			}
			try
			{
				reader.PrepareNewSerializationSession();
				Dictionary<string, MemberInfo> serializableMembersMap = FormatterUtilities.GetSerializableMembersMap(unityObject.GetType(), reader.Context.Config.SerializationPolicy);
				int num = 0;
				object instance = unityObject;
				EntryType entryType;
				string name;
				while ((entryType = reader.PeekEntry(out name)) != EntryType.EndOfNode && entryType != EntryType.EndOfArray && entryType != EntryType.EndOfStream)
				{
					MemberInfo value = null;
					Sirenix.Serialization.Utilities.WeakValueSetter weakValueSetter = null;
					bool flag = false;
					if (entryType == EntryType.Invalid)
					{
						string text = "Encountered invalid entry while reading serialization data for Unity object of type '" + Sirenix.Serialization.Utilities.TypeExtensions.GetNiceFullName(unityObject.GetType()) + "'. This likely means that Unity has filled Odin's stored serialization data with garbage, which can randomly happen after upgrading the Unity version of the project, or when otherwise doing things that have a lot of fragile interactions with the asset database. Locating the asset which causes this error log and causing it to reserialize (IE, modifying it and then causing it to be saved to disk) is likely to 'fix' the issue and make this message go away. Experience shows that this issue is particularly likely to occur on prefab instances, and if this is the case, the parent prefab is also under suspicion, and should be re-saved and re-imported. Note that DATA MAY HAVE BEEN LOST, and you should verify with your version control system (you're using one, right?!) that everything is alright, and if not, use it to rollback the asset to recover your data.\n\n\n";
						text = text + "IF YOU HAVE CONSISTENT REPRODUCTION STEPS THAT MAKE THIS ISSUE REOCCUR, please report it at this issue at 'https://bitbucket.org/sirenix/odin-inspector/issues/526', and copy paste this debug message into your comment, along with any potential actions or recent changes in the project that might have happened to cause this message to occur. If the data dump in this message is cut off, please find the editor's log file (see https://docs.unity3d.com/Manual/LogFiles.html) and copy paste the full version of this message from there.\n\n\nData dump:\n\n    Reader type: " + reader.GetType().Name + "\n";
						try
						{
							text = text + "    Data dump: " + reader.GetDataDump();
						}
						finally
						{
							reader.Context.Config.DebugContext.LogError(text);
							flag = true;
						}
					}
					else if (string.IsNullOrEmpty(name))
					{
						reader.Context.Config.DebugContext.LogError(string.Concat("Entry of type \"", entryType, "\" in node \"", reader.CurrentNodeName, "\" is missing a name."));
						flag = true;
					}
					else if (!serializableMembersMap.TryGetValue(name, out value) || (weakValueSetter = GetCachedUnityMemberSetter(value)) == null)
					{
						flag = true;
					}
					if (flag)
					{
						reader.SkipEntry();
						continue;
					}
					Type containedType = FormatterUtilities.GetContainedType(value);
					Serializer serializer = Serializer.Get(containedType);
					try
					{
						object value2 = serializer.ReadValueWeak(reader);
						weakValueSetter(ref instance, value2);
					}
					catch (Exception exception)
					{
						reader.Context.Config.DebugContext.LogException(exception);
					}
					num++;
					if (num <= 1000)
					{
						continue;
					}
					reader.Context.Config.DebugContext.LogError("Breaking out of infinite reading loop! (Read more than a thousand entries for one type!)");
					break;
				}
			}
			catch (SerializationAbortException innerException)
			{
				throw new SerializationAbortException("Deserialization of type '" + Sirenix.Serialization.Utilities.TypeExtensions.GetNiceFullName(unityObject.GetType()) + "' aborted.", innerException);
			}
			catch (Exception ex)
			{
				Debug.LogException(new Exception("Exception thrown while deserializing type '" + Sirenix.Serialization.Utilities.TypeExtensions.GetNiceFullName(unityObject.GetType()) + "': " + ex.Message, ex));
			}
		}

		public static List<string> SerializePrefabModifications(List<PrefabModification> modifications, ref List<UnityEngine.Object> referencedUnityObjects)
		{
			if (referencedUnityObjects == null)
			{
				referencedUnityObjects = new List<UnityEngine.Object>();
			}
			else if (referencedUnityObjects.Count > 0)
			{
				referencedUnityObjects.Clear();
			}
			if (modifications == null || modifications.Count == 0)
			{
				return new List<string>();
			}
			modifications.Sort(delegate(PrefabModification a, PrefabModification b)
			{
				int num = a.Path.CompareTo(b.Path);
				if (num == 0)
				{
					if ((a.ModificationType == PrefabModificationType.ListLength || a.ModificationType == PrefabModificationType.Dictionary) && b.ModificationType == PrefabModificationType.Value)
					{
						return 1;
					}
					if (a.ModificationType == PrefabModificationType.Value && (b.ModificationType == PrefabModificationType.ListLength || b.ModificationType == PrefabModificationType.Dictionary))
					{
						return -1;
					}
				}
				return num;
			});
			List<string> list = new List<string>();
			Sirenix.Serialization.Utilities.Cache<SerializationContext> cache2 = Sirenix.Serialization.Utilities.Cache<SerializationContext>.Claim();
			Sirenix.Serialization.Utilities.Cache<CachedMemoryStream> cache = CachedMemoryStream.Claim();
			JsonDataWriter jsonDataWriter = (JsonDataWriter)GetCachedUnityWriter(DataFormat.JSON, cache.Value.MemoryStream, cache2);
			Sirenix.Serialization.Utilities.Cache<UnityReferenceResolver> cache3 = Sirenix.Serialization.Utilities.Cache<UnityReferenceResolver>.Claim();
			jsonDataWriter.PrepareNewSerializationSession();
			jsonDataWriter.FormatAsReadable = false;
			jsonDataWriter.EnableTypeOptimization = false;
			cache3.Value.SetReferencedUnityObjects(referencedUnityObjects);
			jsonDataWriter.Context.IndexReferenceResolver = cache3.Value;
			for (int i = 0; i < modifications.Count; i++)
			{
				PrefabModification prefabModification = modifications[i];
				if (prefabModification.ModificationType == PrefabModificationType.ListLength)
				{
					jsonDataWriter.MarkJustStarted();
					jsonDataWriter.WriteString("path", prefabModification.Path);
					jsonDataWriter.WriteInt32("length", prefabModification.NewLength);
					jsonDataWriter.FlushToStream();
					list.Add(GetStringFromStreamAndReset(cache.Value.MemoryStream));
				}
				else if (prefabModification.ModificationType == PrefabModificationType.Value)
				{
					jsonDataWriter.MarkJustStarted();
					jsonDataWriter.WriteString("path", prefabModification.Path);
					if (prefabModification.ReferencePaths != null && prefabModification.ReferencePaths.Count > 0)
					{
						jsonDataWriter.BeginStructNode("references", null);
						for (int j = 0; j < prefabModification.ReferencePaths.Count; j++)
						{
							jsonDataWriter.WriteString(null, prefabModification.ReferencePaths[j]);
						}
						jsonDataWriter.EndNode("references");
					}
					Serializer<object> serializer = Serializer.Get<object>();
					serializer.WriteValueWeak("value", prefabModification.ModifiedValue, jsonDataWriter);
					jsonDataWriter.FlushToStream();
					list.Add(GetStringFromStreamAndReset(cache.Value.MemoryStream));
				}
				else if (prefabModification.ModificationType == PrefabModificationType.Dictionary)
				{
					jsonDataWriter.MarkJustStarted();
					jsonDataWriter.WriteString("path", prefabModification.Path);
					Serializer.Get<object[]>().WriteValue("add_keys", prefabModification.DictionaryKeysAdded, jsonDataWriter);
					Serializer.Get<object[]>().WriteValue("remove_keys", prefabModification.DictionaryKeysRemoved, jsonDataWriter);
					jsonDataWriter.FlushToStream();
					list.Add(GetStringFromStreamAndReset(cache.Value.MemoryStream));
				}
				jsonDataWriter.Context.ResetInternalReferences();
			}
			return list;
		}

		private static string GetStringFromStreamAndReset(Stream stream)
		{
			byte[] array = new byte[stream.Position];
			stream.Position = 0L;
			stream.Read(array, 0, array.Length);
			stream.Position = 0L;
			return Encoding.UTF8.GetString(array);
		}

		public static List<PrefabModification> DeserializePrefabModifications(List<string> modifications, List<UnityEngine.Object> referencedUnityObjects)
		{
			if (modifications == null || modifications.Count == 0)
			{
				return new List<PrefabModification>();
			}
			List<PrefabModification> list = new List<PrefabModification>();
			int num = 0;
			for (int i = 0; i < modifications.Count; i++)
			{
				int num2 = modifications[i].Length * 2;
				if (num2 > num)
				{
					num = num2;
				}
			}
			Sirenix.Serialization.Utilities.Cache<DeserializationContext> cache2 = Sirenix.Serialization.Utilities.Cache<DeserializationContext>.Claim();
			Sirenix.Serialization.Utilities.Cache<CachedMemoryStream> cache = CachedMemoryStream.Claim(num);
			JsonDataReader jsonDataReader = (JsonDataReader)GetCachedUnityReader(DataFormat.JSON, cache.Value.MemoryStream, cache2);
			Sirenix.Serialization.Utilities.Cache<UnityReferenceResolver> cache3 = Sirenix.Serialization.Utilities.Cache<UnityReferenceResolver>.Claim();
			MemoryStream memoryStream = cache.Value.MemoryStream;
			cache3.Value.SetReferencedUnityObjects(referencedUnityObjects);
			jsonDataReader.Context.IndexReferenceResolver = cache3.Value;
			for (int j = 0; j < modifications.Count; j++)
			{
				string s = modifications[j];
				byte[] bytes = Encoding.UTF8.GetBytes(s);
				memoryStream.SetLength(bytes.Length);
				memoryStream.Position = 0L;
				memoryStream.Write(bytes, 0, bytes.Length);
				memoryStream.Position = 0L;
				PrefabModification prefabModification = new PrefabModification();
				jsonDataReader.PrepareNewSerializationSession();
				EntryType entryType = jsonDataReader.PeekEntry(out var name);
				if (entryType == EntryType.EndOfStream)
				{
					jsonDataReader.SkipEntry();
				}
				while ((entryType = jsonDataReader.PeekEntry(out name)) != EntryType.EndOfNode && entryType != EntryType.EndOfArray && entryType != EntryType.EndOfStream)
				{
					if (name == null)
					{
						Debug.LogError(string.Concat("Unexpected entry of type ", entryType, " without a name."));
						jsonDataReader.SkipEntry();
					}
					else if (name.Equals("path", StringComparison.InvariantCultureIgnoreCase))
					{
						jsonDataReader.ReadString(out prefabModification.Path);
					}
					else if (name.Equals("length", StringComparison.InvariantCultureIgnoreCase))
					{
						jsonDataReader.ReadInt32(out prefabModification.NewLength);
						prefabModification.ModificationType = PrefabModificationType.ListLength;
					}
					else if (name.Equals("references", StringComparison.InvariantCultureIgnoreCase))
					{
						prefabModification.ReferencePaths = new List<string>();
						jsonDataReader.EnterNode(out var _);
						while (jsonDataReader.PeekEntry(out name) == EntryType.String)
						{
							jsonDataReader.ReadString(out var value);
							prefabModification.ReferencePaths.Add(value);
						}
						jsonDataReader.ExitNode();
					}
					else if (name.Equals("value", StringComparison.InvariantCultureIgnoreCase))
					{
						prefabModification.ModifiedValue = Serializer.Get<object>().ReadValue(jsonDataReader);
						prefabModification.ModificationType = PrefabModificationType.Value;
					}
					else if (name.Equals("add_keys", StringComparison.InvariantCultureIgnoreCase))
					{
						prefabModification.DictionaryKeysAdded = Serializer.Get<object[]>().ReadValue(jsonDataReader);
						prefabModification.ModificationType = PrefabModificationType.Dictionary;
					}
					else if (name.Equals("remove_keys", StringComparison.InvariantCultureIgnoreCase))
					{
						prefabModification.DictionaryKeysRemoved = Serializer.Get<object[]>().ReadValue(jsonDataReader);
						prefabModification.ModificationType = PrefabModificationType.Dictionary;
					}
					else
					{
						Debug.LogError("Unexpected entry name '" + name + "' while deserializing prefab modifications.");
						jsonDataReader.SkipEntry();
					}
				}
				if (prefabModification.Path != null)
				{
					list.Add(prefabModification);
				}
			}
			return list;
		}

		public static object CreateDefaultUnityInitializedObject(Type type)
		{
			return CreateDefaultUnityInitializedObject(type, 0);
		}

		private static object CreateDefaultUnityInitializedObject(Type type, int depth)
		{
			if (depth > 5)
			{
				return null;
			}
			if (!GuessIfUnityWillSerialize(type))
			{
				if (!type.IsValueType)
				{
					return null;
				}
				return Activator.CreateInstance(type);
			}
			if ((object)type == typeof(string))
			{
				return "";
			}
			if (type.IsEnum)
			{
				Array values = Enum.GetValues(type);
				if (values.Length <= 0)
				{
					return Enum.ToObject(type, 0);
				}
				return values.GetValue(0);
			}
			if (type.IsPrimitive)
			{
				return Activator.CreateInstance(type);
			}
			if (type.IsArray)
			{
				return Array.CreateInstance(type.GetElementType(), 0);
			}
			if (Sirenix.Serialization.Utilities.TypeExtensions.ImplementsOpenGenericClass(type, typeof(List<>)) || typeof(UnityEventBase).IsAssignableFrom(type))
			{
				try
				{
					return Activator.CreateInstance(type);
				}
				catch
				{
					return null;
				}
			}
			if (typeof(UnityEngine.Object).IsAssignableFrom(type))
			{
				return null;
			}
			if ((type.Assembly.GetName().Name.StartsWith("UnityEngine") || type.Assembly.GetName().Name.StartsWith("UnityEditor")) && (object)type.GetConstructor(Type.EmptyTypes) != null)
			{
				try
				{
					return Activator.CreateInstance(type);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					return null;
				}
			}
			if ((object)type.GetConstructor(Type.EmptyTypes) != null)
			{
				return Activator.CreateInstance(type);
			}
			object uninitializedObject = FormatterServices.GetUninitializedObject(type);
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (GuessIfUnityWillSerialize(fieldInfo))
				{
					fieldInfo.SetValue(uninitializedObject, CreateDefaultUnityInitializedObject(fieldInfo.FieldType, depth + 1));
				}
			}
			return uninitializedObject;
		}

		private static void ApplyPrefabModifications(UnityEngine.Object unityObject, List<string> modificationData, List<UnityEngine.Object> referencedUnityObjects)
		{
			if (unityObject == null)
			{
				throw new ArgumentNullException("unityObject");
			}
			if (modificationData == null || modificationData.Count == 0)
			{
				return;
			}
			List<PrefabModification> list = DeserializePrefabModifications(modificationData, referencedUnityObjects);
			for (int i = 0; i < list.Count; i++)
			{
				PrefabModification prefabModification = list[i];
				try
				{
					prefabModification.Apply(unityObject);
				}
				catch (Exception exception)
				{
					Debug.Log("The following exception was thrown when trying to apply a prefab modification for path '" + prefabModification.Path + "':");
					Debug.LogException(exception);
				}
			}
		}

		private static Sirenix.Serialization.Utilities.WeakValueGetter GetCachedUnityMemberGetter(MemberInfo member)
		{
			if (!UnityMemberGetters.TryGetValue(member, out var value))
			{
				value = ((member is FieldInfo) ? Sirenix.Serialization.Utilities.EmitUtilities.CreateWeakInstanceFieldGetter(member.DeclaringType, member as FieldInfo) : ((!(member is PropertyInfo)) ? ((Sirenix.Serialization.Utilities.WeakValueGetter)delegate(ref object instance)
				{
					return FormatterUtilities.GetMemberValue(member, instance);
				}) : Sirenix.Serialization.Utilities.EmitUtilities.CreateWeakInstancePropertyGetter(member.DeclaringType, member as PropertyInfo)));
				UnityMemberGetters.Add(member, value);
			}
			return value;
		}

		private static Sirenix.Serialization.Utilities.WeakValueSetter GetCachedUnityMemberSetter(MemberInfo member)
		{
			if (!UnityMemberSetters.TryGetValue(member, out var value2))
			{
				value2 = ((member is FieldInfo) ? Sirenix.Serialization.Utilities.EmitUtilities.CreateWeakInstanceFieldSetter(member.DeclaringType, member as FieldInfo) : ((!(member is PropertyInfo)) ? ((Sirenix.Serialization.Utilities.WeakValueSetter)delegate(ref object instance, object value)
				{
					FormatterUtilities.SetMemberValue(member, instance, value);
				}) : Sirenix.Serialization.Utilities.EmitUtilities.CreateWeakInstancePropertySetter(member.DeclaringType, member as PropertyInfo)));
				UnityMemberSetters.Add(member, value2);
			}
			return value2;
		}

		private static IDataWriter GetCachedUnityWriter(DataFormat format, Stream stream, SerializationContext context)
		{
			if (!UnityWriters.TryGetValue(format, out var value))
			{
				value = SerializationUtility.CreateWriter(stream, context, format);
				UnityWriters.Add(format, value);
			}
			else
			{
				value.Context = context;
				if (value is BinaryDataWriter)
				{
					(value as BinaryDataWriter).Stream = stream;
				}
				else if (value is JsonDataWriter)
				{
					(value as JsonDataWriter).Stream = stream;
				}
			}
			return value;
		}

		private static IDataReader GetCachedUnityReader(DataFormat format, Stream stream, DeserializationContext context)
		{
			if (!UnityReaders.TryGetValue(format, out var value))
			{
				value = SerializationUtility.CreateReader(stream, context, format);
				UnityReaders.Add(format, value);
			}
			else
			{
				value.Context = context;
				if (value is BinaryDataReader)
				{
					(value as BinaryDataReader).Stream = stream;
				}
				else if (value is JsonDataReader)
				{
					(value as JsonDataReader).Stream = stream;
				}
			}
			return value;
		}
	}
}
