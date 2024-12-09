using System;
using Spine;
using Spine.Unity;

public class SwitchRoundUI : UIView
{
	private SkeletonGraphic skeleton;

	private AnimationState state;

	[SpineAnimation("", "", true, false)]
	public string switchRoundAnimName_CN;

	[SpineAnimation("", "", true, false)]
	public string switchRoundAnimName_EN;

	[SpineEvent("", "", true, false, false)]
	public string buffActiveEventName;

	[SpineEvent("", "", true, false, false)]
	public string equipActiveEventName;

	private Action completeAction;

	public Action OnBuffActive;

	public Action OnEquipActive;

	private string targetSwitchRoundAnimName;

	public override string UIViewName => "SwitchRoundUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		EventManager.RegisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
	}

	private void OnBattleEnd(EventData data)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
		skeleton = base.transform.Find("Root/Anim").GetComponent<SkeletonGraphic>();
		skeleton.Initialize(overwrite: true);
		state = skeleton.AnimationState;
		state.Complete += AnimationEnd;
		state.Event += AnimEventHandle;
		targetSwitchRoundAnimName = ((SingletonDontDestroy<SettingManager>.Instance.Language == 0) ? switchRoundAnimName_CN : switchRoundAnimName_EN);
	}

	public void SwitchToPlayerRound(Action completeAction, Action onequipActiveAction, Action onbuffActiveAction)
	{
		OccupationData occupationData = DataManager.Instance.GetOccupationData(Singleton<GameManager>.Instance.Player.PlayerOccupation);
		this.completeAction = completeAction;
		skeleton.Skeleton.SetSkin(occupationData.PlayerRoundSwitchUISkinName);
		skeleton.Skeleton.SetSlotsToSetupPose();
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("玩家回合");
		OnBuffActive = onbuffActiveAction;
		OnEquipActive = onequipActiveAction;
		SwitchAnim();
	}

	public void SwitchToEnemyRound(Action completeAction, Action onequipActiveAction, Action onbuffActiveAction)
	{
		OccupationData occupationData = DataManager.Instance.GetOccupationData(Singleton<GameManager>.Instance.Player.PlayerOccupation);
		this.completeAction = completeAction;
		skeleton.Skeleton.SetSkin(occupationData.EnemyRoundSwitchUISkinName);
		skeleton.Skeleton.SetSlotsToSetupPose();
		OnBuffActive = onbuffActiveAction;
		OnEquipActive = onequipActiveAction;
		SwitchAnim();
	}

	private void SwitchAnim()
	{
		state.SetAnimation(0, targetSwitchRoundAnimName, loop: false);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("切换木板");
	}

	private void AnimationEnd(TrackEntry trackentry)
	{
		completeAction?.Invoke();
	}

	private void AnimEventHandle(TrackEntry trackEntry, Event e)
	{
		if (e.Data.Name == buffActiveEventName)
		{
			OnBuffActive?.Invoke();
		}
		else if (e.Data.Name == equipActiveEventName)
		{
			OnEquipActive?.Invoke();
		}
	}
}
