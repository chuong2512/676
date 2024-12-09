using System.Collections;

public class EquipCard_E_Hands_23 : EquipCard_E_Hands
{
	public EquipCard_E_Hands_23(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
	}

	private void OnEnemyDead(EventData data)
	{
		Singleton<GameManager>.Instance.StartCoroutine(Effect_IE());
	}

	private IEnumerator Effect_IE()
	{
		yield return null;
		((ArcherPlayerAttr)Singleton<GameManager>.Instance.Player.PlayerAttr).AddSpecialArrow(new Arrow[1]
		{
			new PoisonArrow()
		});
		EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
	}

	private void OnBattleStart(EventData data)
	{
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).AddEquipEffect(this, GetEquipEffectHint(), GetEquipEffectDes).SetEffect();
	}

	private string GetEquipEffectHint()
	{
		return string.Empty;
	}

	private string GetEquipEffectDes()
	{
		return equipmentCardAttr.EquipEffectDesKey.LocalizeText();
	}
}
