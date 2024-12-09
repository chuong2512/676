public class FreezeDamnation : BaseGift
{
	public override GiftName Name => GiftName.FreezeDamnation;

	protected override void Effect()
	{
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Freeze(Singleton<GameManager>.Instance.Player, 2));
	}
}
