public class ProphesyCard_PC_12 : ProphesyCard
{
	public override string ProphesyCode => "PC_12";

	public override void Active(bool isLoad)
	{
		EventManager.RegisterEvent(EventEnum.E_EnterNextRoom, OnEnterNextRoom);
	}

	~ProphesyCard_PC_12()
	{
		EventManager.UnregisterEvent(EventEnum.E_EnterNextRoom, OnEnterNextRoom);
	}

	private void OnEnterNextRoom(EventData data)
	{
		Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryHealth(Singleton<GameManager>.Instance.Player.PlayerAttr.MaxHealth);
		Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney);
	}
}
