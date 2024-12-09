using UnityEngine;

public class Buff_Sharp : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_Sharp;

	public Buff_Sharp(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		buffIconCtrl.BuffEffectHint();
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer, BuffType), buffIconCtrl.transform, isPlayer ? null : new Transform[1] { ((EnemyBase)entityBase).EnemyCtrl.transform }, BuffEffect);
	}

	private void BuffEffect()
	{
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Bleeding(Singleton<GameManager>.Instance.Player, 1));
	}

	public override void UpdateRoundTurn()
	{
	}

	public override string GetBuffHint()
	{
		return "<color=#27dd34ff>" + base.BuffRemainRound + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return base.BuffRemainRound;
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		EventManager.RegisterObjRelatedEvent(entityBase, EventEnum.E_EnemyAtkPlayer, OnEntityAtkPlayer);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		EventManager.UnregisterObjRelatedEvent(entityBase, EventEnum.E_EnemyAtkPlayer, OnEntityAtkPlayer);
	}

	private void OnEntityAtkPlayer(EventData data)
	{
		TakeEffect(entityBase);
	}
}
