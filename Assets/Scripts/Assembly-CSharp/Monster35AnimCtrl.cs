using Spine.Unity;

public class Monster35AnimCtrl : MonsterAnimCtrlBase
{
	[SpineAnimation("", "", true, false)]
	public string stunIdle;

	public void StunIdle()
	{
		base.State.SetAnimation(0, stunIdle, loop: true);
	}
}
