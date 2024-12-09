public abstract class PlayerLogicHandler
{
	protected Player player;

	public PlayerLogicHandler(Player player)
	{
		this.player = player;
	}

	public abstract void PlayerUseASkillCard(SkillCard card);

	public abstract void OnPlayerStoringForce();

	public abstract void OnBattleStart();

	public abstract bool IsCanOpenHidingStage();

	public abstract void HandleRoomInfoForHiddenStage();

	public abstract int CheckAllBuffInfluenceToDamage(int finalDmg, bool isAbsDmg, EntityBase caster, ref string takeDmgDes);

	protected void PlayerConsumeApAmountBySkill(SkillCard card)
	{
		if (card.ApCost > 0)
		{
			player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardApCostReduce, (BattleEffectData)new SimpleEffectData
			{
				strData = card.CardCode
			}, out int IntData);
			int amount = card.ApCost - IntData;
			BaseBuff specificBuff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(player, BuffType.Buff_Agile);
			if (!specificBuff.IsNull())
			{
				amount = 0;
				specificBuff.TakeEffect(player);
			}
			player.PlayerAttr.ComsumeApAmount(amount);
		}
	}

	protected void PlayerConsumeHandCardBySkill(SkillCard card)
	{
		if (!card.MainHandCardCode.IsNullOrEmpty())
		{
			player.PlayerBattleInfo.ComsumeMainHandCards(card.MainHandCardCode, card.MainHandCardConsumeAmount, isDrop: false);
		}
		if (!card.SupHandCardCode.IsNullOrEmpty())
		{
			player.PlayerBattleInfo.ComsumeSupHandCards(card.SupHandCardCode, card.SupHandCardConsumeAmount, isDrop: false);
		}
	}

	protected static void ReportPlayerCastSkill(SkillCard card)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent("玩家使用技能：" + card.CardName);
		}
	}

	protected static void BroadcastPlayerCastSkillEvent(SkillCard card)
	{
		EventManager.BroadcastEvent(EventEnum.E_PlayerUseSkillCard, new SimpleEventData
		{
			stringValue = card.CardCode
		});
	}
}
