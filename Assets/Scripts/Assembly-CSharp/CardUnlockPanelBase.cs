using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUnlockPanelBase : MonoBehaviour
{
	public abstract class CardUnlockPanelHandler
	{
		public abstract void HandleInit(CardUnlockPanelBase cardUnlockPanelBase);
	}

	public class NormalCardUnlockPanelHandler : CardUnlockPanelHandler
	{
		private bool isInit;

		private GameSettlementUI _gameSettlementUi;

		private PlayerOccupation playerOccupation;

		public GameSettlementUI GameSettlementUi => _gameSettlementUi;

		public int SpaceTimeAmount => _gameSettlementUi.AllSpaceTimeAmount;

		public NormalCardUnlockPanelHandler(GameSettlementUI gameSettlementUi)
		{
			_gameSettlementUi = gameSettlementUi;
		}

		public override void HandleInit(CardUnlockPanelBase cardUnlockPanelBase)
		{
			if (!isInit)
			{
				isInit = true;
				playerOccupation = cardUnlockPanelBase.PlayerOccupation;
				DataManager.UsualUnlockData uusualCardUnlockConfig = DataManager.Instance.GetUusualCardUnlockConfig(playerOccupation);
				UnlockCardCtrl[] componentsInChildren = cardUnlockPanelBase.GetComponentsInChildren<UnlockCardCtrl>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					string cardCode = componentsInChildren[i].CardCode;
					bool isUnlocked = SingletonDontDestroy<Game>.Instance.CurrentUserData.IsUsualCardUnlocked(playerOccupation, cardCode);
					uusualCardUnlockConfig.AllCardTimeSpaceNeed.TryGetValue(cardCode, out var value);
					uusualCardUnlockConfig.AllUnlockConditions.TryGetValue(cardCode, out var value2);
					componentsInChildren[i].LoadCard(new NormalUnlockCardHandler(this, cardUnlockPanelBase, componentsInChildren[i], value, isUnlocked, value2));
				}
			}
		}

		public void UnlockCard(NormalUnlockCardHandler normalHandler)
		{
			normalHandler.SetUnlocked();
			SingletonDontDestroy<Game>.Instance.CurrentUserData.UnlockUsualCard(playerOccupation, normalHandler.CardCode);
			_gameSettlementUi.ComsumeSpaceTime(normalHandler.UnlockAmount);
			EventManager.BroadcastEvent(EventEnum.E_UsualCardUnlock, new SimpleEventData
			{
				stringValue = normalHandler.CardCode
			});
		}

		public void ShowMask(bool isIconShow)
		{
			_gameSettlementUi.ShowMask(0f, isIconShow);
		}

		public void HideMask()
		{
			_gameSettlementUi.HideMask();
		}
	}

	public class CheckCardUnlockPanelHandler : CardUnlockPanelHandler
	{
		public override void HandleInit(CardUnlockPanelBase cardUnlockPanelBase)
		{
			PlayerOccupation playerOccupation = cardUnlockPanelBase.PlayerOccupation;
			DataManager.UsualUnlockData uusualCardUnlockConfig = DataManager.Instance.GetUusualCardUnlockConfig(playerOccupation);
			UnlockCardCtrl[] componentsInChildren = cardUnlockPanelBase.GetComponentsInChildren<UnlockCardCtrl>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				string cardCode = componentsInChildren[i].CardCode;
				bool isUnlocked = SingletonDontDestroy<Game>.Instance.CurrentUserData.IsUsualCardUnlocked(playerOccupation, cardCode);
				uusualCardUnlockConfig.AllCardTimeSpaceNeed.TryGetValue(cardCode, out var value);
				componentsInChildren[i].LoadCard(new CheckUnlockCardHandler(componentsInChildren[i], value, isUnlocked));
			}
		}
	}

	public abstract class UnlockCardHandler
	{
		protected UnlockCardCtrl _unlockCardCtrl;

		public string CardCode => _unlockCardCtrl.CardCode;

		public UnlockCardHandler(UnlockCardCtrl ctrl)
		{
			_unlockCardCtrl = ctrl;
		}

		public abstract void HandleClick();

		public abstract void OnLoad();

		public abstract void OnDisable();
	}

	public class NormalUnlockCardHandler : UnlockCardHandler
	{
		private HashSet<string> unlockConditions;

		private bool isCanUnlocked;

		private CardUnlockPanelBase _cardUnlockPanelBase;

		private NormalCardUnlockPanelHandler _normalCardUnlockPanelHandler;

		private bool isRegisterEvent;

		public bool IsUnlocked { get; private set; }

		public int UnlockAmount { get; }

		public NormalUnlockCardHandler(NormalCardUnlockPanelHandler normalCardUnlockPanelHandler, CardUnlockPanelBase cardUnlockPanelBase, UnlockCardCtrl ctrl, int unlockAmount, bool isUnlocked, List<string> conditions)
			: base(ctrl)
		{
			_normalCardUnlockPanelHandler = normalCardUnlockPanelHandler;
			_cardUnlockPanelBase = cardUnlockPanelBase;
			IsUnlocked = isUnlocked;
			UnlockAmount = unlockAmount;
			CheckIsCanUnlock(conditions);
		}

		public override void HandleClick()
		{
			if (IsUnlocked)
			{
				(SingletonDontDestroy<UIManager>.Instance.ShowView("BagCardDesUI") as BagCardDesUI).ShowBigCard(_unlockCardCtrl.CardCode);
			}
			else
			{
				(SingletonDontDestroy<UIManager>.Instance.ShowView("BagCardDesUI") as BagCardDesUI).ShowBigCard(_unlockCardCtrl.CardCode, "Unlock".LocalizeText(), UnlockThisCard, IsCanUnlock());
			}
		}

		public override void OnLoad()
		{
			if (IsUnlocked)
			{
				SetUnlocked();
				return;
			}
			_unlockCardCtrl.SetTimespaceComsume(UnlockAmount);
			if (unlockConditions.Count == 0)
			{
				SetCanUnclokedNotUnlocked();
			}
			else
			{
				SetCanNotUnlocked();
			}
			RegisterEvent();
		}

		private void RegisterEvent()
		{
			if (!isRegisterEvent)
			{
				isRegisterEvent = true;
				EventManager.RegisterEvent(EventEnum.E_UsualCardUnlock, OnUsualCardUnlock);
			}
		}

		private void UnregisterEvent()
		{
			if (isRegisterEvent)
			{
				isRegisterEvent = false;
				EventManager.UnregisterEvent(EventEnum.E_UsualCardUnlock, OnUsualCardUnlock);
			}
		}

		public override void OnDisable()
		{
			UnregisterEvent();
		}

		private void UnlockThisCard()
		{
			_unlockCardCtrl.StartCoroutine(UnlockThisCard_IE());
		}

		private bool CheckIsCanUnlock(List<string> conditions)
		{
			unlockConditions = new HashSet<string>();
			for (int i = 0; i < conditions.Count; i++)
			{
				if (!SingletonDontDestroy<Game>.Instance.CurrentUserData.IsUsualCardUnlocked(_cardUnlockPanelBase.PlayerOccupation, conditions[i]))
				{
					unlockConditions.Add(conditions[i]);
				}
			}
			return unlockConditions.Count == 0;
		}

		private IEnumerator UnlockThisCard_IE()
		{
			_normalCardUnlockPanelHandler.ShowMask(isIconShow: false);
			_normalCardUnlockPanelHandler.UnlockCard(this);
			VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("effect_ui_jiesuokapai_cast");
			vfxBase.transform.position = _unlockCardCtrl.transform.position;
			vfxBase.Play();
			SingletonDontDestroy<UIManager>.Instance.HideView("BagCardDesUI");
			yield return new WaitForSeconds(2f);
			_normalCardUnlockPanelHandler.HideMask();
		}

		private bool IsCanUnlock()
		{
			if (isCanUnlocked)
			{
				return UnlockAmount <= _normalCardUnlockPanelHandler.SpaceTimeAmount;
			}
			return false;
		}

		public void SetUnlocked()
		{
			IsUnlocked = true;
			_unlockCardCtrl.SpaceTimePanel.gameObject.SetActive(value: false);
		}

		private void SetCanUnclokedNotUnlocked()
		{
			IsUnlocked = false;
			isCanUnlocked = true;
		}

		private void SetCanNotUnlocked()
		{
			IsUnlocked = false;
			isCanUnlocked = false;
		}

		private void OnUsualCardUnlock(EventData data)
		{
			if (!isCanUnlocked)
			{
				SimpleEventData simpleEventData;
				if ((simpleEventData = data as SimpleEventData) != null && unlockConditions.Remove(simpleEventData.stringValue) && unlockConditions.Count == 0)
				{
					SetCanUnclokedNotUnlocked();
				}
			}
			else if (!IsUnlocked)
			{
				_unlockCardCtrl.SetLockMask(IsCanUnlock() ? _normalCardUnlockPanelHandler.GameSettlementUi.lockMask_CanUnlockCard : _normalCardUnlockPanelHandler.GameSettlementUi.lockMask_CannotUnlockCard);
			}
		}
	}

	public class CheckUnlockCardHandler : UnlockCardHandler
	{
		private bool isUnlocked;

		private int unlockAmount;

		public CheckUnlockCardHandler(UnlockCardCtrl ctrl, int unlockAmount, bool isUnlocked)
			: base(ctrl)
		{
			this.isUnlocked = isUnlocked;
			this.unlockAmount = unlockAmount;
		}

		public override void HandleClick()
		{
		}

		public override void OnLoad()
		{
			if (isUnlocked)
			{
				SetUnlocked();
			}
			else
			{
				SetCanNotUnlocked();
			}
		}

		private void SetUnlocked()
		{
			_unlockCardCtrl.SpaceTimePanel.gameObject.SetActive(value: false);
		}

		private void SetCanNotUnlocked()
		{
			_unlockCardCtrl.SpaceTimePanel.gameObject.SetActive(value: true);
			_unlockCardCtrl.SetTimespaceComsume(unlockAmount);
		}

		public override void OnDisable()
		{
		}
	}

	public PlayerOccupation PlayerOccupation;

	public void LoadInitCard(CardUnlockPanelHandler cardUnlockPanelHandler)
	{
		cardUnlockPanelHandler.HandleInit(this);
	}
}
