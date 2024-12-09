using UnityEngine;

public class Buff_Cover : BaseBuff
{
	private int dmgReduceAmount;

	public int DmgReduceAmount => dmgReduceAmount;

	public override BuffType BuffType => BuffType.Buff_Cover;

	public Buff_Cover(EntityBase entityBase, int dmgReduceAmount)
		: base(entityBase, 1)
	{
		this.dmgReduceAmount = dmgReduceAmount;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer, BuffType), buffIconCtrl.transform, isPlayer ? null : new Transform[1] { ((EnemyBase)entityBase).EnemyCtrl.transform }, null);
		buffIconCtrl.BuffEffectHint();
	}

	public override void UpdateRoundTurn()
	{
		buffRemainRound -= 0.5f;
		if (buffRemainRound <= 0f)
		{
			entityBase.RemoveBuff(this);
		}
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		dmgReduceAmount += ((Buff_Cover)baseBuff).DmgReduceAmount;
		buffIconCtrl.UpdateBuff();
	}

	protected override void BroadcastNewBuffAddEvent()
	{
		EventManager.BroadcastEvent(entityBase, EventEnum.E_GetNewBuff, new BuffEventData
		{
			buffType = BuffType,
			intValue = dmgReduceAmount
		});
	}

	protected override void BroadcastSameBuffAddEvent(BaseBuff baseBuff)
	{
		EventManager.BroadcastEvent(entityBase, EventEnum.E_GetSameBuff, new BuffEventData
		{
			buffType = BuffType,
			intValue = ((Buff_Cover)baseBuff).DmgReduceAmount
		});
	}

	public override string GetBuffHint()
	{
		return string.Format("{0}{1}</color>", "<color=#27dd34ff>", dmgReduceAmount);
	}

	public override int GetBuffHinAmount()
	{
		return dmgReduceAmount;
	}
}
