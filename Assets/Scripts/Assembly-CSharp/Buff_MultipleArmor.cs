using UnityEngine;

public class Buff_MultipleArmor : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_MultipleArmor;

	public Buff_MultipleArmor(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: false, BuffType), buffIconCtrl.transform, new Transform[1] { ((EnemyBase)entityBase).EnemyCtrl.transform }, BuffEffect);
		buffIconCtrl.BuffEffectHint();
	}

	private void BuffEffect()
	{
		entityBase.TakeDamage(40, null, isAbsDmg: true);
		entityBase.GetBuff(new Buff_HolyProtect(entityBase, 1));
		buffIconCtrl.UpdateBuff();
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
}
