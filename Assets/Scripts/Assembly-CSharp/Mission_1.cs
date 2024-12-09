public class Mission_1 : Mission
{
	protected override string MissionDesKey => "Mission1DesKey";

	protected override int MissionIndex => 1;

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
