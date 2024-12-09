public class ProphesyCard_PC_11 : ProphesyCard
{
	public override string ProphesyCode => "PC_11";

	public override void Active(bool isLoad)
	{
		Singleton<GameManager>.Instance.Player.PlayerLevelUpEffect = new LevelUp_ProphesyCard_11();
	}
}
