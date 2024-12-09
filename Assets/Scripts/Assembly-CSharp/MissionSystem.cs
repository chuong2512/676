public class MissionSystem
{
	private static MissionSystem _instance;

	private Mission currentMission;

	public static MissionSystem Instance => _instance ?? (_instance = new MissionSystem());

	private MissionSystem()
	{
	}

	public void AddNewMission(Mission mission)
	{
		mission.InitMission();
		(SingletonDontDestroy<UIManager>.Instance.ShowView("MissionUI") as MissionUI).ShowMissionDes(mission);
		currentMission = mission;
	}

	public void EndMission()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("MissionUI") as MissionUI).HideMissionDes();
	}

	public void ForceEndMission()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("MissionUI") as MissionUI).HideMissionDes();
		currentMission.ForceEndMission();
	}
}
