using UnityEngine;

public abstract class EscCloseUIView : UIView
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			OnHide();
		}
	}

	protected virtual void OnHide()
	{
	}
}
