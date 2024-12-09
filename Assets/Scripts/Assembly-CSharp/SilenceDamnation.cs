public class SilenceDamnation : BaseGift
{
	public override GiftName Name => GiftName.SilenceDamnation;

	protected override void Effect()
	{
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Silence(Singleton<GameManager>.Instance.Player, 2));
	}
}
