using DG.Tweening;
using UnityEngine.UI;

public class ScreenEffectDefaultLayerUI : UIView
{
	private Image enemyAreaHintImg;

	private Tween enemyAreaHintTween;

	public override string UIViewName => "ScreenEffectDefaultLayerUI";

	public override string UILayerName => "DefaultLayer";

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
	}

	public override void OnSpawnUI()
	{
		InitEnemyAreaHint();
	}

	private void InitEnemyAreaHint()
	{
		enemyAreaHintImg = base.transform.Find("EnemyAreaHint").GetComponent<Image>();
	}

	public void ShowEnemyAreaHint()
	{
		if (enemyAreaHintTween != null && enemyAreaHintTween.IsActive())
		{
			enemyAreaHintTween.Kill();
		}
		SingletonDontDestroy<AudioManager>.Instance.PlayerSound_Loop("技能指示线");
		enemyAreaHintImg.gameObject.SetActive(value: true);
		enemyAreaHintTween = enemyAreaHintImg.DOFade(1f, 0.5f);
	}

	public void HideEnemyAreaHint()
	{
		if (enemyAreaHintTween != null && enemyAreaHintTween.IsActive())
		{
			enemyAreaHintTween.Kill();
		}
		SingletonDontDestroy<AudioManager>.Instance.StopLoopSound("技能指示线");
		enemyAreaHintTween = enemyAreaHintImg.DOFade(0f, 0.5f).OnComplete(HideEnemyAreaHintImg);
	}

	private void HideEnemyAreaHintImg()
	{
		enemyAreaHintImg.gameObject.SetActive(value: false);
	}
}
