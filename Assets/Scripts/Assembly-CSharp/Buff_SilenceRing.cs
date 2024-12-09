public class Buff_SilenceRing : BaseBuff
{
	private int useTime;

	public override BuffType BuffType => BuffType.Buff_SilenceRing;

	public Buff_SilenceRing(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
		useTime = 0;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: true, BuffType), buffIconCtrl.transform, null, BuffEffect);
		buffIconCtrl.BuffEffectHint();
	}

	private void BuffEffect()
	{
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Silence(Singleton<GameManager>.Instance.Player, 1));
	}

	public override void UpdateRoundTurn()
	{
	}

	public override string GetBuffHint()
	{
		return "<color=#ec2125ff>" + useTime + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return useTime;
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		EventManager.RegisterEvent(EventEnum.E_PlayerUseSkillCard, OnPlayerUseASkill);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		EventManager.UnregisterEvent(EventEnum.E_PlayerUseSkillCard, OnPlayerUseASkill);
	}

	private void OnPlayerUseASkill(EventData data)
	{
		useTime++;
		if (useTime >= 3)
		{
			useTime -= 3;
			TakeEffect(entityBase);
		}
		buffIconCtrl.UpdateBuff();
	}
}
