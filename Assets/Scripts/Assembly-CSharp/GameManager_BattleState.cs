public class GameManager_BattleState : GameManagerState
{
	private string _currentBgHandlerSpriteName;

	public GameManager_BattleState(GameManager gameManager, string bgHandlerSpriteName)
		: base(gameManager)
	{
		_currentBgHandlerSpriteName = bgHandlerSpriteName;
	}

	public override void OnEnter()
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("BattleUI", _currentBgHandlerSpriteName);
	}

	public override void OnExit()
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("BattleUI");
		SingletonDontDestroy<UIManager>.Instance.HideView("EnemyMeanHintUI");
	}

	public override void OnUpdate()
	{
	}
}
