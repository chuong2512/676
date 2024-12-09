using System.Collections;
using UnityEngine;

public class Buff_CountDown : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_CountDown;

	public Buff_CountDown(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: false, BuffType), entityBase.EntityTransform, new Transform[1] { entityBase.EntityTransform }, BuffEffect);
	}

	private void BuffEffect()
	{
		BaseBuff.AtkEntity(Singleton<GameManager>.Instance.Player, 20, isAbsDmg: true);
		entityBase.TakeDamage(999, null, isAbsDmg: true);
	}

	public override void UpdateRoundTurn()
	{
		buffRemainRound -= 0.5f;
		if (buffRemainRound == 0f)
		{
			SingletonDontDestroy<Game>.Instance.StartCoroutine(Update_IE());
			return;
		}
		((Enemy_56)entityBase).OnEnemyRound(base.BuffRemainRound);
		buffIconCtrl.UpdateBuff();
	}

	private IEnumerator Update_IE()
	{
		yield return null;
		TakeEffect(entityBase);
	}

	public override string GetBuffHint()
	{
		return base.BuffRemainRound.ToString();
	}

	public override int GetBuffHinAmount()
	{
		return base.BuffRemainRound;
	}
}
