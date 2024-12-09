using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BossBlock : BaseBlock
{
	private const string AssetPath = "Sprites/RoomUI";

	private Image bossHeadportraitImg;

	private EnemyHeapData _heapData;

	private bool isEverInteracted;

	public override string HandleLoadActionName => "HandleLoad_BossBlock";

	public void ActiveBossBlock(EnemyHeapData heapData, int roomSeed, bool isEnd)
	{
		base.RoomSeed = roomSeed;
		bossHeadportraitImg.gameObject.SetActive(!isEnd);
		_heapData = heapData;
		bossHeadportraitImg.transform.localScale = new Vector3(-1f, 1f, 1f);
		BossHeapData bossHeapData = DataManager.Instance.GetBossHeapData(heapData.EnemyHeapCode);
		bossHeadportraitImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(bossHeapData.BossNagtiveHeadPortraitName, "Sprites/RoomUI");
		isEverInteracted = isEnd;
		if (isEnd)
		{
			Vector2Int pos = new Vector2Int(3, 3);
			(SingletonDontDestroy<UIManager>.Instance.ForceGetView("RoomUI") as RoomUI).AddNormalNextRoomBlock_ForBoss(pos);
		}
	}

	protected override void OnAwake()
	{
		base.OnAwake();
		bossHeadportraitImg = base.transform.Find("BossHeadPortrait").GetComponent<Image>();
	}

	protected override void OnClick()
	{
		if (isEverInteracted || RoomUI.IsAnyBlockInteractiong)
		{
			return;
		}
		RoomUI.IsAnyBlockInteractiong = true;
		isEverInteracted = true;
		Random.InitState(base.RoomSeed);
		bossHeadportraitImg.transform.DOScaleX(0f, 0.4f).OnComplete(delegate
		{
			BossHeapData bossHeapData = DataManager.Instance.GetBossHeapData(_heapData.EnemyHeapCode);
			bossHeadportraitImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(bossHeapData.BossPositiveHeadPortraitName, "Sprites/RoomUI");
			bossHeadportraitImg.transform.DOScaleX(1f, 0.4f).OnComplete(delegate
			{
				Singleton<GameManager>.Instance.StartBattle(new BattleSystem.NormalBattleHandler(_heapData), SingletonDontDestroy<CameraController>.Instance.MainCamera.WorldToViewportPoint(base.transform.position));
				EventManager.RegisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
				GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
				if (gameReportUI != null)
				{
					gameReportUI.AddSystemReportContent("开始Boss战斗");
				}
			});
		});
	}

	private void OnBattleEnd(EventData data)
	{
		RoomUI roomUI = SingletonDontDestroy<UIManager>.Instance.ForceGetView("RoomUI") as RoomUI;
		bossHeadportraitImg.gameObject.SetActive(value: false);
		RoomUI.IsAnyBlockInteractiong = false;
		EventManager.UnregisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
		if (Singleton<GameManager>.Instance.CurrentMapLevel != 3 || Singleton<GameManager>.Instance.CurrentMapLayer != 3)
		{
			Vector2Int vector2Int = new Vector2Int(2, 1);
			roomUI.RecycleAEmptyBlock(vector2Int);
			BaseGameEvent @event = GameEventManager.Instace.GetEvent("Event_33");
			GameEventData gameEventData = DataManager.Instance.GetGameEventData(@event.GameEventCode);
			GameEventManager.Instace.TriggeredEvent(vector2Int, @event);
			@event.SetBlockPosition(vector2Int);
			roomUI.AddSpecialEventBlock_ForBoss(vector2Int, gameEventData.GameEventPrefab, @event);
			if (!Singleton<GameManager>.Instance.Player.IsDead)
			{
				Singleton<GameManager>.Instance.AddClearBossHeapID(_heapData.EnemyHeapCode);
				GameSave.SaveGame();
			}
		}
		else if (!Singleton<GameManager>.Instance.Player.IsDead)
		{
			Singleton<GameManager>.Instance.AddClearBossHeapID(_heapData.EnemyHeapCode);
			GameSave.SaveGame();
		}
		Vector2Int pos = new Vector2Int(3, 3);
		roomUI.AddNormalNextRoomBlock_ForBoss(pos);
	}

	public override void ResetBlock()
	{
	}
}
