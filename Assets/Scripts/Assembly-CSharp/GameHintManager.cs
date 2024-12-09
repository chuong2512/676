using System;
using System.Collections.Generic;
using UnityEngine;

public class GameHintManager : Singleton<GameHintManager>
{
	private Queue<FlowingText> allFlowingTextPool = new Queue<FlowingText>();

	public Font damageFont;

	public Font armorFont;

	public Font healingFont;

	public Font bleedingDmgFont;

	public Font poisonDmgFont;

	public Font trueDmgFont;

	public Sprite damageBgSprite;

	public Sprite armorBgSprite;

	public Sprite healingBgSprite;

	public Sprite AbsDmgIconSprite;

	public Sprite PoisonDmgIconSprite;

	public Sprite BleedingDmgIconSprite;

	private Queue<DamageFlowingText> allDamageFlowingTextPool = new Queue<DamageFlowingText>();

	private static uint DmgFlowingTextTime = 1u;

	private Queue<BuffHintCtrl> allBuffHintCtrlPools = new Queue<BuffHintCtrl>();

	private Dictionary<Transform, List<BuffHintCtrl>> allShowingBuffHints = new Dictionary<Transform, List<BuffHintCtrl>>();

	public void AddFlowingText_ScreenPos(string content, Color textColor, Color outlineColor, Transform target, bool isSetParent, Vector3 offsetPos, Action callback = null)
	{
		GetFlowingText(isSetParent ? target : base.transform).LoadFlowingUpText(content, textColor, outlineColor, target, isSetParent, offsetPos, 0.01f, callback);
	}

	public void AddFlowingTextForImmueBuffHing(string content, float scale, Transform target)
	{
		GetFlowingText(target).LoadFlowingUpText(content, Color.white, Color.black, target, isSetParent: true, Vector3.zero, scale);
	}

	public void RecycleAll()
	{
		foreach (Transform item in base.transform)
		{
			FlowingText component = item.GetComponent<FlowingText>();
			if (!(component == null) && !allFlowingTextPool.Contains(component))
			{
				component.RecycleText();
			}
		}
	}

	public void AddFlowingText_ScreenPos(string content, Color textColor, Color outlineColor, Transform target, bool isSetParent, Vector3 offsetPos, Vector2 horizontalRange, Vector2 heightRange, Action callback = null)
	{
		GetFlowingText(isSetParent ? target : base.transform).LoadFlowingText(content, textColor, outlineColor, target, isSetParent, offsetPos, horizontalRange, heightRange, callback);
	}

	public void AddFlowingText_WorldPos(string content, Color textColor, Color outlineColor, Transform target, bool isSetParent, Vector3 offsetPos, Action callback = null, float waitTime = 1f)
	{
		GetFlowingText(isSetParent ? target : base.transform).LoadFlowingUpText(content, textColor, outlineColor, target, isSetParent, offsetPos, 0.01f, callback, waitTime);
	}

	public void AddFlowingDownText_WorldPos(string content, Color textColor, Color outlineColor, Transform target, bool isSetParent, Vector3 offsetPos, Action callback = null)
	{
		GetFlowingText(isSetParent ? target : base.transform).LoadFlowingDownText(content, textColor, outlineColor, target, isSetParent, offsetPos, callback);
	}

	private FlowingText GetFlowingText(Transform root)
	{
		if (allFlowingTextPool.Count > 0)
		{
			FlowingText flowingText = allFlowingTextPool.Dequeue();
			flowingText.gameObject.SetActive(value: true);
			flowingText.SetFlowingTextNotBeRecycled();
			return flowingText;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("FlowingText", "Prefabs", root).GetComponent<FlowingText>();
	}

	public void RecycleFlowingText(FlowingText text)
	{
		text.StopAllCoroutines();
		text.gameObject.SetActive(value: false);
		allFlowingTextPool.Enqueue(text);
	}

	public void ShowDamageFlowingText(Transform target, bool isSetParent, Vector3 offsetPos, Vector2 randomRange, int dmgAmount, float scale, bool isAbsDmg)
	{
		float num = Mathf.Pow(-1f, DmgFlowingTextTime % 4u);
		float num2 = Mathf.Pow(-1f, DmgFlowingTextTime / 2u);
		DmgFlowingTextTime++;
		DamageFlowingText damageFlowingText = GetDamageFlowingText(isSetParent ? target : base.transform);
		damageFlowingText.transform.SetAsLastSibling();
		float x = UnityEngine.Random.Range(num * randomRange.x, 0f);
		float y = UnityEngine.Random.Range(num2 * randomRange.y, 0f);
		damageFlowingText.transform.position = target.position + offsetPos + new Vector3(x, y);
		damageFlowingText.ShowDamageFlowingText(this, dmgAmount.ToString(), scale, isAbsDmg ? trueDmgFont : damageFont, damageBgSprite, isAbsDmg);
	}

	public void ShowArmorDamageFlowingText(Transform target, bool isSetParent, Vector3 offsetPos, int dmgAmount, float scale, bool isAbsDmg)
	{
		DamageFlowingText damageFlowingText = GetDamageFlowingText(isSetParent ? target : base.transform);
		damageFlowingText.transform.SetAsLastSibling();
		damageFlowingText.transform.position = target.position + offsetPos;
		damageFlowingText.ShowArmorDmgFlowingText(this, dmgAmount.ToString(), scale, armorFont, isAbsDmg);
	}

	public void ShowPoisonDamageFlowingText(Transform target, bool isSetParent, Vector3 offsetPos, int dmgAmount, float scale)
	{
		DamageFlowingText damageFlowingText = GetDamageFlowingText(isSetParent ? target : base.transform);
		damageFlowingText.transform.SetAsLastSibling();
		damageFlowingText.transform.position = target.position + offsetPos;
		damageFlowingText.ShowPoisonFlowingText(this, dmgAmount.ToString(), scale, poisonDmgFont);
	}

	public void ShowBleedingDamageFlowingText(Transform target, bool isSetParent, Vector3 offsetPos, int dmgAmount, float scale)
	{
		DamageFlowingText damageFlowingText = GetDamageFlowingText(isSetParent ? target : base.transform);
		damageFlowingText.transform.SetAsLastSibling();
		damageFlowingText.transform.position = target.position + offsetPos;
		damageFlowingText.ShowBleedingFlowingText(this, dmgAmount.ToString(), scale, bleedingDmgFont);
	}

	public void ShowHealingFlowingText(Transform target, bool isSetParent, Vector3 offsetPos, Vector2 randomRange, int dmgAmount, float scale)
	{
		DamageFlowingText damageFlowingText = GetDamageFlowingText(isSetParent ? target : base.transform);
		damageFlowingText.transform.SetAsLastSibling();
		float x = UnityEngine.Random.Range(0f - randomRange.x, randomRange.x);
		float y = UnityEngine.Random.Range(0f - randomRange.y, randomRange.y);
		damageFlowingText.transform.position = target.position + offsetPos + new Vector3(x, y);
		damageFlowingText.ShowDamageFlowingText(this, "+" + dmgAmount, scale, healingFont, healingBgSprite, isAbsDmg: false);
	}

	private DamageFlowingText GetDamageFlowingText(Transform root)
	{
		if (allDamageFlowingTextPool.Count > 0)
		{
			DamageFlowingText damageFlowingText = allDamageFlowingTextPool.Dequeue();
			damageFlowingText.gameObject.SetActive(value: true);
			damageFlowingText.SetFlowingTextNotBeRecycled();
			damageFlowingText.transform.SetParent(root);
			return damageFlowingText;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("DamageFlowingText", "Prefabs", root).GetComponent<DamageFlowingText>();
	}

	public void RecycleDamageFlowingText(DamageFlowingText text)
	{
		text.gameObject.SetActive(value: false);
		allDamageFlowingTextPool.Enqueue(text);
	}

	public void ShowBuffHint(Transform targetTrans, bool isSetParent, float scale, BuffType buffType, Color nameColor)
	{
		BuffHintCtrl buffHint = GetBuffHint(isSetParent ? targetTrans : base.transform);
		buffHint.ShowBuff(this, targetTrans, scale, buffType, nameColor);
		if (!(targetTrans != null))
		{
			return;
		}
		if (allShowingBuffHints.TryGetValue(targetTrans, out var value))
		{
			for (int i = 0; i < value.Count; i++)
			{
				value[i].MoveUp();
			}
			value.Add(buffHint);
		}
		else
		{
			value = new List<BuffHintCtrl> { buffHint };
			allShowingBuffHints[targetTrans] = value;
		}
	}

	private BuffHintCtrl GetBuffHint(Transform root)
	{
		if (allBuffHintCtrlPools.Count > 0)
		{
			BuffHintCtrl buffHintCtrl = allBuffHintCtrlPools.Dequeue();
			buffHintCtrl.gameObject.SetActive(value: true);
			buffHintCtrl.SetBuffHintNotBeRecycled();
			buffHintCtrl.transform.SetParent(root);
			return buffHintCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("BuffHintCtrl", "Prefabs", root).GetComponent<BuffHintCtrl>();
	}

	public void RecycleBuffHintCtrl(Transform target, BuffHintCtrl ctrl)
	{
		ctrl.StopAllCoroutines();
		ctrl.gameObject.SetActive(value: false);
		allBuffHintCtrlPools.Enqueue(ctrl);
		if (target != null)
		{
			allShowingBuffHints[target].Remove(ctrl);
		}
	}
}
