using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HoverWithKeysHint : BaseHoverHint
{
	protected Transform keyRoot;

	protected RectTransform keyRootRect;

	protected float localpositionX;

	private Dictionary<string, KeyCtrl> allShowingKeys = new Dictionary<string, KeyCtrl>();

	private bool isKeyShowAtLeft;

	public RectTransform KeyRootRect => keyRootRect;

	protected override void OnAwake()
	{
		InitKey();
		keyRootRect = keyRoot.GetComponent<RectTransform>();
		localpositionX = keyRootRect.anchoredPosition.x;
	}

	protected abstract void InitKey();

	public void AddKeys(List<KeyValuePair> allKeys)
	{
		if (allKeys == null || allKeys.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair allKey in allKeys)
		{
			if (!allShowingKeys.ContainsKey(allKey.key))
			{
				KeyCtrl keyCtrl = parentUI.GetKeyCtrl(keyRoot);
				keyCtrl.transform.SetParent(keyRoot);
				keyCtrl.LoadKey(allKey.key.LocalizeText(), allKey.value.LocalizeText());
				allShowingKeys.Add(allKey.key, keyCtrl);
			}
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(keyRootRect);
	}

	public void RecycleAllKeys()
	{
		if (allShowingKeys.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<string, KeyCtrl> allShowingKey in allShowingKeys)
		{
			parentUI.RecycleKeyCtrl(allShowingKey.Value);
		}
		allShowingKeys.Clear();
	}

	public void ShowKeyAtLeft()
	{
		if (!isKeyShowAtLeft)
		{
			isKeyShowAtLeft = true;
			keyRootRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, localpositionX, 0f);
		}
	}

	public void ShowKeyAtRight()
	{
		if (isKeyShowAtLeft)
		{
			isKeyShowAtLeft = false;
			keyRootRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, localpositionX, 0f);
		}
	}
}
