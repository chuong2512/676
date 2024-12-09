using UnityEngine.UI;

public class VersionUI : UIView
{
	private Text versionText;

	public override string UIViewName => "VersionUI";

	public override string UILayerName => "OutGameLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		versionText.text = SingletonDontDestroy<Game>.Instance.GameVersion;
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
		versionText = base.transform.Find("Root/Version").GetComponent<Text>();
	}
}
