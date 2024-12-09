using System.Collections;
using DG.Tweening;
using UnityEngine;

public class RadialBlurEffectOnCam : PostEffectBase
{
	private float blurFactor = 0.03f;

	private float lerpFactor = 1.2f;

	private int downSampleFactor = 3;

	private Vector2 blurCenter = new Vector2(0.5f, 0.5f);

	private Tween paraTween;

	private Material matInstance;

	public override PostEffectType PostEffectType => PostEffectType.RadialBlur;

	public override void StartEffect(object args)
	{
		RadialBlueEffectArgs radialBlueEffectArgs = args as RadialBlueEffectArgs;
		blurFactor = radialBlueEffectArgs.blurFactor;
		lerpFactor = radialBlueEffectArgs.lerpFactor;
		downSampleFactor = radialBlueEffectArgs.downSampleFactor;
		blurCenter = radialBlueEffectArgs.blurCenter;
		base.StartEffect(args);
		StartCoroutine(ParaCo(radialBlueEffectArgs));
	}

	public override void Stop()
	{
		if (paraTween != null && paraTween.IsActive())
		{
			paraTween.Kill();
		}
		StopAllCoroutines();
		base.Stop();
	}

	private IEnumerator ParaCo(RadialBlueEffectArgs args)
	{
		base.enabled = true;
		blurFactor = 0f;
		paraTween = DOTween.To(() => blurFactor, delegate(float x)
		{
			blurFactor = x;
		}, args.blurFactor, args.inTime);
		yield return new WaitForSeconds(args.inTime + args.holdTime);
		paraTween = DOTween.To(() => blurFactor, delegate(float x)
		{
			blurFactor = x;
		}, 0f, args.recoverTime).OnComplete(Stop);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if ((bool)_Material)
		{
			if (matInstance == null)
			{
				matInstance = new Material(_Material)
				{
					hideFlags = HideFlags.HideAndDontSave
				};
			}
			RenderTexture temporary = RenderTexture.GetTemporary(source.width >> downSampleFactor, source.height >> downSampleFactor, 0, source.format);
			RenderTexture temporary2 = RenderTexture.GetTemporary(source.width >> downSampleFactor, source.height >> downSampleFactor, 0, source.format);
			Graphics.Blit(source, temporary);
		//	matInstance.SetFloat("_BlurFactor", blurFactor);
		//	matInstance.SetVector("_BlurCenter", blurCenter);
			Graphics.Blit(temporary, temporary2, matInstance, 0);
		//	matInstance.SetTexture("_BlurTex", temporary2);
		//	matInstance.SetFloat("_LerpFactor", lerpFactor);
		//	Graphics.Blit(source, destination, matInstance, 1);
		//	RenderTexture.ReleaseTemporary(temporary);
		//	RenderTexture.ReleaseTemporary(temporary2);
		}
		else
		{
			Graphics.Blit(source, destination);
		}
	}
}
