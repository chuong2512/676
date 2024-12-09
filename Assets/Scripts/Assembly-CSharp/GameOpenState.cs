public class GameOpenState : GameState
{
	public override void OnEnter()
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("GameOpenUI");
	}

	public override void OnExit()
	{
		SingletonDontDestroy<UIManager>.Instance.DestoryView("GameOpenUI");
		SingletonDontDestroy<ResourceManager>.Instance.UnloadAllAssets();
	}

	public override void OnUpdate()
	{
	}
}
