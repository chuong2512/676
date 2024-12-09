using UnityEngine;
using UnityEngine.UI;

public class ImageExtent : Image
{
	public bool toggleTint;

	public override Material GetModifiedMaterial(Material bMaterial)
	{
		Material modifiedMaterial = base.GetModifiedMaterial(bMaterial);
		if (toggleTint)
		{
			modifiedMaterial.EnableKeyword("_TintToggle");
		}
		else
		{
			modifiedMaterial.DisableKeyword("_TintToggle");
		}
		return modifiedMaterial;
	}
}
