using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcherArrowCtrl : PlayerDefenceAttrCtrl
{
	public const int ArrowShowMaxAmount = 5;

	private SingleArrowCtrl[] arrowCtrls;

	private Image aimImg;

	private List<SingleArrowCtrl> arrowList = new List<SingleArrowCtrl>(5);

	private Queue<SingleArrowCtrl> allNotShowingArrowImg = new Queue<SingleArrowCtrl>(5);

	private Vector3[] positionArray;

	private Transform highlightPos;

	private Transform hidePos;

	private bool isAimHighlight;

	public SingleArrowCtrl[] ArrowCtrls => arrowCtrls;

	public override PlayerOccupation PlayerOccupation => PlayerOccupation.Archer;

	private void Awake()
	{
		arrowCtrls = new SingleArrowCtrl[5];
		positionArray = new Vector3[5];
		for (int i = 0; i < 5; i++)
		{
			arrowCtrls[i] = base.transform.Find("ArrowRoot").GetChild(i).GetComponent<SingleArrowCtrl>();
			positionArray[i] = arrowCtrls[i].transform.position;
		}
		aimImg = base.transform.Find("AimImg").GetComponent<Image>();
		highlightPos = base.transform.Find("HighlightPos");
		hidePos = base.transform.Find("HidePos");
	}

	public override void LoadPlayerInfo()
	{
		arrowList.Clear();
		allNotShowingArrowImg.Clear();
		for (int i = 0; i < 5; i++)
		{
			arrowCtrls[i].ShowArrow();
			arrowCtrls[i].SetSpecialArrow(Arrow.ArrowType.Normal);
			arrowCtrls[i].transform.position = positionArray[i];
			arrowList.Add(arrowCtrls[i]);
			arrowCtrls[i].ShowDropHint(isActive: false);
		}
		CancelHighlightAnimHint();
	}

	public void ComsumeArrow(int amount)
	{
		if (amount < 5)
		{
			ComsumeArrowLessThanArrowShowMaxAmount(amount);
		}
		else
		{
			ComsumeArrowMoreOrEqualThanArrowShowMaxAmount();
		}
		TrySupplyArrow();
		if (isAimHighlight && arrowList.Count == 0)
		{
			aimImg.gameObject.SetActive(value: false);
		}
	}

	private void ComsumeArrowLessThanArrowShowMaxAmount(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			SingleArrowCtrl singleArrowCtrl = arrowList[0];
			arrowList.RemoveAt(0);
			singleArrowCtrl.HideArrow();
			allNotShowingArrowImg.Enqueue(singleArrowCtrl);
		}
		StartCoroutine(MoveExistArrow_IE(amount, arrowList.Count, 0.3f));
	}

	private void ComsumeArrowMoreOrEqualThanArrowShowMaxAmount()
	{
		for (int i = 0; i < 5; i++)
		{
			arrowList[i].HideArrow();
			allNotShowingArrowImg.Enqueue(arrowList[i]);
		}
		arrowList.Clear();
	}

	private void TrySupplyArrow()
	{
		ArcherPlayerAttr archerPlayerAttr = (ArcherPlayerAttr)Singleton<GameManager>.Instance.Player.PlayerAttr;
		int num = 0;
		while (arrowList.Count < 5 && arrowList.Count < archerPlayerAttr.PreArrow)
		{
			SingleArrowCtrl singleArrowCtrl = allNotShowingArrowImg.Dequeue();
			singleArrowCtrl.ShowArrow();
			Vector3 position = singleArrowCtrl.transform.position;
			position.x = (positionArray[4] + (float)(num + 1) * (positionArray[1].x - positionArray[0].x) * Vector3.right).x;
			singleArrowCtrl.transform.position = position;
			singleArrowCtrl.SetSpecialArrow((archerPlayerAttr.AllSpecialArrows.Count > arrowList.Count) ? archerPlayerAttr.AllSpecialArrows[arrowList.Count].MArrowType : Arrow.ArrowType.Normal);
			arrowList.Add(singleArrowCtrl);
			num++;
		}
		if (num > 0)
		{
			StartCoroutine(ComsumeArrow_IE(num, 0.3f));
		}
	}

	private IEnumerator MoveExistArrow_IE(int amount, int remainAmount, float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		for (int i = 0; i < remainAmount && i < arrowList.Count; i++)
		{
			arrowList[i].MoveX(positionArray[i].x);
		}
	}

	private IEnumerator ComsumeArrow_IE(int amount, float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		for (int i = 0; i < amount && i < arrowList.Count; i++)
		{
			int num = arrowList.Count - i - 1;
			float x = positionArray[num].x;
			arrowList[num].MoveX(x);
		}
	}

	public void AddNormalArrow()
	{
		if (allNotShowingArrowImg.Count > 0)
		{
			ArcherPlayerAttr archerPlayerAttr = (ArcherPlayerAttr)Singleton<GameManager>.Instance.Player.PlayerAttr;
			int num = 0;
			while (arrowList.Count < 5 && arrowList.Count < archerPlayerAttr.SpecialAttr)
			{
				SingleArrowCtrl singleArrowCtrl = allNotShowingArrowImg.Dequeue();
				singleArrowCtrl.ShowArrow();
				singleArrowCtrl.transform.position = positionArray[4] + (float)(num + 1) * (positionArray[1].x - positionArray[0].x) * Vector3.right;
				singleArrowCtrl.SetSpecialArrow(Arrow.ArrowType.Normal);
				arrowList.Add(singleArrowCtrl);
				num++;
			}
			if (num > 0)
			{
				StartCoroutine(ComsumeArrow_IE(num, 0.1f));
			}
			if (isAimHighlight && !aimImg.gameObject.activeSelf)
			{
				aimImg.gameObject.SetActive(value: true);
			}
		}
	}

	public void SetSpecialArrow(int startIndex, int endIndex)
	{
		if (startIndex != endIndex)
		{
			StartCoroutine(SetSpecialArrow_IE(startIndex, endIndex));
		}
	}

	private IEnumerator SetSpecialArrow_IE(int startIndex, int endIndex)
	{
		ArcherPlayerAttr archerPlayerAttr = (ArcherPlayerAttr)Singleton<GameManager>.Instance.Player.PlayerAttr;
		for (int i = startIndex; i < endIndex; i++)
		{
			arrowList[i].MoveY(hidePos.position.y);
		}
		yield return new WaitForSeconds(0.2f);
		for (int j = startIndex; j < endIndex; j++)
		{
			arrowList[j].SetSpecialArrow(archerPlayerAttr.AllSpecialArrows[j].MArrowType);
		}
		yield return new WaitForSeconds(0.1f);
		for (int k = startIndex; k < endIndex; k++)
		{
			arrowList[k].MoveToZeroLocalPosY();
		}
	}

	public void HighlightAnimHint()
	{
		isAimHighlight = true;
		aimImg.gameObject.SetActive(arrowList.Count > 0);
	}

	public void CancelHighlightAnimHint()
	{
		isAimHighlight = false;
		aimImg.gameObject.SetActive(value: false);
	}

	public void HighlightArrow(int amount, bool isDrop)
	{
		for (int i = 0; i < amount && i < arrowList.Count; i++)
		{
			arrowList[i].MoveY(highlightPos.position.y);
			arrowList[i].ShowDropHint(isDrop);
		}
	}

	public void CancelHighlightArrow(int amount, bool isDrop)
	{
		for (int i = 0; i < amount && i < arrowList.Count; i++)
		{
			arrowList[i].CancelHighlight();
			if (isDrop)
			{
				arrowList[i].ShowDropHint(isActive: false);
			}
		}
	}
}
