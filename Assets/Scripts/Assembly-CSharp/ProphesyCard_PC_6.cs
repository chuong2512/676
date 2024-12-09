public class ProphesyCard_PC_6 : ProphesyCard
{
	public override string ProphesyCode => "PC_6";

	public override void Active(bool isLoad)
	{
		if (!isLoad)
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerEarnMoney(15);
		}
	}
}
