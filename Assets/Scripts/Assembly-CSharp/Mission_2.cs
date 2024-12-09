public class Mission_2 : Mission
{
	protected override string MissionDesKey => "Mission2DesKey";

	protected override int MissionIndex => 2;

	public override void InitMission()
	{
		EventManager.RegisterEvent(EventEnum.E_PlayerUseSkillCard, OnPlayerUseSkillCard);
	}

	protected override void OnMissionOver()
	{
		EventManager.UnregisterEvent(EventEnum.E_PlayerUseSkillCard, OnPlayerUseSkillCard);
	}

	public override string GetMissionDescription()
	{
		return MissionDesKey.LocalizeText();
	}

	private void OnPlayerUseSkillCard(EventData data)
	{
		MissionComplete();
	}
}
