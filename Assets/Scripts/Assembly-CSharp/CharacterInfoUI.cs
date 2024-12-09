using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoUI : UIView
{
	private CharacterUIEquipImgCheckSup ornamentImgCheckSup;

	private CharacterUIEquipImgCheckSup gloveImgCheckSup;

	private CharacterUIEquipImgCheckSup helmetImgCheckSup;

	private CharacterUIEquipImgCheckSup shoesImgCheckSup;

	private CharacterUIEquipImgCheckSup breastplateImgCheckSup;

	private CharacterUIEquipImgCheckSup mainhandImgCheckSup;

	private CharacterUIEquipImgCheckSup suphandImgCheckSup;

	private Text weaponDmgText;

	private Text defenceAttrText;

	private Text drawCardsAmountText;

	private Text armorAmountText;

	private Text memoryText;

	private Text apAmountText;

	private Image healthFillImg;

	private Image expBarFillImg;

	private Image characterIlluImg;

	private Text healthText;

	private Outline healthTxtOutline;

	private int healthCacheAmount;

	private Text levelText;

	private SingleProphesyInfoIconCtrl[] allProphesyInfoIcons;

	private Transform giftRoot;

	private Queue<GiftCtrl> allGiftCtrlPool = new Queue<GiftCtrl>();

	private Dictionary<BaseGift, GiftCtrl> allShowingGiftCtrls = new Dictionary<BaseGift, GiftCtrl>();

	private Transform equipChangeHintRoot;

	private Image equipChangeHintImg;

	private Tween equipChangeRotateTween;

	private Tween equipChangeFadeTween;

	private Tween equipChangeMoveTween;

	public override string UIViewName => "CharacterInfoUI";

	public override string UILayerName => "NormalLayer";

	public Transform HealtnImgTrans => healthFillImg.transform;

	public Transform bubbleHintPoint { get; private set; }

	public override void OnSpawnUI()
	{
		InitCharacterInfoUI();
		InitGift();
		InitEquipChangeHint();
	}

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		LoadPlayerInfo();
		EventManager.RegisterEvent(EventEnum.E_UpdatePlayerHealth, UpdatePlayerHealth);
		EventManager.RegisterEvent(EventEnum.E_PlayerExpUpdate, UpdatePlayerExp);
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		RemoveAllGift();
		EventManager.UnregisterEvent(EventEnum.E_UpdatePlayerHealth, UpdatePlayerHealth);
		EventManager.UnregisterEvent(EventEnum.E_PlayerExpUpdate, UpdatePlayerExp);
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destroy Character Info UI...");
	}

	private void InitCharacterInfoUI()
	{
		ornamentImgCheckSup = base.transform.Find("Bg/Ornament/Ornament").GetComponent<CharacterUIEquipImgCheckSup>();
		gloveImgCheckSup = base.transform.Find("Bg/Glove/Glove").GetComponent<CharacterUIEquipImgCheckSup>();
		helmetImgCheckSup = base.transform.Find("Bg/Helmet/Helmet").GetComponent<CharacterUIEquipImgCheckSup>();
		shoesImgCheckSup = base.transform.Find("Bg/Shoes/Shoes").GetComponent<CharacterUIEquipImgCheckSup>();
		breastplateImgCheckSup = base.transform.Find("Bg/Breastplate/Breastplate").GetComponent<CharacterUIEquipImgCheckSup>();
		mainhandImgCheckSup = base.transform.Find("Bg/MainHand/MainHand").GetComponent<CharacterUIEquipImgCheckSup>();
		suphandImgCheckSup = base.transform.Find("Bg/SupHand/SupHand").GetComponent<CharacterUIEquipImgCheckSup>();
		weaponDmgText = base.transform.Find("Bg/MainHand/Dmg").GetComponent<Text>();
		defenceAttrText = base.transform.Find("Bg/SupHand/Block").GetComponent<Text>();
		drawCardsAmountText = base.transform.Find("Bg/DrawCardAmount/Amount").GetComponent<Text>();
		armorAmountText = base.transform.Find("Bg/Armor/Amount").GetComponent<Text>();
		memoryText = base.transform.Find("Bg/Memory/Amount").GetComponent<Text>();
		apAmountText = base.transform.Find("Bg/ApAmount/Amount").GetComponent<Text>();
		healthFillImg = base.transform.Find("Bg/HealthBar/Fill").GetComponent<Image>();
		characterIlluImg = base.transform.Find("Bg/CharacterImg").GetComponent<Image>();
		healthText = base.transform.Find("Bg/HealthBar/HealthText").GetComponent<Text>();
		healthTxtOutline = healthText.GetComponent<Outline>();
		expBarFillImg = base.transform.Find("Bg/ExpBar").GetComponent<Image>();
		levelText = base.transform.Find("Bg/LevelText").GetComponent<Text>();
		bubbleHintPoint = base.transform.Find("Bg/BubbleHintPoint");
		allProphesyInfoIcons = new SingleProphesyInfoIconCtrl[3];
		for (int i = 0; i < allProphesyInfoIcons.Length; i++)
		{
			allProphesyInfoIcons[i] = base.transform.Find("Bg/ProphesyRoot").GetChild(i).GetComponent<SingleProphesyInfoIconCtrl>();
		}
	}

	private void LoadPlayerInfo()
	{
		PlayerEquipment playerEquipment = Singleton<GameManager>.Instance.Player.PlayerEquipment;
		PlayerAttr playerAttr = Singleton<GameManager>.Instance.Player.PlayerAttr;
		EquipOrnament(playerEquipment.Ornament.CardCode, isNeedHint: false);
		EquipGlove(playerEquipment.Glove.CardCode, isNeedHint: false);
		EquipHelmet(playerEquipment.Helmet.CardCode, isNeedHint: false);
		EquipShoes(playerEquipment.Shoes.CardCode, isNeedHint: false);
		EquipBreastplate(playerEquipment.Breastplate.CardCode, isNeedHint: false);
		EquipMainHand(playerEquipment.MainHandWeapon.CardCode, isNeedHint: false);
		EquipSupHand(playerEquipment.SupHandWeapon.CardCode, isNeedHint: false);
		SetWeaponDmg(playerAttr.BaseAtkDmg);
		SetDefenceAttrAmount(playerAttr.BaseDefenceAttr);
		SetArmorAmount(playerAttr.BaseArmor);
		SetDrawCardsAmount(playerAttr.DrawCardAmount);
		SetHealthInfo(playerAttr.Health, playerAttr.MaxHealth);
		SetMemory(playerAttr.MemoryAmount);
		SetApAmount(playerAttr.BaseApAmount);
		SetPlayerExpInfo(playerAttr.CurrentExp, playerAttr.NextLevelExp);
		LoadCharacterIllustration();
		LoadGift();
		LoadPlayerProphesyInfo(Singleton<GameManager>.Instance.Player.AllProphesyCards, isShow: true);
	}

	public void LoadPlayerProphesyInfo(List<string> allProphesyCards, bool isShow)
	{
		if (allProphesyCards == null || allProphesyCards.Count == 0)
		{
			for (int i = 0; i < allProphesyInfoIcons.Length; i++)
			{
				allProphesyInfoIcons[i].gameObject.SetActive(value: false);
			}
			return;
		}
		for (int j = 0; j < allProphesyCards.Count; j++)
		{
			allProphesyInfoIcons[j].gameObject.SetActive(isShow);
			ProphesyCardData prophesyCardDataByCardData = DataManager.Instance.GetProphesyCardDataByCardData(allProphesyCards[j]);
			allProphesyInfoIcons[j].LoadCard(prophesyCardDataByCardData);
		}
		for (int k = allProphesyCards.Count; k < 3; k++)
		{
			allProphesyInfoIcons[k].gameObject.SetActive(value: false);
		}
	}

	public Transform GetProphesyCardCtrlByIndex(int index)
	{
		return allProphesyInfoIcons[index].transform;
	}

	public void ActiveProphesyCardCtrlByIndex(int index)
	{
		allProphesyInfoIcons[index].gameObject.SetActive(value: true);
	}

	private void LoadCharacterIllustration()
	{
		Player player = Singleton<GameManager>.Instance.Player;
		OccupationData occupationData = DataManager.Instance.GetOccupationData(player.PlayerOccupation);
		characterIlluImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(occupationData.CharacterUIIllustraionSprite, occupationData.DefaultSpritePath);
	}

	private void UpdatePlayerHealth(EventData eventData)
	{
		SetHealthInfo(Singleton<GameManager>.Instance.Player.PlayerAttr.Health, Singleton<GameManager>.Instance.Player.PlayerAttr.MaxHealth);
	}

	private void UpdatePlayerExp(EventData data)
	{
		SetPlayerExpInfo(Singleton<GameManager>.Instance.Player.PlayerAttr.CurrentExp, Singleton<GameManager>.Instance.Player.PlayerAttr.NextLevelExp);
	}

	private void SetPlayerExpInfo(int currentExp, int nextLevelExp)
	{
		float fillAmount = (float)currentExp / (float)nextLevelExp;
		expBarFillImg.fillAmount = fillAmount;
		levelText.text = $"Lv:{Singleton<GameManager>.Instance.Player.PlayerAttr.Level}";
	}

	public void SetHealthInfo(int health, int maxHealth)
	{
		float num = (float)health / (float)maxHealth;
		if (health == healthCacheAmount)
		{
			healthFillImg.fillAmount = num;
			healthText.text = $"{health} / {maxHealth}";
			return;
		}
		healthText.DOKill();
		healthTxtOutline.DOKill();
		healthFillImg.DOKill();
		int currentNum = healthCacheAmount;
		healthCacheAmount = health;
		healthTxtOutline.effectColor = healthText.color;
		healthTxtOutline.DOColor(Color.black, 0.7f);
		healthFillImg.DOFillAmount(num, 0.7f);
		DOTween.To(() => currentNum, delegate(int x)
		{
			currentNum = x;
		}, health, 0.7f).OnUpdate(delegate
		{
			healthText.text = $"{currentNum} / {maxHealth}";
		});
	}

	public void SetArmorAmount(int armorAmount)
	{
		armorAmountText.text = armorAmount.ToString();
	}

	public void SetDrawCardsAmount(int drawAmount)
	{
		drawCardsAmountText.text = drawAmount.ToString();
	}

	public void SetDefenceAttrAmount(int defenceAttrAmount)
	{
		defenceAttrText.text = defenceAttrAmount.ToString();
	}

	public void SetWeaponDmg(int dmg)
	{
		weaponDmgText.text = dmg.ToString();
	}

	public void SetMemory(int memory)
	{
		memoryText.text = memory.ToString();
	}

	public void SetApAmount(int ap)
	{
		apAmountText.text = ap.ToString();
	}

	public void EquipSupHand(string equipCode, bool isNeedHint)
	{
		suphandImgCheckSup.LoadEquip(equipCode);
		if (isNeedHint)
		{
			ShowEquipChangeHint(suphandImgCheckSup.transform);
		}
	}

	public void EquipMainHand(string equipCode, bool isNeedHint)
	{
		mainhandImgCheckSup.LoadEquip(equipCode);
		if (isNeedHint)
		{
			ShowEquipChangeHint(mainhandImgCheckSup.transform);
		}
	}

	public void EquipBreastplate(string equipCode, bool isNeedHint)
	{
		breastplateImgCheckSup.LoadEquip(equipCode);
		if (isNeedHint)
		{
			ShowEquipChangeHint(breastplateImgCheckSup.transform);
		}
	}

	public void EquipShoes(string equipCode, bool isNeedHint)
	{
		shoesImgCheckSup.LoadEquip(equipCode);
		if (isNeedHint)
		{
			ShowEquipChangeHint(shoesImgCheckSup.transform);
		}
	}

	public void EquipHelmet(string equipCode, bool isNeedHint)
	{
		helmetImgCheckSup.LoadEquip(equipCode);
		if (isNeedHint)
		{
			ShowEquipChangeHint(helmetImgCheckSup.transform);
		}
	}

	public void EquipGlove(string equipCode, bool isNeedHint)
	{
		gloveImgCheckSup.LoadEquip(equipCode);
		if (isNeedHint)
		{
			ShowEquipChangeHint(gloveImgCheckSup.transform);
		}
	}

	public void EquipOrnament(string equipCode, bool isNeedHint)
	{
		ornamentImgCheckSup.LoadEquip(equipCode);
		if (isNeedHint)
		{
			ShowEquipChangeHint(ornamentImgCheckSup.transform);
		}
	}

	private void InitGift()
	{
		giftRoot = base.transform.Find("Bg/HealthBar/GiftRoot");
	}

	public void AddGift(BaseGift gift)
	{
		GiftCtrl giftCtrl = GetGiftCtrl();
		giftCtrl.LoadGift(gift);
		allShowingGiftCtrls.Add(gift, giftCtrl);
	}

	private void LoadGift()
	{
		foreach (BaseGift allGift in Singleton<GameManager>.Instance.Player.PlayerBattleInfo.allGifts)
		{
			AddGift(allGift);
		}
	}

	private GiftCtrl GetGiftCtrl()
	{
		if (allGiftCtrlPool.Count > 0)
		{
			GiftCtrl giftCtrl = allGiftCtrlPool.Dequeue();
			giftCtrl.gameObject.SetActive(value: true);
			return giftCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("GiftCtrl", "Prefabs", giftRoot).GetComponent<GiftCtrl>();
	}

	public void RemoveAllGift()
	{
		if (allShowingGiftCtrls.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<BaseGift, GiftCtrl> allShowingGiftCtrl in allShowingGiftCtrls)
		{
			allShowingGiftCtrl.Value.gameObject.SetActive(value: false);
			allGiftCtrlPool.Enqueue(allShowingGiftCtrl.Value);
		}
		allShowingGiftCtrls.Clear();
	}

	private void InitEquipChangeHint()
	{
		equipChangeHintRoot = base.transform.Find("Bg/EquipChangeHint");
		equipChangeHintImg = base.transform.Find("Bg/EquipChangeHint/EquipChangeHint").GetComponent<Image>();
	}

	private void ShowEquipChangeHint(Transform targetTrans)
	{
		if (base.gameObject.activeSelf)
		{
			if (equipChangeRotateTween != null && equipChangeRotateTween.IsActive())
			{
				equipChangeRotateTween.Kill();
			}
			if (equipChangeFadeTween != null && equipChangeFadeTween.IsActive())
			{
				equipChangeFadeTween.Kill();
			}
			if (equipChangeMoveTween != null && equipChangeMoveTween.IsActive())
			{
				equipChangeMoveTween.Kill();
			}
			equipChangeHintImg.transform.localRotation = Quaternion.identity;
			equipChangeHintRoot.position = targetTrans.position;
			equipChangeHintImg.color = Color.white;
			StartCoroutine(ShowEquipChangeHint_IE());
		}
	}

	private IEnumerator ShowEquipChangeHint_IE()
	{
		equipChangeRotateTween = equipChangeHintImg.transform.DOLocalRotate(new Vector3(0f, 0f, -25f), 1.2f);
		yield return new WaitForSeconds(0.2f);
		equipChangeFadeTween = equipChangeHintImg.DOFade(0f, 1f);
		equipChangeMoveTween = equipChangeHintRoot.transform.DOLocalMoveY(equipChangeHintRoot.localPosition.y - 40f, 1f);
	}
}
