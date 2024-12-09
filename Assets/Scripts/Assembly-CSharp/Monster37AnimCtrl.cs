using Spine.Unity;

public class Monster37AnimCtrl : MonsterAnimCtrlBase
{
	[SpineAnimation("", "", true, false)]
	public string rebornAnimName;

	[SpineAnimation("", "", true, false)]
	public string idleAfterRebornAnimName;

	public void RebornAnim()
	{
		base.State.SetAnimation(0, rebornAnimName, loop: false);
		base.State.AddAnimation(0, idleAfterRebornAnimName, loop: true, 0f);
	}
}
