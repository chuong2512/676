public class Buff_Power : BaseBuff
{
	private int power;

	private bool isDebuff;

	public int Power => power;

	public override BuffType BuffType => BuffType.Buff_Power;

	public override bool IsDebuff => isDebuff;

	public Buff_Power(EntityBase entityBase, int power)
		: base(entityBase, int.MaxValue)
	{
		this.power = power;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		int num = ((Buff_Power)baseBuff).Power;
		power += num;
		if (num > 0)
		{
			BaseBuff.ShowBuffHint(entityBase.EntityTransform, !isPlayer, entityBase.BuffHintScale, BuffType, isDebuff: false);
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的力量发生变化，当前的力量值为：" + power);
		}
		if (num > 0)
		{
			Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadAddConfig(isPlayer, BuffType), entityBase.EntityTransform, null, null);
		}
		if (power == 0)
		{
			entityBase.RemoveBuff(this);
			return;
		}
		if (power > 0)
		{
			isDebuff = false;
		}
		else if (power < 0)
		{
			isDebuff = true;
		}
		buffIconCtrl.UpdateBuff();
		EventManager.BroadcastEvent(entityBase, EventEnum.E_GetSameBuff, new BuffEventData
		{
			buffType = BuffType
		});
	}

	public override void HandleNewBuffAdd()
	{
		if (power > 0)
		{
			isDebuff = false;
			BaseBuff.ShowBuffHint(entityBase.EntityTransform, !isPlayer, entityBase.BuffHintScale, BuffType, isDebuff: false);
			Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadAddConfig(isPlayer, BuffType), entityBase.EntityTransform, null, null);
		}
		else if (power < 0)
		{
			isDebuff = true;
		}
		EventManager.BroadcastEvent(entityBase, EventEnum.E_GetNewBuff, new SimpleEventData
		{
			intValue = (int)BuffType
		});
	}

	public override string GetBuffHint()
	{
		return ((power > 0) ? "<color=#27dd34ff>" : "<color=#ec2125ff>") + power + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return power;
	}
}
