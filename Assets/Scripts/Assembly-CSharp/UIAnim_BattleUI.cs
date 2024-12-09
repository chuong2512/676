using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIAnim_BattleUI : MonoBehaviour
{
	public Transform mainListParent;

	public Transform offListParent;

	private List<Image> mainHandParticleList = new List<Image>();

	private List<Image> offHandParticleList = new List<Image>();

	public Transform discardPile_Main;

	public Transform discardPile_Off;

	public Image drawPile_Main;

	public Image drawPile_Off;

	public Text mainText;

	public Text offText;

	private List<Tween> tweenList_Main = new List<Tween>();

	private List<Tween> tweenList_Sup = new List<Tween>();

	public Image mainLight;

	public Image offLight;

	private void Awake()
	{
		drawPile_Main.material.SetFloat("_MaxRatio", 0f);
		drawPile_Off.material.SetFloat("_MaxRatio", 0f);
		Image image = offLight;
		bool flag2 = (mainLight.enabled = false);
		image.enabled = flag2;
	}

	public void ResetMain()
	{
		foreach (Tween item in tweenList_Main)
		{
			if (item.IsActive())
			{
				item.Complete(withCallbacks: false);
			}
		}
		mainText.transform.localScale = Vector3.one;
	}

	public void ResetSup()
	{
		foreach (Tween item in tweenList_Sup)
		{
			if (item.IsActive())
			{
				item.Complete(withCallbacks: false);
			}
		}
		offText.transform.localScale = Vector3.one;
	}

	public void ShuffleCard_Main(int num)
	{
		ResetMain();
		StartCoroutine(MainCo(num));
	}

	private IEnumerator MainCo(int num)
	{
		mainLight.enabled = true;
		mainLight.WithCol(0f);
		drawPile_Main.material.SetFloat("_MaxRatio", 1f);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("特效_洗牌");
		for (int i = 0; i < num; i++)
		{
			Image card = GetAvailableMainCard(i);
			card.material.SetFloat("_MaxRatio", 0f);
			card.transform.position = discardPile_Main.position;
			card.gameObject.SetActive(value: true);
			tweenList_Main.Add(card.material.DOFloat(1f, "_MaxRatio", 1f).OnComplete(delegate
			{
				card.gameObject.SetActive(value: false);
			}));
			float duration = 1f / (float)num;
			tweenList_Main.Add(mainText.transform.DOPunchScale(Vector3.one * 1.2f, duration, 2));
			tweenList_Main.Add(card.transform.DOMoveY(drawPile_Main.transform.position.y, 0.5f));
			yield return new WaitForSeconds(duration);
			tweenList_Main.Add(drawPile_Main.material.DOFloat(0.7f, "_MaxRatio", duration).OnComplete(delegate
			{
				drawPile_Main.material.SetFloat("_MaxRatio", 1f);
			}));
			tweenList_Main.Add(mainLight.DOFade(0.7f, duration).OnComplete(delegate
			{
				mainLight.color = Color.white;
			}));
		}
		tweenList_Main.Add(drawPile_Main.material.DOFloat(0f, "_MaxRatio", 1f).SetEase(Ease.InCubic));
		tweenList_Main.Add(mainLight.DOFade(0f, 1f).OnComplete(delegate
		{
			offLight.enabled = false;
		}));
	}

	public void ShuffleCard_Sup(int num)
	{
		ResetSup();
		StartCoroutine(SupCo(num));
	}

	private IEnumerator SupCo(int num)
	{
		offLight.enabled = true;
		offLight.WithCol(0f);
		drawPile_Off.material.SetFloat("_MaxRatio", 1f);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("特效_洗牌");
		for (int i = 0; i < num; i++)
		{
			Image card = GetAvailableSupCard(i);
			card.material.SetFloat("_MaxRatio", 0f);
			card.transform.position = discardPile_Off.position;
			card.gameObject.SetActive(value: true);
			tweenList_Sup.Add(card.material.DOFloat(1f, "_MaxRatio", 1f).OnComplete(delegate
			{
				card.gameObject.SetActive(value: false);
			}));
			float duration = 1f / (float)num;
			tweenList_Sup.Add(offText.transform.DOPunchScale(Vector3.one * 1.2f, duration, 2));
			tweenList_Sup.Add(card.transform.DOMoveY(drawPile_Off.transform.position.y, 0.5f));
			yield return new WaitForSeconds(duration);
			tweenList_Sup.Add(drawPile_Off.material.DOFloat(0.7f, "_MaxRatio", duration).OnComplete(delegate
			{
				drawPile_Off.material.SetFloat("_MaxRatio", 1f);
			}));
			tweenList_Sup.Add(offLight.DOFade(0.7f, duration).OnComplete(delegate
			{
				offLight.color = Color.white;
			}));
		}
		tweenList_Sup.Add(drawPile_Off.material.DOFloat(0f, "_MaxRatio", 1f).SetEase(Ease.InCubic));
		tweenList_Sup.Add(offLight.DOFade(0f, 1f).OnComplete(delegate
		{
			offLight.enabled = false;
		}));
	}

	private Image GetAvailableMainCard(int num)
	{
		Image image;
		if (num >= mainHandParticleList.Count)
		{
			image = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("mainHandParticleCard", "Prefabs", mainListParent).GetComponent<Image>();
			mainHandParticleList.Add(image);
		}
		else
		{
			image = mainHandParticleList[num];
		}
		return image;
	}

	private Image GetAvailableSupCard(int num)
	{
		Image image;
		if (num >= offHandParticleList.Count)
		{
			image = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("offHandParticleCard", "Prefabs", offListParent).GetComponent<Image>();
			offHandParticleList.Add(image);
		}
		else
		{
			image = offHandParticleList[num];
		}
		return image;
	}
}
