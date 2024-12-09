using System;

public class Buff_Baptism : BaseBuff
{
	private int faithAmount;

	private KnightPlayerAttr _knightPlayerAttr;

	public int FaithAmount => faithAmount;

	public override BuffType BuffType => BuffType.Buff_Baptism;

	public Buff_Baptism(Player player, int round)
		: base(player, round)
	{
		if (player.PlayerOccupation != PlayerOccupation.Knight)
		{
			throw new Exception("Add wrong buff to player, player occupation is not match");
		}
		faithAmount = round;
		_knightPlayerAttr = (KnightPlayerAttr)player.PlayerAttr;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: true, BuffType), buffIconCtrl.transform, null, BuffEffect);
		buffIconCtrl.BuffEffectHint();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的洗礼buff效果触发，buff获得信仰为：" + faithAmount);
		}
	}

	private void BuffEffect()
	{
		_knightPlayerAttr.AddSpecialAttr(faithAmount);
		entityBase.RemoveBuff(this);
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		faithAmount += ((Buff_Baptism)baseBuff).FaithAmount;
		buffIconCtrl.UpdateBuff();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的洗礼buff的效果提升，当前能获得信仰的层数为：" + faithAmount);
		}
	}

	public override string GetBuffHint()
	{
		return "<color=#e9e9e9ff>" + faithAmount + "</color>";
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
