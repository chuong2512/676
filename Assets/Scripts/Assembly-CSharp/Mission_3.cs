public class Mission_3 : Mission
{
	protected override string MissionDesKey => "Mission3DesKey";

	protected override int MissionIndex => 3;

	public override void InitMission()
	{
		EventManager.RegisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
	}

	protected override void OnMissionOver()
	{
		EventManager.UnregisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
	}

	public override string GetMissionDescription()
	{
		return MissionDesKey.LocalizeText();
	}

	private void OnEnemyDead(EventData data)
	{
		MissionComplete();
	}
}
