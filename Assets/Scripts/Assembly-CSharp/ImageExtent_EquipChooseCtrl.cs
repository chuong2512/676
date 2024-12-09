using UnityEngine;
using UnityEngine.UI;

public class ImageExtent_EquipChooseCtrl : Image
{
	private float m_threshold;

	public float threshold
	{
		get
		{
			return m_threshold;
		}
		set
		{
			if (m_threshold != value)
			{
				m_threshold = value;
				SetMaterialDirty();
			}
		}
	}

	public override Material GetModifiedMaterial(Material bMaterial)
	{
		Material modifiedMaterial = base.GetModifiedMaterial(bMaterial);
		modifiedMaterial.SetFloat("_ThreshHold", threshold);
		return modifiedMaterial;
	}
}
