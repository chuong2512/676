using UnityEngine;

public class Buff_Defence : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_Defence;

	public Buff_Defence(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer, BuffType), buffIconCtrl.transform, isPlayer ? null : new Transform[1] { ((EnemyBase)entityBase).EnemyCtrl.transform }, null);
		buffIconCtrl.BuffEffectHint();
	}

	public override string GetBuffImmueHint()
	{
		return "CannotEnterDefenceStat".LocalizeText();
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		entityBase.GetBuff(new Buff_Stable(entityBase, 1));
	}

	public override string GetBuffHint()
	{
		if (isPlayer)
		{
			return "<color=#e9e9e9ff>" + ((PlayerAttr)entityBase.EntityAttr).DefenceAttr + "</color>";
		}
		return "<color=#e9e9e9ff>" + ((EnemyAttr)entityBase.EntityAttr).Block + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		if (isPlayer)
		{
			return ((PlayerAttr)entityBase.EntityAttr).DefenceAttr;
		}
		return ((EnemyAttr)entityBase.EntityAttr).Block;
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		EventManager.RegisterEvent(isPlayer ? EventEnum.E_PlayerBlockDmg : EventEnum.E_EnemyBlockDmg, OnEntityBlocked);
		if (isPlayer)
		{
			(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).SetPlayerSpecialPossHeadPortrait();
			EventManager.RegisterObjRelatedEvent(entityBase, EventEnum.E_GetSameBuff, OnGetSameBuff);
			EventManager.RegisterObjRelatedEvent(entityBase, EventEnum.E_GetNewBuff, OnGetNewBuff);
			EventManager.RegisterObjRelatedEvent(entityBase, EventEnum.E_RemoveBuff, OnRemoveBuff);
		}
	}

	public override void HandleBuffRemove()
	{
		BaseBuff specificBuff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(entityBase, BuffType.Buff_DefenceRestrik);
		if (!specificBuff.IsNull())
		{
			entityBase.RemoveBuff(specificBuff);
		}
		BaseBuff specificBuff2 = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(entityBase, BuffType.Buff_Stable);
		if (!specificBuff2.IsNull())
		{
			entityBase.RemoveBuff(specificBuff2);
		}
		EventManager.UnregisterEvent(isPlayer ? EventEnum.E_PlayerBlockDmg : EventEnum.E_EnemyBlockDmg, OnEntityBlocked);
		if (isPlayer)
		{
			(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).SetPlayerNormalHeapPortrait();
			EventManager.UnregisterObjRelatedEvent(entityBase, EventEnum.E_GetSameBuff, OnGetSameBuff);
			EventManager.UnregisterObjRelatedEvent(entityBase, EventEnum.E_GetNewBuff, OnGetNewBuff);
			EventManager.UnregisterObjRelatedEvent(entityBase, EventEnum.E_RemoveBuff, OnRemoveBuff);
		}
		base.HandleBuffRemove();
	}

	private void OnGetSameBuff(EventData data)
	{
		BuffEventData buffEventData;
		if ((buffEventData = data as BuffEventData) != null && (buffEventData.buffType == BuffType.Buff_Stable || buffEventData.buffType == BuffType.Buff_JianShou))
		{
			buffIconCtrl.UpdateBuff();
		}
	}

	private void OnGetNewBuff(EventData data)
	{
		BuffEventData buffEventData;
		if ((buffEventData = data as BuffEventData) != null && (buffEventData.buffType == BuffType.Buff_Stable || buffEventData.buffType == BuffType.Buff_JianShou))
		{
			buffIconCtrl.UpdateBuff();
		}
	}

	private void OnRemoveBuff(EventData data)
	{
		BuffEventData buffEventData;
		if ((buffEventData = data as BuffEventData) != null && (buffEventData.buffType == BuffType.Buff_Stable || buffEventData.buffType == BuffType.Buff_JianShou))
		{
			buffIconCtrl.UpdateBuff();
		}
	}

	private void OnEntityBlocked(EventData data)
	{
		TakeEffect(entityBase);
	}
}
