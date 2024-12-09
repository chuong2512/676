using UnityEngine;

public abstract class UIView : MonoBehaviour
{
	public static readonly Color CoinColor = "E7BD49FF".HexColorToColor();

	public virtual bool AutoHideBySwitchScene => true;

	public abstract string UIViewName { get; }

	public abstract string UILayerName { get; }

	public abstract void ShowView(params object[] objs);

	public abstract void HideView();

	public abstract void OnDestroyUI();

	public abstract void OnSpawnUI();

	public virtual void ReInitUI()
	{
	}
}
