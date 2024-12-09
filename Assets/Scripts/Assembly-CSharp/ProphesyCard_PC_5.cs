public class ProphesyCard_PC_5 : ProphesyCard
{
	public override string ProphesyCode => "PC_5";

	public override void Active(bool isLoad)
	{
		if (!isLoad)
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.VarifyMaxHealth(5);
		}
	}
}
