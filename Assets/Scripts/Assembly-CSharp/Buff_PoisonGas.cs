using UnityEngine;

public class Buff_PoisonGas : BaseBuff
{
	private int poisonGasAmount;

	public int PoisonGasAmount => poisonGasAmount;

	public override BuffType BuffType => BuffType.Buff_PoisonGas;

	public Buff_PoisonGas(EntityBase entityBase, int poisonGasAmount)
		: base(entityBase, int.MaxValue)
	{
		this.poisonGasAmount = poisonGasAmount;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		buffIconCtrl.BuffEffectHint();
		Transform[] array = new Transform[Singleton<EnemyController>.Instance.AllEnemies.Count];
		for (int i = 0; i < Singleton<EnemyController>.Instance.AllEnemies.Count; i++)
		{
			array[i] = Singleton<EnemyController>.Instance.AllEnemies[i].EnemyCtrl.transform;
		}
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer, BuffType), buffIconCtrl.transform, isPlayer ? array : null, BuffEffect);
	}

	private void BuffEffect()
	{
		if (!isPlayer)
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_DeadPoison(Singleton<GameManager>.Instance.Player, poisonGasAmount));
			return;
		}
		for (int i = 0; i < Singleton<EnemyController>.Instance.AllEnemies.Count; i++)
		{
			Singleton<EnemyController>.Instance.AllEnemies[i].GetBuff(new Buff_DeadPoison(Singleton<EnemyController>.Instance.AllEnemies[i], poisonGasAmount));
		}
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		poisonGasAmount += ((Buff_PoisonGas)baseBuff).PoisonGasAmount;
		buffIconCtrl.UpdateBuff();
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		EventManager.RegisterEvent(EventEnum.E_PlayerRound, OnPlayerRound);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		EventManager.UnregisterEvent(EventEnum.E_PlayerRound, OnPlayerRound);
	}

	private void OnPlayerRound(EventData data)
	{
		TakeEffect(entityBase);
	}

	public override string GetBuffHint()
	{
		return "<color=#e9e9e9ff>" + poisonGasAmount + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return poisonGasAmount;
	}
}
