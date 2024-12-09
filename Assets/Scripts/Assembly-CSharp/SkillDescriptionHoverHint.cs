using UnityEngine;
using UnityEngine.UI;

public class SkillDescriptionHoverHint : HoverWithKeysHint
{
	private Text nameText;

	private Text descriptionText;

	private UsualNoDesCardInfo[] allComsumeCardInfos;

	private Transform[] allCardStatTrans;

	private Transform comsumePanel;

	private Transform cardComsumePanel;

	private Transform comsumeItem0Bottom;

	private Transform comsumeItem1Bottom;

	private Text comsumeItem0Text;

	private Text comsumeItem1Text;

	private Text tmpVisualTxt;

	protected override void OnAwake()
	{
		base.OnAwake();
		nameText = base.transform.Find("NameBottom/TmpTxtBg/TitleBg/TitleTxt").GetComponent<Text>();
		tmpVisualTxt = base.transform.Find("NameBottom/TmpTxtBg").GetComponent<Text>();
		descriptionText = base.transform.Find("Description").GetComponent<Text>();
		comsumePanel = base.transform.Find("Comsume");
		cardComsumePanel = base.transform.Find("CardComsume");
		allComsumeCardInfos = new UsualNoDesCardInfo[3];
		allCardStatTrans = new Transform[3];
		for (int i = 0; i < allComsumeCardInfos.Length; i++)
		{
			allComsumeCardInfos[i] = base.transform.Find("CardComsume/CardRoot").GetChild(i).GetComponent<UsualNoDesCardInfo>();
			allCardStatTrans[i] = allComsumeCardInfos[i].transform.Find("StatBottom");
		}
		comsumeItem0Bottom = base.transform.Find("Comsume/ComsumeItem_0");
		comsumeItem0Text = comsumeItem0Bottom.Find("ComsumeContent").GetComponent<Text>();
		comsumeItem1Bottom = base.transform.Find("Comsume/ComsumeItem_1");
		comsumeItem1Text = comsumeItem1Bottom.Find("ComsumeContent").GetComponent<Text>();
	}

	protected override void InitKey()
	{
		keyRoot = base.transform.Find("NameBottom/KeyRoot");
	}

	public void LoadSkillBaseInfo(string nameStr, string contentStr)
	{
		string text3 = (tmpVisualTxt.text = (nameText.text = nameStr));
		descriptionText.text = contentStr;
	}

	public void LoadSkillComsumeInfo(PlayerOccupation playerOccupation, string skillCode, bool isCheckCardStat)
	{
		comsumePanel.gameObject.SetActive(value: true);
		cardComsumePanel.gameObject.SetActive(value: true);
		SkillCardAttr skillCardAttr = DataManager.Instance.GetSkillCardAttr(playerOccupation, skillCode);
		OccupationData occupationData = DataManager.Instance.GetOccupationData(playerOccupation);
		AddKeys(skillCardAttr.AllKeys);
		comsumeItem0Text.text = string.Format("{0} ×{1}", "ActionPoint".LocalizeText(), skillCardAttr.ApCost);
		if (skillCardAttr.SpecialAttrCost < 0)
		{
			comsumeItem1Text.text = string.Format("COMSUMEALLFAITH".LocalizeText(), occupationData.OccupationSpecialAttrDes.key.LocalizeText());
		}
		else
		{
			comsumeItem1Text.text = $"{occupationData.OccupationSpecialAttrDes.key.LocalizeText()} ×{skillCardAttr.SpecialAttrCost}";
		}
		SetCardStat(skillCardAttr, isCheckCardStat);
	}

	private void SetCardStat(SkillCardAttr data, bool isCheckCardStat)
	{
		int num = 0;
		if (!string.IsNullOrEmpty(data.MainHandCardCode))
		{
			int mainHandCardConsumeAmount = data.MainHandCardConsumeAmount;
			for (int i = 0; i < mainHandCardConsumeAmount; i++)
			{
				allComsumeCardInfos[num].gameObject.SetActive(value: true);
				allComsumeCardInfos[num].LoadCard(data.MainHandCardCode);
				if (isCheckCardStat)
				{
					if (Singleton<GameManager>.Instance.Player.PlayerBattleInfo.AllEquipedMainHandCards.TryGetValue(data.MainHandCardCode, out var value) && value > i)
					{
						allComsumeCardInfos[num].SetCardUsable();
						allCardStatTrans[num].gameObject.SetActive(value: false);
					}
					else
					{
						allComsumeCardInfos[num].SetCardUnusable();
						allCardStatTrans[num].gameObject.SetActive(value: true);
					}
				}
				else
				{
					allComsumeCardInfos[num].SetCardUsable();
					allCardStatTrans[num].gameObject.SetActive(value: false);
				}
				num++;
			}
		}
		if (!string.IsNullOrEmpty(data.SupHandCardCode))
		{
			int supHandCardConsumeAmount = data.SupHandCardConsumeAmount;
			for (int j = 0; j < supHandCardConsumeAmount; j++)
			{
				allComsumeCardInfos[num].gameObject.SetActive(value: true);
				allComsumeCardInfos[num].LoadCard(data.SupHandCardCode);
				if (isCheckCardStat)
				{
					if (Singleton<GameManager>.Instance.Player.PlayerBattleInfo.AllEquipedSupHandCards.TryGetValue(data.SupHandCardCode, out var value2) && value2 > j)
					{
						allComsumeCardInfos[num].SetCardUsable();
						allCardStatTrans[num].gameObject.SetActive(value: false);
					}
					else
					{
						allComsumeCardInfos[num].SetCardUnusable();
						allCardStatTrans[num].gameObject.SetActive(value: true);
					}
				}
				else
				{
					allComsumeCardInfos[num].SetCardUsable();
					allCardStatTrans[num].gameObject.SetActive(value: false);
				}
				num++;
			}
		}
		for (int k = num; k < allComsumeCardInfos.Length; k++)
		{
			allComsumeCardInfos[k].gameObject.SetActive(value: false);
		}
	}

	public void HideComsume()
	{
		comsumePanel.gameObject.SetActive(value: false);
		cardComsumePanel.gameObject.SetActive(value: false);
	}

	public void ForceRebuildLayoutImmediate()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.m_RectTransform);
	}
}
