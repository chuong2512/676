using System;

public class GameMenuState : GameState
{
	public override void OnEnter()
	{
		EventManager.ClearAllEvent();
		CheckAppData();
	}

	public override void OnExit()
	{
		SingletonDontDestroy<UIManager>.Instance.DestoryView("GameMenuUI");
		SingletonDontDestroy<ResourceManager>.Instance.UnloadAllAssets();
	}

	public override void OnUpdate()
	{
	}

	private void CheckAppData()
	{
		if (SingletonDontDestroy<Game>.Instance.AppData == null || SingletonDontDestroy<Game>.Instance.isForcedtoPlayCG)
		{
			SingletonDontDestroy<UIManager>.Instance.ShowView("CGUI", new Action(ShowMenuUI));
		}
		else
		{
			ShowMenuUI();
		}
	}

	private void ShowMenuUI()
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("GameMenuUI");
	}
}
