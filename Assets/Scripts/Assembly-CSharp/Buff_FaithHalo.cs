using System;

public class Buff_FaithHalo : BaseBuff
{
	private int faithAmount;

	private KnightPlayerAttr _knightPlayerAttr;

	public int FaithAmount => faithAmount;

	public override BuffType BuffType => BuffType.Buff_FaithHalo;

	public Buff_FaithHalo(Player player, int faithAmount)
		: base(player, int.MaxValue)
	{
		if (player.PlayerOccupation != PlayerOccupation.Knight)
		{
			throw new Exception("Add wrong buff to player, player occupation is not match");
		}
		this.faithAmount = faithAmount;
		_knightPlayerAttr = (KnightPlayerAttr)player.PlayerAttr;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: true, BuffType), buffIconCtrl.transform, null, BuffEffect);
		buffIconCtrl.BuffEffectHint();
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("特效_信仰回复");
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的信仰buff效果触发，获得额外的信仰值为：" + faithAmount);
		}
	}

	private void BuffEffect()
	{
		_knightPlayerAttr.AddSpecialAttr(faithAmount);
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		faithAmount += ((Buff_FaithHalo)baseBuff).FaithAmount;
		buffIconCtrl.UpdateBuff();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的信仰光环buff效果获得提升，每回合能获得的额外信仰值为：" + faithAmount);
		}
	}

	public override string GetBuffHint()
	{
		return "<color=#27dd34ff>" + faithAmount + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return faithAmount;
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
