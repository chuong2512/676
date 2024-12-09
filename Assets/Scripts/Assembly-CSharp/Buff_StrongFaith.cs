using System;

public class Buff_StrongFaith : BaseBuff
{
	private KnightPlayerAttr _knightPlayerAttr;

	public override BuffType BuffType => BuffType.Buff_StrongFaith;

	public Buff_StrongFaith(Player player, int round)
		: base(player, round)
	{
		if (player.PlayerOccupation != PlayerOccupation.Knight)
		{
			throw new Exception("Add wrong buff to player, player occupation is not match");
		}
		_knightPlayerAttr = (KnightPlayerAttr)player.PlayerAttr;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		buffIconCtrl.BuffEffectHint();
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: true, BuffType), buffIconCtrl.transform, null, BuffEffect);
	}

	private void BuffEffect()
	{
		_knightPlayerAttr.AddSpecialAttr(1);
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		EventManager.RegisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseAUsualCard);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		EventManager.UnregisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseAUsualCard);
	}

	private void OnPlayerUseAUsualCard(EventData data)
	{
		TakeEffect(entityBase);
	}

	public override string GetBuffHint()
	{
		return string.Empty;
	}

	public override int GetBuffHinAmount()
	{
		return 0;
	}
}
