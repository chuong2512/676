public class ProphesyCard_PC_16 : ProphesyCard
{
	public override string ProphesyCode => "PC_16";

	public override void Active(bool isLoad)
	{
		if (!isLoad)
		{
			OnLoadRoomInfo(null);
		}
		EventManager.RegisterEvent(EventEnum.E_LoadRoomInfo, OnLoadRoomInfo);
	}

	~ProphesyCard_PC_16()
	{
		EventManager.UnregisterEvent(EventEnum.E_LoadRoomInfo, OnLoadRoomInfo);
	}

	private void OnLoadRoomInfo(EventData data)
	{
		if (Singleton<GameManager>.Instance.CurrentMapLevel == 1 && (Singleton<GameManager>.Instance.CurrentMapLayer == 1 || Singleton<GameManager>.Instance.CurrentMapLayer == 2))
		{
			(SingletonDontDestroy<UIManager>.Instance.GetView("RoomUI") as RoomUI).ForceOpenAllRoomBlocks(isNeedAnim: true);
		}
	}
}
