public class BreakDamnation : BaseGift
{
	public override GiftName Name => GiftName.BreakDamnation;

	protected override void Effect()
	{
		Singleton<GameManager>.Instance.Player.PlayerAttr.ReduceArmor(Singleton<GameManager>.Instance.Player.PlayerAttr.Armor);
	}
}
