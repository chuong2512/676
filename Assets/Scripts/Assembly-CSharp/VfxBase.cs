using UnityEngine;

public abstract class VfxBase : MonoBehaviour
{
	private bool isRecycled;

	public string vfxName;

	public string soundName;

	public string soundName2;

	protected abstract void PlayVfx(bool isMute);

	protected abstract void OnRecycle();

	protected abstract void OnInit();

	private void Awake()
	{
		OnInit();
	}

	private void OnDisable()
	{
		Recycle(isSetParent: false);
	}

	public void Play(bool isMute = false)
	{
		PlayVfx(isMute);
		TryPlayVfxSound(isMute);
		isRecycled = false;
	}

	protected void TryPlayVfxSound(bool isMute)
	{
		if (!isMute && !soundName.IsNullOrEmpty())
		{
			if (soundName2.IsNullOrEmpty())
			{
				SingletonDontDestroy<AudioManager>.Instance.PlaySound(soundName);
			}
			else
			{
				SingletonDontDestroy<AudioManager>.Instance.PlaySound((Random.value < 0.5f) ? soundName : soundName2);
			}
		}
	}

	public void Recycle(bool isSetParent = true)
	{
		if (!isRecycled)
		{
			isRecycled = true;
			if (SingletonDontDestroy<Game>.Instance.isTest)
			{
				base.gameObject.SetActive(value: false);
				return;
			}
			Singleton<VfxManager>.Instance.RecycleVfx(vfxName, this, isSetParent);
			OnRecycle();
		}
	}
}
