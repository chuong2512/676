using Spine.Unity;

public class Monster32AnimCtrl : MonsterAnimCtrlBase
{
	[SpineAnimation("", "", true, false)]
	public string awakeIdleAnimName;

	[SpineAnimation("", "", true, false)]
	public string awakeAnimName;

	public void AwakeIdle()
	{
		base.State.SetAnimation(0, awakeIdleAnimName, loop: true);
	}

	public void AwakeAnim()
	{
		base.State.SetAnimation(0, awakeAnimName, loop: false);
		base.State.AddAnimation(0, awakeIdleAnimName, loop: true, 0f);
	}
}
