using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialUsualCard_BC_P_21 : SpecialUsualCard
{
	private static int CardUseTimeInBattle;

	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_21(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new AllEnemyRandomDownHandler();
		pointupHandler = new AllEnemyUpHandler();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		int baseValue = 2;
		int num = RealDmg(player);
		return string.Format(specialUsualCardAttr.DesKeyOnBattle.LocalizeText(), BaseCard.GetValueColor(baseValue, num), num, CardUseTimeInBattle + 1);
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		realDmg = RealDmg(player, out atkDes);
		SingletonDontDestroy<Game>.Instance.StartCoroutine(Effect_IE(player));
		handler?.Invoke();
	}

	private IEnumerator Effect_IE(Player player)
	{
		List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
		int i = 0;
		while (i < CardUseTimeInBattle + 1)
		{
			if (allEnemies.Count == 0)
			{
				yield break;
			}
			int i2 = i;
			EnemyBase enTarget = allEnemies[UnityEngine.Random.Range(0, allEnemies.Count)];
			UsualCard.HandleEffect(base.EffectConfig, new Transform[1] { enTarget.EnemyCtrl.transform }, delegate
			{
				Effect(player, enTarget, i2);
			});
			yield return new WaitForSeconds(0.2f);
			int num = i + 1;
			i = num;
		}
		CardUseTimeInBattle++;
		EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
	}

	private void Effect(Player player, EnemyBase entityBase, int i)
	{
		if (BaseCard.IsEntityCanDodgeAttack(entityBase, out var buff))
		{
			buff.TakeEffect(entityBase);
		}
		else
		{
			player.PlayerAtkEnemy(entityBase, realDmg, isTrueDmg: false);
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{entityBase.EntityName}造成第{i + 1}次非真实的{realDmg}点伤害({atkDes}");
		}
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, "基础伤害(2)", IntData, out var pwdBuff, out var rate);
		return BaseCard.GetRealDamage(player, 2, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		return BaseCard.GetRealDamage(player, 2, IntData);
	}

	protected override void OnEquiped()
	{
		base.OnEquiped();
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnReleased()
	{
		base.OnReleased();
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	private void OnBattleStart(EventData data)
	{
		CardUseTimeInBattle = 0;
		EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
	}
}
