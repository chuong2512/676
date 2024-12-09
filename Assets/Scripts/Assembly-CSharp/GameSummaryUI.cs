using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSummaryUI : UIView
{
	private Action hideAction;

	private Transform mainCardRoot;

	private Transform supCardRoot;

	private CharacterUIEquipImgCheckSup suphandImgCheck;

	private CharacterUIEquipImgCheckSup mainhandImgCheck;

	private CharacterUIEquipImgCheckSup gloveImgCheck;

	private CharacterUIEquipImgCheckSup ornamentImgCheck;

	private CharacterUIEquipImgCheckSup helmetImgCheck;

	private CharacterUIEquipImgCheckSup breasplateImgCheck;

	private CharacterUIEquipImgCheckSup shoesImgCheck;

	private UIAnim_GameSummary anim;

	private Text playerDmgText;

	private Text playerDefenceAttrText;

	private Text memoryAmountText;

	private Text drawcardAmountText;

	private Text apAmountText;

	private Text armorAmountText;

	private Transform skillRoot;

	private Text btnText;

	private Image illustrationImg;

	private GameSummary_GameProgress _gameProgress;

	private Queue<SummaryCardCtrl> allSummaryCardPools = new Queue<SummaryCardCtrl>();

	private List<SummaryCardCtrl> allShowingSummaryCards = new List<SummaryCardCtrl>();

	private Queue<SummarySkillItemCtrl> allSummarySkillCtrlPools = new Queue<SummarySkillItemCtrl>();

	private List<SummarySkillItemCtrl> allShowingSummarySkillCtrls = new List<SummarySkillItemCtrl>();

	private Text backBtnText;

	public override string UIViewName => "GameSummaryUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		ShowSpecificPlayerRecord((RecordData)objs[0]);
		hideAction = (Action)objs[1];
		bool flag = (bool)objs[2];
		btnText.text = (flag ? "BACKTOMENU".LocalizeText() : "confirm".LocalizeText());
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("点击战绩条");
		anim.StartAnim();
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
		mainCardRoot = base.transform.Find("Bg/CardPanel/MainCardRoot/Mask/Content");
		supCardRoot = base.transform.Find("Bg/CardPanel/SupCardRoot/Mask/Content");
		Transform transform = base.transform.Find("Bg/PlayerInfoPanel");
		suphandImgCheck = transform.Find("ItemPack/SupHand/SupHand").GetComponent<CharacterUIEquipImgCheckSup>();
		playerDefenceAttrText = transform.Find("ItemPack/SupHand/Block").GetComponent<Text>();
		mainhandImgCheck = transform.Find("ItemPack/MainHand/MainHand").GetComponent<CharacterUIEquipImgCheckSup>();
		playerDmgText = transform.Find("ItemPack/MainHand/Dmg").GetComponent<Text>();
		gloveImgCheck = transform.Find("ItemPack/Glove/Glove").GetComponent<CharacterUIEquipImgCheckSup>();
		ornamentImgCheck = transform.Find("ItemPack/Ornament/Ornament").GetComponent<CharacterUIEquipImgCheckSup>();
		helmetImgCheck = transform.Find("ItemPack/Helmet/Helmet").GetComponent<CharacterUIEquipImgCheckSup>();
		breasplateImgCheck = transform.Find("ItemPack/Breastplate/Breastplate").GetComponent<CharacterUIEquipImgCheckSup>();
		shoesImgCheck = transform.Find("ItemPack/Shoes/Shoes").GetComponent<CharacterUIEquipImgCheckSup>();
		armorAmountText = transform.Find("AttrPack/Armor/Amount").GetComponent<Text>();
		drawcardAmountText = transform.Find("AttrPack/DrawCardAmount/Amount").GetComponent<Text>();
		apAmountText = transform.Find("AttrPack/ApAmount/Amount").GetComponent<Text>();
		memoryAmountText = transform.Find("AttrPack/Memory/Amount").GetComponent<Text>();
		skillRoot = transform.Find("SkillPanel");
		backBtnText = base.transform.Find("Bg/BackBtn/Text").GetComponent<Text>();
		base.transform.Find("Bg/BackBtn").GetComponent<Button>().onClick.AddListener(OnClickBackBtn);
		illustrationImg = base.transform.Find("Bg/PlayerInfoPanel/PlayerIllustration").GetComponent<Image>();
		_gameProgress = base.transform.Find("Bg/GameProgress").GetComponent<GameSummary_GameProgress>();
		btnText = base.transform.Find("Bg/BackBtn/Text").GetComponent<Text>();
		anim = GetComponent<UIAnim_GameSummary>();
		anim.Init();
	}

	private void ShowSpecificPlayerRecord(RecordData data)
	{
		RecycleAllCardSummaryObj();
		RecycleAllSkillItem();
		SetOccupationRelated(data.PlayerOccupation);
		LoadPlayerHandCards(data);
		LoadPlayerAttr(data);
		LoadEquipmentIcon(data);
		LoadSkill(data);
		_gameProgress.LoadGameProgress(data);
	}

	private void LoadPlayerHandCards(RecordData data)
	{
		List<Transform> list = new List<Transform>();
		List<Transform> list2 = new List<Transform>();
		foreach (KeyValuePair<string, int> mainHandCard in data.MainHandCards)
		{
			SummaryCardCtrl cardSummaryObj = GetCardSummaryObj(mainCardRoot);
			cardSummaryObj.LoadCard(mainHandCard.Key, mainHandCard.Value);
			cardSummaryObj.transform.SetSiblingIndex(list.Count);
			allShowingSummaryCards.Add(cardSummaryObj);
			list.Add(cardSummaryObj.transform);
		}
		foreach (KeyValuePair<string, int> supHandCard in data.SupHandCards)
		{
			SummaryCardCtrl cardSummaryObj2 = GetCardSummaryObj(supCardRoot);
			cardSummaryObj2.LoadCard(supHandCard.Key, supHandCard.Value);
			cardSummaryObj2.transform.SetSiblingIndex(list2.Count);
			allShowingSummaryCards.Add(cardSummaryObj2);
			list2.Add(cardSummaryObj2.transform);
		}
		anim.SetItems(list, list2);
	}

	private void LoadPlayerAttr(RecordData data)
	{
		memoryAmountText.text = data.MemoryAmount.ToString();
		drawcardAmountText.text = data.DrawCardAmount.ToString();
		apAmountText.text = data.ActionPointAmount.ToString();
		armorAmountText.text = data.ArmorAmount.ToString();
		playerDmgText.text = data.AtkDmg.ToString();
		playerDefenceAttrText.text = data.DefenceAttr.ToString();
	}

	private void LoadEquipmentIcon(RecordData data)
	{
		helmetImgCheck.LoadEquip(data.Helmet);
		ornamentImgCheck.LoadEquip(data.Ornament);
		suphandImgCheck.LoadEquip(data.SupHandWeapon);
		breasplateImgCheck.LoadEquip(data.Breasplate);
		mainhandImgCheck.LoadEquip(data.MainHandWeapon);
		shoesImgCheck.LoadEquip(data.Shoes);
		gloveImgCheck.LoadEquip(data.Glove);
	}

	public void SetOccupationRelated(PlayerOccupation playerOccupation)
	{
		OccupationData occupationData = DataManager.Instance.GetOccupationData(playerOccupation);
		illustrationImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(occupationData.GameSummaryIllustrationName, occupationData.DefaultSpritePath);
		_gameProgress.SetOccupation(occupationData);
	}

	private SummaryCardCtrl GetCardSummaryObj(Transform root)
	{
		if (allSummaryCardPools.Count > 0)
		{
			SummaryCardCtrl summaryCardCtrl = allSummaryCardPools.Dequeue();
			summaryCardCtrl.transform.SetParent(root);
			summaryCardCtrl.gameObject.SetActive(value: true);
			return summaryCardCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("SummaryCard", "Prefabs", root).GetComponent<SummaryCardCtrl>();
	}

	private void RecycleAllCardSummaryObj()
	{
		if (allShowingSummaryCards.Count > 0)
		{
			for (int i = 0; i < allShowingSummaryCards.Count; i++)
			{
				allShowingSummaryCards[i].gameObject.SetActive(value: false);
				allSummaryCardPools.Enqueue(allShowingSummaryCards[i]);
			}
			allShowingSummaryCards.Clear();
		}
	}

	private void LoadSkill(RecordData data)
	{
		for (int i = 0; i < data.AllEquipedSkills.Count; i++)
		{
			if (!data.AllEquipedSkills[i].IsNullOrEmpty())
			{
				SummarySkillItemCtrl summarySkillItem = GetSummarySkillItem();
				summarySkillItem.LoadSkill(data.PlayerOccupation, data.AllEquipedSkills[i]);
				summarySkillItem.transform.SetSiblingIndex(allShowingSummarySkillCtrls.Count);
				allShowingSummarySkillCtrls.Add(summarySkillItem);
			}
		}
	}

	private SummarySkillItemCtrl GetSummarySkillItem()
	{
		if (allSummarySkillCtrlPools.Count > 0)
		{
			SummarySkillItemCtrl summarySkillItemCtrl = allSummarySkillCtrlPools.Dequeue();
			summarySkillItemCtrl.gameObject.SetActive(value: true);
			return summarySkillItemCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("SummarySkillItem", "Prefabs", skillRoot).GetComponent<SummarySkillItemCtrl>();
	}

	private void RecycleAllSkillItem()
	{
		if (allShowingSummarySkillCtrls.Count > 0)
		{
			for (int i = 0; i < allShowingSummarySkillCtrls.Count; i++)
			{
				allShowingSummarySkillCtrls[i].gameObject.SetActive(value: false);
				allSummarySkillCtrlPools.Enqueue(allShowingSummarySkillCtrls[i]);
			}
			allShowingSummarySkillCtrls.Clear();
		}
	}

	private void OnClickBackBtn()
	{
		hideAction?.Invoke();
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("退出按钮");
		CloseUI();
	}

	private void CloseUI()
	{
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}
}
