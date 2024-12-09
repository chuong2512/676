using UnityEngine;

public abstract class EntityAttr
{
	public int MaxHealth;

	public int Health;

	public int Armor { get; protected set; }

	public int BaseArmor { get; set; }

	public void SetBaseArmor(int value)
	{
		BaseArmor = value;
		OnBaseArmorChanged();
	}

	public void AddBaseArmor(int value)
	{
		BaseArmor += value;
		OnBaseArmorChanged();
	}

	public void ReduceBaseArmor(int value)
	{
		BaseArmor -= value;
		OnBaseArmorChanged();
	}

	protected virtual void OnBaseArmorChanged()
	{
	}

	public void AddArmor(int value)
	{
		if (value != 0)
		{
			Armor += value;
			OnArmorChanged(isAdd: true);
		}
	}

	public int ReduceArmor(int value)
	{
		if (value == 0)
		{
			return 0;
		}
		if (Armor > 0)
		{
			int result = 0;
			if (Armor >= value)
			{
				Armor -= value;
			}
			else
			{
				result = value - Armor;
				Armor = 0;
			}
			OnArmorChanged(isAdd: false);
			return result;
		}
		return value;
	}

	protected virtual void OnArmorChanged(bool isAdd)
	{
	}

	public void RecoveryHealth(int value)
	{
		if (value != 0)
		{
			Health = Mathf.Min(Health + value, MaxHealth);
			OnHealthChanged(isAdd: true);
		}
	}

	public void ReduceHealth(int value)
	{
		if (value != 0)
		{
			Health -= value;
			OnHealthChanged(isAdd: false);
		}
	}

	public void VarifyMaxHealth(int value)
	{
		if (value != 0)
		{
			MaxHealth += value;
			if (MaxHealth <= 0)
			{
				MaxHealth = 1;
			}
			if (value > 0)
			{
				Health += value;
			}
			else if (Health > MaxHealth)
			{
				Health = MaxHealth;
			}
			OnHealthChanged(value > 0);
		}
	}

	protected virtual void OnHealthChanged(bool isAdd)
	{
	}
}
