using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameEvent
{
	[Serializable]
	public enum RepeatType
	{
		NoRepeat,
		LayerRepeat,
		LevelRepeat,
		FollowUp
	}

	protected Action eventOverAction;

	protected Vector2Int eventBlockPosition;

	public abstract string GameEventCode { get; }

	public virtual void StartEvent(int randomSeed, Action eventOverAction)
	{
		UnityEngine.Random.InitState(randomSeed);
		this.eventOverAction = eventOverAction;
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("遇到事件");
		OnStartEvent();
	}

	public void SetBlockPosition(Vector2Int pos)
	{
		eventBlockPosition = pos;
	}

	protected abstract void OnStartEvent();

	protected void EventOver()
	{
		OnEventOver();
		eventOverAction?.Invoke();
		GameEventManager.Instace.OnEventOver(eventBlockPosition, this);
		GameSave.SaveGame();
	}

	protected virtual void OnEventOver()
	{
	}

	public virtual void ClearEvent()
	{
	}

	public virtual void InitEventByData(GameEventSaveData saveData)
	{
	}

	public bool IsCanTrigger(string mapLayer)
	{
		GameEventData gameEventData = DataManager.Instance.GetGameEventData(GameEventCode);
		if (gameEventData.MapLimit != null && gameEventData.MapLimit.Count != 0)
		{
			return gameEventData.MapLimit.Contains(mapLayer);
		}
		return true;
	}

	public virtual GameEventSaveData GetGameEventData()
	{
		return new GameEventSaveData(eventBlockPosition.x, eventBlockPosition.y, GameEventCode);
	}

	protected static void HideGameEventView()
	{
		RoomUI.IsAnyBlockInteractiong = false;
		SingletonDontDestroy<UIManager>.Instance.HideView("GameEventUI");
	}

	protected static void VarifyEventRoomInfo(Vector2Int pos, string oldEventCode, string newEventCode)
	{
		RoomUI obj = SingletonDontDestroy<UIManager>.Instance.ForceGetView("RoomUI") as RoomUI;
		obj.CurrentRoomInfo.eventInfos[new Vec2Seri(pos.x, pos.y)] = newEventCode;
		GameEventData gameEventData = DataManager.Instance.GetGameEventData(oldEventCode);
		obj.VarifyEventRoomInfo(gameEventData.GameEventPrefab, newEventCode);
		GameSave.SaveGame();
	}

	protected static void StartFollowEvent(string eventCode, Action callback)
	{
		GameEventManager.Instace.GetEvent(eventCode).StartEvent(UnityEngine.Random.Range(int.MinValue, int.MaxValue), callback);
	}

	protected static void Event_GetSkill(string skillCode)
	{
		Singleton<GameManager>.Instance.Player.PlayerInventory.AddSkill(skillCode, isNew: true);
		(SingletonDontDestroy<UIManager>.Instance.ShowView("EventResultUI") as EventResultUI).ShowSkillDes(skillCode);
	}

	protected static void Event_GetEquipment(string equipCode)
	{
		Singleton<GameManager>.Instance.Player.PlayerInventory.AddEquipment(equipCode);
		(SingletonDontDestroy<UIManager>.Instance.ShowView("EventResultUI") as EventResultUI).ShowEquip(equipCode);
	}

	protected static void Event_GetEquipments(List<string> equipCodes)
	{
		Sprite[] array = new Sprite[equipCodes.Count];
		string[] array2 = new string[equipCodes.Count];
		for (int i = 0; i < equipCodes.Count; i++)
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.AddEquipment(equipCodes[i]);
			EquipmentCard equipmentCard = FactoryManager.GetEquipmentCard(equipCodes[i]);
			array[i] = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(equipmentCard.ImageName, "Sprites/Equipment");
			array2[i] = equipmentCard.CardName + ":\n" + equipmentCard.CardNormalDes;
		}
		(SingletonDontDestroy<UIManager>.Instance.ShowView("EventResultUI") as EventResultUI).ShowEquip(equipCodes);
	}

	protected static void Event_GetSpecialUsualCard(string cardCode)
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拾取");
		Singleton<GameManager>.Instance.Player.PlayerInventory.AddSpecialUsualCards(cardCode, 1, isNew: true);
		(SingletonDontDestroy<UIManager>.Instance.ShowView("EventResultUI") as EventResultUI).ShowCard(cardCode);
	}

	protected static void Event_GetGift(BaseGift gift, int optIndex, Action callback)
	{
		if (gift == null)
		{
			callback?.Invoke();
			return;
		}
		GiftData giftDataByGiftName = DataManager.Instance.GetGiftDataByGiftName(gift.Name);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound((giftDataByGiftName.GiftType == BaseGift.GiftType.Blessing) ? "获得祝福" : "获得诅咒");
		Transform transform = (SingletonDontDestroy<UIManager>.Instance.GetView("GameEventUI") as GameEventUI).AllEventBtnCtrls[optIndex].Btn.transform;
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("effect_general_trail_jinengzhuangbei");
		vfxBase.Play();
		Vector3 position = transform.position;
		position.z = -5f;
		vfxBase.transform.position = position;
		Vector3 position2 = (SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI).HealtnImgTrans.position;
		position2.z = -5f;
		float x = Mathf.Clamp(position.x, position2.x, UnityEngine.Random.Range(0.3f, 0.7f));
		float y = (position.y + position2.y) / 2f + UnityEngine.Random.Range(4f, 8f);
		vfxBase.transform.TransformMoveByBezier(position, new Vector3(x, y, -5f), position2, 0.65f, delegate
		{
			vfxBase.Recycle();
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.GetGift(gift);
			callback?.Invoke();
		});
	}

	protected static void Event_RecoveryHealth(int value, int optIndex, bool isHideEventView, Action completeHandler)
	{
		GameEventUI gameEventUi = SingletonDontDestroy<UIManager>.Instance.GetView("GameEventUI") as GameEventUI;
		gameEventUi.SetBtnActive(isActive: false);
		Transform transform = gameEventUi.AllEventBtnCtrls[optIndex].Btn.transform;
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("effect_general_trail_shengming");
		vfxBase.Play();
		Vector3 position = transform.position;
		position.z = -5f;
		vfxBase.transform.position = position;
		Transform target = gameEventUi.HealthBarCtrl.transform;
		float x = Mathf.Clamp(position.x, target.position.x, UnityEngine.Random.Range(0.3f, 0.7f));
		float y = Mathf.Clamp(position.y, target.position.y, UnityEngine.Random.Range(0.3f, 0.7f));
		vfxBase.transform.TransformMoveByBezier(position, new Vector3(x, y, -5f), target.position, 0.75f, delegate
		{
			VfxBase vfxBase2 = Singleton<VfxManager>.Instance.LoadVfx("effect_ui_shengminghuode_hit");
			vfxBase2.transform.position = vfxBase.transform.position;
			vfxBase2.Play();
			vfxBase.Recycle();
			completeHandler();
			gameEventUi.HealthBarCtrl.UpdateHealth(Singleton<GameManager>.Instance.Player.PlayerAttr.Health, Singleton<GameManager>.Instance.Player.PlayerAttr.MaxHealth);
			Vector2 vector = new Vector2(0.75f, 1.25f);
			if (isHideEventView)
			{
				Singleton<GameHintManager>.Instance.AddFlowingText_ScreenPos("+" + value, Color.green, Color.black, target, isSetParent: false, Vector3.zero, vector, vector, HideGameEventView);
			}
			else
			{
				Singleton<GameHintManager>.Instance.AddFlowingText_ScreenPos("+" + value, Color.green, Color.black, target, isSetParent: false, Vector3.zero, vector, vector);
			}
		});
	}

	protected static void Event_ReduceHealth(int value, bool isHideEventView, Action completeHandler)
	{
		completeHandler();
		GameEventUI obj = SingletonDontDestroy<UIManager>.Instance.GetView("GameEventUI") as GameEventUI;
		obj.SetBtnActive(isActive: false);
		obj.HealthBarCtrl.UpdateHealth(Singleton<GameManager>.Instance.Player.PlayerAttr.Health, Singleton<GameManager>.Instance.Player.PlayerAttr.MaxHealth);
		Transform transform = obj.HealthBarCtrl.transform;
		Vector2 vector = new Vector2(0.75f, 1.25f);
		if (isHideEventView)
		{
			Singleton<GameHintManager>.Instance.AddFlowingText_ScreenPos("-" + value, Color.red, Color.black, transform, isSetParent: false, Vector3.zero, vector, vector, HideGameEventView);
		}
		else
		{
			Singleton<GameHintManager>.Instance.AddFlowingText_ScreenPos("-" + value, Color.red, Color.black, transform, isSetParent: false, Vector3.zero, vector, vector);
		}
	}

	protected static void Event_GainMoney(int amount, int optIndex, bool isHideEventView, Action completeHandler, bool isBtnActive = false)
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("获得金钱");
		GameEventUI gameEventUi = SingletonDontDestroy<UIManager>.Instance.GetView("GameEventUI") as GameEventUI;
		if (!isBtnActive)
		{
			gameEventUi.SetBtnActive(isActive: false);
		}
		Transform transform = gameEventUi.AllEventBtnCtrls[optIndex].Btn.transform;
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("effect_general_trail_jinengzhuangbei");
		vfxBase.Play();
		Vector3 position = transform.position;
		position.z = -5f;
		vfxBase.transform.position = position;
		Transform target = gameEventUi.CoinAmountTrans;
		float x = Mathf.Clamp(position.x, target.position.x, UnityEngine.Random.Range(0.3f, 0.7f));
		float y = Mathf.Clamp(position.y, target.position.y, UnityEngine.Random.Range(0.3f, 0.7f));
		vfxBase.transform.TransformMoveByBezier(position, new Vector3(x, y, -5f), target.position, 0.7f, delegate
		{
			vfxBase.Recycle();
			completeHandler();
			gameEventUi.UpdatePlayerCoinAmount();
			if (isHideEventView)
			{
				Singleton<GameHintManager>.Instance.AddFlowingText_ScreenPos("+" + amount, Color.yellow, Color.black, target, isSetParent: false, Vector3.zero, HideGameEventView);
			}
			else
			{
				Singleton<GameHintManager>.Instance.AddFlowingText_ScreenPos("+" + amount, Color.yellow, Color.black, target, isSetParent: false, Vector3.zero);
			}
		});
	}

	protected static void Event_LossMoney(int amount, int optIndex, bool isHideEventView, Action completeHandler)
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("购买商品");
		completeHandler();
		GameEventUI gameEventUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameEventUI") as GameEventUI;
		gameEventUI.SetBtnActive(isActive: false);
		if (isHideEventView)
		{
			Singleton<GameHintManager>.Instance.AddFlowingText_ScreenPos("-" + amount, Color.yellow, Color.black, gameEventUI.AllEventBtnCtrls[optIndex].Btn.transform, isSetParent: false, Vector3.zero, HideGameEventView);
		}
		else
		{
			Singleton<GameHintManager>.Instance.AddFlowingText_ScreenPos("-" + amount, Color.yellow, Color.black, gameEventUI.AllEventBtnCtrls[optIndex].Btn.transform, isSetParent: false, Vector3.zero);
		}
		gameEventUI.UpdatePlayerCoinAmount();
	}
}
