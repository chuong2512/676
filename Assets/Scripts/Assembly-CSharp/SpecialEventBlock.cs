using System.Collections;
using UnityEngine;

public class SpecialEventBlock : BaseBlock
{
	public string eventCode;

	[HideInInspector]
	public int roomSeed;

	public BaseGameEvent GameEvent { get; protected set; }

	public override string HandleLoadActionName => "HandleLoad_EventBlock";

	public virtual void ActiveEventBlock(Vector2Int pos, int roomSeed, BaseGameEvent gameEvent, bool isAutoActive)
	{
		this.roomSeed = roomSeed;
		GameEvent = gameEvent;
		base.BlockPosition = pos;
		if (isAutoActive)
		{
			StartCoroutine(ActiveRandomEventBlock_IE());
		}
	}

	public void ChangeEventCode(string newEventCode)
	{
		eventCode = newEventCode;
		GameEvent = GameEventManager.Instace.GetEvent(newEventCode);
		GameEvent.SetBlockPosition(base.BlockPosition);
		GameEventManager.Instace.ChangeEventOnMap(base.BlockPosition, GameEvent);
	}

	protected IEnumerator ActiveRandomEventBlock_IE()
	{
		RoomUI.IsAnyBlockInteractiong = true;
		yield return new WaitForSeconds(0.8f);
		StartEvent();
	}

	protected override void OnClick()
	{
		if (!RoomUI.IsAnyBlockInteractiong)
		{
			StartEvent();
		}
	}

	private void StartEvent()
	{
		GameEvent.StartEvent(roomSeed, ClearEventBlock);
	}

	private void ClearEventBlock()
	{
		RoomUI obj = SingletonDontDestroy<UIManager>.Instance.ForceGetView("RoomUI") as RoomUI;
		GameEventData gameEventData = DataManager.Instance.GetGameEventData(eventCode);
		obj.RecycleSpecialEventBlock(gameEventData.GameEventPrefab);
		obj.AddEmptyBlock(base.BlockPosition, isSetSprite: false);
	}

	public override void ResetBlock()
	{
	}
}
