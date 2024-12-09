using UnityEngine;

public class NextRoomBlock : BaseBlock
{
	public override string HandleLoadActionName => "HandleLoad_ExitBlock";

	public void ActiveNextRoomBlock(Vector2Int pos, int roomSeed)
	{
		base.RoomSeed = roomSeed;
		base.BlockPosition = pos;
	}

	protected override void OnClick()
	{
		if (!RoomUI.IsAnyBlockInteractiong && (Singleton<GameManager>.Instance.CurrentMapLevel != 3 || Singleton<GameManager>.Instance.CurrentMapLayer != 3))
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("点击传送门音效");
			(SingletonDontDestroy<UIManager>.Instance.ShowView("SystemHintUI") as SystemHintUI).ShowTwoChosenSystemHint("ConfirmNextRoomQuestion".LocalizeText(), Effect);
		}
	}

	private void Effect()
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent("点击进入下一层格子");
		}
		Singleton<GameManager>.Instance.SwitchToNextRoom();
		EventManager.BroadcastEvent(EventEnum.E_EnterNextRoom, null);
	}

	public override void ResetBlock()
	{
	}
}
