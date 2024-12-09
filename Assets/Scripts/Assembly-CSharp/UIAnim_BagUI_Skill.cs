using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIAnim_BagUI_Skill : MonoBehaviour
{
	private List<Tween> tweenList = new List<Tween>();

	[SerializeField]
	private List<CanvasGroup> cgs = new List<CanvasGroup>();

	[SerializeField]
	private Image scrollBG;

	[SerializeField]
	private Text equipedSkillTitleTxt;

	[SerializeField]
	private Text inventorySkillTitleTxt;

	[SerializeField]
	private Text hintTxt;

	public void StartAnim(Dictionary<string, SkillSlotCtrl> allShowingInventorySkillSlots)
	{
		Reset(allShowingInventorySkillSlots);
		StopAllCoroutines();
		StartCoroutine(StartAnimCo(allShowingInventorySkillSlots));
	}

	private void Reset(Dictionary<string, SkillSlotCtrl> allShowingInventorySkillSlots)
	{
		foreach (Tween tween in tweenList)
		{
			tween.KillTween();
		}
		foreach (SkillSlotCtrl value in allShowingInventorySkillSlots.Values)
		{
			value.CanvasGroup.alpha = 0f;
		}
		hintTxt.WithCol(0f);
		equipedSkillTitleTxt.WithCol(0f);
		inventorySkillTitleTxt.WithCol(0f);
		scrollBG.fillAmount = 0.05f;
		foreach (CanvasGroup cg in cgs)
		{
			cg.alpha = 0f;
		}
	}

	private IEnumerator StartAnimCo(Dictionary<string, SkillSlotCtrl> allShowingInventorySkillSlots)
	{
		tweenList.Add(scrollBG.DOFillAmount(1f, 0.3f));
		yield return new WaitForSeconds(0.2f);
		tweenList.Add(hintTxt.DOFade(1f, 1f));
		tweenList.Add(equipedSkillTitleTxt.DOFade(1f, 1f));
		tweenList.Add(inventorySkillTitleTxt.DOFade(1f, 1f));
		yield return new WaitForSeconds(0.2f);
		foreach (SkillSlotCtrl value in allShowingInventorySkillSlots.Values)
		{
			tweenList.Add(value.CanvasGroup.DOFade(1f, 0.5f));
		}
		foreach (CanvasGroup cg in cgs)
		{
			tweenList.Add(cg.DOFade(1f, 0.5f));
			yield return new WaitForSeconds(0.03f);
		}
	}
}
