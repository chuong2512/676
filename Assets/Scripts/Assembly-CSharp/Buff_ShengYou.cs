using System;

public class Buff_ShengYou : BaseBuff
{
	private int faithAddAmount;

	private KnightPlayerAttr _knightPlayerAttr;

	public int FaithAddAmount => faithAddAmount;

	public override BuffType BuffType => BuffType.Buff_ShengYou;

	public Buff_ShengYou(Player player, int faithAddAmount)
		: base(player, int.MaxValue)
	{
		if (player.PlayerOccupation != PlayerOccupation.Knight)
		{
			throw new Exception("Add wrong buff to player, player occupation is not match");
		}
		this.faithAddAmount = faithAddAmount;
		_knightPlayerAttr = (KnightPlayerAttr)player.PlayerAttr;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的圣佑buff效果触发，获得信仰：" + faithAddAmount);
		}
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: true, BuffType), buffIconCtrl.transform, null, BuffEffect);
		buffIconCtrl.BuffEffectHint();
	}

	private void BuffEffect()
	{
		_knightPlayerAttr.AddSpecialAttr(faithAddAmount);
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		faithAddAmount += ((Buff_ShengYou)baseBuff).FaithAddAmount;
		buffIconCtrl.UpdateBuff();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的圣佑buff效果提升，每次格挡反击将会获得的信仰值为：" + faithAddAmount);
		}
	}

	public override string GetBuffHint()
	{
		return "<color=#e9e9e9ff>" + faithAddAmount + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return faithAddAmount;
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		EventManager.RegisterEvent(EventEnum.E_PlayerBlockDmg, OnPlayerBlockDmg);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		EventManager.UnregisterEvent(EventEnum.E_PlayerBlockDmg, OnPlayerBlockDmg);
	}

	private void OnPlayerBlockDmg(EventData data)
	{
		TakeEffect(entityBase);
	}
}
