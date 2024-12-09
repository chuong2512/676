public class ProphesyCard_PC_17 : ProphesyCard
{
	public override string ProphesyCode => "PC_17";

	public override void Active(bool isLoad)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
	}

	~ProphesyCard_PC_17()
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
	}

	private void OnBattleEnd(EventData data)
	{
		Singleton<GameManager>.Instance.Player.PlayerAttr.VarifyMaxHealth(1);
	}
}
