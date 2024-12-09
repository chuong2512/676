using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UsualHealthBarCtrl : MonoBehaviour
{
	private Image healthFillImg;

	private Image healthFill_HurtImg;

	private Image healthFill_HealImg;

	private Text healthText;

	private int currentHealth;

	private int currentMaxHealth;

	private Tween hintHealthBarMoveTween;

	private void Awake()
	{
		healthFillImg = base.transform.Find("Background/Fill").GetComponent<Image>();
		healthFill_HurtImg = base.transform.Find("Background/Fill_Hurt").GetComponent<Image>();
		healthFill_HealImg = base.transform.Find("Background/Fill_Heal").GetComponent<Image>();
		healthText = base.transform.Find("HealthText").GetComponent<Text>();
	}

	public void LoadHealth(int health, int maxHealth)
	{
		currentHealth = health;
		currentMaxHealth = maxHealth;
		healthFillImg.fillAmount = (float)health / (float)maxHealth;
		StringBuilder stringBuilder = new StringBuilder(15);
		stringBuilder.Append(health).Append("/").Append(maxHealth);
		healthText.text = stringBuilder.ToString();
	}

	public void UpdateHealth(int health, int maxHealth)
	{
		if (hintHealthBarMoveTween != null && hintHealthBarMoveTween.IsActive())
		{
			hintHealthBarMoveTween.Complete();
		}
		float num = (float)health / (float)maxHealth;
		float num2 = (float)currentHealth / (float)currentMaxHealth;
		currentHealth = health;
		currentMaxHealth = maxHealth;
		StringBuilder stringBuilder = new StringBuilder(15);
		stringBuilder.Append(health).Append("/").Append(maxHealth);
		healthText.text = stringBuilder.ToString();
		if (num > num2)
		{
			healthFill_HealImg.gameObject.SetActive(value: true);
			healthFill_HealImg.fillAmount = num;
			hintHealthBarMoveTween = healthFillImg.DOFillAmount(num, 0.5f).OnComplete(delegate
			{
				healthFill_HealImg.gameObject.SetActive(value: false);
			});
		}
		else if (num < num2)
		{
			healthFillImg.fillAmount = num;
			healthFill_HurtImg.gameObject.SetActive(value: true);
			healthFill_HurtImg.fillAmount = num2;
			hintHealthBarMoveTween = healthFill_HurtImg.DOFillAmount(num, 0.5f).OnComplete(delegate
			{
				healthFill_HurtImg.gameObject.SetActive(value: false);
			});
		}
	}
}
