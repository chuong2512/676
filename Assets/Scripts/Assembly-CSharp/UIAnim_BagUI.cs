using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIAnim_BagUI : UIAnimBase
{
	[SerializeField]
	private List<Transform> buttonList;

	[SerializeField]
	private Text coinTxt;

	[SerializeField]
	private Image coinImg;

	[SerializeField]
	private Transform buttonListStartTrans;

	private UIAnim_Common equiAnim;

	private UIAnim_BagUI_Skill skillAnim;

	private UIAnim_BagUI_Card cardAnim;

	private float originalButtonListPosX;

	private List<Tween> tweenList = new List<Tween>();

	public void Init()
	{
		originalButtonListPosX = buttonList[0].localPosition.x;
		equiAnim = GetComponentInChildren<UIAnim_Common>(includeInactive: true);
		equiAnim.Init();
		cardAnim = GetComponentInChildren<UIAnim_BagUI_Card>(includeInactive: true);
		cardAnim.Init();
		skillAnim = GetComponentInChildren<UIAnim_BagUI_Skill>(includeInactive: true);
	}

	public override void StartAnim()
	{
		Reset();
		StopAllCoroutines();
		StartCoroutine(StartAnimCo());
	}

	private void Reset()
	{
		foreach (Transform button in buttonList)
		{
			Vector3 localPosition = button.localPosition;
			localPosition = (button.localPosition = localPosition.WithV3(buttonListStartTrans.localPosition.x));
		}
		foreach (Tween tween in tweenList)
		{
			tween.KillTween();
		}
		coinImg.WithCol(0f);
		coinTxt.WithCol(0f);
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private IEnumerator StartAnimCo()
	{
		WaitForSeconds waitfor100mili = new WaitForSeconds(0.1f);
		foreach (Transform button in buttonList)
		{
			tweenList.Add(button.DOLocalMoveX(originalButtonListPosX, 0.5f).SetEase(Ease.InQuint));
			yield return waitfor100mili;
		}
		tweenList.Add(coinImg.Fade(0f, 1f));
		tweenList.Add(coinTxt.Fade(0f, 1f));
	}

	public void ResetEquipSlotAnim(Dictionary<string, EquipSlotCtrl> allShowingEquipSlots)
	{
		List<CanvasGroup> list = new List<CanvasGroup>();
		foreach (EquipSlotCtrl value in allShowingEquipSlots.Values)
		{
			list.Add(value.CanvasGroup);
		}
		equiAnim.SetSlotsAnim(list);
	}

	public void StartEquipPanel(Dictionary<string, EquipSlotCtrl> allShowingEquipSlots)
	{
		equiAnim.StartAnim();
		ResetEquipSlotAnim(allShowingEquipSlots);
	}

	public void StartSkillPanel(Dictionary<string, SkillSlotCtrl> allShowingInventorySkillSlots)
	{
		skillAnim.StartAnim(allShowingInventorySkillSlots);
	}

	public void StartCardPanel(Dictionary<string, BagCardSlotCtrl> mainHandSlots, Dictionary<string, BagCardSlotCtrl> supHandCardSlots, Dictionary<string, BagCardCtrl> mainHandCards, Dictionary<string, BagCardCtrl> supHandCards)
	{
		cardAnim.StartAnim(mainHandSlots, supHandCardSlots, mainHandCards, supHandCards);
	}

	public void ResetCardSlotAnim(Dictionary<string, BagCardSlotCtrl> allShowingInventoryBagCardSlots, Dictionary<string, BagCardCtrl> allShowingInventoryBagCards)
	{
		cardAnim.SetSlotsAnim(allShowingInventoryBagCardSlots, allShowingInventoryBagCards);
	}
}
