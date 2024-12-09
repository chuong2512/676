using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleEquipDesCtrl : MonoBehaviour, ILocalization
{
	public Sprite CNEquipedSprite;

	public Sprite ENEquipedSprite;

	private RectTransform m_RectTransform;

	private RectTransform suitListRectTransform;

	private RectTransform suitInfoRectTransform;

	private Image equipIconImg;

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

	private GameObject functionBtnRootObj;

	private Button functionBtn;

	private Text functionBtnNameText;

	private GameObject equipedHintObject;

	private Action functionAction;

	private Image suitIcon;

	private void Awake()
	{
		m_RectTransform = GetComponent<RectTransform>();
		equipIconImg = base.transform.Find("TitleRoot/EquipIcon").GetComponent<Image>();
		equipNameText = base.transform.Find("TitleRoot/EquipName").GetComponent<Text>();
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
		suitInfoRectTransform = suitInfoListRoot.GetComponent<RectTransform>();
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
		functionBtnRootObj = base.transform.Find("FunctionButton").gameObject;
		functionBtn = base.transform.Find("FunctionButton/Button").GetComponent<Button>();
		functionBtn.onClick.AddListener(OnClickFucntionBtn);
		functionBtnNameText = base.transform.Find("FunctionButton/Button/FunctionName").GetComponent<Text>();
		equipedHintObject = base.transform.Find("EquipedHint").gameObject;
		Localization();
	}

	public void Localization()
	{
		base.transform.Find("EquipedHint/Hint").GetComponent<Image>().sprite = ((SingletonDontDestroy<SettingManager>.Instance.Language == 0) ? CNEquipedSprite : ENEquipedSprite);
	}

	private void OnDisable()
	{
		functionBtnRootObj.gameObject.SetActive(value: false);
		functionAction = null;
	}

	public void LoadEquip(string equipCode, bool isEquiped, Action<List<KeyValuePair>, bool> AddKeysAction)
	{
		EquipmentCard equipmentCard = FactoryManager.GetEquipmentCard(equipCode);
		equipTypeText.text = equipmentCard.EquipmentCardAttr.EquipmentType.ToString().LocalizeText();
		equipNameText.text = equipmentCard.CardName;
		equipIconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(equipmentCard.ImageName, "Sprites/Equipment");
		contentText.text = equipmentCard.CardNormalDes;
		List<KeyValuePair> keyDescription = equipmentCard.GetKeyDescription();
		AddKeysAction?.Invoke(keyDescription, arg2: true);
		if (equipmentCard.SuitType != 0)
		{
			SuitHandler.SuitInfo suitInfo = SuitHandler.GetSuitInfo(equipmentCard.SuitType);
			AddKeysAction?.Invoke(suitInfo.SuitKeys, arg2: true);
			suitNameText.text = (equipmentCard.SuitType.ToString() + "_Name").LocalizeText();
			lineObject.gameObject.SetActive(value: true);
			suitListRoot.gameObject.SetActive(value: true);
			suitInfoListRoot.gameObject.SetActive(value: true);
			suitIcon.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(suitInfo.SuitIconName, "Sprites/Suit");
			HashSet<string> containSuits = Singleton<GameManager>.Instance.Player.PlayerEquipment.SuitHandler.GetContainSuits(equipmentCard.SuitType);
			List<string> suitEquips = SuitHandler.GetSuitInfo(equipmentCard.SuitType).SuitEquips;
			for (int i = 0; i < suitEquips.Count; i++)
			{
				allSuitNameObjs[i].SetActive(value: true);
				EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(suitEquips[i]);
				allSuitNames[i].text = equipmentCardAttr.NameKey.LocalizeText();
				allSuitNames[i].color = ((!containSuits.IsNull() && containSuits.Contains(suitEquips[i])) ? SuitHandler.ContainColor : SuitHandler.LackColor);
			}
			for (int j = suitEquips.Count; j < 7; j++)
			{
				allSuitNameObjs[j].SetActive(value: false);
			}
			int[] suitNeedAmount = SuitHandler.GetSuitInfo(equipmentCard.SuitType).SuitNeedAmount;
			string[] suitContentKeys = SuitHandler.GetSuitInfo(equipmentCard.SuitType).SuitContentKeys;
			for (int k = 0; k < suitNeedAmount.Length; k++)
			{
				allSuitInfoObjs[k].gameObject.SetActive(value: true);
				allSuitAmountTexts[k].text = suitNeedAmount[k] + "Piece".LocalizeText();
				allSuitContentTexts[k].color = ((suitNeedAmount[k] <= ((!containSuits.IsNull()) ? containSuits.Count : 0)) ? SuitHandler.ContainColor : SuitHandler.LackColor_White);
				allSuitAmountTexts[k].color = ((suitNeedAmount[k] <= ((!containSuits.IsNull()) ? containSuits.Count : 0)) ? SuitHandler.ContainColor : SuitHandler.LackColor);
				allSuitContentTexts[k].text = suitContentKeys[k].LocalizeText();
			}
			for (int l = suitNeedAmount.Length; l < 3; l++)
			{
				allSuitInfoObjs[l].gameObject.SetActive(value: false);
			}
		}
		else
		{
			lineObject.gameObject.SetActive(value: false);
			suitListRoot.gameObject.SetActive(value: false);
			suitInfoListRoot.gameObject.SetActive(value: false);
		}
		equipedHintObject.SetActive(isEquiped);
		StartCoroutine(ReBuild());
	}

	private IEnumerator ReBuild()
	{
		yield return null;
		LayoutRebuilder.ForceRebuildLayoutImmediate(suitListRectTransform);
		LayoutRebuilder.ForceRebuildLayoutImmediate(suitInfoRectTransform);
		LayoutRebuilder.ForceRebuildLayoutImmediate(m_RectTransform);
	}

	public void LoadEquip(string equipCode, bool isEquiped, Action<List<KeyValuePair>, bool> AddKeysAction, string btnName, Action functionAction, bool isInteractive)
	{
		if (functionAction != null && !btnName.IsNullOrEmpty())
		{
			this.functionAction = functionAction;
			functionBtnRootObj.gameObject.SetActive(value: true);
			functionBtnNameText.text = btnName;
			functionBtn.interactable = isInteractive;
		}
		LoadEquip(equipCode, isEquiped, AddKeysAction);
	}

	private void OnClickFucntionBtn()
	{
		functionAction?.Invoke();
		SingletonDontDestroy<UIManager>.Instance.HideView("EquipDesUI");
	}
}
