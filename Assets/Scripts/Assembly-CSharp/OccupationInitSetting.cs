using System;
using System.Collections.Generic;

[Serializable]
public struct OccupationInitSetting
{
	public string PlayerOccupation;

	public int MaxHealth;

	public string InitHelmet;

	public string InitBreastplate;

	public string InitGlove;

	public string InitShoes;

	public string InitTrinket;

	public string InitMainhand;

	public string InitSuphand;

	public List<CardConfig> InitMainhandArray;

	public List<CardConfig> InitSuphandArray;

	public string[] InitSkillArray;

	public int InitMoney;
}
