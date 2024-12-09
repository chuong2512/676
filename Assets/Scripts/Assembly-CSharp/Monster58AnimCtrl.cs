using Spine.Unity;

public class Monster58AnimCtrl : MonsterAnimCtrlBase
{
	[SpineAnimation("", "", true, false)]
	public string idle1AnimName;

	public void SetIdle1()
	{
		base.State.SetAnimation(0, idle1AnimName, loop: true);
	}
}
