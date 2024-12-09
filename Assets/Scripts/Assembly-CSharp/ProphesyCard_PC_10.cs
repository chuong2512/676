using System.Collections.Generic;
using UnityEngine;

public class ProphesyCard_PC_10 : ProphesyCard
{
	public override string ProphesyCode => "PC_10";

	public override void Active(bool isLoad)
	{
		if (!isLoad)
		{
			List<string> list = AllRandomInventory.Instance.AllStatisfiedConditionSkills(1, Singleton<GameManager>.Instance.Player.PlayerOccupation);
			string skillCode = list[Random.Range(0, list.Count)];
			Singleton<GameManager>.Instance.Player.PlayerInventory.AddSkill(skillCode, isNew: true);
		}
	}
}
