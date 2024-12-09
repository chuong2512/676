using System.Collections.Generic;
using UnityEngine;

public class ProphesyCard_PC_4 : ProphesyCard
{
	public override string ProphesyCode => "PC_4";

	public override void Active(bool isLoad)
	{
		if (!isLoad)
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.ReduceHealth(10);
			List<string> list = AllRandomInventory.Instance.AllSatisfiedEquipsWithStageLimit(2, 1);
			string equipCode = list[Random.Range(0, list.Count)];
			Singleton<GameManager>.Instance.Player.PlayerInventory.AddEquipment(equipCode);
		}
	}
}
