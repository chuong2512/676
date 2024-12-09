public class PlagueDamnation : BaseGift
{
	public override GiftName Name => GiftName.PlagueDamnation;

	protected override void Effect()
	{
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_DeadPoison(Singleton<GameManager>.Instance.Player, 2));
	}
}
