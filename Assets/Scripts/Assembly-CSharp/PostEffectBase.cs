using UnityEngine;

public class PostEffectBase : MonoBehaviour
{
	public Material _Material;

	public virtual PostEffectType PostEffectType { get; }

	public virtual void StartEffect(object args)
	{
		base.enabled = true;
	}

	public virtual void Stop()
	{
		base.enabled = false;
	}
}
