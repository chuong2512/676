using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MapCharacterCtrl : MonoBehaviour
{
	public float xDistance;

	public float yDistance;

	public float height;

	private Transform shadowImgTrans;

	private Transform playerTrans;

	private Image playerImg;

	private PlayerOccupation currentOccupation;

	private void OnEnable()
	{
		if (currentOccupation != Singleton<GameManager>.Instance.Player.PlayerOccupation)
		{
			LoadOccupation();
		}
	}

	private void Awake()
	{
		shadowImgTrans = base.transform.Find("Shadow");
		playerTrans = base.transform.Find("Player");
		playerImg = base.transform.Find("Player").GetComponent<Image>();
	}

	private void LoadOccupation()
	{
		currentOccupation = Singleton<GameManager>.Instance.Player.PlayerOccupation;
		OccupationData occupationData = DataManager.Instance.GetOccupationData(currentOccupation);
		playerImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(occupationData.PieceSpriteName, occupationData.DefaultSpritePath);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Vector3 targetPos = base.transform.position + new Vector3(xDistance, yDistance);
			JumpToPosition(targetPos, null);
		}
	}

	public void JumpToPosition(Vector3 targetPos, Action callback)
	{
		StartCoroutine(Jump_IE(targetPos, callback));
	}

	private IEnumerator Jump_IE(Vector3 targetPos, Action callback)
	{
		yield return StartCoroutine(Turn_IE(targetPos.x));
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("起跳");
		playerTrans.DOLocalRotate(new Vector3(0f, 0f, (0f - playerTrans.localScale.x) * 15f), 0.1f);
		yield return new WaitForSeconds(0.1f);
		Vector3 startPos = playerTrans.localPosition;
		shadowImgTrans.DOScale(0.8f, 0.2f);
		base.transform.DOMove(targetPos, 0.4f);
		playerTrans.DOLocalMoveY(startPos.y + height, 0.2f).SetEase(Ease.OutQuad);
		playerTrans.DOLocalRotateQuaternion(Quaternion.identity, 0.2f);
		yield return new WaitForSeconds(0.2f);
		shadowImgTrans.DOScale(1f, 0.2f);
		playerTrans.DOLocalMoveY(startPos.y, 0.2f).SetEase(Ease.InQuad);
		yield return new WaitForSeconds(0.2f);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("棋子落子");
		callback?.Invoke();
	}

	private IEnumerator Turn_IE(float x)
	{
		if (playerTrans.localScale.x > 0f && x < base.transform.position.x)
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("起跳");
			playerTrans.DOScaleX(-1f, 0.3f);
			shadowImgTrans.DOScale(-1f, 0.3f);
			yield return new WaitForSeconds(0.3f);
		}
		else if (playerTrans.localScale.x < 0f && x > base.transform.position.x)
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("起跳");
			playerTrans.DOScaleX(1f, 0.3f);
			shadowImgTrans.DOScale(1f, 0.3f);
			yield return new WaitForSeconds(0.3f);
		}
	}
}
