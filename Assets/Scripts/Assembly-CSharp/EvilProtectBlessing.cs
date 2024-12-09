public class EvilProtectBlessing : BaseGift
{
	public override GiftName Name => GiftName.EvilProtectBlessing;

	protected override void Effect()
	{
		Player player = Singleton<GameManager>.Instance.Player;
		player.GetBuff(new Buff_DamageRestrik(player, 5));
		player.GetBuff(new Buff_Heal(player, 10));
		player.GetBuff(new Buff_Power(player, 3));
	}
}
