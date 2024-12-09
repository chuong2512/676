using System;
using System.Collections.Generic;
using Spine;
using Spine.Unity;

public class Monster50AnimCtrl : MonsterAnimCtrlBase
{
	private bool isPlayingP2;

	[SpineAnimation("", "", true, false)]
	public string leftHandInjuryAnimName;

	[SpineAnimation("", "", true, false)]
	public string rightHandInjuryAnimName;

	[SpineAnimation("", "", true, false)]
	public string headInjuryAnimName;

	[SpineAnimation("", "", true, false)]
	public string leftHandDeadAnimName;

	[SpineAnimation("", "", true, false)]
	public string rightHandDeadAnimName;

	[SpineAnimation("", "", true, false)]
	public string leftHandDeadIdleAnimName;

	[SpineAnimation("", "", true, false)]
	public string rightHandDeadIdleAnimName;

	[SpineAnimation("", "", true, false)]
	public string bothDeadIdleAnimName;

	[SpineAnimation("", "", true, false)]
	public string xuliAnimName;

	[SpineAnimation("", "", true, false)]
	public string leftHandGrowSpikeAnimName;

	[SpineAnimation("", "", true, false)]
	public string rightHandGrowSpikeAnimName;

	[SpineAnimation("", "", true, false)]
	public string p2AnimName;

	[SpineSkin("", "", true, false, false)]
	public string normalSkinName;

	[SpineSkin("", "", true, false, false)]
	public string angrySkinName;

	private Enemy_50 _enemy50;

	[SpineEvent("", "", true, false, false)]
	public string leftHandInjuryCompleteEvent;

	[SpineEvent("", "", true, false, false)]
	public string rightHandInjuryCompleteEvent;

	[SpineEvent("", "", true, false, false)]
	public string headInjuryCompleteEvent;

	[SpineEvent("", "", true, false, false)]
	public string leftHandDeadCompleteEvent;

	[SpineEvent("", "", true, false, false)]
	public string rightHandDeadCompleteEvent;

	[SpineEvent("", "", true, false, false)]
	public string completeEvent;

	[SpineEvent("", "", true, false, false)]
	public string p2CompleteEvent;

	[SpineEvent("", "", true, false, false)]
	public string track3ClearEvent;

	private Dictionary<string, Action> allEventHandler;

	private void OnDisable()
	{
		SingletonDontDestroy<AudioManager>.Instance.StopLoopSound("Monster/Monster_50_idle");
	}

	public override void Init()
	{
		base.Init();
		InitEvent();
		isPlayingP2 = false;
	}

	public void SetNormalSkin()
	{
		SkeletonAnimation.Skeleton.SetSkin(normalSkinName);
	}

	public void SetEnemyBase(Enemy_50 thisEnemy)
	{
		_enemy50 = thisEnemy;
	}

	private void ClearTrack(int trackIndex, float mixTime)
	{
		base.State.SetEmptyAnimation(trackIndex, mixTime);
	}

	public void GrowSpike_LeftHand()
	{
		base.State.SetAnimation(5, leftHandGrowSpikeAnimName, loop: false);
	}

	public void GrowSpike_RightHand()
	{
		base.State.SetAnimation(6, rightHandGrowSpikeAnimName, loop: false);
	}

	public void CancelSpike_LeftHand()
	{
		ClearTrack(5, 0.5f);
	}

	public void CancelSpike_RightHand()
	{
		ClearTrack(6, 0.5f);
	}

	public override void FlashWhite()
	{
		base.State.SetAnimation(3, headInjuryAnimName, loop: false);
	}

	public void FlashWhite_LeftHand()
	{
		base.State.SetAnimation(1, leftHandInjuryAnimName, loop: false);
	}

	public void FlashWhite_RightHand()
	{
		base.State.SetAnimation(2, rightHandInjuryAnimName, loop: false);
	}

	public void LeftHandDead()
	{
		base.State.SetAnimation(1, leftHandDeadAnimName, loop: false);
	}

	public void RightHandDead()
	{
		base.State.SetAnimation(2, rightHandDeadAnimName, loop: false);
	}

	private void OnInjuryComplete()
	{
		SetIdle();
	}

	private void SetIdle()
	{
		switch (_enemy50.CurrentState)
		{
		case 1:
		case 2:
		case 3:
			base.State.SetAnimation(0, xuliAnimName, loop: true);
			break;
		case 4:
			base.State.SetAnimation(0, idleAnimName, loop: true);
			break;
		}
	}

	private void AddP2()
	{
		if (!isPlayingP2)
		{
			isPlayingP2 = true;
			base.State.SetAnimation(0, p2AnimName, loop: false);
		}
	}

	private void InitEvent()
	{
		allEventHandler = new Dictionary<string, Action>
		{
			{ headInjuryCompleteEvent, HeadInjuryComplete_EventHandler },
			{ leftHandInjuryCompleteEvent, LeftHandInjuryComplete_EventHandler },
			{ rightHandInjuryCompleteEvent, RightHandInjuryComplete_EventHandler },
			{ completeEvent, Complete_EventHandler },
			{ leftHandDeadCompleteEvent, LeftHandDeadComplete_EventHandler },
			{ rightHandDeadCompleteEvent, RightHandDeadComplete_EventHandler },
			{ p2CompleteEvent, P2Complete_EventHandler },
			{ track3ClearEvent, Track3Clear_EventHandler }
		};
	}

	protected override void HandleEvent(TrackEntry trackEntry, Event e)
	{
		base.HandleEvent(trackEntry, e);
		allEventHandler[e.Data.name]();
	}

	private void Track3Clear_EventHandler()
	{
		ClearTrack(3, 0.2f);
	}

	private void P2Complete_EventHandler()
	{
		isPlayingP2 = false;
		base.State.SetAnimation(0, xuliAnimName, loop: true);
		SkeletonAnimation.Skeleton.SetSkin(angrySkinName);
		SkeletonAnimation.Skeleton.SetSlotsToSetupPose();
		base.State.Apply(SkeletonAnimation.Skeleton);
	}

	private void RightHandDeadComplete_EventHandler()
	{
		base.State.SetAnimation(2, rightHandDeadIdleAnimName, loop: true);
		if (!_enemy50.IsDead)
		{
			AddP2();
		}
	}

	private void LeftHandDeadComplete_EventHandler()
	{
		base.State.SetAnimation(1, leftHandDeadIdleAnimName, loop: true);
		if (!_enemy50.IsDead)
		{
			AddP2();
		}
	}

	private void LeftHandInjuryComplete_EventHandler()
	{
		ClearTrack(1, 0.5f);
		OnInjuryComplete();
	}

	private void RightHandInjuryComplete_EventHandler()
	{
		ClearTrack(2, 0.5f);
		OnInjuryComplete();
	}

	private void HeadInjuryComplete_EventHandler()
	{
		ClearTrack(3, 0.5f);
		OnInjuryComplete();
	}

	private void Complete_EventHandler()
	{
		SetIdle();
	}
}
