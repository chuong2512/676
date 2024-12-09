using System.Collections.Generic;
using UnityEngine;

public class LevelUp_ProphesyCard_14 : PlayerLevelUpEffect
{
	protected override string IconName => "获得1张随机基础牌";

	public override string NameKey => "LevelUpEffect_PC14_NameKey";

	public override string DesKey => "LevelUpEffect_PC14_DesKey";

	public override void Effect()
	{
		List<string> list = AllRandomInventory.Instance.AllStatisfiedSpecialUsualCards(1);
		string cardCode = list[Random.Range(0, list.Count)];
		Singleton<GameManager>.Instance.Player.PlayerInventory.AddSpecialUsualCards(cardCode, 1, isNew: true);
		(SingletonDontDestroy<UIManager>.Instance.ShowView("EventResultUI") as EventResultUI).ShowCard(cardCode);
	}
}
