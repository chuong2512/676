using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArcherPlayerAttr : PlayerAttr
{
	private int Arrow;

	private int preArrow;

	private int MaxArrow;

	private List<Arrow> allSpecialArrows;

	public override int SpecialAttr => Arrow;

	public int PreArrow => preArrow;

	public override int DefenceAttr => MaxArrow;

	public override int BaseDefenceAttr => MaxArrow;

	public List<Arrow> AllSpecialArrows => allSpecialArrows;

	public bool IsFullArrow => MaxArrow == Arrow;

	public ArcherPlayerAttr(Player player, int MaxHealth)
		: base(player)
	{
		base.MaxHealth = (Health = MaxHealth);
		allSpecialArrows = new List<Arrow>();
	}

	public ArcherPlayerAttr(Player player, OccupationInitSetting initSetting)
		: base(player)
	{
		MaxHealth = (Health = initSetting.MaxHealth);
		allSpecialArrows = new List<Arrow>();
	}

	protected override void OnStartBattle()
	{
		base.OnStartBattle();
		Arrow = (preArrow = MaxArrow);
		allSpecialArrows.Clear();
	}

	public override void AddSpecialAttr(int amount)
	{
		if (amount != 0 && Arrow != MaxArrow)
		{
			int realAmount = Mathf.Min(amount, MaxArrow - Arrow);
			Arrow += realAmount;
			preArrow += realAmount;
			Singleton<BattleEffectManager>.Instance.HandleUsualEffectConfig("Usual_GetArrow_EffectConfig", null, null, delegate
			{
				EventManager.BroadcastEvent(EventEnum.E_AddSpecialAttr, new SimpleEventData
				{
					intValue = realAmount
				});
				EventManager.BroadcastEvent(EventEnum.E_UpdateSpecialAttr, null);
				EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
				BattleUI obj = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
				obj.UpdatePlayerSpecialAttr(SpecialAttrShowStr());
				obj.SetSpecialAttrSprite(isHighlight: true);
				((ArcherArrowCtrl)obj.PlayerDefenceAttrCtrl).AddNormalArrow();
			});
		}
	}

	public override string SpecialAttrShowStr()
	{
		return $"{preArrow}/{MaxArrow}";
	}

	public override void ComsumeSpecialAttr(int amount)
	{
		ComsumeSpecialAttrNotUpdate(amount);
		ComsumeSpecialAttr(null, amount, isDrop: true);
	}

	public void ComsumeSpecialAttr(EntityBase[] targets, int amount, bool isDrop)
	{
		if (amount == 0)
		{
			return;
		}
		preArrow -= amount;
		ReduceSpecialArrow(amount);
		Singleton<BattleEffectManager>.Instance.HandleUsualEffectConfig("Usual_ComsumeArrow_EffectConfig", null, null, delegate
		{
			EventManager.BroadcastEvent(EventEnum.E_ComsumeSpecialAttr, new SimpleEventData
			{
				intValue = amount,
				boolValue = isDrop,
				objValue = targets
			});
			BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
			battleUI.UpdatePlayerSpecialAttr(SpecialAttrShowStr());
			((ArcherArrowCtrl)battleUI.PlayerDefenceAttrCtrl).ComsumeArrow(amount);
			if (Arrow == 0)
			{
				battleUI.SetSpecialAttrSprite(isHighlight: false);
			}
		});
	}

	public void ComsumeSpecialAttrNotUpdate(int amount)
	{
		Arrow -= amount;
		EventManager.BroadcastEvent(EventEnum.E_UpdateSpecialAttr, null);
		EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
	}

	public override void AddBaseDefenceAttr(int value)
	{
		MaxArrow += value;
		OnMaxArrowChanged();
	}

	public override void ReduceBaseDefenceAttra(int value)
	{
		MaxArrow -= value;
		OnMaxArrowChanged();
	}

	public void SetMaxArrow(int value)
	{
		MaxArrow = value;
	}

	protected void OnMaxArrowChanged()
	{
		CharacterInfoUI characterInfoUI = SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI;
		if (characterInfoUI != null)
		{
			characterInfoUI.SetDefenceAttrAmount(MaxArrow);
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddSystemReportContent($"当前格挡发生变化，当前基础格挡: {base.BaseArmor}");
		}
	}

	public int GetSpecificSpecialArrowAmount(Arrow.ArrowType arrowType)
	{
		int num = 0;
		for (int i = 0; i < allSpecialArrows.Count; i++)
		{
			if (allSpecialArrows[i].MArrowType == arrowType)
			{
				num++;
			}
		}
		return num;
	}

	public void AddRandomSpecialArrows(int amount, bool isNeedReplacePreEffect)
	{
		List<Type> list = (from t in typeof(Arrow).Assembly.GetTypes()
			where typeof(Arrow).IsAssignableFrom(t)
			where !t.IsAbstract && t.IsClass
			select t).ToList();
		Arrow[] array = new Arrow[amount];
		for (int i = 0; i < array.Length; i++)
		{
			Type type = list[UnityEngine.Random.Range(0, list.Count)];
			array[i] = (Arrow)Activator.CreateInstance(type);
		}
		AddSpecialArrow(array, isNeedReplacePreEffect);
	}

	public void AddSpecialArrow(Arrow[] arrows, bool isNeedReplacePreEffect = true)
	{
		ArcherArrowCtrl archerArrowCtrl = (ArcherArrowCtrl)(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).PlayerDefenceAttrCtrl;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < arrows.Length; i++)
		{
			if (i + 1 <= allSpecialArrows.Count)
			{
				if (isNeedReplacePreEffect)
				{
					allSpecialArrows[i] = arrows[i];
					allSpecialArrows[i].OnAddArrow(archerArrowCtrl.ArrowCtrls[i].transform);
				}
				else
				{
					num2++;
				}
				num++;
			}
			else if (i + 1 <= Arrow)
			{
				allSpecialArrows.Add(arrows[i]);
				allSpecialArrows[i].OnAddArrow(archerArrowCtrl.ArrowCtrls[i].transform);
				num++;
			}
		}
		archerArrowCtrl.SetSpecialArrow(num2, num2 + num);
		EventManager.BroadcastEvent(EventEnum.E_AddSpecialArrow, new SimpleEventData
		{
			intValue = num
		});
	}

	public void ActiveSpecialArrow(EntityBase[] targets)
	{
		if (allSpecialArrows.Count > 0)
		{
			allSpecialArrows[0].ArrowEffect(targets);
		}
	}

	private void ReduceSpecialArrow(int amount)
	{
		if (allSpecialArrows.Count > 0)
		{
			int count = ((allSpecialArrows.Count >= amount) ? amount : allSpecialArrows.Count);
			allSpecialArrows.RemoveRange(0, count);
		}
	}
}
