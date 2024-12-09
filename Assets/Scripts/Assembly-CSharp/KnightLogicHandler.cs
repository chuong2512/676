using UnityEngine;

public class KnightLogicHandler : PlayerLogicHandler
{
	private KnightPlayerAttr knightPlayerAttr;

	public KnightLogicHandler(Player player, PlayerAttr playerAttr)
		: base(player)
	{
		knightPlayerAttr = (KnightPlayerAttr)playerAttr;
	}

	public override void OnBattleStart()
	{
		player.GetBuff(new Buff_StrongFaith(player, int.MaxValue));
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).SetPlayerNormalHeapPortrait();
	}

	public override bool IsCanOpenHidingStage()
	{
		if (player.IsPlayerHaveEquip("E_Offhand_13"))
		{
			return player.IsPlayerHaveEquip("E_Mainhand_13");
		}
		return false;
	}

	public override void HandleRoomInfoForHiddenStage()
	{
		if (Random.value > 0.5f)
		{
			HandleStageEvent(3, 1, "Event_41");
			HandleStageEvent(3, 2, "Event_43");
		}
		else
		{
			HandleStageEvent(3, 1, "Event_43");
			HandleStageEvent(3, 2, "Event_41");
		}
	}

	private void HandleStageEvent(int level, int layer, string eventCode)
	{
		RoomInfo roomInfo = RoomManager.Instance.GetRoomInfo(level, layer, isAutoDelete: false);
		Vector2Int randomBlockPos = roomInfo.GetRandomBlockPos();
		roomInfo.SetEventCode(randomBlockPos, eventCode);
	}

	public override void PlayerUseASkillCard(SkillCard card)
	{
		PlayerLogicHandler.ReportPlayerCastSkill(card);
		PlayerConsumeHandCardBySkill(card);
		card.SkillCardEffect(player, delegate
		{
			player.TryRemoveDefenceBuff(card.IsWillBreakDefence);
		});
		PlayerLogicHandler.BroadcastPlayerCastSkillEvent(card);
		PlayerConsumeApAmountBySkill(card);
		KnightConsumeFaithBySkill(card);
	}

	public override void OnPlayerStoringForce()
	{
		BaseBuff specificBuff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(player, BuffType.Buff_Defence);
		if (!specificBuff.IsNull() && !Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(player, BuffType.Buff_DefenceProtect))
		{
			player.RemoveBuff(specificBuff);
		}
		if (!Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(player, BuffType.Buff_FaithProtect))
		{
			knightPlayerAttr.ComsumeSpecialAttr(knightPlayerAttr.Faith);
		}
	}

	public override int CheckAllBuffInfluenceToDamage(int finalDmg, bool isAbsDmg, EntityBase caster, ref string takeDmgDes)
	{
		finalDmg = EntityBase.CheckHolyProtectBuff(player, finalDmg, ref takeDmgDes);
		finalDmg = EntityBase.CheckArmorBrokenBuff(player, finalDmg, ref takeDmgDes);
		finalDmg = CheckDefenceBuff(finalDmg, caster, ref takeDmgDes);
		finalDmg = EntityBase.CheckShadowEscapeBuff(player, finalDmg, ref takeDmgDes);
		finalDmg = EntityBase.CheckHoldPositionBuff(player, finalDmg, ref takeDmgDes);
		return finalDmg;
	}

	protected int CheckDefenceBuff(int finalDmg, EntityBase caster, ref string takeDmgDes)
	{
		if (Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(player, BuffType.Buff_Defence))
		{
			finalDmg = Mathf.Max(0, finalDmg - knightPlayerAttr.Block);
			takeDmgDes += $",{player.EntityName}当前处于防御状态，伤害受到格挡，剩余伤害为{finalDmg}";
			OnBlockDmg(caster);
		}
		return finalDmg;
	}

	protected static void OnBlockDmg(EntityBase caster)
	{
		EventManager.BroadcastEvent(EventEnum.E_PlayerBlockDmg, new SimpleEventData
		{
			objValue = caster
		});
	}

	private void KnightConsumeFaithBySkill(SkillCard card)
	{
		if (card.SpecialAttrCost > 0)
		{
			knightPlayerAttr.ComsumeSpecialAttr(card.SpecialAttrCost);
		}
	}
}
