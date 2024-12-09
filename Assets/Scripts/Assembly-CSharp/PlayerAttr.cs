using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttr : EntityAttr
{
	protected static readonly Dictionary<int, int> ExpData = new Dictionary<int, int>
	{
		{ 2, 30 },
		{ 3, 40 },
		{ 4, 45 },
		{ 5, 50 },
		{ 6, 55 },
		{ 7, 60 },
		{ 8, 65 },
		{ 9, 70 },
		{ 10, 75 },
		{ 11, 80 },
		{ 12, 85 },
		{ 13, 90 },
		{ 14, 95 }
	};

	protected const int OtherNextLevelExp = 100;

	protected bool isLockingSpecialAttr;

	public int Level;

	public int ApAmount;

	public int MemoryAmount;

	public int StoringForceMainCardKeep;

	public int StoringForceSupCardKeep;

	public bool IsWillTakeFlameDevourDmg = true;

	protected Player _player;

	public abstract int SpecialAttr { get; }

	public abstract int DefenceAttr { get; }

	public abstract int BaseDefenceAttr { get; }

	public bool IsLockingSpecialAttr
	{
		set
		{
			isLockingSpecialAttr = value;
		}
	}

	public int BaseApAmount { get; private set; }

	public int DrawCardAmount { get; private set; }

	public HashSet<string> StoringForceSpecificCardKeep { get; }

	public int BaseAtkDmg { get; private set; }

	public int AtkDmg { get; private set; }

	public int CurrentExp { get; private set; }

	public int NextLevelExp { get; private set; }

	public abstract void AddSpecialAttr(int amount);

	public abstract void ComsumeSpecialAttr(int amount);

	public abstract string SpecialAttrShowStr();

	public abstract void AddBaseDefenceAttr(int value);

	public abstract void ReduceBaseDefenceAttra(int value);

	public PlayerAttr(Player player)
	{
		isLockingSpecialAttr = false;
		_player = player;
		StoringForceSpecificCardKeep = new HashSet<string>();
		CurrentExp = 0;
		NextLevelExp = ExpData[2];
		Level = 1;
	}

	public PlayerAttrSaveInfo GetPlayerAttrSaveInfo()
	{
		return new PlayerAttrSaveInfo(Level, CurrentExp, NextLevelExp, MaxHealth, Health);
	}

	public virtual void LoadPlayerAttrSaveInfo(PlayerAttrSaveInfo saveInfo)
	{
		Level = saveInfo.playerLevel;
		CurrentExp = saveInfo.currentExp;
		NextLevelExp = saveInfo.nextLevelExp;
		MaxHealth = saveInfo.playerMaxHealth;
		Health = saveInfo.playerCurrentHealth;
	}

	public void StartBattle()
	{
		base.Armor = base.BaseArmor;
		ApAmount = 0;
		AtkDmg = BaseAtkDmg;
		OnStartBattle();
	}

	protected virtual void OnStartBattle()
	{
	}

	public void EndBattle()
	{
		base.Armor = 0;
		ApAmount = 0;
		OnEndBattle();
	}

	protected virtual void OnEndBattle()
	{
	}

	public void StartRound()
	{
		RecoveryApAmount(BaseApAmount);
	}

	public void AddStoringForceSpecificCardKeep(string cardCode)
	{
		StoringForceSpecificCardKeep.Add(cardCode);
	}

	public void RemoveStroingForceSpecificCardKeep(string cardCode)
	{
		StoringForceSpecificCardKeep.Remove(cardCode);
	}

	public void SetMemoryAmount(int value)
	{
		MemoryAmount = value;
		if (_player.PlayerBattleInfo.CurrentSkillList.Count > MemoryAmount)
		{
			_player.PlayerBattleInfo.ReduceSkillAmount(_player.PlayerBattleInfo.CurrentSkillList.Count - MemoryAmount);
		}
		else if (_player.PlayerBattleInfo.CurrentSkillList.Count < MemoryAmount)
		{
			_player.PlayerBattleInfo.AddSkillAmount(MemoryAmount - _player.PlayerBattleInfo.CurrentSkillList.Count);
		}
		CharacterInfoUI characterInfoUI = SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI;
		if (characterInfoUI != null)
		{
			characterInfoUI.SetMemory(MemoryAmount);
		}
	}

	private void ResetEXP()
	{
		Level = 1;
		CurrentExp = 0;
		NextLevelExp = ExpData[2];
	}

	public int GainExp(int expAmount)
	{
		CurrentExp += expAmount;
		int num = CurrentExp - NextLevelExp;
		int result = 0;
		if (num >= 0)
		{
			CurrentExp = 0;
			result = LevelUp(num);
		}
		EventManager.BroadcastEvent(EventEnum.E_PlayerExpUpdate, null);
		return result;
	}

	private int LevelUp(int remainExp)
	{
		Level++;
		NextLevelExp = ((Level < 13) ? ExpData[Level + 1] : 100);
		EventManager.BroadcastEvent(EventEnum.E_PlayerLevelUp, null);
		_player.AddTimePieces(1);
		return GainExp(remainExp) + 1;
	}

	public void SetExp(int currentExp, int nextLevelExp)
	{
		CurrentExp = currentExp;
		NextLevelExp = nextLevelExp;
	}

	public void SetBaseApAmount(int value)
	{
		BaseApAmount = value;
		CharacterInfoUI characterInfoUI = SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI;
		if (characterInfoUI != null)
		{
			characterInfoUI.SetApAmount(BaseApAmount);
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddSystemReportContent($"添加基础体力：{value}, 当前体力剩余: {BaseApAmount}");
		}
	}

	public void AddBaseApAmount(int value)
	{
		BaseApAmount += value;
		CharacterInfoUI characterInfoUI = SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI;
		if (characterInfoUI != null)
		{
			characterInfoUI.SetApAmount(BaseApAmount);
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddSystemReportContent($"添加基础体力：{value}, 当前体力剩余: {BaseApAmount}");
		}
	}

	public void ReduceBaseApAmount(int value)
	{
		BaseApAmount -= value;
		CharacterInfoUI characterInfoUI = SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI;
		if (characterInfoUI != null)
		{
			characterInfoUI.SetApAmount(BaseApAmount);
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddSystemReportContent($"减少基础体力：{value}, 当前体力剩余: {BaseApAmount}");
		}
	}

	public void RecoveryApAmount(int amount)
	{
		if (amount != 0)
		{
			ApAmount += amount;
			Singleton<BattleEffectManager>.Instance.HandleUsualEffectConfig("Usual_RecoveryAP_EffectConfig", null, null, delegate
			{
				EventManager.BroadcastEvent(EventEnum.E_UpdateApAmount, null);
				BattleUI obj = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
				obj.UpdatePlayerApAmount();
				obj.SetApSprite(isHighlight: true);
				obj.SetNextRoundBtnHighlight(isHighlight: false);
			});
		}
	}

	public void ComsumeApAmount(int amount)
	{
		if (amount == 0)
		{
			return;
		}
		ApAmount -= amount;
		ApAmount = Mathf.Clamp(ApAmount, 0, ApAmount);
		Singleton<BattleEffectManager>.Instance.HandleUsualEffectConfig("Usual_ComsumeAp_EffectConfig", null, null, delegate
		{
			EventManager.BroadcastEvent(EventEnum.E_UpdateApAmount, null);
			BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
			battleUI.UpdatePlayerApAmount();
			if (ApAmount == 0)
			{
				battleUI.SetApSprite(isHighlight: false);
				battleUI.SetNextRoundBtnHighlight(isHighlight: true);
			}
		});
	}

	protected override void OnBaseArmorChanged()
	{
		base.OnBaseArmorChanged();
		CharacterInfoUI characterInfoUI = SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI;
		if (characterInfoUI != null)
		{
			characterInfoUI.SetArmorAmount(base.BaseArmor);
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddSystemReportContent($"当前护甲发生变化，当前基础护甲: {base.BaseArmor}");
		}
	}

	public void SetAtkDmg(int value)
	{
		BaseAtkDmg = value;
		CharacterInfoUI characterInfoUI = SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI;
		if (characterInfoUI != null)
		{
			characterInfoUI.SetWeaponDmg(value);
		}
	}

	public void AddAtkDmgOnBattle(int value)
	{
		AtkDmg += value;
		UpdateAtkDmgToBattleUI();
		EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
	}

	public void ReduceAtkDmgOnBattle(int value)
	{
		AtkDmg -= value;
		UpdateAtkDmgToBattleUI();
		EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
	}

	protected void UpdateAtkDmgToBattleUI()
	{
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).SetPlayerAtkDmg(BaseAtkDmg, AtkDmg - BaseAtkDmg);
	}

	public void SetDrawCardAmount(int amount)
	{
		DrawCardAmount = amount;
		CharacterInfoUI characterInfoUI = SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI;
		if (characterInfoUI != null)
		{
			characterInfoUI.SetDrawCardsAmount(amount);
		}
	}

	protected override void OnHealthChanged(bool isAdd)
	{
		base.OnHealthChanged(isAdd);
		if (Health <= 0)
		{
			Health = 0;
			_player.Dead();
			EventManager.BroadcastEvent(EventEnum.E_UpdatePlayerHealth, null);
		}
		else if (Singleton<GameManager>.Instance.CurrentManagerState is GameManager_BattleState)
		{
			Singleton<BattleEffectManager>.Instance.HandleUsualEffectConfig(isAdd ? "Usual_HealthRecovery_Player_EffectConfig" : "Usual_HealthReduce_Player_EffectConfig", null, null, delegate
			{
				EventManager.BroadcastEvent(EventEnum.E_UpdatePlayerHealth, null);
			});
		}
		else
		{
			EventManager.BroadcastEvent(EventEnum.E_UpdatePlayerHealth, null);
		}
	}

	protected override void OnArmorChanged(bool isAdd)
	{
		base.OnArmorChanged(isAdd);
		Singleton<BattleEffectManager>.Instance.HandleUsualEffectConfig(isAdd ? "Usual_AddArmor_Player_EffectConfig" : "Usual_ReduceArmor_Player_EffectConfig", _player.ArmorTrans, null, delegate
		{
			(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).UpdatePlayerArmor(base.Armor);
			EventManager.BroadcastEvent(EventEnum.E_OnPlayerArmorChanged, null);
			EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
		});
	}
}
