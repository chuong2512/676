using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : EntityBase
{
	private bool isPlayerEverStoringForce;

	private PlayerLogicHandler playerLogicHandler;

	private int timePiecesAmount;

	public override Camp Camp => Camp.Player;

	public override EntityAttr EntityAttr => PlayerAttr;

	public override float BuffHintScale => 0.01f;

	public override bool IsActionOver => true;

	public override string EntityName => "玩家";

	public override Transform EntityTransform
	{
		get
		{
			BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
			if (!(battleUI != null))
			{
				return null;
			}
			return battleUI.PlayerHeadProtraitTrans;
		}
	}

	public override Transform ArmorTrans
	{
		get
		{
			BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
			if (!(battleUI != null))
			{
				return null;
			}
			return battleUI.ArmorTransform;
		}
	}

	public PlayerAttr PlayerAttr { get; private set; }

	public PlayerEquipment PlayerEquipment { get; private set; }

	public PlayerBattleInfo PlayerBattleInfo { get; private set; }

	public PlayerInventory PlayerInventory { get; private set; }

	public PlayerOccupation PlayerOccupation { get; private set; }

	public PlayerEffectContainer PlayerEffectContainer { get; private set; }

	public PlayerLevelUpEffect PlayerLevelUpEffect { get; set; }

	public bool IsPlayerCastingCard { get; set; }

	public bool IsPlayerEverStoringForce => isPlayerEverStoringForce;

	public bool IsCanOpenHidingStage => playerLogicHandler.IsCanOpenHidingStage();

	public List<string> AllProphesyCards { get; private set; }

	public int TimePiecesAmount => timePiecesAmount;

	public Player()
	{
		PlayerEffectContainer = new PlayerEffectContainer();
		AllProphesyCards = null;
	}

	public void AddTimePieces(int amount)
	{
		timePiecesAmount += amount;
		(SingletonDontDestroy<UIManager>.Instance.GetView("RoomUI") as RoomUI).UpdateTimePiecesAmount(timePiecesAmount);
	}

	public void ActivePlayerProphesy(List<string> prophesyCards, bool isLoad)
	{
		if (prophesyCards != null && prophesyCards.Count > 0)
		{
			AllProphesyCards = prophesyCards;
			for (int i = 0; i < prophesyCards.Count; i++)
			{
				FactoryManager.GetProphesyCard(prophesyCards[i]).Active(isLoad);
			}
		}
		else
		{
			AllProphesyCards = null;
		}
	}

	public void ActivePlayerProphesy(string prophesyCard)
	{
		if (AllProphesyCards == null)
		{
			AllProphesyCards = new List<string>();
		}
		FactoryManager.GetProphesyCard(prophesyCard).Active(isLoad: false);
	}

	public void SetPlayerProphesyCards(List<string> prophesyCards)
	{
		AllProphesyCards = prophesyCards;
	}

	public void LoadPlayerInfo(GameSaveInfo saveInfo)
	{
		CreatePlayerBySaveInfo(saveInfo.PlayerSaveInfo);
		PlayerSaveInfo playerSaveInfo = saveInfo.PlayerSaveInfo;
		PlayerOccupation = playerSaveInfo.PlayerOccupation;
		PlayerAttr.LoadPlayerAttrSaveInfo(saveInfo.PlayerSaveInfo.PlayerAttrSaveInfo);
		PlayerEquipment.LoadPlayerEquipmentSaveInfo(saveInfo.PlayerSaveInfo.PlayerEquipmentSaveInfo);
		PlayerBattleInfo.LoadPlayerBattleSaveInfo(saveInfo.PlayerSaveInfo.PlayerBattleSaveInfo);
		PlayerInventory.LoadPlayerInventorySaveInfo(saveInfo.PlayerSaveInfo.PlayerInventorySaveInfo);
		AllRandomInventory.Instance.InitPlayerEquipRandomInventory(EquipmentType.Helmet, PlayerInventory.AllHelmets, PlayerEquipment.Helmet.CardCode);
		AllRandomInventory.Instance.InitPlayerEquipRandomInventory(EquipmentType.Breastplate, PlayerInventory.AllBreasplates, PlayerEquipment.Breastplate.CardCode);
		AllRandomInventory.Instance.InitPlayerEquipRandomInventory(EquipmentType.Glove, PlayerInventory.AllGloves, PlayerEquipment.Glove.CardCode);
		AllRandomInventory.Instance.InitPlayerEquipRandomInventory(EquipmentType.Ornament, PlayerInventory.AllOrnaments, PlayerEquipment.Ornament.CardCode);
		AllRandomInventory.Instance.InitPlayerEquipRandomInventory(EquipmentType.Shoes, PlayerInventory.AllShoes, PlayerEquipment.Shoes.CardCode);
		AllRandomInventory.Instance.InitPlayerEquipRandomInventory(EquipmentType.MainHandWeapon, PlayerInventory.AllMainHands, PlayerEquipment.MainHandWeapon.CardCode);
		AllRandomInventory.Instance.InitPlayerEquipRandomInventory(EquipmentType.SupHandWeapon, PlayerInventory.AllSupHands, PlayerEquipment.SupHandWeapon.CardCode);
		AllRandomInventory.Instance.InitPlayerSkillRandomInventory(PlayerInventory.AllSkills, PlayerBattleInfo.CurrentSkillList);
		AllRandomInventory.Instance.InitPlayerSpecialCardRandomInventory(PlayerInventory.AllSpecialUsualCards, PlayerBattleInfo.AllEquipedMainHandCards, PlayerBattleInfo.AllEquipedSupHandCards);
		timePiecesAmount = saveInfo.PlayerSaveInfo.allTimePiecesAmount;
	}

	public void LoadProphesys(List<string> prophesyCodes)
	{
		ActivePlayerProphesy(prophesyCodes, isLoad: true);
		(SingletonDontDestroy<UIManager>.Instance.ShowView("CharacterInfoUI") as CharacterInfoUI).LoadPlayerProphesyInfo(prophesyCodes, isShow: true);
	}

	private List<string> GetPlayerProphesyCodes()
	{
		return AllProphesyCards;
	}

	public PlayerSaveInfo GetPlayerSaveInfo()
	{
		return new PlayerSaveInfo(PlayerOccupation, PlayerAttr.GetPlayerAttrSaveInfo(), GetPlayerBattleSaveInfo(), GetPlayerEquipmentSaveInfo(), GetPlayerInventorySaveInfo(), GetPlayerProphesyCodes(), timePiecesAmount);
	}

	private PlayerEquipmentSaveInfo GetPlayerEquipmentSaveInfo()
	{
		return new PlayerEquipmentSaveInfo(this);
	}

	private PlayerInventorySaveInfo GetPlayerInventorySaveInfo()
	{
		return new PlayerInventorySaveInfo(this);
	}

	private PlayerBattleSaveInfo GetPlayerBattleSaveInfo()
	{
		return new PlayerBattleSaveInfo(this);
	}

	public void CreatePlayerByOccupation(PlayerOccupation playerOccupation, int presuppositionIndex)
	{
		OccupationInitSetting playerOccupationInitSetting = DataManager.Instance.GetPlayerOccupationInitSetting(playerOccupation);
		CardPresuppositionStruct cardPresuppositionByIndex = SingletonDontDestroy<Game>.Instance.CurrentUserData.GetCardPresuppositionByIndex(playerOccupation, presuppositionIndex);
		PlayerOccupation = playerOccupation;
		switch (PlayerOccupation)
		{
		case PlayerOccupation.Knight:
			CreateDefaultKnightPlayerInfo(playerOccupationInitSetting);
			break;
		case PlayerOccupation.Archer:
			CreateDefaultArcherPlayerInfo(playerOccupationInitSetting);
			break;
		}
		AllProphesyCards = null;
		PlayerLevelUpEffect = new DefaultLevelUpEffect();
		PlayerEquipment.SetPlayerInitEquipment(playerOccupationInitSetting);
		PlayerInventory.SetPlayerInitInventory(playerOccupationInitSetting, SingletonDontDestroy<Game>.Instance.CurrentUserData.AllUnlockedMainHandCards[playerOccupation], SingletonDontDestroy<Game>.Instance.CurrentUserData.AllUnlockedSupHandCards[playerOccupation]);
		PlayerBattleInfo.SetPlayerInitBattleInfo(playerOccupationInitSetting, cardPresuppositionByIndex);
		AllRandomInventory.Instance.InitPlayerRandomInventory(playerOccupation, removeInit: true);
	}

	public void CreatePlayerBySaveInfo(PlayerSaveInfo saveInfo)
	{
		PlayerOccupation = saveInfo.PlayerOccupation;
		OccupationInitSetting playerOccupationInitSetting = DataManager.Instance.GetPlayerOccupationInitSetting(PlayerOccupation);
		switch (PlayerOccupation)
		{
		case PlayerOccupation.Knight:
			CreateDefaultKnightPlayerInfo(playerOccupationInitSetting);
			break;
		case PlayerOccupation.Archer:
			CreateDefaultArcherPlayerInfo(playerOccupationInitSetting);
			break;
		}
		PlayerLevelUpEffect = new DefaultLevelUpEffect();
		AllRandomInventory.Instance.InitPlayerRandomInventory(PlayerOccupation, removeInit: false);
	}

	private void CreateDefaultKnightPlayerInfo(OccupationInitSetting initSetting)
	{
		PlayerAttr = new KnightPlayerAttr(this, initSetting);
		PlayerInventory = new PlayerInventory(this);
		PlayerBattleInfo = new PlayerBattleInfo(this);
		PlayerEquipment = new PlayerEquipment(this);
		PlayerEffectContainer = new PlayerEffectContainer();
		playerLogicHandler = new KnightLogicHandler(this, PlayerAttr);
	}

	private void CreateDefaultArcherPlayerInfo(OccupationInitSetting initSetting)
	{
		PlayerAttr = new ArcherPlayerAttr(this, initSetting);
		PlayerInventory = new PlayerInventory(this);
		PlayerBattleInfo = new PlayerBattleInfo(this);
		PlayerEquipment = new PlayerEquipment(this);
		PlayerEffectContainer = new PlayerEffectContainer();
		playerLogicHandler = new ArcherLogicHandler(this, PlayerAttr);
	}

	public void CreateArcherPlayerInfoForGuide()
	{
		PlayerOccupation = PlayerOccupation.Archer;
		PlayerAttr = new ArcherPlayerAttr(this, 50);
		PlayerInventory = new PlayerInventory(this);
		PlayerBattleInfo = new PlayerBattleInfo(this);
		PlayerEquipment = new PlayerEquipment(this);
		PlayerEffectContainer = new PlayerEffectContainer();
		playerLogicHandler = new ArcherLogicHandler(this, PlayerAttr);
	}

	public void HandleRoomInfoForHiddenStage()
	{
		playerLogicHandler.HandleRoomInfoForHiddenStage();
	}

	public void StartBattle()
	{
		IsPlayerCastingCard = false;
		PlayerAttr.StartBattle();
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).LoadPlayerInfo();
		playerLogicHandler.OnBattleStart();
		SingletonDontDestroy<Game>.Instance.StartCoroutine(StartBattle_IE());
	}

	private IEnumerator StartBattle_IE()
	{
		yield return null;
		PlayerBattleInfo.StartBattle();
	}

	public void EndPlayerRound()
	{
		PlayerAttr.ComsumeApAmount(PlayerAttr.ApAmount);
	}

	public void EndBattle()
	{
		PlayerAttr.EndBattle();
		PlayerBattleInfo.EndBattle();
	}

	public void StartRound()
	{
		BattleUI obj = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		SetPlayerIsEverStoringForce(value: false);
		obj.SetStoringForceBtnHighlight(isHighlight: false);
		obj.SetNextRoundBtnInteractive(isInteractive: true);
		obj.SetNextRoundBtnHighlight(isHighlight: false);
		PlayerAttr.StartRound();
	}

	protected override void OnImmueBuff(BaseBuff buff)
	{
		Singleton<GameHintManager>.Instance.AddFlowingTextForImmueBuffHing(buff.GetBuffImmueHint(), 1f, EntityTransform);
	}

	private void SetPlayerIsEverStoringForce(bool value)
	{
		isPlayerEverStoringForce = value;
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).SetStoringForceBtnInteractive(!value);
	}

	public override int TakeDamage(int dmg, EntityBase caster, bool isAbsDmg)
	{
		int finalDmg;
		int num = TakeDamageProcess(dmg, caster, isAbsDmg, out finalDmg);
		ScreenEffectUI screenEffectUI = SingletonDontDestroy<UIManager>.Instance.ShowView("ScreenEffectUI") as ScreenEffectUI;
		if (finalDmg > 0)
		{
			screenEffectUI.ShowDmgBlink((float)finalDmg / (float)PlayerAttr.MaxHealth, (num > 0) ? TakeDmgBlinkCtrl.BlinkType.blood : TakeDmgBlinkCtrl.BlinkType.shield);
		}
		return num;
	}

	protected override int CheckAllBuffInfluenceToDamage(int finalDmg, bool isAbsDmg, EntityBase caster, ref string takeDmgDes)
	{
		return playerLogicHandler.CheckAllBuffInfluenceToDamage(finalDmg, isAbsDmg, caster, ref takeDmgDes);
	}

	public override void Dead()
	{
		base.Dead();
		EventManager.BroadcastEvent(EventEnum.E_PlayerDead, null);
	}

	public void Revive()
	{
		isDead = false;
		PlayerAttr.RecoveryHealth(PlayerAttr.MaxHealth);
	}

	public int PlayerAtkEnemy(EntityBase enTarget, int dmg, bool isTrueDmg)
	{
		int result = enTarget.TakeDamage(dmg, this, isTrueDmg);
		PlayerEffectContainer.TakeEffect(BattleEffectType.UponPlayerAtkEnemy, new SimpleEffectData
		{
			objData = enTarget,
			intData = dmg,
			boolData = isTrueDmg
		});
		EventManager.BroadcastEvent(EventEnum.E_PlayerAtkEnemy, new SimpleEventData
		{
			objValue = enTarget
		});
		return result;
	}

	public void PlayerUseAUsualCard(UsualCard card, bool isMainHand, Action endAction)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent("玩家使用卡牌：" + card.CardName);
		}
		if (isMainHand)
		{
			PlayerBattleInfo.ComsumeMainHandCards(card.CardCode, isDrop: false);
		}
		else
		{
			PlayerBattleInfo.ComsumeSupHandCards(card.CardCode, isDrop: false);
		}
		card.UsualCardEffect(this, isMainHand, delegate
		{
			TryRemoveDefenceBuff(card.IsWillBreakDefence);
			endAction?.Invoke();
		});
		EventManager.BroadcastEvent(EventEnum.E_PlayerUseUsualCard, new SimpleEventData
		{
			stringValue = card.CardCode
		});
		PlayerUseCardApReduce(card, out var amount);
		PlayerAttr.ComsumeApAmount(card.ApCost - amount);
	}

	public void TryRemoveDefenceBuff(bool isWillBreadkDefence)
	{
		BaseBuff specificBuff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(this, BuffType.Buff_MagicProtect);
		BaseBuff specificBuff2 = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(this, BuffType.Buff_Defence);
		if (isWillBreadkDefence && specificBuff2 != null)
		{
			if (specificBuff.IsNull())
			{
				RemoveBuff(specificBuff2);
			}
			else
			{
				specificBuff.TakeEffect(this);
			}
		}
	}

	public void PlayerUseCardApReduce(UsualCard card, out int amount)
	{
		PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingUsualCardApCostReduce, (BattleEffectData)new SimpleEffectData
		{
			strData = card.CardCode
		}, out amount);
	}

	public void PlayerUseAUsualCardPowUp(UsualCard card, out int IntData)
	{
		PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingUsualCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = card.CardCode
		}, out IntData);
	}

	public void PlayerUseASkillCardPowUp(SkillCard card, EntityBase target, out int IntData)
	{
		PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = card.CardCode,
			objData = target
		}, out IntData);
	}

	public void PlayerUseASkillCard(SkillCard card)
	{
		playerLogicHandler.PlayerUseASkillCard(card);
	}

	public void StoringForce()
	{
		if (!isPlayerEverStoringForce)
		{
			EventManager.BroadcastEvent(EventEnum.E_PlayerStoringForce, null);
			BattleUI obj = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
			if (PlayerAttr.StoringForceMainCardKeep > 0 && PlayerBattleInfo.MainHandCardAmount > 0)
			{
				OnComfirmKeepMainCard(Extension.RandomFromList(randomAmount: Mathf.Min(PlayerAttr.StoringForceMainCardKeep, PlayerBattleInfo.MainHandCardAmount), pool: PlayerBattleInfo.MainHandCards).ToList());
			}
			else
			{
				PlayerBattleInfo.ClearMainHandCards(PlayerAttr.StoringForceSpecificCardKeep);
			}
			if (PlayerAttr.StoringForceSupCardKeep > 0 && PlayerBattleInfo.SupHandCardAmount > 0)
			{
				OnComfirmKeepSupCard(Extension.RandomFromList(randomAmount: Mathf.Min(PlayerAttr.StoringForceSupCardKeep, PlayerBattleInfo.SupHandCardAmount), pool: PlayerBattleInfo.SupHandCards).ToList());
			}
			else
			{
				PlayerBattleInfo.ClearSupHandCards(PlayerAttr.StoringForceSpecificCardKeep);
			}
			PlayerBattleInfo.DrawCardWhenStoringForce();
			playerLogicHandler.OnPlayerStoringForce();
			SetPlayerIsEverStoringForce(value: true);
			obj.SetStoringForceBtnHighlight(isHighlight: false);
		}
	}

	private void OnComfirmKeepMainCard(List<string> allCards)
	{
		PlayerBattleInfo.ClearMainHandCards(allCards, PlayerAttr.StoringForceSpecificCardKeep);
	}

	private void OnComfirmKeepSupCard(List<string> allCards)
	{
		PlayerBattleInfo.ClearSupHandCards(allCards, PlayerAttr.StoringForceSpecificCardKeep);
	}

	public override void AddBuffIcon(BaseBuff buff)
	{
		base.AddBuffIcon(buff);
		if (DataManager.Instance.GetBuffDataByBuffType(buff.BuffType).IsNeedShow)
		{
			((BattleUI)SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI")).AddBuff(buff);
		}
	}

	public override void RemoveBuffIcon(BaseBuff buff)
	{
		base.RemoveBuffIcon(buff);
		((BattleUI)SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI")).RemoveBuff(buff);
	}

	public override void UpdateBuffIcon(BaseBuff buff)
	{
		base.UpdateBuffIcon(buff);
		((BattleUI)SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI")).UpdateBuff(buff);
	}

	public override void GetBuff(BaseBuff buff)
	{
		base.GetBuff(buff);
		EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
	}

	public override void RemoveBuff(BaseBuff buff)
	{
		base.RemoveBuff(buff);
		EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
	}

	protected override void OnEntityGetHurtOnBattle(int healthDmg, int armorDmg, bool isAbsDmg)
	{
		base.OnEntityGetHurtOnBattle(healthDmg, armorDmg, isAbsDmg);
		if (Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
			if (healthDmg > 0)
			{
				Singleton<GameHintManager>.Instance.ShowDamageFlowingText(battleUI.PlayerHeadProtraitTrans, isSetParent: false, Vector3.zero, Vector2.one * 0.7f, healthDmg, 0.005f, isAbsDmg);
			}
			if (armorDmg > 0)
			{
				Singleton<GameHintManager>.Instance.ShowArmorDamageFlowingText(battleUI.ArmorTransform, isSetParent: false, Vector3.zero, armorDmg, 0.005f, isAbsDmg);
			}
		}
	}

	public override void EntityRecoveryHealthOnBattle(int value)
	{
		base.EntityRecoveryHealthOnBattle(value);
		if (Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
			Singleton<GameHintManager>.Instance.ShowHealingFlowingText(battleUI.PlayerHealthTransform, isSetParent: false, Vector3.zero, Vector2.zero, value, 0.005f);
		}
	}

	public List<string> GetPlayerAllSkills()
	{
		List<string> list = new List<string>(PlayerInventory.AllSkills);
		for (int i = 0; i < PlayerBattleInfo.CurrentSkillList.Count; i++)
		{
			if (!PlayerBattleInfo.CurrentSkillList[i].IsNullOrEmpty())
			{
				list.Add(PlayerBattleInfo.CurrentSkillList[i]);
			}
		}
		return list;
	}

	public bool IsPlayerHaveSkill(string skillCode)
	{
		if (!PlayerInventory.IsHaveSkill(skillCode))
		{
			return PlayerBattleInfo.IsEquipedSkill(skillCode);
		}
		return true;
	}

	public bool IsPlayerHaveEquip(string equipCode)
	{
		if (!PlayerEquipment.IsPlayerEquiped(equipCode))
		{
			return PlayerInventory.IsHaveEquipment(equipCode);
		}
		return true;
	}
}
