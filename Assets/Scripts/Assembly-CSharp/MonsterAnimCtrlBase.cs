using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;

public class MonsterAnimCtrlBase : MonoBehaviour
{
	[Serializable]
	public class SpineSkeletonEventStruct
	{
		[SpineAnimation("", "", true, false)]
		public string theAnim;

		[SpineEvent("", "", true, false, false)]
		public string theEvent;

		public UnityEvent OnEventCallBack;

		public Transform followedBone;

		public List<GameObject> vfxPrefab;

		public List<string> vfxNames;

		public List<BaseEffectConfig> vfxConfig;

		public bool isCreating = true;

		public void RunEvent(Transform trans)
		{
			if (isCreating)
			{
				foreach (GameObject item in vfxPrefab)
				{
					UnityEngine.Object.Instantiate(item, (followedBone == null) ? trans : followedBone);
					item.transform.localPosition = Vector2.zero;
				}
				return;
			}
			foreach (string vfxName in vfxNames)
			{
				VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx(vfxName);
				vfxBase.transform.SetParent((followedBone == null) ? trans : followedBone);
				vfxBase.transform.localPosition = Vector2.zero;
				vfxBase.Play();
			}
			foreach (BaseEffectConfig item2 in vfxConfig)
			{
				Singleton<BattleEffectManager>.Instance.HandleEffectConfig(item2, followedBone, null, delegate
				{
					OnEventCallBack?.Invoke();
				});
			}
		}
	}

	[HideInInspector]
	public bool isInitOver;

	[HideInInspector]
	public SkeletonAnimation SkeletonAnimation;

	private Spine.AnimationState state;

	[HideInInspector]
	public MeshRenderer meshRenderer;

	[SpineAnimation("", "", true, false)]
	public string idleAnimName;

	protected MaterialPropertyBlock _propBlock;

	private static readonly int BlackProperty = Shader.PropertyToID("_Black");

	private static readonly int TintColor = Shader.PropertyToID("_Color");

	public List<SpineSkeletonEventStruct> allEventStruct;

	public Transform centerTrans;

	[SpineAnimation("", "", true, false)]
	public string animationReadyToPlay;

	public bool isLoop;

	protected Spine.AnimationState State
	{
		get
		{
			if (state == null)
			{
				SkeletonAnimation.Initialize(overwrite: false);
				state = SkeletonAnimation.AnimationState;
			}
			return state;
		}
	}

	public MeshRenderer MeshRenderer
	{
		get
		{
			if (meshRenderer == null)
			{
				meshRenderer = GetComponent<MeshRenderer>();
			}
			return meshRenderer;
		}
	}

	public virtual void Init()
	{
		isInitOver = false;
		SkeletonAnimation = GetComponent<SkeletonAnimation>();
		if (SkeletonAnimation.AnimationState == null)
		{
			SkeletonAnimation.Initialize(overwrite: false);
		}
		state = SkeletonAnimation.AnimationState;
		state.Event += HandleEvent;
		meshRenderer = GetComponent<MeshRenderer>();
		isInitOver = true;
		_propBlock = new MaterialPropertyBlock();
	}

	public virtual void PlaySkeletonAnim(List<EnemySkeletonAnimEffectStep.EnemySkeletonEffect> animList)
	{
		if (animList != null && animList.Count > 0)
		{
			State.SetAnimation(animList[0].skeletonAnimTrack, animList[0].skeletonAnimName, animList[0].isLoop);
			for (int i = 1; i < animList.Count; i++)
			{
				State.AddAnimation(animList[i].skeletonAnimTrack, animList[i].skeletonAnimName, animList[i].isLoop, 0f);
			}
		}
	}

	public virtual void PlaySkeletonAnim(EnemySkeletonAnimEffectStep.EnemySkeletonEffect effect)
	{
		State.SetAnimation(effect.skeletonAnimTrack, effect.skeletonAnimName, effect.isLoop);
	}

	public virtual void ChangeSkin(string skinName)
	{
		SkeletonAnimation.Skeleton.SetSkin(skinName);
	}

	public void IdleAnim()
	{
		State.SetAnimation(0, idleAnimName, loop: true);
	}

	public virtual void FlashWhite()
	{
		StartCoroutine(FlashWhiteCo());
	}

	private IEnumerator FlashWhiteCo()
	{
		ChangeBlackColor(Color.white);
		yield return new WaitForSeconds(0.1f);
		ChangeBlackColor(Color.black);
	}

	public void FadeToTransparent(float duration)
	{
		Color tmpColor = Color.white;
		DOTween.To(() => tmpColor, delegate(Color x)
		{
			tmpColor = x;
		}, Color.clear, duration).OnUpdate(delegate
		{
			ChangeTintColor(tmpColor);
		});
	}

	public void ChangeTintColor(Color tintColor)
	{
		MeshRenderer.GetPropertyBlock(_propBlock);
		_propBlock.SetColor(TintColor, tintColor);
		MeshRenderer.SetPropertyBlock(_propBlock);
	}

	public void ChangeBlackColor(Color tintColor)
	{
		MeshRenderer.GetPropertyBlock(_propBlock);
		_propBlock.SetColor(BlackProperty, tintColor);
		MeshRenderer.SetPropertyBlock(_propBlock);
	}

	protected virtual void HandleEvent(TrackEntry trackEntry, Spine.Event e)
	{
		foreach (SpineSkeletonEventStruct item in allEventStruct)
		{
			if (trackEntry.Animation.name == item.theAnim && e.Data.Name == item.theEvent)
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
