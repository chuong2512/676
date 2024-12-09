public abstract class GameManagerState
{
	private GameManager gameManager;

	public GameManagerState(GameManager gameManager)
	{
		this.gameManager = gameManager;
	}

	public abstract void OnEnter();

	public abstract void OnExit();

	public abstract void OnUpdate();
}
