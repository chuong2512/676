using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetailInfoUI : EscCloseUIView
{
	private CharacterUIEquipImgCheckSup ornamentImgCheckSup;

	private CharacterUIEquipImgCheckSup gloveImgCheckSup;

	private CharacterUIEquipImgCheckSup helmetImgCheckSup;

	private CharacterUIEquipImgCheckSup shoesImgCheckSup;

	private CharacterUIEquipImgCheckSup breasplateImgCheckSup;

	private CharacterUIEquipImgCheckSup mainHandImgCheckSup;

	private CharacterUIEquipImgCheckSup supHandImgCheckSup;

	private Text atkDmgText;

	private Text defenceAttrAmountText;

	private Text memoryAmountText;

	private Text apAmountText;

	private Text drawcardsAmountText;

	private Text armorAmountText;

	private Transform skillRoot;

	private Text playerOccupationDesText;

	private Text playerOccupationSpecificDesText;

	private Transform bgTrans;

	private Queue<PlayerInfoSkillIconCtrl> allSkillIconPools = new Queue<PlayerInfoSkillIconCtrl>();

	private HashSet<PlayerInfoSkillIconCtrl> allShowingSkillIcons = new HashSet<PlayerInfoSkillIconCtrl>();

	public override string UIViewName => "PlayerDetailInfoUI";

	public override string UILayerName => "NormalLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		ShowPlayerInfo();
		bgTrans.localScale = Vector3.zero;
		bgTrans.DOScale(1f, 0.2f);
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		RecycleAllSkillIcon();
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destroy Player Detail Info UI...");
	}

	protected override void OnHide()
	{
		base.OnHide();
		OnClickCloseBtn();
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
	}

	public override void OnSpawnUI()
	{
		bgTrans = base.transform.Find("Mask/Bg");
		ornamentImgCheckSup = bgTrans.Find("Ornament/Ornament").GetComponent<CharacterUIEquipImgCheckSup>();
		gloveImgCheckSup = bgTrans.Find("Glove/Glove").GetComponent<CharacterUIEquipImgCheckSup>();
		helmetImgCheckSup = bgTrans.Find("Helmet/Helmet").GetComponent<CharacterUIEquipImgCheckSup>();
		shoesImgCheckSup = bgTrans.Find("Shoes/Shoes").GetComponent<CharacterUIEquipImgCheckSup>();
		breasplateImgCheckSup = bgTrans.Find("Breastplate/Breastplate").GetComponent<CharacterUIEquipImgCheckSup>();
		mainHandImgCheckSup = bgTrans.Find("MainHand/MainHand").GetComponent<CharacterUIEquipImgCheckSup>();
		supHandImgCheckSup = bgTrans.Find("SupHand/SupHand").GetComponent<CharacterUIEquipImgCheckSup>();
		atkDmgText = bgTrans.Find("MainHand/Dmg").GetComponent<Text>();
		defenceAttrAmountText = bgTrans.Find("SupHand/Block").GetComponent<Text>();
		memoryAmountText = bgTrans.Find("Memory/Amount").GetComponent<Text>();
		apAmountText = bgTrans.Find("ApAmount/Amount").GetComponent<Text>();
		drawcardsAmountText = bgTrans.Find("DrawCardAmount/Amount").GetComponent<Text>();
		armorAmountText = bgTrans.Find("Armor/Amount").GetComponent<Text>();
		skillRoot = bgTrans.Find("SkillRoot");
		playerOccupationDesText = bgTrans.Find("IntroductionBg/PlayerOccupationDes").GetComponent<Text>();
		playerOccupationSpecificDesText = bgTrans.Find("IntroductionBg/PlayerOccupationSpecificDes").GetComponent<Text>();
		bgTrans.Find("CloseBtn").GetComponent<Button>().onClick.AddListener(OnClickCloseBtn);
	}

	private void ShowPlayerInfo()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("装备界面");
		Player player = Singleton<GameManager>.Instance.Player;
		ornamentImgCheckSup.LoadEquip(player.PlayerEquipment.Ornament.CardCode);
		gloveImgCheckSup.LoadEquip(player.PlayerEquipment.Glove.CardCode);
		helmetImgCheckSup.LoadEquip(player.PlayerEquipment.Helmet.CardCode);
		shoesImgCheckSup.LoadEquip(player.PlayerEquipment.Shoes.CardCode);
		breasplateImgCheckSup.LoadEquip(player.PlayerEquipment.Breastplate.CardCode);
		mainHandImgCheckSup.LoadEquip(player.PlayerEquipment.MainHandWeapon.CardCode);
		supHandImgCheckSup.LoadEquip(player.PlayerEquipment.SupHandWeapon.CardCode);
		atkDmgText.text = player.PlayerAttr.BaseAtkDmg.ToString();
		defenceAttrAmountText.text = player.PlayerAttr.DefenceAttr.ToString();
		memoryAmountText.text = player.PlayerAttr.MemoryAmount.ToString();
		apAmountText.text = player.PlayerAttr.BaseApAmount.ToString();
		drawcardsAmountText.text = player.PlayerAttr.DrawCardAmount.ToString();
		armorAmountText.text = player.PlayerAttr.Armor.ToString();
		playerOccupationDesText.text = (player.PlayerOccupation.ToString() + "_Des").LocalizeText();
		playerOccupationSpecificDesText.text = (player.PlayerOccupation.ToString() + "_SpecificDes").LocalizeText();
		LoadSkill();
	}

	private void LoadSkill()
	{
		List<string> currentSkillList = Singleton<GameManager>.Instance.Player.PlayerBattleInfo.CurrentSkillList;
		for (int i = 0; i < currentSkillList.Count; i++)
		{
			if (!currentSkillList[i].IsNullOrEmpty())
			{
				PlayerInfoSkillIconCtrl skilllICon = GetSkilllICon();
				skilllICon.LoadSkill(currentSkillList[i]);
				allShowingSkillIcons.Add(skilllICon);
			}
		}
	}

	private PlayerInfoSkillIconCtrl GetSkilllICon()
	{
		if (allSkillIconPools.Count > 0)
		{
			PlayerInfoSkillIconCtrl playerInfoSkillIconCtrl = allSkillIconPools.Dequeue();
			playerInfoSkillIconCtrl.gameObject.SetActive(value: true);
			return playerInfoSkillIconCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("PlayerInfoSkillIcon", "Prefabs", skillRoot).GetComponent<PlayerInfoSkillIconCtrl>();
	}

	private void RecycleAllSkillIcon()
	{
		if (allShowingSkillIcons.Count <= 0)
		{
			return;
		}
		foreach (PlayerInfoSkillIconCtrl allShowingSkillIcon in allShowingSkillIcons)
		{
			allShowingSkillIcon.gameObject.SetActive(value: false);
			allSkillIconPools.Enqueue(allShowingSkillIcon);
		}
		allShowingSkillIcons.Clear();
	}

	private void OnClickCloseBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}
}
