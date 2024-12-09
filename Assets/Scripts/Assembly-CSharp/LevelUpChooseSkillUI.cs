using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpChooseSkillUI : UIView
{
	private const int MaxSkillChooseAmount = 3;

	private LevelUpSkillSlotCtrl[] allSkillSlots;

	private int chooseTime;

	private LevelUpSkillSlotCtrl currentChooseLevelUpSkillSlot;

	private Transform startPoint;

	private Transform endPoint;

	private Transform titleTrans;

	private Text hintText;

	private CanvasGroup skillRootCg;

	public override string UIViewName => "LevelUpChooseSkillUI";

	public override string UILayerName => "NormalLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destory Level Up Choose Skill UI ...");
	}

	public override void OnSpawnUI()
	{
		startPoint = base.transform.Find("Bg/StartPoint");
		endPoint = base.transform.Find("Bg/EndPoint");
		hintText = base.transform.Find("Bg/Hint").GetComponent<Text>();
		allSkillSlots = new LevelUpSkillSlotCtrl[4];
		for (int i = 0; i < allSkillSlots.Length; i++)
		{
			allSkillSlots[i] = base.transform.Find("Bg/SkillRoot").GetChild(i).GetComponent<LevelUpSkillSlotCtrl>();
		}
		titleTrans = base.transform.Find("Bg/Title");
		skillRootCg = base.transform.Find("Bg/SkillRoot").GetComponent<CanvasGroup>();
	}

	public void ShowPlayerLevelUpSkillChoose(int chooseTime)
	{
		this.chooseTime = chooseTime;
		FreshSkill();
		StartCoroutine(ShowAnim_IE());
	}

	private IEnumerator ShowAnim_IE()
	{
		titleTrans.position = new Vector3(titleTrans.position.x, startPoint.position.y, titleTrans.transform.position.z);
		hintText.color = new Color(hintText.color.r, hintText.color.g, hintText.color.b, 0f);
		skillRootCg.alpha = 0f;
		titleTrans.DOMoveY(endPoint.position.y, 0.6f).SetEase(Ease.OutBack);
		yield return new WaitForSeconds(0.2f);
		hintText.DOFade(1f, 0.4f);
		yield return new WaitForSeconds(0.2f);
		skillRootCg.DOFade(1f, 0.4f);
	}

	private void FreshSkill()
	{
		if (chooseTime > 0)
		{
			chooseTime--;
			currentChooseLevelUpSkillSlot = null;
			LoadMapLevelSkill(3, Singleton<GameManager>.Instance.CurrentMapLevel);
			allSkillSlots[3].LoadEffect(this);
		}
		else
		{
			GameSave.SaveGame();
			SingletonDontDestroy<UIManager>.Instance.HideView(this);
		}
	}

	private void LoadMapLevelSkill(int loadAmount, int levelLimit)
	{
		List<string> list = AllRandomInventory.Instance.AllStatisfiedConditionSkills(levelLimit, Singleton<GameManager>.Instance.Player.PlayerOccupation);
		int i;
		for (i = 3 - loadAmount; i < 3; i++)
		{
			if (list.Count <= 0)
			{
				break;
			}
			int index = Random.Range(0, list.Count);
			allSkillSlots[i].LoadSkill(list[index], this);
			list.RemoveAt(index);
		}
		if (i < 3)
		{
			LoadMapLevelSkill(3 - i, ++levelLimit);
		}
	}

	public void OnChooseSkill(LevelUpSkillSlotCtrl ctrl)
	{
		currentChooseLevelUpSkillSlot = ctrl;
	}

	public void OnClickConfirmBtn()
	{
		if (!(currentChooseLevelUpSkillSlot == null))
		{
			if (currentChooseLevelUpSkillSlot.CurrentSkillCode.IsNullOrEmpty())
			{
				Singleton<GameManager>.Instance.Player.PlayerLevelUpEffect.Effect();
			}
			else
			{
				Singleton<GameManager>.Instance.Player.PlayerInventory.AddSkill(currentChooseLevelUpSkillSlot.CurrentSkillCode, isNew: true);
			}
			FreshSkill();
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
		}
	}
}
