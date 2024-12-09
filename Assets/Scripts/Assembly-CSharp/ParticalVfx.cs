using System.Collections;
using UnityEngine;

public class ParticalVfx : VfxBase
{
	private ParticleSystem par;

	private TrailRenderer[] trails;

	private float originalScale;

	protected override void OnInit()
	{
		par = GetComponent<ParticleSystem>();
		originalScale = base.transform.localScale.x;
		trails = GetComponentsInChildren<TrailRenderer>(includeInactive: true);
	}

	protected override void PlayVfx(bool isMute)
	{
		par.Play();
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(CheckVfxLife_IE());
		}
	}

	private void ClearTrailRenderer()
	{
		if (trails != null && trails.Length != 0)
		{
			TrailRenderer[] array = trails;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Clear();
			}
		}
	}

	private IEnumerator CheckVfxLife_IE()
	{
		while (par.IsAlive(withChildren: true) && base.gameObject.activeInHierarchy)
		{
			yield return null;
		}
		Recycle();
	}

	protected override void OnRecycle()
	{
		base.transform.localScale = Vector3.one * originalScale;
		ClearTrailRenderer();
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}

	public void PlayInTest()
	{
		par.Play();
		TryPlayVfxSound(isMute: false);
	}
}
