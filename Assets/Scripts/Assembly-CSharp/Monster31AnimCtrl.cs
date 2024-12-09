using System;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class Monster31AnimCtrl : MonsterAnimCtrlBase
{
	public enum SkinType
	{
		Fire,
		Earth,
		Water
	}

	private const string SlotSpriteNameFormat = "Slot{0}_{1}";

	private const string SlotSpritePath = "Sprites/SlotsElements";

	public SpriteRenderer[] slotRenderers;

	public SpriteRenderer[] slotShowUpRenderers;

	[SpineAnimation("", "", true, false)]
	public string changeSkinAnimName;

	[SpineEvent("", "", true, false, false)]
	public string onChangeSkinEventName;

	[SpineSkin("", "", true, false, false)]
	public string fireSkin;

	[SpineSkin("", "", true, false, false)]
	public string earthSkin;

	[SpineSkin("", "", true, false, false)]
	public string oceanSkin;

	private Action onSkinChangeAction;

	private Action onSkinChangeCompleteAction;

	public void ChangeSkinAnim(Action onSkinChangeAction, Action onCompleteAction)
	{
		base.State.SetAnimation(0, changeSkinAnimName, loop: false);
		this.onSkinChangeAction = onSkinChangeAction;
		onSkinChangeCompleteAction = onCompleteAction;
	}

	public void OnSkinChangeAction()
	{
		onSkinChangeAction?.Invoke();
		onSkinChangeAction = null;
	}

	public void OnSkinChangeCompleteAction()
	{
		onSkinChangeCompleteAction?.Invoke();
		onSkinChangeCompleteAction = null;
	}

	public void SetSkin(SkinType skinType)
	{
		switch (skinType)
		{
		case SkinType.Fire:
			SkeletonAnimation.Skeleton.SetSkin(fireSkin);
			break;
		case SkinType.Earth:
			SkeletonAnimation.Skeleton.SetSkin(earthSkin);
			break;
		case SkinType.Water:
			SkeletonAnimation.Skeleton.SetSkin(oceanSkin);
			break;
		}
		SkeletonAnimation.Skeleton.SetSlotsToSetupPose();
	}

	public void SetSlot(int index, SkinType slotType)
	{
		string spriteName = $"Slot{index}_{slotType.ToString()}";
		slotRenderers[index].sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(spriteName, "Sprites/SlotsElements");
		slotShowUpRenderers[index].color = Color.white;
		slotShowUpRenderers[index].DOColor(Color.clear, 1f);
	}

	public void ClearSlot(int index)
	{
		slotRenderers[index].sprite = null;
	}

	public void ClearAllSlots()
	{
		for (int i = 0; i < slotRenderers.Length; i++)
		{
			slotRenderers[i].sprite = null;
		}
	}
}
