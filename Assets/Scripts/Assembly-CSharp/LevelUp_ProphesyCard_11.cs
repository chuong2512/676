public class LevelUp_ProphesyCard_11 : PlayerLevelUpEffect
{
	protected override string IconName => "获得20金币";

	public override string NameKey => "LevelUpEffect_PC11_NameKey";

	public override string DesKey => "LevelUpEffect_PC11_DesKey";

	public override void Effect()
	{
		Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerEarnMoney(20);
	}
}
