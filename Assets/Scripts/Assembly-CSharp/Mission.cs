public abstract class Mission
{
	protected abstract string MissionDesKey { get; }

	protected abstract int MissionIndex { get; }

	public abstract void InitMission();

	protected abstract void OnMissionOver();

	public abstract string GetMissionDescription();

	protected void MissionComplete()
	{
		OnMissionOver();
		EventManager.BroadcastEvent(EventEnum.E_MissionComplete, new SimpleEventData
		{
			intValue = MissionIndex
		});
		MissionSystem.Instance.EndMission();
	}

	public virtual void ForceEndMission()
	{
	}
}
