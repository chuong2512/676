public class GamePlayState : GameState
{
	public override void OnEnter()
	{
		Singleton<GameManager>.Instance.StartGame();
		SingletonDontDestroy<UIManager>.Instance.ReInitAllUI();
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
	}
}
