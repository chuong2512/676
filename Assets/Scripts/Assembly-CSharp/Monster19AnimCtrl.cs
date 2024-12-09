using Spine.Unity;

public class Monster19AnimCtrl : MonsterAnimCtrlBase
{
	[SpineAnimation("", "", true, false)]
	public string lowHealthAnimName;

	public void SetLowHealthAnim()
	{
		base.State.SetAnimation(0, lowHealthAnimName, loop: true);
	}
}
