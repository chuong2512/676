using System;

public class UsualCard_BC_M_8 : UsualCard_Archer
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public UsualCard_BC_M_8(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new SelfDownHandler();
		pointupHandler = new SelfUpHandler();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		Singleton<GameManager>.Instance.Player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingUsualCard, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		int num = IntData + 1;
		return string.Format(usualCardAttr.DesKeyOnBattle.LocalizeText(), BaseCard.GetValueColor(1, num), num);
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		UsualCard.HandleEffect(base.EffectConfig, null, delegate
		{
			Effect(player, handler);
		});
	}

	private void Effect(Player player, Action handler)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingUsualCard, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		player.GetBuff(new Buff_ShootStrengthen(player, 1 + IntData));
		handler?.Invoke();
	}

	protected override bool IsSatisfySpecialStatus(Player player)
	{
		return true;
	}
}
