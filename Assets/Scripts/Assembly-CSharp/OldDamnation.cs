public class OldDamnation : BaseGift
{
	public override GiftName Name => GiftName.OldDamnation;

	protected override void Effect()
	{
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Shocked(Singleton<GameManager>.Instance.Player, 3));
	}
}
