using System.Collections.Generic;
using UnityEngine;

public class VfxManager : Singleton<VfxManager>
{
	public bool isUseObjectPool = true;

	private Dictionary<string, Queue<VfxBase>> allVfxObjectPools = new Dictionary<string, Queue<VfxBase>>();

	public VfxBase LoadVfx(string vfxName)
	{
		if (isUseObjectPool && allVfxObjectPools.TryGetValue(vfxName, out var value) && value.Count > 0)
		{
			VfxBase vfxBase = value.Dequeue();
			vfxBase.gameObject.SetActive(value: true);
			return vfxBase;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadVfxInstance(vfxName, base.transform).GetComponent<VfxBase>();
	}

	public void RecycleVfx(string vfxName, VfxBase vfxBase, bool isSetParent)
	{
		vfxBase.transform.rotation = Quaternion.identity;
		if (isSetParent)
		{
			vfxBase.transform.SetParent(base.transform);
		}
		vfxBase.gameObject.SetActive(value: false);
		if (allVfxObjectPools.TryGetValue(vfxName, out var value))
		{
			value.Enqueue(vfxBase);
			return;
		}
		Queue<VfxBase> queue = new Queue<VfxBase>(3);
		queue.Enqueue(vfxBase);
		allVfxObjectPools.Add(vfxName, queue);
	}
}
