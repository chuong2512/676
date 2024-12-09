using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : UIView
{
	private Text nameText;

	private Transform meanRoot;

	private Transform buffRoot;

	private UsualHealthBarCtrl _usualHealthBarCtrl;

	private Dictionary<BuffType, BuffIconCtrl> allShowingBuff = new Dictionary<BuffType, BuffIconCtrl>();

	private Queue<BuffIconCtrl> allBuffIconPool = new Queue<BuffIconCtrl>();

	private static Queue<MeanIconCtrl> meanIconPool = new Queue<MeanIconCtrl>();

	private List<MeanIconCtrl> showingMeanIcons = new List<MeanIconCtrl>();

	public override string UIViewName => "BossUI";

	public override string UILayerName => "NormalLayer";

	public Transform HealthBarTrans => _usualHealthBarCtrl.transform;

	public Transform ArmorTrans { get; private set; }

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		SetName((string)objs[0]);
		SetInitHealth((int)objs[1]);
		UpdateArmor((int)objs[2]);
	}

	public override void HideView()
	{
		ClearHealthBarBuff();
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
		_usualHealthBarCtrl = base.transform.Find("Bg/UsualHealthBar").GetComponent<UsualHealthBarCtrl>();
		nameText = base.transform.Find("Bg/NameBottom/Name").GetComponent<Text>();
		meanRoot = base.transform.Find("Bg/MeanRoot");
		buffRoot = base.transform.Find("Bg/BuffRoot");
		ArmorTrans = base.transform.Find("Bg/ArmorBottom/Icon");
	}

	private void SetName(string name)
	{
		nameText.text = name;
	}

	private void SetInitHealth(int maxHealth)
	{
		_usualHealthBarCtrl.LoadHealth(maxHealth, maxHealth);
	}

	public void UpdateHealth(int health, int maxHealth)
	{
		_usualHealthBarCtrl.UpdateHealth(health, maxHealth);
	}

	public void UpdateArmor(int armor)
	{
	}

	public void AddBuff(BaseBuff buff)
	{
		BuffIconCtrl buffIcon = GetBuffIcon();
		buffIcon.transform.SetAsLastSibling();
		buffIcon.transform.DOComplete();
		buffIcon.LoadBuff(buff, isScreen: false);
		allShowingBuff.Add(buff.BuffType, buffIcon);
	}

	public void RemoveBuff(BaseBuff buff)
	{
		if (allShowingBuff.TryGetValue(buff.BuffType, out var value))
		{
			value.gameObject.SetActive(value: false);
			allShowingBuff.Remove(buff.BuffType);
			allBuffIconPool.Enqueue(value);
		}
	}

	public void ClearHealthBarBuff()
	{
		if (allShowingBuff.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<BuffType, BuffIconCtrl> item in allShowingBuff)
		{
			item.Value.gameObject.SetActive(value: false);
			allBuffIconPool.Enqueue(item.Value);
		}
		allShowingBuff.Clear();
	}

	public void UpdateBuff(BaseBuff buff)
	{
		if (allShowingBuff.TryGetValue(buff.BuffType, out var value))
		{
			value.UpdateBuff();
		}
	}

	private BuffIconCtrl GetBuffIcon()
	{
		if (allBuffIconPool.Count > 0)
		{
			BuffIconCtrl buffIconCtrl = allBuffIconPool.Dequeue();
			buffIconCtrl.gameObject.SetActive(value: true);
			return buffIconCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("BuffIcon", "Prefabs", buffRoot).GetComponent<BuffIconCtrl>();
	}

	private void AddMean(MeanHandler meanHandler)
	{
		MeanIconCtrl meanIcon = GetMeanIcon();
		meanIcon.transform.SetParent(meanRoot);
		meanIcon.transform.localScale = Vector3.one;
		meanIcon.SetMean(meanHandler);
		showingMeanIcons.Add(meanIcon);
	}

	public void AddMean(MeanHandler[] meanHandlers)
	{
		RecycleMeanIcon();
		for (int i = 0; i < meanHandlers.Length; i++)
		{
			AddMean(meanHandlers[i]);
		}
	}

	protected MeanIconCtrl GetMeanIcon()
	{
		if (meanIconPool.Count > 0)
		{
			MeanIconCtrl meanIconCtrl = meanIconPool.Dequeue();
			if (meanIconCtrl == null)
			{
				return GetMeanIcon();
			}
			meanIconCtrl.gameObject.SetActive(value: true);
			return meanIconCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("MeanIcon", "Prefabs", meanRoot).GetComponent<MeanIconCtrl>();
	}

	public void RecycleMeanIcon()
	{
		if (showingMeanIcons.Count > 0)
		{
			for (int i = 0; i < showingMeanIcons.Count; i++)
			{
				showingMeanIcons[i].gameObject.SetActive(value: false);
				meanIconPool.Enqueue(showingMeanIcons[i]);
			}
			showingMeanIcons.Clear();
		}
	}

	public void HideEnemyMean()
	{
		meanRoot.gameObject.SetActive(value: false);
	}

	public void ShowEnemyMean()
	{
		meanRoot.gameObject.SetActive(value: true);
	}
}
