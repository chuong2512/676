using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarCtrl : MonoBehaviour
{
	private UsualHealthBarCtrl _healthBar;

	private Transform buffRoot;

	private Dictionary<BuffType, BuffIconCtrl> allShowingBuff = new Dictionary<BuffType, BuffIconCtrl>();

	private Queue<BuffIconCtrl> allBuffIconPool = new Queue<BuffIconCtrl>();

	private Text nameText;

	private Text armorText;

	private Image armorImgBg;

	private Tween armorTween;

	public Transform ArmorTrans => armorImgBg.transform;

	private void Awake()
	{
		buffRoot = base.transform.Find("BuffRoot");
		nameText = base.transform.Find("Name").GetComponent<Text>();
		armorImgBg = base.transform.Find("UsualHealthBar/ArmorBg").GetComponent<Image>();
		armorText = base.transform.Find("UsualHealthBar/ArmorBg/ArmorAmount").GetComponent<Text>();
		_healthBar = base.transform.Find("UsualHealthBar").GetComponent<UsualHealthBarCtrl>();
	}

	public void InitHealthBar(string entityName, int maxHealth, int armorAmount)
	{
		nameText.text = entityName;
		_healthBar.LoadHealth(maxHealth, maxHealth);
		ClearHealthBarBuff();
		UpdateAmor(armorAmount);
	}

	public void UpdateHealth(int currentHealth, int maxHealth)
	{
		_healthBar.UpdateHealth(currentHealth, maxHealth);
	}

	public void UpdateAmor(int armor)
	{
		if (!armorTween.IsNull() && armorTween.IsActive())
		{
			armorTween.Complete();
		}
		armorTween = armorText.transform.TransformHint();
		armorImgBg.enabled = armor > 0;
		armorText.text = ((armor > 0) ? armor.ToString() : string.Empty);
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
}
