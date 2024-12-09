public class Buff_ProtectHalo : BaseBuff
{
	private int armorAmount;

	public int ArmorAmounr => armorAmount;

	public override BuffType BuffType => BuffType.Buff_ProtectHalo;

	public Buff_ProtectHalo(EntityBase entityBase, int armorAmount)
		: base(entityBase, int.MaxValue)
	{
		this.armorAmount = armorAmount;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: true, BuffType), buffIconCtrl.transform, null, BuffEffect);
		buffIconCtrl.BuffEffectHint();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的守护光环buff效果触发，获得护甲值：" + armorAmount);
		}
	}

	private void BuffEffect()
	{
		Singleton<GameManager>.Instance.Player.PlayerAttr.AddArmor(armorAmount);
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		armorAmount += ((Buff_ProtectHalo)baseBuff).ArmorAmounr;
		buffIconCtrl.UpdateBuff();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的守护光环buff效果获得提升，每回合能获得的护甲值为：" + armorAmount);
		}
	}

	public override string GetBuffHint()
	{
		return "<color=#27dd34ff>" + armorAmount + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return armorAmount;
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
}
