using System;

[Serializable]
public class BuffData
{
	public string BuffType;

	public string BuffEffectConfigForPlayer;

	public string BuffEffectConfigForEnemy;

	public string BuffAddEffectForPlayer;

	public string BuffAddEffectForEnemy;

	public bool IsDebuff;

	public bool IsNeedShow;

	public BuffRoundType BuffRoundType;

	public BuffData()
	{
		BuffEffectConfigForPlayer = (BuffEffectConfigForEnemy = (BuffAddEffectForPlayer = (BuffAddEffectForEnemy = string.Empty)));
	}
}
