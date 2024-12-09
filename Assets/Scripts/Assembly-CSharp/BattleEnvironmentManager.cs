using System.Collections.Generic;
using UnityEngine;

public class BattleEnvironmentManager : MonoBehaviour
{
	private static BattleEnvironmentManager _instance;

	[HideInInspector]
	public Transform enemyStartPoint;

	[HideInInspector]
	public Transform enemyEndPoint;

	[HideInInspector]
	public Transform enemyRoot;

	private Transform bgRoot;

	private Dictionary<EnemyBaseCtrl, EnemyChosenHint> allShowingEnemyChosenHint = new Dictionary<EnemyBaseCtrl, EnemyChosenHint>();

	private Queue<EnemyChosenHint> allEnemyChosenHintPool = new Queue<EnemyChosenHint>();

	private Dictionary<string, GameObject> allLevelBgMap = new Dictionary<string, GameObject>();

	private string currentShowingObjName;

	public static BattleEnvironmentManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("BattleEnvironment", "Prefabs", null).GetComponent<BattleEnvironmentManager>();
				_instance.transform.position = Vector3.zero;
			}
			return _instance;
		}
	}

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		enemyStartPoint = base.transform.Find("Enemy_Start");
		enemyEndPoint = base.transform.Find("Enemy_End");
		enemyRoot = base.transform.Find("EnemyRoot");
		bgRoot = base.transform.Find("BgRoot");
	}

	public void SetEnemyHintHighlight(EnemyBaseCtrl enemy)
	{
		if (allShowingEnemyChosenHint.TryGetValue(enemy, out var value))
		{
			value.SetChosen();
		}
	}

	public void SetAllEnemyHintHighlight()
	{
		foreach (KeyValuePair<EnemyBaseCtrl, EnemyChosenHint> item in allShowingEnemyChosenHint)
		{
			item.Value.SetChosen();
		}
	}

	public void CancelEnemyHintHighlight(EnemyBaseCtrl enemy)
	{
		if (allShowingEnemyChosenHint.TryGetValue(enemy, out var value))
		{
			value.SetPossible();
		}
	}

	public void CancelAllEnemyHintHighlight()
	{
		foreach (KeyValuePair<EnemyBaseCtrl, EnemyChosenHint> item in allShowingEnemyChosenHint)
		{
			item.Value.SetPossible();
		}
	}

	public void ShowEnemyHint(EnemyBaseCtrl enemy, Vector2 offset)
	{
		if (!allShowingEnemyChosenHint.ContainsKey(enemy))
		{
			EnemyChosenHint enemyChosenHint = GetEnemyChosenHint();
			enemyChosenHint.ShowHint(enemy.transform, offset);
			allShowingEnemyChosenHint[enemy] = enemyChosenHint;
			enemyChosenHint.SetPossible();
		}
	}

	public void ShowEnemyRandomHint(EnemyBaseCtrl enemy, Vector2 offset)
	{
		if (!allShowingEnemyChosenHint.ContainsKey(enemy))
		{
			EnemyChosenHint enemyChosenHint = GetEnemyChosenHint();
			enemyChosenHint.ShowHint(enemy.transform, offset);
			allShowingEnemyChosenHint[enemy] = enemyChosenHint;
			enemyChosenHint.SetRandom();
		}
	}

	private EnemyChosenHint GetEnemyChosenHint()
	{
		if (allEnemyChosenHintPool.Count > 0)
		{
			EnemyChosenHint enemyChosenHint = allEnemyChosenHintPool.Dequeue();
			enemyChosenHint.gameObject.SetActive(value: true);
			return enemyChosenHint;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("EnemyChosenHint", "Prefabs", base.transform).GetComponent<EnemyChosenHint>();
	}

	public void HideAllEnemyHint()
	{
		if (allShowingEnemyChosenHint.Count > 0)
		{
			foreach (EnemyChosenHint value in allShowingEnemyChosenHint.Values)
			{
				value.gameObject.SetActive(value: false);
				allEnemyChosenHintPool.Enqueue(value);
			}
		}
		allShowingEnemyChosenHint.Clear();
	}

	public void SetBg(string name)
	{
		bgRoot.localScale = SingletonDontDestroy<UIManager>.Instance.ScaleRate * Vector3.one;
		base.transform.localScale = Vector3.one * UIManager.WorldScale;
		currentShowingObjName = name;
		bgRoot.gameObject.SetActive(value: true);
		if (allLevelBgMap.TryGetValue(name, out var value))
		{
			value.SetActive(value: true);
			return;
		}
		value = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace(name, "Prefabs/BattleBg", bgRoot);
		value.transform.SetParent(bgRoot);
		value.transform.localScale = Vector3.one;
		value.transform.localPosition = Vector3.zero;
		allLevelBgMap.Add(name, value);
	}

	public void HideBg()
	{
		if (!currentShowingObjName.IsNullOrEmpty())
		{
			bgRoot.gameObject.SetActive(value: false);
			allLevelBgMap[currentShowingObjName].SetActive(value: false);
			currentShowingObjName = string.Empty;
		}
	}
}
