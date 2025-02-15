using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.Serialization.Utilities;
using UnityEngine;

namespace Sirenix.Serialization
{
	public static class FormatterLocator
	{
		private struct FormatterInfo
		{
			public Type FormatterType;

			public Type TargetType;

			public bool AskIfCanFormatTypes;

			public int Priority;
		}

		private struct FormatterLocatorInfo
		{
			public IFormatterLocator LocatorInstance;

			public int Priority;
		}

		private static readonly object LOCK;

		private static readonly Dictionary<Type, IFormatter> FormatterInstances;

		private static readonly DoubleLookupDictionary<Type, ISerializationPolicy, IFormatter> TypeFormatterMap;

		private static readonly List<FormatterLocatorInfo> FormatterLocators;

		private static readonly List<FormatterInfo> FormatterInfos;

		[Obsolete("Use the new IFormatterLocator interface instead, and register your custom locator with the RegisterFormatterLocator assembly attribute.", true)]
		public static event Func<Type, IFormatter> FormatterResolve
		{
			add
			{
				throw new NotSupportedException();
			}
			remove
			{
				throw new NotSupportedException();
			}
		}

		static FormatterLocator()
		{
			LOCK = new object();
			FormatterInstances = new Dictionary<Type, IFormatter>(FastTypeComparer.Instance);
			TypeFormatterMap = new DoubleLookupDictionary<Type, ISerializationPolicy, IFormatter>(FastTypeComparer.Instance, ReferenceEqualityComparer<ISerializationPolicy>.Default);
			FormatterLocators = new List<FormatterLocatorInfo>();
			FormatterInfos = new List<FormatterInfo>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				try
				{
					string name = assembly.GetName().Name;
					if (name.StartsWith("System.") || name.StartsWith("UnityEngine") || name.StartsWith("UnityEditor") || name == "mscorlib" || ((assembly.GetName().Name == "Sirenix.Serialization.AOTGenerated" || assembly.IsDefined(typeof(EmittedAssemblyAttribute), inherit: true)) && EmitUtilities.CanEmit))
					{
						continue;
					}
					object[] array = assembly.SafeGetCustomAttributes(typeof(RegisterFormatterAttribute), inherit: true);
					foreach (object obj in array)
					{
						RegisterFormatterAttribute registerFormatterAttribute = (RegisterFormatterAttribute)obj;
						if (registerFormatterAttribute.FormatterType.IsClass && !registerFormatterAttribute.FormatterType.IsAbstract && (object)registerFormatterAttribute.FormatterType.GetConstructor(Type.EmptyTypes) != null && registerFormatterAttribute.FormatterType.ImplementsOpenGenericInterface(typeof(IFormatter<>)))
						{
							FormatterInfos.Add(new FormatterInfo
							{
								FormatterType = registerFormatterAttribute.FormatterType,
								TargetType = registerFormatterAttribute.FormatterType.GetArgumentsOfInheritedOpenGenericInterface(typeof(IFormatter<>))[0],
								AskIfCanFormatTypes = typeof(IAskIfCanFormatTypes).IsAssignableFrom(registerFormatterAttribute.FormatterType),
								Priority = registerFormatterAttribute.Priority
							});
						}
					}
					object[] array2 = assembly.SafeGetCustomAttributes(typeof(RegisterFormatterLocatorAttribute), inherit: true);
					foreach (object obj2 in array2)
					{
						RegisterFormatterLocatorAttribute registerFormatterLocatorAttribute = (RegisterFormatterLocatorAttribute)obj2;
						if (registerFormatterLocatorAttribute.FormatterLocatorType.IsClass && !registerFormatterLocatorAttribute.FormatterLocatorType.IsAbstract && (object)registerFormatterLocatorAttribute.FormatterLocatorType.GetConstructor(Type.EmptyTypes) != null && typeof(IFormatterLocator).IsAssignableFrom(registerFormatterLocatorAttribute.FormatterLocatorType))
						{
							try
							{
								FormatterLocators.Add(new FormatterLocatorInfo
								{
									LocatorInstance = (IFormatterLocator)Activator.CreateInstance(registerFormatterLocatorAttribute.FormatterLocatorType),
									Priority = registerFormatterLocatorAttribute.Priority
								});
							}
							catch (Exception innerException)
							{
								Debug.LogException(new Exception("Exception was thrown while instantiating FormatterLocator of type " + registerFormatterLocatorAttribute.FormatterLocatorType.FullName + ".", innerException));
							}
						}
					}
				}
				catch (TypeLoadException)
				{
					if (assembly.GetName().Name == "OdinSerializer")
					{
						Debug.LogError("A TypeLoadException occurred when FormatterLocator tried to load types from assembly '" + assembly.FullName + "'. No serialization formatters in this assembly will be found. Serialization will be utterly broken.");
					}
				}
				catch (ReflectionTypeLoadException)
				{
					if (assembly.GetName().Name == "OdinSerializer")
					{
						Debug.LogError("A ReflectionTypeLoadException occurred when FormatterLocator tried to load types from assembly '" + assembly.FullName + "'. No serialization formatters in this assembly will be found. Serialization will be utterly broken.");
					}
				}
				catch (MissingMemberException)
				{
					if (assembly.GetName().Name == "OdinSerializer")
					{
						Debug.LogError("A ReflectionTypeLoadException occurred when FormatterLocator tried to load types from assembly '" + assembly.FullName + "'. No serialization formatters in this assembly will be found. Serialization will be utterly broken.");
					}
				}
			}
			FormatterInfos.Sort(delegate(FormatterInfo a, FormatterInfo b)
			{
				int num2 = -a.Priority.CompareTo(b.Priority);
				if (num2 == 0)
				{
					num2 = a.FormatterType.Name.CompareTo(b.FormatterType.Name);
				}
				return num2;
			});
			FormatterLocators.Sort(delegate(FormatterLocatorInfo a, FormatterLocatorInfo b)
			{
				int num = -a.Priority.CompareTo(b.Priority);
				if (num == 0)
				{
					num = a.LocatorInstance.GetType().Name.CompareTo(b.LocatorInstance.GetType().Name);
				}
				return num;
			});
		}

		public static IFormatter<T> GetFormatter<T>(ISerializationPolicy policy)
		{
			return (IFormatter<T>)GetFormatter(typeof(T), policy);
		}

		public static IFormatter GetFormatter(Type type, ISerializationPolicy policy)
		{
			if ((object)type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (policy == null)
			{
				policy = SerializationPolicies.Strict;
			}
			lock (LOCK)
			{
				if (!TypeFormatterMap.TryGetInnerValue(type, policy, out var value))
				{
					try
					{
						value = CreateFormatter(type, policy);
					}
					catch (TargetInvocationException ex)
					{
						if (!(ex.GetBaseException() is ExecutionEngineException))
						{
							throw ex;
						}
						LogAOTError(type, ex.GetBaseException() as ExecutionEngineException);
					}
					catch (TypeInitializationException ex2)
					{
						if (!(ex2.GetBaseException() is ExecutionEngineException))
						{
							throw ex2;
						}
						LogAOTError(type, ex2.GetBaseException() as ExecutionEngineException);
					}
					catch (ExecutionEngineException ex3)
					{
						LogAOTError(type, ex3);
					}
					TypeFormatterMap.AddInner(type, policy, value);
					return value;
				}
				return value;
			}
		}

		private static void LogAOTError(Type type, Exception ex)
		{
			string[] value = new List<string>(GetAllPossibleMissingAOTTypes(type)).ToArray();
			Debug.LogError("Creating a serialization formatter for the type '" + type.GetNiceFullName() + "' failed due to missing AOT support. \n\n Please use Odin's AOT generation feature to generate an AOT dll before building, and MAKE SURE that all of the following types were automatically added to the supported types list after a scan (if they were not, please REPORT AN ISSUE with the details of which exact types the scan is missing and ADD THEM MANUALLY): \n\n" + string.Join("\n", value) + "\n\nIF ALL THE TYPES ARE IN THE SUPPORT LIST AND YOU STILL GET THIS ERROR, PLEASE REPORT AN ISSUE.The exception contained the following message: \n" + ex.Message);
			throw new SerializationAbortException("AOT formatter support was missing for type '" + type.GetNiceFullName() + "'.", ex);
		}

		private static IEnumerable<string> GetAllPossibleMissingAOTTypes(Type type)
		{
			yield return type.GetNiceFullName() + " (name string: '" + TwoWaySerializationBinder.Default.BindToName(type) + "')";
			if (!type.IsGenericType)
			{
				yield break;
			}
			Type[] genericArguments = type.GetGenericArguments();
			foreach (Type arg in genericArguments)
			{
				yield return arg.GetNiceFullName() + " (name string: '" + TwoWaySerializationBinder.Default.BindToName(arg) + "')";
				if (!arg.IsGenericType)
				{
					continue;
				}
				foreach (string allPossibleMissingAOTType in GetAllPossibleMissingAOTTypes(arg))
				{
					yield return allPossibleMissingAOTType;
				}
			}
		}

		internal static List<IFormatter> GetAllCompatiblePredefinedFormatters(Type type, ISerializationPolicy policy)
		{
			if (FormatterUtilities.IsPrimitiveType(type))
			{
				throw new ArgumentException("Cannot create formatters for a primitive type like " + type.Name);
			}
			List<IFormatter> list = new List<IFormatter>();
			for (int i = 0; i < FormatterLocators.Count; i++)
			{
				try
				{
					if (FormatterLocators[i].LocatorInstance.TryGetFormatter(type, FormatterLocationStep.BeforeRegisteredFormatters, policy, out var formatter))
					{
						list.Add(formatter);
					}
				}
				catch (TargetInvocationException ex)
				{
					throw ex;
				}
				catch (TypeInitializationException ex2)
				{
					throw ex2;
				}
				catch (ExecutionEngineException ex3)
				{
					throw ex3;
				}
				catch (Exception innerException)
				{
					Debug.LogException(new Exception("Exception was thrown while calling FormatterLocator " + FormatterLocators[i].GetType().FullName + ".", innerException));
				}
			}
			for (int j = 0; j < FormatterInfos.Count; j++)
			{
				FormatterInfo formatterInfo = FormatterInfos[j];
				Type type2 = null;
				if ((object)type == formatterInfo.TargetType)
				{
					type2 = formatterInfo.FormatterType;
				}
				else if (formatterInfo.FormatterType.IsGenericType && formatterInfo.TargetType.IsGenericParameter)
				{
					if (formatterInfo.FormatterType.TryInferGenericParameters(out var inferredParams, type))
					{
						type2 = formatterInfo.FormatterType.GetGenericTypeDefinition().MakeGenericType(inferredParams);
					}
				}
				else if (type.IsGenericType && formatterInfo.FormatterType.IsGenericType && formatterInfo.TargetType.IsGenericType && (object)type.GetGenericTypeDefinition() == formatterInfo.TargetType.GetGenericTypeDefinition())
				{
					Type[] genericArguments = type.GetGenericArguments();
					if (formatterInfo.FormatterType.AreGenericConstraintsSatisfiedBy(genericArguments))
					{
						type2 = formatterInfo.FormatterType.GetGenericTypeDefinition().MakeGenericType(genericArguments);
					}
				}
				if ((object)type2 != null)
				{
					IFormatter formatterInstance = GetFormatterInstance(type2);
					if (formatterInstance != null && (!formatterInfo.AskIfCanFormatTypes || ((IAskIfCanFormatTypes)formatterInstance).CanFormatType(type)))
					{
						list.Add(formatterInstance);
					}
				}
			}
			for (int k = 0; k < FormatterLocators.Count; k++)
			{
				try
				{
					if (FormatterLocators[k].LocatorInstance.TryGetFormatter(type, FormatterLocationStep.AfterRegisteredFormatters, policy, out var formatter2))
					{
						list.Add(formatter2);
					}
				}
				catch (TargetInvocationException ex4)
				{
					throw ex4;
				}
				catch (TypeInitializationException ex5)
				{
					throw ex5;
				}
				catch (ExecutionEngineException ex6)
				{
					throw ex6;
				}
				catch (Exception innerException2)
				{
					Debug.LogException(new Exception("Exception was thrown while calling FormatterLocator " + FormatterLocators[k].GetType().FullName + ".", innerException2));
				}
			}
			list.Add((IFormatter)Activator.CreateInstance(typeof(ReflectionFormatter<>).MakeGenericType(type)));
			return list;
		}

		private static IFormatter CreateFormatter(Type type, ISerializationPolicy policy)
		{
			if (FormatterUtilities.IsPrimitiveType(type))
			{
				throw new ArgumentException("Cannot create formatters for a primitive type like " + type.Name);
			}
			for (int i = 0; i < FormatterLocators.Count; i++)
			{
				try
				{
					if (FormatterLocators[i].LocatorInstance.TryGetFormatter(type, FormatterLocationStep.BeforeRegisteredFormatters, policy, out var formatter))
					{
						return formatter;
					}
				}
				catch (TargetInvocationException ex)
				{
					throw ex;
				}
				catch (TypeInitializationException ex2)
				{
					throw ex2;
				}
				catch (ExecutionEngineException ex3)
				{
					throw ex3;
				}
				catch (Exception innerException)
				{
					Debug.LogException(new Exception("Exception was thrown while calling FormatterLocator " + FormatterLocators[i].GetType().FullName + ".", innerException));
				}
			}
			for (int j = 0; j < FormatterInfos.Count; j++)
			{
				FormatterInfo formatterInfo = FormatterInfos[j];
				Type type2 = null;
				if ((object)type == formatterInfo.TargetType)
				{
					type2 = formatterInfo.FormatterType;
				}
				else if (formatterInfo.FormatterType.IsGenericType && formatterInfo.TargetType.IsGenericParameter)
				{
					if (formatterInfo.FormatterType.TryInferGenericParameters(out var inferredParams, type))
					{
						type2 = formatterInfo.FormatterType.GetGenericTypeDefinition().MakeGenericType(inferredParams);
					}
				}
				else if (type.IsGenericType && formatterInfo.FormatterType.IsGenericType && formatterInfo.TargetType.IsGenericType && (object)type.GetGenericTypeDefinition() == formatterInfo.TargetType.GetGenericTypeDefinition())
				{
					Type[] genericArguments = type.GetGenericArguments();
					if (formatterInfo.FormatterType.AreGenericConstraintsSatisfiedBy(genericArguments))
					{
						type2 = formatterInfo.FormatterType.GetGenericTypeDefinition().MakeGenericType(genericArguments);
					}
				}
				if ((object)type2 != null)
				{
					IFormatter formatterInstance = GetFormatterInstance(type2);
					if (formatterInstance != null && (!formatterInfo.AskIfCanFormatTypes || ((IAskIfCanFormatTypes)formatterInstance).CanFormatType(type)))
					{
						return formatterInstance;
					}
				}
			}
			for (int k = 0; k < FormatterLocators.Count; k++)
			{
				try
				{
					if (FormatterLocators[k].LocatorInstance.TryGetFormatter(type, FormatterLocationStep.AfterRegisteredFormatters, policy, out var formatter2))
					{
						return formatter2;
					}
				}
				catch (TargetInvocationException ex4)
				{
					throw ex4;
				}
				catch (TypeInitializationException ex5)
				{
					throw ex5;
				}
				catch (ExecutionEngineException ex6)
				{
					throw ex6;
				}
				catch (Exception innerException2)
				{
					Debug.LogException(new Exception("Exception was thrown while calling FormatterLocator " + FormatterLocators[k].GetType().FullName + ".", innerException2));
				}
			}
			if (EmitUtilities.CanEmit)
			{
				IFormatter emittedFormatter = FormatterEmitter.GetEmittedFormatter(type, policy);
				if (emittedFormatter != null)
				{
					return emittedFormatter;
				}
			}
			if (EmitUtilities.CanEmit)
			{
				Debug.LogWarning("Fallback to reflection for type " + type.Name + " when emit is possible on this platform.");
			}
			return (IFormatter)Activator.CreateInstance(typeof(ReflectionFormatter<>).MakeGenericType(type));
		}

		private static IFormatter GetFormatterInstance(Type type)
		{
			if (!FormatterInstances.TryGetValue(type, out var value))
			{
				try
				{
					value = (IFormatter)Activator.CreateInstance(type);
					FormatterInstances.Add(type, value);
					return value;
				}
				catch (TargetInvocationException ex)
				{
					throw ex;
				}
				catch (TypeInitializationException ex2)
				{
					throw ex2;
				}
				catch (ExecutionEngineException ex3)
				{
					throw ex3;
				}
				catch (Exception innerException)
				{
					Debug.LogException(new Exception("Exception was thrown while instantiating formatter '" + type.GetNiceFullName() + "'.", innerException));
					return value;
				}
			}
			return value;
		}
	}
}
