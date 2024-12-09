using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem
{
	public abstract class BattleHandler
	{
		protected BattleSystem battleSystem;

		protected EnemyHeapData _enemyHeapData;

		public EnemyHeapData EnemyHeapData => _enemyHeapData;

		public BattleHandler(EnemyHeapData enemyHeapData)
		{
			_enemyHeapData = enemyHeapData;
		}

		public void StartBattle(BattleSystem battleSystem)
		{
			this.battleSystem = battleSystem;
			List<bool> list = new List<bool>(_enemyHeapData.MonsterInfo.Count);
			for (int i = 0; i < _enemyHeapData.MonsterInfo.Count; i++)
			{
				list.Add(item: false);
			}
			SingletonDontDestroy<AudioManager>.Instance.PlayMainBGM(_enemyHeapData.HeapBGMName, isReplaceMainBgm: false);
			BattleEnvironmentManager.Instance.SetBg(_enemyHeapData.HeapBgHandlerPrefabName);
			Singleton<EnemyController>.Instance.AddMonster(_enemyHeapData.MonsterInfo, list, delegate
			{
				battleSystem.IsBattleInitOver = true;
			});
			OnBattleStart();
		}

		protected abstract void OnBattleStart();

		public abstract void OnBattleEnd();
	}

	public class NormalBattleHandler : BattleHandler
	{
		public NormalBattleHandler(EnemyHeapData enemyHeapData)
			: base(enemyHeapData)
		{
		}

		protected override void OnBattleStart()
		{
		}

		public override void OnBattleEnd()
		{
			Singleton<GameManager>.Instance.StartCoroutine(EndBattle_IE());
		}

		private IEnumerator EndBattle_IE()
		{
			MaskUI maskUi = SingletonDontDestroy<UIManager>.Instance.ShowView("MaskUI") as MaskUI;
			maskUi.ActiveRaycast();
			yield return new WaitForSeconds(1f);
			maskUi.ShowMask(1.5f, delegate
			{
				SingletonDontDestroy<AudioManager>.Instance.PauseMainBGM();
				BattleEnvironmentManager.Instance.HideBg();
				Singleton<GameManager>.Instance.EndBattle(!Singleton<GameManager>.Instance.Player.IsDead);
				maskUi.ShowFade(null);
				EndBattleMask();
			});
		}

		private void EndBattleMask()
		{
			if (!Singleton<GameManager>.Instance.Player.IsDead)
			{
				if (Singleton<GameManager>.Instance.CurrentMapLayer == 3 && Singleton<GameManager>.Instance.CurrentMapLevel == 3)
				{
					Singleton<GameManager>.Instance.GamePass();
					return;
				}
				SpoilsUI spoilsUI = SingletonDontDestroy<UIManager>.Instance.ShowView("SpoilsUI") as SpoilsUI;
				bool isAnythingGet = false;
				HandleBattleEndEquipDrop(spoilsUI, ref isAnythingGet);
				HandleBattleEndCardDrop(spoilsUI, ref isAnythingGet);
				HandleBattleEndCoinGet(spoilsUI);
				HandleBattleEndExpGet(spoilsUI);
				spoilsUI.SetNothingActive(!isAnythingGet);
			}
		}

		private void HandleBattleEndEquipDrop(SpoilsUI spoilsUi, ref bool isAnythingGet)
		{
			float dropRate = _enemyHeapData.DropRate;
			Singleton<GameManager>.Instance.Player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponBattleOverEquipExtraDropRate, (BattleEffectData)null, out float FloatData);
			dropRate = Mathf.Clamp01(dropRate + FloatData);
			List<string> list = null;
			if (_enemyHeapData.DropType > 0 && UnityEngine.Random.value <= dropRate)
			{
				switch (_enemyHeapData.DropType)
				{
				case 1:
					list = AllRandomInventory.Instance.AllSatisfiedEquipsPlayerNotHave(2);
					break;
				case 2:
					list = AllRandomInventory.Instance.AllSatisfiedEquipsPlayerNotHave(8);
					break;
				case 3:
					list = AllRandomInventory.Instance.AllSatisfiedEpicSuitEquips(Singleton<GameManager>.Instance.Player.PlayerOccupation);
					break;
				}
				string text = list[UnityEngine.Random.Range(0, list.Count)];
				spoilsUi.SetSpoilEquipItem(text);
				isAnythingGet = true;
				Singleton<GameManager>.Instance.Player.PlayerInventory.AddEquipment(text);
			}
			else
			{
				spoilsUi.SetSpoilEquipItem(string.Empty);
			}
		}

		private void HandleBattleEndCardDrop(SpoilsUI spoilsUi, ref bool isAnythingGet)
		{
			float cardRate = _enemyHeapData.CardRate;
			Singleton<GameManager>.Instance.Player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponBattleOverCardExtraDropRate, (BattleEffectData)null, out float FloatData);
			cardRate = Mathf.Clamp01(cardRate + FloatData);
			if (UnityEngine.Random.value <= cardRate)
			{
				int sourceLimit = 0;
				switch (_enemyHeapData.DropType)
				{
				case 1:
					sourceLimit = 1;
					break;
				case 2:
					sourceLimit = 4;
					break;
				}
				List<string> list = AllRandomInventory.Instance.AllStatisfiedSpecialUsualCards(sourceLimit);
				string cardCode = list[UnityEngine.Random.Range(0, list.Count)];
				Singleton<GameManager>.Instance.Player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponPlayerGetSpecialCardExtraAmount, (BattleEffectData)null, out int IntData);
				int amount = IntData + 1;
				isAnythingGet = true;
				spoilsUi.SetSpoilCardItem(cardCode, amount);
				Singleton<GameManager>.Instance.Player.PlayerInventory.AddSpecialUsualCards(cardCode, amount, isNew: true);
			}
			else
			{
				spoilsUi.SetSpoilCardItem(string.Empty, 0);
			}
		}

		private void HandleBattleEndCoinGet(SpoilsUI spoilsUi)
		{
			int num = UnityEngine.Random.Range(_enemyHeapData.MinDropCoin, _enemyHeapData.MaxDropCoin + 1);
			Singleton<GameManager>.Instance.Player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponBattleOverExtraCoin, new SimpleEffectData
			{
				intData = num
			}, out var IntData, out var _);
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerEarnMoney(num + IntData);
			spoilsUi.SetMoneyText(num, IntData);
		}

		private void HandleBattleEndExpGet(SpoilsUI spoilsUi)
		{
			float preRate = (float)Singleton<GameManager>.Instance.Player.PlayerAttr.CurrentExp / (float)Singleton<GameManager>.Instance.Player.PlayerAttr.NextLevelExp;
			int heapExp = _enemyHeapData.HeapExp;
			Singleton<GameManager>.Instance.Player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponBattleOverExtraExp, (BattleEffectData)new SimpleEffectData
			{
				intData = heapExp
			}, out int IntData);
			heapExp += IntData;
			int levelUp = Singleton<GameManager>.Instance.Player.PlayerAttr.GainExp(heapExp);
			if (levelUp > 0)
			{
				spoilsUi.SetComfirmAction(delegate
				{
					(SingletonDontDestroy<UIManager>.Instance.ShowView("LevelUpChooseSkillUI") as LevelUpChooseSkillUI).ShowPlayerLevelUpSkillChoose(levelUp);
				});
			}
			else
			{
				GameSave.SaveGame();
			}
			float currentRate = (float)Singleton<GameManager>.Instance.Player.PlayerAttr.CurrentExp / (float)Singleton<GameManager>.Instance.Player.PlayerAttr.NextLevelExp;
			spoilsUi.SetExpGain(preRate, currentRate, levelUp, heapExp);
		}
	}

	public class EventBattleHandler : BattleHandler
	{
		public EventBattleHandler(EnemyHeapData enemyHeapData)
			: base(enemyHeapData)
		{
		}

		protected override void OnBattleStart()
		{
		}

		public override void OnBattleEnd()
		{
			Singleton<GameManager>.Instance.StartCoroutine(EndBattle_IE());
		}

		private IEnumerator EndBattle_IE()
		{
			MaskUI maskUi = SingletonDontDestroy<UIManager>.Instance.ShowView("MaskUI") as MaskUI;
			maskUi.ActiveRaycast();
			yield return new WaitForSeconds(1f);
			maskUi.ShowMask(1.5f, delegate
			{
				SingletonDontDestroy<AudioManager>.Instance.PauseMainBGM();
				BattleEnvironmentManager.Instance.HideBg();
				Singleton<GameManager>.Instance.EndBattle(!Singleton<GameManager>.Instance.Player.IsDead);
				maskUi.ShowFade(delegate
				{
					SingletonDontDestroy<AudioManager>.Instance.RecoveryToMainBGM();
				});
			});
		}
	}

	public class HiddenBossBattleHandler : BattleHandler
	{
		private EnemyBase _enemyBase;

		public HiddenBossBattleHandler(EnemyHeapData enemyHeapData)
			: base(enemyHeapData)
		{
		}

		protected override void OnBattleStart()
		{
			_enemyBase = Singleton<EnemyController>.Instance.GetEnemyBase("Monster_50");
		}

		public override void OnBattleEnd()
		{
			if (Singleton<GameManager>.Instance.Player.IsDead)
			{
				GameFailed();
			}
			else
			{
				GamePass();
			}
		}

		private void GameFailed()
		{
			Singleton<GameManager>.Instance.EndGameProcess();
			Singleton<GameManager>.Instance.GameEndTryHandleUsuerData(isGamePass: false);
			Singleton<GameManager>.Instance.StartCoroutine(EndBattle_IE(isGamePass: false));
		}

		private void GamePass()
		{
			Singleton<GameManager>.Instance.EndGameProcess();
			SingletonDontDestroy<Game>.Instance.CurrentUserData.DefeatEvilDragon();
			Singleton<GameManager>.Instance.GameEndTryHandleUsuerData(isGamePass: true);
			Singleton<GameManager>.Instance.StartCoroutine(EndBattle_IE(isGamePass: true));
		}

		private IEnumerator EndBattle_IE(bool isGamePass)
		{
			MaskUI maskUi = SingletonDontDestroy<UIManager>.Instance.ShowView("MaskUI") as MaskUI;
			maskUi.ActiveRaycast();
			yield return new WaitForSeconds(1f);
			maskUi.ShowMask(1.5f, delegate
			{
				SingletonDontDestroy<AudioManager>.Instance.PauseMainBGM();
				SingletonDontDestroy<UIManager>.Instance.HideView("BossUI");
				BattleEnvironmentManager.Instance.HideBg();
				Singleton<GameManager>.Instance.EndBattle(isSwitchRoomStat: true);
				maskUi.ShowFade(delegate
				{
					EndBattleMask(isGamePass);
				});
			});
		}

		private void EndBattleMask(bool isGamePass)
		{
			(SingletonDontDestroy<UIManager>.Instance.GetView("RoomUI") as RoomUI).HiddenBossBlock.StartGameWinAnim(_enemyBase, ShowGameSummaryUI);
		}

		private void ShowGameSummaryUI()
		{
			SingletonDontDestroy<UIManager>.Instance.HideView("BattleUI");
			SingletonDontDestroy<UIManager>.Instance.ShowView("GameSummaryUI", SingletonDontDestroy<Game>.Instance.CurrentUserData.GetNewestRecordData(), new Action(ShowGameSettlementUI), false);
		}

		private void ShowGameSettlementUI()
		{
			SingletonDontDestroy<UIManager>.Instance.ShowView("GameSettlementUI");
		}
	}

	private BattleHandler currentBattleHandler;

	public bool IsInBattle { get; private set; }

	public bool IsBattleInitOver { get; private set; }

	public int BattleRoundAmount { get; private set; }

	public Round BattleRound { get; private set; }

	public BuffSystem BuffSystem { get; }

	public EnemyBase EnemyPlayerChoose { get; private set; }

	public HashSet<EntityBase> allEntitiesInAction { get; }

	public BattleSystem()
	{
		BuffSystem = new BuffSystem();
		allEntitiesInAction = new HashSet<EntityBase>();
	}

	public void StartBattle(BattleHandler handler)
	{
		Singleton<EnemyController>.Instance.RecycleAllEnemy();
		BuffSystem.ClearAllBuff();
		BattleRoundAmount = 0;
		IsInBattle = true;
		IsBattleInitOver = false;
		StartPlayerRound();
		EventManager.RegisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
		currentBattleHandler = handler;
		currentBattleHandler.StartBattle(this);
	}

	public void StartGuideBattle()
	{
		currentBattleHandler = null;
		BattleRoundAmount = 0;
		IsInBattle = true;
		IsBattleInitOver = false;
		StartPlayerRound();
		BattleEnvironmentManager.Instance.SetBg("Bg_1");
		EventManager.RegisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
		SingletonDontDestroy<AudioManager>.Instance.PlayMainBGM("卡牌_战斗界面_普通战斗音乐");
	}

	public void EndGuideBattle()
	{
		IsInBattle = false;
		EventManager.BroadcastEvent(EventEnum.E_OnBattleEnd, null);
		SetEnemyPlayerChoose(null);
		BuffSystem.OnBattleEnd();
		EventManager.UnregisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
		Singleton<GameManager>.Instance.Player.EndBattle();
	}

	public void EndBattle()
	{
		IsInBattle = false;
		EventManager.BroadcastEvent(EventEnum.E_OnBattleEnd, null);
		SetEnemyPlayerChoose(null);
		BuffSystem.OnBattleEnd();
		EventManager.UnregisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
		currentBattleHandler.OnBattleEnd();
	}

	public void BattleFail()
	{
		IsInBattle = false;
		EventManager.BroadcastEvent(EventEnum.E_OnBattleEnd, null);
		SetEnemyPlayerChoose(null);
		BuffSystem.OnBattleEnd();
		EventManager.UnregisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
		BattleEnvironmentManager.Instance.HideBg();
	}

	private void OnEnemyDead(EventData data)
	{
		SimpleEventData simpleEventData;
		if ((simpleEventData = data as SimpleEventData) != null && simpleEventData.objValue == EnemyPlayerChoose)
		{
			SetEnemyPlayerChoose(null);
		}
	}

	private void StartPlayerRound()
	{
		if (IsInBattle)
		{
			BattleRound = Round.Switch;
			(SingletonDontDestroy<UIManager>.Instance.ShowView("SwitchRoundUI") as SwitchRoundUI).SwitchToPlayerRound(PlayerRound, ActivePlayerRoundEquip, delegate
			{
				OnTurnSwitch(EventEnum.E_PlayerRound);
			});
		}
	}

	private void ActivePlayerRoundEquip()
	{
		EventManager.BroadcastEvent(EventEnum.E_PlayerEquipRound, null);
	}

	private void PlayerRound()
	{
		SetEnemyPlayerChoose(null);
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddSystemReportContent("<color=#cae283ff>---------开始玩家回合---------</color>");
		}
		BattleRoundAmount++;
		BattleRound = Round.PlayerRound;
		Singleton<GameManager>.Instance.Player.StartRound();
		if (BattleRoundAmount == 1)
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.OnEffectAllGifts();
		}
	}

	public void EndPlayerRound()
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddSystemReportContent("<color=#cae283ff>---------结束玩家回合---------</color>");
		}
		EventManager.BroadcastEvent(EventEnum.E_PlayerEndRound, null);
		Singleton<GameManager>.Instance.Player.EndPlayerRound();
		StartEnemyRound();
	}

	private void StartEnemyRound()
	{
		if (IsInBattle)
		{
			BattleRound = Round.Switch;
			(SingletonDontDestroy<UIManager>.Instance.ShowView("SwitchRoundUI") as SwitchRoundUI).SwitchToEnemyRound(EnemyRound, ActiveEnemyRoundEquip, delegate
			{
				OnTurnSwitch(EventEnum.E_EnemyRound);
			});
		}
	}

	private void ActiveEnemyRoundEquip()
	{
		EventManager.BroadcastEvent(EventEnum.E_EnemyEquipRound, null);
	}

	private void EnemyRound()
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddSystemReportContent("<color=#e39e84ff>---------开始怪物回合---------</color>");
		}
		BattleRound = Round.EnemyRound;
		EnemyBase nextEnemy = Singleton<EnemyController>.Instance.GetNextEnemy();
		if (nextEnemy != null)
		{
			nextEnemy.StartBattleAction();
		}
		else
		{
			EndBattle();
		}
	}

	public void EndEnemyRound()
	{
		EnemyBase nextEnemy = Singleton<EnemyController>.Instance.GetNextEnemy();
		if (nextEnemy != null)
		{
			nextEnemy.StartBattleAction();
			return;
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddSystemReportContent("<color=#e39e84ff>---------结束怪物回合---------</color>");
		}
		EventManager.BroadcastEvent(EventEnum.E_EnemyEndRound, null);
		StartPlayerRound();
		Singleton<EnemyController>.Instance.ResetEnemyFlags();
	}

	private void OnTurnSwitch(EventEnum roundEnum)
	{
		if (BattleRoundAmount != 0)
		{
			BuffSystem.OnTurnRound();
		}
		EventManager.BroadcastEvent(EventEnum.E_RoundTurn, null);
		EventManager.BroadcastEvent(roundEnum, null);
	}

	public void SetEnemyPlayerChoose(EnemyBase enemy)
	{
		EnemyPlayerChoose = enemy;
	}
}
