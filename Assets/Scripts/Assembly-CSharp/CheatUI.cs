using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class CheatUI : UIView
{
	private Transform cheatUIPanel;

	private bool isShow;

	private InputField spacetimeInputField;

	private InputField preEndMapLevelInputField;

	private InputField getEquipByTypeInputField;

	private InputField specialUsualCardInputField;

	private InputField suphandDrawSpecificCardInputField;

	private InputField mainhandDrawSpecificCardInputField;

	private Dropdown giftDropDown;

	private InputField addMonsterInputField;

	private InputField eventInputField;

	private InputField skillInputField;

	private InputField equipmentInputField;

	private Dropdown buffTypeDropDown;

	private Toggle buffIsPlayerToggle;

	private InputField enemyIndexInputField;

	private InputField buffAmountInputField;

	private InputField varifyPlayerMaxHealthInputField;

	private InputField varifyPlayerHealthInputField;

	private InputField battleStartMonsterHeapInputField;

	private InputField addMoneyInputField;

	private InputField drawSupHandCardInputField;

	private InputField drawMainHandCardInputField;

	private InputField skillsInput;

	public override string UIViewName => "CheatUI";

	public override string UILayerName => "TipsLayer";

	public override bool AutoHideBySwitchScene => false;

	public override void OnSpawnUI()
	{
		cheatUIPanel = base.transform.Find("Bg");
		cheatUIPanel.gameObject.SetActive(value: false);
		base.transform.Find("CheatBtn").GetComponent<Button>().onClick.AddListener(OnClickCheatUIBtn);
		base.transform.Find("Bg/List2/KillAllEnemies").GetComponent<Button>().onClick.AddListener(OnClickKillAllEnemies);
		base.transform.Find("Bg/List2/DrawMainHand").GetComponent<Button>().onClick.AddListener(DrawMainHand);
		drawMainHandCardInputField = base.transform.Find("Bg/List2/DrawMainHand/InputField").GetComponent<InputField>();
		base.transform.Find("Bg/List2/DrawSupHand").GetComponent<Button>().onClick.AddListener(DrawSupHand);
		drawSupHandCardInputField = base.transform.Find("Bg/List2/DrawSupHand/InputField").GetComponent<InputField>();
		base.transform.Find("Bg/List1/SaveGame").GetComponent<Button>().onClick.AddListener(SaveGame);
		base.transform.Find("Bg/List1/AddMoney").GetComponent<Button>().onClick.AddListener(OnVarifyMoney);
		addMoneyInputField = base.transform.Find("Bg/List1/AddMoney/InputField").GetComponent<InputField>();
		base.transform.Find("Bg/List1/StartBattle").GetComponent<Button>().onClick.AddListener(OnStartBattle);
		battleStartMonsterHeapInputField = base.transform.Find("Bg/List1/StartBattle/InputField").GetComponent<InputField>();
		base.transform.Find("Bg/List3/VarifyPlayerHealth").GetComponent<Button>().onClick.AddListener(VarifyPlayerHealth);
		varifyPlayerHealthInputField = base.transform.Find("Bg/List3/VarifyPlayerHealth/InputField").GetComponent<InputField>();
		base.transform.Find("Bg/List3/VarifyPlayerMaxHealth").GetComponent<Button>().onClick.AddListener(VarifyPlayerMaxHealth);
		varifyPlayerMaxHealthInputField = base.transform.Find("Bg/List3/VarifyPlayerMaxHealth/InputField").GetComponent<InputField>();
		base.transform.Find("Bg/List2/GetBuff").GetComponent<Button>().onClick.AddListener(GetBuff);
		buffTypeDropDown = base.transform.Find("Bg/List2/GetBuff/Dropdown").GetComponent<Dropdown>();
		buffIsPlayerToggle = base.transform.Find("Bg/List2/GetBuff/Toggle").GetComponent<Toggle>();
		enemyIndexInputField = base.transform.Find("Bg/List2/GetBuff/Index").GetComponent<InputField>();
		buffAmountInputField = base.transform.Find("Bg/List2/GetBuff/Amount").GetComponent<InputField>();
		base.transform.Find("Bg/List1/GetEquipment").GetComponent<Button>().onClick.AddListener(GetEquipment);
		equipmentInputField = base.transform.Find("Bg/List1/GetEquipment/InputField").GetComponent<InputField>();
		base.transform.Find("Bg/List1/GetSkill").GetComponent<Button>().onClick.AddListener(GetSkill);
		skillInputField = base.transform.Find("Bg/List1/GetSkill/InputField").GetComponent<InputField>();
		base.transform.Find("Bg/List1/TriggerEvent").GetComponent<Button>().onClick.AddListener(TriggerEvent);
		eventInputField = base.transform.Find("Bg/List1/TriggerEvent/InputField").GetComponent<InputField>();
		base.transform.Find("Bg/List1/SwitchMap").GetComponent<Button>().onClick.AddListener(SwitchMap);
		base.transform.Find("Bg/List2/AddMonster").GetComponent<Button>().onClick.AddListener(AddMonster);
		addMonsterInputField = base.transform.Find("Bg/List2/AddMonster/InputField").GetComponent<InputField>();
		base.transform.Find("Bg/List2/GetAp").GetComponent<Button>().onClick.AddListener(GetAp);
		base.transform.Find("Bg/List2/GetFaith").GetComponent<Button>().onClick.AddListener(GetFaith);
		base.transform.Find("Bg/List1/OpenShop").GetComponent<Button>().onClick.AddListener(OpenShop);
		giftDropDown = base.transform.Find("Bg/List1/GetGift/Dropdown").GetComponent<Dropdown>();
		base.transform.Find("Bg/List1/GetGift").GetComponent<Button>().onClick.AddListener(GetGift);
		base.transform.Find("Bg/List1/GetAllSkills").GetComponent<Button>().onClick.AddListener(GetAllSkills);
		base.transform.Find("Bg/List1/GetAllUsualCards").GetComponent<Button>().onClick.AddListener(GetAllUsualCards);
		base.transform.Find("Bg/List1/GetAllEquips").GetComponent<Button>().onClick.AddListener(GetAllEquips);
		mainhandDrawSpecificCardInputField = base.transform.Find("Bg/List2/MainHandDrawSpecificCard/InputField").GetComponent<InputField>();
		suphandDrawSpecificCardInputField = base.transform.Find("Bg/List2/SupHandDrawSpecificCard/InputField").GetComponent<InputField>();
		skillsInput = base.transform.Find("Bg/List2/ChangeSkills/InputField").GetComponent<InputField>();
		base.transform.Find("Bg/List2/MainHandDrawSpecificCard").GetComponent<Button>().onClick.AddListener(MainHandDrawSpecificCard);
		base.transform.Find("Bg/List2/SupHandDrawSpecificCard").GetComponent<Button>().onClick.AddListener(SupHandDrawSpecificCard);
		base.transform.Find("Bg/List2/ChangeSkills").GetComponent<Button>().onClick.AddListener(ChangeSkills);
		base.transform.Find("Bg/List1/OpenCardShop").GetComponent<Button>().onClick.AddListener(OnClickOpenCardShop);
		base.transform.Find("Bg/List1/OpenAllRoomBlock").GetComponent<Button>().onClick.AddListener(OpenAllRoomBlock);
		specialUsualCardInputField = base.transform.Find("Bg/List1/GetSpecialUsualCard/InputField").GetComponent<InputField>();
		base.transform.Find("Bg/List1/GetSpecialUsualCard").GetComponent<Button>().onClick.AddListener(GetSpecialUsualCard);
		base.transform.Find("Bg/List1/GetAllSpecialUsualCard").GetComponent<Button>().onClick.AddListener(GetAllSpecialUsualCard);
		base.transform.Find("Bg/List1/ShowPlayerSummaryInfo").GetComponent<Button>().onClick.AddListener(ShowPlayerSummaryInfo);
		getEquipByTypeInputField = base.transform.Find("Bg/List1/GetEquipmentByType/InputField").GetComponent<InputField>();
		base.transform.Find("Bg/List1/GetEquipmentByType").GetComponent<Button>().onClick.AddListener(OnClickGetEquipByType);
		base.transform.Find("Bg/List1/UnlockProphesy").GetComponent<Button>().onClick.AddListener(UnlockProphesySystem);
		base.transform.Find("Bg/List1/SetPreEndMapLevel").GetComponent<Button>().onClick.AddListener(SetPreMapLevelInputField);
		preEndMapLevelInputField = base.transform.Find("Bg/List1/SetPreEndMapLevel/InputField").GetComponent<InputField>();
		base.transform.Find("Bg/List1/GetSpaceTime").GetComponent<Button>().onClick.AddListener(GetSpaceTime);
		spacetimeInputField = base.transform.Find("Bg/List1/GetSpaceTime/InputField").GetComponent<InputField>();
		base.transform.Find("Bg/List1/UnlockAllBaseCard").GetComponent<Button>().onClick.AddListener(UnlockAllBaseUsualCard);
		base.transform.Find("Bg/List1/UnlockCardTestBtn").GetComponent<Button>().onClick.AddListener(OnClickUnlockCardTestBtn);
		base.transform.Find("Bg/List1/StartGuide").GetComponent<Button>().onClick.AddListener(StartGuide);
	}

	private void StartGuide()
	{
		GuideSystem.Instance.StartBattleGuide();
	}

	private void OnClickUnlockCardTestBtn()
	{
		Singleton<GameManager>.Instance.CurrentMapLevel = 3;
		Singleton<GameManager>.Instance.CurrentMapLayer = 2;
		Singleton<GameManager>.Instance.AddClearBossHeapID("MHeap_16");
		Singleton<GameManager>.Instance.AddClearBossHeapID("MHeap_34");
		Singleton<GameManager>.Instance.Player.PlayerAttr.Level = 8;
		StartCoroutine(Singleton<GameManager>.Instance.GameOver_IE());
	}

	private void UnlockAllBaseUsualCard()
	{
		SingletonDontDestroy<Game>.Instance.CurrentUserData.UnlockAllUsualCard();
		GameSave.SaveUserData();
	}

	private void GetSpaceTime()
	{
		if (int.TryParse(spacetimeInputField.text, out var result))
		{
			SingletonDontDestroy<Game>.Instance.CurrentUserData.AddCoin(result, isAutoSave: true);
		}
	}

	private void SetPreMapLevelInputField()
	{
		if (int.TryParse(preEndMapLevelInputField.text, out var result) && result >= 0 && result <= 3)
		{
			SingletonDontDestroy<Game>.Instance.CurrentUserData.SetPreEndMapLevel(result);
			GameSave.SaveUserData();
		}
	}

	private void UnlockProphesySystem()
	{
		SingletonDontDestroy<Game>.Instance.CurrentUserData.IsEverFinishGame = true;
		SingletonDontDestroy<Game>.Instance.CurrentUserData.IsUnlockPrephesyCard = true;
		GameSave.SaveUserData();
	}

	private void OnClickGetEquipByType()
	{
		int sourceLimit = getEquipByTypeInputField.text.Str2Int();
		List<string> list = AllRandomInventory.Instance.AllSatisfiedEquipsPlayerNotHave(sourceLimit);
		for (int i = 0; i < list.Count; i++)
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.AddEquipment(list[i]);
		}
	}

	private void ShowPlayerSummaryInfo()
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("GameSummaryUI");
	}

	private void GetAllSpecialUsualCard()
	{
		List<string> list = AllRandomInventory.Instance.AllStatisfiedSpecialUsualCards(int.MaxValue);
		for (int i = 0; i < list.Count; i++)
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.AddSpecialUsualCards(list[i], 1, isNew: true);
		}
	}

	private void GetSpecialUsualCard()
	{
		string text = specialUsualCardInputField.text;
		if (DataManager.Instance.GetSpecialUsualCardAttr(text) != null)
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.AddSpecialUsualCards(text, 1, isNew: true);
		}
	}

	private void OpenAllRoomBlock()
	{
		(SingletonDontDestroy<UIManager>.Instance.GetView("RoomUI") as RoomUI).ForceOpenAllRoomBlocks(isNeedAnim: true);
	}

	private void OnClickOpenCardShop()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("CardShopUI") as CardShopUI).ShowShop(Singleton<GameManager>.Instance.CurrentMapLevel, Singleton<GameManager>.Instance.CurrentMapLayer, Random.Range(int.MinValue, int.MaxValue));
	}

	private void SupHandDrawSpecificCard()
	{
		string text = suphandDrawSpecificCardInputField.text;
		if (FactoryManager.GetUsualCard(text).IsNull())
		{
			Debug.LogError("卡牌的编号输入不正确");
			return;
		}
		int amount = 8 - Singleton<GameManager>.Instance.Player.PlayerBattleInfo.SupHandCardAmount;
		Singleton<GameManager>.Instance.Player.PlayerBattleInfo.PutSpecificCardsIntoSupHand(text, amount);
	}

	private void MainHandDrawSpecificCard()
	{
		string text = mainhandDrawSpecificCardInputField.text;
		if (FactoryManager.GetUsualCard(text).IsNull())
		{
			Debug.LogError("卡牌的编号输入不正确");
			return;
		}
		int amount = 8 - Singleton<GameManager>.Instance.Player.PlayerBattleInfo.MainHandCardAmount;
		Singleton<GameManager>.Instance.Player.PlayerBattleInfo.PutSpecificCardsIntoMainHand(text, amount);
	}

	private void GetAllEquips()
	{
		List<string> list = AllRandomInventory.Instance.AllHaventGetEquips();
		for (int i = 0; i < list.Count; i++)
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.AddEquipment(list[i]);
		}
	}

	private void GetAllUsualCards()
	{
	}

	private void GetAllSkills()
	{
		for (int i = 0; i < 4; i++)
		{
			List<string> list = AllRandomInventory.Instance.AllStatisfiedConditionSkills(i, Singleton<GameManager>.Instance.Player.PlayerOccupation);
			for (int j = 0; j < list.Count; j++)
			{
				Singleton<GameManager>.Instance.Player.PlayerInventory.AddSkill(list[j], isNew: true);
			}
		}
	}

	private void GetGift()
	{
		BaseGift.GiftName value = (BaseGift.GiftName)giftDropDown.value;
		GiftManager.Instace.GetSpecificGift(value, out var gift);
		if (!gift.IsNull())
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.GetGift(gift);
		}
	}

	private void OpenShop()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("ShopUI") as ShopUI).ShowShop(Singleton<GameManager>.Instance.CurrentMapLevel, Singleton<GameManager>.Instance.CurrentMapLayer, Random.Range(int.MinValue, int.MaxValue));
	}

	private void GetFaith()
	{
		Singleton<GameManager>.Instance.Player.PlayerAttr.AddSpecialAttr(10);
	}

	private void GetAp()
	{
		Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryApAmount(10);
	}

	private void AddMonster()
	{
		string monsterCode = "Monster_" + addMonsterInputField.text;
		Singleton<EnemyController>.Instance.AddMonster(monsterCode, actionFlag: false);
	}

	private void SwitchMap()
	{
		Singleton<GameManager>.Instance.SwitchToNextRoom();
	}

	private void TriggerEvent()
	{
		string eventCode = "Event_" + eventInputField.text;
		BaseGameEvent @event = GameEventManager.Instace.GetEvent(eventCode);
		if (!@event.IsNull())
		{
			@event.StartEvent(0, null);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent("开始随机事件：" + @event.GameEventCode);
			}
		}
	}

	private void GetSkill()
	{
		string text = skillInputField.text;
		if (!DataManager.Instance.GetSkillCardAttr(Singleton<GameManager>.Instance.Player.PlayerOccupation, text).IsNull())
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.AddSkill(text, isNew: true);
		}
	}

	private void GetEquipment()
	{
		if (!DataManager.Instance.GetEquipmentCardAttr(equipmentInputField.text).IsNull())
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.AddEquipment(equipmentInputField.text);
		}
	}

	private void GetBuff()
	{
		EntityBase entityBase;
		if (buffIsPlayerToggle.isOn)
		{
			entityBase = Singleton<GameManager>.Instance.Player;
		}
		else
		{
			int index = enemyIndexInputField.text.Str2Int();
			entityBase = Singleton<EnemyController>.Instance.AllEnemies[index];
		}
		int num = buffAmountInputField.text.Str2Int();
		BuffType value = (BuffType)buffTypeDropDown.value;
		if (value == BuffType.Buff_BianCe)
		{
			entityBase.GetBuff(new Buff_BianCe(entityBase, 1, Singleton<GameManager>.Instance.Player.PlayerAttr.AtkDmg));
			return;
		}
		BaseBuff buff = (BaseBuff)Assembly.GetExecutingAssembly().CreateInstance(value.ToString(), ignoreCase: false, BindingFlags.Default, null, new object[2] { entityBase, num }, null, null);
		entityBase.GetBuff(buff);
	}

	private void VarifyPlayerMaxHealth()
	{
		int value = varifyPlayerMaxHealthInputField.text.Str2Int();
		Singleton<GameManager>.Instance.Player.PlayerAttr.VarifyMaxHealth(value);
	}

	private void VarifyPlayerHealth()
	{
		int num = varifyPlayerHealthInputField.text.Str2Int();
		if (num > 0)
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryHealth(num);
		}
		else
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.ReduceHealth(-num);
		}
	}

	private void OnStartBattle()
	{
		string enemyHeapCode = "MHeap_" + battleStartMonsterHeapInputField.text;
		EnemyHeapData specificEnemyHeap = DataManager.Instance.GetSpecificEnemyHeap(enemyHeapCode);
		if (!specificEnemyHeap.IsNull())
		{
			Singleton<GameManager>.Instance.StartBattle(new BattleSystem.NormalBattleHandler(specificEnemyHeap), Vector2.one * 0.5f);
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddSystemReportContent("开始普通怪战斗");
		}
	}

	private void OnVarifyMoney()
	{
		int num = addMoneyInputField.text.Str2Int();
		if (num > 0)
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerEarnMoney(num);
		}
		else
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(-num);
		}
	}

	private void SaveGame()
	{
		GameSave.SaveGame(isNeedCheckGameValue: false);
	}

	private void DrawSupHand()
	{
		int amount = drawSupHandCardInputField.text.Str2Int();
		Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TryDrawSupHandCards(amount);
	}

	private void DrawMainHand()
	{
		int amount = drawMainHandCardInputField.text.Str2Int();
		Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TryDrawMainHandCards(amount);
	}

	private void OnClickKillAllEnemies()
	{
		List<EnemyBase> list = new List<EnemyBase>(Singleton<EnemyController>.Instance.AllEnemies);
		for (int i = 0; i < list.Count; i++)
		{
			list[i].EntityAttr.ReduceHealth(999);
		}
	}

	private void OnClickCheatUIBtn()
	{
		cheatUIPanel.gameObject.SetActive(!isShow);
		isShow = !isShow;
	}

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
		Debug.Log("Destroy Cheat UI...");
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			OnClickCheatUIBtn();
		}
		else if (Input.GetKeyDown(KeyCode.E))
		{
			Transform transform = base.transform.Find("CheatBtn");
			transform.gameObject.SetActive(!transform.gameObject.activeSelf);
		}
		else if (Input.GetKeyDown(KeyCode.W))
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TryDrawMainHandCards(4);
		}
		else if (Input.GetKeyDown(KeyCode.R))
		{
			int amount = 8 - Singleton<GameManager>.Instance.Player.PlayerBattleInfo.MainHandCardAmount;
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.PutSpecificCardsIntoMainHand("BC_P_17", amount);
		}
		else if (Input.GetKeyDown(KeyCode.F))
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TryDrawSupHandCards(4);
		}
	}

	private void ChangeSkills()
	{
		List<string> skillsToChangedTo = skillsInput.text.Split('/').ToList();
		Singleton<GameManager>.Instance.Player.PlayerBattleInfo.ChangeSkillsInBattle_Cheat(skillsToChangedTo);
	}
}
