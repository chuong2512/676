using UnityEngine;

public class Buff_Cowardly : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_Cowardly;

	public Buff_Cowardly(EntityBase entityBase, int round)
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
		((Enemy_58)entityBase).ForceToAction4();
	}

	public override void UpdateRoundTurn()
	{
	}

	public override string GetBuffHint()
	{
		return string.Empty;
	}

	public override int GetBuffHinAmount()
	{
		return 0;
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		EventManager.RegisterObjRelatedEvent(entityBase, EventEnum.E_EntityGetHurt, OnEntityGetHurt);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		EventManager.UnregisterObjRelatedEvent(entityBase, EventEnum.E_EntityGetHurt, OnEntityGetHurt);
	}

	private void OnEntityGetHurt(EventData data)
	{
		SimpleEventData simpleEventData;
		if ((simpleEventData = data as SimpleEventData) != null && simpleEventData.intValue > 0)
		{
			TakeEffect(entityBase);
		}
	}
}
