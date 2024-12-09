using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;

public class SpineSkeletonVFXCtrl : SerializedMonoBehaviour
{
	[Serializable]
	public class SpineSkeletonEventStruct
	{
		[SpineEvent("", "", true, false, false)]
		public string theEvent;

		public SpineAnimAction OnEventCallBack;

		public Transform followedBone;

		public List<GameObject> vfxPrefab;

		public List<BaseEffectConfig> vfxConfig;

		public bool isCreating = true;

		public void RunEvent(Transform trans)
		{
			if (!isCreating)
			{
				return;
			}
			foreach (GameObject item in vfxPrefab)
			{
				UnityEngine.Object.Instantiate(item, (followedBone == null) ? trans : followedBone);
				item.transform.localPosition = Vector2.zero;
			}
			foreach (BaseEffectConfig item2 in vfxConfig)
			{
				Singleton<TempEffectManager>.Instance.HandleEffectConfing(item2, (followedBone == null) ? trans : followedBone, delegate
				{
					OnEventCallBack?.Invoke();
				});
			}
		}
	}

	[Serializable]
	public class SpineAnimAction : UnityEvent
	{
	}

	private SkeletonAnimation skeletonAnimation;

	private Spine.AnimationState state;

	public List<SpineSkeletonEventStruct> allEventStruct;

	private Dictionary<string, SpineSkeletonEventStruct> allEventDic = new Dictionary<string, SpineSkeletonEventStruct>();

	public Transform centerTrans;

	[SpineAnimation("", "", true, false)]
	public string animationReadyToPlay;

	public bool isLoop;

	private void Awake()
	{
		Init();
	}

	private void Init()
	{
		skeletonAnimation = GetComponent<SkeletonAnimation>();
		if (skeletonAnimation.AnimationState == null)
		{
			skeletonAnimation.Initialize(overwrite: false);
		}
		skeletonAnimation.Initialize(overwrite: false);
		state = skeletonAnimation.AnimationState;
		state.Event += HandleEvent;
		if (allEventStruct == null || allEventStruct.Count == 0)
		{
			return;
		}
		foreach (SpineSkeletonEventStruct item in allEventStruct)
		{
			allEventDic.Add(item.theEvent, item);
		}
	}

	protected virtual void HandleEvent(TrackEntry trackEntry, Spine.Event e)
	{
		foreach (SpineSkeletonEventStruct item in allEventStruct)
		{
			if (e.Data.Name == item.theEvent)
			{
				item.OnEventCallBack?.Invoke();
				item.RunEvent((centerTrans == null) ? base.transform : centerTrans);
			}
		}
	}

	private void PlayAnim()
	{
		if (animationReadyToPlay.IsNullOrEmpty())
		{
			Debug.LogWarning("必须选择 播放动画");
		}
		state.SetAnimation(0, animationReadyToPlay, isLoop);
	}

	private void StopAnim()
	{
		state.SetEmptyAnimation(0, 0.2f);
	}
}
