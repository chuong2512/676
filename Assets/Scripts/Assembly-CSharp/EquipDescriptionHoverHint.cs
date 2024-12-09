using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipDescriptionHoverHint : HoverWithKeysHint
{
	public Color containColor;

	public Color notContainColor;

	private RectTransform suitListRectTransform;

	private RectTransform suitInforRectTransform;

	private Text equipNameText;

	private Text equipTypeText;

	private Text contentText;

	private Transform lineObject;

	private Text suitNameText;

	private Transform suitListRoot;

	private GameObject[] allSuitNameObjs;

	private Text[] allSuitNames;

	private Transform suitInfoListRoot;

	private GameObject[] allSuitInfoObjs;

	private Text[] allSuitAmountTexts;

	private Text[] allSuitContentTexts;

	private Image suitIcon;

	private Text tmpVisualTxt;

	protected override void OnAwake()
	{
		base.OnAwake();
		equipNameText = base.transform.Find("NameBottom/TmpTxtBg/TitleBg/TitleTxt").GetComponent<Text>();
		tmpVisualTxt = base.transform.Find("NameBottom/TmpTxtBg").GetComponent<Text>();
		equipTypeText = base.transform.Find("TitleRoot/EquipType").GetComponent<Text>();
		contentText = base.transform.Find("Content").GetComponent<Text>();
		lineObject = base.transform.Find("Line");
		suitNameText = base.transform.Find("Line/EquipEffectTitle").GetComponent<Text>();
		suitListRoot = base.transform.Find("SuitList");
		suitListRectTransform = suitListRoot.GetComponent<RectTransform>();
		allSuitNameObjs = new GameObject[7];
		allSuitNames = new Text[7];
		for (int i = 0; i < allSuitNameObjs.Length; i++)
		{
			allSuitNameObjs[i] = suitListRoot.GetChild(i).gameObject;
			allSuitNames[i] = suitListRoot.GetChild(i).Find("Name").GetComponent<Text>();
		}
		suitInfoListRoot = base.transform.Find("SuitInfoList");
		suitInforRectTransform = suitInfoListRoot.GetComponent<RectTransform>();
		suitIcon = base.transform.Find("Line/SuitIcon").GetComponent<Image>();
		allSuitInfoObjs = new GameObject[3];
		allSuitAmountTexts = new Text[3];
		allSuitContentTexts = new Text[3];
		for (int j = 0; j < allSuitInfoObjs.Length; j++)
		{
			allSuitInfoObjs[j] = suitInfoListRoot.GetChild(j).gameObject;
			allSuitAmountTexts[j] = suitInfoListRoot.GetChild(j).Find("Bottom/Amount").GetComponent<Text>();
			allSuitContentTexts[j] = suitInfoListRoot.GetChild(j).Find("Content").GetComponent<Text>();
		}
	}

	protected override void InitKey()
	{
		keyRoot = base.transform.Find("NameBottom/KeyRoot");
	}

	public void SetEquipHoverDescriptionBaseInfo(string nameStr, string contentStr, string equipTypeStr)
	{
		string text3 = (tmpVisualTxt.text = (equipNameText.text = nameStr));
		contentText.text = contentStr;
		equipTypeText.text = equipTypeStr;
	}

	public void LoadSuitInfo(SuitHandler.SuitInfo suitInfo, bool checkContain)
	{
		lineObject.gameObject.SetActive(value: true);
		suitListRoot.gameObject.SetActive(value: true);
		suitInfoListRoot.gameObject.SetActive(value: true);
		SuitType suitType = (SuitType)Enum.Parse(typeof(SuitType), suitInfo.SuitType);
		suitNameText.text = (suitInfo.SuitType + "_Name").LocalizeText();
		lineObject.gameObject.SetActive(value: true);
		suitListRoot.gameObject.SetActive(value: true);
		suitInfoListRoot.gameObject.SetActive(value: true);
		suitIcon.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(suitInfo.SuitIconName, "Sprites/Suit");
		List<string> suitEquips = SuitHandler.GetSuitInfo(suitType).SuitEquips;
		HashSet<string> hashSet = (checkContain ? Singleton<GameManager>.Instance.Player.PlayerEquipment.SuitHandler.GetContainSuits(suitType) : null);
		for (int i = 0; i < suitEquips.Count; i++)
		{
			allSuitNameObjs[i].SetActive(value: true);
			EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(suitEquips[i]);
			allSuitNames[i].text = equipmentCardAttr.NameKey.LocalizeText();
			allSuitNames[i].color = ((!hashSet.IsNull() && hashSet.Contains(suitEquips[i])) ? SuitHandler.ContainColor : SuitHandler.LackColor);
		}
		for (int j = suitEquips.Count; j < 7; j++)
		{
			allSuitNameObjs[j].SetActive(value: false);
		}
		int[] suitNeedAmount = SuitHandler.GetSuitInfo(suitType).SuitNeedAmount;
		string[] suitContentKeys = SuitHandler.GetSuitInfo(suitType).SuitContentKeys;
		for (int k = 0; k < suitNeedAmount.Length; k++)
		{
			allSuitInfoObjs[k].gameObject.SetActive(value: true);
			allSuitAmountTexts[k].text = suitNeedAmount[k] + "Piece".LocalizeText();
			allSuitContentTexts[k].text = suitContentKeys[k].LocalizeText();
			allSuitAmountTexts[k].color = ((suitNeedAmount[k] <= ((!hashSet.IsNull()) ? hashSet.Count : 0)) ? SuitHandler.ContainColor : SuitHandler.LackColor);
		}
		for (int l = suitNeedAmount.Length; l < 3; l++)
		{
			allSuitInfoObjs[l].gameObject.SetActive(value: false);
		}
		AddKeys(suitInfo.SuitKeys);
	}

	public void HideSuitInfo()
	{
		lineObject.gameObject.SetActive(value: false);
		suitListRoot.gameObject.SetActive(value: false);
		suitInfoListRoot.gameObject.SetActive(value: false);
	}

	public void ForceRebuildLayoutImmediate()
	{
		StartCoroutine(ForceRebuildLayoutImmediate_IE());
	}

	private IEnumerator ForceRebuildLayoutImmediate_IE()
	{
		yield return null;
		LayoutRebuilder.ForceRebuildLayoutImmediate(suitListRectTransform);
		LayoutRebuilder.ForceRebuildLayoutImmediate(suitInforRectTransform);
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.m_RectTransform);
	}
}
