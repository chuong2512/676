public class Mission_4 : Mission
{
	private int amount = 2;

	protected override string MissionDesKey => "Mission4DesKey";

	protected override int MissionIndex => 4;

	public override void InitMission()
	{
		amount = 2;
		EventManager.RegisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
	}

	protected override void OnMissionOver()
	{
		EventManager.UnregisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
	}

	private void OnEnemyDead(EventData data)
	{
		amount--;
		if (amount == 0)
		{
			MissionComplete();
		}
	}

	public override string GetMissionDescription()
	{
		return MissionDesKey.LocalizeText();
	}
}
