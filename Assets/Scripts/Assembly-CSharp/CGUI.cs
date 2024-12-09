using System;

public class CGUI : UIView
{
	private Action finsihAct;

	private VideoController _controller;

	public override string UIViewName => "CGUI";

	public override string UILayerName => "OutGameLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		finsihAct = (Action)objs[0];
		_controller.StartAnim();
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		finsihAct?.Invoke();
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
		_controller = GetComponent<VideoController>();
	}
}
