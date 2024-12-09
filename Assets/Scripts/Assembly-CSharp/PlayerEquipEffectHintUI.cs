using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEquipEffectHintUI : UIView
{
	private Image equipEffectHintImg;

	private bool isShowing;

	private Queue<string> equipShowQueue = new Queue<string>();

	public override string UIViewName => "PlayerEquipEffectHintUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		equipShowQueue.Clear();
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destory Player Equip Efffect Hint UI...");
	}

	public override void OnSpawnUI()
	{
		equipEffectHintImg = base.transform.Find("EquipEffectHint").GetComponent<Image>();
	}

	public void AddHint(string equipCode)
	{
		if (isShowing)
		{
			equipShowQueue.Enqueue(equipCode);
			return;
		}
		isShowing = true;
		equipShowQueue.Enqueue(equipCode);
		StartCoroutine(ShowEquip_IE());
	}

	private IEnumerator ShowEquip_IE()
	{
		while (equipShowQueue.Count > 0 && Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			string cardCode = equipShowQueue.Dequeue();
			EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(cardCode);
			equipEffectHintImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(equipmentCardAttr.ImageName, "Sprites/Equipment");
			equipEffectHintImg.SetNativeSize();
			equipEffectHintImg.transform.localScale = Vector3.one * 0.4f;
			equipEffectHintImg.color = new Color(1f, 1f, 1f, 0.4f);
			yield return StartCoroutine(ShowSingleEquip_IE());
		}
		isShowing = false;
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}

	private IEnumerator ShowSingleEquip_IE()
	{
		bool isOver = false;
		Sequence sequence = DOTween.Sequence();
		equipEffectHintImg.transform.DOScale(1f, 1f);
		sequence.Append(equipEffectHintImg.DOFade(0.8f, 1f));
		sequence.Append(equipEffectHintImg.DOFade(0f, 1.2f));
		sequence.OnComplete(delegate
		{
			isOver = true;
		});
		while (!isOver)
		{
			yield return null;
		}
	}
}
