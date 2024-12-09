using System.Collections.Generic;

public class Buff_MagicSquama : BaseBuff
{
	private int accumulate;

	public override BuffType BuffType => BuffType.Buff_MagicSquama;

	public Buff_MagicSquama(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
		accumulate = 0;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer, BuffType), buffIconCtrl.transform, null, BuffEffect);
	}

	private void BuffEffect()
	{
		BaseBuff.AtkEntity(Singleton<GameManager>.Instance.Player, 10, isAbsDmg: true);
		List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
		for (int i = 0; i < allEnemies.Count; i++)
		{
			allEnemies[i].GetBuff(new Buff_Power(allEnemies[i], 1));
		}
	}

	public override void UpdateRoundTurn()
	{
	}

	public override string GetBuffHint()
	{
		return "<color=#27dd34ff>" + accumulate + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return accumulate;
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		EventManager.RegisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseUsualCard);
		EventManager.RegisterEvent(EventEnum.E_PlayerUseSkillCard, OnPlayerUseSkill);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		EventManager.UnregisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseUsualCard);
		EventManager.UnregisterEvent(EventEnum.E_PlayerUseSkillCard, OnPlayerUseSkill);
	}

	private void OnPlayerUseUsualCard(EventData data)
	{
		AddAccumulate();
	}

	private void OnPlayerUseSkill(EventData data)
	{
		AddAccumulate();
	}

	private void AddAccumulate()
	{
		accumulate++;
		if (accumulate == 10)
		{
			TakeEffect(entityBase);
			accumulate = 0;
		}
		buffIconCtrl.UpdateBuff();
	}
}
