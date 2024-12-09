public class FragileDamnation : BaseGift
{
	public override GiftName Name => GiftName.FragileDamnation;

	protected override void Effect()
	{
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_BrokenArmor(Singleton<GameManager>.Instance.Player, 3));
	}
}
