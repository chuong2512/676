using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideSystem
{
	private class BattleGuide
	{
		private GuideStep currentStep;

		public void StartGuide()
		{
			(SingletonDontDestroy<UIManager>.Instance.ShowView("MaskUI") as MaskUI).ShowMask(LoadGameScene);
		}

		private void LoadGameScene()
		{
			SingletonDontDestroy<SceneManager>.Instance.LoadScene(2, OnSceneLoaded);
		}

		private void OnSceneLoaded(int level)
		{
			SingletonDontDestroy<UIManager>.Instance.HideAllShowingView();
			(SingletonDontDestroy<UIManager>.Instance.ShowView("MaskUI") as MaskUI).ShowFade(StartFirstGuide);
		}

		private void StartFirstGuide()
		{
			InitSomeVariable();
			SwitchToNextStep(new GuideStep_1());
		}

		private void InitSomeVariable()
		{
			Singleton<GameManager>.Instance.InitGameVariables();
			Player player = Singleton<GameManager>.Instance.Player;
			player.CreateArcherPlayerInfoForGuide();
			player.PlayerAttr.SetAtkDmg(4);
			((ArcherPlayerAttr)player.PlayerAttr).SetMaxArrow(10);
			player.PlayerAttr.SetDrawCardAmount(0);
			player.PlayerAttr.SetBaseApAmount(3);
			player.PlayerAttr.SetMemoryAmount(1);
			player.PlayerBattleInfo.AllEquipedMainHandCards = new Dictionary<string, int>(1) { { "BC_M_6", 8 } };
			player.PlayerBattleInfo.AllEquipedSupHandCards = new Dictionary<string, int>(1) { { "BC_O_6", 8 } };
			player.PlayerBattleInfo.CurrentSkillList = new List<string>(1) { "S_A_16" };
			SingletonDontDestroy<UIManager>.Instance.ShowView("BattleUI", "战斗界面底框1");
			Singleton<GameManager>.Instance.BattleSystem.StartGuideBattle();
			player.StartBattle();
		}

		public void SwitchToNextStep(GuideStep step)
		{
			currentStep?.OnEndGuideStep();
			currentStep = step;
			currentStep.OnStartGuideStep();
		}

		public void ForceEndGuide()
		{
			currentStep.ForceEndGuideStep();
		}
	}

	private abstract class GuideStep
	{
		protected Guide currentGuide;

		public abstract void OnStartGuideStep();

		public abstract void OnEndGuideStep();

		public abstract void ForceEndGuideStep();

		public void SwitchToNexGuide(Guide guide)
		{
			currentGuide?.OnEndGuide();
			currentGuide = guide;
			currentGuide.OnStartGuide();
		}
	}

	private class GuideStep_1 : GuideStep
	{
		public override void OnStartGuideStep()
		{
			BattleUI obj = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
			obj.LockNextRoundBtn();
			obj.LockSkills();
			obj.LockSotringForceBtn();
			obj.SetPlayerInfoDetailBtnBgActive(isActive: false);
			Singleton<EnemyController>.Instance.AddMonster("Monster_998", actionFlag: false);
			EventManager.RegisterEvent(EventEnum.E_PlayerDead, OnPlayerDead);
			SingletonDontDestroy<UIManager>.Instance.ShowView("GuideTipsUI", null, new List<string>(2) { "Code_1_1", "Code_1_2", "Code_1_3" }, new Action(StartGuide));
		}

		private void StartGuide()
		{
			SwitchToNexGuide(new Guide_01(this));
		}

		private void OnPlayerDead(EventData data)
		{
			ForceEndGuideStep();
			_instance.EndBattleGuide(isPass: false);
		}

		public override void OnEndGuideStep()
		{
			currentGuide?.OnEndGuide();
			EventManager.UnregisterEvent(EventEnum.E_PlayerDead, OnPlayerDead);
		}

		public override void ForceEndGuideStep()
		{
			currentGuide.ForceEndGuide();
			EventManager.UnregisterEvent(EventEnum.E_PlayerDead, OnPlayerDead);
		}
	}

	private class GuideStep_2 : GuideStep
	{
		public override void OnStartGuideStep()
		{
			Player player = Singleton<GameManager>.Instance.Player;
			player.PlayerAttr.RecoveryHealth(player.PlayerAttr.MaxHealth - player.PlayerAttr.Health);
			player.PlayerBattleInfo.ClearMainHandCards(null);
			player.PlayerBattleInfo.ClearSupHandCards(null);
			player.PlayerBattleInfo.ClearMainHandAllPiles();
			player.PlayerBattleInfo.ClearSupHandAllPiles();
			player.PlayerBattleInfo.AllEquipedMainHandCards = new Dictionary<string, int> { { "BC_M_6", 8 } };
			player.PlayerAttr.SetDrawCardAmount(4);
			player.PlayerBattleInfo.ShuffleInitMainCards();
			player.PlayerBattleInfo.TryDrawMainHandCards(4);
			player.PlayerAttr.RecoveryApAmount(player.PlayerAttr.BaseApAmount - player.PlayerAttr.ApAmount);
			SwitchToNexGuide(new Guide_02(this));
		}

		public override void OnEndGuideStep()
		{
			currentGuide?.OnEndGuide();
		}

		public override void ForceEndGuideStep()
		{
			currentGuide.ForceEndGuide();
		}
	}

	private class GuideStep_3 : GuideStep
	{
		public override void OnStartGuideStep()
		{
			Player player = Singleton<GameManager>.Instance.Player;
			player.PlayerBattleInfo.ClearMainHandCards(null);
			player.PlayerBattleInfo.ClearSupHandCards(null);
			player.PlayerBattleInfo.ClearMainHandAllPiles();
			player.PlayerBattleInfo.ClearSupHandAllPiles();
			player.PlayerBattleInfo.AllEquipedMainHandCards = new Dictionary<string, int>
			{
				{ "BC_M_6", 6 },
				{ "BC_M_7", 3 }
			};
			player.PlayerBattleInfo.AllEquipedSupHandCards = new Dictionary<string, int>
			{
				{ "BC_O_6", 6 },
				{ "BC_O_7", 3 }
			};
			player.PlayerAttr.SetDrawCardAmount(4);
			player.PlayerBattleInfo.ShuffleInitMainCards();
			player.PlayerBattleInfo.ShuffleInitSupCards();
			player.PlayerBattleInfo.TryDrawMainHandCards(4);
			player.PlayerBattleInfo.TryDrawSupHandCards(4);
			player.PlayerAttr.RecoveryApAmount(player.PlayerAttr.BaseApAmount - player.PlayerAttr.ApAmount);
			SwitchToNexGuide(new Guide_04(this));
			EventManager.RegisterEvent(EventEnum.E_PlayerDead, OnPlayerDead);
		}

		private void OnPlayerDead(EventData data)
		{
			_instance.EndBattleGuide(isPass: false);
			EventManager.UnregisterEvent(EventEnum.E_PlayerDead, OnPlayerDead);
		}

		public override void OnEndGuideStep()
		{
			currentGuide?.OnEndGuide();
			EventManager.UnregisterEvent(EventEnum.E_PlayerDead, OnPlayerDead);
		}

		public override void ForceEndGuideStep()
		{
			currentGuide.ForceEndGuide();
		}
	}

	private abstract class Guide
	{
		protected GuideStep parentStep;

		protected Guide(GuideStep parentStep)
		{
			this.parentStep = parentStep;
		}

		public abstract void OnStartGuide();

		public abstract void OnEndGuide();

		public abstract void ForceEndGuide();
	}

	private class Guide_01 : Guide
	{
		private const string Guide_BubbleDialogKey_01 = "Guide_BubbleDialogKey_01";

		private bool isRegistHurtEvent;

		private bool isRegistApUpdateEvent;

		private bool isRegistPlayerRoundEvent;

		private bool isRegistMissionCompleteEvent;

		public Guide_01(GuideStep parentStep)
			: base(parentStep)
		{
			isRegistHurtEvent = false;
			isRegistApUpdateEvent = false;
			isRegistPlayerRoundEvent = false;
			isRegistMissionCompleteEvent = false;
		}

		public override void OnStartGuide()
		{
			OnGuideTipsOver();
			EventManager.RegisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_EntityGetHurt, OnPlayerGetHurt);
			EventManager.RegisterEvent(EventEnum.E_UpdateApAmount, OnApAmountUpdate);
			EventManager.RegisterEvent(EventEnum.E_PlayerRound, OnPlayerRound);
			isRegistHurtEvent = true;
			isRegistApUpdateEvent = true;
			isRegistPlayerRoundEvent = true;
			Singleton<GameManager>.Instance.Player.PlayerAttr.SetDrawCardAmount(3);
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TryDrawMainHandCards(3);
		}

		private void OnGuideTipsOver()
		{
			MissionSystem.Instance.AddNewMission(new Mission_1());
			EventManager.RegisterEvent(EventEnum.E_MissionComplete, OnMissionComplete);
			isRegistMissionCompleteEvent = true;
			(SingletonDontDestroy<UIManager>.Instance.GetView("MissionUI") as MissionUI).AddGuideTips(new List<string>(2) { "Code_2_1", "Code_2_2" });
		}

		private void OnMissionComplete(EventData data)
		{
			SimpleEventData simpleEventData;
			if ((simpleEventData = data as SimpleEventData) != null && simpleEventData.intValue == 1)
			{
				EventManager.UnregisterEvent(EventEnum.E_MissionComplete, OnMissionComplete);
				isRegistMissionCompleteEvent = false;
				_instance.SwitchToNextStep(new GuideStep_2());
			}
		}

		private void OnPlayerGetHurt(EventData data)
		{
			BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
			(SingletonDontDestroy<UIManager>.Instance.ShowView("BubbleTalkUI") as BubbleTalkUI).ShowPLeftBubble("Guide_BubbleDialogKey_01".LocalizeText(), battleUI.bubbleHintPoint.position);
		}

		private void OnApAmountUpdate(EventData data)
		{
			if (Singleton<GameManager>.Instance.Player.PlayerAttr.ApAmount == 0)
			{
				(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).UnlockNextRoundBtn();
				EventManager.UnregisterEvent(EventEnum.E_UpdateApAmount, OnApAmountUpdate);
				isRegistApUpdateEvent = false;
			}
		}

		private void OnPlayerRound(EventData data)
		{
			if (Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount == 1)
			{
				List<string> list = new List<string>(2) { "Code_2_2", "Code_3_1" };
				MissionUI missionUI = SingletonDontDestroy<UIManager>.Instance.GetView("MissionUI") as MissionUI;
				missionUI.AddGuideTips(list);
				SingletonDontDestroy<UIManager>.Instance.ShowView("GuideTipsUI", missionUI.GuideTipsBtnTrans, list, null);
				(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).UnlockStoringForceBtn();
				EventManager.UnregisterEvent(EventEnum.E_PlayerRound, OnPlayerRound);
				isRegistPlayerRoundEvent = false;
			}
		}

		public override void OnEndGuide()
		{
			EventManager.UnregisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_EntityGetHurt, OnPlayerGetHurt);
		}

		public override void ForceEndGuide()
		{
			if (isRegistMissionCompleteEvent)
			{
				EventManager.UnregisterEvent(EventEnum.E_MissionComplete, OnMissionComplete);
			}
			if (isRegistHurtEvent)
			{
				EventManager.UnregisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_EntityGetHurt, OnPlayerGetHurt);
			}
			if (isRegistApUpdateEvent)
			{
				EventManager.UnregisterEvent(EventEnum.E_UpdateApAmount, OnApAmountUpdate);
			}
			if (isRegistPlayerRoundEvent)
			{
				EventManager.UnregisterEvent(EventEnum.E_PlayerRound, OnPlayerRound);
			}
		}
	}

	private class Guide_02 : Guide
	{
		private const string Guide_BubbleDialogKey_02 = "Guide_BubbleDialogKey_02";

		private const string Guide_BubbleDialogKey_03 = "Guide_BubbleDialogKey_03";

		private bool isRegistMissionCompleteEvent;

		private bool isRegistPlayerUseUsualCardEvent;

		public Guide_02(GuideStep parentStep)
			: base(parentStep)
		{
			isRegistMissionCompleteEvent = false;
			isRegistPlayerUseUsualCardEvent = false;
		}

		public override void OnStartGuide()
		{
			BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
			battleUI.BlockUIRaycast();
			(SingletonDontDestroy<UIManager>.Instance.ShowView("BubbleTalkUI") as BubbleTalkUI).ShowPLeftBubble("Guide_BubbleDialogKey_02".LocalizeText(), battleUI.bubbleHintPoint.position);
			SingletonDontDestroy<Game>.Instance.StartCoroutine(WaitBubbleTalkOver_IE());
		}

		private IEnumerator WaitBubbleTalkOver_IE()
		{
			yield return new WaitForSeconds(3f);
			OnBubbleTalkOver();
			yield return new WaitForSeconds(0.5f);
			MissionUI missionUI = SingletonDontDestroy<UIManager>.Instance.GetView("MissionUI") as MissionUI;
			missionUI.AddGuideTips(new List<string>(1) { "Code_5_1" });
			SingletonDontDestroy<UIManager>.Instance.ShowView("GuideTipsUI", missionUI.GuideTipsBtnTrans, new List<string>(1) { "Code_4_1" }, new Action(OnGuideTipsOver));
		}

		private void OnBubbleTalkOver()
		{
			MissionSystem.Instance.AddNewMission(new Mission_2());
			(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).UnlockSkills();
			EventManager.RegisterEvent(EventEnum.E_MissionComplete, OnMissionComplete);
			EventManager.RegisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseUsualCard);
			isRegistMissionCompleteEvent = true;
			isRegistPlayerUseUsualCardEvent = true;
		}

		private void OnGuideTipsOver()
		{
			Singleton<EnemyController>.Instance.SummorMonster(new List<string>(1) { "Monster_996" }, new List<bool>(1) { false }, null);
			(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).RecieveUIRaycast();
		}

		private void OnMissionComplete(EventData data)
		{
			SimpleEventData simpleEventData;
			if ((simpleEventData = data as SimpleEventData) != null && simpleEventData.intValue == 2)
			{
				EventManager.UnregisterEvent(EventEnum.E_MissionComplete, OnMissionComplete);
				isRegistMissionCompleteEvent = false;
				parentStep.SwitchToNexGuide(new Guide_03(parentStep));
			}
		}

		private void OnPlayerUseUsualCard(EventData data)
		{
			BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
			(SingletonDontDestroy<UIManager>.Instance.ShowView("BubbleTalkUI") as BubbleTalkUI).ShowPLeftBubble("Guide_BubbleDialogKey_03".LocalizeText(), battleUI.bubbleHintPoint.position);
			Singleton<GameManager>.Instance.Player.PlayerAttr.AddSpecialAttr(1);
		}

		public override void OnEndGuide()
		{
			EventManager.UnregisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseUsualCard);
		}

		public override void ForceEndGuide()
		{
			if (isRegistMissionCompleteEvent)
			{
				EventManager.UnregisterEvent(EventEnum.E_MissionComplete, OnMissionComplete);
			}
			if (isRegistPlayerUseUsualCardEvent)
			{
				EventManager.UnregisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseUsualCard);
			}
		}
	}

	private class Guide_03 : Guide
	{
		private bool isRegistMissionCompleteEvent;

		public Guide_03(GuideStep parentStep)
			: base(parentStep)
		{
			isRegistMissionCompleteEvent = false;
		}

		public override void OnStartGuide()
		{
			MissionSystem.Instance.AddNewMission(new Mission_3());
			EventManager.RegisterEvent(EventEnum.E_MissionComplete, OnMissionComplete);
			isRegistMissionCompleteEvent = true;
		}

		private void OnMissionComplete(EventData data)
		{
			SimpleEventData simpleEventData;
			if ((simpleEventData = data as SimpleEventData) != null && simpleEventData.intValue == 3)
			{
				EventManager.UnregisterEvent(EventEnum.E_MissionComplete, OnMissionComplete);
				isRegistMissionCompleteEvent = false;
				_instance.SwitchToNextStep(new GuideStep_3());
			}
		}

		public override void OnEndGuide()
		{
		}

		public override void ForceEndGuide()
		{
			if (isRegistMissionCompleteEvent)
			{
				EventManager.UnregisterEvent(EventEnum.E_MissionComplete, OnMissionComplete);
			}
		}
	}

	private class Guide_04 : Guide
	{
		private const string Guide_BubbleDialogKey_04 = "GUIDE_BUBBLEDIALOGKEY_04";

		public Guide_04(GuideStep parentStep)
			: base(parentStep)
		{
		}

		public override void OnStartGuide()
		{
			BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
			battleUI.BlockUIRaycast();
			(SingletonDontDestroy<UIManager>.Instance.ShowView("BubbleTalkUI") as BubbleTalkUI).ShowPLeftBubble("GUIDE_BUBBLEDIALOGKEY_04".LocalizeText(), battleUI.bubbleHintPoint.position);
			SingletonDontDestroy<Game>.Instance.StartCoroutine(WaitBubbleTalkOver_IE());
		}

		private IEnumerator WaitBubbleTalkOver_IE()
		{
			yield return new WaitForSeconds(3f);
			BubbleTalkOver();
			Singleton<EnemyController>.Instance.SummorMonster(new List<string>(2) { "Monster_995", "Monster_995" }, new List<bool>(2) { false, false }, null);
		}

		private void BubbleTalkOver()
		{
			(SingletonDontDestroy<UIManager>.Instance.GetView("MissionUI") as MissionUI).AddGuideTips(new List<string>(1) { "Code_6_1" });
			MissionSystem.Instance.AddNewMission(new Mission_4());
			EventManager.RegisterEvent(EventEnum.E_MissionComplete, OnMissionComplete);
			(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).RecieveUIRaycast();
		}

		private void OnMissionComplete(EventData data)
		{
			EventManager.UnregisterEvent(EventEnum.E_MissionComplete, OnMissionComplete);
			_instance.EndBattleGuide(isPass: true);
		}

		public override void OnEndGuide()
		{
		}

		public override void ForceEndGuide()
		{
			EventManager.UnregisterEvent(EventEnum.E_MissionComplete, OnMissionComplete);
		}
	}

	private class Guide_05 : Guide
	{
		public Guide_05()
			: base(null)
		{
		}

		public Guide_05(GuideStep parentStep)
			: base(parentStep)
		{
		}

		public override void OnStartGuide()
		{
			EventManager.RegisterPermanentEvent(EventEnum.E_LoadRoomInfo, OnLoadRoomInfo);
		}

		private void OnLoadRoomInfo(EventData data)
		{
			SingletonDontDestroy<UIManager>.Instance.ShowView("GuideTipsUI", null, new List<string>(2) { "Code_8_1", "Code_8_2" }, null);
			EventManager.UnregisterPermanentEvent(EventEnum.E_LoadRoomInfo, OnLoadRoomInfo);
			_instance.EndAGameGuide(this);
		}

		public override void OnEndGuide()
		{
		}

		public override void ForceEndGuide()
		{
			EventManager.UnregisterPermanentEvent(EventEnum.E_LoadRoomInfo, OnLoadRoomInfo);
		}
	}

	private class Guide_06 : Guide
	{
		public Guide_06()
			: base(null)
		{
		}

		public Guide_06(GuideStep parentStep)
			: base(parentStep)
		{
		}

		public override void OnStartGuide()
		{
			EventManager.RegisterPermanentEvent(EventEnum.E_ShowUIView, OnShowGameSettlementUI);
		}

		private void OnShowGameSettlementUI(EventData data)
		{
			SimpleEventData simpleEventData;
			if ((simpleEventData = data as SimpleEventData) != null && simpleEventData.stringValue == "GameSettlementUI")
			{
				SingletonDontDestroy<UIManager>.Instance.ShowView("GuideTipsUI", null, new List<string>(1) { "Code_10_1" }, null);
				EventManager.UnregisterPermanentEvent(EventEnum.E_ShowUIView, OnShowGameSettlementUI);
				_instance.EndAGameGuide(this);
			}
		}

		public override void OnEndGuide()
		{
		}

		public override void ForceEndGuide()
		{
			EventManager.UnregisterPermanentEvent(EventEnum.E_ShowUIView, OnShowGameSettlementUI);
		}
	}

	private class Guide_07 : Guide
	{
		public Guide_07()
			: base(null)
		{
		}

		public Guide_07(GuideStep parentStep)
			: base(parentStep)
		{
		}

		public override void OnStartGuide()
		{
			EventManager.RegisterPermanentEvent(EventEnum.E_ShowUIView, OnShowMenuUI);
		}

		private void OnShowMenuUI(EventData data)
		{
			SimpleEventData simpleEventData;
			if ((simpleEventData = data as SimpleEventData) != null && simpleEventData.stringValue == "PurchasedItemUI" && SingletonDontDestroy<Game>.Instance.CurrentUserData.IsEverFinishGame)
			{
				SingletonDontDestroy<UIManager>.Instance.ShowView("GuideTipsUI", null, new List<string>(1) { "Code_11_1" }, null);
				EventManager.UnregisterPermanentEvent(EventEnum.E_ShowUIView, OnShowMenuUI);
				_instance.EndAGameGuide(this);
			}
		}

		public override void OnEndGuide()
		{
		}

		public override void ForceEndGuide()
		{
			EventManager.UnregisterPermanentEvent(EventEnum.E_ShowUIView, OnShowMenuUI);
		}
	}

	private class Guide_08 : Guide
	{
		public Guide_08()
			: base(null)
		{
		}

		public Guide_08(GuideStep parentStep)
			: base(parentStep)
		{
		}

		public override void OnStartGuide()
		{
			EventManager.RegisterPermanentEvent(EventEnum.E_ShowUIView, OnShowChooseOccupationUI);
		}

		private void OnShowChooseOccupationUI(EventData data)
		{
			SimpleEventData simpleEventData;
			if ((simpleEventData = data as SimpleEventData) != null && simpleEventData.stringValue == "ChooseOccupationUI" && SingletonDontDestroy<Game>.Instance.CurrentUserData.IsUnlockPrephesyCard)
			{
				SingletonDontDestroy<UIManager>.Instance.ShowView("GuideTipsUI", null, new List<string>(1) { "Code_12_1" }, null);
				EventManager.UnregisterPermanentEvent(EventEnum.E_ShowUIView, OnShowChooseOccupationUI);
				_instance.EndAGameGuide(this);
			}
		}

		public override void OnEndGuide()
		{
		}

		public override void ForceEndGuide()
		{
			EventManager.UnregisterPermanentEvent(EventEnum.E_ShowUIView, OnShowChooseOccupationUI);
		}
	}

	private static GuideSystem _instance;

	private readonly List<string> AllNeedTriggerGameGuides = new List<string> { "GuideSystem+Guide_05", "GuideSystem+Guide_06", "GuideSystem+Guide_07", "GuideSystem+Guide_08" };

	private List<Guide> allProcessingGuides = new List<Guide>();

	private BattleGuide _battleGuide;

	public static GuideSystem Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GuideSystem();
			}
			return _instance;
		}
	}

	private GuideSystem()
	{
	}

	public void InitGameGuide()
	{
		if (allProcessingGuides.Count > 0)
		{
			for (int i = 0; i < allProcessingGuides.Count; i++)
			{
				allProcessingGuides[i].ForceEndGuide();
			}
		}
		allProcessingGuides.Clear();
		for (int j = 0; j < AllNeedTriggerGameGuides.Count; j++)
		{
			if (!SingletonDontDestroy<Game>.Instance.CurrentUserData.IsGameGuideEverTrigger(AllNeedTriggerGameGuides[j]))
			{
				Guide guide = (Guide)typeof(Guide).Assembly.CreateInstance(AllNeedTriggerGameGuides[j]);
				guide.OnStartGuide();
				allProcessingGuides.Add(guide);
			}
		}
	}

	private void EndAGameGuide(Guide guide)
	{
		allProcessingGuides.Remove(guide);
		SingletonDontDestroy<Game>.Instance.CurrentUserData.TriggerGameGuideCode(guide.ToString());
	}

	public void StartBattleGuide()
	{
		_battleGuide = new BattleGuide();
		_battleGuide.StartGuide();
		EventManager.RegisterEvent(EventEnum.E_BackToMenu, OnBackToMenu);
	}

	private void OnBackToMenu(EventData data)
	{
		_battleGuide.ForceEndGuide();
		OnForceEndGuide();
		EventManager.UnregisterEvent(EventEnum.E_BackToMenu, OnBackToMenu);
	}

	private void OnForceEndGuide()
	{
		BattleUI obj = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		obj.UnlockNextRoundBtn();
		obj.UnlockSkills();
		obj.UnlockStoringForceBtn();
		obj.SetPlayerInfoDetailBtnBgActive(isActive: true);
	}

	public void EndBattleGuide(bool isPass)
	{
		Singleton<GameManager>.Instance.BattleSystem.EndGuideBattle();
		if (isPass)
		{
			SingletonDontDestroy<UIManager>.Instance.ShowView("GuideTipsUI", null, new List<string>(1) { "Code_7_1" }, new Action(OnBattleGuideEnd));
		}
		else
		{
			SingletonDontDestroy<UIManager>.Instance.ShowView("GuideTipsUI", null, new List<string>(1) { "Code_13_1" }, new Action(OnBattleGuideEnd));
		}
		_battleGuide = null;
	}

	private void OnBattleGuideEnd()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("MaskUI") as MaskUI).ShowMask(0.5f, BackToMenuUI);
	}

	private void BackToMenuUI()
	{
		BattleEnvironmentManager.Instance.HideBg();
		SingletonDontDestroy<UIManager>.Instance.DestoryAllView();
		EventManager.ClearAllEvent();
		SingletonDontDestroy<Game>.Instance.SwitchScene(1);
	}

	private void SwitchToNextStep(GuideStep step)
	{
		_battleGuide.SwitchToNextStep(step);
	}
}
