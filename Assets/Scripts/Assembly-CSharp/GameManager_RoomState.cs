public class GameManager_RoomState : GameManagerState
{
	public GameManager_RoomState(GameManager gameManager)
		: base(gameManager)
	{
	}

	public override void OnEnter()
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("RoomUI");
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
	}
}
