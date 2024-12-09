using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMeanHintUI : UIView
{
	private Transform keyCtrlRoot;

	private Transform leftRoot;

	private RectTransform leftRectTrans;

	private Transform rightRoot;

	private RectTransform rightRectTrans;

	private Queue<MeanCtrl> allShowingMeanCtrl = new Queue<MeanCtrl>();

	private Queue<MeanCtrl> allMeanCtrlPool = new Queue<MeanCtrl>();

	public override string UIViewName => "EnemyMeanHintUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		HideAllMeanCtrl();
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
		keyCtrlRoot = base.transform.Find("Root");
		leftRoot = base.transform.Find("Root/LeftRoot");
		leftRectTrans = leftRoot.GetComponent<RectTransform>();
		rightRoot = base.transform.Find("Root/RightRoot");
		rightRectTrans = rightRoot.GetComponent<RectTransform>();
	}

	public void ShowEnemyMean(EnemyBaseCtrl enemy, List<MeanHandler> allMeans)
	{
		if (!Singleton<GameManager>.Instance.Player.IsPlayerCastingCard)
		{
			bool flag = SingletonDontDestroy<CameraController>.Instance.MainCamera.WorldToViewportPoint(enemy.transform.position).x < 0.5f;
			for (int i = 0; i < allMeans.Count; i++)
			{
				MeanCtrl meanCtrl = GetMeanCtrl();
				meanCtrl.LoadMean(allMeans[i].GetMeanIcon(), allMeans[i].GetDetailDesStr());
				meanCtrl.transform.SetParent(flag ? leftRoot : rightRoot);
				allShowingMeanCtrl.Enqueue(meanCtrl);
			}
			Vector3 position = enemy.transform.position + (Vector3)(enemy.BoxCollider2D.offset + new Vector2(flag ? (enemy.BoxCollider2D.size.x / 2f) : ((0f - enemy.BoxCollider2D.size.x) / 2f), enemy.BoxCollider2D.size.y / 2f));
			if (flag)
			{
				leftRoot.position = position;
			}
			else
			{
				rightRoot.position = position;
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(flag ? leftRectTrans : rightRectTrans);
		}
	}

	private MeanCtrl GetMeanCtrl()
	{
		if (allMeanCtrlPool.Count > 0)
		{
			MeanCtrl meanCtrl = allMeanCtrlPool.Dequeue();
			meanCtrl.gameObject.SetActive(value: true);
			return meanCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("MeanCtrl", "Prefabs", keyCtrlRoot).GetComponent<MeanCtrl>();
	}

	private void HideAllMeanCtrl()
	{
		while (allShowingMeanCtrl.Count > 0)
		{
			MeanCtrl meanCtrl = allShowingMeanCtrl.Dequeue();
			meanCtrl.gameObject.SetActive(value: false);
			allMeanCtrlPool.Enqueue(meanCtrl);
		}
	}
}
