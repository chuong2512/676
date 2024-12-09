using System.Collections.Generic;
using UnityEngine.UI;

public class NormalMessageHoverHint : HoverWithKeysHint
{
	private Text messageText;

	protected override void OnAwake()
	{
		base.OnAwake();
		messageText = base.transform.Find("Content").GetComponent<Text>();
	}

	protected override void InitKey()
	{
		keyRoot = base.transform.Find("KeyItem/KeyRoot");
	}

	public void SetMessageHint(string content, List<KeyValuePair> allKeys)
	{
		messageText.text = content;
		if (allKeys != null && allKeys.Count > 0)
		{
			AddKeys(allKeys);
			LayoutRebuilder.ForceRebuildLayoutImmediate(keyRootRect);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.m_RectTransform);
	}
}
