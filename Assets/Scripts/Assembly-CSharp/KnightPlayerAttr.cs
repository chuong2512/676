using UnityEngine;

public class KnightPlayerAttr : PlayerAttr
{
	public int Faith;

	private int extraBlock;

	public override int SpecialAttr => Faith;

	public override int DefenceAttr => Block;

	public override int BaseDefenceAttr => BaseBlock;

	public int Block { get; protected set; }

	public int BaseBlock { get; set; }

	public KnightPlayerAttr(Player player, OccupationInitSetting initSetting)
		: base(player)
	{
		MaxHealth = (Health = initSetting.MaxHealth);
	}

	protected override void OnStartBattle()
	{
		base.OnStartBattle();
		Block = BaseBlock;
		Faith = 0;
		extraBlock = 0;
	}

	protected override void OnEndBattle()
	{
		base.OnEndBattle();
		Faith = 0;
	}

	public override void AddSpecialAttr(int amount)
	{
		if (amount != 0 && !isLockingSpecialAttr)
		{
			Faith += amount;
			Faith = Mathf.Clamp(Faith, 0, 9999);
			Singleton<BattleEffectManager>.Instance.HandleUsualEffectConfig("Usual_GetFaith_EffectConfig", null, null, delegate
			{
				EventManager.BroadcastEvent(EventEnum.E_UpdateSpecialAttr, null);
				EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
				BattleUI obj = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
				obj.UpdatePlayerSpecialAttr(SpecialAttrShowStr());
				obj.SetSpecialAttrSprite(isHighlight: true);
			});
		}
	}

	public override string SpecialAttrShowStr()
	{
		return $"× {Faith}";
	}

	public override void ComsumeSpecialAttr(int amount)
	{
		if (amount == 0)
		{
			return;
		}
		if (amount > Faith)
		{
			Debug.LogError("The faith you wanna cost is out of range...");
		}
		Faith -= amount;
		Singleton<BattleEffectManager>.Instance.HandleUsualEffectConfig("Usual_ComsumeFaith_EffectConfig", null, null, delegate
		{
			EventManager.BroadcastEvent(EventEnum.E_UpdateSpecialAttr, null);
			EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
			EventManager.BroadcastEvent(EventEnum.E_ComsumeSpecialAttr, new SimpleEventData
			{
				intValue = amount
			});
			BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
			battleUI.UpdatePlayerSpecialAttr(SpecialAttrShowStr());
			if (Faith == 0)
			{
				battleUI.SetSpecialAttrSprite(isHighlight: false);
			}
		});
	}

	public void SetBaseBlock(int value)
	{
		BaseBlock = value;
		OnBaseBlockChanged();
	}

	public override void AddBaseDefenceAttr(int value)
	{
		BaseBlock += value;
		OnBaseBlockChanged();
	}

	public override void ReduceBaseDefenceAttra(int value)
	{
		BaseBlock -= value;
		OnBaseBlockChanged();
	}

	public void AddBlock(int value)
	{
		if (value != 0)
		{
			Block += value;
			OnBlockChanged();
		}
	}

	public void ReduceBlock(int value)
	{
		if (value != 0)
		{
			Block -= value;
			OnBlockChanged();
		}
	}

	public void AddExtraBlock(int value)
	{
		Block += value;
		extraBlock += value;
		OnExtraBlockChanged();
	}

	public void ReduceExtraBlock(int value)
	{
		Block -= value;
		extraBlock -= value;
		OnExtraBlockChanged();
	}

	private void OnExtraBlockChanged()
	{
		((KnightBlockCtrl)(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).PlayerDefenceAttrCtrl).UpdatePlayerBaseBlock(extraBlock + BaseBlock);
		EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
	}

	private void OnBlockChanged()
	{
		((KnightBlockCtrl)(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).PlayerDefenceAttrCtrl).UpdatePlayerStableBlock(Block - BaseBlock - extraBlock);
		EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
	}

	protected void OnBaseBlockChanged()
	{
		CharacterInfoUI characterInfoUI = SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI;
		if (characterInfoUI != null)
		{
			characterInfoUI.SetDefenceAttrAmount(BaseBlock);
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddSystemReportContent($"当前格挡发生变化，当前基础格挡: {BaseBlock}");
		}
	}
}
