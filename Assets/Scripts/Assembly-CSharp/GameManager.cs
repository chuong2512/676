using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	private GameManagerState currentManagerState;

	public int CurrentMapLayer;

	public int CurrentMapLevel;

	public float timeUsed;

	private bool isGamePlaying;

	private List<string> allClearBossHeapIDList = new List<string>(4);

	public GameManagerState CurrentManagerState => currentManagerState;

	public Player Player { get; private set; }

	public BattleSystem BattleSystem { get; private set; }

	public List<string> AllClearBossHeapIdList => allClearBossHeapIDList;

	public void AddClearBossHeapID(string heapCode)
	{
		allClearBossHeapIDList.Add(heapCode);
	}

	private void Update()
	{
		currentManagerState?.OnUpdate();
		if (isGamePlaying)
		{
			timeUsed += Time.deltaTime;
		}
	}

	public void StartGame()
	{
		InitGameVariables();
		if (SingletonDontDestroy<Game>.Instance.IsTryLoadSaveData)
		{
			GameSaveInfo gameSaveInfo = GameSave.LoadGame();
			if (gameSaveInfo.IsNull())
			{
				StartNewGame();
			}
			else
			{
				LoadGame(gameSaveInfo);
			}
		}
		else
		{
			StartNewGame();
		}
		EventManager.BroadcastEvent(EventEnum.E_GamePlayStart, null);
	}

	public void InitGameVariables()
	{
		Player = new Player();
		BattleSystem = new BattleSystem();
	}

	private void StartNewGame()
	{
		StepOverNewerGuide();
	}

	private void StartGameTimeCounter()
	{
		isGamePlaying = true;
	}

	private void EndGameTimeCounter()
	{
		isGamePlaying = false;
	}

	public void GamePass()
	{
		if (Player.IsCanOpenHidingStage || SingletonDontDestroy<Game>.Instance.isSatisfiedHiddenStage)
		{
			OpenHidenRoom();
			return;
		}
		EndGameProcess();
		if (SingletonDontDestroy<Game>.Instance.CurrentUserData.IsUnlockPrephesyCard && !SingletonDontDestroy<Game>.Instance.isMustShowProphesyBook)
		{
			ShowTimeAndSpaceUI();
		}
		else
		{
			StartCoroutine(ShowProphesyBook_IE());
		}
		GameEndTryHandleUsuerData(isGamePass: true);
	}

	public void EndGameProcess()
	{
		SingletonDontDestroy<AudioManager>.Instance.PauseMainBGM();
		EndGameTimeCounter();
		GameSave.DeleteOldSaveData();
	}

	private IEnumerator ShowProphesyBook_IE()
	{
		MaskUI maskUi = SingletonDontDestroy<UIManager>.Instance.GetView("MaskUI") as MaskUI;
		maskUi.ActiveRaycast();
		Vector3 specialRoomBlockWorldPos = (SingletonDontDestroy<UIManager>.Instance.GetView("RoomUI") as RoomUI).GetSpecialRoomBlockWorldPos(new Vector2Int(3, 1));
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("预言书弹出");
		GameObject prophesyBook = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("ProphesyBook", "Prefabs", null);
		prophesyBook.transform.position = specialRoomBlockWorldPos;
		yield return new WaitForSeconds(2f);
		maskUi.DeactiveRaycast();
		UnityEngine.Object.Destroy(prophesyBook);
		SingletonDontDestroy<Game>.Instance.CurrentUserData.TryUnlockPlot("Plot_0");
		SingletonDontDestroy<UIManager>.Instance.ShowView("PlotUI", "Plot_0", new Action(ShowTimeAndSpaceUI));
	}

	private void ShowTimeAndSpaceUI()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("TimeAndSpaceUI") as TimeAndSpaceUI).SetFinishAct(delegate
		{
			SingletonDontDestroy<UIManager>.Instance.HideView("BattleUI");
			SingletonDontDestroy<UIManager>.Instance.ShowView("GameSummaryUI", SingletonDontDestroy<Game>.Instance.CurrentUserData.GetNewestRecordData(), new Action(ShowGameSettlementUI), false);
		});
	}

	private void ShowGameSettlementUI()
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("GameSettlementUI");
	}

	private void OpenHidenRoom()
	{
		RoomManager.Instance.GenerateHiddenRoom(Player.PlayerOccupation);
		CurrentMapLevel = 4;
		CurrentMapLayer = 1;
		ShowOpenHiddenRoomVfx();
	}

	public void GameEndTryHandleUsuerData(bool isGamePass)
	{
		if (isGamePass)
		{
			SingletonDontDestroy<Game>.Instance.CurrentUserData.IsUnlockPrephesyCard = true;
		}
		SingletonDontDestroy<Game>.Instance.CurrentUserData.SetPreEndMapLevel(CurrentMapLevel);
		SingletonDontDestroy<Game>.Instance.CurrentUserData.IsEverFinishGame = true;
		SingletonDontDestroy<Game>.Instance.CurrentUserData.AddNewRecord(GetCurrentRecordData(isGamePass));
		GameSave.SaveUserData();
	}

	private RecordData GetCurrentRecordData(bool isGamePass)
	{
		Player player = Player;
		RecordData recordData = new RecordData(player.PlayerOccupation);
		foreach (KeyValuePair<string, int> allEquipedMainHandCard in player.PlayerBattleInfo.AllEquipedMainHandCards)
		{
			recordData.AddMainHandCard(allEquipedMainHandCard.Key, allEquipedMainHandCard.Value);
		}
		foreach (KeyValuePair<string, int> allEquipedSupHandCard in player.PlayerBattleInfo.AllEquipedSupHandCards)
		{
			recordData.AddSupHandCard(allEquipedSupHandCard.Key, allEquipedSupHandCard.Value);
		}
		recordData.allClearBossHeapIDList = new List<string>(allClearBossHeapIDList);
		recordData.Helmet = player.PlayerEquipment.Helmet.CardCode;
		recordData.Ornament = player.PlayerEquipment.Ornament.CardCode;
		recordData.SupHandWeapon = player.PlayerEquipment.SupHandWeapon.CardCode;
		recordData.Breasplate = player.PlayerEquipment.Breastplate.CardCode;
		recordData.MainHandWeapon = player.PlayerEquipment.MainHandWeapon.CardCode;
		recordData.Shoes = player.PlayerEquipment.Shoes.CardCode;
		recordData.Glove = player.PlayerEquipment.Glove.CardCode;
		recordData.isGamePass = isGamePass;
		int num = (recordData.timeUsed = Mathf.FloorToInt(timeUsed));
		recordData.mapLayer = CurrentMapLayer;
		recordData.mapLevel = CurrentMapLevel;
		recordData.MemoryAmount = player.PlayerAttr.MemoryAmount;
		recordData.DrawCardAmount = player.PlayerAttr.DrawCardAmount;
		recordData.ActionPointAmount = player.PlayerAttr.BaseApAmount;
		recordData.ArmorAmount = player.PlayerAttr.BaseArmor;
		recordData.AtkDmg = player.PlayerAttr.AtkDmg;
		recordData.DefenceAttr = player.PlayerAttr.DefenceAttr;
		List<string> currentSkillList = player.PlayerBattleInfo.CurrentSkillList;
		for (int i = 0; i < currentSkillList.Count; i++)
		{
			recordData.AddSkill(currentSkillList[i]);
		}
		return recordData;
	}

	private void OnPlayerDead(EventData data)
	{
		StartCoroutine(GameOver_IE());
	}

	public IEnumerator GameOver_IE()
	{
		MaskUI maskUi = SingletonDontDestroy<UIManager>.Instance.ShowView("MaskUI") as MaskUI;
		maskUi.ActiveRaycast();
		yield return new WaitForSeconds(1f);
		maskUi.DeactiveRaycast();
		EndGameProcess();
		GameEndTryHandleUsuerData(isGamePass: false);
		SingletonDontDestroy<UIManager>.Instance.ShowView("DeathUI", new Action(GameOver_Act1), new Action(GameOver_Act2));
	}

	private void GameOver_Act1()
	{
		EndBattle(isSwitchRoomStat: false);
		BattleSystem.BattleFail();
		Singleton<EnemyController>.Instance.RecycleAllEnemy();
		SingletonDontDestroy<UIManager>.Instance.HideView("SettingUI");
		SingletonDontDestroy<UIManager>.Instance.HideView("PlayerDetailInfoUI");
	}

	private void GameOver_Act2()
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("BattleUI");
		SingletonDontDestroy<UIManager>.Instance.ShowView("GameSummaryUI", SingletonDontDestroy<Game>.Instance.CurrentUserData.GetNewestRecordData(), new Action(ShowGameSettlementUI), false);
	}

	public void StepOverNewerGuide()
	{
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.ForceGetView("BattleUI") as BattleUI;
		if (!battleUI.IsNull())
		{
			battleUI.SetPlayerInfoDetailBtnBgActive(isActive: true);
			BattleSkillSlotCtrl.isLocked = false;
			battleUI.UnlockNextRoundBtn();
			battleUI.UnlockStoringForceBtn();
			battleUI.UnlockPlayerFaithForGuideSystem();
		}
		GameTempData.ClearGameTempData();
		GameEventManager.Instace.InitGameEventManager();
		GiftManager.Instace.InitGiftManager();
		RoomManager.Instance.GenerateRoom(Player.PlayerOccupation);
		CurrentMapLayer = 1;
		CurrentMapLevel = 1;
		allClearBossHeapIDList.Clear();
		Player.CreatePlayerByOccupation(SingletonDontDestroy<Game>.Instance.playerOccupation, SingletonDontDestroy<Game>.Instance.presuppositionIndex);
		if (SingletonDontDestroy<Game>.Instance.isCanJudgeUnlockHiddenStage && SingletonDontDestroy<Game>.Instance.CurrentUserData.IsUnlockPrephesyCard)
		{
			Player.HandleRoomInfoForHiddenStage();
		}
		(SingletonDontDestroy<UIManager>.Instance.ShowView("MaskUI") as MaskUI).ShowMaskAndFade(0f, delegate
		{
			if (SingletonDontDestroy<Game>.Instance.isShowingSwithAnim)
			{
				(SingletonDontDestroy<UIManager>.Instance.ShowView("RoomSwitchUI") as RoomSwitchUI).InitMap();
			}
		}, delegate
		{
			if (SingletonDontDestroy<Game>.Instance.isShowingSwithAnim)
			{
				(SingletonDontDestroy<UIManager>.Instance.ShowView("RoomSwitchUI") as RoomSwitchUI).StartMap(LoadNewRoomInfo);
			}
			else
			{
				LoadNewRoomInfo();
			}
		});
	}

	private void LoadNewRoomInfo()
	{
		if (SingletonDontDestroy<Game>.Instance.isOpenHidenRoom)
		{
			RoomManager.Instance.GenerateHiddenRoom(Player.PlayerOccupation);
			CurrentMapLayer = 1;
			CurrentMapLevel = 4;
		}
		SwitchGameManagerState(new GameManager_RoomState(this));
		(SingletonDontDestroy<UIManager>.Instance.GetView("RoomUI") as RoomUI).LoadRoomInfo();
		timeUsed = 0f;
		StartGameTimeCounter();
		if (SingletonDontDestroy<Game>.Instance.CurrentUserData.IsUnlockPrephesyCard && !SingletonDontDestroy<Game>.Instance.isStepOverProphesyCard)
		{
			SingletonDontDestroy<UIManager>.Instance.ShowView("ProphesyCardUI", SingletonDontDestroy<Game>.Instance.CurrentUserData.PreEndMapLevel, new Action(SaveStartNewGameAndPlayBGM));
		}
		else
		{
			(SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI).LoadPlayerProphesyInfo(null, isShow: false);
			SaveStartNewGameAndPlayBGM();
		}
	}

	private void SaveStartNewGameAndPlayBGM()
	{
		PlayerGameBGMByLevel();
		GameSave.SaveGame();
	}

	private void LoadGame(GameSaveInfo saveInfo)
	{
		GameEventManager.Instace.InitGameEventManager(saveInfo.GameSaveProcessInfo);
		GiftManager.Instace.InitGiftManager();
		CurrentMapLayer = saveInfo.GameSaveProcessInfo.currentMapLayer;
		CurrentMapLevel = saveInfo.GameSaveProcessInfo.currentMapLevel;
		allClearBossHeapIDList = saveInfo.GameSaveProcessInfo.allClearBossHeapIDList;
		RoomManager.Instance.LoadRoom(saveInfo.GameSaveProcessInfo);
		Player.LoadPlayerInfo(saveInfo);
		timeUsed = saveInfo.timeUsed;
		StartGameTimeCounter();
		GameTempData.ShopData = saveInfo.GameSaveProcessInfo.shopData;
		SwitchGameManagerState(new GameManager_RoomState(this));
		(SingletonDontDestroy<UIManager>.Instance.GetView("RoomUI") as RoomUI).LoadRoomInfo(saveInfo.GameSaveProcessInfo);
		Player.LoadProphesys(saveInfo.PlayerSaveInfo.AllProphesyCodes);
		PlayerGameBGMByLevel();
	}

	public void SwitchToNextRoom()
	{
		if (CurrentMapLayer < 3)
		{
			CurrentMapLayer++;
			Player.AddTimePieces(2);
			ShowRoomSwitchUI();
		}
		else if (CurrentMapLevel < 3)
		{
			CurrentMapLevel++;
			CurrentMapLayer = 1;
			SetGameBGMByLevel();
			Player.AddTimePieces(2);
			ShowRoomSwitchUI();
		}
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("进入下一层");
	}

	private void SetGameBGMByLevel()
	{
		switch (CurrentMapLevel)
		{
		case 1:
			SingletonDontDestroy<AudioManager>.Instance.SetMainBGM("探索界面_第一关");
			break;
		case 2:
			SingletonDontDestroy<AudioManager>.Instance.SetMainBGM("探索界面_第二关");
			break;
		case 3:
		case 4:
			SingletonDontDestroy<AudioManager>.Instance.SetMainBGM("探索界面_第三关");
			break;
		}
	}

	private void PlayerGameBGMByLevel()
	{
		switch (CurrentMapLevel)
		{
		case 1:
			SingletonDontDestroy<AudioManager>.Instance.PlayMainBGM("探索界面_第一关");
			break;
		case 2:
			SingletonDontDestroy<AudioManager>.Instance.PlayMainBGM("探索界面_第二关");
			break;
		case 3:
		case 4:
			SingletonDontDestroy<AudioManager>.Instance.PlayMainBGM("探索界面_第三关");
			break;
		}
	}

	private void ShowOpenHiddenRoomVfx()
	{
		StartCoroutine(ShowOpenHiddenRoomVfxCo());
	}

	private IEnumerator ShowOpenHiddenRoomVfxCo()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("进入第四关动画音效");
		RoomUI roomUi = SingletonDontDestroy<UIManager>.Instance.GetView("RoomUI") as RoomUI;
		MaskUI maskUi = SingletonDontDestroy<UIManager>.Instance.ShowView("MaskUI") as MaskUI;
		Vector3 vfxPosition = roomUi.GetSpecialRoomBlockWorldPos(new Vector2Int(3, 3));
		yield return new WaitForSeconds(1.21f);
		maskUi.ActiveRaycast();
		SingletonDontDestroy<UIManager>.Instance.GetView("RoomUI").transform.DOShakePosition(5.5f, 2.5f, 20, 1f, snapping: false, fadeOut: false);
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("effect_general_pass3-4");
		vfxBase.Play();
		vfxBase.transform.position = vfxPosition;
		vfxBase.transform.SetParent(roomUi.transform);
		vfxBase.transform.position = vfxBase.transform.position.WithV3(null, null, -2f);
		yield return new WaitForSeconds(2f);
		SingletonDontDestroy<CameraController>.Instance.ActivatePostEffect(PostEffectType.RadialBlur, new RadialBlueEffectArgs(0.02f, 0.8f, 3, Vector3.one * 0.5f, 1f, 0.5f, 1.2f));
		yield return new WaitForSeconds(0.5f);
		ShowRoomSwitchUI();
	}

	private void ShowRoomSwitchUI()
	{
		if (SingletonDontDestroy<Game>.Instance.isShowingSwithAnim)
		{
			MaskUI maskUi = SingletonDontDestroy<UIManager>.Instance.ShowView("MaskUI") as MaskUI;
			maskUi.ShowMask(delegate
			{
				RoomSwitchUI roomSwitchUi = SingletonDontDestroy<UIManager>.Instance.ShowView("RoomSwitchUI") as RoomSwitchUI;
				maskUi.ShowFade(delegate
				{
					roomSwitchUi.SwitchToNextRoom(CurrentMapLevel, CurrentMapLayer, LoadNextRoomInfo);
				});
			});
		}
		else
		{
			LoadNextRoomInfo();
			(SingletonDontDestroy<UIManager>.Instance.ShowView("MaskUI") as MaskUI).DeactiveRaycast();
		}
	}

	private void LoadNextRoomInfo()
	{
		SingletonDontDestroy<AudioManager>.Instance.RecoveryToMainBGM();
		(SingletonDontDestroy<UIManager>.Instance.GetView("RoomUI") as RoomUI).LoadRoomInfo();
		EventManager.BroadcastEvent((CurrentMapLayer == 1) ? EventEnum.E_SwitchToNextLevel : EventEnum.E_SwitchToNextLayer, null);
		GameSave.SaveGame();
	}

	public void StartBattle(BattleSystem.BattleHandler handler, Vector3 startPos)
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("战斗转场");
		SingletonDontDestroy<CameraController>.Instance.ActivatePostEffect(PostEffectType.RadialBlur, new RadialBlueEffectArgs(0.02f, 1.2f, 3, startPos, 0.5f, 0f, 0.7f));
		MaskUI maskUi = SingletonDontDestroy<UIManager>.Instance.ShowView("MaskUI") as MaskUI;
		maskUi.ShowMask(delegate
		{
			StartBattleProcess(maskUi, handler);
		});
	}

	private void StartBattleProcess(MaskUI maskUi, BattleSystem.BattleHandler handler)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("RoomUI");
		SingletonDontDestroy<UIManager>.Instance.HideView("CharacterInfoUI");
		maskUi.ShowFade(null);
		SwitchGameManagerState(new GameManager_BattleState(this, handler.EnemyHeapData.HeapBattleUISpriteName));
		StartCoroutine(StartBattle_IE(handler));
	}

	private IEnumerator StartBattle_IE(BattleSystem.BattleHandler handler)
	{
		EventManager.RegisterEvent(EventEnum.E_PlayerDead, OnPlayerDead);
		BattleSystem.StartBattle(handler);
		yield return null;
		Player.StartBattle();
		yield return null;
		EventManager.BroadcastEvent(EventEnum.E_OnBattleStart, null);
	}

	public void EndBattle(bool isSwitchRoomStat)
	{
		if (isSwitchRoomStat)
		{
			SwitchGameManagerState(new GameManager_RoomState(this));
		}
		Player.EndBattle();
		EventManager.BroadcastEvent(EventEnum.E_BattleFinallyEnd, null);
		EventManager.UnregisterEvent(EventEnum.E_PlayerDead, OnPlayerDead);
	}

	public void SwitchGameManagerState(GameManagerState state)
	{
		if (state != null)
		{
			if (currentManagerState != null)
			{
				currentManagerState.OnExit();
			}
			currentManagerState = state;
			currentManagerState.OnEnter();
		}
	}
}
