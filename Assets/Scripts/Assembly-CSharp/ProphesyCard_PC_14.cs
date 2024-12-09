public class ProphesyCard_PC_14 : ProphesyCard
{
	public override string ProphesyCode => "PC_14";

	public override void Active(bool isLoad)
	{
		Singleton<GameManager>.Instance.Player.PlayerLevelUpEffect = new LevelUp_ProphesyCard_14();
	}
}
