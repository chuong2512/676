using UnityEngine;

public class ArcherLogicHandler : PlayerLogicHandler
{
	private ArcherPlayerAttr archerPlayerAttr;

	public ArcherLogicHandler(Player player, PlayerAttr playerAttr)
		: base(player)
	{
		archerPlayerAttr = (ArcherPlayerAttr)playerAttr;
	}

	public override void PlayerUseASkillCard(SkillCard card)
	{
		PlayerLogicHandler.ReportPlayerCastSkill(card);
		PlayerConsumeHandCardBySkill(card);
		card.SkillCardEffect(player, null);
		PlayerLogicHandler.BroadcastPlayerCastSkillEvent(card);
		PlayerConsumeApAmountBySkill(card);
	}

	public override void OnPlayerStoringForce()
	{
	}

	private void HandleStageEvent(int level, int layer, string eventCode)
	{
		RoomInfo roomInfo = RoomManager.Instance.GetRoomInfo(level, layer, isAutoDelete: false);
		Vector2Int randomBlockPos = roomInfo.GetRandomBlockPos();
		roomInfo.SetEventCode(randomBlockPos, eventCode);
	}

	public override void OnBattleStart()
	{
		player.GetBuff(new Buff_ArrowSupplement(player, 1));
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).SetPlayerNormalHeapPortrait();
	}

	public override bool IsCanOpenHidingStage()
	{
		return player.IsPlayerHaveEquip("E_Mainhand_25");
	}

	public override void HandleRoomInfoForHiddenStage()
	{
		HandleStageEvent(3, 1, "Event_47");
		HandleStageEvent(3, 2, "Event_49");
	}

	public override int CheckAllBuffInfluenceToDamage(int finalDmg, bool isAbsDmg, EntityBase caster, ref string takeDmgDes)
	{
		finalDmg = EntityBase.CheckHolyProtectBuff(player, finalDmg, ref takeDmgDes);
		finalDmg = EntityBase.CheckArmorBrokenBuff(player, finalDmg, ref takeDmgDes);
		finalDmg = CheckCoverBuff(finalDmg, ref takeDmgDes);
		finalDmg = EntityBase.CheckShadowEscapeBuff(player, finalDmg, ref takeDmgDes);
		finalDmg = EntityBase.CheckHoldPositionBuff(player, finalDmg, ref takeDmgDes);
		return finalDmg;
	}

	protected int CheckCoverBuff(int finalDmg, ref string takeDmgDes)
	{
		Buff_Cover buff_Cover;
		if ((buff_Cover = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(player, BuffType.Buff_Cover) as Buff_Cover) != null)
		{
			finalDmg -= buff_Cover.DmgReduceAmount;
			finalDmg = Mathf.Clamp(finalDmg, 0, int.MaxValue);
			buff_Cover.TakeEffect(player);
			takeDmgDes += $",{player.EntityName}因为掩护buff，伤害减少，剩余伤害为{finalDmg}";
		}
		return finalDmg;
	}
}
