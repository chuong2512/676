using System.Collections.Generic;
using UnityEngine;

public class Buff_PunishmentHalo : BaseBuff
{
	private int dmg;

	private List<EnemyBase> targetEnemies;

	public int Damage => dmg;

	public override BuffType BuffType => BuffType.Buff_PunishmentHalo;

	public Buff_PunishmentHalo(EntityBase entityBase, int dmg)
		: base(entityBase, int.MaxValue)
	{
		this.dmg = dmg;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的惩戒光环buff效果触发，所有敌人受到的真实伤害为：" + dmg);
		}
		targetEnemies = new List<EnemyBase>(Singleton<EnemyController>.Instance.AllEnemies);
		Transform[] array = new Transform[targetEnemies.Count];
		for (int i = 0; i < targetEnemies.Count; i++)
		{
			array[i] = targetEnemies[i].EnemyCtrl.transform;
		}
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: true, BuffType), buffIconCtrl.transform, array, BuffEffect);
		buffIconCtrl.BuffEffectHint();
	}

	private void BuffEffect()
	{
		for (int i = 0; i < targetEnemies.Count; i++)
		{
			BaseBuff.AtkEntity(targetEnemies[i], dmg, isAbsDmg: true);
		}
		targetEnemies.Clear();
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		dmg += ((Buff_PunishmentHalo)baseBuff).Damage;
		buffIconCtrl.UpdateBuff();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的惩戒光环buff效果获得提升，每回合能对所有的敌人造成的真实伤害为：" + dmg);
		}
	}

	public override string GetBuffHint()
	{
		return "<color=#27dd34ff>" + dmg + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return dmg;
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
