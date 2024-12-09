using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseSkillUI : UIView
{
	private Transform skillRoot;

	private int maxChooseAmount;

	private bool isMustEqualLimit;

	private Action<List<string>> callback;

	private List<string> chooseList = new List<string>();

	private List<SkillChooseCtrl> chooseCtrlsList = new List<SkillChooseCtrl>();

	private Queue<SkillChooseCtrl> allSkillChooseCtrlPool = new Queue<SkillChooseCtrl>();

	private Dictionary<string, SkillChooseCtrl> allShowingSkillChooseCtrls = new Dictionary<string, SkillChooseCtrl>();

	private Text titleText;

	private string baseTitleStr;

	private CanvasGroup _canvasGroup;

	public override string UIViewName => "ChooseSkillUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		_canvasGroup.blocksRaycasts = true;
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		RecycleAllSkillChooseCtrl();
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destroy Choose Skill UI...");
	}

	public override void OnSpawnUI()
	{
		skillRoot = base.transform.Find("BgMask/Bg/Mask/Root");
		base.transform.Find("BgMask/ConfirmBtn").GetComponent<Button>().onClick.AddListener(OnClickComfirm);
		titleText = base.transform.Find("BgMask/Title").GetComponent<Text>();
		_canvasGroup = GetComponent<CanvasGroup>();
	}

	public void ShowSkillChoose(string title, List<string> allSkills, int chooseAmount, bool isMustEqualLimit, Action<List<string>> callback)
	{
		maxChooseAmount = chooseAmount;
		this.isMustEqualLimit = isMustEqualLimit;
		this.callback = callback;
		chooseList.Clear();
		chooseCtrlsList.Clear();
		baseTitleStr = title;
		UpdateTitle();
		for (int i = 0; i < allSkills.Count; i++)
		{
			SkillChooseCtrl skillCHooseCtrl = GetSkillCHooseCtrl();
			skillCHooseCtrl.LoadSkill(this, allSkills[i]);
			allShowingSkillChooseCtrls.Add(allSkills[i], skillCHooseCtrl);
		}
	}

	private void UpdateTitle()
	{
		titleText.text = $"{baseTitleStr}（{chooseList.Count}/{maxChooseAmount}）";
	}

	public void OnChooseSkill(SkillChooseCtrl ctrl)
	{
		if (!chooseList.Contains(ctrl.currentSkill))
		{
			if (maxChooseAmount == chooseList.Count)
			{
				allShowingSkillChooseCtrls[chooseList[0]].SetHighlightActive(isActive: false);
				chooseList.RemoveAt(0);
				chooseCtrlsList.RemoveAt(0);
			}
			chooseCtrlsList.Add(ctrl);
			chooseList.Add(ctrl.currentSkill);
			ctrl.SetHighlightActive(isActive: true);
			UpdateTitle();
		}
	}

	private void RecycleAllSkillChooseCtrl()
	{
		if (allShowingSkillChooseCtrls.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<string, SkillChooseCtrl> allShowingSkillChooseCtrl in allShowingSkillChooseCtrls)
		{
			allShowingSkillChooseCtrl.Value.gameObject.SetActive(value: false);
			allSkillChooseCtrlPool.Enqueue(allShowingSkillChooseCtrl.Value);
		}
		allShowingSkillChooseCtrls.Clear();
	}

	private SkillChooseCtrl GetSkillCHooseCtrl()
	{
		if (allSkillChooseCtrlPool.Count > 0)
		{
			SkillChooseCtrl skillChooseCtrl = allSkillChooseCtrlPool.Dequeue();
			skillChooseCtrl.gameObject.SetActive(value: true);
			return skillChooseCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("SkillChooseCtrl", "Prefabs", skillRoot).GetComponent<SkillChooseCtrl>();
	}

	private void OnClickComfirm()
	{
		if (!isMustEqualLimit || maxChooseAmount == chooseList.Count)
		{
			_canvasGroup.blocksRaycasts = false;
			StartCoroutine(Confirm_IE());
		}
	}

	private IEnumerator Confirm_IE()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("事件_献祭技能装备");
		for (int i = 0; i < chooseCtrlsList.Count; i++)
		{
			chooseCtrlsList[i].BurnEquip();
		}
		yield return new WaitForSeconds(2f);
		callback?.Invoke(chooseList);
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}
}
