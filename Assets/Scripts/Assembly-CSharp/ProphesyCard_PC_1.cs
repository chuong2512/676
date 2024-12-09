using System.Collections.Generic;
using UnityEngine;

public class ProphesyCard_PC_1 : ProphesyCard
{
	public override string ProphesyCode => "PC_1";

	public override void Active(bool isLoad)
	{
		if (!isLoad)
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(20);
			List<string> list = AllRandomInventory.Instance.AllStatisfiedSpecialUsualCards(1);
			string cardCode = list[Random.Range(0, list.Count)];
			Singleton<GameManager>.Instance.Player.PlayerInventory.AddSpecialUsualCards(cardCode, 1, isNew: true);
		}
	}
}
