using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTalkUI : UIView
{
	private Transform root;

	private Queue<BubbleMTopTalkCtrl> allMTopPools = new Queue<BubbleMTopTalkCtrl>();

	private Dictionary<BubbleMTopTalkCtrl, Coroutine> allShowingMTopBubbles = new Dictionary<BubbleMTopTalkCtrl, Coroutine>();

	private Queue<BubbleMRightTalkCtrl> allMRightPools = new Queue<BubbleMRightTalkCtrl>();

	private Dictionary<BubbleMRightTalkCtrl, Coroutine> allShowingMRightBubbles = new Dictionary<BubbleMRightTalkCtrl, Coroutine>();

	private Queue<BubblePLeftTalkCtrl> allPLeftPools = new Queue<BubblePLeftTalkCtrl>();

	private BubblePLeftTalkCtrl currentShowingPleft;

	private Queue<BubbleWitchTalkCtrl> allWitchPools = new Queue<BubbleWitchTalkCtrl>();

	private BubbleWitchTalkCtrl currentShowingWitch;

	private Queue<BubbleShopTalkCtrl> allShopPools = new Queue<BubbleShopTalkCtrl>();

	private BubbleShopTalkCtrl currentShowingShop;

	public override string UIViewName => "BubbleTalkUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destory Player Bubble UI...");
	}

	public override void OnSpawnUI()
	{
		root = base.transform.Find("Root");
	}

	private IEnumerator BubbleFollow_IE(Transform bubbleTarget, Transform followRoot)
	{
		while (true)
		{
			bubbleTarget.position = followRoot.position;
			yield return null;
		}
	}

	public void ShowMTopBubble(string content, Transform root)
	{
		BubbleMTopTalkCtrl mTopCtrl = GetMTopCtrl();
		mTopCtrl.ShowMTopBubble(content, this);
		Coroutine value = StartCoroutine(BubbleFollow_IE(mTopCtrl.transform, root));
		allShowingMTopBubbles.Add(mTopCtrl, value);
	}

	private BubbleMTopTalkCtrl GetMTopCtrl()
	{
		if (allMTopPools.Count > 0)
		{
			BubbleMTopTalkCtrl bubbleMTopTalkCtrl = allMTopPools.Dequeue();
			bubbleMTopTalkCtrl.gameObject.SetActive(value: true);
			return bubbleMTopTalkCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("BubbleType_MTop", "Prefabs", root).GetComponent<BubbleMTopTalkCtrl>();
	}

	public void RecycleMTopBubble(BubbleMTopTalkCtrl ctrl)
	{
		if (allShowingMTopBubbles.TryGetValue(ctrl, out var value))
		{
			allMTopPools.Enqueue(ctrl);
			StopCoroutine(value);
			allShowingMTopBubbles.Remove(ctrl);
		}
	}

	public void ShowMRightBubble(string content, Transform root)
	{
		BubbleMRightTalkCtrl mRightCtrl = GetMRightCtrl();
		mRightCtrl.ShowMRightBubble(content, this);
		Coroutine value = StartCoroutine(BubbleFollow_IE(mRightCtrl.transform, root));
		allShowingMRightBubbles.Add(mRightCtrl, value);
	}

	private BubbleMRightTalkCtrl GetMRightCtrl()
	{
		if (allMRightPools.Count > 0)
		{
			BubbleMRightTalkCtrl bubbleMRightTalkCtrl = allMRightPools.Dequeue();
			bubbleMRightTalkCtrl.gameObject.SetActive(value: true);
			return bubbleMRightTalkCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("BubbleType_MRight", "Prefabs", root).GetComponent<BubbleMRightTalkCtrl>();
	}

	public void RecycleMRightBubble(BubbleMRightTalkCtrl ctrl)
	{
		if (allShowingMRightBubbles.TryGetValue(ctrl, out var value))
		{
			allMRightPools.Enqueue(ctrl);
			StopCoroutine(value);
			allShowingMRightBubbles.Remove(ctrl);
		}
	}

	public void ShowPLeftBubble(string content, Vector3 pos)
	{
		if (currentShowingPleft != null)
		{
			currentShowingPleft.SpecialHide();
		}
		BubblePLeftTalkCtrl pLeftCtrl = GetPLeftCtrl();
		pLeftCtrl.ShowPLeftBubble(content, pos, this);
		currentShowingPleft = pLeftCtrl;
	}

	private BubblePLeftTalkCtrl GetPLeftCtrl()
	{
		if (allPLeftPools.Count > 0)
		{
			BubblePLeftTalkCtrl bubblePLeftTalkCtrl = allPLeftPools.Dequeue();
			bubblePLeftTalkCtrl.gameObject.SetActive(value: true);
			return bubblePLeftTalkCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("BubbleType_PLeft", "Prefabs", root).GetComponent<BubblePLeftTalkCtrl>();
	}

	public void RecyclePLeftBubble(BubblePLeftTalkCtrl ctrl)
	{
		allPLeftPools.Enqueue(ctrl);
		if (ctrl == currentShowingPleft)
		{
			currentShowingPleft = null;
		}
	}

	public void ShowWitchBubble(string content, Vector3 pos)
	{
		if (currentShowingWitch != null)
		{
			currentShowingWitch.SpecialHide();
		}
		BubbleWitchTalkCtrl witchCtrl = GetWitchCtrl();
		witchCtrl.ShowWitchBubble(content, pos, this);
		currentShowingWitch = witchCtrl;
	}

	private BubbleWitchTalkCtrl GetWitchCtrl()
	{
		if (allWitchPools.Count > 0)
		{
			BubbleWitchTalkCtrl bubbleWitchTalkCtrl = allWitchPools.Dequeue();
			bubbleWitchTalkCtrl.gameObject.SetActive(value: true);
			return bubbleWitchTalkCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("BubbleType_Witch", "Prefabs", root).GetComponent<BubbleWitchTalkCtrl>();
	}

	public void RecycleWitchBubble(BubbleWitchTalkCtrl ctrl)
	{
		allWitchPools.Enqueue(ctrl);
		if (ctrl == currentShowingWitch)
		{
			currentShowingWitch = null;
		}
	}

	public void ShowShopBubble(string content, Vector3 pos)
	{
		if (currentShowingShop != null)
		{
			currentShowingShop.SpecialHide();
		}
		BubbleShopTalkCtrl shopCtrl = GetShopCtrl();
		shopCtrl.ShowShopBubble(content, pos, this);
		currentShowingShop = shopCtrl;
	}

	private BubbleShopTalkCtrl GetShopCtrl()
	{
		if (allShopPools.Count > 0)
		{
			BubbleShopTalkCtrl bubbleShopTalkCtrl = allShopPools.Dequeue();
			bubbleShopTalkCtrl.gameObject.SetActive(value: true);
			return bubbleShopTalkCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("BubbleType_Shop", "Prefabs", root).GetComponent<BubbleShopTalkCtrl>();
	}

	public void RecycleShopBubble(BubbleShopTalkCtrl ctrl)
	{
		allShopPools.Enqueue(ctrl);
		if (ctrl == currentShowingShop)
		{
			currentShowingShop = null;
		}
	}
}
