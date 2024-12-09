using System;

[Serializable]
public class PlayerAttrSaveInfo
{
	public int playerMaxHealth;

	public int playerCurrentHealth;

	public int playerLevel;

	public int currentExp;

	public int nextLevelExp;

	public PlayerAttrSaveInfo(int playerLevel, int currentExp, int nextLevelExp, int playerMaxHealth, int playerCurrentHealth)
	{
		this.playerLevel = playerLevel;
		this.currentExp = currentExp;
		this.nextLevelExp = nextLevelExp;
		this.playerMaxHealth = playerMaxHealth;
		this.playerCurrentHealth = playerCurrentHealth;
	}
}
