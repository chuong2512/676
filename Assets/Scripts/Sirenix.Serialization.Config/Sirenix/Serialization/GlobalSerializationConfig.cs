using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Sirenix.Serialization
{
	[HideMonoScript]
	[SirenixGlobalConfig]
	public class GlobalSerializationConfig : GlobalConfig<GlobalSerializationConfig>
	{
		public const string ODIN_SERIALIZATION_CAUTIONARY_WARNING_TEXT = "Odin's custom serialization protocol is stable and fast, and doesn't use any reflection on standalone platforms in order to serialize and deserialize your data. It is built to be fast, reliable and resilient above all.\n\n*Words of caution* \nHowever, caveats apply - there is a reason Unity chose such a drastically limited serialization protocol. It keeps things simple and manageable, and limits how much complexity you can introduce into your data structures. It can be very easy to get carried away and shoot yourself in the foot when all limitations suddenly disappear, and hence we have included this cautionary warning.\n\nThere can of course be valid reasons to use a more powerful serialization protocol such as Odin's. However, please read the 'Words of caution' section under 'Serialize Anything' in the Manual, so you know what you're getting into.\n\n*For those on AOT platforms* \nAdditionally, if you are building to AOT platforms, you should be aware that Odin's serialization currently makes use of reflection behind the scenes on all AOT platforms, which may result in lag spikes in your game if you make heavy use of it.";

		public const string ODIN_PREFAB_CAUTIONARY_WARNING_TEXT = "In 2018.3, Unity introduced a new prefab workflow, and in so doing, changed how all prefabs fundamentally work. Despite our best efforts, we have so far been unable to achieve a stable implementation of Odin-serialized prefab modifications on prefab instances and variants in the new prefab workflow.This has nothing to do with Odin serializer itself, which remains rock solid. Odin-serialized ScriptableObjects and non-prefab Components/Behaviours are still perfectly stable - you are only seeing this message because this is an Odin-serialized prefab asset or instance.\n\nWe are currently in the process of doing an in-depth review of possible ways of implementing Odin-serialized modifications properly with the new prefab workflow. Meanwhile, using prefabs with Odin serialization in 2018.3 and above will be considered a *deprecated feature*.\n\nWe would like to emphasize that *our efforts may fail*. There is a real chance that we will conclude that we simply cannot support this feature any more in the new prefab workflow, in which case it will be deprecated *permanently*. We hope it won't come to that, but we sadly cannot promise that it won't happen. If we do deprecate the feature, we will not remove it, but we *will* stop supporting it. You may continue using it, but only at your own risk.\n\nPlease keep all this in mind, if you wish to continue using Odin-serialized prefabs.";

		public const string ODIN_SERIALIZATION_CAUTIONARY_WARNING_BUTTON_TEXT = "I know what I'm about, son. Hide message forever.";

		public const string ODIN_PREFAB_CAUTIONARY_WARNING_BUTTON_TEXT = "I understand that prefabs might break. Hide message forever.";

		private static readonly DataFormat[] BuildFormats = new DataFormat[2]
		{
			DataFormat.Binary,
			DataFormat.JSON
		};

		[Title("Warning messages", null, TitleAlignments.Left, true, true)]
		[ToggleLeft]
		[DetailedInfoBox("Click to show warning message.", "Odin's custom serialization protocol is stable and fast, and doesn't use any reflection on standalone platforms in order to serialize and deserialize your data. It is built to be fast, reliable and resilient above all.\n\n*Words of caution* \nHowever, caveats apply - there is a reason Unity chose such a drastically limited serialization protocol. It keeps things simple and manageable, and limits how much complexity you can introduce into your data structures. It can be very easy to get carried away and shoot yourself in the foot when all limitations suddenly disappear, and hence we have included this cautionary warning.\n\nThere can of course be valid reasons to use a more powerful serialization protocol such as Odin's. However, please read the 'Words of caution' section under 'Serialize Anything' in the Manual, so you know what you're getting into.\n\n*For those on AOT platforms* \nAdditionally, if you are building to AOT platforms, you should be aware that Odin's serialization currently makes use of reflection behind the scenes on all AOT platforms, which may result in lag spikes in your game if you make heavy use of it.", InfoMessageType.Info, null)]
		public bool HideSerializationCautionaryMessage;

		[ToggleLeft]
		[DetailedInfoBox("Click to show warning message.", "In 2018.3, Unity introduced a new prefab workflow, and in so doing, changed how all prefabs fundamentally work. Despite our best efforts, we have so far been unable to achieve a stable implementation of Odin-serialized prefab modifications on prefab instances and variants in the new prefab workflow.This has nothing to do with Odin serializer itself, which remains rock solid. Odin-serialized ScriptableObjects and non-prefab Components/Behaviours are still perfectly stable - you are only seeing this message because this is an Odin-serialized prefab asset or instance.\n\nWe are currently in the process of doing an in-depth review of possible ways of implementing Odin-serialized modifications properly with the new prefab workflow. Meanwhile, using prefabs with Odin serialization in 2018.3 and above will be considered a *deprecated feature*.\n\nWe would like to emphasize that *our efforts may fail*. There is a real chance that we will conclude that we simply cannot support this feature any more in the new prefab workflow, in which case it will be deprecated *permanently*. We hope it won't come to that, but we sadly cannot promise that it won't happen. If we do deprecate the feature, we will not remove it, but we *will* stop supporting it. You may continue using it, but only at your own risk.\n\nPlease keep all this in mind, if you wish to continue using Odin-serialized prefabs.", InfoMessageType.Info, null)]
		public bool HidePrefabCautionaryMessage;

		[ToggleLeft]
		[SerializeField]
		[InfoBox("Enabling this will hide all warning messages that will show up in the inspector when the OdinSerialize attribute potentially does not achieve the desired effect.", InfoMessageType.Info, null)]
		public bool HideOdinSerializeAttributeWarningMessages;

		[SerializeField]
		[ToggleLeft]
		[LabelText("Hide Non-Serialized SerializeField/ShowInInspector Warning Messages")]
		[InfoBox("Enabling this will hide all warning messages that show up when the SerializeField and the ShowInInspector attributes are used together on non-serialized fields or properties.", InfoMessageType.Info, null)]
		public bool HideNonSerializedShowInInspectorWarningMessages;

		[SerializeField]
		[Title("Data formatting options", null, TitleAlignments.Left, true, true)]
		[ValueDropdown("BuildFormats")]
		private DataFormat buildSerializationFormat;

		[SerializeField]
		private DataFormat editorSerializationFormat = DataFormat.Nodes;

		[SerializeField]
		[Title("Logging and error handling", null, TitleAlignments.Left, true, true)]
		private LoggingPolicy loggingPolicy;

		[SerializeField]
		private ErrorHandlingPolicy errorHandlingPolicy;

		public ILogger Logger => DefaultLoggers.UnityLogger;

		public DataFormat EditorSerializationFormat
		{
			get
			{
				return editorSerializationFormat;
			}
			set
			{
				editorSerializationFormat = value;
			}
		}

		public DataFormat BuildSerializationFormat
		{
			get
			{
				return buildSerializationFormat;
			}
			set
			{
				buildSerializationFormat = value;
			}
		}

		public LoggingPolicy LoggingPolicy
		{
			get
			{
				return loggingPolicy;
			}
			set
			{
				loggingPolicy = value;
			}
		}

		public ErrorHandlingPolicy ErrorHandlingPolicy
		{
			get
			{
				return errorHandlingPolicy;
			}
			set
			{
				errorHandlingPolicy = value;
			}
		}

		[OnInspectorGUI]
		private void OnInspectorGUI()
		{
			GUIStyle style = new GUIStyle(GUI.skin.label)
			{
				richText = true
			};
			GUIStyle gUIStyle = new GUIStyle(GUI.skin.box);
			gUIStyle.padding = new RectOffset(7, 7, 7, 7);
			GUIStyle gUIStyle2 = new GUIStyle(GUI.skin.label);
			gUIStyle2.clipping = TextClipping.Overflow;
			gUIStyle2.wordWrap = true;
			GUILayout.Space(20f);
			GUILayout.BeginVertical(gUIStyle);
			GUILayout.Label("<b>Serialization Formats</b>", style);
			GUILayout.Label("The serialization format of the data in specially serialized Unity objects. Binary is recommended for builds; JSON has the benefit of being human-readable but has significantly worse performance.\n\nWith the special editor-only node format, the serialized data will be formatted in such a way that, if the asset is saved with Unity's text format (Edit -> Project Settings -> Editor -> Asset Serialization -> Mode), the data will be mergeable when using version control systems. This makes the custom serialized data a lot less fragile, but comes at a performance cost during serialization and deserialization. The node format is recommended in the editor.\n\nThis setting can be overridden on a per-instance basis.\n", gUIStyle2);
			GUILayout.Label("<b>Error Handling Policy</b>", style);
			GUILayout.Label("The policy for handling any errors and irregularities that crop up during deserialization. Resilient is the recommended option, as it will always try to recover as much data as possible from a corrupt serialization stream.\n", gUIStyle2);
			GUILayout.Label("<b>Logging Policy</b>", style);
			GUILayout.Label("Use this to determine the criticality of the events that are logged by the serialization system. Recommended value is to log only errors, and to log warnings and errors when you suspect issues in the system.", gUIStyle2);
			GUILayout.EndVertical();
		}
	}
}
