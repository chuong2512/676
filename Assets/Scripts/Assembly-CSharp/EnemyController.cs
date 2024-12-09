using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyController : Singleton<EnemyController>
{
	public const int MaxEnemyAmount = 5;

	private Dictionary<string, Queue<EnemyBaseCtrl>> allEnemyPool = new Dictionary<string, Queue<EnemyBaseCtrl>>();

	private List<EnemyBase> allEnemies = new List<EnemyBase>();

	private List<Transform> allEnemyQueueTrans = new List<Transform>();

	private List<EnemyBase> dieingEnemies = new List<EnemyBase>();

	private List<bool> allEnemiesActionFlag = new List<bool>();

	private int currentActionEnemyIndex = -1;

	private bool isAddLeft;

	private string triggeringEnemyHeapCode = string.Empty;

	public List<EnemyBase> AllEnemies => allEnemies;

	protected override void Awake()
	{
		base.Awake();
		if (!(this != Singleton<EnemyController>.Instance))
		{
			EventManager.RegisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
		}
	}

	public EnemyHeapData GetEnemyHeapData(string enemyHeapData)
	{
		return DataManager.Instance.GetSpecificEnemyHeap(enemyHeapData);
	}

	public void SetTriggeringEnemyHeap(string enemyHeapCode)
	{
		triggeringEnemyHeapCode = enemyHeapCode;
	}

	private void OnBattleEnd(EventData data)
	{
		if (!triggeringEnemyHeapCode.IsNullOrEmpty())
		{
			triggeringEnemyHeapCode = string.Empty;
		}
	}

	public EnemyBase AddSpecialMonster(string monsterCode, bool actionFlag)
	{
		EnemyBaseCtrl enemyCtrl = GetEnemyCtrl(monsterCode);
		allEnemyQueueTrans.Add(enemyCtrl.transform);
		allEnemies.Add(enemyCtrl.EnemyEntity);
		allEnemiesActionFlag.Add(actionFlag);
		enemyCtrl.EnemyEntity.StartBattle();
		enemyCtrl.EnemyEntity.OnBattleReady();
		SingletonDontDestroy<Game>.Instance.CurrentUserData.TryUnlockMonsterIllu(monsterCode);
		return enemyCtrl.EnemyEntity;
	}

	public EnemyBase AddMonster(string monsterCode, bool actionFlag)
	{
		int count = allEnemies.Count;
		EnemyBaseCtrl enemyCtrl = GetEnemyCtrl(monsterCode);
		if (isAddLeft)
		{
			isAddLeft = false;
			allEnemyQueueTrans.Insert(0, enemyCtrl.transform);
		}
		else
		{
			isAddLeft = true;
			allEnemyQueueTrans.Add(enemyCtrl.transform);
		}
		allEnemies.Add(enemyCtrl.EnemyEntity);
		allEnemiesActionFlag.Add(actionFlag);
		ReAdjustMonstersPos(count);
		enemyCtrl.EnemyEntity.StartBattle();
		enemyCtrl.EnemyEntity.OnBattleReady();
		SingletonDontDestroy<Game>.Instance.CurrentUserData.TryUnlockMonsterIllu(monsterCode);
		return enemyCtrl.EnemyEntity;
	}

	public EnemyBase AddMonster(string monsterCode, bool isLeft, bool actionFlag)
	{
		int count = allEnemies.Count;
		EnemyBaseCtrl enemyCtrl = GetEnemyCtrl(monsterCode);
		if (isLeft)
		{
			allEnemyQueueTrans.Insert(0, enemyCtrl.transform);
		}
		else
		{
			allEnemyQueueTrans.Add(enemyCtrl.transform);
		}
		allEnemies.Add(enemyCtrl.EnemyEntity);
		allEnemiesActionFlag.Add(actionFlag);
		ReAdjustMonstersPos(count);
		enemyCtrl.EnemyEntity.StartBattle();
		enemyCtrl.EnemyEntity.OnBattleReady();
		SingletonDontDestroy<Game>.Instance.CurrentUserData.TryUnlockMonsterIllu(monsterCode);
		return enemyCtrl.EnemyEntity;
	}

	public List<EnemyBase> SummorMonster(List<string> monsterCode, List<bool> actionFlag, Action EndAction)
	{
		int count = allEnemies.Count;
		int num = 0;
		List<EnemyBase> enemyList = new List<EnemyBase>(monsterCode.Count);
		for (int i = 0; i < monsterCode.Count; i++)
		{
			EnemyBaseCtrl enemyCtrl = GetEnemyCtrl(monsterCode[i]);
			if (isAddLeft)
			{
				num++;
				isAddLeft = false;
				allEnemyQueueTrans.Insert(0, enemyCtrl.transform);
			}
			else
			{
				isAddLeft = true;
				allEnemyQueueTrans.Add(enemyCtrl.transform);
			}
			enemyList.Add(enemyCtrl.EnemyEntity);
			allEnemies.Add(enemyCtrl.EnemyEntity);
			allEnemiesActionFlag.Add(actionFlag[i]);
			enemyCtrl.gameObject.SetActive(value: false);
		}
		SummorMonsterReadjustPos(count, num, delegate
		{
			for (int j = 0; j < enemyList.Count; j++)
			{
				enemyList[j].EnemyCtrl.gameObject.SetActive(value: true);
				enemyList[j].StartBattle();
				enemyList[j].OnBattleReady();
				VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("SummorSmoke");
				vfxBase.transform.position = enemyList[j].EnemyCtrl.ShadowTrans.position;
				vfxBase.Play();
			}
			EndAction?.Invoke();
		});
		SingletonDontDestroy<Game>.Instance.CurrentUserData.TryUnlockMonsterIllu(monsterCode);
		return enemyList;
	}

	public void AddMonster(List<string> monsterCodes, List<bool> actionFlags, Action callback)
	{
		int count = allEnemies.Count;
		for (int i = 0; i < monsterCodes.Count; i++)
		{
			EnemyBaseCtrl enemyCtrl = GetEnemyCtrl(monsterCodes[i]);
			allEnemies.Add(enemyCtrl.EnemyEntity);
			allEnemyQueueTrans.Add(enemyCtrl.transform);
			allEnemiesActionFlag.Add(actionFlags[i]);
			enemyCtrl.EnemyEntity.StartBattle();
		}
		for (int j = 0; j < allEnemies.Count; j++)
		{
			allEnemies[j].OnBattleReady();
		}
		SingletonDontDestroy<Game>.Instance.CurrentUserData.TryUnlockMonsterIllu(monsterCodes);
		ReAdjustMonstersPosWithoutMove(count);
		callback?.Invoke();
	}

	public EnemyBase GetEnemyBase(string enemyCode)
	{
		for (int i = 0; i < allEnemies.Count; i++)
		{
			if (allEnemies[i].EnemyCode == enemyCode)
			{
				return allEnemies[i];
			}
		}
		return null;
	}

	public void AddDieingMonster(EnemyBase enemyBase)
	{
		dieingEnemies.Add(enemyBase);
	}

	public void RemoveDieingMonster(EnemyBase enemyBase)
	{
		dieingEnemies.Remove(enemyBase);
	}

	public void RemoveMonster(EnemyBase enemyBase, bool isNeedReAdjust)
	{
		bool flag = false;
		bool flag2 = true;
		for (int i = 0; i < allEnemies.Count; i++)
		{
			if (allEnemies[i] == enemyBase)
			{
				allEnemies.RemoveAt(i);
				flag2 = false;
				allEnemyQueueTrans.Remove(enemyBase.EnemyCtrl.transform);
				allEnemiesActionFlag.RemoveAt(i);
				if (i == currentActionEnemyIndex)
				{
					flag = true;
					currentActionEnemyIndex = -1;
				}
				break;
			}
		}
		if (flag2)
		{
			return;
		}
		if (allEnemies.Count == 0 && Singleton<GameManager>.Instance.CurrentManagerState is GameManager_BattleState)
		{
			Singleton<GameManager>.Instance.BattleSystem.EndBattle();
			return;
		}
		if (isNeedReAdjust)
		{
			ReAdjustMonstersPos();
		}
		if (flag)
		{
			StartCoroutine(WaitWhenAEnemyDead_IE());
		}
	}

	private IEnumerator WaitWhenAEnemyDead_IE()
	{
		while (dieingEnemies.Count > 0)
		{
			yield return null;
		}
		Singleton<GameManager>.Instance.BattleSystem.EndEnemyRound();
	}

	public void RecycleEnemy(string enemyCode, EnemyBaseCtrl enemyBaseCtrl)
	{
		enemyBaseCtrl.gameObject.SetActive(value: false);
		if (allEnemyPool.TryGetValue(enemyCode, out var value))
		{
			value.Enqueue(enemyBaseCtrl);
			return;
		}
		value = new Queue<EnemyBaseCtrl>();
		value.Enqueue(enemyBaseCtrl);
		allEnemyPool[enemyCode] = value;
	}

	public void RecycleAllEnemy()
	{
		for (int i = 0; i < allEnemies.Count; i++)
		{
			allEnemies[i].EnemyCtrl.DeAtiveThisEntity();
			allEnemies[i].EnemyCtrl.gameObject.SetActive(value: false);
			if (allEnemyPool.TryGetValue(allEnemies[i].EnemyCode, out var value))
			{
				value.Enqueue(allEnemies[i].EnemyCtrl);
				continue;
			}
			value = new Queue<EnemyBaseCtrl>();
			value.Enqueue(allEnemies[i].EnemyCtrl);
			allEnemyPool[allEnemies[i].EnemyCode] = value;
		}
		allEnemies.Clear();
		allEnemiesActionFlag.Clear();
		allEnemyQueueTrans.Clear();
	}

	public void ReAdjustMonstersPos(int preAmount = -1, Action callback = null)
	{
		int count = allEnemyQueueTrans.Count;
		Vector3[] reAdjustMonsterPos = GetReAdjustMonsterPos(count);
		if (preAmount > 0)
		{
			for (int i = 0; i < preAmount; i++)
			{
				allEnemyQueueTrans[i].transform.position = reAdjustMonsterPos[i];
			}
			for (int j = 0; j <= preAmount; j++)
			{
				if (j == preAmount)
				{
					allEnemyQueueTrans[j].transform.DOMove(reAdjustMonsterPos[j], 0.4f).OnComplete(delegate
					{
						callback?.Invoke();
					});
				}
				else
				{
					allEnemyQueueTrans[j].transform.DOMove(reAdjustMonsterPos[j], 0.4f);
				}
			}
			return;
		}
		for (int k = 0; k < count; k++)
		{
			if (k == count - 1)
			{
				allEnemyQueueTrans[k].transform.DOMove(reAdjustMonsterPos[k], 0.4f).OnComplete(delegate
				{
					callback?.Invoke();
				});
			}
			else
			{
				allEnemyQueueTrans[k].transform.DOMove(reAdjustMonsterPos[k], 0.4f);
			}
		}
	}

	public void ReAdjustMonstersPosWithoutMove(int preAmount = -1)
	{
		int count = allEnemyQueueTrans.Count;
		Vector3[] reAdjustMonsterPos = GetReAdjustMonsterPos(count);
		if (preAmount > 0)
		{
			for (int i = 0; i < preAmount; i++)
			{
				allEnemyQueueTrans[i].transform.position = reAdjustMonsterPos[i];
			}
			for (int j = 0; j <= preAmount; j++)
			{
				allEnemyQueueTrans[j].transform.position = reAdjustMonsterPos[j];
			}
		}
		else
		{
			for (int k = 0; k < count; k++)
			{
				allEnemyQueueTrans[k].transform.position = reAdjustMonsterPos[k];
			}
		}
	}

	private void SummorMonsterReadjustPos(int preAmount, int preIndex, Action callback)
	{
		int count = allEnemyQueueTrans.Count;
		Vector3[] reAdjustMonsterPos = GetReAdjustMonsterPos(count);
		if (preAmount > 0)
		{
			for (int i = preIndex; i < preAmount + preIndex; i++)
			{
				if (i == preIndex)
				{
					allEnemyQueueTrans[i].transform.DOMove(reAdjustMonsterPos[i], 0.4f).OnComplete(delegate
					{
						callback?.Invoke();
					});
				}
				else
				{
					allEnemyQueueTrans[i].transform.DOMove(reAdjustMonsterPos[i], 0.4f);
				}
			}
			for (int j = 0; j < count; j++)
			{
				if (j < preIndex || j >= preIndex + preAmount)
				{
					allEnemyQueueTrans[j].transform.position = reAdjustMonsterPos[j];
				}
			}
		}
		else
		{
			for (int k = 0; k < count; k++)
			{
				allEnemyQueueTrans[k].transform.position = reAdjustMonsterPos[k];
			}
			callback?.Invoke();
		}
	}

	private Vector3[] GetReAdjustMonsterPos(int currentAmount)
	{
		Vector3[] array = new Vector3[currentAmount];
		Vector3 position = BattleEnvironmentManager.Instance.enemyStartPoint.position;
		float num = (BattleEnvironmentManager.Instance.enemyEndPoint.position.x - position.x) / 4f;
		int num2 = 5 - currentAmount;
		float num3 = position.x + (float)num2 * 0.5f * num;
		for (int i = 0; i < currentAmount; i++)
		{
			array[i] = new Vector3(num3 + num * (float)i, position.y, position.z);
		}
		return array;
	}

	public int GetSpecificMonsterAmount(string enemyCode)
	{
		int num = 0;
		for (int i = 0; i < allEnemies.Count; i++)
		{
			if (allEnemies[i].EnemyCode == enemyCode)
			{
				num++;
			}
		}
		return num;
	}

	private EnemyBaseCtrl GetEnemyCtrl(string monsterCode)
	{
		if (allEnemyPool.TryGetValue(monsterCode, out var value))
		{
			EnemyBaseCtrl enemyBaseCtrl = value.Dequeue();
			enemyBaseCtrl.gameObject.SetActive(value: true);
			if (value.Count == 0)
			{
				allEnemyPool.Remove(monsterCode);
			}
			return enemyBaseCtrl;
		}
		Transform enemyRoot = BattleEnvironmentManager.Instance.enemyRoot;
		GameObject obj = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace(monsterCode, "Prefabs/Monster", null);
		obj.transform.SetParent(enemyRoot, worldPositionStays: false);
		return obj.GetComponent<EnemyBaseCtrl>();
	}

	public EnemyBase GetNextEnemy()
	{
		if (!Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			currentActionEnemyIndex = -1;
			return null;
		}
		for (int i = 0; i < allEnemiesActionFlag.Count; i++)
		{
			if (!allEnemiesActionFlag[i] && !allEnemies[i].IsDead)
			{
				currentActionEnemyIndex = i;
				allEnemiesActionFlag[i] = true;
				return allEnemies[i];
			}
		}
		currentActionEnemyIndex = -1;
		return null;
	}

	public void ResetEnemyFlags()
	{
		for (int i = 0; i < allEnemiesActionFlag.Count; i++)
		{
			allEnemiesActionFlag[i] = false;
		}
		currentActionEnemyIndex = -1;
	}

	public EnemyBase GetEemeyByCode(string enemyCode)
	{
		for (int i = 0; i < allEnemies.Count; i++)
		{
			if (allEnemies[i].EnemyCode == enemyCode)
			{
				return allEnemies[i];
			}
		}
		return null;
	}

	public bool IsAllEnemyActionOver()
	{
		for (int i = 0; i < allEnemies.Count; i++)
		{
			if (!allEnemies[i].IsActionOver)
			{
				return false;
			}
		}
		return true;
	}
}
