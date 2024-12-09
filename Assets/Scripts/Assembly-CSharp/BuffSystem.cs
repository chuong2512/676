using System.Collections.Generic;

public class BuffSystem
{
	private Dictionary<EntityBase, List<BaseBuff>> allEntityBuffContainer = new Dictionary<EntityBase, List<BaseBuff>>();

	public void ClearAllBuff()
	{
		allEntityBuffContainer.Clear();
	}

	public void GetBuff(EntityBase entityBase, BaseBuff baseBuff)
	{
		List<BaseBuff> value = null;
		if (allEntityBuffContainer.TryGetValue(entityBase, out value))
		{
			for (int i = 0; i < value.Count; i++)
			{
				if (value[i].BuffType == baseBuff.BuffType)
				{
					value[i].HandleSameBuffAdd(baseBuff);
					if (IsEntityGetBuff(entityBase, baseBuff.BuffType))
					{
						entityBase.UpdateBuffIcon(value[i]);
					}
					return;
				}
			}
			value.Add(baseBuff);
			baseBuff.HandleNewBuffAdd();
			entityBase.AddBuffIcon(baseBuff);
		}
		else
		{
			value = new List<BaseBuff> { baseBuff };
			baseBuff.HandleNewBuffAdd();
			allEntityBuffContainer[entityBase] = value;
			entityBase.AddBuffIcon(baseBuff);
		}
	}

	public void RemoveBuff(EntityBase entityBase, BaseBuff buff)
	{
		List<BaseBuff> value = null;
		if (allEntityBuffContainer.TryGetValue(entityBase, out value))
		{
			value.Remove(buff);
			buff.HandleBuffRemove();
			entityBase.RemoveBuffIcon(buff);
		}
	}

	public void RemoveBuff(EntityBase entityBase, BuffType type)
	{
		if (!allEntityBuffContainer.TryGetValue(entityBase, out var value))
		{
			return;
		}
		for (int i = 0; i < value.Count; i++)
		{
			if (value[i].BuffType == type)
			{
				value[i].HandleBuffRemove();
				value.RemoveAt(i);
				i--;
			}
		}
	}

	public void RemoveBuff(EntityBase entityBase)
	{
		if (allEntityBuffContainer.TryGetValue(entityBase, out var value))
		{
			for (int i = 0; i < value.Count; i++)
			{
				value[i].HandleBuffRemove();
			}
			allEntityBuffContainer.Remove(entityBase);
		}
	}

	public bool IsEntityGetBuff(EntityBase entityBase, BuffType type)
	{
		if (entityBase == null)
		{
			return false;
		}
		List<BaseBuff> value = null;
		if (allEntityBuffContainer.TryGetValue(entityBase, out value))
		{
			for (int i = 0; i < value.Count; i++)
			{
				if (value[i].BuffType == type)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	public BaseBuff GetSpecificBuff(EntityBase entityBase, BuffType type)
	{
		List<BaseBuff> value = null;
		if (allEntityBuffContainer.TryGetValue(entityBase, out value))
		{
			for (int i = 0; i < value.Count; i++)
			{
				if (value[i].BuffType == type)
				{
					return value[i];
				}
			}
		}
		return null;
	}

	public BaseBuff[] GetAllBuff(EntityBase entityBase)
	{
		if (!allEntityBuffContainer.TryGetValue(entityBase, out var value))
		{
			return null;
		}
		return value.ToArray();
	}

	public List<BaseBuff> GetAllDebuff(EntityBase entityBase)
	{
		if (allEntityBuffContainer.TryGetValue(entityBase, out var value))
		{
			List<BaseBuff> list = new List<BaseBuff>();
			for (int i = 0; i < value.Count; i++)
			{
				if (value[i].IsDebuff)
				{
					list.Add(value[i]);
				}
			}
			return list;
		}
		return null;
	}

	public void OnTurnRound()
	{
		foreach (KeyValuePair<EntityBase, List<BaseBuff>> item in allEntityBuffContainer)
		{
			List<BaseBuff> list = new List<BaseBuff>(item.Value);
			for (int i = 0; i < list.Count; i++)
			{
				list[i].UpdateRoundTurn();
			}
		}
	}

	public void OnBattleEnd()
	{
		foreach (KeyValuePair<EntityBase, List<BaseBuff>> item in allEntityBuffContainer)
		{
			for (int i = 0; i < item.Value.Count; i++)
			{
				item.Value[i].HandleBuffRemove();
			}
		}
		allEntityBuffContainer.Clear();
	}
}
