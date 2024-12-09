using System.Collections;
using UnityEngine;

public class CastHintUI : UIView
{
	public int linePointAmount;

	private LineRenderer _lineRenderer;

	private Transform castHintTopPoint;

	private Transform castHintRoot;

	private bool isShowingCastHint;

	public override string UIViewName => "CastHintUI";

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
		Debug.Log("Destroy Cast Hint UI ...");
	}

	public override void OnSpawnUI()
	{
		InitCastHintPanel();
	}

	private void InitCastHintPanel()
	{
		castHintRoot = base.transform.Find("Root/HintRoot");
		_lineRenderer = castHintRoot.GetComponent<LineRenderer>();
		castHintRoot.gameObject.SetActive(value: false);
		castHintTopPoint = base.transform.Find("Root/TopPoint");
	}

	public void StartShowCastHint(Transform target)
	{
		isShowingCastHint = true;
		castHintRoot.gameObject.SetActive(value: true);
		_lineRenderer.positionCount = linePointAmount;
		StartCoroutine(ShowCastHint_IE(target));
	}

	public void EndShowCastHint()
	{
		isShowingCastHint = false;
		castHintRoot.gameObject.SetActive(value: false);
	}

	private IEnumerator ShowCastHint_IE(Transform target)
	{
		while (isShowingCastHint)
		{
			MoveByPoint(target);
			yield return null;
		}
	}

	private void MoveByPoint(Transform target)
	{
		Vector3 vector = SingletonDontDestroy<CameraController>.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
		Vector3 vector2 = new Vector3(target.position.x, 2f * castHintTopPoint.position.y - vector.y, 0f);
		float num = 1f / (float)(linePointAmount - 1);
		for (int i = 0; i < linePointAmount; i++)
		{
			_lineRenderer.SetPosition(i, Bezier.GetV3Point(target.position, vector2, vector, num * (float)i));
		}
	}
}
